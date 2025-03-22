using DavaCustomItems.Configs;
using DavaCustomItems.Managers;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using InventorySystem.Items.MicroHID.Modules;
using MEC;
using UnityEngine;

namespace DavaCustomItems.Items.Weapons;

[CustomItem(ItemType.MicroHID)]
public class CustomMicro : BaseLightItem
{
    public override uint Id { get; set; } = (uint)CustomItemsEnum.CustomMicro;
    public override string Name { get; set; } = "Custom Micro";
    public override string Description { get; set; }
    public override float Weight { get; set; } = 22;
    public override ItemType Type { get; set; } = ItemType.MicroHID;
    public override SpawnProperties SpawnProperties { get; set; }

    public override LightConfig LightConfig { get; set; } = new()
    { 
        MovementSmoothing = 100,
        Color = Color.blue,
        ShouldMakeLight = true,
        ShouldFollowPlayer = true,
        Scale = Vector3.one,
        Intensity = 20,
        Range = 10,
        LightType = LightType.Area,
    };

    internal override bool GetLigthFromConfig { get; set; } = false;

    private Dictionary<MicroHidPhase, Color> PhaseToColor = new()
    {
        { MicroHidPhase.Standby, new Color(0f, 0f, 1f, 0.7f) },
        { MicroHidPhase.WindingUp, new Color(0f, 1f, 1f, 0.7f) },
        { MicroHidPhase.WindingDown, new Color(0, 0.5f, 1f, 0.7f) },
        { MicroHidPhase.WoundUpSustain, new Color(1f, 0f, 1f, 0.7f) },
        { MicroHidPhase.Firing, new Color(1f, 0f, 0f, 0.7f) },
    };

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
        var wrong_micors = Pickup.List.Where(x => x.Type == ItemType.MicroHID && !TrackedSerials.Contains(x.Serial)).ToList();
        foreach (var item in wrong_micors)
        {
            rotation = item.Rotation;
            pos = item.Position;
            item.Destroy();
        }
        Timing.CallDelayed(1, () =>
        {
            var micro = Get((uint)CustomItemsEnum.CustomMicro).Spawn(pos);
            micro.Rotation = rotation;
            micro.Position = pos;
        });
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
            lightId = LightFollowManager.MakeLightAndFollow(Item.Get(serial).Owner, LightConfig);
            LightSerialManager.AddLight(serial, lightId);
        }
        else
        {
            lightId = LightSerialManager.GetLightId(serial);
        }
        if (!LightManager.IsLightExists(lightId))
        {
            lightId = LightFollowManager.MakeLightAndFollow(Item.Get(serial).Owner, LightConfig);
            LightSerialManager.AddLight(serial, lightId);
        }
        LightManager.SetLightColor(lightId, PhaseToColor[phase]);
    }

}
