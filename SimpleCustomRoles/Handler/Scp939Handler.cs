using LabApi.Events.Arguments.Scp939Events;
using LabApi.Events.CustomHandlers;
using SimpleCustomRoles.Helpers;

namespace SimpleCustomRoles.Handler;

internal class Scp939Handler : CustomEventsHandler
{
    public override void OnScp939CreatingAmnesticCloud(Scp939CreatingAmnesticCloudEventArgs ev)
    {
        if (CustomRoleHelpers.TryGetCustomRole(ev.Player, out var role))
            ev.IsAllowed = role.Scp.Scp939.CanCreateCloud;
    }
}
