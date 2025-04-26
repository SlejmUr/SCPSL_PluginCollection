using LabApi.Events.Arguments.Scp173Events;
using LabApi.Events.CustomHandlers;
using SimpleCustomRoles.Helpers;

namespace SimpleCustomRoles.Handler;

public class Scp173Handler : CustomEventsHandler
{
    public override void OnScp173AddingObserver(Scp173AddingObserverEventArgs ev)
    {
        if (CustomRoleHelpers.TryGetCustomRole(ev.Target, out var role))
            ev.IsAllowed = role.Extra.Observe173;
    }
    public override void OnScp173CreatingTantrum(Scp173CreatingTantrumEventArgs ev)
    {
        if (CustomRoleHelpers.TryGetCustomRole(ev.Player, out var role))
            ev.IsAllowed = role.Scp.Scp173.CanPlaceTantrum;
    }

    public override void OnScp173BreakneckSpeedChanging(Scp173BreakneckSpeedChangingEventArgs ev)
    {
        if (CustomRoleHelpers.TryGetCustomRole(ev.Player, out var role))
            ev.IsAllowed = role.Scp.Scp173.CanUseBreakneckSpeed;
    }
}
