using InventorySystem;
using LabApi.Features.Wrappers;
using MEC;
using SimpleCustomRoles.Helpers;

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
        if (Main.Instance.PlayerCustomRole.ContainsKey(player.UserId))
        {
            CL.Debug("Player removed " + player.UserId);
            Main.Instance.PlayerCustomRole.Remove(player.UserId);
        }
        SetCustomInfoToPlayer(player, customRoleInfo);
    }
    public static void SetCustomInfoToPlayer(Player player, CustomRoleInfo customRoleInfo)
    {
        if (Main.Instance.Config.Debug)
            CL.Info("SetCustomInfoToPlayer: " + player.UserId + " Role: " + customRoleInfo.RoleName);
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
        // player. = customRoleInfo.RoleName;
        if (customRoleInfo.Location.UseDefault)
        {
            if (player.Role != customRoleInfo.RoleToSpawnAs)
                player.SetRole(customRoleInfo.RoleToSpawnAs, PlayerRoles.RoleChangeReason.RemoteAdmin, PlayerRoles.RoleSpawnFlags.UseSpawnpoint);
        }
        else
        {
            if (player.Role != customRoleInfo.RoleToSpawnAs)
                player.SetRole(customRoleInfo.RoleToSpawnAs, PlayerRoles.RoleChangeReason.RemoteAdmin, PlayerRoles.RoleSpawnFlags.None);
            switch (customRoleInfo.Location.LocationSpawnPriority)
            {
                case LocationSpawnPriority.None:
                    break;
                case LocationSpawnPriority.SpawnZone:
                    if (customRoleInfo.Location.SpawnZones.Count > 0)
                    {
                        var tp = Room.Get(customRoleInfo.Location.SpawnZones.RandomItem()).ToList().RandomItem();
                        player.Position = tp.AdjustRoomPosition() + customRoleInfo.Location.OffsetPosition.ConvertFromV3();
                    }
                    break;
                case LocationSpawnPriority.SpawnRoom:
                    if (customRoleInfo.Location.SpawnRooms.Count > 0)
                    {
                        var tp = Room.Get(customRoleInfo.Location.SpawnRooms.RandomItem()).ToList().RandomItem();
                        player.Position = tp.AdjustRoomPosition() + customRoleInfo.Location.OffsetPosition.ConvertFromV3();
                    }
                    break;
                case LocationSpawnPriority.ExactPosition:
                    if (customRoleInfo.Location.ExactPosition.ConvertFromV3() != new V3(0, 0, 0).ConvertFromV3())
                    {
                        player.Position = customRoleInfo.Location.ExactPosition.ConvertFromV3();
                    }
                    break;
                case LocationSpawnPriority.FullRandom:
                    player.Position = Room.List.ToList().RandomItem().AdjustRoomPosition() + customRoleInfo.Location.OffsetPosition.ConvertFromV3();
                    break;
                default:
                    break;
            }
        }
        //Inventory and ammo
        Timing.CallDelayed(2f, () =>
        {
            player.ClearInventory(true, true);
            foreach (var item in customRoleInfo.Inventory.InventoryItems)
            {
                player.Inventory.ServerAddItem(item, InventorySystem.Items.ItemAddReason.StartingItem);
            }
            player.ClearAmmo();
            foreach (var ammo in customRoleInfo.Inventory.Ammos)
            {
                player.SetAmmo(ammo.Key, ammo.Value);
            }
        });
        

        // Custom Item
        foreach (var item in customRoleInfo.Inventory.CustomItemIds)
        {
            // Custom items currently not exists
            return;
        }

        //  Health
        player.MaxHealth = MathWithFloat(customRoleInfo.Health.Health.SetType, player.MaxHealth, customRoleInfo.Health.Health.Value);
        player.Health = MathWithFloat(customRoleInfo.Health.Health.SetType, player.Health, customRoleInfo.Health.Health.Value);
        if (player.IsSCP)
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
                player.EnableEffect(EffectHelper.GetEffectFromName(player, effect.EffectTypeName), effect.Intensity, effect.Duration);
            });
        }
        ScaleHelper.SetScale(player, new V3(1).ConvertFromV3());
        //  Scale
        if (customRoleInfo.Advanced.Scale.ConvertFromV3() != new V3().ConvertFromV3())
        {
            Timing.CallDelayed(2.5f, () =>
            {
                ScaleHelper.SetScale(player, customRoleInfo.Advanced.Scale.ConvertFromV3());
            });
        }
        //  HINT
        bool alreadyhaveinfo = false;
        if (customRoleInfo.Hint.SpawnBroadcast != string.Empty && customRoleInfo.Hint.SpawnHint != string.Empty)
        {
            alreadyhaveinfo = true;
            Server.SendBroadcast(player, customRoleInfo.Hint.SpawnBroadcast, customRoleInfo.Hint.SpawnBroadcastDuration);
            player.SendHint(customRoleInfo.Hint.SpawnHint, customRoleInfo.Hint.SpawnHintDuration);

        }
        if (customRoleInfo.Hint.SpawnBroadcast != string.Empty && !alreadyhaveinfo)
        {
            Server.SendBroadcast(player, customRoleInfo.Hint.SpawnBroadcast, customRoleInfo.Hint.SpawnBroadcastDuration);
        }
        if (customRoleInfo.Hint.SpawnHint != string.Empty)
        {
            player.SendHint(customRoleInfo.Hint.SpawnHint, customRoleInfo.Hint.SpawnHintDuration);
        }
        if (customRoleInfo.Hint.SpawnBroadcastToAll != string.Empty)
        {
            Timing.CallDelayed(customRoleInfo.Hint.SpawnBroadcastDuration + 0.5f, () =>
            {
                Server.SendBroadcast(customRoleInfo.Hint.SpawnBroadcastToAll, customRoleInfo.Hint.SpawnBroadcastToAllDuration);
            });
        }

        //  Appearance
        if (customRoleInfo.Advanced.RoleAppearance != PlayerRoles.RoleTypeId.None)
        {
            Timing.CallDelayed(2.5f, () =>
            {
                player.ChangeAppearance(customRoleInfo.Advanced.RoleAppearance);
                CL.Debug("Role Appearance should have changed!");
            });
        }


        //Candy
        if (customRoleInfo.Advanced.Candy.CandiesToGive.Count != 0)
        {
            foreach (var item in customRoleInfo.Advanced.Candy.CandiesToGive)
            {
                if (item != InventorySystem.Items.Usables.Scp330.CandyKindID.None)
                {
                    player.ReferenceHub.GrantCandy(item, InventorySystem.Items.ItemAddReason.AdminCommand);
                }
            }
        }

        player.IsBypassEnabled = customRoleInfo.Advanced.BypassModeEnabled;

        if (customRoleInfo.Advanced.OpenDoorsNextToSpawn)
        {
            Timing.CallDelayed(2.5f, () =>
            {
                if (player.Room == null)
                    return;
                foreach (var door in player.Room!.Doors)
                {
                    door.IsOpened = true;
                }
            });
        }

        foreach (var item in customRoleInfo.Advanced.FriendlyFire)
        {
            // player.SetCustomRoleFriendlyFire(customRoleInfo.RoleName, item.RoleType, item.Value);
        }

        if (!string.IsNullOrEmpty(customRoleInfo.EventCaller.OnSpawned))
        {
            // Call event
            Server.RunCommand($"{customRoleInfo.EventCaller.OnSpawned} {player.PlayerId} {customRoleInfo.RoleName}");
        }

        Main.Instance.PlayerCustomRole.Add(player.UserId, customRoleInfo);

        if (Main.Instance.Config.Debug)
            CL.Info("SetCustomInfoToPlayer: " + player.UserId + " Role: " + customRoleInfo.RoleName + " Success");
    }

    public static void UnSetCustomInfoToPlayer(Player player)
    {
        if (!Main.Instance.PlayerCustomRole.ContainsKey(player.UserId))
        {
            return;
        }
        //player.UniqueRole = string.Empty;
        player.IsBypassEnabled = false;
        ScaleHelper.SetScale(player, new V3(1).ConvertFromV3());
        player.SetRole(player.Role, PlayerRoles.RoleChangeReason.LateJoin, PlayerRoles.RoleSpawnFlags.All);
        Main.Instance.PlayerCustomRole.Remove(player.UserId);
    }

}
