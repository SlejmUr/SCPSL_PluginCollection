using CustomPlayerEffects;
using LabApi.Features.Wrappers;

namespace SimpleCustomRoles.Helpers;

/// <summary>
/// Help with LabApi non enum existing effect.
/// Supports created effects!
/// </summary>
internal static class EffectHelper
{
    public static string GetEffectNameFromType(Player player, StatusEffectBase @base)
    {

        return player.ReferenceHub.playerEffectsController.AllEffects.FirstOrDefault(x => x == @base).GetType().Name;
    }

    public static StatusEffectBase GetEffectFromName(Player player, string name)
    {
        return player.ReferenceHub.playerEffectsController.AllEffects.FirstOrDefault(x => x.GetType().Name == name);
    }
}
