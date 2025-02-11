using LabApi.Events.Arguments.Scp049Events;
using MEC;
using SimpleCustomRoles.RoleInfo;

namespace SimpleCustomRoles.Handler;

public class SCP_049_Handler
{
    public static void FinishingRecall(Scp049ResurrectingBodyEventArgs args)
    {
        if (Main.Instance.PlayerCustomRole.TryGetValue(args.Player.UserId, out var role))
        {
            if (!role.Scp_Specific.Scp049.CanRecall)
            {
                args.IsAllowed = false;
                return;
            }
            if (role.Scp_Specific.Scp049.RoleAfterKilled != PlayerRoles.RoleTypeId.None)
            {
                Timing.CallDelayed(0.2f, () =>
                {
                    args.Player.SetRole(role.Scp_Specific.Scp049.RoleAfterKilled, PlayerRoles.RoleChangeReason.Revived, PlayerRoles.RoleSpawnFlags.None);
                });
                
                return;
            }
            else
            {
                if (!string.IsNullOrEmpty(role.Scp_Specific.Scp049.RoleNameToRespawnAs))
                {
                    var customRoleInfo = Main.Instance.AfterDeathRoles.Where(x => x.RoleName == role.Scp_Specific.Scp049.RoleNameToRespawnAs).FirstOrDefault();
                    if (customRoleInfo == null)
                        return;
                    Timing.CallDelayed(0.2f, () =>
                    {
                        RoleSetter.SetCustomInfoToPlayer(args.Target, customRoleInfo);
                    });
                    return;
                }
                else if (role.Scp_Specific.Scp049.RoleNameRandom.Count != 0)
                {
                    var customRoleInfo = Main.Instance.AfterDeathRoles.Where(x => x.RoleName == role.Scp_Specific.Scp049.RoleNameRandom.RandomItem()).FirstOrDefault();
                    if (customRoleInfo == null)
                        return;
                    Timing.CallDelayed(0.2f, () =>
                    {
                        RoleSetter.SetCustomInfoToPlayer(args.Target, customRoleInfo);
                    });
                    return;
                }
            }

        }

        var list = Main.Instance.SPC_SpecificRoles.Where(x=>x.Scp_Specific.Scp0492.CanSpawnIfNoCustom094 == true ).ToList();

        if (list.Count > 0) 
        {
            role = list.RandomItem();
            var random = RandomGenerator.GetInt16(1, 10000, true);
            if (random <= role.Scp_Specific.Scp0492.ChanceForSpawn)
            {
                LabApi.Features.Console.Logger.Info("Role selected to revive");
                Timing.CallDelayed(0.2f, () =>
                {
                    RoleSetter.SetCustomInfoToPlayer(args.Target, role);
                });
                Main.Instance.SPC_SpecificRoles.Remove(role);
            }
            
        }
    }
}
