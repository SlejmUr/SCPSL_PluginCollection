namespace SimpleCustomRoles.RoleYaml.SCP;

public class Scp939Info
{
    public bool CanCreateCloud { get; set; } = true;

    public MathValueFloat CloudFailCooldown { get; set; } = new();
    public MathValueFloat CloudPlacedCooldown { get; set; } = new();
}
