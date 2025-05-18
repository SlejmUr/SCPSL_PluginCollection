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
        DoorPermissionFlags flags = DoorPermissionFlags.None;

        if (player.IsBypassEnabled)
            return true;

        if (player.RoleBase is IDoorPermissionProvider doorPermissionProvider)
            flags |= doorPermissionProvider.GetPermissions(requester);

        foreach (Item item in player.Items)
            if (item is KeycardItem keycardItem)
                flags |= keycardItem.Base.GetPermissions(requester);

        return requester.PermissionsPolicy.CheckPermissions(flags);
    }
}
