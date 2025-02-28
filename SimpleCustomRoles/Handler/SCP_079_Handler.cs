using Exiled.Events.EventArgs.Scp079;
using SimpleCustomRoles.RoleInfo;

namespace SimpleCustomRoles.Handler;

public class SCP_079_Handler
{
    public static void ChangingCamera(ChangingCameraEventArgs args)
    {
        if (!Main.Instance.PlayerCustomRole.TryGetValue(args.Player.UserId, out CustomRoleInfo role))
            return;
        args.AuxiliaryPowerCost = RoleSetter.MathWithFloat(
            role.Scp_Specific.Scp079.ChangingCameraCost.SetType, 
            args.AuxiliaryPowerCost, 
            role.Scp_Specific.Scp079.ChangingCameraCost.AuxiliaryPowerCost);
    }
}
