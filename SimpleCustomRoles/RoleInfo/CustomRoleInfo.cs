using Exiled.API.Enums;
using InventorySystem.Items.Usables.Scp330;
using PlayerRoles;
using PlayerRoles.PlayableScps.Scp079;
using Respawning;
using System.Collections.Generic;
using System.ComponentModel;

namespace SimpleCustomRoles.RoleInfo
{
    public class CustomRoleInfo
    {
        [Description("Role name, can be the name of the file, just be different then others. REQUIRED (DONT USE SPACE!)")]
        public string RoleName { get; set; }

        [Description("Role display name")]
        public string DisplayRoleName { get; set; } = string.Empty;

        [Description("Role display color")]
        public string RoleDisplayColorHex { get; set; } = "#ffffff";

        [Description("Can the role display")]
        public bool RoleCanDisplay { get; set; } = true;

        [Description("REQUIRED! Role spawning chance. 0 means NEVER, min 1, max 10000 [10 000] (so 0.01 = 1, 60 = 6000 [6 000])")]
        public int SpawnChance { get; set; } = 0;

        [Description("NOTDEAD! Role spawn ammount")]
        public int SpawnAmount { get; set; } = 0;

        [Description("Type of the Role. Check CustomRoleTypes.txt")]
        public CustomRoleType RoleType { get; set; } = CustomRoleType.Regular;

        [Description("REQUIRED! From RoleTypeIds it declare what Role it will spawn as.")]
        public RoleTypeId RoleToSpawnAs { get; set; } = RoleTypeId.None;

        [Description("TEAMREPLACE! From RoleTypeIds it declare what Role it will replace from.")]
        public RoleTypeId RoleToReplace { get; set; } = RoleTypeId.None;

        [Description("TEAMREPLACE! From Team it declare what teams it will pick a random user from.")]
        public Team ReplaceFromTeam { get; set; } = Team.Dead;

        [Description("WAVE!")]
        public SpawnWaveSpecific SpawnWaveSpecific { get; set; } = new SpawnWaveSpecific();

        [Description("Location, declare where to spawn.")]
        public Location Location { get; set; } = new Location();

        [Description("Set Inventory Releated Actions !!IT WILL REPLACE AND CLEAR THE INVENTORY OF THE ALREADY REPLACED CLASS!!")]
        public Inventory Inventory { get; set; } = new Inventory();

        [Description("Set Effects to the player")]
        public List<Effect> Effects { get; set; } = new List<Effect>();

        [Description("Modify health, ahp, hume")]
        public HealthClass Health { get; set; } = new HealthClass();

        [Description("Using hint, broadcast")]
        public HintStuff Hint { get; set; } = new HintStuff();

        [Description("Advanced configurations.")]
        public Advanced Advanced { get; set; } = new Advanced();

        [Description("SCP Specific actions.")]
        public SCP_Specific Scp_Specific { get; set; } = new SCP_Specific();

        [Description("Making a Console command with some premade values. Good for ScriptedEvents")]
        public EventCaller EventCaller { get; set; } = new EventCaller();
    }
    public enum CustomRoleType
    {
        Regular,        // Only appears when start of the game.
        AfterDead,      // Only appears after dying
        InWave,         // Only appears inside the SpawnWave.
        SPC_Specific,   // Only appears if set by Custom SCP's.
        Escape,         // Only appears after Players escaped.
    }
    public class SpawnWaveSpecific
    {
        [Description("Spawning Team Type.")]
        public SpawnableTeamType Team { get; set; } = SpawnableTeamType.None;

        [Description("Minimum Team Member Required to spawn this class.")]
        public int MinimumTeamMemberRequired { get; set; } = 0;

        [Description("Should skip the minimum check.")]
        public bool SkipMinimumCheck { get; set; } = false;
    }
    public class Inventory
    {
        [Description("Items to spawn in.")]
        public List<ItemType> InventoryItems { get; set; } = new List<ItemType>();

        [Description("Denied using items list")]
        public List<ItemType> DeniedUsingItems { get; set; } = new List<ItemType>();

        [Description("Cannot drop items list.")]
        public List<ItemType> CannotDropItems { get; set; } = new List<ItemType>();

        [Description("Ammos and amount of it.")]
        public Dictionary<AmmoType, ushort> Ammos { get; set; } = new Dictionary<AmmoType, ushort>();

        [Description("IF you use custom item, you can declare the IDS's here.")]
        public List<uint> CustomItemIds { get; set; } = new List<uint>();
    }
    public class HintStuff
    {
        [Description("Suggestion: Say that what the user spawned as. Can use something like <color=#ededb4><b>TEMP</b></color>\\n\\tFav role")]
        public string SpawnBroadcast { get; set; } = "";

        [Description("Suggestion: Set as 15, so when you spawned most of the UI is gonna get obscured. After 10 or 8 second it will be removed/hidden.")]
        public ushort SpawnBroadcastDuration { get; set; } = 0;

        [Description("Suggestion: Any hints to display.")]
        public string SpawnHint { get; set; } = "";

        [Description("Suggestion: Set as 15, so when you spawned most of the UI is gonna get obscured. After 10 or 8 second it will be removed/hidden.")]
        public float SpawnHintDuration { get; set; } = 0;

        [Description("Broadcast to All users.")]
        public string SpawnBroadcastToAll { get; set; } = "";

        [Description("Suggestion: Set as 15, so when you spawned most of the UI is gonna get obscured. After 10 or 8 second it will be removed/hidden.")]
        public ushort SpawnBroadcastToAllDuration { get; set; } = 0;

    }
    public class ValueSetter
    {
        public float Value { get; set; } = 0;

        [Description("Value's Math option. Check MathOptions.txt.")]
        public MathOption SetType { get; set; } = MathOption.None;
    }
    public class HealthClass
    {

        [Description("Health Value edit.")]
        public ValueSetter Health { get; set; } = new ValueSetter();

        [Description("AHP Value edit. (Used if human class)")]
        public ValueSetter Ahp { get; set; } = new ValueSetter();

        [Description("HumeShield Value edit. (Used if SCP class)")]
        public ValueSetter HumeShield { get; set; } = new ValueSetter();
    }
    public class Location
    {

        [Description("Should use Default spawn.")]
        public bool UseDefault { get; set; } = true;

        [Description("Prioritize where you spawn from below set values. See LocationSpawnPrioritys.txt")]
        public LocationSpawnPriority LocationSpawnPriority { get; set; } = LocationSpawnPriority.FullRandom;

        [Description("Zone List to Spawn in. Check ZoneTypes.txt")]
        public List<ZoneType> SpawnZones { get; set; }

        [Description("Room Types to Spawn in. Check RoomTypes.txt")]
        public List<RoomType> SpawnRooms { get; set; }

        [Description("Exact Position with Vector3.")]
        public V3 ExactPosition { get; set; } = new V3();

        [Description("Offset by all Spawn Position (Except when Default is True).")]
        public V3 OffsetPosition { get; set; } = new V3();
    }
    public enum LocationSpawnPriority
    {
        None,
        SpawnZone,
        SpawnRoom,
        ExactPosition,
        FullRandom
    }
    public class Effect
    {

        [Description("Effect can be removed with SCP-500.")]
        public bool CanRemovedWithSCP500 { get; set; } = true;

        [Description("Effect Type to add into the user. Check EffectTypes.txt")]
        public EffectType EffectType { get; set; }

        [Description("Duration how long the effect should last.")]
        public float Duration { get; set; }

        [Description("Intensity of the effect.")]
        public byte Intensity { get; set; }

    }
    public class Advanced
    {
        [Description("Player Scale")]
        public V3 Scale { get; set; } = new V3();

        [Description("Player Appearance to others")]
        public RoleTypeId RoleAppearance { get; set; } = RoleTypeId.None;

        [Description("Enable Door Bypassing.")]
        public bool BypassModeEnabled { get; set; } = false;

        [Description("Able to charge the jailbird.")]
        public bool CanChargeJailBird { get; set; } = true;

        [Description("Open all dorrs next to spawned place.")]
        public bool OpenDoorsNextToSpawn { get; set; } = false;

        [Description("Can trigger 096 Raging.")]
        public bool CanTrigger096 { get; set; } = true;

        [Description("Escaping options.")]
        public Escape Escaping { get; set; } = new Escape();

        [Description("TODO.")]
        public DeadBy DeadBy { get; set; } = new DeadBy();

        [Description("Candy releated actions.")]
        public CandyStuff Candy { get; set; } = new CandyStuff();

        [Description("Damager for Receiving and Sending damaga values.")]
        public Damager Damager { get; set; } = new Damager();

        [Description("Friendly Fire to each Role.")]
        public List<FF> FriendlyFire { get; set; } = new List<FF>();
        public class FF
        {
            public RoleTypeId RoleType { get; set; } = RoleTypeId.None;
            public float Value { get; set; } = 0;
        }

        public class Escape
        {

            [Description("Can the player escape.")]
            public bool CanEscape { get; set; } = true;

            [Description("Role Gathered after Escaping with the Scenario.")]
            public Dictionary<EscapeScenario, RoleTypeId> RoleAfterEscape = new Dictionary<EscapeScenario, RoleTypeId>();

            [Description("Custom Role after Escaping with the Scenario.")]
            public Dictionary<EscapeScenario, string> RoleNameAfterEscape = new Dictionary<EscapeScenario, string>();
        }
    }
    public class RoleSetAfter
    {
        //  RoleType must be set to AfterDead!
        public RoleTypeId RoleAfterKilled { get; set; } = RoleTypeId.None;
        public string RoleNameToRespawnAs { get; set; } = string.Empty;
        public List<string> RoleNameRandom { get; set; } = new List<string>();
    }
    public class DeadBy : RoleSetAfter
    {
        public bool IsConfigurated { get; set; } = false;
        public RoleTypeId KillerRole { get; set; } = RoleTypeId.None;
        public Team KillerTeam { get; set; } = Team.Dead;
    }
    public class CandyStuff
    {

        [Description("Candies to give to the player when spawned. Check CandyKindIDs.txt")]
        public List<CandyKindID> CandiesToGive { get; set; } = new List<CandyKindID>();

        [Description("Can the user Take candies from the bowl.")]
        public bool CanTakeCandy { get; set; } = true;

        [Description("Max candies can taken from the bowl.")]
        public int MaxTakeCandy { get; set; } = 2;

        [Description("Player can eat any Candy")]
        public bool GlobalCanEatCandy { get; set; } = true;

        [Description("Player can drop any Candy")]
        public bool GlobalCanDropCandy { get; set; } = true;

        [Description("Show how many candies can the user get.")]
        public bool ShowCandyLeft { get; set; } = false;

        [Description("Special candy properties.")]
        public Dictionary<CandyKindID, CandySpecific> SpecialCandy { get; set; } = new Dictionary<CandyKindID, CandySpecific>();
        public class CandySpecific
        {

            [Description("Can user eat this type of Candy.")]
            public bool CanEatCandy { get; set; } = true;

            [Description("Can user drop this type of Candy.")]
            public bool CanDropCandy { get; set; } = true;
        }
    }
    public class SCP_Specific
    {

        [Description("SCP-049 Options.")]
        public _049 Scp049 { get; set; } = new _049();

        [Description("SCP-049-2 Options.")]
        public _0492 Scp0492 { get; set; } = new _0492();

        [Description("Experimental SCP-079 Options.")]
        public _079 Scp079 { get; set; } = new _079();

        [Description("SCP-069 Options.")]
        public _096 Scp096 { get; set; } = new _096();

        [Description("SCP-173 Options.")]
        public _173 Scp173 { get; set; } = new _173();
        public class _049 : RoleSetAfter
        {

            [Description("Can use Recall Ability.")]
            public bool CanRecall { get; set; } = true;
        }

        public class _0492
        {
            public bool CanConsumeCorpse { get; set; } = true;
            public bool CanSpawnIfNoCustom094 { get; set; } = false;
            public int ChanceForSpawn { get; set; } = 0;
        }

        public class _096
        {

            [Description("Set Enraging.")]
            public ValueSetter Enraging { get; set; } = new ValueSetter();

            [Description("Can 096 Charge.")]
            public bool CanCharge { get; set; } = true;

            [Description("Can 096 Use TryNotToCry Ability.")]
            public bool CanTryingNotToCry { get; set; } = true;

            [Description("Define global state of Prying with 096.")]
            public bool CanPry { get; set; } = true;

            [Description("List of Doors to 096 cannot be Pryed on.")]
            public List<DoorType> DoorToNotPryOn { get; set; } = new List<DoorType>();
        }

        public class _173
        {

            [Description("Can 173 Place Tantrum.")]
            public bool CanPlaceTantrum { get; set; } = true;

            [Description("Can 173 Use Breakneck Speed.")]
            public bool CanUseBreakneckSpeed { get; set; } = true;
        }

        public class _079
        {
            public class PowerCostSet
            {
                public MathOption SetType { get; set; } = MathOption.None;
                public float AuxiliaryPowerCost { get; set; } = 0;
            }

            public class CanDoStuff
            {
                public bool IsAllowed { get; set; } = true;
            }

            public class GainXP : CanDoStuff
            {
                public RoleTypeId RoleType { get; set; }
                public Scp079HudTranslation HudTranslation { get; set; }
                public MathOption SetType { get; set; } = MathOption.None;
                public int XPAmount { get; set; }
            }

            [Description("Changing Camera Cost.")]
            public PowerCostSet ChangingCameraCost { get; set; } = new PowerCostSet();


            [Description("Gaining XP Releated actions.")]
            public List<GainXP> GainingXP { get; set; } = new List<GainXP>();
        }
    }

    public class Damager
    {
        [Description("Damage Dictionary that Player Received.")]
        public Dictionary<DamageType, ValueSetter> DamageReceivedDict { get; set; } = new Dictionary<DamageType, ValueSetter>();

        [Description("Damage Dictionary that Player Sent/Dealt.")]
        public Dictionary<DamageType, ValueSetter> DamageSentDict { get; set; } = new Dictionary<DamageType, ValueSetter>();
    }

    public enum MathOption
    {
        None,
        Set,
        Add,
        Subtract,
        Multiply,
        Divide,
    }

    public class EventCaller
    {
        public string OnDied { get; set; } = string.Empty;
        public string OnKill { get; set; } = string.Empty;
        public string OnSpawnWave { get; set; } = string.Empty;
        public string OnDealDamage { get; set; } = string.Empty;
        public string OnReceiveDamage { get; set; } = string.Empty;
        public string OnSpawned { get; set; } = string.Empty;
    }

    public class V3 //Vector3
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public V3()
        {
            X = 0;
            Y = 0;
            Z = 0;
        }
        public V3(float all)
        {
            X = all;
            Y = all;
            Z = all;
        }
        public V3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
