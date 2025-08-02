namespace SimpleCustomRoles.RoleYaml.SCP;

public class Scp173Info
{
    public bool CanPlaceTantrum { get; set; } = true;
    public bool CanUseBreakneckSpeed { get; set; } = true;


    public MathValueFloat BlinkCooldown { get; set; } = new();
    public MathValueFloat BlinkSustainTime { get; set; } = new();

    public MathValueFloat BreakneckRechargeTime { get; set; } = new();
    public MathValueFloat BreakneckStareLimit { get; set; } = new();

    public MathValueFloat TantrumCooldown { get; set; } = new();

    public MathValueFloat BlinkDistance { get; set; } = new();
    public MathValueFloat BreakneckDistanceMultiplier { get; set; } = new();
}
