using Interactables.Interobjects.DoorUtils;
using LabApi.Events.Arguments.PlayerEvents;

namespace RemoteKC;

internal class Events
{
    internal static void InteractingDoor(PlayerInteractingDoorEventArgs ev)
    {
        if (ev.Door.IsLocked)
            return;
        if (!ev.CanOpen && PermissionCheck.HasKeycardPermission(ev.Door, ev.Player))
        {
            ev.CanOpen = true;
            ev.IsAllowed = true;
        }
    }

    internal static void InteractingLocker(PlayerInteractingLockerEventArgs ev)
    {
        if (!ev.CanOpen)
            ev.CanOpen = PermissionCheck.HasKeycardPermission(ev.Chamber, ev.Player);
    }

    internal static void UnlockingGenerator(PlayerUnlockingGeneratorEventArgs ev)
    {
        if (!ev.IsAllowed && PermissionCheck.HasKeycardPermission(ev.Generator, ev.Player))
        {
            ev.Generator.Base.IsUnlocked = true;
            ev.Generator.Base._deniedStopwatch.Restart();
            ev.IsAllowed = false;
        }
    }

    internal static void UnlockingWarheadButton(PlayerUnlockingWarheadButtonEventArgs ev)
    {
        if (!ev.IsAllowed)
            ev.IsAllowed = PermissionCheck.HasPlayerPermission(ev.Player, WarheadButton.Button);
    }
}

public class WarheadButton : IDoorPermissionRequester
{
    public static WarheadButton Button { get; set; } = new();

    public DoorPermissionsPolicy PermissionsPolicy => new(DoorPermissionFlags.AlphaWarhead, false, false);

    public string RequesterLogSignature => "WarheadButton";
}
