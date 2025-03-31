using Exiled.API.Extensions;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;

namespace DavaCustomItems.Items.Weapons;

[CustomItem(ItemType.GunRevolver)]
public class EffectCloner : CustomWeapon
{
    public override uint Id { get; set; } = (uint)CustomItemsEnum.EffectCloner;
    public override string Name { get; set; } = "Effect Cloner";
    public override string Description { get; set; } = "You gain the effect of the target!";
    public override float Weight { get; set; } = 4f;
    public override SpawnProperties SpawnProperties { get; set; }
    public override float Damage { get; set; }
    public override ItemType Type { get; set; } = ItemType.GunRevolver;

    public override void OnShot(ShotEventArgs ev)
    {
        if (ev.Target == null)
            return;
        foreach (var effectBase in ev.Target.ActiveEffects)
        {
            if (effectBase.GetEffectType() == Exiled.API.Enums.EffectType.Invisible)
                return;
            ev.Player.EnableEffect(effectBase.GetEffectType(), effectBase._intensity, effectBase._duration, true);
        }
    }
}
