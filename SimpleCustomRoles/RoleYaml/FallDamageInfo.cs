namespace SimpleCustomRoles.RoleYaml;

public class FallDamageInfo
{
    public bool Enabled { get; set; } = true;
    public MathValueFloat MinVelocity { get; set; } = new();
    public MathValueFloat Power { get; set; } = new();
    public MathValueFloat Multiplier { get; set; } = new();
    public MathValueFloat Absolute { get; set; } = new();
    public MathValueFloat ImmunityTime { get; set; } = new();
    public MathValueFloat MaxDamage { get; set; } = new();
}
