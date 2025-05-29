namespace SimpleCustomRoles.RoleYaml.SCP;

public class Scp939Info
{
    public bool CanCreateCloud { get; set; } = true;

    public MathValue CloudFailCooldown { get; set; } = new();
    public MathValue CloudPlacedCooldown { get; set; } = new();
}
