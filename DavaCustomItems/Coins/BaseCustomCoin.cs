using DavaCustomItems.Configs;
using DavaCustomItems.Managers;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Spawn;
using Exiled.API.Interfaces;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.Features;
using InventorySystem;
using InventorySystem.Items;
using InventorySystem.Items.Coin;
using MEC;
using PlayerRoles.FirstPersonControl;
using Scp914;
using System.Collections.Generic;
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

    public string CoinPickupHint = string.Empty;

    private static Dictionary<ushort, int> FlipByCoin = [];
    private static Dictionary<string, DateTime> CooldownByPlayer = [];

    #region Override
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
        Exiled.Events.Handlers.Player.DroppedItem += DroppedItem;
        Exiled.Events.Handlers.Player.FlippingCoin += CoinFlipping;
        Exiled.Events.Handlers.Map.FillingLocker += Map_FillingLocker;
        Coin.OnFlipped += Coin_OnFlipped;
        Scp914Upgrader.OnUpgraded += Scp914_Upgraded;
        InventoryExtensions.OnItemRemoved += InventoryExtensions_OnItemRemoved;
        Inventory.OnCurrentItemChanged += Inventory_OnCurrentItemChanged;
    }

    public override void UnsubscribeEvents()
    {
        base.UnsubscribeEvents();
        Exiled.Events.Handlers.Map.FillingLocker -= Map_FillingLocker;
        Exiled.Events.Handlers.Player.DroppedItem -= DroppedItem;
        Exiled.Events.Handlers.Player.FlippingCoin -= CoinFlipping;
        Coin.OnFlipped -= Coin_OnFlipped;
        Scp914Upgrader.OnUpgraded -= Scp914_Upgraded;
        InventoryExtensions.OnItemRemoved -= InventoryExtensions_OnItemRemoved;
        Inventory.OnCurrentItemChanged -= Inventory_OnCurrentItemChanged;
    }

    public override Pickup Spawn(Vector3 position, Item item, Player previousOwner = null)
    {
        var pickup = base.Spawn(position, item, previousOwner);
        if (LightConfig.ShouldShowLightOnSpawn)
        {
            LightSerialManager.AddLight(item.Serial, LightManager.MakeLight(position, LightConfig, true));
        }
        return pickup;
    }

    public override void OnAcquired(Player player, Item item, bool displayMessage)
    {

        if (!FlipByCoin.ContainsKey(item.Serial))
            FlipByCoin.Add(item.Serial, 0);


        if (!LightSerialManager.HasSerial(item.Serial))
        {
            LightSerialManager.AddLight(item.Serial, LightManager.MakeLight(player.Position, LightConfig, true));
        }
        else
        {
            if (!LightManager.IsLightExists(LightSerialManager.GetLightId(item.Serial)))
            {
                LightSerialManager.AddLight(item.Serial, LightManager.MakeLight(player.Position, LightConfig, false));
            }
                
        }
    }

    public override void OnPickingUp(PickingUpItemEventArgs ev)
    {
        if (LightSerialManager.HasSerial(ev.Pickup.Serial))
        {
            LightManager.HideLight(LightSerialManager.GetLightId(ev.Pickup.Serial));
        }
    }

    public override void OnChanging(ChangingItemEventArgs ev)
    {
        ev.Player.ShowHint(CoinPickupHint, 3);
    }
    #endregion
    #region Subscribed

    private void Inventory_OnCurrentItemChanged(ReferenceHub user, ItemIdentifier from_item, ItemIdentifier to_item)
    {

        if (from_item != ItemIdentifier.None && TrackedSerials.Contains(from_item.SerialNumber) && !TrackedSerials.Contains(to_item.SerialNumber))
        {
            if (LightSerialManager.HasSerial(from_item.SerialNumber))
            {
                LightManager.HideLight(LightSerialManager.GetLightId(from_item.SerialNumber));
                return;
            }
        }

        if (to_item == ItemIdentifier.None)
            return;

        if (TrackedSerials.Contains(to_item.SerialNumber) && LightConfig.ShouldFollowPlayer)
        {
            if (LightSerialManager.HasSerial(to_item.SerialNumber))
            {
                LightManager.StartFollow(LightSerialManager.GetLightId(to_item.SerialNumber), Player.Get(user));
                return;
            }
        }

        if (TrackedSerials.Contains(to_item.SerialNumber))
        {
            if (LightSerialManager.HasSerial(to_item.SerialNumber))
            {
                LightManager.ShowLight(LightSerialManager.GetLightId(to_item.SerialNumber));
            }
        }
    }

    private void Map_FillingLocker(FillingLockerEventArgs ev)
    {
        var random = RNGManager.RNG.Next(1, 101);
        bool check = random <= ExtraConfig.ReplaceCoinChance;
        //Log.Info($"{random} {ExtraConfig.ReplaceCoinChance} {check} {Rarity}");
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

    private void DroppedItem(DroppedItemEventArgs ev)
    {
        if (!Check(ev.Pickup))
            return;
        Timing.CallDelayed(0.2f, () =>
        {
            LightManager.StopFollowAndStartFollow(ev.Player, ev.Pickup);
        });
    }

    private void InventoryExtensions_OnItemRemoved(ReferenceHub arg1, ItemBase arg2, InventorySystem.Items.Pickups.ItemPickupBase arg3)
    {
        ushort serial = ushort.MaxValue;

        if (arg2 != null && this.TrackedSerials.Contains(arg2.ItemSerial) && arg3 != null && this.TrackedSerials.Contains(arg3.Info.Serial))
        {
            return;
        }

        if (arg2 != null && this.TrackedSerials.Contains(arg2.ItemSerial))
        {
            serial = arg2.ItemSerial;
        }

        if (arg3 != null && this.TrackedSerials.Contains(arg3.Info.Serial))
        {
            serial = arg3.Info.Serial;
        }

        if (LightSerialManager.HasSerial(serial))
        {
            var LightId = LightSerialManager.GetLightId(serial);
            LightManager.RemoveLight(LightId);
            LightSerialManager.RemoveLight(serial);
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
