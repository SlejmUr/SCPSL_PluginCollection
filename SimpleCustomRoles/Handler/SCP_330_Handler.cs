using Exiled.API.Features;
using Exiled.Events.EventArgs.Scp330;
using Exiled.Events.Features;

namespace SimpleCustomRoles.Handler
{
    internal class SCP_330_Handler
    {
        public static void DroppingScp330(DroppingScp330EventArgs args)
        {
            if (Main.Instance.PlayerCustomRole.TryGetValue(args.Player.UserId, out var role))
            {
                args.IsAllowed = role.Advanced.Candy.GlobalCanDropCandy;


                if (role.Advanced.Candy.SpecialCandy.TryGetValue(args.Candy, out var specific))
                {
                    args.IsAllowed = specific.CanEatCandy;
                }
            }
        }

        public static void EatingScp330(EatingScp330EventArgs args)
        {
            if (Main.Instance.PlayerCustomRole.TryGetValue(args.Player.UserId, out var role))
            {
                args.IsAllowed = role.Advanced.Candy.GlobalCanEatCandy;

                if (role.Advanced.Candy.SpecialCandy.TryGetValue(args.Candy.Kind, out var specific))
                {
                    args.IsAllowed = specific.CanEatCandy;
                }
            }
        }

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
                if (args.IsAllowed && !args.ShouldSever)
                {
                    args.Player.ShowHint($"You can take {(role.Advanced.Candy.MaxTakeCandy - args.UsageCount - 1)} more candy", 5);
                }
            }
        }
    }
}
