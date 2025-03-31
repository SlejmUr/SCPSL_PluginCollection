using Exiled.API.Extensions;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;

namespace DavaCustomItems.Items.Weapons;

[CustomItem(ItemType.GunRevolver)]
public class EffectClonerGiveAHP : CustomWeapon
{
    public override uint Id { get; set; } = (uint)CustomItemsEnum.EffectClonerGiveAHP;
    public override string Name { get; set; } = "Effect Cloner AHP";
    public override string Description { get; set; } = "You gain AHP when you shoot someone & also copy the effect of target!";
    public override float Weight { get; set; } = 4f;
    public override SpawnProperties SpawnProperties { get; set; }
    public override float Damage { get; set; }
    public override ItemType Type { get; set; } = ItemType.GunRevolver;

    public override void OnShot(ShotEventArgs ev)
    {
        if (ev.Target == null)
            return;
        ev.Player.AddAhp(5, 75, 1.5f);
        foreach (var effectBase in ev.Target.ActiveEffects)
        {
            if (effectBase.GetEffectType() == Exiled.API.Enums.EffectType.Invisible)
                return;
            ev.Player.EnableEffect(effectBase.GetEffectType(), effectBase._intensity, effectBase._duration, true);
        }
    }
}
