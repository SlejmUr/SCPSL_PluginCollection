
using LabApi.Events.Arguments.Scp173Events;
using LabApi.Events.CustomHandlers;
using SimpleCustomRoles.Helpers;

namespace SimpleCustomRoles.Handler;

public class Scp173Handler : CustomEventsHandler
{
    public override void OnScp173CreatingTantrum(Scp173CreatingTantrumEventArgs ev)
    {
        if (CustomRoleHelpers.TryGetCustomRole(ev.Player, out var role))
            ev.IsAllowed = role.ScpSpecific.Scp173.CanPlaceTantrum;
    }

    public override void OnScp173BreakneckSpeedChanging(Scp173BreakneckSpeedChangingEventArgs ev)
    {
        if (CustomRoleHelpers.TryGetCustomRole(ev.Player, out var role))
            ev.IsAllowed = role.ScpSpecific.Scp173.CanUseBreakneckSpeed;
    }
}
