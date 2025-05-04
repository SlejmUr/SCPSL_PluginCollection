using InventorySystem;
using LabApi.Features.Stores;
using LabApi.Features.Wrappers;
using LabApiExtensions.Extensions;
using MEC;
using PlayerRoles.FirstPersonControl;
using PlayerRoles.PlayableScps.Scp106;
using PlayerRoles.PlayableScps.Scp1507;
using PlayerStatsSystem;
using SimpleCustomRoles.Helpers;
using SimpleCustomRoles.RoleYaml;
using SimpleCustomRoles.RoleYaml.Enums;
using UnityEngine;

namespace SimpleCustomRoles.RoleInfo;

public class CustomRoleInfoStorage(Player owner) : CustomDataStore(owner)
{
    public CustomRoleBaseInfo Role;
    public string OldCustomInfo = string.Empty;
    public bool DontResetRole { get; set; }
    public void Apply()
    {
        if (Role == null)
            return;
        SpawnToPostion();
        SetInventory();
        SetStats();
        SetCommon();
        SetFpc();
        SetHints();
        SetCustomInfo();
        Timing.CallDelayed(4f, () =>
        {
            SetExtraFpc();
            SetScpRoleInfos();
        });
    }

    public void Reset()
    {
        Role = null;
        Owner.IsBypassEnabled = false;
        //ScaleHelper.SetScale(Owner, Vector3.one);
        Owner.Position += Vector3.up;
        if (!DontResetRole)
        {
            Owner.SetRole(Owner.Role, PlayerRoles.RoleChangeReason.RemoteAdmin, PlayerRoles.RoleSpawnFlags.All);
            Owner.ChangeAppearance(Owner.Role);
            AppearanceSyncExtension.RemovePlayer(Owner);
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
            if (Role.Inventory.Clear)
                Owner.ClearInventory(true, true);
            foreach (var item in Role.Inventory.Items)
            {
                Owner.Inventory.ServerAddItem(item, InventorySystem.Items.ItemAddReason.StartingItem);
            }
            foreach (var ammo in Role.Inventory.Ammos)
            {
                Owner.SetAmmo(ammo.Key, ammo.Value);
            }

            if (Role.Candy.Candies.Count != 0)
            {
                if (Owner.Items.Any(x => x is Scp330Item))
                {
                    Scp330Item bag = (Scp330Item)Owner.Items.FirstOrDefault(x => x is Scp330Item);
                    bag.AddCandies(Role.Candy.Candies);
                }
                else if (!Owner.IsInventoryFull)
                {
                    Scp330Item bag = (Scp330Item)Owner.AddItem(ItemType.SCP330, InventorySystem.Items.ItemAddReason.StartingItem);
                    bag.AddCandies(Role.Candy.Candies);
                }
            }
            if (Main.Instance.Config.CustomItemUseName)
            {
                foreach (var item in Role.Inventory.CustomNames)
                {
                    Server.RunCommand(string.Format(Main.Instance.Config.CustomItemCommand, item, Owner.PlayerId));
                }
            }
            else
            {
                foreach (var item in Role.Inventory.CustomIds)
                {
                    Server.RunCommand(string.Format(Main.Instance.Config.CustomItemCommand, item, Owner.PlayerId));
                }
            }
            
        });
    }

    private void SetStats()
    {
        Owner.MaxHealth = Role.Stats.MaxHealth.Math.MathWithFloat(Owner.MaxHealth, Role.Stats.MaxHealth.Value);
        Owner.Health = Role.Stats.Health.Math.MathWithFloat(Owner.Health, Role.Stats.Health.Value);
        Owner.MaxArtificialHealth = Role.Stats.MaxAhp.Math.MathWithFloat(Owner.MaxArtificialHealth, Role.Stats.MaxAhp.Value);
        Owner.ArtificialHealth = Role.Stats.Ahp.Math.MathWithFloat(Owner.ArtificialHealth, Role.Stats.Ahp.Value);
        Owner.MaxHumeShield = Role.Stats.MaxHumeShield.Math.MathWithFloat(Owner.MaxHumeShield, Role.Stats.MaxHumeShield.Value);
        Owner.HumeShield = Role.Stats.HumeShield.Math.MathWithFloat(Owner.HumeShield, Role.Stats.HumeShield.Value);
        var max = Owner.ReferenceHub.playerStats.GetModule<StaminaStat>().MaxValue;
        max = Role.Stats.MaxStamina.Math.MathWithFloat(max, Role.Stats.MaxStamina.Value);
        Owner.ReferenceHub.playerStats.GetModule<StaminaStat>().MaxValue = max;
        Owner.Gravity = Role.Stats.Gravity;
    }

    private void SetCommon()
    {
        foreach (var effect in Role.Effects)
        {
            // this time seems good I guess.
            Timing.CallDelayed(3f, () =>
            {
                Owner.EnableEffect(effect.EffectName, effect.Intensity, effect.Duration);
            });
        }
        Owner.IsBypassEnabled = Role.Extra.Bypass;
        Timing.CallDelayed(3f, () =>
        {
            if (Owner.Room == null)
                return;
            foreach (var door in Owner.Room.Doors)
            {
                door.IsOpened = true;
            }
        });
    }

    private void SetFpc()
    {
        // Scale
        /*
        if (Role.Fpc.Scale != Vector3.one)
        {
            Timing.CallDelayed(3.5f, () =>
            {
                ScaleHelper.SetScale(Owner, Role.Fpc.Scale);
            });
        }
        */
        // Todo: FakeScale

        //  Appearance
        if (Role.Fpc.Appearance != PlayerRoles.RoleTypeId.None)
        {
            Timing.CallDelayed(3.5f, () =>
            {
                AppearanceSyncExtension.AddPlayer(Owner, Role.Fpc.Appearance);
            });
        }
        // Voice Channel
        if (Role.Fpc.VoiceChatChannel != VoiceChat.VoiceChatChannel.None)
        {
            Timing.CallDelayed(3.5f, () =>
            {
                if (Owner.VoiceModule != null)
                    Owner.VoiceModule.CurrentChannel = Role.Fpc.VoiceChatChannel;
            });
        }
    }

    private void SetExtraFpc()
    {
        if (Owner.RoleBase is not FpcStandardRoleBase fpcStandardRoleBase)
            return;
        fpcStandardRoleBase.FpcModule.FallDamageSettings.Enabled = Role.FallDamage.Enabled;

        Role.FallDamage.Absolute.MathWithValue(ref fpcStandardRoleBase.FpcModule.FallDamageSettings.Absolute);
        Role.FallDamage.ImmunityTime.MathWithValue(ref fpcStandardRoleBase.FpcModule.FallDamageSettings.ImmunityTime);
        Role.FallDamage.MinVelocity.MathWithValue(ref fpcStandardRoleBase.FpcModule.FallDamageSettings.MinVelocity);
        Role.FallDamage.Power.MathWithValue(ref fpcStandardRoleBase.FpcModule.FallDamageSettings.Power);
        Role.FallDamage.MaxDamage.MathWithValue(ref fpcStandardRoleBase.FpcModule.FallDamageSettings.MaxDamage);
        Role.FallDamage.Multiplier.MathWithValue(ref fpcStandardRoleBase.FpcModule.FallDamageSettings.Multiplier);

        Role.Movement.CrouchSpeed.MathWithValue(ref fpcStandardRoleBase.FpcModule.CrouchSpeed);
        Role.Movement.JumpSpeed.MathWithValue(ref fpcStandardRoleBase.FpcModule.JumpSpeed);
        Role.Movement.SneakSpeed.MathWithValue(ref fpcStandardRoleBase.FpcModule.SneakSpeed);
        Role.Movement.SprintSpeed.MathWithValue(ref fpcStandardRoleBase.FpcModule.SprintSpeed);
        Role.Movement.WalkSpeed.MathWithValue(ref fpcStandardRoleBase.FpcModule.WalkSpeed);
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

        if (Role.Display.RoleCanDisplay)
            Owner.CustomInfo += $"{Role.Display.AreaRoleName}";
    }

    private void SetScpRoleInfos()
    {
        if (Owner.RoleBase is Scp106Role scp106Role && scp106Role != null)
        {
            if (scp106Role.SubroutineModule.TryGetSubroutine(out Scp106Attack scp106Attack))
            {
                Role.Scp.Scp106.AttackHitCooldown.MathWithValue(ref scp106Attack._hitCooldown);
                Role.Scp.Scp106.AttackMissCooldown.MathWithValue(ref scp106Attack._missCooldown);
                Role.Scp.Scp106.AttackDamage.MathWithValue(ref scp106Attack._damage);
            }
        }
        if (Owner.RoleBase is Scp1507Role scp1507Role && scp1507Role != null)
        {
            if (scp1507Role.SubroutineModule.TryGetSubroutine(out Scp1507AttackAbility attackAbility))
            {
                Role.Scp.Scp1507.AttackDamage.MathWithValue(ref attackAbility._damage);
            }
        }
    }
}