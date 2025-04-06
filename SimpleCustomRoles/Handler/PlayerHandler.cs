using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Wrappers;
using MEC;
using Respawning.Waves;
using SimpleCustomRoles.Helpers;
using SimpleCustomRoles.RoleInfo;
using UnityEngine;
using static SimpleCustomRoles.Helpers.DamageHelper;

namespace SimpleCustomRoles.Handler;

public class PlayerHandler : CustomEventsHandler
{
    public override void OnPlayerChangingRole(PlayerChangingRoleEventArgs ev)
    {
        CustomRoleHelpers.UnSetCustomInfoToPlayer(ev.Player);
    }

    public override void OnPlayerDroppingItem(PlayerDroppingItemEventArgs ev)
    {
        if (!CustomRoleHelpers.TryGetCustomRole(ev.Player, out var role))
            return;
        if (!role.Advanced.CannotDropItems.Contains(ev.Item.Type))
            return;
        ev.IsAllowed = false;
    }

    public override void OnPlayerHurting(PlayerHurtingEventArgs ev)
    {
        if (ev.Target == null)
            return;
        if (!CustomRoleHelpers.TryGetCustomRole(ev.Target, out var role))
            return;
        CustomRoleInfo attackerRole = null;
        float Damage = GetDamageValue(ev.DamageHandler);
        var damageType = GetDamageType(ev.DamageHandler);
        if (ev.Player != null && ev.Player != ev.Target)
        {
            if (!CustomRoleHelpers.TryGetCustomRole(ev.Player, out attackerRole))
                return;
            if (!attackerRole.Advanced.Damager.DamageSent.Any(x => x.Key.DamageType == damageType))
                return;
            Damage = CalculateDamage(attackerRole.Advanced.Damager.DamageSent, ev.DamageHandler, Damage, damageType);
        }
        if (role.Advanced.Damager.DamageReceived.Any(x => x.Key.DamageType == damageType))
            Damage = CalculateDamage(role.Advanced.Damager.DamageReceived, ev.DamageHandler, Damage, damageType);
        if (attackerRole != null)
            CommandHelper.RunCommand(attackerRole.Events.OnDealDamage, $"{ev.Player.PlayerId} {ev.Target.PlayerId} {damageType} {Damage}");
        CommandHelper.RunCommand(role.Events.OnReceiveDamage, $"{ev.Target.PlayerId} {(ev.Player != null ? ev.Player.PlayerId : 0)} {damageType} {Damage}");
        SetDamageValue(ev.DamageHandler, Damage);
    }


    public override void OnPlayerChangedSpectator(PlayerChangedSpectatorEventArgs ev)
    {
        if (ev.OldTarget == null && ev.NewTarget == null)
            return;
        if (ev.OldTarget != null && CustomRoleHelpers.Contains(ev.OldTarget))
        {
            // todo dont clear, just queue
            ev.Player.ClearBroadcasts();
        }
        if (ev.NewTarget != null && CustomRoleHelpers.TryGetCustomRole(ev.NewTarget, out var role))
        {
            if (!role.CanDisplay)
                return;
            Server.SendBroadcast(ev.Player, $"\nThis user has a special role: <color={role.DisplayColor}>{role.DisplayRolename}</color>", Main.Instance.Config.SpectatorBroadcastTime);
        }
    }

    public override void OnPlayerUsingItem(PlayerUsingItemEventArgs ev)
    {
        if (!CustomRoleHelpers.TryGetCustomRole(ev.Player, out var role))
            return;
        if (role.Advanced.DeniedUsingItems.Contains(ev.Item.Type))
        {
            ev.IsAllowed = false;
            return;
        }
        if (ev.Item.Type != ItemType.SCP500)
            return;
        Timing.CallDelayed(3f, () =>
        {
            foreach (var effect in role.Effects)
            {
                if (effect.Removable)
                    continue;
                ev.Player.EnableEffect(EffectHelper.GetEffectFromName(ev.Player, effect.EffectName), effect.Intensity, effect.Duration);
                CL.Debug($"(Used 500) Effect {effect.EffectName}", Main.Instance.Config.Debug);
            }
        });
    }

    public override void OnPlayerDeath(PlayerDeathEventArgs ev)
    {
        if (CustomRoleHelpers.TryGetCustomRole(ev.Player, out var died_playerRole))
            CommandHelper.RunCommand(died_playerRole.Events.OnDied, $"{ev.Player.PlayerId} {(ev.Attacker != null ? ev.Attacker.PlayerId : 0)} {GetDamageType(ev.DamageHandler)} {GetDamageValue(ev.DamageHandler)}");
        CustomRoleHelpers.UnSetCustomInfoToPlayer(ev.Player);
        if (ev.Attacker == null)
            return;
        if (!CustomRoleHelpers.TryGetCustomRole(ev.Attacker, out var role))
            return;
        CommandHelper.RunCommand(role.Events.OnKill, $"{ev.Player.PlayerId} {(ev.Attacker != null ? ev.Attacker.PlayerId : 0)} {GetDamageType(ev.DamageHandler)} {GetDamageValue(ev.DamageHandler)}");
        if (!role.Advanced.DeadBy.IsConfigurated)
            return;
        if (role.Advanced.DeadBy.AfterDeath != PlayerRoles.RoleTypeId.None)
            ev.Player.SetRole(role.Advanced.DeadBy.AfterDeath, PlayerRoles.RoleChangeReason.RemoteAdmin, PlayerRoles.RoleSpawnFlags.None);
        else
        {
            CustomRoleInfo customRoleInfo = null;
            if (!string.IsNullOrEmpty(role.Advanced.DeadBy.RoleName))
                customRoleInfo = Main.Instance.AfterDeathRoles.Where(x => x.Rolename == role.Advanced.DeadBy.RoleName).FirstOrDefault();
            else if (role.Advanced.DeadBy.Random.Count != 0)
                customRoleInfo = Main.Instance.AfterDeathRoles.Where(x => x.Rolename == role.Advanced.DeadBy.Random.RandomItem()).FirstOrDefault();
            if (customRoleInfo == null)
                return;
            CustomRoleHelpers.SetCustomInfoToPlayer(ev.Player, customRoleInfo);
        }
    }

    public override void OnPlayerDying(PlayerDyingEventArgs ev)
    {
        if (!CustomRoleHelpers.TryGetCustomRole(ev.Player, out var dying_playerRole))
            return;
        var damageType = GetDamageType(ev.DamageHandler);
        Server.RunCommand($"{dying_playerRole.Events.OnDying} {ev.Player.PlayerId} {(ev.Attacker != null ? ev.Attacker.PlayerId : 0)} {damageType}");
    }

    public override void OnPlayerEscaping(PlayerEscapingEventArgs ev)
    {
        if (!CustomRoleHelpers.TryGetCustomRole(ev.Player, out var role))
            return;
        
        if (!role.Advanced.Escaping.CanEscape)
        {
            ev.IsAllowed = role.Advanced.Escaping.CanEscape;
            return;
        }
        CustomRoleHelpers.UnSetCustomInfoToPlayer(ev.Player);
        if (role.Advanced.Escaping.RoleAfterEscape.TryGetValue(ev.EscapeScenario, out var roleTypeId) && roleTypeId != PlayerRoles.RoleTypeId.None)
            ev.NewRole = roleTypeId;
        if (role.Advanced.Escaping.RoleNameAfterEscape.TryGetValue(ev.EscapeScenario, out var rolename) && !string.IsNullOrEmpty(rolename))
        {
            Timing.CallDelayed(2.5f, () =>
            {
                var escapeRole = Main.Instance.EscapeRoles.FirstOrDefault(x => x.Rolename == rolename);
                if (escapeRole == default)
                    return;
                CustomRoleHelpers.SetCustomInfoToPlayer(ev.Player, escapeRole);
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
            CustomRoleHelpers.UnSetCustomInfoToPlayer(Player.Get(item));
        }

        foreach (var item in Main.Instance.InWaveRoles.Where(x=>x.SpawnWave.Faction == wave.TargetFaction && x.RoleType == CustomRoleType.InWave))
        {

            if (item.SpawnWave.SkipCheck || item.SpawnWave.MinRequired > players.Count)
                continue;

            var referenceHub = players.Where(x => x.roleManager.CurrentRole.RoleTypeId == item.ReplaceRole).ToList().RandomItem();
            CustomRoleHelpers.SetCustomInfoToPlayer(Player.Get(referenceHub), item);
            tmp.Add(item);
        }

        foreach (var item in tmp)
        {
            Main.Instance.InWaveRoles.Remove(item);
        }

        foreach (var item in CustomDataStoreManagerExtended.GetPlayers<CustomRoleInfoStorage>())
        {
            if (!CustomRoleHelpers.TryGetCustomRole(item, out var role))
                continue;
            CommandHelper.RunCommand(role.Events.OnSpawnWave, $"{item.PlayerId} {role.Rolename} {wave.TargetFaction} {players.Count}");
        }
    }
}
