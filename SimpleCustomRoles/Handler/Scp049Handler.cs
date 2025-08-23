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
            if (!role.Scp.Scp049.CanRecall)
            {
                ev.IsAllowed = false;
                return;
            }
            if (CustomRoleHelpers.SetNewRole(ev.Target, role.Scp.Scp049))
                return;
        }

        var list = Main.Instance.ScpSpecificRoles.Where(x => x.Scp.Scp0492.CanSpawnIfNoCustom094 == true).ToList();

        if (list.Count > 0)
        {
            role = list.RandomItem();
            var random = RandomGenerator.GetInt16(1, 10000, true);
            if (random <= role.Scp.Scp0492.ChanceForSpawn)
            {
                Timing.CallDelayed(0.2f, () =>
                {
                    CustomRoleHelpers.SetCustomInfoToPlayer(ev.Target, role);
                });
                Main.Instance.ScpSpecificRoles.Remove(role);
            }

        }
    }

    public override void OnScp049StartingResurrection(Scp049StartingResurrectionEventArgs ev)
    {
        if (!CustomRoleHelpers.TryGetCustomRole(ev.Target, out var role))
            return;
        if (role.Extra.CannotRevivedByScp049)
            ev.IsAllowed = false;
    }
}
