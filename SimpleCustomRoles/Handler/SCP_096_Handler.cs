using Exiled.Events.EventArgs.Scp096;
using SimpleCustomRoles.RoleInfo;

namespace SimpleCustomRoles.Handler;

public class SCP_096_Handler
{
    public static void AddingTarget(AddingTargetEventArgs args)
    {
        if (!Main.Instance.PlayerCustomRole.TryGetValue(args.Target.UserId, out CustomRoleInfo role))
            return;
        args.IsAllowed = role.Advanced.CanTrigger096;
    }

    public static void StartPryingGate(StartPryingGateEventArgs args)
    {
        if (!Main.Instance.PlayerCustomRole.TryGetValue(args.Player.UserId, out CustomRoleInfo role))
            return;
        if (role.Scp_Specific.Scp096.DoorToNotPryOn.Contains(args.Door.Type))
        {
            args.IsAllowed = false;
            return;
        }
        args.IsAllowed = role.Scp_Specific.Scp096.CanPry;
    }

    public static void Charging(ChargingEventArgs args)
    {
        if (!Main.Instance.PlayerCustomRole.TryGetValue(args.Player.UserId, out CustomRoleInfo role))
            return;
        args.IsAllowed = role.Scp_Specific.Scp096.CanCharge;
    }

    public static void TryingNotToCry(TryingNotToCryEventArgs args)
    {
        if (!Main.Instance.PlayerCustomRole.TryGetValue(args.Player.UserId, out CustomRoleInfo role))
            return;
        args.IsAllowed = role.Scp_Specific.Scp096.CanTryingNotToCry;
    }

    public static void Enraging(EnragingEventArgs args)
    {
        if (!Main.Instance.PlayerCustomRole.TryGetValue(args.Player.UserId, out CustomRoleInfo role))
            return;
        args.InitialDuration = RoleSetter.MathWithFloat(role.Scp_Specific.Scp096.Enraging.SetType, args.InitialDuration, role.Scp_Specific.Scp096.Enraging.Value);
    }
}
