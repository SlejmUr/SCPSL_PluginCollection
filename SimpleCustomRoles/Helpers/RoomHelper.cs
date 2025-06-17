using LabApi.Features.Wrappers;

namespace SimpleCustomRoles.Helpers;

public static class RoomHelper
{
    public static UnityEngine.Vector3 AdjustRoomPosition(this Room Room)
    {
        UnityEngine.Vector3 Position = Room.Position;
        Position.y += 1.5f;
        return Position;
    }
}
