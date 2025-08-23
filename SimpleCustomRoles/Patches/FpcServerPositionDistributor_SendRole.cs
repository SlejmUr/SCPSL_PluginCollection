using HarmonyLib;
using LabApi.Features.Wrappers;
using PlayerRoles;
using PlayerRoles.FirstPersonControl.NetworkMessages;
using PlayerRoles.Subroutines;

namespace SimpleCustomRoles.Patches;

/// <summary>
/// Patch until NW Add something to deal with faking roles.
/// Credit: ttroublett
/// </summary>
// [HarmonyPatch(typeof(FpcServerPositionDistributor), nameof(FpcServerPositionDistributor.SendRole))] // Patch disabled until actual solution found for this shit.
internal class FpcServerPositionDistributor_SendRole
{
    [HarmonyPrefix]
    private static bool Prefix(ReferenceHub receiver, ReferenceHub hub, RoleTypeId targetRole)
    {
        if (receiver == hub)
        {
            targetRole = hub.roleManager.CurrentRole.RoleTypeId;
        }

        bool flag = FpcServerPositionDistributor.IsDistributionActive(targetRole);

        RoleTypeId roleTypeToSend = flag ? RoleTypeId.Overwatch : targetRole;
        Player player = Player.Get(hub);
        Player receiverPlayer = Player.Get(receiver);

        // This throws error on quit...
        player.ChangeAppearance(roleTypeToSend, [receiverPlayer], skipJump: true, unitId: (byte)player.UnitId);

        hub.roleManager.PreviouslySentRole[receiver.netId] = targetRole;
        if (targetRole != hub.roleManager.CurrentRole.RoleTypeId || hub.roleManager.CurrentRole is not ISubroutinedRole currentRole)
        {
            return false;
        }

        foreach (SubroutineBase allSubroutine in currentRole.SubroutineModule.AllSubroutines)
        {
            receiver.connectionToClient.Send(new SubroutineMessage(allSubroutine, true));
        }

        return false;
    }
}
