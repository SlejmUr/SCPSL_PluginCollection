using System.ComponentModel;

namespace SimpleCustomRoles.RoleYaml;

public class EffectInfo
{
    [Description("Effect can be removed with SCP-500.")]
    public bool Removable { get; set; } = true;

    [Description("Can this effect be usable on person")]
    public bool CanEnable { get; set; } = true;

    [Description("Effect Type to add into the user.")]
    public string EffectName { get; set; }

    [Description("Duration how long the effect should last.")]
    public float Duration { get; set; }

    [Description("Intensity of the effect.")]
    public byte Intensity { get; set; }
}
