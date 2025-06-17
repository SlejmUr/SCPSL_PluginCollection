namespace SimpleCustomRoles.RoleYaml.SCP;

public class Scp173Info
{
    public bool CanPlaceTantrum { get; set; } = true;
    public bool CanUseBreakneckSpeed { get; set; } = true;


    public MathValue BlinkCooldown { get; set; } = new();
    public MathValue BlinkSustainTime { get; set; } = new();

    public MathValue BreakneckRechargeTime { get; set; } = new();
    public MathValue BreakneckStareLimit { get; set; } = new();

    public MathValue TantrumCooldown { get; set; } = new();

    public MathValue BlinkDistance { get; set; } = new();
    public MathValue BreakneckDistanceMultiplier { get; set; } = new();
}
