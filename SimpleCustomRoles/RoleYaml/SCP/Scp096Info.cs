using LabApi.Features.Enums;

namespace SimpleCustomRoles.RoleYaml.SCP;

public class Scp096Info
{
    public bool CanCharge { get; set; } = true;
    public bool CanTryingNotToCry { get; set; } = true;
    public bool CanPry { get; set; } = true;
    public MathValue Enraging { get; set; } = new();
    public List<DoorName> DoorToNotPryOn { get; set; } = [];

    public MathValue AttackCooldown { get; set; } = new();

    public MathValue ChargeDuration { get; set; } = new();
    public MathValue ChargeCooldown { get; set; } = new();

    // TODO: 096 Movement speeds.

    public MathValue DefaultActivationTime { get; set; } = new();

    public MathValue NormalHumeRegenerationRate { get; set; } = new();

    public MathValue MaxRageTime { get; set; } = new();
    public MathValue MinimumEnrageTime { get; set; } = new();
    public MathValue TimePerExtraTarget { get; set; } = new();

    public MathValue CalmingShieldMultiplier { get; set; } = new();
    public MathValue EnragingShieldMultiplier { get; set; } = new();
}
