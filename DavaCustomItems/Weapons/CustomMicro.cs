using DavaCustomItems.Configs;
using DavaCustomItems.Managers;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using InventorySystem.Items.MicroHID.Modules;
using MEC;
using UnityEngine;

namespace DavaCustomItems.Weapons;

[CustomItem(ItemType.MicroHID)]
public class CustomMicro : CustomItem
{
    public override uint Id { get; set; } = 666;
    public override string Name { get; set; } = "Custom Micro";
    public override string Description { get; set; }
    public override float Weight { get; set; }
    public override ItemType Type { get; set; } = ItemType.MicroHID;
    public override SpawnProperties SpawnProperties { get; set; }

    private LightConfig LightConfig = new()
    { 
        MovementSmoothing = 100,
        Color = Color.blue,
        ShouldMakeLight = true,
        ShouldFollowPlayer = true,
        Scale = Vector3.one,
        Intensity = 40,
        Range = 10,
        LightType = LightType.Area,
    };

    private Dictionary<MicroHidPhase, Color> PhaseToColor = new()
    {
        { MicroHidPhase.Standby, Color.blue },
        { MicroHidPhase.WindingUp, Color.cyan },
        { MicroHidPhase.WindingDown, new Color(0, 0.5f, 1f, 1f) },
        { MicroHidPhase.WoundUpSustain, Color.magenta },
        { MicroHidPhase.Firing, Color.red },
    };

    public override void OnAcquired(Player player, Item item, bool displayMessage)
    {
        if (!LightSerialManager.HasSerial(item.Serial))
        {
            LightSerialManager.AddLight(item.Serial, LightManager.MakeLightAndFollow(player, LightConfig));
        }
    }

    public override void SubscribeEvents()
    {
        base.SubscribeEvents();
        CycleController.OnPhaseChanged += CycleController_OnPhaseChanged;
    }

    public override void UnsubscribeEvents()
    {
        base.UnsubscribeEvents();
        CycleController.OnPhaseChanged -= CycleController_OnPhaseChanged;
    }

    public override void SpawnAll()
    {
        Quaternion rotation = Quaternion.identity;
        Vector3 pos = Vector3.one;
        var wrong_micors = Pickup.List.Where(x => x.Type == ItemType.MicroHID && !this.TrackedSerials.Contains(x.Serial)).ToList();
        foreach (var item in wrong_micors)
        {
            rotation = item.Rotation;
            pos = item.Position;
            item.Destroy();
        }
        Timing.CallDelayed(1, () =>
        {
            var micro = Get(666).Spawn(pos);
            micro.Rotation = rotation;
            micro.Position = pos;
        });
    }

    public override void OnDroppingItem(DroppingItemEventArgs ev)
    {
        var serial = ev.Item.Serial;
        if (LightSerialManager.HasSerial(serial))
        {
            var lightid = LightSerialManager.GetLightId(serial);
            LightSerialManager.RemoveLight(serial);
            LightManager.RemoveLight(lightid);
        }
    }

    private void CycleController_OnPhaseChanged(ushort serial, MicroHidPhase phase)
    {
        if (!TrackedSerials.Contains(serial))
        {
            return;
        }
        int lightId = -1;
        if (!LightSerialManager.HasSerial(serial))
        {
            lightId = LightManager.MakeLightAndFollow(Item.Get(serial).Owner, LightConfig);
            LightSerialManager.AddLight(serial, lightId);
        }
        else
        {
            lightId = LightSerialManager.GetLightId(serial);
        }
        if (!LightManager.IsLightExists(lightId))
        {
            lightId = LightManager.MakeLightAndFollow(Item.Get(serial).Owner, LightConfig);
            LightSerialManager.AddLight(serial, lightId);
        }
        LightManager.SetLightColor(lightId, PhaseToColor[phase]);
    }

}
