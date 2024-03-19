using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Respawning;
using SimpleCustomRoles.RoleInfo;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MEC;

namespace SimpleCustomRoles.Handler
{
    internal class TheHandler
    {
        static List<CustomRoleInfo> PlayersRolled;
        static List<CustomRoleInfo> SpawningRoles;
        static Dictionary<string, CustomRoleInfo> PlayerCustomRole;
        static List<CustomRoleInfo> AfterDeathRoles;

        public static void Died(DiedEventArgs args)
        {
            Log.Info("Died");
            PlayerCustomRole.Remove(args.Player.UserId);
            args.Player.Scale = Vector3.one;
            args.Player.Position += new Vector3(0, 1, 0);
            Log.Info(args.Attacker == null);
            if (args.Attacker == null)
                return;
            if (PlayerCustomRole.TryGetValue(args.Attacker.UserId, out var role))
            {
                Log.Info("1");
                if (role.Advanced.DeadBy.IsConfigurated)
                {
                    Log.Info("2");
                    if (role.Advanced.DeadBy.RoleAfterKilled != PlayerRoles.RoleTypeId.None)
                    {
                        Log.Info("3");
                        args.Player.Role.Set(role.Advanced.DeadBy.RoleAfterKilled, PlayerRoles.RoleSpawnFlags.None);
                    }
                    else
                    {
                        Log.Info("4");
                        if (!string.IsNullOrEmpty(role.Advanced.DeadBy.RoleNameToRespawnAs))
                        {
                            Log.Info("4.1");
                            var customRoleInfo = AfterDeathRoles.Where(x=>x.RoleName == role.Advanced.DeadBy.RoleNameToRespawnAs).FirstOrDefault();
                            if (customRoleInfo == null)
                                return;
                            Log.Info("5");
                            SetCustomInfoToPlayer(args.Player, customRoleInfo);
                        }
                        else if (role.Advanced.DeadBy.RoleNameRandom.Count != 0)
                        {
                            Log.Info("4.2");
                            var customRoleInfo = AfterDeathRoles.Where(x => x.RoleName == role.Advanced.DeadBy.RoleNameRandom.RandomItem()).FirstOrDefault();
                            if (customRoleInfo == null)
                                return;
                            Log.Info("6");
                            SetCustomInfoToPlayer(args.Player, customRoleInfo);
                        }
                    }
                }
            }
        }

        public static void Escaping(EscapingEventArgs args)
        {
            if (PlayerCustomRole.TryGetValue(args.Player.UserId, out var role))
            {
                args.IsAllowed = role.Advanced.CanEscape;
                if (role.Advanced.RoleAfterEscape != PlayerRoles.RoleTypeId.None)
                {
                    args.NewRole = role.Advanced.RoleAfterEscape;
                }
            }
        }

        public static void RespawnManager_ServerOnRespawned(SpawnableTeamType spawnableTeamType, List<ReferenceHub> players)
        {
            Log.Info("RespawnManager_ServerOnRespawned");
            if (players.Count == 0)
                return;
            foreach (var item in SpawningRoles)
            {
                if (!item.ReplaceInSpawnWave)
                    continue;
                if (spawnableTeamType != item.SpawnWaveSpecific.Team)
                {
                    Log.Info("item.SpawnWaveSpecific.Team");
                    continue;
                }
                Log.Info(players.Count);
                Log.Info(item.SpawnWaveSpecific.MinimumTeamMemberRequired);
                if (item.SpawnWaveSpecific.MinimumTeamMemberRequired > players.Count)
                {
                    Log.Info("Count > MinimumTeamMemberRequired");
                    continue;
                }

                var referenceHub = players.Where(x => x.roleManager.CurrentRole.RoleTypeId == item.RoleToReplace).GetRandomValue();

                var player = Player.List.Where(x => x.ReferenceHub == referenceHub).FirstOrDefault();
                if (player == null)
                    continue;
                Log.Info("Player choosen: " + player.UserId);
                SetCustomInfoToPlayer(player, item);
            }
        }

        public static void WaitingForPlayers()
        {
            PlayersRolled = new List<CustomRoleInfo>();
            PlayerCustomRole = new Dictionary<string, CustomRoleInfo>();
            SpawningRoles = new List<CustomRoleInfo>();
            AfterDeathRoles = new List<CustomRoleInfo>();
            Main.Instance.RolesLoader.Load();
            Log.Info("Loading custom roles!");
            foreach (var item in Main.Instance.RolesLoader.RoleInfos)
            {
                if (item.UsedAfterDeath)
                {
                    Log.Info($"After Death Role added: " + item.RoleName);
                    AfterDeathRoles.Add(item);
                    continue;
                }
                for (int i = 0; i < item.SpawnAmount; i++)
                {
                    bool IsSpawning = false;
                    var random = RandomGenerator.GetInt16(1, 100, true);
                    if (random <= item.SpawnChance)
                    {
                        IsSpawning = true;
                        PlayersRolled.Add(item);
                    }
                    Log.Info($"Rolled chance: {random} for Role {item.RoleName}. Role is " + (IsSpawning ? "" : "NOT ")  + "spawning.");
                }
            }
        }

        public static void RoundStarted()
        {
            foreach (var item in PlayersRolled)
            {
                if (item.ReplaceInSpawnWave)
                {
                    SpawningRoles.Add(item);
                    continue;
                }
                Player player = null;
                if (item.RoleToReplace == PlayerRoles.RoleTypeId.None && item.ReplaceFromTeam != PlayerRoles.Team.Dead)
                {
                    player = Player.List.Where(x => x.Role.Team == item.ReplaceFromTeam).GetRandomValue();
                }
                else
                {
                    player = Player.List.Where(x => x.Role == item.RoleToReplace).GetRandomValue();
                }
                if (player == null)
                    continue;
                Log.Info("Player Selected to spawn: " + player.UserId);
                SetCustomInfoToPlayer(player, item);
            }
        }


        public static void SetCustomInfoToPlayer(Player player , CustomRoleInfo customRoleInfo)
        {
            if (PlayerCustomRole.ContainsKey(player.UserId))
            {
                return;
            }
            if (customRoleInfo.Location.UseDefault)
            {
                player.Role.Set(customRoleInfo.RoleToSpawnAs, PlayerRoles.RoleSpawnFlags.UseSpawnpoint);
            }
            else
            {
                player.Role.Set(customRoleInfo.RoleToSpawnAs, PlayerRoles.RoleSpawnFlags.None);
                switch (customRoleInfo.Location.LocationSpawnPriority)
                {
                    case LocationSpawnPriority.None:
                        break;
                    case LocationSpawnPriority.SpawnZone:
                        if (customRoleInfo.Location.SpawnZones.Count > 0)
                        {
                            var tp = Room.List.Where(x => customRoleInfo.Location.SpawnZones.Contains(x.Zone)).GetRandomValue();
                            player.Position = tp.AdjustRoomPosition();
                        }
                        break;
                    case LocationSpawnPriority.SpawnRoom:
                        if (customRoleInfo.Location.SpawnRooms.Count > 0)
                        {
                            var tp = Room.List.Where(x => customRoleInfo.Location.SpawnRooms.Contains(x.Type)).GetRandomValue();
                            player.Position = tp.AdjustRoomPosition();
                        }
                        break;
                    case LocationSpawnPriority.ExactPosition:
                        if (customRoleInfo.Location.ExactPosition.ConvertFromV3() != new V3(0, 0, 0).ConvertFromV3())
                        {
                            player.Position = customRoleInfo.Location.ExactPosition.ConvertFromV3();
                        }
                        break;
                    case LocationSpawnPriority.FullRandom:
                        if (customRoleInfo.Location.SpawnRooms.Count > 0)
                        {
                            var tp = Room.List.GetRandomValue();
                            player.Position = tp.AdjustRoomPosition();
                        }
                        break;
                    default:
                        break;
                }
            }
            player.ClearInventory(true);
            player.ResetInventory(customRoleInfo.InventoryItems);
            player.ClearAmmo();
            foreach (var ammo in customRoleInfo.Ammos)
            {
                player.SetAmmo(ammo.Key, ammo.Value);
            }
            player.Health += customRoleInfo.HealthModifiers.Health;
            player.MaxHealth += customRoleInfo.HealthModifiers.Health;
            if (player.IsScp)
            {
                player.HumeShield += customRoleInfo.HealthModifiers.HumeShield;
            }
            if (player.IsHuman)
            {
                player.ArtificialHealth += customRoleInfo.HealthModifiers.Ahp;
                player.MaxArtificialHealth += customRoleInfo.HealthModifiers.Ahp;
            }
            foreach (var effect in customRoleInfo.Effects)
            {
                player.EnableEffect(effect.EffectType, effect.Intensity, effect.Duration);

            }
            if (Main.Instance.Config.Debug)
            {
                Log.Info(customRoleInfo.Advanced.Scale.ConvertFromV3());
                Log.Info(new V3(0, 0, 0).ConvertFromV3());
                Log.Info(customRoleInfo.Advanced.Scale.ConvertFromV3() != new V3(0, 0, 0).ConvertFromV3());
            }
            if (customRoleInfo.Advanced.Scale.ConvertFromV3() != new V3(0, 0, 0).ConvertFromV3())
            {
                if (Main.Instance.Config.Debug)
                {
                    Log.Info("Scale Before: " + player.Scale);
                }
                Timing.CallDelayed(2.5f, () => 
                {
                    player.Scale = customRoleInfo.Advanced.Scale.ConvertFromV3();
                    if (Main.Instance.Config.Debug)
                    {
                        Log.Info("Scale After: " + player.Scale);
                    }
                });
            }

            if (customRoleInfo.Hint.SpawnBroadcast != string.Empty)
            {
                Exiled.API.Features.Broadcast broadcast = new Exiled.API.Features.Broadcast(customRoleInfo.Hint.SpawnBroadcast, customRoleInfo.Hint.SpawnBroadcastDuration);
                player.Broadcast(broadcast);
            }

            if (customRoleInfo.Hint.SpawnHint != string.Empty)
            {
                player.ShowHint(customRoleInfo.Hint.SpawnHint, customRoleInfo.Hint.SpawnHintDuration);
            }
            if (customRoleInfo.Advanced.RoleAppearance != PlayerRoles.RoleTypeId.None)
            {
                player.ChangeAppearance(customRoleInfo.Advanced.RoleAppearance);
            }

            player.IsBypassModeEnabled = customRoleInfo.Advanced.BypassEnabled;

            PlayerCustomRole.Add(player.UserId, customRoleInfo);
        }
    }
}
