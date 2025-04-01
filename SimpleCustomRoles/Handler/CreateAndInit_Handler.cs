using Exiled.API.Extensions;
using Exiled.API.Features;
using MEC;
using Respawning.Waves;
using SimpleCustomRoles.RoleInfo;

namespace SimpleCustomRoles.Handler;

internal class CreateAndInit_Handler
{
    public static void RespawnManager_ServerOnRespawned(SpawnableWaveBase wave, List<ReferenceHub> players)
    {
        if (players.Count == 0)
            return;
        // create a tmp list to store set roles (to later clear)
        List<CustomRoleInfo> tmp = [];

        // reset every player if had custom roles.
        foreach (var item in players)
        {
            var player = Player.List.FirstOrDefault(x => x.ReferenceHub == item);
            if (player == null)
                continue;
            RoleSetter.UnSetCustomInfoToPlayer(player);
        }

        foreach (var item in Main.Instance.InWaveRoles.Where(x => x.SpawnWaveSpecific.Faction == wave.TargetFaction && x.RoleType == CustomRoleType.InWave))
        {
            // miminum check
            if (!item.SpawnWaveSpecific.SkipMinimumCheck && item.SpawnWaveSpecific.MinimumTeamMemberRequired > players.Count)
                continue;

            var referenceHub = players.Where(x => x.roleManager.CurrentRole.RoleTypeId == item.RoleToReplace).GetRandomValue();

            var player = Player.List.FirstOrDefault(x => x.ReferenceHub == referenceHub);
            if (player == null)
                continue;
            if (Main.Instance.Config.Debug)
                Log.Info("Player choosen: " + player.UserId);
            // set the player as a role
            RoleSetter.SetFromCMD(player, item);
            tmp.Add(item);
        }
        // remove
        foreach (var item in tmp)
        {
            Main.Instance.InWaveRoles.Remove(item);
        }

        // execute onwave event calls.
        foreach (var item in Main.Instance.PlayerCustomRole)
        {
            var player = Player.List.Where(x => x.RawUserId == item.Key).FirstOrDefault();
            if (!string.IsNullOrEmpty(item.Value.EventCaller.OnSpawnWave))
            {
                // Call event
                Server.ExecuteCommand($"{item.Value.EventCaller.OnSpawnWave} {player.Id} {item.Value.RoleName} {wave.TargetFaction} {players.Count}");
            }
        }
    }

    public static void ReloadRoles()
    {
        Main.Instance.RegularRoles = [];
        Main.Instance.PlayerCustomRole = [];
        Main.Instance.InWaveRoles = [];
        Main.Instance.AfterDeathRoles = [];
        Main.Instance.SPC_SpecificRoles = [];
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
                if (item.RoleType == CustomRoleType.SPC_Specific)
                    Main.Instance.SPC_SpecificRoles.Add(item);
                if (item.RoleType == CustomRoleType.InWave)
                    Main.Instance.InWaveRoles.Add(item);
            }
        }
    }

    public static void WaitingForPlayers()
    {
        Main.Instance.PlayerCustomRole = [];
        Main.Instance.RegularRoles = [];
        Main.Instance.InWaveRoles = [];
        Main.Instance.AfterDeathRoles = [];
        Main.Instance.SPC_SpecificRoles = [];
        Main.Instance.EscapeRoles = [];
        Main.Instance.RolesLoader.Load();
        Log.Debug("Loaded custom roles!");
    }

    public static void RoundStarted()
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
                Log.Debug($"After Death Role added: " + item.RoleName);
                Main.Instance.AfterDeathRoles.Add(item);
                continue;
            }
            if (item.RoleType == CustomRoleType.Escape)
            {
                Log.Debug($"Escape Role added: " + item.RoleName);
                Main.Instance.EscapeRoles.Add(item);
                continue;
            }
            for (int i = 0; i < item.SpawnAmount; i++)
            {
                bool IsSpawning = false;
                var random = RandomGenerator.GetInt16(1, 10000, true);
                if (!RoleSetter.IsShouldSpawn(item))
                {
                    Log.Debug($"Role has been no longer spawn: {item.RoleName} (Reason: Player limited)");
                    continue;
                }
                int chance = item.SpawnChance;
                if (Main.Instance.Config.UsePlayerPercent)
                {
                    Log.Debug($"Basic chance: {chance}");
                    float chance_mulitplier = ((float)Server.PlayerCount / (float)Server.MaxPlayerCount);
                    chance = (int)(chance * chance_mulitplier);
                    Log.Debug($"Final chance: {chance}");
                }
                chance = (int)((float)chance * Main.Instance.Config.SpawnRateMultiplier);
                if (random <= chance)
                {
                    IsSpawning = true;
                    if (item.RoleType == CustomRoleType.SPC_Specific)
                        Main.Instance.SPC_SpecificRoles.Add(item);
                    if (item.RoleType == CustomRoleType.SPC_Specific)
                        Main.Instance.InWaveRoles.Add(item);
                    else
                        Main.Instance.RegularRoles.Add(item);
                }
                Log.Debug($"Rolled chance: {random}/{chance} for Role {item.RoleName}. Role is " + (IsSpawning ? "" : "NOT ") + "spawning.");
            }
        }

        foreach (var item in Main.Instance.RegularRoles)
        {
            Player player = null;
            if (item.RoleToReplace == PlayerRoles.RoleTypeId.None && item.ReplaceFromTeam != PlayerRoles.Team.Dead)
            {
                player = Player.List.GetRandomValue(x => x.Role.Team == item.ReplaceFromTeam);
            }
            else
            {
                player = Player.List.GetRandomValue(x => x.Role == item.RoleToReplace);
            }
            if (player == null)
                continue;
            if (Main.Instance.Config.Debug)
                Log.Info("Player Selected to spawn: " + player.UserId);
            RoleSetter.SetCustomInfoToPlayer(player, item);
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
