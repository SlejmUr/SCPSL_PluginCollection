using Exiled.Events.EventArgs.Scp0492;

namespace SimpleCustomRoles.Handler
{
    public class SCP_0492_Handler
    {
        public static void ConsumingCorpse(ConsumingCorpseEventArgs args)
        {
            if (Main.Instance.PlayerCustomRole.TryGetValue(args.Player.UserId, out var role))
            {
                if (!role.SCP_Specific.SCP_0492.CanConsumeCorpse)
                {
                    args.IsAllowed = false;
                    return;
                }
            }
        }
    }
}
