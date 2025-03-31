using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;

namespace DavaCustomItems.Items.Weapons;

[CustomItem(ItemType.GunCOM15)]
public class SwapperGunWithReload : CustomWeapon
{
    public override uint Id { get; set; } = (uint)CustomItemsEnum.SwapperGunWithReload;
    public override string Name { get; set; } = "Swapper Gun";
    public override string Description { get; set; } = "Swapping to the other player who shot the gun.";
    public override float Weight { get; set; } = 1.5f;
    public override SpawnProperties SpawnProperties { get; set; } = new();
    public override ItemType Type => ItemType.GunCOM15;
    public override float Damage { get; set; } = 0f;

    public override void OnShot(ShotEventArgs ev)
    {
        if (ev.Target == null)
            return;
        Timing.RunCoroutine(SwapUser(ev.Player, ev.Target));
    }

    IEnumerator<float> SwapUser(Player from, Player to)
    {
        yield return 0.1f;
        var p_Position = from.Position;
        var t_Position = to.Position;
        var p_Rotation = from.Rotation;
        var t_Rotation = to.Rotation;

        yield return 0.1f;
        from.Position = t_Position;
        to.Position = p_Position;
        from.Rotation = t_Rotation;
        to.Rotation = p_Rotation;
    }
}
