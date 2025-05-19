using Interactables.Interobjects.DoorUtils;
using LabApi.Features.Wrappers;

namespace RemoteKC;

internal static class PermissionCheck
{
    public static bool HasKeycardPermission(Door door, Player player) =>
    HasPlayerPermission(player, door.Base);

    public static bool HasKeycardPermission(LockerChamber chamber, Player player) =>
        HasPlayerPermission(player, chamber.Base);

    public static bool HasKeycardPermission(Generator generator, Player player) =>
        HasPlayerPermission(player, generator.Base);

    public static bool HasPlayerPermission(Player player, IDoorPermissionRequester requester)
    {
        if (player.IsBypassEnabled)
            return true;

        if (player.RoleBase is IDoorPermissionProvider doorPermissionProvider && 
            requester.PermissionsPolicy.CheckPermissions(doorPermissionProvider.GetPermissions(requester)))
            return true;

        foreach (var item in player.Items)
        {
            if (item.Base is InventorySystem.Items.Keycards.KeycardItem keycardItem &&
                keycardItem is not InventorySystem.Items.Keycards.SingleUseKeycardItem &&
                requester.PermissionsPolicy.CheckPermissions(keycardItem.GetPermissions(requester)))
                return true;
        }

        return false;
    }
}
