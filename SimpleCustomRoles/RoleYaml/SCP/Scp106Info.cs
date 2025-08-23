using System.ComponentModel;

namespace SimpleCustomRoles.RoleYaml.SCP;

public class Scp106Info
{
    public MathValueFloat AttackHitCooldown { get; set; } = new();
    public MathValueFloat AttackMissCooldown { get; set; } = new();
    public MathValueInt AttackDamage { get; set; } = new();

    public MathValueFloat CorrodingEffect { get; set; } = new();

    public MathValueFloat VigorBonus { get; set; } = new();

    [Description("Use negative number!")]
    public MathValueFloat SinkholeCooldownBonus { get; set; } = new();

    public MathValueFloat VigorCostPerMinute { get; set; } = new();
    public MathValueFloat MinVigorAtlas { get; set; } = new();

    public MathValueFloat StalkVigorRegeneration { get; set; } = new();
    public MathValueFloat StalkCostStationary { get; set; } = new();
    public MathValueFloat StalkCostMoving { get; set; } = new();

    public MathValueFloat MinVigorSubmerge { get; set; } = new();
    public MathValueFloat MovementRange { get; set; } = new();
    public MathValueFloat MovementTimer { get; set; } = new();

}
