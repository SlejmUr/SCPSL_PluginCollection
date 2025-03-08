using DavaCustomItems.Configs;
using DavaCustomItems.Managers;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
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

    public int LightId;
    public LightConfig LightConfig { get; set; } = new();
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
        Exiled.Events.Handlers.Player.FlippingCoin += CoinFlipping;
    }

    public override void UnsubscribeEvents()
    {
        base.UnsubscribeEvents();
        Exiled.Events.Handlers.Player.ChangedItem -= ChangedItem;
        Exiled.Events.Handlers.Player.DroppedItem -= DroppedItem;
        Exiled.Events.Handlers.Player.FlippingCoin -= CoinFlipping;

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
                if (!LightManager.IsLightExists(LightId))
                    LightId = LightManager.MakeLight(ev.Player.Position, LightConfig);
                LightManager.LightFollowPlayer(LightId, ev.Player);
            }
        }
    }

    public void DroppedItem(DroppedItemEventArgs ev)
    {
        if (!this.Check(ev.Pickup))
            return;
        if (!LightManager.IsLightExists(LightId))
            LightId = LightManager.MakeLight(ev.Pickup.Position, LightConfig);
        LightManager.StopFollowPlayer(ev.Player);
        Timing.CallDelayed(1f, () => 
        {
            LightManager.SetNewLightPos(LightId, ev.Pickup.Position);
        });
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

    public void CoinFlipping(FlippingCoinEventArgs ev)
    {
        CoinFlipActions.RunActions(ev.Player, ev.IsTails, Rarity);
    }
}
