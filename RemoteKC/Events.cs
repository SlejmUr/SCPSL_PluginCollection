using Interactables.Interobjects.DoorUtils;
using LabApi.Events.Arguments.PlayerEvents;

namespace RemoteKC;

internal class Events
{
    internal static void InteractingDoor(PlayerInteractingDoorEventArgs ev)
    {
        ev.CanOpen = PermissionCheck.HasKeycardPermission(ev.Door, ev.Player);
    }

    internal static void InteractingLocker(PlayerInteractingLockerEventArgs ev)
    {
        ev.CanOpen = PermissionCheck.HasKeycardPermission(ev.Chamber, ev.Player);
    }

    internal static void UnlockingGenerator(PlayerUnlockingGeneratorEventArgs ev)
    {
        ev.IsAllowed = PermissionCheck.HasKeycardPermission(ev.Generator, ev.Player);
    }

    internal static void UnlockingWarheadButton(PlayerUnlockingWarheadButtonEventArgs ev)
    {
        ev.IsAllowed = PermissionCheck.HasPlayerPermission(ev.Player, WarheadButton.Button);
    }
}

public class WarheadButton : IDoorPermissionRequester
{
    public static WarheadButton Button { get; set; } = new();

    public DoorPermissionsPolicy PermissionsPolicy => new DoorPermissionsPolicy(DoorPermissionFlags.AlphaWarhead, false, false);

    public string RequesterLogSignature => "WarheadButton";
}
