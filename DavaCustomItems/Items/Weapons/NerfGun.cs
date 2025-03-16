using Exiled.API.Features.Attributes;
using Exiled.API.Features;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;

namespace DavaCustomItems.Items.Weapons;

[CustomItem(ItemType.GunCrossvec)]
public class NerfGun : CustomWeapon
{
    public override float Damage { get; set; } = 1;
    public override uint Id { get; set; } = 600;
    public override string Name { get; set; } = "Nerf Gun";
    public override string Description { get; set; } // Only Does 1 Damage
    public override float Weight { get; set; } = 1.5f;
    public override ItemType Type => ItemType.GunCrossvec;
    public override byte ClipSize { get; set; } = 30;
    public override SpawnProperties SpawnProperties { get; set; } = new()
    { 
        Limit = 1,
        LockerSpawnPoints =
        [
            new LockerSpawnPoint()
            {
                Chance = 0.15f,
                Type = Exiled.API.Enums.LockerType.RifleRack,
                UseChamber = true,
                Zone = Exiled.API.Enums.ZoneType.HeavyContainment,
            }
        ]
    };

    public override void ShowPickedUpMessage(Player player)
    {

    }

    public override void ShowSelectedMessage(Player player)
    {

    }
}
