using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;

namespace DavaCustomItems.Items.PassiveItem;

[CustomItem(ItemType.Adrenaline)]
public class AdrenalinBoost : CustomItem
{
    public override uint Id { get; set; } = (uint)CustomItemsEnum.AdrenalinBoost;
    public override string Name { get; set; } = "Adrenalin Boost";
    public override string Description { get; set; }
    public override float Weight { get; set; } = 5;
    public override SpawnProperties SpawnProperties { get; set; } = new()
    { 
        Limit = 3,
        LockerSpawnPoints =
        [
            new LockerSpawnPoint()
            {
                Chance = 100,
                UseChamber = true,
                Zone = Exiled.API.Enums.ZoneType.LightContainment,
                Type = Exiled.API.Enums.LockerType.Adrenaline,
            }
        ]
    };

    public override void SubscribeEvents()
    {
        base.SubscribeEvents();
        Exiled.Events.Handlers.Player.UsedItem += UsedItem;
    }

    public override void UnsubscribeEvents()
    {
        Exiled.Events.Handlers.Player.UsedItem -= UsedItem;
        base.UnsubscribeEvents();
    }

    private void UsedItem(UsedItemEventArgs ev)
    {
        if (!Check(ev.Item))
            return;
        ev.Player.EnableEffect(Exiled.API.Enums.EffectType.MovementBoost, 80, 10);
    }
}
