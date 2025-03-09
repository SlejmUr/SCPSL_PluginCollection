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

    public void CoinFlipping(FlippingCoinEventArgs ev)
    {
        System.Random random = new System.Random();
        int randomNumber = random.Next(1, 101); // 1-100% chance between good and bad
        bool isTails = false;

        if (Rarity == CoinRarityType.SuperUnluckyCoin && randomNumber < 50)
        {
            isTails = true;
        }
        if (Rarity == CoinRarityType.UnluckyCoin && randomNumber < 50) // will replace 50 with a config variable
        {
            isTails = true;
        }
        if (Rarity == CoinRarityType.NormalCoin && randomNumber < 50)
        {
            isTails = true;
        }
        if (Rarity == CoinRarityType.RareCoin && randomNumber < 50)
        {
            isTails = true;
        }
        if (Rarity == CoinRarityType.LegendaryCoin && randomNumber < 50)
        {
            isTails = true;
        }
        // We return what the thing is to make sure it is synced. (ie: Player dont see Tails and we set as Heads)
        ev.IsTails = isTails; 
        CoinFlipActions.RunActions(ev.Player, isTails, Rarity, ExtraConfig);
        FlippedNumber++;
        // TODO: Do this actually more and better logic.
        if (ExtraConfig.MaxFlipping == FlippedNumber)
        {
            Timing.CallDelayed(0.1f, () => ev.Player.RemoveItem(ev.Item));
        }

    }
}
