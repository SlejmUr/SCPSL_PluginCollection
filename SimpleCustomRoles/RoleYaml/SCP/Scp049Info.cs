namespace SimpleCustomRoles.RoleYaml.SCP;

public class Scp049Info : NewRoleInfo
{
    public bool CanRecall { get; set; } = true;

    public MathValue SenseBaseCooldown { get; set; } = new();
    public MathValue SenseTargetLostCooldown { get; set; } = new();
    public MathValue SenseAttemptFailCooldown { get; set; } = new();
    public MathValue SenseEffectDuration { get; set; } = new();

    public MathValueInt ResurrectMaxResurrection { get; set; } = new();
    public MathValue ResurrectHumanCorpseDuration { get; set; } = new();
    public MathValue ResurrectTargetCorpseDuration { get; set; } = new();

    public MathValue CallBaseCooldown { get; set; } = new();
    public MathValue CallEffectDuration { get; set; } = new();

    public MathValue AttackCooldownTime { get; set; } = new();
    public MathValue AttackDistance { get; set; } = new();
    public MathValue AttackEffectDuration { get; set; } = new();
}
