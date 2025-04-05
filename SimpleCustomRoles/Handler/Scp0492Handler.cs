using LabApi.Events.Arguments.Scp0492Events;
using LabApi.Events.CustomHandlers;
using SimpleCustomRoles.Helpers;

namespace SimpleCustomRoles.Handler;

public class Scp0492Handler : CustomEventsHandler
{
    public override void OnScp0492ConsumingCorpse(Scp0492ConsumingCorpseEventArgs ev)
    {
        if (CustomRoleHelpers.TryGetCustomRole(ev.Player, out var role))
        {
            ev.IsAllowed = role.ScpSpecific.Scp0492.CanConsumeCorpse;
        }
    }
}
