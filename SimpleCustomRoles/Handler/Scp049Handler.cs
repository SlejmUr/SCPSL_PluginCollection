using LabApi.Events.Arguments.Scp049Events;
using LabApi.Events.CustomHandlers;
using MEC;
using SimpleCustomRoles.Helpers;

namespace SimpleCustomRoles.Handler;

public class Scp049Handler : CustomEventsHandler
{
    public override void OnScp049ResurrectingBody(Scp049ResurrectingBodyEventArgs ev)
    {
        if (CustomRoleHelpers.TryGetCustomRole(ev.Player, out var role))
        {
            if (!role.ScpSpecific.Scp049.CanRecall)
            {
                ev.IsAllowed = false;
                return;
            }
            if (role.ScpSpecific.Scp049.AfterDeath != PlayerRoles.RoleTypeId.None)
            {
                Timing.CallDelayed(0.2f, () =>
                {
                    ev.Player.SetRole(role.ScpSpecific.Scp049.AfterDeath, PlayerRoles.RoleChangeReason.Revived, PlayerRoles.RoleSpawnFlags.None);
                });

                return;
            }
            else
            {
                if (!string.IsNullOrEmpty(role.ScpSpecific.Scp049.RoleName))
                {
                    var customRoleInfo = Main.Instance.AfterDeathRoles.Where(x => x.Rolename == role.ScpSpecific.Scp049.RoleName).FirstOrDefault();
                    if (customRoleInfo == null)
                        return;
                    Timing.CallDelayed(0.2f, () =>
                    {
                        CustomRoleHelpers.SetCustomInfoToPlayer(ev.Target, customRoleInfo);
                    });
                    return;
                }
                else if (role.ScpSpecific.Scp049.Random.Count != 0)
                {
                    var customRoleInfo = Main.Instance.AfterDeathRoles.Where(x => x.Rolename == role.ScpSpecific.Scp049.Random.RandomItem()).FirstOrDefault();
                    if (customRoleInfo == null)
                        return;
                    Timing.CallDelayed(0.2f, () =>
                    {
                        CustomRoleHelpers.SetCustomInfoToPlayer(ev.Target, customRoleInfo);
                    });
                    return;
                }
            }

        }

        var list = Main.Instance.ScpSpecificRoles.Where(x => x.ScpSpecific.Scp0492.CanSpawnIfNoCustom094 == true).ToList();

        if (list.Count > 0)
        {
            role = list.RandomItem();
            var random = RandomGenerator.GetInt16(1, 10000, true);
            if (random <= role.ScpSpecific.Scp0492.ChanceForSpawn)
            {
                LabApi.Features.Console.Logger.Info("Role selected to revive");
                Timing.CallDelayed(0.2f, () =>
                {
                    CustomRoleHelpers.SetCustomInfoToPlayer(ev.Target, role);
                });
                Main.Instance.ScpSpecificRoles.Remove(role);
            }

        }
    }
}
