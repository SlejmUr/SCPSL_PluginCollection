using LabApi.Features.Enums;

namespace SimpleCustomRoles.RoleYaml.SCP;

public class Scp096Info
{
    public bool CanCharge { get; set; } = true;
    public bool CanTryingNotToCry { get; set; } = true;
    public bool CanPry { get; set; } = true;
    public MathValue Enraging { get; set; } = new();
    public List<DoorName> DoorToNotPryOn { get; set; } = [];
}
