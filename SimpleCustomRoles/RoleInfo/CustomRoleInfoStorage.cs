using InventorySystem;
using LabApi.Features.Stores;
using LabApi.Features.Wrappers;
using MEC;
using SimpleCustomRoles.Helpers;
using UnityEngine;

namespace SimpleCustomRoles.RoleInfo;

public class CustomRoleInfoStorage(Player owner) : CustomDataStore(owner)
{
    public CustomRoleInfo Role;
    public string OldCustomInfo = string.Empty;
    public bool DontResetRole;
    public void Apply()
    {
        if (Role == null)
            return;
        SpawnToPostion();
        SetInventory();
        SetStats();
        SetCommon();
        SetAdvanced();
        SetHints();
        SetCustomInfo();
        SendCommand();
    }

    public void Reset()
    {
        Role = null;
        Owner.IsBypassEnabled = false;
        ScaleHelper.SetScale(Owner, Vector3.one);
        Owner.Position += Vector3.up;
        if (!DontResetRole)
        {
            Owner.SetRole(Owner.Role, PlayerRoles.RoleChangeReason.RemoteAdmin, PlayerRoles.RoleSpawnFlags.All);
            Owner.ChangeAppearance(Owner.Role);
        }

        if (string.IsNullOrEmpty(OldCustomInfo))
            OldCustomInfo = string.Empty;
        Owner.CustomInfo = OldCustomInfo;
    }

    public override void OnInstanceDestroyed()
    {
        Reset();
    }

    private void SpawnToPostion()
    {
        if (Role.Location.UseDefault)
        {
            if (Owner.Role != Role.RoleToSpawn)
                Owner.SetRole(Role.RoleToSpawn, PlayerRoles.RoleChangeReason.RemoteAdmin, PlayerRoles.RoleSpawnFlags.UseSpawnpoint);
        }
        else
        {
            if (Owner.Role != Role.RoleToSpawn)
                Owner.SetRole(Role.RoleToSpawn, PlayerRoles.RoleChangeReason.RemoteAdmin, PlayerRoles.RoleSpawnFlags.None);
            MoveToLocation();
        }
    }

    private void MoveToLocation()
    {
        Timing.CallDelayed(1, ()=>
        {
            switch (Role.Location.Priority)
            {
                case LocationSpawnPriority.None:
                    break;
                case LocationSpawnPriority.SpawnZone:
                    if (Role.Location.SpawnZones.Count > 0)
                    {
                        var tp = Room.Get(Role.Location.SpawnZones.RandomItem()).Where(x => !Role.Location.ExludeRooms.Contains(x.Name)).ToList().RandomItem();
                        Owner.Position = tp.AdjustRoomPosition() + Role.Location.OffsetPosition;
                    }
                    break;
                case LocationSpawnPriority.SpawnRoom:
                    if (Role.Location.SpawnRooms.Count > 0)
                    {
                        var tp = Room.Get(Role.Location.SpawnRooms.RandomItem()).ToList().RandomItem();
                        Owner.Position = tp.AdjustRoomPosition() + Role.Location.OffsetPosition;
                    }
                    break;
                case LocationSpawnPriority.ExactPosition:
                    if (Role.Location.ExactPosition != Vector3.zero)
                    {
                        Owner.Position = Role.Location.ExactPosition;
                    }
                    break;
                case LocationSpawnPriority.FullRandom:
                    Owner.Position = Room.List.ToList().RandomItem().AdjustRoomPosition() + Role.Location.OffsetPosition;
                    break;
                default:
                    break;
            }
        });
    }

    private void SetInventory()
    {
        Timing.CallDelayed(2f, () =>
        {
            Owner.ClearInventory(true, true);
            foreach (var item in Role.Inventory.Items)
            {
                Owner.Inventory.ServerAddItem(item, InventorySystem.Items.ItemAddReason.StartingItem);
            }
            Owner.ClearAmmo();
            foreach (var ammo in Role.Inventory.Ammos)
            {
                Owner.SetAmmo(ammo.Key, ammo.Value);
            }
        });
    }

    private void SetStats()
    {
        Owner.MaxHealth = Role.Health.Health.Math.MathWithFloat(Owner.MaxHealth, Role.Health.Health.Value);
        Owner.Health = Role.Health.Health.Math.MathWithFloat(Owner.Health, Role.Health.Health.Value);
        if (Owner.IsSCP)
        {
            Owner.HumeShield = Role.Health.HumeShield.Math.MathWithFloat(Owner.HumeShield, Role.Health.HumeShield.Value);
        }
        if (Owner.IsHuman)
        {
            Owner.MaxArtificialHealth = Role.Health.Ahp.Math.MathWithFloat(Owner.MaxArtificialHealth, Role.Health.Ahp.Value);
            Owner.ArtificialHealth = Role.Health.Ahp.Math.MathWithFloat(Owner.ArtificialHealth, Role.Health.Ahp.Value);
        }
    }

    private void SetCommon()
    {
        foreach (var effect in Role.Effects)
        {
            // this time seems good I guess.
            Timing.CallDelayed(3f, () =>
            {
                Owner.EnableEffect(EffectHelper.GetEffectFromName(Owner, effect.EffectName), effect.Intensity, effect.Duration);
            });
        }
        ScaleHelper.SetScale(Owner, Vector3.one);
    }

    private void SetAdvanced()
    {
        if (Role.Advanced.Scale != Vector3.one)
        {
            Timing.CallDelayed(2.5f, () =>
            {
                ScaleHelper.SetScale(Owner, Role.Advanced.Scale);
            });
        }
        //  Appearance
        if (Role.Advanced.Appearance != PlayerRoles.RoleTypeId.None)
        {
            Timing.CallDelayed(2.5f, () =>
            {
                Owner.ChangeAppearance(Role.Advanced.Appearance);
            });
        }


        //Candy
        if (Role.Advanced.Candy.Candies.Count != 0)
        {
            foreach (var item in Role.Advanced.Candy.Candies)
            {
                if (item != InventorySystem.Items.Usables.Scp330.CandyKindID.None)
                {
                    Owner.ReferenceHub.GrantCandy(item, InventorySystem.Items.ItemAddReason.AdminCommand);
                }
            }
        }

        Owner.IsBypassEnabled = Role.Advanced.Bypass;

        if (Role.Advanced.OpenDoorsNextToSpawn)
        {
            Timing.CallDelayed(2.5f, () =>
            {
                if (Owner.Room == null)
                    return;
                foreach (var door in Owner.Room!.Doors)
                {
                    door.IsOpened = true;
                }
            });
        }

        foreach (var item in Role.Advanced.FriendlyFire)
        {
            // player.SetCustomRoleFriendlyFire(customRoleInfo.RoleName, item.RoleType, item.Value);
        }

    }

    private void SetHints()
    {
        bool alreadyhaveinfo = false;
        if (Role.Hint.Broadcast != string.Empty && Role.Hint.Hint != string.Empty)
        {
            alreadyhaveinfo = true;
            Server.SendBroadcast(Owner, Role.Hint.Broadcast, Role.Hint.BroadcastDuration);
            Owner.SendHint(Role.Hint.Hint, Role.Hint.HintDuration);

        }
        if (Role.Hint.Broadcast != string.Empty && !alreadyhaveinfo)
        {
            Server.SendBroadcast(Owner, Role.Hint.Broadcast, Role.Hint.BroadcastDuration);
        }
        if (Role.Hint.Hint != string.Empty)
        {
            Owner.SendHint(Role.Hint.Hint, Role.Hint.HintDuration);
        }
        if (Role.Hint.BroadcastAll != string.Empty)
        {
            Timing.CallDelayed(Role.Hint.BroadcastDuration + 0.5f, () =>
            {
                Server.SendBroadcast(Role.Hint.BroadcastAll, Role.Hint.BroadcastAllDuration);
            });
        }
    }

    private void SetCustomInfo()
    {
        OldCustomInfo = string.Empty;
        if (!string.IsNullOrEmpty(Owner.CustomInfo))
            OldCustomInfo = Owner.CustomInfo;

        if (Role.CanDisplay)
            Owner.CustomInfo += $"{Role.DisplayRolename}";
    }

    private void SendCommand()
    {
        if (!string.IsNullOrEmpty(Role.Events.OnSpawned))
        {
            // Call event
            Server.RunCommand($"{Role.Events.OnSpawned} {Owner.PlayerId} {Role.Rolename}");
        }
    }
}