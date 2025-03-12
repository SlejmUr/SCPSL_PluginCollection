using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups.Projectiles;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using PlayerRoles.FirstPersonControl;

namespace DavaCustomItems.PassiveItem;

[CustomItem(ItemType.Lantern)]
public class BrokenLamp : CustomItem
{
    public override uint Id { get; set; } = 701;
    public override string Name { get; set; } = "Broken Lamp";
    public override string Description { get; set; } = "A lamp with a funky Bulb";
    public override float Weight { get; set; }
    public override SpawnProperties SpawnProperties { get; set; }

    const int MaxUses = 5;
    const int CooldownTime = 10;

    private Dictionary<Player, int> playerUses = [];
    private Dictionary<Player, bool> canUse = [];

    public override void SubscribeEvents()
    {
        base.SubscribeEvents();
        Exiled.Events.Handlers.Player.TogglingFlashlight += TogglingLantern;
    }

    public override void UnsubscribeEvents()
    {
        base.UnsubscribeEvents();
        Exiled.Events.Handlers.Player.TogglingFlashlight -= TogglingLantern;
    }

    public void TogglingLantern(TogglingFlashlightEventArgs ev)
    {
        // if there is a usage and it cannot be used we return and not allow using.
        if (canUse.TryGetValue(ev.Player, out bool can_user) && !can_user)
        {
            ev.IsAllowed = false;
            return;
        }

        if (playerUses.TryGetValue(ev.Player, out int maxuses) && maxuses >= MaxUses)
        {
            ev.IsAllowed = false;
            ev.Player.ShowHint("The Light No longer Turns on...", 5);
            return;
        }

        if (ev.NewState)
        {
            ev.IsAllowed = false;
            FlashGrenade flash = (FlashGrenade)FlashGrenade.Create(ItemType.GrenadeFlash);
            flash.FuseTime = 5;
            flash.SpawnActive(ev.Player.Position);

            Timing.CallDelayed(8, () =>
            {
                ev.Player.ShowHint("The Light Flashes Brightly, It looks Broken...", 5);
            });

            // add into can use false if not there
            if (!canUse.ContainsKey(ev.Player))
                canUse.Add(ev.Player, false);
            canUse[ev.Player] = false;

            if (!playerUses.ContainsKey(ev.Player))
                playerUses.Add(ev.Player, 0);
            playerUses[ev.Player]++;
            Timing.RunCoroutine(WaitThenUse(ev.Player));
        }
    }

    public IEnumerator<float> WaitThenUse(Player player)
    {
        yield return CooldownTime;
        if (!canUse.ContainsKey(player))
            canUse.Add(player, true);
        canUse[player] = true;
        yield break;
    }
}