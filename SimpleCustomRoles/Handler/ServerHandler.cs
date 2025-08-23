using LabApi.Events.Arguments.ServerEvents;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Wrappers;
using LabApiExtensions.Extensions;
using MEC;
using SimpleCustomRoles.Helpers;
using SimpleCustomRoles.RoleInfo;
using SimpleCustomRoles.RoleYaml;
using SimpleCustomRoles.RoleYaml.Enums;

namespace SimpleCustomRoles.Handler;

internal class ServerHandler : CustomEventsHandler
{
    public override void OnServerRoundEnded(RoundEndedEventArgs ev)
    {
        //AppearanceSyncExtension.Stop();
    }

    public static void ReloadRoles()
    {
        Main.Instance.InWaveRoles = [];
        Main.Instance.ScpSpecificRoles = [];
        RolesLoader.Load();
        foreach (var item in RolesLoader.RoleInfos)
        {
            if (item.RoleType == CustomRoleType.AfterDead)
            {
                continue;
            }
            for (int i = 0; i < item.Spawn.SpawnAmount; i++)
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
        Main.Instance.InWaveRoles = [];
        Main.Instance.ScpSpecificRoles = [];
        Main.Instance.EscapeRoles = [];
        RolesLoader.Load();
        CL.Info("Loaded custom roles!");
    }

    public override void OnServerRoundStarted()
    {
        //AppearanceSyncExtension.Start();
        if (Main.Instance.Config.IsPaused)
            return;

        // Round lock for certain roles.
        if (!Round.IsLocked)
        {
            Round.IsLocked = true;
            Timing.CallDelayed(5, () =>
            {
                Round.IsLocked = false;
            });
        }
        List<CustomRoleBaseInfo> RegularRoles = [];
        foreach (var item in RolesLoader.RoleInfos)
        {
            if (item.RoleType == CustomRoleType.AfterDead)
                continue;
            for (int i = 0; i < item.Spawn.SpawnAmount; i++)
            {
                bool IsSpawning = false;
                var random = RandomGenerator.GetInt16(1, 10000, true);
                if (!CustomRoleHelpers.IsShouldSpawn(item))
                {
                    CL.Debug($"Role has been no longer spawn: {item.Rolename} (Reason: Player limited)", Main.Instance.Config.Debug);
                    continue;
                }
                if (!GroupHelper.CanSpawn(item.Rolegroup, ref RegularRoles))
                {
                    CL.Debug($"Role has been no longer spawn: {item.Rolename} (Reason: Group limited)", Main.Instance.Config.Debug);
                    break;
                }
                GroupHelper.DenyCustomRoles(item.Rolegroup, ref RegularRoles);
                int chance = item.Spawn.SpawnChance;
                if (Main.Instance.Config.UsePlayerPercent && !item.Spawn.DenyChance)
                {
                    CL.Debug($"Basic chance: {chance}", Main.Instance.Config.Debug);
                    float chance_mulitplier = ((float)Server.PlayerCount / (float)Server.MaxPlayers);
                    chance = (int)(chance * chance_mulitplier);
                    CL.Debug($"Final chance: {chance}", Main.Instance.Config.Debug);
                }
                if (!item.Spawn.DenyChance)
                    chance = (int)((float)chance * Main.Instance.Config.SpawnRateMultiplier);
                if (random <= chance)
                {
                    IsSpawning = true;
                    if (item.RoleType == CustomRoleType.ScpSpecific)
                        Main.Instance.ScpSpecificRoles.Add(item);
                    if (item.RoleType == CustomRoleType.InWave)
                        Main.Instance.InWaveRoles.Add(item);
                    else
                        RegularRoles.Add(item);
                }
                CL.Debug($"Rolled chance: {random}/{chance} for Role {item.Rolename}. Role is " + (IsSpawning ? "" : "NOT ") + "spawning.", Main.Instance.Config.Debug);
            }
        }
        List<Player> NotRoll = [];
        Timing.CallDelayed(0.2f, () =>
        {
            foreach (var item in RegularRoles)
            {
                Player player = null;

                if (item.ReplaceRole == PlayerRoles.RoleTypeId.None && item.ReplaceTeam != PlayerRoles.Team.Dead)
                {
                    var list = Player.ReadyList.Where(x => x.Team == item.ReplaceTeam && !CustomRoleHelpers.Contains(x) && !NotRoll.Contains(x)).ToList();
                    if (list.Count > 0)
                        player = list.RandomItem();
                }
                else
                {
                    var list = Player.ReadyList.Where(x => x.Role == item.ReplaceRole && !CustomRoleHelpers.Contains(x) && !NotRoll.Contains(x)).ToList();
                    if (list.Count > 0)
                        player = list.RandomItem();
                }
                if (player == null)
                    continue;

                CL.Info($"Player Selected to spawn: {player.UserId} as {item.Rolename}");
                NotRoll.Add(player);
                CustomRoleHelpers.SetCustomInfoToPlayer(player, item);
            }
            RegularRoles.Clear();
        });
    }
}
