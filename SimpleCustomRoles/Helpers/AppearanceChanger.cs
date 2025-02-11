using LabApi.Features.Wrappers;
using Mirror;
using PlayerRoles.FirstPersonControl;
using PlayerRoles.PlayableScps.Scp049.Zombies;
using PlayerRoles;
using UnityEngine;
using RelativePositioning;

namespace SimpleCustomRoles.Helpers;

/// <summary>
/// All code from here is ported from Exiled & Edited to current version.
/// License: Creative Commons Attribution-ShareAlike 3.0 Unported
/// </summary>
internal static class AppearanceChanger
{
    public static void ChangeAppearance(this Player player, RoleTypeId type, bool skipJump = false, byte unitId = 0) => 
        ChangeAppearance(player, type, Player.List.Where(x => x != player), skipJump, unitId);

    public static void ChangeAppearance(this Player player, RoleTypeId type, IEnumerable<Player> playersToAffect, bool skipJump = false, byte unitId = 0)
    {
        if (player.Connection == null || !player.Connection.isReady)
            return;
        var roleBase = player.RoleBase;
        bool isRisky = type.GetTeam() is Team.Dead || !player.IsAlive;

        NetworkWriterPooled writer = NetworkWriterPool.Get();
        writer.WriteUShort(38952);
        writer.WriteUInt(player.NetworkId);
        writer.WriteRoleType(type);
        
        if (roleBase is HumanRole && player.UnitId != -1)
        {
            if (player.RoleBase is not HumanRole)
                isRisky = true;
            writer.WriteByte(unitId);
        }

        if (roleBase is FpcStandardRoleBase fpc)
        {
            if (player.RoleBase is not FpcStandardRoleBase playerfpc)
                isRisky = true;
            else
                fpc = playerfpc;

            ushort value = 0;
            fpc?.FpcModule.MouseLook.GetSyncValues(0, out value, out ushort _);
            writer.WriteRelativePosition(new(player.ReferenceHub));
            writer.WriteUShort(value);
        }

        if (roleBase is ZombieRole)
        {
            if (player.RoleBase is not ZombieRole)
                isRisky = true;

            writer.WriteUShort((ushort)Mathf.Clamp(Mathf.CeilToInt(player.MaxHealth), ushort.MinValue, ushort.MaxValue));
        }

        foreach (Player target in playersToAffect)
        {
            if (target != player || !isRisky)
                target.Connection.Send(writer.ToArraySegment());
            else
                CL.Error($"Prevent Seld-Desync of {player.Nickname} with {type}");
        }

        NetworkWriterPool.Return(writer);

        // To counter a bug that makes the player invisible until they move after changing their appearance, we will teleport them upwards slightly to force a new position update for all clients.
        if (!skipJump)
            player.Position += Vector3.up * 0.25f;
    }

}
