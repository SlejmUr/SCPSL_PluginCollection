using System.ComponentModel;

namespace SimpleCustomRoles.RoleYaml;

public class DamageMaker
{
    [Description("Type of the damage.")]
    public DamageType DamageType { get; set; } = DamageType.None;

    [Description("The SubType if exists.")]
    public DamageSubType DamageSubType { get; set; } = DamageSubType.None;

    [Description("The SubType type if exists.")]
    public object SubType { get; set; }
}
