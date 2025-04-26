namespace SimpleCustomRoles.RoleYaml;

public class FallDamageInfo
{
    public bool Enabled { get; set; } = true;
    public MathValue MinVelocity { get; set; } = new();
    public MathValue Power { get; set; } = new();
    public MathValue Multiplier { get; set; } = new();
    public MathValue Absolute { get; set; } = new();
    public MathValue ImmunityTime { get; set; } = new();
    public MathValue MaxDamage { get; set; } = new();
}
