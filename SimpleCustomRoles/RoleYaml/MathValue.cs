using SimpleCustomRoles.RoleYaml.Enums;
using System.ComponentModel;

namespace SimpleCustomRoles.RoleYaml;

public class MathValue
{
    public float Value { get; set; } = 0;

    [Description("Value's Math option. Check MathOptions.txt.")]
    public MathOption Math { get; set; } = MathOption.None;
}


public class MathValueInt
{
    public int Value { get; set; } = 0;

    [Description("Value's Math option. Check MathOptions.txt.")]
    public MathOption Math { get; set; } = MathOption.None;
}
