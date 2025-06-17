using HarmonyLib;
using InventorySystem;
using InventorySystem.Items.Armor;
using LabApi.Features.Wrappers;
using SimpleCustomRoles.Helpers;

namespace SimpleCustomRoles.Patches;

[HarmonyPatch(typeof(BodyArmorUtils), nameof(BodyArmorUtils.RemoveEverythingExceedingLimits))]
internal static class BodyArmorUtils_RemoveEverythingExceedingLimits
{
    public static bool Prefix(Inventory inv)
    {
        Player player = Player.Get(inv._hub);
        if (CustomRoleHelpers.TryGetCustomRole(player, out var role) && role != null && role.Inventory.Keep)
            return false;
        return true;
    }
}
