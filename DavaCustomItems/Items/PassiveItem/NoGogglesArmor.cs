using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Items;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using PlayerRoles.FirstPersonControl;

namespace DavaCustomItems.Items.PassiveItem;

[CustomItem(ItemType.ArmorCombat)]
public class NoGogglesArmor : CustomArmor
{
    public override uint Id { get; set; } = (uint)CustomItemsEnum.NoGogglesArmor;
    public override string Name { get; set; } = "No Goggle Armor";
    public override string Description { get; set; } = "This Armor makes the equipped player not seen in Goggles";
    public override float Weight { get; set; }
    public override SpawnProperties SpawnProperties { get; set; }

    private Player Owner;

    public override void SubscribeEvents()
    {
        base.SubscribeEvents();
        CustomPlayerEffects.Scp1344.OnPlayerSeen += Scp1344_OnPlayerSeen;
    }

    public override void UnsubscribeEvents()
    {
        base.UnsubscribeEvents();
        CustomPlayerEffects.Scp1344.OnPlayerSeen -= Scp1344_OnPlayerSeen;
    }

    private void Scp1344_OnPlayerSeen(ReferenceHub who, ReferenceHub target)
    {
        Log.Info($"Scp1344_OnPlayerSeen | who: {who} target: {target} owner: {Owner}");
        if (Owner == null)
            return;
        Log.Info($"who: {who} target: {target} owner: {Owner}");
        if (Owner.ReferenceHub != target)
            return;
        Timing.CallDelayed(1, () =>
        {
            if (Owner.Role.Base is IFpcRole fpc && CustomPlayerEffects.Scp1344.Trackers.TryGetValue(fpc, out var particle))
            {
                var emi = particle.emission;
                emi.enabled = false;
            }
        });
    }

    public override void OnAcquired(Player player, Item item, bool displayMessage)
    {
        base.OnAcquired(player, item, displayMessage);
        Owner = player;
        Timing.CallDelayed(1, () =>
        {
            if (Owner.Role.Base is IFpcRole fpc && CustomPlayerEffects.Scp1344.Trackers.TryGetValue(fpc, out var particle))
            {
                var emi = particle.emission;
                emi.enabled = false;
            }
        });
    }

    public override void OnDroppingItem(DroppingItemEventArgs ev)
    {
        if (ev.Player != Owner)
            return;
        if (!Check(ev.Item))
            return;
        if (Owner.Role.Base is IFpcRole fpc && CustomPlayerEffects.Scp1344.Trackers.TryGetValue(fpc, out var particle))
        {
            var emi = particle.emission;
            emi.enabled = true;
        }
        Owner = null;
    }
}
