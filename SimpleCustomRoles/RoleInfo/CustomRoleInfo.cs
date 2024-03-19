using Exiled.API.Enums;
using PlayerRoles;
using Respawning;
using System.Collections.Generic;

namespace SimpleCustomRoles.RoleInfo
{
    public class CustomRoleInfo
    {
        public string RoleName { get; set; }
        public int SpawnChance { get; set; } = 0;
        public int SpawnAmount { get; set; } = 0;
        public bool ReplaceInSpawnWave { get; set; } = false;
        public SpawnWaveSpecific SpawnWaveSpecific { get; set; } = new SpawnWaveSpecific();
        public RoleTypeId RoleToSpawnAs { get; set; } = RoleTypeId.None;
        public RoleTypeId RoleToReplace { get; set; } = RoleTypeId.None;
        public Team ReplaceFromTeam { get; set; } = Team.Dead;
        public Location Location { get; set; } = new Location();
        public List<ItemType> InventoryItems { get; set; } = new List<ItemType>();
        public Dictionary<AmmoType, ushort> Ammos { get; set; } = new Dictionary<AmmoType, ushort>();
        public List<Effect> Effects { get; set; } = new List<Effect>();
        public List<ushort> CustomItemIds { get; set; } = new List<ushort>();
        public HealthMod HealthModifiers { get; set; } = new HealthMod();
        public HintStuff Hint { get; set; } = new HintStuff();
        public Advanced Advanced { get; set; } = new Advanced();

    }

    public class SpawnWaveSpecific
    {
        public SpawnableTeamType Team { get; set; } = SpawnableTeamType.None;
        public int MinimumTeamMemberRequired { get; set; } = 0;
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


    public class Location
    {
        public bool UseDefault { get; set; } = true;
        public LocationSpawnPriority LocationSpawnPriority { get; set; } = LocationSpawnPriority.FullRandom;
        public List<ZoneType> SpawnZones { get; set; }
        public List<RoomType> SpawnRooms { get; set; }
        public V3 ExactPosition { get; set; } = new V3();
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
        public EffectType EffectType { get; set; }
        public float Duration { get; set; }
        public byte Intensity { get; set; }

    }

    public class Advanced
    {
        public string RunOnServer { get; set; }
        public V3 Scale { get; set; } = new V3();
        public RoleTypeId RoleAppearance { get; set; } = RoleTypeId.None;
        public bool CanEscape { get; set; } = true;
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
