using SimpleCustomRoles.RoleInfo;
using System.ComponentModel;

namespace SimpleCustomRoles.RoleYaml;

public class DamageInfo
{
    [Description("Damage Dictionary that Player Received.")]
    public Dictionary<DamageMaker, MathValue> DamageReceived { get; set; } = [];

    [Description("Damage Dictionary that Player Sent/Dealt.")]
    public Dictionary<DamageMaker, MathValue> DamageDealt { get; set; } = [];
}
