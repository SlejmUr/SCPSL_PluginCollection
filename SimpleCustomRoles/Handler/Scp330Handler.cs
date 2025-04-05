using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Wrappers;
using SimpleCustomRoles.Helpers;

namespace SimpleCustomRoles.Handler;

internal class Scp330Handler : CustomEventsHandler
{
    public static void DroppingScp330(PlayerDroppingItemEventArgs args)
    {
        if (!CustomRoleHelpers.TryGetCustomRole(args.Player, out var role))
            return;
        args.IsAllowed = role.Advanced.Candy.GlobalCanDropCandy;
        if (args.Item is not Scp330Item item)
            return;
        if (item.Base.IsCandySelected && role.Advanced.Candy.SpecialCandy.TryGetValue(item.Base.Candies[item.Base.SelectedCandyId], out var specific))
            args.IsAllowed = specific.CanEatCandy;
    }

    public override void OnPlayerInteractingScp330(PlayerInteractingScp330EventArgs ev)
    {
        if (!CustomRoleHelpers.TryGetCustomRole(ev.Player, out var role))
            return;
        if (ev.IsAllowed)
        {
            ev.IsAllowed = role.Advanced.Candy.CanTakeCandy;
            if (!ev.IsAllowed)
                return;
        }

        ev.AllowPunishment = ev.Uses >= role.Advanced.Candy.MaxTakeCandy;
        if (ev.IsAllowed && !ev.AllowPunishment && role.Advanced.Candy.ShowCandyLeft)
            ev.Player.SendHint($"You can take {(role.Advanced.Candy.MaxTakeCandy - ev.Uses - 1)} more candy", 2);
    }

    /*
    public static void InteractingScp330(InteractingScp330EventArgs args)
    {
        if (Main.Instance.PlayerCustomRole.TryGetValue(args.Player.UserId, out var role))
        {
            if (args.IsAllowed)
            {
                args.IsAllowed = role.Advanced.Candy.CanTakeCandy;
                if (!args.IsAllowed)
                    return;
            }

            args.ShouldSever = args.UsageCount >= role.Advanced.Candy.MaxTakeCandy;
            if (args.IsAllowed && !args.ShouldSever && role.Advanced.Candy.ShowCandyLeft)
            {
                args.Player.ShowHint($"You can take {(role.Advanced.Candy.MaxTakeCandy - args.UsageCount - 1)} more candy", 2);
            }
        }
    }
    */
}
