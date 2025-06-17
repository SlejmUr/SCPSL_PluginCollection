using System.ComponentModel;

namespace SimpleCustomRoles.RoleYaml.SCP;

public class Scp106Info
{
    public MathValue AttackHitCooldown { get; set; } = new();
    public MathValue AttackMissCooldown { get; set; } = new();
    public MathValueInt AttackDamage { get; set; } = new();

    public MathValue CorrodingEffect { get; set; } = new();

    public MathValue VigorBonus { get; set; } = new();

    [Description("Use negative number!")]
    public MathValue SinkholeCooldownBonus { get; set; } = new();

    public MathValue VigorCostPerMinute { get; set; } = new();
    public MathValue MinVigorAtlas { get; set; } = new();

    public MathValue StalkVigorRegeneration { get; set; } = new();
    public MathValue StalkCostStationary { get; set; } = new();
    public MathValue StalkCostMoving { get; set; } = new();

    public MathValue MinVigorSubmerge { get; set; } = new();
    public MathValue MovementRange { get; set; } = new();
    public MathValue MovementTimer { get; set; } = new();

}
