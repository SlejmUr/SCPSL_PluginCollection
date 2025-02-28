using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using SimpleCustomRoles.RoleInfo;
using System.Linq;

namespace SimpleCustomRoles.Handler;

public class TheHandler
{
    public static void ChangingRole(ChangingRoleEventArgs args)
    {
        if (args.Reason == Exiled.API.Enums.SpawnReason.ForceClass)
            RoleSetter.UnSetCustomInfoToPlayer(args.Player);
    }

    public static void Hurting(HurtingEventArgs args)
    {
        if (args.Player == null)
            return;
        if (!Main.Instance.PlayerCustomRole.TryGetValue(args.Player.UserId, out var role))
            return;

        // Set Attacker DMG first.
        CustomRoleInfo attacker_role = null;
        if (args.Attacker != null && 
            Main.Instance.PlayerCustomRole.TryGetValue(args.Attacker.UserId, out attacker_role) &&
            attacker_role.Advanced.Damager.DamageSentDict.TryGetValue(args.DamageHandler.Type, out var valueSetter))
        {
            args.Amount = RoleSetter.MathWithFloat(valueSetter.SetType, args.Amount, valueSetter.Value);
        }
        // set Receiver DMG
        if (role.Advanced.Damager.DamageReceivedDict.TryGetValue(args.DamageHandler.Type, out var dmg))
            args.Amount = RoleSetter.MathWithFloat(dmg.SetType, args.Amount, dmg.Value);

        // Call event
        if (attacker_role != null && !string.IsNullOrEmpty(attacker_role.EventCaller.OnDealDamage))
            Server.ExecuteCommand($"{attacker_role.EventCaller.OnDealDamage} {args.Attacker.Id} {args.Player.Id}  {args.DamageHandler.Type} {args.Amount}");

        // Call Event
        if (!string.IsNullOrEmpty(role.EventCaller.OnReceiveDamage))
        {
            int attackerID = 0;
            if (args.Attacker != null)
                attackerID = args.Attacker.Id;
            // Call event
            Server.ExecuteCommand($"{role.EventCaller.OnReceiveDamage} {args.Player.Id} {attackerID} {args.DamageHandler.Type} {args.Amount}");
        }
    }

    public static void ChangingSpectatedPlayer(ChangingSpectatedPlayerEventArgs args)
    {
        // if any spec null reutnr
        if (args.OldTarget == null && args.NewTarget == null)
            return;

        // clear if old taget has custom role
        if (args.OldTarget != null && Main.Instance.PlayerCustomRole.ContainsKey(args.OldTarget.UserId))
            args.Player.ClearBroadcasts();

        // show broadcast if has newtarget and can be displayed
        if (args.NewTarget != null && Main.Instance.PlayerCustomRole.TryGetValue(args.NewTarget.UserId, out var role) && role.RoleCanDisplay)
        {
            Exiled.API.Features.Broadcast broadcast = new($"\nThis user has a special role: <color={role.RoleDisplayColorHex}>{role.DisplayRoleName}</color>", Main.Instance.Config.SpectatorBroadcastTime);
            args.Player.Broadcast(broadcast, false);
        }
    }

    public static void DroppingItem(DroppingItemEventArgs args)
    {
        if (!Main.Instance.PlayerCustomRole.TryGetValue(args.Player.UserId, out var role))
            return;
        // Disable dropping if not allowed
        if (role.Inventory.CannotDropItems.Contains(args.Item.Type))
            args.IsAllowed = false;
    }

    public static void UsingItem(UsingItemEventArgs args)
    {
        if (!Main.Instance.PlayerCustomRole.TryGetValue(args.Player.UserId, out var role))
            return;

        // disable using if not allowed
        if (role.Inventory.DeniedUsingItems.Contains(args.Item.Type))
        {
            args.IsAllowed = false;
            return;
        }
        // wait 3s to re-enable effects
        Timing.CallDelayed(3f, () =>
        {
            foreach (var effect in role.Effects)
            {
                if (!effect.CanRemovedWithSCP500)
                {
                    Log.Debug($"(Used 500) Effect {effect.EffectType}: IsSet? " + args.Player.EnableEffect(effect.EffectType, effect.Intensity, effect.Duration));
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
                int attackerID = args.Attacker != null ? args.Attacker.Id : 0;
                // Call event
                Server.ExecuteCommand($"{died_player_role.EventCaller.OnDied} {args.Player.Id} {attackerID} {args.DamageHandler.Type}");
            }
        }
        RoleSetter.UnSetCustomInfoToPlayer(args.Player, true);
        if (args.Attacker == null)
            return;
        if (!Main.Instance.PlayerCustomRole.TryGetValue(args.Attacker.UserId, out var role))
            return;
        if (!string.IsNullOrEmpty(role.EventCaller.OnKill))
        {
            // Call event
            Server.ExecuteCommand($"{role.EventCaller.OnKill} {args.Attacker.Id} {args.Player.Id} {args.DamageHandler.Type}");
        }
        if (!role.Advanced.DeadBy.IsConfigurated)
            return;
        if (role.Advanced.DeadBy.RoleAfterKilled != PlayerRoles.RoleTypeId.None)
        {
            args.Player.Role.Set(role.Advanced.DeadBy.RoleAfterKilled, PlayerRoles.RoleSpawnFlags.None);
            return;
        }

        if (!string.IsNullOrEmpty(role.Advanced.DeadBy.RoleNameToRespawnAs))
        {
            var customRoleInfo = Main.Instance.AfterDeathRoles.FirstOrDefault(x => x.RoleName == role.Advanced.DeadBy.RoleNameToRespawnAs);
            if (customRoleInfo == null)
                return;
            RoleSetter.SetFromCMD(args.Player, customRoleInfo);
        }
        else if (role.Advanced.DeadBy.RoleNameRandom.Count != 0)
        {
            var customRoleInfo = Main.Instance.AfterDeathRoles.FirstOrDefault(x => x.RoleName == role.Advanced.DeadBy.RoleNameRandom.RandomItem());
            if (customRoleInfo == null)
                return;
            RoleSetter.SetFromCMD(args.Player, customRoleInfo);
        }
    }

    public static void Dying(DyingEventArgs ev)
    {
        // return if no custom role or does not have dying event
        if (!Main.Instance.PlayerCustomRole.TryGetValue(ev.Player.UserId, out var dying_player_role))
            return;
        if (string.IsNullOrEmpty(dying_player_role.EventCaller.OnDying))
            return;
        // set attackerid 0 if null otherwise id.
        int attackerID = ev.Attacker == null ? 0 : ev.Attacker.Id;
        // Call event
        Log.Info($"{dying_player_role.EventCaller.OnDying} {ev.Player.Id} {attackerID} {ev.DamageHandler.Type}");
        Server.ExecuteCommand($"{dying_player_role.EventCaller.OnDying} {ev.Player.Id}  {attackerID} {ev.DamageHandler.Type}");
    }

    public static void Escaping(EscapingEventArgs args)
    {
        if (!Main.Instance.PlayerCustomRole.TryGetValue(args.Player.UserId, out var role))
            return;
        // short check if can escape.
        args.IsAllowed = role.Advanced.Escaping.CanEscape;
        if (!args.IsAllowed)
            return;

        // set to role to the type we want + remove from old one.
        if (role.Advanced.Escaping.RoleAfterEscape.TryGetValue(args.EscapeScenario, out var roleTypeId) && roleTypeId != PlayerRoles.RoleTypeId.None)
        {
            RoleSetter.UnSetCustomInfoToPlayer(args.Player, true);
            args.NewRole = roleTypeId;
        }
        // set to custom role to the type we want
        if (role.Advanced.Escaping.RoleNameAfterEscape.TryGetValue(args.EscapeScenario, out var rolename) && !string.IsNullOrEmpty(rolename))
        {
            Timing.CallDelayed(2.5f, () =>
            {
                var escapeRole = Main.Instance.EscapeRoles.FirstOrDefault(x => x.RoleName == rolename);
                RoleSetter.SetFromCMD(args.Player, escapeRole);
            });
        }
    }
}
