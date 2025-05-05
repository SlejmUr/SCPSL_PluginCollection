using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Arguments.ServerEvents;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Wrappers;
using LabApiExtensions.Enums;
using LabApiExtensions.Extensions;
using MEC;
using SimpleCustomRoles.Helpers;
using SimpleCustomRoles.RoleYaml;
using SimpleCustomRoles.RoleYaml.Enums;

namespace SimpleCustomRoles.Handler;

public class PlayerHandler : CustomEventsHandler
{
    public override void OnPlayerChangingRole(PlayerChangingRoleEventArgs ev)
    {
        if (ev.ChangeReason == PlayerRoles.RoleChangeReason.Destroyed)
            return;
        if (ev.ChangeReason != PlayerRoles.RoleChangeReason.RemoteAdmin)
            CustomRoleHelpers.UnSetCustomInfoToPlayer(ev.Player);
        AppearanceSyncExtension.ForceSync(ev.Player);
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
        float Damage = ev.DamageHandler.GetDamageValue();
        DamageType damageType = ev.DamageHandler.GetDamageType();

        if (ev.Attacker != null && CustomRoleHelpers.TryGetCustomRole(ev.Attacker, out var attacker_role))
        {
            if (attacker_role.Damage.DamageDealt.Any(x => x.Key.DamageType == damageType))
                Damage = attacker_role.Damage.DamageDealt.CalculateDamage(ev.DamageHandler, Damage, damageType);  
        }
        if (CustomRoleHelpers.TryGetCustomRole(ev.Player, out var player_role))
        {
            if (player_role.Damage.DamageReceived.Any(x => x.Key.DamageType == damageType))
                Damage = player_role.Damage.DamageReceived.CalculateDamage(ev.DamageHandler, Damage, damageType);
        }
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
                ev.Player.EnableEffect(effect.EffectName, effect.Intensity, effect.Duration);
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
        if (CustomRoleHelpers.SetNewRole(ev.Player, kv.FirstOrDefault().Value))
        {
            Timing.CallDelayed(0.8f, () =>
            {
                ev.Player.Position = ev.Attacker.Position;
            });
        }
    }

    public override void OnPlayerEscaping(PlayerEscapingEventArgs ev)
    {
        if (!CustomRoleHelpers.TryGetCustomRole(ev.Player, out var role))
        {
            if (Main.Instance.Config.EscapeConfigs.Count == 0 )
                return;
            var found = Main.Instance.Config.EscapeConfigs.FirstOrDefault(x=>x.Key.ShouldBeCuffer == ev.Player.IsDisarmed && x.Key.EscapeRole == ev.Player.Role);
            if (found.Value == PlayerRoles.RoleTypeId.None)
                return;
            ev.IsAllowed = true;
            ev.NewRole = found.Value;
        }

        if (!role.Escape.CanEscape)
        {
            ev.IsAllowed = role.Escape.CanEscape;
            return;
        }
        CustomRoleHelpers.UnSetCustomInfoToPlayer(ev.Player);
        var found2 = role.Escape.ConfigToRole.FirstOrDefault(x => x.Key.ShouldBeCuffer == ev.Player.IsDisarmed && x.Key.EscapeRole == ev.Player.Role);
        if (found2.Value == null)
            return;
        ev.NewRole = found2.Value.RoleType;
        Timing.CallDelayed(1.5f, () =>
        {
            CustomRoleHelpers.SetNewRole(ev.Player, found2.Value);
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
