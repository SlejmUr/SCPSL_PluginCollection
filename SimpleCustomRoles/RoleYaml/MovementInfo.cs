namespace SimpleCustomRoles.RoleYaml;

public class MovementInfo
{
    public MathValue CrouchSpeed { get; set; } = new();
    public MathValue SneakSpeed { get; set; } = new();
    public MathValue WalkSpeed { get; set; } = new();
    public MathValue SprintSpeed { get; set; } = new();
    public MathValue JumpSpeed { get; set; } = new();
}
