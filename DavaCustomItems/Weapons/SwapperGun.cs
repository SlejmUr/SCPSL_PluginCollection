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
    public override string Description { get; set; } = "Swapping to the other player who shot the gun. (The gun will destroyed.) CURRENTLY BUGGED!";
    public override float Weight { get; set; } = 1.5f;
    public override SpawnProperties SpawnProperties { get; set; } = new();
    public override ItemType Type => ItemType.GunCOM15;
    public override float Damage { get; set; } = 0f;
    public override byte ClipSize { get; set; } = 1;

    public override void OnShot(ShotEventArgs ev)
    {
        Timing.CallDelayed(0.1f, () => 
        {
            ev.Player.RemoveHeldItem();
        });
        
        if (ev.Target == null)
            return;
        // Bugged! Do not use anyhting here
        return;
        Timing.CallDelayed(1f, () =>
        {
            var pammo = ev.Player.Ammo;
            var tammo = ev.Target.Ammo;
            var p_ArtificialHealth = ev.Player.ArtificialHealth;
            var t_ArtificialHealth = ev.Target.ArtificialHealth;
            var p_Position = ev.Player.Position;
            var t_Position = ev.Target.Position;
            var p_Rotation = ev.Player.Rotation;
            var t_Rotation = ev.Target.Rotation;
            var p_Items = ev.Player.Items.ToList();
            var t_Items = ev.Target.Items.ToList();
            var p_Health = ev.Player.Health;
            var t_Health = ev.Target.Health;
            var p_ComponentsInChildren = ev.Player.ComponentsInChildren;
            var t_ComponentsInChildren = ev.Target.ComponentsInChildren;
            var p_CustomInfo = ev.Player.CustomInfo;
            var t_CustomInfo = ev.Target.CustomInfo;
            var p_CustomRoleFriendlyFireMultiplier = ev.Player.CustomRoleFriendlyFireMultiplier;
            var t_CustomRoleFriendlyFireMultiplier = ev.Target.CustomRoleFriendlyFireMultiplier;
            var p_HumeShield = ev.Player.HumeShield;
            var t_HumeShield = ev.Target.HumeShield;
            var p_MaxArtificialHealth = ev.Player.MaxArtificialHealth;
            var t_MaxArtificialHealth = ev.Target.MaxArtificialHealth;
            var p_MaxHealth = ev.Player.MaxHealth;
            var t_MaxHealth = ev.Target.MaxHealth;
            var p_Stamina = ev.Player.Stamina;
            var t_Stamina = ev.Target.Stamina;
            var p_UniqueRole = ev.Player.UniqueRole;
            var t_UniqueRole = ev.Target.UniqueRole;
            var p_UnitId = ev.Player.UnitId;
            var t_UnitId = ev.Target.UnitId;
            var p_VoiceChannel = ev.Player.VoiceChannel;
            var t_VoiceChannel = ev.Target.VoiceChannel;
            var p_VoiceChatMuteFlags = ev.Player.VoiceChatMuteFlags;
            var t_VoiceChatMuteFlags = ev.Target.VoiceChatMuteFlags;

            // role change!
            var p_Role = ev.Player.Role.Type;
            var t_Role = ev.Target.Role.Type;

            ev.Target.Role.Set(p_Role, PlayerRoles.RoleSpawnFlags.None);
            ev.Player.Role.Set(t_Role, PlayerRoles.RoleSpawnFlags.None);
            ev.Player.ClearInventory();
            ev.Target.ClearInventory();
            tammo.ForEach(x => ev.Player.Inventory.ServerSetAmmo(x.Key, x.Value));
            pammo.ForEach(x => ev.Target.Inventory.ServerSetAmmo(x.Key, x.Value));
            t_Items.ForEach(ev.Player.AddItem);
            p_Items.ForEach(ev.Target.AddItem);
            ev.Player.ArtificialHealth = t_ArtificialHealth;
            ev.Target.ArtificialHealth = p_ArtificialHealth;
            ev.Player.Position = t_Position;
            ev.Target.Position = p_Position;
            ev.Player.Rotation = t_Rotation;
            ev.Target.Rotation = p_Rotation;
            ev.Player.Stamina = t_Stamina;
            ev.Target.Stamina = p_Stamina;
            ev.Player.Stamina = t_Stamina;
            ev.Target.Stamina = p_Stamina;
            ev.Player.MaxHealth = t_MaxHealth;
            ev.Target.MaxHealth = p_MaxHealth;
            ev.Player.Health = t_Health;
            ev.Target.Health = p_Health;
            ev.Player.HumeShield = t_HumeShield;
            ev.Target.HumeShield = p_HumeShield;
        });
    }
}
