namespace SimpleCustomRoles.RoleYaml.SCP;

public class Scp049Info : NewRoleInfo
{
    public bool CanRecall { get; set; } = true;

    public MathValueFloat SenseBaseCooldown { get; set; } = new();
    public MathValueFloat SenseTargetLostCooldown { get; set; } = new();
    public MathValueFloat SenseAttemptFailCooldown { get; set; } = new();
    public MathValueFloat SenseEffectDuration { get; set; } = new();

    public MathValueInt ResurrectMaxResurrection { get; set; } = new();
    public MathValueFloat ResurrectHumanCorpseDuration { get; set; } = new();
    public MathValueFloat ResurrectTargetCorpseDuration { get; set; } = new();

    public MathValueFloat CallBaseCooldown { get; set; } = new();
    public MathValueFloat CallEffectDuration { get; set; } = new();

    public MathValueFloat AttackCooldownTime { get; set; } = new();
    public MathValueFloat AttackDistance { get; set; } = new();
    public MathValueFloat AttackEffectDuration { get; set; } = new();
}
