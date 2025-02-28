using Exiled.API.Extensions;
using Exiled.API.Features;
using MEC;
using System.Collections.Generic;
using System.Linq;

namespace SimpleCustomRoles.RoleInfo;

public class RoleSetter
{
    public static float MathWithFloat(MathOption mathOption, float inFloat, float myValue)
    {
        return mathOption switch
        {
            MathOption.None => inFloat,
            MathOption.Set => myValue,
            MathOption.Add => inFloat + myValue,
            MathOption.Subtract => inFloat - myValue,
            MathOption.Multiply => inFloat * myValue,
            MathOption.Divide => inFloat / myValue,
            _ => inFloat,
        };
    }

    public static int MathWithInt(MathOption mathOption, int inInt, int myValue)
    {
        return mathOption switch
        {
            MathOption.None => inInt,
            MathOption.Set => myValue,
            MathOption.Add => inInt + myValue,
            MathOption.Subtract => inInt - myValue,
            MathOption.Multiply => inInt * myValue,
            MathOption.Divide => inInt / myValue,
            _ => inInt,
        };
    }

    public static void SetFromCMD(Player player, CustomRoleInfo customRoleInfo)
    {
        UnSetCustomInfoToPlayer(player);
        Timing.CallDelayed(0.5f, ()=> { SetCustomInfoToPlayer(player, customRoleInfo); });

    }

    static Dictionary<string, string> UserIdToOldCustomInfo = [];

    public static void SetCustomInfoToPlayer(Player player, CustomRoleInfo customRoleInfo)
    {
        if (Main.Instance.Config.Debug)
            Log.Info("SetCustomInfoToPlayer: " + player.UserId + " Role: " + customRoleInfo.RoleName);
        if (Main.Instance.PlayerCustomRole.ContainsKey(player.UserId))
        {
            return;
        }
        /*
        var tmp_max_health = player.MaxHealth;
        var tmp_health = player.Health;
        var tmp_hume = player.HumeShield;
        var tmp_max_ahp = player.MaxArtificialHealth;
        var tmp_ahp = player.ArtificialHealth
        */
        player.UniqueRole = customRoleInfo.RoleName;
        if (customRoleInfo.Location.UseDefault)
        {
            if (player.Role.Type != customRoleInfo.RoleToSpawnAs)
                player.Role.Set(customRoleInfo.RoleToSpawnAs, Exiled.API.Enums.SpawnReason.ForceClass, PlayerRoles.RoleSpawnFlags.UseSpawnpoint);
        }
        else
        {
            if (player.Role.Type != customRoleInfo.RoleToSpawnAs)
                player.Role.Set(customRoleInfo.RoleToSpawnAs, Exiled.API.Enums.SpawnReason.ForceClass, PlayerRoles.RoleSpawnFlags.None);
            switch (customRoleInfo.Location.LocationSpawnPriority)
            {
                case LocationSpawnPriority.None:
                    break;
                case LocationSpawnPriority.SpawnZone:
                    if (customRoleInfo.Location.SpawnZones.Count > 0)
                    {
                        var tp = Room.Get(customRoleInfo.Location.SpawnZones.RandomItem()).Where(x => !customRoleInfo.Location.ExludeSpawnRooms.Contains(x.Type)).ToList().RandomItem();
                        player.Teleport(tp.AdjustRoomPosition() + customRoleInfo.Location.OffsetPosition.ConvertFromV3());
                    }
                    break;
                case LocationSpawnPriority.SpawnRoom:
                    if (customRoleInfo.Location.SpawnRooms.Count > 0)
                    {
                        var tp = Room.Get(x => customRoleInfo.Location.SpawnRooms.Contains(x.Type)).GetRandomValue();
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
        player.ResetInventory(customRoleInfo.Inventory.InventoryItems);
        player.ClearAmmo();
        foreach (var ammo in customRoleInfo.Inventory.Ammos)
        {
            player.SetAmmo(ammo.Key, ammo.Value);
        }

        // Custom Item
        foreach (var item in customRoleInfo.Inventory.CustomItemIds)
        {
            var id = Exiled.CustomItems.API.Features.CustomItem.Get(item);
            id.Give(player);
        }

        //  Health
        player.MaxHealth = MathWithFloat(customRoleInfo.Health.Health.SetType, player.MaxHealth, customRoleInfo.Health.Health.Value);
        player.Health = MathWithFloat(customRoleInfo.Health.Health.SetType, player.Health, customRoleInfo.Health.Health.Value);
        if (player.IsScp)
        {
            player.HumeShield = MathWithFloat(customRoleInfo.Health.HumeShield.SetType, player.HumeShield, customRoleInfo.Health.HumeShield.Value);
        }
        if (player.IsHuman)
        {
            player.MaxArtificialHealth = MathWithFloat(customRoleInfo.Health.Ahp.SetType, player.MaxArtificialHealth, customRoleInfo.Health.Ahp.Value);
            player.ArtificialHealth = MathWithFloat(customRoleInfo.Health.Ahp.SetType, player.ArtificialHealth, customRoleInfo.Health.Ahp.Value);
        }

        //  Effect
        foreach (var effect in customRoleInfo.Effects)
        {
            // this time seems good I guess.
            Timing.CallDelayed(3f, () =>
            {
                player.EnableEffect(effect.EffectType, effect.Intensity, effect.Duration);
            });
        }
        player.Scale = UnityEngine.Vector3.one;
        //  Scale
        if (customRoleInfo.Advanced.Scale.ConvertFromV3() != new V3().ConvertFromV3())
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
            Exiled.API.Features.Broadcast broadcast = new(customRoleInfo.Hint.SpawnBroadcast, customRoleInfo.Hint.SpawnBroadcastDuration);
            player.Broadcast(broadcast, true);
            player.ShowHint(customRoleInfo.Hint.SpawnHint, customRoleInfo.Hint.SpawnHintDuration);

        }
        if (customRoleInfo.Hint.SpawnBroadcast != string.Empty && !alreadyhaveinfo)
        {
            Exiled.API.Features.Broadcast broadcast = new(customRoleInfo.Hint.SpawnBroadcast, customRoleInfo.Hint.SpawnBroadcastDuration);
            player.Broadcast(broadcast, true);
        }
        if (customRoleInfo.Hint.SpawnHint != string.Empty)
        {
            player.ShowHint(customRoleInfo.Hint.SpawnHint, customRoleInfo.Hint.SpawnHintDuration);
        }
        if (customRoleInfo.Hint.SpawnBroadcastToAll != string.Empty)
        {
            Timing.CallDelayed(customRoleInfo.Hint.SpawnBroadcastDuration + 0.5f, () =>
            {
                Broadcast.Singleton.RpcAddElement(customRoleInfo.Hint.SpawnBroadcastToAll, customRoleInfo.Hint.SpawnBroadcastToAllDuration, Broadcast.BroadcastFlags.Normal);
            });
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

        player.IsBypassModeEnabled = customRoleInfo.Advanced.BypassModeEnabled;

        if (customRoleInfo.Advanced.OpenDoorsNextToSpawn)
        {
            Timing.CallDelayed(2.5f, () =>
            {
                var inroom = Room.List.Where(x => x.Players.Contains(player)).FirstOrDefault();
                foreach (var door in inroom.Doors)
                {
                    door.IsOpen = true;
                }
            });
        }

        foreach (var item in customRoleInfo.Advanced.FriendlyFire)
        {
            player.SetCustomRoleFriendlyFire(customRoleInfo.RoleName, item.RoleType, item.Value);
        }

        if (!string.IsNullOrEmpty(customRoleInfo.EventCaller.OnSpawned))
        {
            // Call event
            Server.ExecuteCommand($"{customRoleInfo.EventCaller.OnSpawned} {player.Id} {customRoleInfo.RoleName}");
        }
        if (UserIdToOldCustomInfo.ContainsKey(player.UserId))
            UserIdToOldCustomInfo.Remove(player.UserId);

        string custominfo = string.Empty;
        if (!string.IsNullOrEmpty(player.CustomInfo))
            custominfo = player.CustomInfo;

        UserIdToOldCustomInfo.Add(player.UserId, custominfo);

        if (customRoleInfo.RoleCanDisplay)
            player.CustomInfo += $"{customRoleInfo.DisplayRoleName}";

        Main.Instance.PlayerCustomRole.Add(player.UserId, customRoleInfo);

        if (Main.Instance.Config.Debug)
            Log.Info("SetCustomInfoToPlayer: " + player.UserId + " Role: " + customRoleInfo.RoleName + " Success");
    }

    public static void UnSetCustomInfoToPlayer(Player player, bool DontResetRole = false)
    {
        if (!Main.Instance.PlayerCustomRole.ContainsKey(player.UserId))
            return;

        player.UniqueRole = string.Empty;
        player.IsBypassModeEnabled = false;
        player.Scale = UnityEngine.Vector3.one;
        if (!DontResetRole)
        {
            player.Role.Set(player.Role.Type, Exiled.API.Enums.SpawnReason.LateJoin, PlayerRoles.RoleSpawnFlags.All);
            player.ChangeAppearance(player.Role.Type);
        }
        if (!UserIdToOldCustomInfo.TryGetValue(player.UserId, out string custominfo))
            player.CustomInfo = string.Empty;
        else
        {
            if (string.IsNullOrEmpty(custominfo))
                custominfo = string.Empty;
            player.CustomInfo = custominfo;
        }
        UserIdToOldCustomInfo.Remove(player.UserId);

        Main.Instance.PlayerCustomRole.Remove(player.UserId);
    }

}
