using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Item;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.Features;
using MEC;
using Respawning;
using SimpleCustomRoles.RoleInfo;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SimpleCustomRoles.Handler
{
    public class TheHandler
    {
        public static void Hurting(HurtingEventArgs args)
        {
            if (args.Player == null)
                return;

            //  this can happen tbh, grenade for example.
            /*
            if (args.Attacker == args.Player)
                return;
            */
            if (!Main.Instance.PlayerCustomRole.ContainsKey(args.Player.UserId))
                return;

            bool toret = Main.Instance.PlayerCustomRole.TryGetValue(args.Player.UserId, out var role);
            if (!toret)
                return;

            toret = role.Advanced.Damager.DamageDict.TryGetValue(args.DamageHandler.Type, out var dmg);
            if (!toret)
                return;

            if (dmg.IsSet)
            {
                args.Amount = dmg.Damage;
            }
            else if (dmg.IsAddition)
            {
                args.Amount += dmg.Damage;
            }
        }

        public static void ChangingSpectatedPlayer(ChangingSpectatedPlayerEventArgs args)
        {
            if (args.OldTarget == null && args.NewTarget == null)
                return;
            if (args.OldTarget != null && Main.Instance.PlayerCustomRole.ContainsKey(args.OldTarget.UserId))
            {
                args.Player.ClearBroadcasts();
            }
            if (args.NewTarget.UserId != null && Main.Instance.PlayerCustomRole.TryGetValue(args.NewTarget.UserId, out var role))
            {
                Exiled.API.Features.Broadcast broadcast = new Exiled.API.Features.Broadcast($"\nThis user has a special role: <color={role.Advanced.ColorHex}>{role.DisplayRoleName}</color>", Main.Instance.Config.SpectatorBroadcastTime);
                args.Player.Broadcast(broadcast, false);
            }
        }

        public static void ChargingJailbird(ChargingJailbirdEventArgs args)
        {
            if (Main.Instance.PlayerCustomRole.TryGetValue(args.Player.UserId, out var role))
            {
                args.IsAllowed = role.Advanced.CanChargeJailBird;
            }
        }

        public static void DroppingItem(DroppingItemEventArgs args)
        {
            if (Main.Instance.PlayerCustomRole.TryGetValue(args.Player.UserId, out var role))
            {
                if (role.CannotDropItems.Contains(args.Item.Type))
                {
                    args.IsAllowed = false;
                    return;
                }

            }
        }

        public static void UsingItem(UsingItemEventArgs args)
        {
            if (Main.Instance.PlayerCustomRole.TryGetValue(args.Player.UserId, out var role))
            {
                if (role.DeniedUsingItems.Contains(args.Item.Type))
                {
                    args.IsAllowed = false;
                    return;
                }

                Timing.CallDelayed(3f, () => 
                {
                    foreach (var effect in role.Effects)
                    {
                        if (!effect.CanRemovedwithSCP500)
                        {
                            Log.Debug($"(Used 500) Effect {effect.EffectType.ToString()}: IsSet? " + args.Player.EnableEffect(effect.EffectType, effect.Intensity, effect.Duration));
                        }
                    }
                });
            }          
        }

        public static void Died(DiedEventArgs args)
        {
            Main.Instance.PlayerCustomRole.Remove(args.Player.UserId);
            args.Player.Scale = new Vector3(1, 1, 1);
            args.Player.Position += new Vector3(0, 1, 0);
            if (args.Attacker == null)
                return;
            if (Main.Instance.PlayerCustomRole.TryGetValue(args.Attacker.UserId, out var role))
            {
                if (role.Advanced.DeadBy.IsConfigurated)
                {
                    if (role.Advanced.DeadBy.RoleAfterKilled != PlayerRoles.RoleTypeId.None)
                    {
                        args.Player.Role.Set(role.Advanced.DeadBy.RoleAfterKilled, PlayerRoles.RoleSpawnFlags.None);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(role.Advanced.DeadBy.RoleNameToRespawnAs))
                        {
                            var customRoleInfo = Main.Instance.AfterDeathRoles.Where(x=>x.RoleName == role.Advanced.DeadBy.RoleNameToRespawnAs).FirstOrDefault();
                            if (customRoleInfo == null)
                                return;
                            RoleSetter.SetCustomInfoToPlayer(args.Player, customRoleInfo);
                        }
                        else if (role.Advanced.DeadBy.RoleNameRandom.Count != 0)
                        {
                            var customRoleInfo = Main.Instance.AfterDeathRoles.Where(x => x.RoleName == role.Advanced.DeadBy.RoleNameRandom.RandomItem()).FirstOrDefault();
                            if (customRoleInfo == null)
                                return;
                            RoleSetter.SetCustomInfoToPlayer(args.Player, customRoleInfo);
                        }
                    }
                }
            }
        }

        public static void Escaping(EscapingEventArgs args)
        {
            if (Main.Instance.PlayerCustomRole.TryGetValue(args.Player.UserId, out var role))
            {
                if (role.Advanced.RoleAfterEscape != PlayerRoles.RoleTypeId.None)
                {
                    args.IsAllowed = role.Advanced.CanEscape;
                    args.NewRole = role.Advanced.RoleAfterEscape;
                    Main.Instance.PlayerCustomRole.Remove(args.Player.UserId);
                }
            }
        }

        public static void RespawnManager_ServerOnRespawned(SpawnableTeamType spawnableTeamType, List<ReferenceHub> players)
        {
            if (players.Count == 0)
                return;
            List<CustomRoleInfo> tmp = new List<CustomRoleInfo>();

            foreach (var item in players)
            {
                var player = Player.List.Where(x => x.ReferenceHub == item).FirstOrDefault();
                if (player == null)
                    continue;
                player.Scale = new Vector3(1, 1, 1);

                //remove again if we already have one.
                Main.Instance.PlayerCustomRole.Remove(player.UserId);
            }

            foreach (var item in Main.Instance.SpawningRoles.Where(x=>x.SpawnWaveSpecific.Team == spawnableTeamType && x.ReplaceInSpawnWave))
            {

                if (!item.SpawnWaveSpecific.SkipMinimumCheck)
                {
                    if (item.SpawnWaveSpecific.MinimumTeamMemberRequired > players.Count)
                    {
                        continue;
                    }
                }

                var referenceHub = players.Where(x => x.roleManager.CurrentRole.RoleTypeId == item.RoleToReplace).GetRandomValue();

                var player = Player.List.Where(x => x.ReferenceHub == referenceHub).FirstOrDefault();
                if (player == null)
                    continue;
                if (Main.Instance.Config.Debug)
                    Log.Info("Player choosen: " + player.UserId);
                
                RoleSetter.SetCustomInfoToPlayer(player, item);
                tmp.Add(item);
            }

            foreach (var item in tmp)
            {
                Main.Instance.SpawningRoles.Remove(item);
            }
        }

        public static void ReloadRoles()
        {
            Main.Instance.PlayersRolled = new List<CustomRoleInfo>();
            Main.Instance.PlayerCustomRole = new Dictionary<string, CustomRoleInfo>();
            Main.Instance.SpawningRoles = new List<CustomRoleInfo>();
            Main.Instance.AfterDeathRoles = new List<CustomRoleInfo>();
            Main.Instance.ScpSpecificRoles = new List<CustomRoleInfo>();
            Main.Instance.RolesLoader.Load();
            foreach (var item in Main.Instance.RolesLoader.RoleInfos)
            {
                if (item.UsedAfterDeath)
                {
                    Main.Instance.AfterDeathRoles.Add(item);
                    continue;
                }
                for (int i = 0; i < item.SpawnAmount; i++)
                {
                    if (item.SCP_Specific.SCP_Specific_Role)
                        Main.Instance.ScpSpecificRoles.Add(item);
                    if (item.ReplaceInSpawnWave)
                        Main.Instance.SpawningRoles.Add(item);
                }

            }
        }

        public static void WaitingForPlayers()
        {
            Main.Instance.PlayersRolled = new List<CustomRoleInfo>();
            Main.Instance.PlayerCustomRole = new Dictionary<string, CustomRoleInfo>();
            Main.Instance.SpawningRoles = new List<CustomRoleInfo>();
            Main.Instance.AfterDeathRoles = new List<CustomRoleInfo>();
            Main.Instance.ScpSpecificRoles = new List<CustomRoleInfo>();
            Main.Instance.RolesLoader.Load();
            if (Main.Instance.Config.Debug)
                Log.Info("Loading custom roles!");
            foreach (var item in Main.Instance.RolesLoader.RoleInfos)
            {
                if (item.UsedAfterDeath)
                {
                    if (Main.Instance.Config.Debug)
                        Log.Info($"After Death Role added: " + item.RoleName);
                    Main.Instance.AfterDeathRoles.Add(item);
                    continue;
                }
                for (int i = 0; i < item.SpawnAmount; i++)
                {
                    bool IsSpawning = false;
                    var random = RandomGenerator.GetInt16(1, 10000, true);
                    if (random <= item.SpawnChance)
                    {
                        IsSpawning = true;
                        if (item.SCP_Specific.SCP_Specific_Role)
                            Main.Instance.ScpSpecificRoles.Add(item);
                        else
                            Main.Instance.PlayersRolled.Add(item);
                    }
                    if (Main.Instance.Config.Debug)
                        Log.Info($"Rolled chance: {random}/{item.SpawnChance} for Role {item.RoleName}. Role is " + (IsSpawning ? "" : "NOT ")  + "spawning.");
                }
            }
            Log.Info("Loading custom roles finished!");
        }

        public static void RoundStarted()
        {
            foreach (var item in Main.Instance.PlayersRolled)
            {
                if (item.ReplaceInSpawnWave)
                {
                    Main.Instance.SpawningRoles.Add(item);
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
                if (Main.Instance.Config.Debug)
                    Log.Info("Player Selected to spawn: " + player.UserId);
                RoleSetter.SetCustomInfoToPlayer(player, item);
            }
            Main.Instance.PlayersRolled.Clear();
        }


    }
}
