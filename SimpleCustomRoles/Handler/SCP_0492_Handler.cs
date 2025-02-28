using Exiled.Events.EventArgs.Scp0492;
using SimpleCustomRoles.RoleInfo;

namespace SimpleCustomRoles.Handler;

public class SCP_0492_Handler
{
    public static void ConsumingCorpse(ConsumingCorpseEventArgs args)
    {
        if (!Main.Instance.PlayerCustomRole.TryGetValue(args.Player.UserId, out CustomRoleInfo role))
            return;
        args.IsAllowed = role.Scp_Specific.Scp0492.CanConsumeCorpse;
    }
}
