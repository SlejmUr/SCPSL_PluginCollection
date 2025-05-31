using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Wrappers;
using SimpleCustomRoles.Helpers;

namespace SimpleCustomRoles.Handler;

internal class Scp330Handler : CustomEventsHandler
{
    public override void OnPlayerInteractingScp330(PlayerInteractingScp330EventArgs ev)
    {
        if (!CustomRoleHelpers.TryGetCustomRole(ev.Player, out var role))
            return;
        if (ev.IsAllowed)
        {
            ev.IsAllowed = role.Candy.CanTakeCandy;
            if (!ev.IsAllowed)
                return;
        }

        ev.AllowPunishment = ev.Uses >= role.Candy.MaxTakeCandy;
        if (ev.IsAllowed && !ev.AllowPunishment && role.Candy.ShowCandyLeft)
            ev.Player.SendHint($"You can take {(role.Candy.MaxTakeCandy - ev.Uses - 1)} more candy", 2);
    }


    public override void OnPlayerUsingItem(PlayerUsingItemEventArgs ev)
    {
        if (!CustomRoleHelpers.TryGetCustomRole(ev.Player, out var role))
            return;
        if (ev.UsableItem is not Scp330Item item)
            return;
        ev.IsAllowed = role.Candy.GlobalCanEatCandy;
        if (item.Base.IsCandySelected && role.Deniable.Candies.TryGetValue(item.Base.Candies[item.Base.SelectedCandyId], out var deniable))
            ev.IsAllowed = deniable.CanUse;
    }

    public override void OnPlayerDroppingItem(PlayerDroppingItemEventArgs ev)
    {
        if (!CustomRoleHelpers.TryGetCustomRole(ev.Player, out var role))
            return;
        if (ev.Item is not Scp330Item item)
            return;
        ev.IsAllowed = role.Candy.GlobalCanDropCandy;
        if (item.Base.IsCandySelected && role.Deniable.Candies.TryGetValue(item.Base.Candies[item.Base.SelectedCandyId], out var deniable))
            ev.IsAllowed = deniable.CanDrop;
    }
}
