using HarmonyLib;
using LabApi.Features.Wrappers;
using Mirror;
using PlayerRoles.PlayableScps.Scp173;
using SimpleCustomRoles.Helpers;

namespace SimpleCustomRoles.Patches;

[HarmonyPatch(typeof(Scp173BlinkTimer), nameof(Scp173BlinkTimer.OnObserversChanged))]
internal static class Scp173BlinkTimer_OnObserversChanged
{
    [HarmonyPrefix]
    public static bool Prefix(Scp173BlinkTimer __instance, int current)
    {
        if (!__instance.Role.TryGetOwner(out ReferenceHub hub))
            return true;
        Player player = Player.Get(hub);
        if (!CustomRoleHelpers.TryGetCustomRole(player, out var role))
            return true;
        if (role.Scp.Scp173.BlinkCooldown.Math == MathOption.None)
            return true;
        __instance._initialStopTime = NetworkTime.time;
        __instance._totalCooldown = role.Scp.Scp173.BlinkCooldown.MathCalculation(3f);
        __instance._endSustainTime = (current > 0) ? (-1.0) : (NetworkTime.time + role.Scp.Scp173.BlinkSustainTime.MathCalculation(2.0f));
        __instance.ServerSendRpc(true);
        return false;
    }
}
