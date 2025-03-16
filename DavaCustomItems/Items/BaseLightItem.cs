using DavaCustomItems.Configs;
using DavaCustomItems.Managers;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using Exiled.CustomItems.API.EventArgs;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using InventorySystem;
using InventorySystem.Items;
using MEC;
using UnityEngine;

namespace DavaCustomItems.Items;

public abstract class BaseLightItem : CustomItem
{
    internal virtual bool GetLigthFromConfig { get; set; } = true;
    public virtual LightConfig LightConfig { get; set; } = new();

    public override void Init()
    {
        base.Init();
        if (!GetLigthFromConfig)
            return;
        if (Main.Instance.Config.LightConfigs.TryGetValue(Name, out var config))
            LightConfig = config;
        else
            LightConfig = new();
    }

    public override void SubscribeEvents()
    {
        base.SubscribeEvents();
        Exiled.Events.Handlers.Player.DroppedItem += DroppedItem;
        InventoryExtensions.OnItemRemoved += InventoryExtensions_OnItemRemoved;
        Inventory.OnCurrentItemChanged += Inventory_OnCurrentItemChanged;
    }

    public override void UnsubscribeEvents()
    {
        Exiled.Events.Handlers.Player.DroppedItem += DroppedItem;
        InventoryExtensions.OnItemRemoved += InventoryExtensions_OnItemRemoved;
        Inventory.OnCurrentItemChanged += Inventory_OnCurrentItemChanged;
        base.UnsubscribeEvents();
    }

    public override Pickup Spawn(Vector3 position, Item item, Player previousOwner = null)
    {
        var pickup = base.Spawn(position, item, previousOwner);
        LightSerialManager.AddLight(item.Serial, LightManager.MakeLight(position, LightConfig, LightConfig.ShouldShowLightOnSpawn));
        return pickup;
    }

    public override void OnAcquired(Player player, Item item, bool displayMessage)
    {
        base.OnAcquired(player, item, displayMessage);
        if (!LightSerialManager.HasSerial(item.Serial))
            LightSerialManager.AddLight(item.Serial, LightManager.MakeLight(player.Position, LightConfig, false));
    }

    public override void OnPickingUp(PickingUpItemEventArgs ev)
    {
        if (!LightSerialManager.HasSerial(ev.Pickup.Serial))
            return;
        LightManager.HideLight(LightSerialManager.GetLightId(ev.Pickup.Serial));
    }

    public override void OnOwnerDying(OwnerDyingEventArgs ev)
    {
        LightManager.StopFollow(ev.Player);
    }

    private void DroppedItem(DroppedItemEventArgs ev)
    {
        if (!Check(ev.Pickup))
            return;
        if (!LightConfig.ShouldFollowPickup)
            return;
        Timing.CallDelayed(0.2f, () =>
        {
            LightManager.StopFollowAndStartFollow(ev.Player, ev.Pickup);
        });
    }

    private void Inventory_OnCurrentItemChanged(ReferenceHub user, ItemIdentifier from_item, ItemIdentifier to_item)
    {
        // Checks if the item to swapped from is tracked and the next is not tracker plus the old has a serial

        // Hide the light if the swapped from item is tracked and the swapped to item is not.
        if (from_item != ItemIdentifier.None && TrackedSerials.Contains(from_item.SerialNumber))
        {
            if (LightSerialManager.HasSerial(from_item.SerialNumber))
                LightManager.HideLight(LightSerialManager.GetLightId(from_item.SerialNumber));
        }

        if (to_item == ItemIdentifier.None)
            return;

        // Check if the to item is tracked, and has a serial, plus also should folllow our player.
        if (TrackedSerials.Contains(to_item.SerialNumber) && LightConfig.ShouldFollowPlayer && LightSerialManager.HasSerial(to_item.SerialNumber))
        {
            LightManager.StartFollow(LightSerialManager.GetLightId(to_item.SerialNumber), Player.Get(user));
        }
    }

    private void InventoryExtensions_OnItemRemoved(ReferenceHub arg1, ItemBase arg2, InventorySystem.Items.Pickups.ItemPickupBase arg3)
    {
        ushort serial = ushort.MaxValue;

        // return if we just made a pickup from the item.
        if (arg2 != null && TrackedSerials.Contains(arg2.ItemSerial) && arg3 != null && TrackedSerials.Contains(arg3.Info.Serial))
        {
            return;
        }

        // get the serial of the item we destroyed
        if (arg2 != null && TrackedSerials.Contains(arg2.ItemSerial))
        {
            serial = arg2.ItemSerial;
        }

        // get the serial of the pickup
        if (arg3 != null && TrackedSerials.Contains(arg3.Info.Serial))
        {
            serial = arg3.Info.Serial;
        }

        // if has a serial get the light and destroy
        if (LightSerialManager.HasSerial(serial))
        {
            var LightId = LightSerialManager.GetLightId(serial);
            LightManager.RemoveLight(LightId); // with this remove we also remove from the serial.
        }
    }
}
