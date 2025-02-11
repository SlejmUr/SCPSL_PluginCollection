using LabApi.Features.Wrappers;
using Mirror;
using UnityEngine;

namespace SimpleCustomRoles.Helpers;

/// <summary>
/// All code from here is ported from Exiled & Edited to current version.
/// License: Creative Commons Attribution-ShareAlike 3.0 Unported
/// </summary>
internal class ScaleHelper
{
    public static void SetScale(Player player, Vector3 value)
    {
        if (value == GetScale(player))
            return;

        try
        {
            player.ReferenceHub.transform.localScale = value;

            foreach (Player target in Player.List)
                NetworkServer.SendSpawnMessage(player.ReferenceHub.networkIdentity, target.Connection);
        }
        catch (Exception exception)
        {
            LabApi.Features.Console.Logger.Error($"{nameof(SetScale)} error: {exception}");
        }
    }

    public static Vector3 GetScale(Player player)
    {
        return player.ReferenceHub.transform.localScale;
    }
}
