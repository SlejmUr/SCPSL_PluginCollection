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
        public string RoleName { get; set; }
        public string DisplayRoleName { get; set; } = string.Empty;
        public string RoleDisplayColorHex { get; set; } = "#ffffff";
        public int SpawnChance { get; set; } = 0;
        public int SpawnAmount { get; set; } = 0;
        public RoleType RoleType { get; set; } = RoleType.Regular;
        public RoleTypeId RoleToSpawnAs { get; set; } = RoleTypeId.None;
        public RoleTypeId RoleToReplace { get; set; } = RoleTypeId.None;
        public Team ReplaceFromTeam { get; set; } = Team.Dead;
        public SpawnWaveSpecific SpawnWaveSpecific { get; set; } = new SpawnWaveSpecific();
        public Location Location { get; set; } = new Location();
        public Inventory Inventory { get; set; } = new Inventory();
        public List<Effect> Effects { get; set; } = new List<Effect>();
        public HealthClass Health { get; set; } = new HealthClass();
        public HintStuff Hint { get; set; } = new HintStuff();
        public Advanced Advanced { get; set; } = new Advanced();
        public SCP_Specific SCP_Specific { get; set; } = new SCP_Specific();
        public EventCaller EventCaller { get; set; } = new EventCaller();
    }
    public enum RoleType
    {
        Regular,        // Only appears when start of the game.
        AfterDead,      // Only appears after dying
        InWave,         // Only appears inside the SpawnWave.
        SPC_Specific    // Only appears if set by Custom SCP's.
    }
    public class SpawnWaveSpecific
    {
        public SpawnableTeamType Team { get; set; } = SpawnableTeamType.None;
        public int MinimumTeamMemberRequired { get; set; } = 0;
        public bool SkipMinimumCheck { get; set; } = false;
    }
    public class Inventory
    {
        public List<ItemType> InventoryItems { get; set; } = new List<ItemType>();
        public List<ItemType> DeniedUsingItems { get; set; } = new List<ItemType>();
        public List<ItemType> CannotDropItems { get; set; } = new List<ItemType>();
        public Dictionary<AmmoType, ushort> Ammos { get; set; } = new Dictionary<AmmoType, ushort>();
        public List<uint> CustomItemIds { get; set; } = new List<uint>();
    }
    public class HintStuff
    {
        public string SpawnBroadcast { get; set; } = "";
        public ushort SpawnBroadcastDuration { get; set; } = 0;
        public string SpawnHint { get; set; } = "";
        public float SpawnHintDuration { get; set; } = 0;
        public string SpawnBroadcastToAll { get; set; } = "";
        public ushort SpawnBroadcastToAllDuration { get; set; } = 0;

    }
    public class HealthClass
    {
        public class ValueSetter
        {
            public float Value { get; set; } = 0;
            public MathOption SetType { get; set; } = MathOption.None;
        }

        public ValueSetter Health { get; set; } = new ValueSetter();
        public ValueSetter Ahp { get; set; } = new ValueSetter();
        public ValueSetter HumeShield { get; set; } = new ValueSetter();
    }
    public class Location
    {
        public bool UseDefault { get; set; } = true;
        public LocationSpawnPriority LocationSpawnPriority { get; set; } = LocationSpawnPriority.FullRandom;
        public List<ZoneType> SpawnZones { get; set; }
        public List<RoomType> SpawnRooms { get; set; }
        public V3 ExactPosition { get; set; } = new V3();
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
        public bool CanRemovedwithSCP500 { get; set; } = false;
        public EffectType EffectType { get; set; }
        public float Duration { get; set; }
        public byte Intensity { get; set; }

    }
    public class Advanced
    {
        [Description("yippee")]
        public V3 Scale { get; set; } = new V3();
        public RoleTypeId RoleAppearance { get; set; } = RoleTypeId.None;
        public RoleTypeId RoleAfterEscape { get; set; } = RoleTypeId.None;
        public bool CanEscape { get; set; } = true;
        public bool BypassModeEnabled { get; set; } = false;
        public bool CanChargeJailBird { get; set; } = true;
        public bool OpenDoorsNextToSpawn { get; set; } = false;
        public bool CanTrigger096 { get; set; } = true;
        public DeadBy DeadBy { get; set; } = new DeadBy();
        public CandyStuff Candy { get; set; } = new CandyStuff();
        public Damager Damager { get; set; } = new Damager();
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
        public List<CandyKindID> CandiesToGive { get; set; } = new List<CandyKindID>();
        public bool CanTakeCandy { get; set; } = true;
        public int MaxTakeCandy { get; set; } = 2;
        public bool GlobalCanEatCandy { get; set; } = true;
        public bool GlobalCanDropCandy { get; set; } = true;
        public bool ShowCandyLeft { get; set; } = false;
        public Dictionary<CandyKindID, CandySpecific> SpecialCandy { get; set; } = new Dictionary<CandyKindID, CandySpecific>();
        public class CandySpecific
        {
            public bool CanEatCandy { get; set; } = true;
            public bool CanDropCandy { get; set; } = true;
        }
    }
    public class SCP_Specific
    {
        public _049 SCP_049 { get; set; } = new _049();
        public _0492 SCP_0492 { get; set; } = new _0492();
        public _079 SCP_079 { get; set; } = new _079();
        public _096 SCP_096 { get; set; } = new _096();
        public _173 SCP_173 { get; set; } = new _173();
        public class _049 : RoleSetAfter
        {
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
            public MathOption Enraging_SetType { get; set; } = MathOption.None;
            public float Enraging_InitialDuration { get; set; } = 0;
            public bool CanCharge { get; set; } = true;
            public bool CanTryingNotToCry { get; set; } = true;

            [Description("Define global state of Prying with 096.")]
            public bool CanPry { get; set; } = true;

            [Description("List of Doors to 096 cannot be Pryed on.")]
            public List<DoorType> DoorToNotPryOn { get; set; } = new List<DoorType>();
        }

        public class _173
        {
            public bool CanPlaceTantrum { get; set; } = true;
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

            public PowerCostSet ChangingCameraCost { get; set; } = new PowerCostSet();
            public List<GainXP> GainingXP { get; set; } = new List<GainXP>();
            public bool CanRxecall { get; set; } = true;
        }
    }

    public class Damager
    {
        public Dictionary<DamageType, SubDamager> DamageReceivedDict { get; set; } = new Dictionary<DamageType, SubDamager>();
        public Dictionary<DamageType, SubDamager> DamageSentDict { get; set; } = new Dictionary<DamageType, SubDamager>();
        public class SubDamager
        {
            public MathOption SetType { get; set; } = MathOption.None;
            public float Damage { get; set; } = 0f;

        }
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
