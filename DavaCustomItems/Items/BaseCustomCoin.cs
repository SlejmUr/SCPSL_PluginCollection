using DavaCustomItems.Coins;
using DavaCustomItems.Configs;
using DavaCustomItems.Managers;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Items;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using InventorySystem;
using InventorySystem.Items.Coin;
using MEC;
using Scp914;

namespace DavaCustomItems.Items;

[CustomItem(ItemType.Coin)]
public class BaseCustomCoin : BaseLightItem
{
    public override uint Id { get; set; }
    public override string Name { get; set; } 
    public override string Description { get; set; }
    public override float Weight { get; set; }
    public override SpawnProperties SpawnProperties { get; set; }
    public override ItemType Type { get; set; } = ItemType.Coin;
    public CoinRarityType Rarity { get; set; } = CoinRarityType.None;

    public CoinExtraConfig ExtraConfig { get; set; } = new();

    public string CoinPickupHint = string.Empty;

    private static Dictionary<ushort, int> FlipByCoin = [];
    private static Dictionary<string, DateTime> CooldownByPlayer = [];

    #region Override

    public override void SubscribeEvents()
    {
        base.SubscribeEvents();
        Exiled.Events.Handlers.Player.FlippingCoin += CoinFlipping;
        Exiled.Events.Handlers.Map.FillingLocker += Map_FillingLocker;
        Coin.OnFlipped += Coin_OnFlipped;
        Scp914Upgrader.OnUpgraded += Scp914_Upgraded;
    }

    public override void UnsubscribeEvents()
    {
        base.UnsubscribeEvents();
        Exiled.Events.Handlers.Map.FillingLocker -= Map_FillingLocker;
        Exiled.Events.Handlers.Player.FlippingCoin -= CoinFlipping;
        Coin.OnFlipped -= Coin_OnFlipped;
        Scp914Upgrader.OnUpgraded -= Scp914_Upgraded;
    }

    public override void OnAcquired(Player player, Item item, bool displayMessage)
    {

        if (!FlipByCoin.ContainsKey(item.Serial))
            FlipByCoin.Add(item.Serial, 0);

        base.OnAcquired(player, item, false);
    }

    public override void OnChanging(ChangingItemEventArgs ev)
    {
        ev.Player.ShowHint(CoinPickupHint, 3);
    }
    #endregion
    #region Subscribed

    private void Map_FillingLocker(FillingLockerEventArgs ev)
    {
        var random = RNGManager.RNG.Next(1, 101);
        bool check = random <= ExtraConfig.ReplaceCoinChance;
        //Log.Info($"{random} {ExtraConfig.ReplaceCoinChance} {check} {Rarity} {ev.Pickup.Type}");
        if (ev.Pickup.Type == ItemType.Coin && 
            ExtraConfig.ReplaceNormalCoinAmount != 0 && check)
        {
            ev.IsAllowed = false;
            Spawn(ev.Pickup.Position);
            Log.Info($"Coin replaced with {Rarity} Coin");
            ExtraConfig.ReplaceNormalCoinAmount--;
        }
        if (ExtraConfig.ItemsToReplace.TryGetValue(ev.Pickup.Type, out int value) && value != 0 && check)
        {
            ev.IsAllowed = false;
            Log.Info($"{ev.Pickup.Type} replaced with {Rarity} Coin");
            Spawn(ev.Pickup.Position);
            ExtraConfig.ItemsToReplace[ev.Pickup.Type]--;
        }
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
                Timing.CallDelayed(1, () => 
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
                Timing.CallDelayed(1, () =>
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
    public void CoinFlipping(FlippingCoinEventArgs ev)
    {
        // Make sure we always set this.

        if (!Check(ev.Item))
            return;

        if (!CooldownByPlayer.ContainsKey(ev.Player.UserId))
        {
            CooldownByPlayer.Add(ev.Player.UserId, DateTime.UtcNow);
            return;
        }

        if (CooldownByPlayer.TryGetValue(ev.Player.UserId, out var value))
        {
            var total = (DateTime.UtcNow - value).TotalSeconds;
            if (total < ExtraConfig.CoolDown)
            {
                ev.IsAllowed = false;
                return;
            }
        }

        CooldownByPlayer[ev.Player.UserId] = DateTime.UtcNow;
    }

    private void Coin_OnFlipped(ushort serial, bool isTails)
    {
        if (!TrackedSerials.Contains(serial))
            return;

        if (!InventoryExtensions.ServerTryGetItemWithSerial(serial, out var item))
            return;

        var owner = Player.Get(item.Owner);

        var configKV = ExtraConfig.NameAndWeight.GetRandomWeight(kv => kv.Key.IsTails == isTails);
        Log.Info($"Player Flipped {owner.Id} ConfigName : {configKV.ActionName} {serial} {Rarity}");

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
                Log.Info($"Running {splitted_action} with {owner.Id} {serial} {Rarity}");
                effect.RunAction(owner, ExtraConfig, settings);
            }
        }
        else
        {
            var effect = CoinAction.Actions.FirstOrDefault(x => x.ActionName == configKV.ActionName);
            if (effect.ActionName == default)
                return; // ?
            Log.Info($"Running {configKV.ActionName} with {owner.Id} {serial} {Rarity}");
            effect.RunAction(owner, ExtraConfig, settings);
        }

        if (!FlipByCoin.ContainsKey(serial))
            FlipByCoin[serial] = 0;
        FlipByCoin[serial]++;
        Log.Info($"FlippedNumber for {owner.Id} is {FlipByCoin[serial]}");

        if (ExtraConfig.MaxFlipping == FlipByCoin[serial])
        {
            Timing.CallDelayed(0.1f, () => owner.RemoveItem(serial));
            Timing.CallDelayed(3f, () => owner.ShowHint("Your coin broke!", 5));
            return;
        }
        if (RNGManager.RNG.NextDouble() < ExtraConfig.CoinBrakeChance)
        {
            Timing.CallDelayed(0.1f, () => owner.RemoveItem(serial));
            Timing.CallDelayed(3f, () => owner.ShowHint("Your coin broke!", 5));
        }
    }
    #endregion
}
