using DavaCustomItems.Configs;
using DavaCustomItems.Managers;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Items;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
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

    public override void OnAcquired(Player player, Item item, bool displayMessage)
    {
        if (!this.Check(item))
            return;
        LightId = LightManager.MakeLight(player.Position, LightConfig, false);
    }

    public void ChangedItem(ChangedItemEventArgs ev)
    {
        if (this.Check(ev.OldItem))
        {
            LightManager.HideLight(LightId);
        }
        if (this.Check(ev.Item))
        {
            LightManager.ShowLight(LightId);
        }
        if (this.Check(ev.Item) && LightConfig.ShouldFollowPlayer)
        {
            LightManager.StartFollow(LightId, ev.Player);
        }
    }

    public void DroppedItem(DroppedItemEventArgs ev)
    {
        if (!this.Check(ev.Pickup))
            return;
        LightManager.StopFollowAndStartFollow(ev.Player, ev.Pickup);
    }

    public override void OnPickingUp(PickingUpItemEventArgs ev)
    {
        if (!this.Check(ev.Pickup))
            return;
        LightManager.HideLight(LightId);
    }

    public override void Destroy()
    {
        base.Destroy();
        LightManager.RemoveLight(LightId);
    }

    private void Coin_OnFlipped(ushort serial, bool isTails)
    {
        var item = Item.Get(serial);
        if (item == null) 
            return;
        if (!Check(item))
            return;
        var owner = item.Owner;
        var configKV = ExtraConfig.NameAndWeight.GetRandomWeight(kv => kv.Key.IsTails == isTails);
        Log.Info($"Player Flipped {owner.Id} ConfigName : {configKV.ActionName} {serial}");

        List<object> settings = [];

        // getting the extra settings var from it.
        if (configKV.UseWeight)
        {
            if (ExtraConfig.ExtraSettingsAndWeight.TryGetValue(configKV.ExtraSettingsParameter, out var dict2))
                settings = dict2.GetRandomWeight();
        }
        else
        {
            if (ExtraConfig.ExtraSettings.TryGetValue(configKV.ExtraSettingsParameter, out var dict2))
                settings = dict2;
        }

        if (configKV.ActionName.Contains('&'))
        {
            var splitted = configKV.ActionName.Split('&');
            foreach (var splitted_action in splitted)
            {
                var effect = CoinAction.Actions.FirstOrDefault(x => x.ActionName == splitted_action);
                if (effect.ActionName == default)
                    return; // ?
                Log.Info($"Running {splitted_action} with {owner.Id} {serial}");
                effect.RunAction(owner, ExtraConfig, settings);
            }
        }
        else
        {
            var effect = CoinAction.Actions.FirstOrDefault(x => x.ActionName == configKV.ActionName);
            if (effect.ActionName == default)
                return; // ?
            Log.Info($"Running {configKV.ActionName} with {owner.Id} {serial}");
            effect.RunAction(owner, ExtraConfig, settings);
        }
        
        FlippedNumber++;
        if (ExtraConfig.MaxFlipping == FlippedNumber)
        {
            Timing.CallDelayed(0.1f, () => owner.RemoveItem(item));
            owner.ShowHint("Your coin broke!", 5);
            return;
        }
        if (RNGManager.RNG.NextDouble() < ExtraConfig.CoinBrakeChance)
        {
            Timing.CallDelayed(0.1f, () => owner.RemoveItem(item));
            owner.ShowHint("Your coin broke!", 5);
        }
    }
}
