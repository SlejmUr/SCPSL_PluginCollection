using LabApi.Events.Arguments.Scp096Events;
using SimpleCustomRoles.RoleInfo;

namespace SimpleCustomRoles.Handler;

public class SCP_096_Handler
{
    public static void AddingTarget(Scp096AddingTargetEventArgs args)
    {
        if (Main.Instance.PlayerCustomRole.TryGetValue(args.Target.UserId, out var role))
        {
            args.IsAllowed = role.Advanced.CanTrigger096;
        }
    }

    public static void StartPryingGate(Scp096PryingGateEventArgs args)
    {
        if (Main.Instance.PlayerCustomRole.TryGetValue(args.Player.UserId, out var role))
        {
            if (role.Scp_Specific.Scp096.DoorToNotPryOn.Contains(args.Gate.DoorName))
            {
                args.IsAllowed = false;
                return;
            }
            args.IsAllowed = role.Scp_Specific.Scp096.CanPry;
        }
    }

    public static void Charging(Scp096ChargingEventArgs args)
    {
        if (Main.Instance.PlayerCustomRole.TryGetValue(args.Player.UserId, out var role))
        {
            args.IsAllowed = role.Scp_Specific.Scp096.CanCharge;
        }
    }

    public static void TryingNotToCry(Scp096TryingNotToCryEventArgs args)
    {
        if (Main.Instance.PlayerCustomRole.TryGetValue(args.Player.UserId, out var role))
        {
            args.IsAllowed = role.Scp_Specific.Scp096.CanTryingNotToCry;
        }
    }

    public static void Enraging(Scp096EnragingEventArgs args)
    {
        if (Main.Instance.PlayerCustomRole.TryGetValue(args.Player.UserId, out var role))
        {
            args.InitialDuration = RoleSetter.MathWithFloat(role.Scp_Specific.Scp096.Enraging.SetType, args.InitialDuration, role.Scp_Specific.Scp096.Enraging.Value);
        }
    }
}
