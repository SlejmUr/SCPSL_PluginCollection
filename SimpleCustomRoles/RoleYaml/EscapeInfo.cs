using System.ComponentModel;

namespace SimpleCustomRoles.RoleYaml;

public class EscapeInfo
{
    [Description("Can the player escape.")]
    public bool CanEscape { get; set; } = true;

    [Description("Role Gathered after Escaping with the Scenario.")]
    public Dictionary<EscapeConfig, NewRoleInfo> ConfigToRole { get; set; } = [];
}
