using PlayerRoles;

namespace SimpleCustomRoles.RoleYaml;

public class KillerRoleInfo
{
    public string KillerCustom { get; set; } = string.Empty;
    public RoleTypeId KillerRole { get; set; } = RoleTypeId.None;
    public Team KillerTeam { get; set; } = Team.Dead;
}
