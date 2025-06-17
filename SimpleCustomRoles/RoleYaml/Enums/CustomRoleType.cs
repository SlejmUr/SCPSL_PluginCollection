namespace SimpleCustomRoles.RoleYaml.Enums;

public enum CustomRoleType
{
    Regular,        // Only appears when start of the game.
    AfterDead,      // Only appears after dying
    InWave,         // Only appears inside the SpawnWave.
    ScpSpecific,    // Only appears if set by Custom SCP's.
}