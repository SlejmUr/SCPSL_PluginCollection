using InventorySystem;
using InventorySystem.Items.Firearms;
using InventorySystem.Items.Firearms.Modules;
using LabApi.Features.Stores;
using LabApi.Features.Wrappers;
using LabApiExtensions.Extensions;
using MEC;
using PlayerRoles.FirstPersonControl;
using PlayerRoles.PlayableScps.Scp049;
using PlayerRoles.PlayableScps.Scp106;
using PlayerRoles.PlayableScps.Scp1507;
using PlayerRoles.PlayableScps.Scp939;
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
    public bool ResetRole { get; set; }
    public void Apply()
    {
        if (Role == null)
            return;
        Timing.RunCoroutine(ApplyCor());
    }

    public void Reset()
    {
        Role = null;
        Owner.IsBypassEnabled = false;
        ScaleHelper.SetScale(Owner, Vector3.one, false, true);
        AppearanceSyncExtension.RemovePlayer(Owner, false);
        Owner.Position += Vector3.up;
        if (string.IsNullOrEmpty(OldCustomInfo))
            OldCustomInfo = string.Empty;
        Owner.CustomInfo = OldCustomInfo;
        if (ResetRole)
            Owner.SetRole(Owner.Role, PlayerRoles.RoleChangeReason.None, PlayerRoles.RoleSpawnFlags.None);
    }

    internal IEnumerator<float> ApplyCor()
    {
        yield return Timing.WaitForSeconds(0.1f);
        SpawnToPostion();
        yield return Timing.WaitForSeconds(0.1f);
        MoveToLocation();
        yield return Timing.WaitForSeconds(0.2f);
        SetInventory();
        SetHints();
        yield return Timing.WaitForSeconds(0.2f);
        SetMaxStats();
        yield return Timing.WaitForSeconds(0.1f);
        SetStats();
        yield return Timing.WaitForSeconds(0.2f);
        SetCommon();
        yield return Timing.WaitForOneFrame;
        yield return Timing.WaitForOneFrame;
        SetFpc();
        SetCustomInfo();
        SetExtraFpc();
        SetScpRoleInfos();
    }

    private void SpawnToPostion()
    {
        if (Role.RoleToSpawn == PlayerRoles.RoleTypeId.Scp0492)
            Owner.SetRole(Role.RoleToSpawn, PlayerRoles.RoleChangeReason.None, PlayerRoles.RoleSpawnFlags.None);
        else if (Owner.Role != Role.RoleToSpawn)
            Owner.SetRole(Role.RoleToSpawn, PlayerRoles.RoleChangeReason.None, PlayerRoles.RoleSpawnFlags.All);

        if (Role.Extra.ForceSet)
        {
            Owner.SetRole(PlayerRoles.RoleTypeId.Spectator, PlayerRoles.RoleChangeReason.None, PlayerRoles.RoleSpawnFlags.All);
            Timing.CallDelayed(0.2f, () => Owner.SetRole(Role.RoleToSpawn, PlayerRoles.RoleChangeReason.None, PlayerRoles.RoleSpawnFlags.All));
        }
    }

    private void MoveToLocation()
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
                    var tp = Room.Get(Role.Location.SpawnRooms.RandomItem()).ToList().FirstOrDefault();
                    Owner.Position = tp.AdjustRoomPosition() + Role.Location.OffsetPosition;
                }
                break;
            case LocationSpawnPriority.ExactPosition:
                if (Role.Location.ExactPosition != Vector3.zero)
                    Owner.Position = Role.Location.ExactPosition;
                break;
            case LocationSpawnPriority.FullRandom:
                Owner.Position = Room.List.ToList().RandomItem().AdjustRoomPosition() + Role.Location.OffsetPosition;
                break;
            default:
                break;
        }
    }

    private void SetInventory()
    {
        if (Role.Inventory.Clear)
        {
            Owner.ClearInventory();
        }
        foreach (var item in Role.Inventory.Items)
        {
            var itemBase = Owner.Inventory.ServerAddItem(item, InventorySystem.Items.ItemAddReason.StartingItem);
            if (itemBase is Firearm firearm && firearm.TryGetModule(out IPrimaryAmmoContainerModule ammo))
            {
                ammo.ServerModifyAmmo(ammo.AmmoMax);
            }
        }
        foreach (var ammo in Role.Inventory.Ammos)
        {
            Owner.SetAmmo(ammo.Key, ammo.Value);
        }

        if (Role.Candy.Candies.Count != 0)
        {
            Scp330Item bag = null;
            if (Owner.Items.Any(x => x is Scp330Item))
            {
                bag = (Scp330Item)Owner.Items.FirstOrDefault(x => x is Scp330Item);
            }
            else if (!Owner.IsInventoryFull)
            {
                bag = (Scp330Item)Owner.AddItem(ItemType.SCP330, InventorySystem.Items.ItemAddReason.StartingItem);
            }
            bag?.SetCandies(Role.Candy.Candies);
            // TODO: Fix many candies addition
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
    }

    private void SetMaxStats()
    {
        Owner.MaxHealth = Role.Stats.MaxHealth.Math.MathWithFloat(Owner.MaxHealth, Role.Stats.MaxHealth.Value);
        Owner.MaxArtificialHealth = Role.Stats.MaxAhp.Math.MathWithFloat(Owner.MaxArtificialHealth, Role.Stats.MaxAhp.Value);
        Owner.MaxHumeShield = Role.Stats.MaxHumeShield.Math.MathWithFloat(Owner.MaxHumeShield, Role.Stats.MaxHumeShield.Value);
        var max = Owner.ReferenceHub.playerStats.GetModule<StaminaStat>().MaxValue;
        max = Role.Stats.MaxStamina.Math.MathWithFloat(max, Role.Stats.MaxStamina.Value);
        Owner.ReferenceHub.playerStats.GetModule<StaminaStat>().MaxValue = max;

        if (Main.Instance.Config.EffectOnHUDEnabled)
            EffectOnHUD.ShowEffects.AddHpModifier(Owner, "Custom Role", (int)(Owner.MaxHealth - Owner.Health));

    }

    private void SetStats()
    {
        Owner.Health = Role.Stats.Health.Math.MathWithFloat(Owner.Health, Role.Stats.Health.Value);
        Owner.ArtificialHealth = Role.Stats.Ahp.Math.MathWithFloat(Owner.ArtificialHealth, Role.Stats.Ahp.Value);
        Owner.HumeShield = Role.Stats.HumeShield.Math.MathWithFloat(Owner.HumeShield, Role.Stats.HumeShield.Value);
        Owner.Gravity = Role.Stats.Gravity;
    }

    private void SetCommon()
    {
        foreach (var effect in Role.Effects)
        {
            if (!effect.CanEnable)
                continue;
            // this time seems good I guess.
            Owner.EnableEffect(effect.EffectName, effect.Intensity, effect.Duration);
        }
        Owner.IsBypassEnabled = Role.Extra.Bypass;
        if (Role.Extra.OpenDoorsNextToSpawn)
        {
            if (Owner.Room == null)
                return;

            foreach (var door in Owner.Room.Doors)
            {
                door.IsOpened = true;
            }
        }
    }

    private void SetFpc()
    {
        // Scale
        
        if (Role.Fpc.Scale != Vector3.one)
        {
            ScaleHelper.SetScale(Owner, Role.Fpc.Scale, false, true);
        }

        if (Role.Fpc.FakeScale != Vector3.one)
        {
            ScaleHelper.SetScale(Owner, Role.Fpc.FakeScale, true, true);
        }

        //  Appearance
        if (Role.Fpc.Appearance != PlayerRoles.RoleTypeId.None)
        {
            AppearanceSyncExtension.AddPlayer(Owner, Role.Fpc.Appearance);
        }
        // Voice Channel
        if (Role.Fpc.VoiceChatChannel != VoiceChat.VoiceChatChannel.None)
        {
            if (Owner.VoiceModule != null)
                Owner.VoiceModule.CurrentChannel = Role.Fpc.VoiceChatChannel;
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
            Server.SendBroadcast(Role.Hint.BroadcastAll, Role.Hint.BroadcastAllDuration);
        }
    }

    private void SetCustomInfo()
    {
        OldCustomInfo = string.Empty;
        if (!string.IsNullOrEmpty(Owner.CustomInfo))
            OldCustomInfo = Owner.CustomInfo;

        if (Role.Display.RoleCanDisplay)
            Owner.CustomInfo += Role.Display.AreaRoleName;
    }

    private void SetScpRoleInfos()
    {
        if (Owner.RoleBase is Scp049Role scp049Role && scp049Role != null)
        {
            if (scp049Role.SubroutineModule.TryGetSubroutine(out Scp049AttackAbility scp049AttackAbility))
            {
                Role.Scp.Scp049.AttackEffectDuration.MathWithValue(ref scp049AttackAbility._statusEffectDuration);
            }
        }
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

        if (Owner.RoleBase is Scp939Role scp939Role && scp939Role != null)
        {
            if (scp939Role.SubroutineModule.TryGetSubroutine(out Scp939AmnesticCloudAbility scp939AmnesticCloudAbility))
            {
                Role.Scp.Scp939.CloudFailCooldown.MathWithValue(ref scp939AmnesticCloudAbility._failedCooldown);
                Role.Scp.Scp939.CloudPlacedCooldown.MathWithValue(ref scp939AmnesticCloudAbility._placedCooldown);
            }
        }
    }
}