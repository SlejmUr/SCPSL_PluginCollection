using CustomPlayerEffects;
using LabApi.Features.Wrappers;
using SimpleCustomRoles.Helpers;

namespace SimpleCustomRoles.Handler;

internal static class SubHandle
{
    internal static void StatusEffectBase_OnEnabled(StatusEffectBase @base)
    {
        Player player = Player.Get(@base.Hub);
        if (!CustomRoleHelpers.TryGetCustomRole(player, out var customRole))
            return;
        var effect = customRole.Effects.FirstOrDefault(x => x.EffectName == @base.name);
        if (effect == default)
            return;
        if (effect.CanEnable)
            return;
        @base.DisableEffect();
    }
}
