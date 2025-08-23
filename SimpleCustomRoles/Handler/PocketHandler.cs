using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using SimpleCustomRoles.Helpers;

namespace SimpleCustomRoles.Handler;

public class PocketHandler : CustomEventsHandler
{
    public override void OnPlayerEnteringPocketDimension(PlayerEnteringPocketDimensionEventArgs ev)
    {
        if (!CustomRoleHelpers.TryGetCustomRole(ev.Player, out var role))
            return;
        ev.IsAllowed = role.Pocket.CanEnter;
    }

    public override void OnPlayerEnteredPocketDimension(PlayerEnteredPocketDimensionEventArgs ev)
    {
        if (!CustomRoleHelpers.TryGetCustomRoleStorage(ev.Player, out var storage))
            return;
        storage.ChangeScale();
    }

    public override void OnPlayerLeavingPocketDimension(PlayerLeavingPocketDimensionEventArgs ev)
    {
        if (!CustomRoleHelpers.TryGetCustomRole(ev.Player, out var role))
            return;
        ev.IsAllowed = role.Pocket.CanExit;
        if (ev.IsAllowed)
            ev.IsSuccessful = role.Pocket.ForceExit;
    }

    public override void OnPlayerLeftPocketDimension(PlayerLeftPocketDimensionEventArgs ev)
    {
        if (!CustomRoleHelpers.TryGetCustomRoleStorage(ev.Player, out var storage))
            return;
        if (!ev.IsSuccessful)
            return;
        storage.ChangeScale();
    }
}
