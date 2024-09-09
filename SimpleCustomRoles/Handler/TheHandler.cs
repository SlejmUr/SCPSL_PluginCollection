using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Item;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.Features;
using MEC;
using Respawning;
using SimpleCustomRoles.RoleInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SimpleCustomRoles.Handler
{
    public class TheHandler
    {
        public static void ChangingRole(ChangingRoleEventArgs args)
        {
            if (Main.Instance.PlayerCustomRole.ContainsKey(args.Player.UserId))
            {
                Main.Instance.PlayerCustomRole.Remove(args.Player.UserId);
            }
        }

        public static void Hurting(HurtingEventArgs args)
        {
            if (args.Player == null)
                return;

            CustomRoleInfo attacker_role = null;
            if (args.Attacker != null)
            {
                if (Main.Instance.PlayerCustomRole.TryGetValue(args.Attacker.UserId, out attacker_role))
                {
                    if (attacker_role.Advanced.Damager.DamageSentDict.TryGetValue(args.DamageHandler.Type, out var valueSetter))
                    {
                        args.Amount = RoleSetter.MathWithFloat(valueSetter.SetType, args.Amount, valueSetter.Value);
                    }
                }
            }

            if (!Main.Instance.PlayerCustomRole.TryGetValue(args.Player.UserId, out var role))
                return;

            if (role.Advanced.Damager.DamageReceivedDict.TryGetValue(args.DamageHandler.Type, out var dmg))
            {
                args.Amount = RoleSetter.MathWithFloat(dmg.SetType, args.Amount, dmg.Value);
            }

            if (attacker_role != null && !string.IsNullOrEmpty(attacker_role.EventCaller.OnDealDamage))
            {
                // Call event
                Server.ExecuteCommand($"{attacker_role.EventCaller.OnDealDamage} {args.Attacker.Id} {args.Player.Id}  {args.DamageHandler.Type.ToString()} {args.Amount}");
            }

            if (!string.IsNullOrEmpty(role.EventCaller.OnReceiveDamage))
            {
                int attackerID = 0;
                if (args.Attacker != null)
                    attackerID = args.Attacker.Id;
                // Call event
                Server.ExecuteCommand($"{role.EventCaller.OnReceiveDamage} {args.Player.Id} {attackerID} {args.DamageHandler.Type.ToString()} {args.Amount}");
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
            if (args.NewTarget != null && Main.Instance.PlayerCustomRole.TryGetValue(args.NewTarget.UserId, out var role))
            {
                if (!role.RoleCanDisplay)
                    return;
                Exiled.API.Features.Broadcast broadcast = new Exiled.API.Features.Broadcast($"\nThis user has a special role: <color={role.RoleDisplayColorHex}>{role.DisplayRoleName}</color>", Main.Instance.Config.SpectatorBroadcastTime);
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
            if (!Main.Instance.PlayerCustomRole.TryGetValue(args.Player.UserId, out var role))
                return;
            if (role.Inventory.CannotDropItems.Contains(args.Item.Type))
            {
                args.IsAllowed = false;
                return;
            }
        }

        public static void UsingItem(UsingItemEventArgs args)
        {
            if (!Main.Instance.PlayerCustomRole.TryGetValue(args.Player.UserId, out var role))
                return;
            if (role.Inventory.DeniedUsingItems.Contains(args.Item.Type))
            {
                args.IsAllowed = false;
                return;
            }

            Timing.CallDelayed(3f, () =>
            {
                foreach (var effect in role.Effects)
                {
                    if (!effect.CanRemovedWithSCP500)
                    {
                        Log.Debug($"(Used 500) Effect {effect.EffectType.ToString()}: IsSet? " + args.Player.EnableEffect(effect.EffectType, effect.Intensity, effect.Duration));
                    }
                }
            });
        }

        public static void Died(DiedEventArgs args)
        {
            if (Main.Instance.PlayerCustomRole.TryGetValue(args.Player.UserId, out var died_player_role))
            {
                if (!string.IsNullOrEmpty(died_player_role.EventCaller.OnDied))
                {
                    int attackerID = 0;
                    if (args.Attacker != null)
                        attackerID = args.Attacker.Id;
                    // Call event
                    Server.ExecuteCommand($"{died_player_role.EventCaller.OnDied} {args.Player.Id} {attackerID} {args.DamageHandler.Type.ToString()}");
                }
            }
            Main.Instance.PlayerCustomRole.Remove(args.Player.UserId);
            args.Player.Scale = new Vector3(1, 1, 1);
            args.Player.Position += new Vector3(0, 1, 0);
            if (args.Attacker == null)
                return;
            if (Main.Instance.PlayerCustomRole.TryGetValue(args.Attacker.UserId, out var role))
            {
                if (!string.IsNullOrEmpty(role.EventCaller.OnKill))
                {
                    // Call event
                    Server.ExecuteCommand($"{role.EventCaller.OnKill} {args.Attacker.Id} {args.Player.Id} {args.DamageHandler.Type.ToString()}");
                }
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
            if (!Main.Instance.PlayerCustomRole.TryGetValue(args.Player.UserId, out var role))
                return;
            args.IsAllowed = role.Advanced.Escaping.CanEscape;
            if (!args.IsAllowed)
            {
                Log.Info("escape not allowed");
                return;
            }
            Log.Info("escape now as a role: " + args.NewRole);
            Main.Instance.PlayerCustomRole.Remove(args.Player.UserId);
            if (role.Advanced.Escaping.RoleAfterEscape.TryGetValue(args.EscapeScenario, out var roleTypeId) && roleTypeId != PlayerRoles.RoleTypeId.None)
            {
                args.NewRole = roleTypeId;
            }
            if (role.Advanced.Escaping.RoleNameAfterEscape.TryGetValue(args.EscapeScenario, out var rolename) && !string.IsNullOrEmpty(rolename))
            {
                Timing.CallDelayed(2.5f, () =>
                {
                    var escapeRole = Main.Instance.EscapeRoles.Where(x => x.RoleName == rolename).FirstOrDefault();
                    RoleSetter.SetCustomInfoToPlayer(args.Player, escapeRole);
                });
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

            foreach (var item in Main.Instance.InWaveRoles.Where(x=>x.SpawnWaveSpecific.Team == spawnableTeamType && x.RoleType == CustomRoleType.InWave))
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
                Main.Instance.InWaveRoles.Remove(item);
            }

            foreach (var item in Main.Instance.PlayerCustomRole)
            {
                var player = Player.List.Where(x=>x.RawUserId == item.Key).FirstOrDefault();
                if (!string.IsNullOrEmpty(item.Value.EventCaller.OnSpawnWave))
                {
                    // Call event
                    Server.ExecuteCommand($"{item.Value.EventCaller.OnSpawnWave} {player.Id} {item.Value.RoleName} {spawnableTeamType.ToString()} {players.Count}");
                }
            }
        }

        public static void ReloadRoles()
        {
            Main.Instance.RegularRoles = new List<CustomRoleInfo>();
            Main.Instance.PlayerCustomRole = new Dictionary<string, CustomRoleInfo>();
            Main.Instance.InWaveRoles = new List<CustomRoleInfo>();
            Main.Instance.AfterDeathRoles = new List<CustomRoleInfo>();
            Main.Instance.SPC_SpecificRoles = new List<CustomRoleInfo>();
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
            Main.Instance.PlayerCustomRole = new Dictionary<string, CustomRoleInfo>();
            Main.Instance.RegularRoles = new List<CustomRoleInfo>();
            Main.Instance.InWaveRoles = new List<CustomRoleInfo>();
            Main.Instance.AfterDeathRoles = new List<CustomRoleInfo>();
            Main.Instance.SPC_SpecificRoles = new List<CustomRoleInfo>();
            Main.Instance.EscapeRoles = new List<CustomRoleInfo>();
            Main.Instance.RolesLoader.Load();
            if (Main.Instance.Config.Debug)
                Log.Info("Loading custom roles!");
            foreach (var item in Main.Instance.RolesLoader.RoleInfos)
            {
                if (item.RoleType == CustomRoleType.AfterDead)
                {
                    if (Main.Instance.Config.Debug)
                        Log.Info($"After Death Role added: " + item.RoleName);
                    Main.Instance.AfterDeathRoles.Add(item);
                    continue;
                }
                if (item.RoleType == CustomRoleType.Escape)
                {
                    if (Main.Instance.Config.Debug)
                        Log.Info($"Escape Role added: " + item.RoleName);
                    Main.Instance.EscapeRoles.Add(item);
                    continue;
                }
                for (int i = 0; i < item.SpawnAmount; i++)
                {
                    bool IsSpawning = false;
                    var random = RandomGenerator.GetInt16(1, 10000, true);
                    if (random <= item.SpawnChance)
                    {
                        IsSpawning = true;
                        if (item.RoleType == CustomRoleType.SPC_Specific)
                            Main.Instance.SPC_SpecificRoles.Add(item);
                        else
                            Main.Instance.RegularRoles.Add(item);
                    }
                    if (Main.Instance.Config.Debug)
                        Log.Info($"Rolled chance: {random}/{item.SpawnChance} for Role {item.RoleName}. Role is " + (IsSpawning ? "" : "NOT ")  + "spawning.");
                }
            }
            Log.Info("Loading custom roles finished!");
        }

        public static void RoundStarted()
        {
            if (Main.Instance.Config.IsPaused)
                return;
            foreach (var item in Main.Instance.RegularRoles)
            {
                if (item.RoleType == CustomRoleType.InWave)
                {
                    Main.Instance.InWaveRoles.Add(item);
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
            Main.Instance.RegularRoles.Clear();
        }


    }
}
