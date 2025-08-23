using PlayerRoles.PlayableScps.Scp049.Zombies;
using LabApi.Events.Arguments.Scp0492Events;
using LabApi.Events.CustomHandlers;
using SimpleCustomRoles.Helpers;

namespace SimpleCustomRoles.Handler;

public class Scp0492Handler : CustomEventsHandler
{
    public override void OnScp0492ConsumingCorpse(Scp0492ConsumingCorpseEventArgs ev)
    {
        if (!CustomRoleHelpers.TryGetCustomRole(ev.Player, out var role))
            return;
        ev.IsAllowed = role.Scp.Scp0492.CanConsumeCorpse;
        if (!ev.IsAllowed)
            return;
        ev.HealAmount = role.Scp.Scp0492.ConsumeHealth.MathCalculation(ev.HealAmount);
    }

    public override void OnScp0492StartingConsumingCorpse(Scp0492StartingConsumingCorpseEventArgs ev)
    {
        if (!CustomRoleHelpers.TryGetCustomRole(ev.Player, out var role))
            return;
        if (ev.Error is ZombieConsumeAbility.ConsumeError.CannotCancel or ZombieConsumeAbility.ConsumeError.TargetNotValid)
            return;
        if (!role.Scp.Scp0492.ForceEat)
            return;
        ev.IsAllowed = true;
    }
}
