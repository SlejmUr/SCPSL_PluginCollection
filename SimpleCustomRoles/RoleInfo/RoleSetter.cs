using Exiled.API.Extensions;
using Exiled.API.Features;
using MEC;
using System.Linq;

namespace SimpleCustomRoles.RoleInfo
{
    public class RoleSetter
    {
        public static void SetFromCMD(Player player, CustomRoleInfo customRoleInfo)
        {
            if (Main.Instance.PlayerCustomRole.ContainsKey(player.UserId))
            {
                Log.Debug("player removed " + player.UserId);
                Main.Instance.PlayerCustomRole.Remove(player.UserId);
            }
            SetCustomInfoToPlayer(player, customRoleInfo);
        }
        public static void SetCustomInfoToPlayer(Player player, CustomRoleInfo customRoleInfo)
        {
            if (Main.Instance.Config.Debug)
                Log.Info("SetCustomInfoToPlayer: " + player.UserId + " Role: " + customRoleInfo.RoleName);
            if (Main.Instance.PlayerCustomRole.ContainsKey(player.UserId))
            {
                return;
            }
            if (customRoleInfo.Location.UseDefault)
            {
                player.Role.Set(customRoleInfo.RoleToSpawnAs, Exiled.API.Enums.SpawnReason.ForceClass, PlayerRoles.RoleSpawnFlags.UseSpawnpoint);
            }
            else
            {
                player.Role.Set(customRoleInfo.RoleToSpawnAs, Exiled.API.Enums.SpawnReason.ForceClass, PlayerRoles.RoleSpawnFlags.None);
                switch (customRoleInfo.Location.LocationSpawnPriority)
                {
                    case LocationSpawnPriority.None:
                        break;
                    case LocationSpawnPriority.SpawnZone:
                        if (customRoleInfo.Location.SpawnZones.Count > 0)
                        {
                            var tp = Room.List.Where(x => customRoleInfo.Location.SpawnZones.Contains(x.Zone)).GetRandomValue();
                            player.Teleport(tp.AdjustRoomPosition() + customRoleInfo.Location.OffsetPosition.ConvertFromV3());
                        }
                        break;
                    case LocationSpawnPriority.SpawnRoom:
                        if (customRoleInfo.Location.SpawnRooms.Count > 0)
                        {
                            var tp = Room.List.Where(x => customRoleInfo.Location.SpawnRooms.Contains(x.Type)).GetRandomValue();
                            player.Teleport(tp.AdjustRoomPosition() + customRoleInfo.Location.OffsetPosition.ConvertFromV3());
                        }
                        break;
                    case LocationSpawnPriority.ExactPosition:
                        if (customRoleInfo.Location.ExactPosition.ConvertFromV3() != new V3(0, 0, 0).ConvertFromV3())
                        {
                            player.Teleport(customRoleInfo.Location.ExactPosition.ConvertFromV3());
                        }
                        break;
                    case LocationSpawnPriority.FullRandom:
                        player.Teleport(Room.List.GetRandomValue().AdjustRoomPosition() + customRoleInfo.Location.OffsetPosition.ConvertFromV3());
                        break;
                    default:
                        break;
                }
            }
            //Inventory and ammo
            player.ClearInventory(true);
            player.ResetInventory(customRoleInfo.InventoryItems);
            player.ClearAmmo();
            foreach (var ammo in customRoleInfo.Ammos)
            {
                player.SetAmmo(ammo.Key, ammo.Value);
            }

            // Custom Item
            foreach (var item in customRoleInfo.CustomItemIds)
            {
                var id = Exiled.CustomItems.API.Features.CustomItem.Get(item);
                id.Give(player);
            }

            //  HealthMod
            player.MaxHealth += customRoleInfo.HealthModifiers.Health;
            player.Health += customRoleInfo.HealthModifiers.Health;
            if (player.IsScp)
            {
                player.HumeShield += customRoleInfo.HealthModifiers.HumeShield;
            }
            if (player.IsHuman)
            {
                player.MaxArtificialHealth += customRoleInfo.HealthModifiers.Ahp;
                player.ArtificialHealth += customRoleInfo.HealthModifiers.Ahp;
            }

            //  HealthSet
            if (customRoleInfo.HealthReplacer.UseReplace)
            {
                player.MaxHealth = customRoleInfo.HealthReplacer.Health;
                player.Health = customRoleInfo.HealthReplacer.Health;
                if (player.IsScp)
                {
                    player.HumeShield = customRoleInfo.HealthReplacer.HumeShield;
                }
                if (player.IsHuman)
                {
                    player.MaxArtificialHealth = customRoleInfo.HealthReplacer.Ahp;
                    player.ArtificialHealth = customRoleInfo.HealthReplacer.Ahp;
                }
            }

            //  Effect
            foreach (var effect in customRoleInfo.Effects)
            {
                Timing.CallDelayed(3f, () =>
                {
                    //Make it do the stuff
                    Log.Info($"Effect {effect.EffectType.ToString()}: IsSet? " + player.EnableEffect(effect.EffectType, effect.Intensity, effect.Duration));
                });
            }
            //  Scale
            if (customRoleInfo.Advanced.Scale.ConvertFromV3() != new V3(0, 0, 0).ConvertFromV3())
            {
                Timing.CallDelayed(2.5f, () =>
                {
                    player.Scale = customRoleInfo.Advanced.Scale.ConvertFromV3();
                });
            }
            //  HINT
            bool alreadyhaveinfo = false;
            if (customRoleInfo.Hint.SpawnBroadcast != string.Empty && customRoleInfo.Hint.SpawnHint != string.Empty)
            {
                alreadyhaveinfo = true;
                Exiled.API.Features.Broadcast broadcast = new Exiled.API.Features.Broadcast(customRoleInfo.Hint.SpawnBroadcast, customRoleInfo.Hint.SpawnBroadcastDuration);
                player.Broadcast(broadcast, true);
                player.ShowHint(customRoleInfo.Hint.SpawnHint, customRoleInfo.Hint.SpawnHintDuration);

            }
            if (customRoleInfo.Hint.SpawnBroadcast != string.Empty && !alreadyhaveinfo)
            {
                Exiled.API.Features.Broadcast broadcast = new Exiled.API.Features.Broadcast(customRoleInfo.Hint.SpawnBroadcast, customRoleInfo.Hint.SpawnBroadcastDuration);
                player.Broadcast(broadcast, true);
            }
            if (customRoleInfo.Hint.SpawnHint != string.Empty)
            {
                player.ShowHint(customRoleInfo.Hint.SpawnHint, customRoleInfo.Hint.SpawnHintDuration);
            }

            //  Appearance
            if (customRoleInfo.Advanced.RoleAppearance != PlayerRoles.RoleTypeId.None)
            {
                Timing.CallDelayed(2.5f, () =>
                {
                    player.ChangeAppearance(customRoleInfo.Advanced.RoleAppearance);
                    Log.Debug("Role Appearance should have changed!");
                });
            }


            //Candy
            if (customRoleInfo.Advanced.Candy.CandiesToGive.Count != 0)
            {
                foreach (var item in customRoleInfo.Advanced.Candy.CandiesToGive)
                {
                    if (item != InventorySystem.Items.Usables.Scp330.CandyKindID.None)
                    {
                        player.TryAddCandy(item);
                    }
                }
            }

            player.IsBypassModeEnabled = customRoleInfo.Advanced.BypassEnabled;

            Main.Instance.PlayerCustomRole.Add(player.UserId, customRoleInfo);
        }
    }
}
