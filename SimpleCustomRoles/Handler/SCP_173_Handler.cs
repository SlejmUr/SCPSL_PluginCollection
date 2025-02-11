
using LabApi.Events.Arguments.Scp173Events;

namespace SimpleCustomRoles.Handler;

public class SCP_173_Handler
{
    public static void PlacingTantrum(Scp173CreatingTantrumEventArgs args)
    {
        if (Main.Instance.PlayerCustomRole.TryGetValue(args.Player.UserId, out var role))
        {
            args.IsAllowed = role.Scp_Specific.Scp173.CanPlaceTantrum;
        }
    }
    public static void UsingBreakneckSpeeds(Scp173BreakneckSpeedChangingEventArgs args)
    {
        if (Main.Instance.PlayerCustomRole.TryGetValue(args.Player.UserId, out var role))
        {
            args.IsAllowed = role.Scp_Specific.Scp173.CanUseBreakneckSpeed;
        }
    }
}
