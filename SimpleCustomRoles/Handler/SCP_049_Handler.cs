using Exiled.Events.EventArgs.Scp049;
using SimpleCustomRoles.RoleInfo;
using System.Linq;

namespace SimpleCustomRoles.Handler
{
    public class SCP_049_Handler
    {
        public static void FinishingRecall(FinishingRecallEventArgs args)
        {
            if (Main.Instance.PlayerCustomRole.TryGetValue(args.Player.UserId, out var role))
            {
                if (!role.SCP_Specific.SCP_049.CanRecall)
                {
                    args.IsAllowed = false;
                    return;
                }
                if (role.SCP_Specific.SCP_049.RoleAfterKilled != PlayerRoles.RoleTypeId.None)
                {
                    args.Player.Role.Set(role.SCP_Specific.SCP_049.RoleAfterKilled, PlayerRoles.RoleSpawnFlags.None);
                }
                else
                {
                    if (!string.IsNullOrEmpty(role.SCP_Specific.SCP_049.RoleNameToRespawnAs))
                    {
                        var customRoleInfo = Main.Instance.ScpSpecificRoles.Where(x => x.RoleName == role.Advanced.DeadBy.RoleNameToRespawnAs).FirstOrDefault();
                        if (customRoleInfo == null)
                            return;
                        RoleSetter.SetCustomInfoToPlayer(args.Player, customRoleInfo);
                    }
                    else if (role.SCP_Specific.SCP_049.RoleNameRandom.Count != 0)
                    {
                        var customRoleInfo = Main.Instance.ScpSpecificRoles.Where(x => x.RoleName == role.Advanced.DeadBy.RoleNameRandom.RandomItem()).FirstOrDefault();
                        if (customRoleInfo == null)
                            return;
                        RoleSetter.SetCustomInfoToPlayer(args.Player, customRoleInfo);
                    }
                }

            }
        }
    }
}
