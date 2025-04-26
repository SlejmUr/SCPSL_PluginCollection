using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Arguments.ServerEvents;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Wrappers;
using MEC;
using SimpleCustomRoles.Helpers;
using SimpleCustomRoles.RoleYaml;
using SimpleCustomRoles.RoleYaml.Enums;

namespace SimpleCustomRoles.Handler;

public class PlayerHandler : CustomEventsHandler
{
    public override void OnPlayerChangingRole(PlayerChangingRoleEventArgs ev)
    {
        CustomRoleHelpers.UnSetCustomInfoToPlayer(ev.Player);
        if (ev.ChangeReason == PlayerRoles.RoleChangeReason.Destroyed)
            return;
        AppearanceSync.ForceSync(ev.Player);
    }

    public override void OnPlayerDroppingItem(PlayerDroppingItemEventArgs ev)
    {
        if (!CustomRoleHelpers.TryGetCustomRole(ev.Player, out var role))
            return;
        if (!role.Deniable.Items.TryGetValue(ev.Item.Type, out var deniable))
            return;
        ev.IsAllowed = deniable.CanDrop;
    }

    public override void OnPlayerHurting(PlayerHurtingEventArgs ev)
    {
        if (ev.Attacker == null)
            return;
        if (!CustomRoleHelpers.TryGetCustomRole(ev.Attacker, out var role))
            return;
        CustomRoleBaseInfo attackerRole = null;
        float Damage = ev.DamageHandler.GetDamageValue();
        DamageType damageType = ev.DamageHandler.GetDamageType();
        if (ev.Player != null && ev.Player != ev.Attacker)
        {
            if (!CustomRoleHelpers.TryGetCustomRole(ev.Player, out attackerRole))
                return;
            if (!attackerRole.Damage.DamageDealt.Any(x => x.Key.DamageType == damageType))
                return;
            Damage = attackerRole.Damage.DamageDealt.CalculateDamage(ev.DamageHandler, Damage, damageType);
        }
        if (role.Damage.DamageReceived.Any(x => x.Key.DamageType == damageType))
            Damage = role.Damage.DamageReceived.CalculateDamage(ev.DamageHandler, Damage, damageType);
        if (attackerRole != null)
        ev.DamageHandler.SetDamageValue(Damage);
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
            if (!role.Display.RoleCanDisplay)
                return;
            Server.SendBroadcast(ev.Player, $"\nThis user has a special role: <color={role.Display.ColorHex}>{role.Display.SpectatorRoleName}</color>", Main.Instance.Config.SpectatorBroadcastTime);
        }
    }

    public override void OnPlayerUsingItem(PlayerUsingItemEventArgs ev)
    {
        if (!CustomRoleHelpers.TryGetCustomRole(ev.Player, out var role))
            return;
        if (!role.Deniable.Items.TryGetValue(ev.UsableItem.Type, out var deniable))
            return;
        if (!deniable.CanUse)
        {
            ev.IsAllowed = false;
            return;
        }
        if (ev.UsableItem.Type != ItemType.SCP500)
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
        CustomRoleHelpers.UnSetCustomInfoToPlayer(ev.Player);
        if (ev.Attacker == null)
            return;
        if (!CustomRoleHelpers.TryGetCustomRole(ev.Attacker, out var role))
            return;
        if (role.KillerToNewRole.Count == 0)
            return;

        var kv = role.KillerToNewRole.Where(x=>
        x.Key.KillerCustom == role.Rolename ||
        x.Key.KillerRole == ev.Attacker.Role ||
        x.Key.KillerTeam == ev.Attacker.Team
        );
        if (!kv.Any())
            return;
        CustomRoleHelpers.SetNewRole(ev.Player, kv.FirstOrDefault().Value);
    }

    public override void OnPlayerEscaping(PlayerEscapingEventArgs ev)
    {
        if (!CustomRoleHelpers.TryGetCustomRole(ev.Player, out var role))
            return;
        
        if (!role.Escape.CanEscape)
        {
            ev.IsAllowed = role.Escape.CanEscape;
            return;
        }
        CustomRoleHelpers.UnSetCustomInfoToPlayer(ev.Player);
        if (!role.Escape.ScenarioToRole.TryGetValue(ev.EscapeScenario, out var roleInfo))
            return;
        ev.NewRole = roleInfo.RoleType;
        Timing.CallDelayed(1.5f, () =>
        {
            CustomRoleHelpers.SetNewRole(ev.Player, roleInfo);
        });
    }

    public override void OnServerWaveRespawned(WaveRespawnedEventArgs ev)
    {
        if (ev.Players.Count == 0)
            return;
        List<CustomRoleBaseInfo> tmp = [];
        foreach (var item in ev.Players)
        {
            CustomRoleHelpers.UnSetCustomInfoToPlayer(item);
        }
        foreach (var item in Main.Instance.InWaveRoles.Where(x => x.Wave.Faction == ev.Wave.Faction && x.RoleType == CustomRoleType.InWave))
        {
            if (item.Wave.SkipCheck || item.Wave.MinRequired > ev.Players.Count)
                continue;
            var list = ev.Players.Where(x => x.Role == item.ReplaceRole).ToList();
            if (list.Count == 0)
                continue;
            CustomRoleHelpers.SetCustomInfoToPlayer(list.RandomItem(), item);
            tmp.Add(item);
        }

        foreach (var item in tmp)
        {
            Main.Instance.InWaveRoles.Remove(item);
        }
    }
}
