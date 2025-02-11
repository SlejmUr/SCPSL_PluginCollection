using LabApi.Features.Wrappers;
using SimpleCustomRoles.RoleInfo;

namespace SimpleCustomRoles;

public static class Extenstions
{
    public static UnityEngine.Vector3 ConvertFromV3(this V3 v3)
    {
        return new UnityEngine.Vector3() 
        { 
            x = v3.X,
            y = v3.Y,
            z = v3.Z
        };

    }
    public static UnityEngine.Vector3 AdjustRoomPosition(this Room Room)
    {
        UnityEngine.Vector3 Position = Room.Position;
        Position.y += 1.5f;
        return Position;
    }
}
