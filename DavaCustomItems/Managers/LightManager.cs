using UnityEngine;
using DavaCustomItems.Configs;
using DavaCustomItems.Components;
using TLight = Exiled.API.Features.Toys.Light;
using Exiled.API.Features;

namespace DavaCustomItems.Managers;

public static class LightManager
{
    public static Action<int> LightRemoved;
    public static Action<int, LightConfig> LightAdded;

    static Dictionary<int, TLight> Lights = [];

    public static IReadOnlyList<int> GetLightIds()
    {
        return [.. Lights.Keys];
    }

    public static int MakeLight(Vector3 Position, LightConfig lightConfig, bool shouldSpawn = true)
    {
        if (!lightConfig.ShouldMakeLight)
            return -1;
        var light = TLight.Create(Position, Vector3.zero, lightConfig.Scale, shouldSpawn, lightConfig.Color);
        int id = TryGetNewID(RNGManager.RNG.Next());
        var lcomponent = light.GameObject.AddComponent<LightConfigComponent>();
        lcomponent.LightConfig = lightConfig;
        lcomponent.LightId = id;
        lcomponent.IsSpawned = shouldSpawn;
        ApplyInternalSettings_TLight(light);
        Lights.Add(id, light);
        LightAdded?.Invoke(id, lightConfig);
        return id;
    }

    public static void ChangeInternalSettings(int LightId, LightConfig lightConfig)
    {
        if (!Lights.TryGetValue(LightId, out TLight light))
            return;
        light.GameObject.GetComponent<LightConfigComponent>().LightConfig = lightConfig;
        ApplyInternalSettings_TLight(light);
    }

    public static void ApplyInternalSettings(int LightId)
    {
        if (!Lights.TryGetValue(LightId, out TLight light))
            return;
        ApplyInternalSettings_TLight(light);
    }

    public static bool IsLightExists(int LightId)
    {
        if (LightId == -1)
            return false;
        return Lights.ContainsKey(LightId);
    }

    public static void RemoveLight(int LightId)
    {
        if (LightId == -1)
            return;
        if (!Lights.TryGetValue(LightId, out TLight light))
            return;
        Log.Info($"RemoveLight: {LightId}");
        Lights.Remove(LightId);
        LightRemoved?.Invoke(LightId);
        // Bug happens here?
        if (light == null)
            return;
        if (light.GameObject == null)
            return;
        light.GameObject.GetComponent<LightConfigComponent>().IsSpawned = false;
        light.Destroy();

    }

    public static void HideLight(int LightId)
    {
        if (LightId == -1)
            return;
        if (!Lights.TryGetValue(LightId, out TLight light))
        {
            Log.Info($"{LightId} cannot found in lights to turn off/unspawn");
            return;
        }
        light.GameObject.GetComponent<LightConfigComponent>().IsSpawned = false;
        light.UnSpawn();
    }

    public static void ShowLight(int LightId)
    {
        if (LightId == -1)
            return;
        if (!Lights.TryGetValue(LightId, out TLight light))
        {
            Log.Info($"{LightId} cannot found in lights to turn on/spawn");
            return;
        }
        light.Spawn();
        light.GameObject.GetComponent<LightConfigComponent>().IsSpawned = true;
        ApplyInternalSettings_TLight(light);
    }

    public static bool IsLightShown(int LightId)
    {
        if (LightId == -1)
            return false;
        if (!Lights.TryGetValue(LightId, out TLight light))
            return false;
        return light.GameObject.GetComponent<LightConfigComponent>().IsSpawned;;
    }

    // Can return null!
    public static TLight GetLight(int LightId)
    {
        if (LightId == -1)
            return null;
        if (Lights.TryGetValue(LightId, out TLight light))
            return light;
        return null;
    }

    public static LightConfig GetLightConfig(int LightId)
    {
        if (LightId == -1)
            return new();
        if (!Lights.TryGetValue(LightId, out TLight light))
            return new();
        return light.GameObject.GetComponent<LightConfigComponent>().LightConfig;
    }

    public static void SetLightPos(int LightId, Vector3 pos)
    {
        if (LightId == -1)
            return;
        if (!Lights.TryGetValue(LightId, out TLight light))
            return;
        light.Position = pos;
    }

    public static void SetLightColor(int LightId, Color color)
    {
        if (LightId == -1)
            return;
        if (!Lights.TryGetValue(LightId, out TLight light))
            return;
        light.Color = color;
        light.GameObject.GetComponent<LightConfigComponent>().LightConfig.Color = color;
    }

    #region Private
    private static int TryGetNewID(int id)
    {
        if (Lights.ContainsKey(id))
            id = RNGManager.RNG.Next();
        else
            return id;
        return TryGetNewID(id);
    }

    private static void ApplyInternalSettings_TLight(TLight light)
    {
        var config = light.GameObject.GetComponent<LightConfigComponent>().LightConfig;
        light.Intensity = config.Intensity;
        light.Range = config.Range;
        light.SpotAngle = config.SpotAngle;
        light.InnerSpotAngle = config.InnerSpotAngle;
        light.ShadowStrength = config.ShadowStrength;
        light.LightShape = config.LightShape;
        light.LightType = config.LightType;
        light.ShadowType = config.ShadowType;
        light.MovementSmoothing = config.MovementSmoothing;
        light.Scale = config.Scale;
        light.Color = config.Color;
    }
    #endregion
}
