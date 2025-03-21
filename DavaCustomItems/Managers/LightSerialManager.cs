namespace DavaCustomItems.Managers;

public static class LightSerialManager
{
    private static Dictionary<ushort, int> SerialToLightIds = [];
    public static void Init()
    {
        LightManager.LightRemoved += LMRemoved;
    }

    public static void UnInit()
    {
        LightManager.LightRemoved -= LMRemoved;
    }
    /// <summary>
    /// Private field, only exist is to remove the Id from the serial
    /// </summary>
    /// <param name="LightId"></param>
    private static void LMRemoved(int LightId)
    {
        if (!SerialToLightIds.Any(x => x.Value == LightId))
            return;
        var x = SerialToLightIds.First(x=>x.Value == LightId);
        if (x.Key == default)
            return;
        SerialToLightIds.Remove(x.Key);
    }

    /// <summary>
    /// Getting current LightId from the <paramref name="serial"/>
    /// </summary>
    /// <param name="serial"></param>
    /// <returns></returns>
    public static int GetLightId(ushort serial)
    {
        if (SerialToLightIds.TryGetValue(serial, out int LightId))
            return LightId;
        return -1;
    }

    /// <summary>
    /// Checking if <paramref name="serial"/> exist in <see cref="SerialToLightIds"/>
    /// </summary>
    /// <param name="serial"></param>
    /// <returns></returns>
    public static bool HasSerial(ushort serial)
    {
        if (serial == ushort.MaxValue)
            return false;
        return SerialToLightIds.ContainsKey(serial);
    }

    /// <summary>
    /// Adding <paramref name="serial"/> and <paramref name="lightId"/> to <see cref="SerialToLightIds"/>
    /// </summary>
    /// <param name="serial"></param>
    /// <param name="lightId"></param>
    public static void AddLight(ushort serial, int lightId)
    {
        if (HasSerial(serial))
            return;
        SerialToLightIds.Add(serial, lightId);
    }

    /// <summary>
    /// Removing <paramref name="serial"/> from <see cref="SerialToLightIds"/>
    /// </summary>
    /// <param name="serial"></param>
    public static void RemoveLight(ushort serial)
    {
        SerialToLightIds.Remove(serial);
    }
}
