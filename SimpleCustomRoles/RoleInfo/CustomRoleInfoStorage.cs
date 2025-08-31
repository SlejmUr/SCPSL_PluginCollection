using InventorySystem;
using InventorySystem.Items.Firearms;
using InventorySystem.Items.Firearms.Modules;
using LabApi.Features.Stores;
using LabApi.Features.Wrappers;
using MEC;
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
    private bool onDefaultScale;
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
        ScaleHelper.SetScale(Owner, Vector3.one);
        Owner.Position += Vector3.up;
        if (string.IsNullOrEmpty(OldCustomInfo))
            OldCustomInfo = string.Empty;
        Owner.CustomInfo = OldCustomInfo;
        if (ResetRole)
            Owner.SetRole(Owner.Role, PlayerRoles.RoleChangeReason.None, PlayerRoles.RoleSpawnFlags.None);
    }

    public void ChangeScale()
    {
        Vector3 scale = Role.Fpc.Scale;
        if (!onDefaultScale)
            scale = Vector3.one;
        ScaleHelper.SetScale(Owner, scale);

        onDefaultScale = !onDefaultScale;
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
#if ENABLEEFFECTHUD
        float originalMaxHealth = Owner.MaxHealth;
#endif
        Owner.MaxHealth = Role.Stats.MaxHealth.MathCalculation(Owner.MaxHealth);
        Owner.MaxArtificialHealth = Role.Stats.MaxAhp.MathCalculation(Owner.MaxArtificialHealth);
        Owner.MaxHumeShield = Role.Stats.MaxHumeShield.MathCalculation(Owner.MaxHumeShield);
        var max = Owner.ReferenceHub.playerStats.GetModule<StaminaStat>().MaxValue;
        max = Role.Stats.MaxStamina.MathCalculation(max);
        Owner.ReferenceHub.playerStats.GetModule<StaminaStat>().MaxValue = max;
#if ENABLEEFFECTHUD
        EffectOnHUD.ShowEffects.AddHpModifier(Owner, "Custom Role", (int)(Owner.MaxHealth - originalMaxHealth));
#endif
    }

    private void SetStats()
    {
        Owner.Health = Role.Stats.Health.MathCalculation(Owner.Health);
        Owner.ArtificialHealth = Role.Stats.Ahp.MathCalculation(Owner.ArtificialHealth);
        Owner.HumeShield = Role.Stats.HumeShield.MathCalculation(Owner.HumeShield);
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
            ScaleHelper.SetScale(Owner, Role.Fpc.Scale);
        }

        if (Role.Fpc.FakeScale != Vector3.one)
        {
            ScaleHelper.SetScale(Owner, Role.Fpc.FakeScale, true);
        }

        //  Appearance
        if (Role.Fpc.Appearance != PlayerRoles.RoleTypeId.None)
        {
            /*
            foreach (var item in Player.ReadyList.Where(x => x != Owner))
            {
                FpcServerPositionDistributor.SendRole(item.ReferenceHub, Owner.ReferenceHub, Role.Fpc.Appearance);
            }
            */
            //AppearanceSyncExtension.AddPlayer(Owner, Role.Fpc.Appearance);
        }
        // Voice Channel
        if (Role.Fpc.VoiceChatChannel != VoiceChat.VoiceChatChannel.None)
        {
            if (Owner.VoiceModule != null)
                Owner.VoiceModule.CurrentChannel = Role.Fpc.VoiceChatChannel;
        }
    }

    internal void SetHints()
    {
        bool useOriginal = true;
        if (!string.IsNullOrEmpty(Role.Hint.Broadcast))
        {
            useOriginal = true;
            Events.TriggerShowBroadcast(Owner, Role, ref useOriginal);
            if (useOriginal)
                Server.SendBroadcast(Owner, Role.Hint.Broadcast, Role.Hint.BroadcastDuration);
        }
        if (!string.IsNullOrEmpty(Role.Hint.Hint))
        {
            useOriginal = true;
            Events.TriggerShowHint(Owner, Role, ref useOriginal);
            if (useOriginal)
                Owner.SendHint(Role.Hint.Hint, Role.Hint.HintDuration);
        }
        if (!string.IsNullOrEmpty(Role.Hint.BroadcastAll))
        {
            useOriginal = true;
            Events.TriggerShowBroadcastAll(Owner, Role, ref useOriginal);
            if (useOriginal)
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
                Role.Scp.Scp049.AttackEffectDuration.MathCalculation(ref scp049AttackAbility._statusEffectDuration);
            }
        }
        if (Owner.RoleBase is Scp106Role scp106Role && scp106Role != null)
        {
            if (scp106Role.SubroutineModule.TryGetSubroutine(out Scp106Attack scp106Attack))
            {
                Role.Scp.Scp106.AttackHitCooldown.MathCalculation(ref scp106Attack._hitCooldown);
                Role.Scp.Scp106.AttackMissCooldown.MathCalculation(ref scp106Attack._missCooldown);
                Role.Scp.Scp106.AttackDamage.MathCalculation(ref scp106Attack._damage);
            }
        }
        if (Owner.RoleBase is Scp1507Role scp1507Role && scp1507Role != null)
        {
            if (scp1507Role.SubroutineModule.TryGetSubroutine(out Scp1507AttackAbility attackAbility))
            {
                Role.Scp.Scp1507.AttackDamage.MathCalculation(ref attackAbility._damage);
            }
        }

        if (Owner.RoleBase is Scp939Role scp939Role && scp939Role != null)
        {
            if (scp939Role.SubroutineModule.TryGetSubroutine(out Scp939AmnesticCloudAbility scp939AmnesticCloudAbility))
            {
                Role.Scp.Scp939.CloudFailCooldown.MathCalculation(ref scp939AmnesticCloudAbility._failedCooldown);
                Role.Scp.Scp939.CloudPlacedCooldown.MathCalculation(ref scp939AmnesticCloudAbility._placedCooldown);
            }
        }
    }
}