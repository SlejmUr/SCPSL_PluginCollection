using DavaCustomItems.Configs;
using DavaCustomItems.Managers;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Items;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using InventorySystem;
using InventorySystem.Items.Coin;
using MEC;

namespace DavaCustomItems.Coins;

[CustomItem(ItemType.Coin)]
public class BaseCustomCoin : CustomItem
{
    public override uint Id { get; set; }
    public override string Name { get; set; } 
    public override string Description { get; set; }
    public override float Weight { get; set; }
    public override SpawnProperties SpawnProperties { get; set; }
    public override ItemType Type { get; set; } = ItemType.Coin;
    public CoinRarityType Rarity { get; set; } = CoinRarityType.None;

    public LightConfig LightConfig { get; set; } = new();
    public CoinExtraConfig ExtraConfig { get; set; } = new();

    private int LightId;
    private int FlippedNumber;

    public override void Init()
    {
        base.Init();
        if (Main.Instance.Config.LightConfigs.TryGetValue(this.Name, out var config))
            LightConfig = config;
        else
            LightConfig = new();
    }

    public override void SubscribeEvents()
    {
        base.SubscribeEvents();
        Exiled.Events.Handlers.Player.ChangedItem += ChangedItem;
        Exiled.Events.Handlers.Player.DroppedItem += DroppedItem;
        Coin.OnFlipped += Coin_OnFlipped;
    }

    public override void UnsubscribeEvents()
    {
        base.UnsubscribeEvents();
        Exiled.Events.Handlers.Player.ChangedItem -= ChangedItem;
        Exiled.Events.Handlers.Player.DroppedItem -= DroppedItem;
        Coin.OnFlipped -= Coin_OnFlipped;
    }

    public void ChangedItem(ChangedItemEventArgs ev)
    {
        if (this.Check(ev.OldItem))
        {
            LightManager.RemoveLight(LightId);

        }
        if (this.Check(ev.Item))
        {
            if (LightConfig.ShouldFollowPlayer)
            {
                LightId = LightManager.MakeLightAndFollow(ev.Player, LightConfig);
            }
        }
    }

    public void DroppedItem(DroppedItemEventArgs ev)
    {
        if (!this.Check(ev.Pickup))
            return;
        LightManager.StopFollowAndStartNew(ev.Player, ev.Pickup, ref LightId, LightConfig, true);
    }

    public override void OnPickingUp(PickingUpItemEventArgs ev)
    {
        if (!this.Check(ev.Pickup))
            return;
        LightManager.RemoveLight(LightId);
    }

    public override void Destroy()
    {
        base.Destroy();
        LightManager.RemoveLight(LightId);
    }

    private void Coin_OnFlipped(ushort serial, bool isTails)
    {
        var item = Item.Get(serial);
        var owner = item.Owner;
        var configKV = ExtraConfig.NameAndWeight.GetRandomWeight(kv => kv.Key.Value == isTails, new("NoAction", true));
        Log.Info($"ConfigName : {configKV.Key}");
        var effect = CoinAction.Actions.FirstOrDefault(x => x.ActionName == configKV.Key);
        if (effect.ActionName == default)
            return; // ?
        effect.RunAction(owner, ExtraConfig, configKV.Key);
        FlippedNumber++;
        if (ExtraConfig.MaxFlipping == FlippedNumber)
        {
            Timing.CallDelayed(0.1f, () => owner.RemoveItem(item));
            return;
        }
        if (RNGManager.RNG.NextDouble() < ExtraConfig.CoinBrakeChance)
        {
            Timing.CallDelayed(0.1f, () => owner.RemoveItem(item));
            owner.ShowHint("Your coin broke!", 5);
        }
    }
}
