using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using InventorySystem;
using MEC;
using Utils.NonAllocLINQ;

namespace DavaCustomItems.Weapons;

[CustomItem(ItemType.GunCOM15)]
public class SwapperGun : CustomWeapon
{
    public override uint Id { get; set; } = 9000;
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
        var pammo = from.Ammo;
        var tammo = to.Ammo;
        var p_ArtificialHealth = from.ArtificialHealth;
        var t_ArtificialHealth = to.ArtificialHealth;
        var p_Position = from.Position;
        var t_Position = to.Position;
        var p_Rotation = from.Rotation;
        var t_Rotation = to.Rotation;
        var p_Items = from.Items.Select(x => x.Type).ToList();
        var t_Items = to.Items.Select(x => x.Type).ToList();
        var p_Health = from.Health;
        var t_Health = to.Health;
        var p_ComponentsInChildren = from.ComponentsInChildren; // ?
        var t_ComponentsInChildren = to.ComponentsInChildren; // ?
        var p_CustomInfo = from.CustomInfo;
        var t_CustomInfo = to.CustomInfo;
        var p_HumeShield = from.HumeShield;
        var t_HumeShield = to.HumeShield;
        var p_MaxArtificialHealth = from.MaxArtificialHealth;
        var t_MaxArtificialHealth = to.MaxArtificialHealth;
        var p_MaxHealth = from.MaxHealth;
        var t_MaxHealth = to.MaxHealth;
        var p_Stamina = from.Stamina;
        var t_Stamina = to.Stamina;
        var p_UnitId = from.UnitId;
        var t_UnitId = to.UnitId;
        var p_Effects = from.ActiveEffects.ToList();
        var t_Effects = to.ActiveEffects.ToList();

        yield return 0.1f;
        from.ClearInventory();
        to.ClearInventory();
        yield return 0.1f;
        tammo.ForEach(x => from.Inventory.ServerSetAmmo(x.Key, x.Value));
        pammo.ForEach(x => to.Inventory.ServerSetAmmo(x.Key, x.Value));
        yield return 0.1f;
        t_Items.ForEach(x => from.AddItem(x));
        p_Items.ForEach(x => to.AddItem(x));
        yield return 0.1f;
        from.ArtificialHealth = t_ArtificialHealth;
        to.ArtificialHealth = p_ArtificialHealth;
        from.Position = t_Position;
        to.Position = p_Position;
        from.Rotation = t_Rotation;
        to.Rotation = p_Rotation;
        from.Stamina = t_Stamina;
        to.Stamina = p_Stamina;
        from.Stamina = t_Stamina;
        to.Stamina = p_Stamina;
        from.MaxHealth = t_MaxHealth;
        to.MaxHealth = p_MaxHealth;
        from.Health = t_Health;
        to.Health = p_Health;
        from.HumeShield = t_HumeShield;
        to.HumeShield = p_HumeShield;
        from.UnitId = t_UnitId;
        to.UnitId = p_UnitId;
        foreach (var item in p_Effects)
        {
            to.EnableEffect(item.GetEffectType(), item.Intensity, item.Duration);
        }
        foreach (var item in t_Effects)
        {
            from.EnableEffect(item.GetEffectType(), item.Intensity, item.Duration);
        }
    }
}
