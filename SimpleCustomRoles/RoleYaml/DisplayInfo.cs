using System.ComponentModel;

namespace SimpleCustomRoles.RoleYaml;

public class DisplayInfo
{
    [Description("Can the role display")]
    public bool RoleCanDisplay { get; set; } = true;

    [Description("Role area name")]
    public string AreaRoleName { get; set; } = string.Empty;

    [Description("Role RA name")]
    public string RARoleName { get; set; } = string.Empty;

    [Description("Role Spectator name")]
    public string SpectatorRoleName { get; set; } = string.Empty;

    [Description("Role color")]
    public string ColorHex { get; set; } = "#ffffff";
}
