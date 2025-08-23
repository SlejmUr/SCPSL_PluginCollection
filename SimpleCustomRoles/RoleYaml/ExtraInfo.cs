using System.ComponentModel;

namespace SimpleCustomRoles.RoleYaml;

public class ExtraInfo
{
    [Description("Enable Door Bypassing.")]
    public bool Bypass { get; set; } = false;

    [Description("Open all doors next to spawned place.")]
    public bool OpenDoorsNextToSpawn { get; set; } = false;

    public bool Trigger096 { get; set; } = true;
    public bool Observe173 { get; set; } = true;

    public bool ForceSet { get; set; } = false;

    public bool CannotRevivedByScp049 { get; set; } = false;
}
