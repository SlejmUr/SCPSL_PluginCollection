using MapGeneration;
using SimpleCustomRoles.RoleYaml.Enums;
using System.ComponentModel;
using UnityEngine;

namespace SimpleCustomRoles.RoleYaml;

public class LocationInfo
{
    [Description("Should use Default spawn.")]
    public bool UseDefault { get; set; } = true;

    [Description("Prioritize where you spawn from below set values. See LocationSpawnPrioritys.txt")]
    public LocationSpawnPriority Priority { get; set; } = LocationSpawnPriority.FullRandom;

    [Description("Zone List to Spawn in. Check FacilityZone.txt")]
    public List<FacilityZone> SpawnZones { get; set; }

    [Description("Room Types to Spawn in. Check RoomName.txt")]
    public List<RoomName> SpawnRooms { get; set; }

    [Description("Room Types to Exlude when Spawn in. (Used with SpawnZone) Check RoomName.txt")]
    public List<RoomName> ExludeRooms { get; set; }

    [Description("Exact Position with Vector3.")]
    public Vector3 ExactPosition { get; set; } = Vector3.one;

    [Description("Offset by all Spawn Position (Except when Default is True).")]
    public Vector3 OffsetPosition { get; set; } = Vector3.one;
}
