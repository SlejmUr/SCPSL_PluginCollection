using Exiled.API.Features;
using Exiled.Events.EventArgs.Scp049;
using MEC;
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
                    Timing.CallDelayed(0.2f, () =>
                    {
                        args.Player.Role.Set(role.SCP_Specific.SCP_049.RoleAfterKilled, PlayerRoles.RoleSpawnFlags.None);
                    });
                    
                    return;
                }
                else
                {
                    if (!string.IsNullOrEmpty(role.SCP_Specific.SCP_049.RoleNameToRespawnAs))
                    {
                        var customRoleInfo = Main.Instance.ScpSpecificRoles.Where(x => x.RoleName == role.Advanced.DeadBy.RoleNameToRespawnAs).FirstOrDefault();
                        if (customRoleInfo == null)
                            return;
                        Timing.CallDelayed(0.2f, () =>
                        {
                            RoleSetter.SetCustomInfoToPlayer(args.Target, customRoleInfo);
                        });
                        
                        Main.Instance.ScpSpecificRoles.Remove(customRoleInfo);
                        return;
                    }
                    else if (role.SCP_Specific.SCP_049.RoleNameRandom.Count != 0)
                    {
                        var customRoleInfo = Main.Instance.ScpSpecificRoles.Where(x => x.RoleName == role.Advanced.DeadBy.RoleNameRandom.RandomItem()).FirstOrDefault();
                        if (customRoleInfo == null)
                            return;
                        Timing.CallDelayed(0.2f, () =>
                        {
                            RoleSetter.SetCustomInfoToPlayer(args.Target, customRoleInfo);
                        });
                        Main.Instance.ScpSpecificRoles.Remove(customRoleInfo);
                        return;
                    }
                }

            }

            var list = Main.Instance.ScpSpecificRoles.Where(x=>x.SCP_Specific.SCP_0492.CanSpawnIfNoCustom094 == true ).ToList();

            if (list.Count > 0) 
            {
                role = list.RandomItem();
                var random = RandomGenerator.GetInt16(1, 10000, true);
                if (random <= role.SCP_Specific.SCP_0492.ChanceForSpawn)
                {
                    Log.Info("Role selected to revive");
                    Timing.CallDelayed(0.2f, () =>
                    {
                        RoleSetter.SetCustomInfoToPlayer(args.Target, role);
                    });
                    Main.Instance.ScpSpecificRoles.Remove(role);
                }
                
            }
        }
    }
}
