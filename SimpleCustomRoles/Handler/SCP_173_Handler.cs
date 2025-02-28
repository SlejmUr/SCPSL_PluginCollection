using Exiled.Events.EventArgs.Scp173;
using SimpleCustomRoles.RoleInfo;

namespace SimpleCustomRoles.Handler;

public class SCP_173_Handler
{
    public static void PlacingTantrum(PlacingTantrumEventArgs args)
    {
        if (!Main.Instance.PlayerCustomRole.TryGetValue(args.Player.UserId, out CustomRoleInfo role))
            return;
        args.IsAllowed = role.Scp_Specific.Scp173.CanPlaceTantrum;
    }
    public static void UsingBreakneckSpeeds(UsingBreakneckSpeedsEventArgs args)
    {
        if (!Main.Instance.PlayerCustomRole.TryGetValue(args.Player.UserId, out CustomRoleInfo role))
            return;
        args.IsAllowed = role.Scp_Specific.Scp173.CanUseBreakneckSpeed;
    }
}
