/*
using Exiled.CustomRoles.API.Features;
using PlayerRoles;
using Exiled.API.Features.Attributes;
using Exiled.Events.EventArgs.Player;
using Exiled.API.Features.Items;
using UnityEngine;


namespace SimpleCustomRoles
{
    [CustomRole(RoleTypeId.Scp0492)]
    public class Bomber_Zombie : CustomRole
    {
        public override uint Id { get; set; } = 0x56A2EB95; // Bomber_Zombie CRC
        public override int MaxHealth { get; set; } = 400;
        public override string Name { get; set; } = "BomberZombie";
        public override string Description { get; set; } = "On-Death Exploding zombie.";
        public override string CustomInfo { get; set; } = string.Empty;

        public override RoleTypeId Role { get; set; } = RoleTypeId.Scp0492;

        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Dying += this.Died;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Dying -= this.Died;
            base.UnsubscribeEvents();
        }

        private void Died(DyingEventArgs e)
        {
            if (e.Player == null) return;
            if (this.Check(e.Player))
            {
                ExplosiveGrenade grenade = (ExplosiveGrenade)Item.Create(ItemType.GrenadeHE);
                grenade.FuseTime = 2.0f;
                grenade.SpawnActive(e.Player.Position + Vector3.up);
                return;
            }
        }
    }
}
*/