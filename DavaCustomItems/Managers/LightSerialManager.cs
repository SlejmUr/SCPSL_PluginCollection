using Exiled.API.Features;

namespace DavaCustomItems.Managers;

public static class LightSerialManager
{

    private static Dictionary<ushort, int> SerialToLightId = [];

    public static int GetLightId(ushort serial)
    {
        int id = GetLightId_Internal(serial);
        Log.Info($"{serial} got light id: {id}");
        return id;
    }

    public static bool HasSerial(ushort serial)
    {
        if (serial == ushort.MaxValue)
            return false;
        var ret = HasSerial_Internal(serial);
        Log.Info($"{serial} is contained: {ret}");
        return ret;
    }

    public static void AddLight(ushort serial, int lightId)
    {
        if (HasSerial_Internal(serial))
        {
            Log.Info($"{serial} has already have light");
            return;
        }
        Log.Info($"{serial} has adding light {lightId}");
        SerialToLightId.Add(serial, lightId);
    }

    public static void RemoveLight(ushort serial)
    {
        SerialToLightId.Remove(serial);
        Log.Info($"{serial} has removed a light");
    }

    private static int GetLightId_Internal(ushort serial)
    {
        if (SerialToLightId.TryGetValue(serial, out int LightId))
            return LightId;
        return -1;
    }

    private static bool HasSerial_Internal(ushort serial)
    {
        return SerialToLightId.ContainsKey(serial);
    }
}
