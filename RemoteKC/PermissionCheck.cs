using Interactables.Interobjects.DoorUtils;
using InventorySystem.Items.Keycards;
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

        foreach (Item item in player.Items)
            if (item is LabApi.Features.Wrappers.KeycardItem keycardItem && 
                keycardItem.Base is not SingleUseKeycardItem && 
                requester.PermissionsPolicy.CheckPermissions(keycardItem.Base.GetPermissions(requester)))
                return true;

        return false;
    }
}
