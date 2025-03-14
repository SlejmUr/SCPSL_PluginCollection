using Exiled.API.Features;

namespace DavaCustomItems.Managers;

public static class LightSerialManager
{

    private static Dictionary<ushort, int> SerialToLightId = [];

    public static int GetLightId(ushort serial)
    {
        if (SerialToLightId.TryGetValue(serial, out int LightId))
            return LightId;
        return -1;
    }

    public static bool HasSerial(ushort serial)
    {
        if (serial == ushort.MaxValue)
            return false;
        return SerialToLightId.ContainsKey(serial);
    }

    public static void AddLight(ushort serial, int lightId)
    {
        if (HasSerial(serial))
        {
            return;
        }
        SerialToLightId.Add(serial, lightId);
    }

    public static void RemoveLight(ushort serial)
    {
        SerialToLightId.Remove(serial);
    }
}
