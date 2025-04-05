using LabApi.Events.CustomHandlers;
using LabApi.Features.Wrappers;
using MEC;
using SimpleCustomRoles.Helpers;
using SimpleCustomRoles.RoleInfo;

namespace SimpleCustomRoles.Handler;

internal class ServerHandler : CustomEventsHandler
{
    public static void ReloadRoles()
    {
        Main.Instance.RegularRoles = [];
        Main.Instance.InWaveRoles = [];
        Main.Instance.AfterDeathRoles = [];
        Main.Instance.ScpSpecificRoles = [];
        Main.Instance.RolesLoader.Load();
        foreach (var item in Main.Instance.RolesLoader.RoleInfos)
        {
            if (item.RoleType == CustomRoleType.AfterDead)
            {
                Main.Instance.AfterDeathRoles.Add(item);
                continue;
            }
            for (int i = 0; i < item.SpawnAmount; i++)
            {
                if (item.RoleType == CustomRoleType.ScpSpecific)
                    Main.Instance.ScpSpecificRoles.Add(item);
                if (item.RoleType == CustomRoleType.InWave)
                    Main.Instance.InWaveRoles.Add(item);
            }

        }
    }

    public override void OnServerWaitingForPlayers()
    {
        Main.Instance.RegularRoles = [];
        Main.Instance.InWaveRoles = [];
        Main.Instance.AfterDeathRoles = [];
        Main.Instance.ScpSpecificRoles = [];
        Main.Instance.EscapeRoles = [];
        Main.Instance.RolesLoader.Load();
        CL.Info("Loaded custom roles!");
    }

    public override void OnServerRoundStarted()
    {
        if (Main.Instance.Config.IsPaused)
            return;

        // Round lock for certain roles.
        bool do_lock = false;
        if (!Round.IsLocked)
        {
            Round.IsLocked = true;
            do_lock = true;
        }

        foreach (var item in Main.Instance.RolesLoader.RoleInfos)
        {
            if (item.RoleType == CustomRoleType.AfterDead)
            {
                CL.Debug($"After Death Role added: " + item.Rolename, Main.Instance.Config.Debug);
                Main.Instance.AfterDeathRoles.Add(item);
                continue;
            }
            if (item.RoleType == CustomRoleType.Escape)
            {
                CL.Debug($"Escape Role added: " + item.Rolename, Main.Instance.Config.Debug);
                Main.Instance.EscapeRoles.Add(item);
                continue;
            }
            for (int i = 0; i < item.SpawnAmount; i++)
            {
                bool IsSpawning = false;
                var random = RandomGenerator.GetInt16(1, 10000, true);
                if (!CustomRoleHelpers.IsShouldSpawn(item))
                {
                    CL.Debug($"Role has been no longer spawn: {item.Rolename} (Reason: Player limited)", Main.Instance.Config.Debug);
                    continue;
                }
                int chance = item.SpawnChance;
                if (Main.Instance.Config.UsePlayerPercent && !item.DenyChance)
                {
                    CL.Debug($"Basic chance: {chance}", Main.Instance.Config.Debug);
                    float chance_mulitplier = ((float)Server.PlayerCount / (float)Server.MaxPlayers);
                    chance = (int)(chance * chance_mulitplier);
                    CL.Debug($"Final chance: {chance}", Main.Instance.Config.Debug);
                }
                if (!item.DenyChance)
                    chance = (int)((float)chance * Main.Instance.Config.SpawnRateMultiplier);
                if (random <= chance)
                {
                    IsSpawning = true;
                    if (item.RoleType == CustomRoleType.ScpSpecific)
                        Main.Instance.ScpSpecificRoles.Add(item);
                    if (item.RoleType == CustomRoleType.InWave)
                        Main.Instance.InWaveRoles.Add(item);
                    else
                        Main.Instance.RegularRoles.Add(item);
                }
                CL.Debug($"Rolled chance: {random}/{chance} for Role {item.Rolename}. Role is " + (IsSpawning ? "" : "NOT ") + "spawning.", Main.Instance.Config.Debug);
            }
        }


        foreach (var item in Main.Instance.RegularRoles)
        {
            Player player = null;
            if (item.ReplaceRole == PlayerRoles.RoleTypeId.None && item.ReplaceTeam != PlayerRoles.Team.Dead)
            {
                var list = Player.List.Where(x => x.Team == item.ReplaceTeam && CustomRoleHelpers.Contains(x)).ToList();
                if (list.Count > 0)
                    player = list.RandomItem();
            }
            else
            {
                var list = Player.List.Where(x => x.Role == item.ReplaceRole && CustomRoleHelpers.Contains(x)).ToList();
                if (list.Count > 0)
                    player = list.RandomItem();
            }
            if (player == null)
                continue;
            CL.Debug("Player Selected to spawn: " + player.UserId, Main.Instance.Config.Debug);
            CustomRoleHelpers.SetCustomInfoToPlayer(player, item);
        }
        Main.Instance.RegularRoles.Clear();

        // if locked by us remove the lock
        if (do_lock)
            Timing.CallDelayed(5, () =>
            {
                Round.IsLocked = false;
            });
    }
}
