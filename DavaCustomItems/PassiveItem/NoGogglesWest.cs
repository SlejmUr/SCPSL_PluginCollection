using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Items;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using PlayerRoles.FirstPersonControl;

namespace DavaCustomItems.PassiveItem;

[CustomItem(ItemType.ArmorCombat)]
public class NoGogglesWest : CustomArmor
{
    public override uint Id { get; set; } = 700;
    public override string Name { get; set; } = "No Goggle West";
    public override string Description { get; set; } = "This west makes the equipped player not seen in Goggles";
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
        if (Owner == null)
            return;
        Log.Info($"who: {who} target: {target} owner: {Owner}");
        if (Owner.ReferenceHub != target)
            return;
        Timing.CallDelayed(1, () =>
        {
            var fpc = Owner.Role.Base as IFpcRole;
            if (fpc != null && CustomPlayerEffects.Scp1344.Trackers.TryGetValue(fpc, out var particle))
            {
                var emi = particle.emission;
                emi.enabled = false;
            }
            else
            {
                Log.Info("emission idk?");
            }
        });
    }

    public override void OnAcquired(Player player, Item item, bool displayMessage)
    {
        Owner = player;
        Log.Info("OnAcquired owner set!");
    }

    public override void OnDroppingItem(DroppingItemEventArgs ev)
    {
        if (ev.Player != Owner)
            return;
        if (!Check(ev.Item))
            return;
        var fpc = Owner.Role.Base as IFpcRole;
        Log.Info("aaaa " + fpc);
        if (fpc != null && CustomPlayerEffects.Scp1344.Trackers.TryGetValue(fpc, out var particle))
        {
            var emi = particle.emission;
            emi.enabled = true;
        }
        Owner = null;
    }
}
