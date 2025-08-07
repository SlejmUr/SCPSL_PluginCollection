using LabApi.Features.Enums;

namespace SimpleCustomRoles.RoleYaml.SCP;

public class Scp096Info
{
    public bool CanCharge { get; set; } = true;
    public bool CanTryingNotToCry { get; set; } = true;
    public bool CanPry { get; set; } = true;
    public MathValueFloat Enraging { get; set; } = new();
    public List<DoorName> DoorToNotPryOn { get; set; } = [];

    public MathValueFloat AttackCooldown { get; set; } = new();

    public MathValueFloat ChargeDuration { get; set; } = new();
    public MathValueFloat ChargeCooldown { get; set; } = new();

    // TODO: 096 Movement speeds.

    public MathValueFloat DefaultActivationTime { get; set; } = new();

    public MathValueFloat NormalHumeRegenerationRate { get; set; } = new();

    public MathValueFloat MaxRageTime { get; set; } = new();
    public MathValueFloat MinimumEnrageTime { get; set; } = new();
    public MathValueFloat TimePerExtraTarget { get; set; } = new();

    public MathValueFloat CalmingShieldMultiplier { get; set; } = new();
    public MathValueFloat EnragingShieldMultiplier { get; set; } = new();
}
