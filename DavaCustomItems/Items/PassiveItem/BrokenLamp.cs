using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Items;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;

namespace DavaCustomItems.Items.PassiveItem;

[CustomItem(ItemType.Lantern)]
public class BrokenLamp : CustomItem
{
    public override uint Id { get; set; } = (uint)CustomItemsEnum.BrokenLamp;
    public override string Name { get; set; } = "Broken Lamp";
    public override string Description { get; set; } = "A lamp with a funky Bulb";
    public override float Weight { get; set; }
    public override SpawnProperties SpawnProperties { get; set; }

    const int MaxUses = 5;
    const int CooldownTime = 10;

    private static Dictionary<ushort, int> serialUses = [];
    private static Dictionary<Player, bool> canUse = [];

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

    public override void OnAcquired(Player player, Item item, bool displayMessage)
    {
        base.OnAcquired(player, item, displayMessage);
        var fl = item as Flashlight;
        fl.IsEmittingLight = false;
    }

    public void TogglingLantern(TogglingFlashlightEventArgs ev)
    {
        // Check if the item being toggled is the Broken Lamp
        if (ev.Item == null || ev.Item.Type != ItemType.Lantern || !TrackedSerials.Contains(ev.Item.Serial))
        {
            return;
        }

        // if there is a usage and it cannot be used we return and not allow using.
        if (canUse.TryGetValue(ev.Player, out bool can_user) && !can_user)
        {
            ev.IsAllowed = false;
            return;
        }

        if (serialUses.TryGetValue(ev.Item.Serial, out int maxuses) && maxuses >= MaxUses)
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

            if (!serialUses.ContainsKey(ev.Item.Serial))
                serialUses.Add(ev.Item.Serial, 0);
            serialUses[ev.Item.Serial]++;
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