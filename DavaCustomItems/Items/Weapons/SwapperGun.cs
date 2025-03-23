using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using InventorySystem;
using MEC;
using Utils.NonAllocLINQ;

namespace DavaCustomItems.Items.Weapons;

[CustomItem(ItemType.GunCOM15)]
public class SwapperGun : CustomWeapon
{
    public override uint Id { get; set; } = (uint)CustomItemsEnum.SwapperGun;
    public override string Name { get; set; } = "Swapper Gun";
    public override string Description { get; set; } = "Swapping to the other player who shot the gun. (The gun will destroyed)";
    public override float Weight { get; set; } = 1.5f;
    public override SpawnProperties SpawnProperties { get; set; } = new();
    public override ItemType Type => ItemType.GunCOM15;
    public override float Damage { get; set; } = 0f;
    public override byte ClipSize { get; set; } = 1;

    public override void OnShot(ShotEventArgs ev)
    {
        ev.Player.RemoveItem(ev.Item, true);
        if (ev.Target == null)
            return;
        if (!string.IsNullOrEmpty(ev.Player.UniqueRole))
            return;
        if (!string.IsNullOrEmpty(ev.Target.UniqueRole))
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
