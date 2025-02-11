using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Features.Wrappers;
using MEC;
using Respawning.Waves;
using SimpleCustomRoles.Helpers;
using SimpleCustomRoles.RoleInfo;
using UnityEngine;

namespace SimpleCustomRoles.Handler;

public class TheHandler
{
    public static void ChangingRole(PlayerChangingRoleEventArgs args)
    {
        if (!Main.Instance.PlayerCustomRole.ContainsKey(args.Player.UserId))
            return;
        Main.Instance.PlayerCustomRole.Remove(args.Player.UserId);
    }

    public static void Hurting(PlayerHurtingEventArgs args)
    {
        if (args.Target == null)
            return;

        if (!Main.Instance.PlayerCustomRole.TryGetValue(args.Target.UserId, out var role))
            return;
        CustomRoleInfo attacker_role = null;
        float Damage = DamageHelper.GetDamageValue(args.DamageHandler);
        var damageType = DamageHelper.GetDamageType(args.DamageHandler);

        if (args.Player != null && args.Player != args.Target)
        {
            if (Main.Instance.PlayerCustomRole.TryGetValue(args.Player.UserId, out attacker_role))
            {
                if (!attacker_role.Advanced.Damager.DamageSentDict.Any(x => x.Key.DamageType == damageType))
                    return;
                var available_subTypes = attacker_role.Advanced.Damager.DamageSentDict.Where(x => x.Key.DamageType == damageType).Select(x => x.Key.DamageSubType);
                foreach (var item in available_subTypes)
                {
                    var obj = DamageHelper.GetObjectBySubType(args.DamageHandler, item);
                    if (obj == null)
                        continue;
                    var sent = attacker_role.Advanced.Damager.DamageSentDict.Where(x => x.Key.DamageType == damageType && x.Key.DamageSubType == item).Where(x => x.Key.subType.ToString() == obj.ToString());
                    if (sent.Any())
                    {
                        var first = sent.First();
                        if (first.Value == null)
                            continue;
                        Damage = RoleSetter.MathWithFloat(first.Value.SetType, Damage, first.Value.Value);
                    }
                }
            }
        }
        if (role.Advanced.Damager.DamageReceivedDict.Any(x => x.Key.DamageType == damageType))
        {
            var available_subTypes = role.Advanced.Damager.DamageReceivedDict.Where(x => x.Key.DamageType == damageType).Select(x => x.Key.DamageSubType);
            foreach (var item in available_subTypes)
            {
                var obj = DamageHelper.GetObjectBySubType(args.DamageHandler, item);
                if (obj == null)
                    continue;
                var sent = role.Advanced.Damager.DamageReceivedDict.Where(x => x.Key.DamageType == damageType && x.Key.DamageSubType == item).Where(x => x.Key.subType.ToString() == obj.ToString());
                if (sent.Any())
                {
                    var first = sent.First();
                    if (first.Value == null)
                        continue;
                    Damage = RoleSetter.MathWithFloat(first.Value.SetType, Damage, first.Value.Value);
                }

            }
        }
        if (attacker_role != null && !string.IsNullOrEmpty(attacker_role.EventCaller.OnDealDamage))
        {
            // Call event
            Server.RunCommand($"{attacker_role.EventCaller.OnDealDamage} {args.Player.PlayerId} {args.Target.PlayerId} {damageType} {Damage}");
        }
        if (!string.IsNullOrEmpty(role.EventCaller.OnReceiveDamage))
        {
            int attackerID = 0;
            if (args.Player != null)
                attackerID = args.Player.PlayerId;
            // Call event
            Server.RunCommand($"{role.EventCaller.OnReceiveDamage} {args.Target.PlayerId} {attackerID} {damageType} {Damage}");
        }
        DamageHelper.SetDamageValue(args.DamageHandler, Damage);
    }

    public static void ChangingSpectatedPlayer(PlayerChangedSpectatorEventArgs args)
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
            Server.SendBroadcast(args.Player, $"\nThis user has a special role: <color={role.RoleDisplayColorHex}>{role.DisplayRoleName}</color>", Main.Instance.Config.SpectatorBroadcastTime);
        }
    }

    public static void DroppingItem(PlayerDroppingItemEventArgs args)
    {
        if (!Main.Instance.PlayerCustomRole.TryGetValue(args.Player.UserId, out var role))
            return;
        if (!role.Inventory.CannotDropItems.Contains(args.Item.Type))
            return;
        args.IsAllowed = false;
    }

    public static void UsingItem(PlayerUsingItemEventArgs args)
    {
        if (!Main.Instance.PlayerCustomRole.TryGetValue(args.Player.UserId, out var role))
            return;
        if (role.Inventory.DeniedUsingItems.Contains(args.Item.Type))
        {
            args.IsAllowed = false;
            return;
        }
        if (args.Item.Type == ItemType.SCP500)
        {
            Timing.CallDelayed(3f, () =>
            {
                foreach (var effect in role.Effects)
                {
                    if (!effect.CanRemovedWithSCP500)
                    {
                        args.Player.EnableEffect(EffectHelper.GetEffectFromName(args.Player, effect.EffectTypeName), effect.Intensity, effect.Duration);
                        CL.Debug($"(Used 500) Effect {effect.EffectTypeName}");
                    }
                }
            });
        }
    }

    public static void Died(PlayerDeathEventArgs args)
    {
        float Damage = DamageHelper.GetDamageValue(args.DamageHandler);
        var damageType = DamageHelper.GetDamageType(args.DamageHandler);
        if (Main.Instance.PlayerCustomRole.TryGetValue(args.Player.UserId, out var died_player_role))
        {
            if (!string.IsNullOrEmpty(died_player_role.EventCaller.OnDied))
            {
                int attackerID = 0;
                if (args.Attacker != null)
                    attackerID = args.Attacker.PlayerId;
                // Call event
                Server.RunCommand($"{died_player_role.EventCaller.OnDied} {args.Player.PlayerId} {attackerID} {damageType}");
            }
        }
        Main.Instance.PlayerCustomRole.Remove(args.Player.UserId);
        ScaleHelper.SetScale(args.Player, new(1,1,1));
        args.Player.Position += new Vector3(0, 1, 0);
        if (args.Attacker == null)
            return;
        if (!Main.Instance.PlayerCustomRole.TryGetValue(args.Attacker.UserId, out var role))
            return;
        if (!string.IsNullOrEmpty(role.EventCaller.OnKill))
        {
            // Call event
            Server.RunCommand($"{role.EventCaller.OnKill} {args.Attacker.PlayerId} {args.Player.PlayerId} {DamageHelper.GetDamageType(args.DamageHandler)}");
        }
        if (!role.Advanced.DeadBy.IsConfigurated)
            return;
        if (role.Advanced.DeadBy.RoleAfterKilled != PlayerRoles.RoleTypeId.None)
        {
            args.Player.SetRole(role.Advanced.DeadBy.RoleAfterKilled, PlayerRoles.RoleChangeReason.RemoteAdmin, PlayerRoles.RoleSpawnFlags.None);
        }
        else
        {
            if (!string.IsNullOrEmpty(role.Advanced.DeadBy.RoleNameToRespawnAs))
            {
                var customRoleInfo = Main.Instance.AfterDeathRoles.Where(x => x.RoleName == role.Advanced.DeadBy.RoleNameToRespawnAs).FirstOrDefault();
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

    public static void Dying(PlayerDyingEventArgs ev)
    {
        var damageType = DamageHelper.GetDamageType(ev.DamageHandler);
        if (!Main.Instance.PlayerCustomRole.TryGetValue(ev.Player.UserId, out var dying_player_role))
            return;
        if (string.IsNullOrEmpty(dying_player_role.EventCaller.OnDying))
            return;
        int attackerID = 0;
        if (ev.Attacker != null)
            attackerID = ev.Attacker.PlayerId;
        // Call event
        Server.RunCommand($"{dying_player_role.EventCaller.OnDying} {ev.Player.PlayerId} {attackerID} {damageType}");
    }

    public static void Escaping(PlayerEscapingEventArgs args)
    {
        if (!Main.Instance.PlayerCustomRole.TryGetValue(args.Player.UserId, out var role))
            return;
        args.IsAllowed = role.Advanced.Escaping.CanEscape;
        if (!args.IsAllowed)
        {
            CL.Info("escape not allowed");
            return;
        }
        CL.Info("escape now as a role: " + args.NewRole);
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

    public static void RespawnManager_ServerOnRespawned(SpawnableWaveBase wave, List<ReferenceHub> players)
    {
        if (players.Count == 0)
            return;
        List<CustomRoleInfo> tmp = [];

        foreach (var item in players)
        {
            var player = Player.List.Where(x => x.ReferenceHub == item).FirstOrDefault();
            if (player == null)
                continue;
            ScaleHelper.SetScale(player, new Vector3(1, 1, 1));

            //remove again if we already have one.
            Main.Instance.PlayerCustomRole.Remove(player.UserId);
        }

        foreach (var item in Main.Instance.InWaveRoles.Where(x=>x.SpawnWaveSpecific.Faction == wave.TargetFaction && x.RoleType == CustomRoleType.InWave))
        {

            if (!item.SpawnWaveSpecific.SkipMinimumCheck)
            {
                if (item.SpawnWaveSpecific.MinimumTeamMemberRequired > players.Count)
                {
                    continue;
                }
            }

            var referenceHub = players.Where(x => x.roleManager.CurrentRole.RoleTypeId == item.RoleToReplace).ToList().RandomItem();

            var player = Player.List.Where(x => x.ReferenceHub == referenceHub).FirstOrDefault();
            if (player == null)
                continue;
            if (Main.Instance.Config.Debug)
                CL.Info("Player choosen: " + player.UserId);
            
            RoleSetter.SetCustomInfoToPlayer(player, item);
            tmp.Add(item);
        }

        foreach (var item in tmp)
        {
            Main.Instance.InWaveRoles.Remove(item);
        }

        foreach (var item in Main.Instance.PlayerCustomRole)
        {
            var player = Player.List.Where(x=>x.UserId == item.Key).FirstOrDefault();
            if (!string.IsNullOrEmpty(item.Value.EventCaller.OnSpawnWave))
            {
                // Call event
                Server.RunCommand($"{item.Value.EventCaller.OnSpawnWave} {player.PlayerId} {item.Value.RoleName} {wave.TargetFaction} {players.Count}");
            }
        }
    }
}
