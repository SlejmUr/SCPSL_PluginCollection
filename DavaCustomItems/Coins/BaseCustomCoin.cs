using DavaCustomItems.Configs;
using DavaCustomItems.Managers;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using InventorySystem;
using InventorySystem.Items.Coin;
using MEC;
using Scp914;
using UnityEngine;

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
    private Dictionary<ushort, int> FlipByCoin = [];

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
        Exiled.Events.Handlers.Map.FillingLocker += Map_FillingLocker;
        Coin.OnFlipped += Coin_OnFlipped;
        Scp914Upgrader.OnUpgraded += Scp914_Upgraded;
    }

    public override void UnsubscribeEvents()
    {
        base.UnsubscribeEvents();
        Exiled.Events.Handlers.Map.FillingLocker -= Map_FillingLocker;
        Exiled.Events.Handlers.Player.ChangedItem -= ChangedItem;
        Exiled.Events.Handlers.Player.DroppedItem -= DroppedItem;
        Coin.OnFlipped -= Coin_OnFlipped;
        Scp914Upgrader.OnUpgraded -= Scp914_Upgraded;
    }

    public override Pickup Spawn(Vector3 position, Item item, Player previousOwner = null)
    {
        var pickup = base.Spawn(position, item, previousOwner);
        if (LightConfig.ShouldShowLightOnSpawn)
            LightId = LightManager.MakeLight(position, LightConfig, true);
        return pickup;
    }

    public override void OnAcquired(Player player, Item item, bool displayMessage)
    {
        if (!Check(item))
            return;
        item.ChangeItemOwner(null, player);

        if (!FlipByCoin.ContainsKey(item.Serial))
            FlipByCoin[item.Serial] = 0;

        if (!LightManager.IsLightExists(LightId))
            LightId = LightManager.MakeLight(player.Position, LightConfig, false);
        //LightManager.ShowLight(LightId);
    }

    public override void OnPickingUp(PickingUpItemEventArgs ev)
    {
        ev.Pickup.PreviousOwner = ev.Player;
        LightManager.HideLight(LightId);
    }

    public override void Destroy()
    {
        base.Destroy();
        LightManager.RemoveLight(LightId);
    }


    private void ChangedItem(ChangedItemEventArgs ev)
    {
        if (Check(ev.OldItem) && !Check(ev.Item))
        {
            LightManager.HideLight(LightId);
        }
        if (Check(ev.Item))
        {
            LightManager.ShowLight(LightId);
        }
        if (Check(ev.Item) && LightConfig.ShouldFollowPlayer)
        {
            LightManager.StartFollow(LightId, ev.Player);
        }
    }

    private void Map_FillingLocker(FillingLockerEventArgs ev)
    {
        if (ev.Pickup.Type == ItemType.Coin && ExtraConfig.ReplaceNormalCoinAmount != 0)
        {
            ev.IsAllowed = false;
            var pickup = Spawn(ev.Pickup.Position);
            ExtraConfig.ReplaceNormalCoinAmount--;
        }
        if (ExtraConfig.ItemsToReplace.TryGetValue(ev.Pickup.Type, out int value) && value != 0)
        {
            ev.IsAllowed = false;
            var pickup = Spawn(ev.Pickup.Position);
            ExtraConfig.ItemsToReplace[ev.Pickup.Type]--;
        }
    }

    private void DroppedItem(DroppedItemEventArgs ev)
    {
        if (!Check(ev.Pickup))
            return;
        LightManager.StopFollowAndStartFollow(ev.Player, ev.Pickup);
    }

    private void Scp914_Upgraded(Scp914Result result, Scp914KnobSetting setting)
    {
        if (!ExtraConfig.Replace914Coins)
            return;

        if (result.ResultingItems != null)
        {
            foreach (var item in result.ResultingItems)
            {
                if (item.ItemTypeId != ItemType.Coin)
                    continue;
                Timing.CallDelayed(2, () => 
                {
                    Player.Get(item.Owner).RemoveItem(Item.Get(item));
                    Give(Player.Get(item.Owner), false);
                });
            }
        }

        if (result.ResultingPickups != null)
        {
            foreach (var item in result.ResultingPickups)
            {
                if (item.Info.ItemId != ItemType.Coin)
                    continue;
                Timing.CallDelayed(2, () =>
                {
                    var prev_pickup_serial = item.Info.Serial;
                    Server.Host.Inventory.ServerRemoveItem(item.Info.Serial, item);
                    item.DestroySelf();
                    Player owner = null;
                    if (item.PreviousOwner.Hub != null)
                        owner = Player.Get(item.PreviousOwner.Hub);
                    Spawn(item.Position, owner);
                });
            }
        }
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
        if (!FlipByCoin.ContainsKey(item.Serial))
            FlipByCoin[item.Serial] = 0;
        FlipByCoin[serial]++;
        Log.Info($"FlippedNumber {owner.Id} {FlipByCoin[serial]}");
        if (ExtraConfig.MaxFlipping == FlipByCoin[serial])
        {
            Timing.CallDelayed(0.1f, () => owner.RemoveItem(item));
            Timing.CallDelayed(3f, () => owner.ShowHint("Your coin broke!", 5));
            return;
        }
        if (RNGManager.RNG.NextDouble() < ExtraConfig.CoinBrakeChance)
        {
            Timing.CallDelayed(0.1f, () => owner.RemoveItem(item));
            Timing.CallDelayed(3f, () => owner.ShowHint("Your coin broke!", 5));
        }
    }
}
