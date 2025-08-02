using System.ComponentModel;

namespace SimpleCustomRoles.RoleYaml;

public class DamageInfo
{
    [Description("Damage Dictionary that Player Received.")]
    public Dictionary<DamageMaker, MathValueFloat> DamageReceived { get; set; } = [];

    [Description("Damage Dictionary that Player Sent/Dealt.")]
    public Dictionary<DamageMaker, MathValueFloat> DamageDealt { get; set; } = [];
}
