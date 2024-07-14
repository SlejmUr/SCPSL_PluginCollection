using Exiled.Events.EventArgs.Scp096;

namespace SimpleCustomRoles.Handler
{
    public class SCP_096_Handler
    {
        public static void AddingTarget(AddingTargetEventArgs args)
        {
            if (Main.Instance.PlayerCustomRole.TryGetValue(args.Target.UserId, out var role))
            {
                args.IsAllowed = role.Advanced.CanTrigger096;
            }
        }
    }
}
