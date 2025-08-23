using LabApi.Events.Arguments.Scp096Events;
using LabApi.Events.CustomHandlers;
using SimpleCustomRoles.Helpers;

namespace SimpleCustomRoles.Handler;

public class Scp096Handler : CustomEventsHandler
{
    public override void OnScp096AddingTarget(Scp096AddingTargetEventArgs ev)
    {
        if (CustomRoleHelpers.TryGetCustomRole(ev.Target, out var role))
            ev.IsAllowed = role.Extra.Trigger096;
    }

    public override void OnScp096PryingGate(Scp096PryingGateEventArgs ev)
    {
        if (!CustomRoleHelpers.TryGetCustomRole(ev.Player, out var role))
            return;
        ev.IsAllowed = role.Scp.Scp096.CanPry;
        if (!ev.IsAllowed)
            return;
        if (role.Scp.Scp096.DoorToNotPryOn.Contains(ev.Gate.DoorName))
            ev.IsAllowed = false;
    }

    public override void OnScp096Charging(Scp096ChargingEventArgs ev)
    {
        if (CustomRoleHelpers.TryGetCustomRole(ev.Player, out var role))
            ev.IsAllowed = role.Scp.Scp096.CanCharge;
    }

    public override void OnScp096TryingNotToCry(Scp096TryingNotToCryEventArgs ev)
    {
        if (CustomRoleHelpers.TryGetCustomRole(ev.Player, out var role))
            ev.IsAllowed = role.Scp.Scp096.CanTryingNotToCry;
    }

    public override void OnScp096Enraging(Scp096EnragingEventArgs ev)
    {
        if (CustomRoleHelpers.TryGetCustomRole(ev.Player, out var role))
            ev.InitialDuration = role.Scp.Scp096.Enraging.MathCalculation(ev.InitialDuration);
    }
}
