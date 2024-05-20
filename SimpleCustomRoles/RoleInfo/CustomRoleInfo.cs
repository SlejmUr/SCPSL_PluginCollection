

using Exiled.API.Enums;
using InventorySystem.Items.Usables.Scp330;
using PlayerRoles;
using Respawning;
using System.Collections.Generic;

namespace SimpleCustomRoles.RoleInfo
{
    public class CustomRoleInfo
    {
        public string RoleName { get; set; }
        public string DisplayRoleName { get; set; }
        public int SpawnChance { get; set; } = 0;
        public int SpawnAmount { get; set; } = 0;
        public bool ReplaceInSpawnWave { get; set; } = false;
        public bool UsedAfterDeath { get; set; } = false;
        public SpawnWaveSpecific SpawnWaveSpecific { get; set; } = new SpawnWaveSpecific();
        public RoleTypeId RoleToSpawnAs { get; set; } = RoleTypeId.None;
        public RoleTypeId RoleToReplace { get; set; } = RoleTypeId.None;
        public Team ReplaceFromTeam { get; set; } = Team.Dead;
        public Location Location { get; set; } = new Location();
        public List<ItemType> InventoryItems { get; set; } = new List<ItemType>(); 
        public List<ItemType> DeniedUsingItems { get; set; } = new List<ItemType>();
        public List<ItemType> CannotDropItems { get; set; } = new List<ItemType>();
        public Dictionary<AmmoType, ushort> Ammos { get; set; } = new Dictionary<AmmoType, ushort>();
        public List<Effect> Effects { get; set; } = new List<Effect>();
        public List<uint> CustomItemIds { get; set; } = new List<uint>();
        public HealthMod HealthModifiers { get; set; } = new HealthMod();
        public HealthReplace HealthReplacer { get; set; } = new HealthReplace();
        public HintStuff Hint { get; set; } = new HintStuff();
        public Advanced Advanced { get; set; } = new Advanced();
        public SCP_Specific SCP_Specific { get; set; } = new SCP_Specific();
    }

    public class SpawnWaveSpecific
    {
        public SpawnableTeamType Team { get; set; } = SpawnableTeamType.None;
        public int MinimumTeamMemberRequired { get; set; } = 0;
        public bool SkipMinimumCheck { get; set; } = false;
    }


    public class HintStuff
    {
        public string SpawnBroadcast { get; set; } = "";
        public ushort SpawnBroadcastDuration { get; set; } = 0;
        public string SpawnHint { get; set; } = "";
        public float SpawnHintDuration { get; set; } = 0;

    }

    public class HealthMod
    {
        public float Health { get; set; } = 0;
        public float Ahp { get; set; } = 0;
        public float HumeShield { get; set; } = 0;
    }

    public class HealthReplace
    {
        public bool UseReplace { get; set; } = false;
        public float Health { get; set; } = 0;
        public float Ahp { get; set; } = 0;
        public float HumeShield { get; set; } = 0;
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
        public V3 Scale { get; set; } = new V3();
        public RoleTypeId RoleAppearance { get; set; } = RoleTypeId.None;
        public bool CanEscape { get; set; } = true;
        public RoleTypeId RoleAfterEscape { get; set; } = RoleTypeId.None;
        public DeadBy DeadBy { get; set; } = new DeadBy();
        public CandyStuff Candy { get; set; } = new CandyStuff();
        public bool BypassEnabled { get; set; } = false;
        public bool CanChargeJailBird { get; set; } = true;
        public string ColorHex { get; set; } = "#ffffff";
        public bool OpenDoorsNextToSpawn { get; set; } = false;
    }

    public class DeadBy
    {
        public bool IsConfigurated { get; set; } = false;
        public RoleTypeId KillerRole { get; set; } = RoleTypeId.None;
        public Team KillerTeam { get; set; } = Team.Dead;
        public RoleTypeId RoleAfterKilled { get; set; } = RoleTypeId.None;
        public string RoleNameToRespawnAs { get; set; }
        public List<string> RoleNameRandom { get; set; } = new List<string>();
    }

    public class CandyStuff
    {
        public List<CandyKindID> CandiesToGive { get; set; } = new List<CandyKindID>();
    }

    public class SCP_Specific
    {
        public bool SCP_Specific_Role { get; set; } = false;
        public _049 SCP_049 { get; set; } = new _049();
        public _0492 SCP_0492 { get; set; } = new _0492();
        public _096 SCP_096 { get; set; } = new _096();
        public class _049
        {
            public bool CanRecall { get; set; } = true;

            // These Applied to the recalled player:

            //Specific role to respawn after you got recalled.
            public RoleTypeId RoleAfterKilled { get; set; } = RoleTypeId.None;
            public string RoleNameToRespawnAs { get; set; }
            public List<string> RoleNameRandom { get; set; } = new List<string>();
        }

        public class _0492
        {
            public bool CanConsumeCorpse { get; set; } = true;
            public bool CanSpawnIfNoCustom094 { get; set; } = false;
            public int ChanceForSpawn { get; set; } = 0;
        }

        public class _096
        {
            public bool CanTrigger096 { get; set; } = true;
        }
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
        public V3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
