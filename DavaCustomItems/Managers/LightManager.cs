using MEC;
using UnityEngine;
using DavaCustomItems.Configs;
using DavaCustomItems.Components;
using TLight = Exiled.API.Features.Toys.Light;
using Exiled.API.Interfaces;
using Exiled.API.Features;

namespace DavaCustomItems.Managers;

public static class LightManager
{
    static Dictionary<int, TLight> Lights = [];
    static Dictionary<IPosition, KeyValuePair<CoroutineHandle, int>> PositionToFollower = [];

    public static Action<int> LightRemoved;
    public static Action<int, LightConfig> LightAdded;

    public static IReadOnlyList<int> GetLightIds()
    {
        return [.. Lights.Keys];
    }

    public static int MakeLight(Vector3 Position, LightConfig lightConfig, bool shouldSpawn = true)
    {
        if (!lightConfig.ShouldMakeLight)
            return -1;
        var ligth = TLight.Create(Position, Vector3.zero, lightConfig.Scale, shouldSpawn, lightConfig.Color);
        ligth.Intensity = lightConfig.Intensity;
        ligth.Range = lightConfig.Range;
        ligth.SpotAngle = lightConfig.SpotAngle;
        ligth.InnerSpotAngle = lightConfig.InnerSpotAngle;
        ligth.ShadowStrength = lightConfig.ShadowStrength;
        ligth.LightShape = lightConfig.LightShape;
        ligth.LightType = lightConfig.LightType;
        ligth.ShadowType = lightConfig.ShadowType;
        ligth.MovementSmoothing = lightConfig.MovementSmoothing;
        int id = TryGetNewID(RNGManager.RNG.Next());
        var lcomponent = ligth.GameObject.AddComponent<LightConfigComponent>();
        lcomponent.LightConfig = lightConfig;
        lcomponent.LightId = id;
        lcomponent.IsSpawned = shouldSpawn;
        Lights.Add(id, ligth);
        LightAdded?.Invoke(id, lightConfig);
        return id;
    }

    private static int TryGetNewID(int id)
    {
        if (Lights.ContainsKey(id))
            id = RNGManager.RNG.Next();
        else
            return id;
        return TryGetNewID(id);
    }

    public static void ChangeInternalSettings(int LightId, LightConfig lightConfig)
    {
        if (!Lights.TryGetValue(LightId, out TLight light))
            return;
        light.GameObject.GetComponent<LightConfigComponent>().LightConfig = lightConfig;
    }

    public static void ApplyInternalSettings(int LightId)
    {
        if (!Lights.TryGetValue(LightId, out TLight light))
            return;
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
        Log.Info($"{LightId} unspawned");
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
        ApplyInternalSettings(LightId);
        Log.Info($"{LightId} spawned");
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

    public static int MakeLightAndFollow(IPosition position, LightConfig lightConfig)
    {
        int id = MakeLight(position.Position, lightConfig);
        if (id == -1)
            return id;
        StartFollow(id, position);
        return id;
    }

    public static void StartFollow(int LightId, IPosition position)
    {
        if (LightId == -1)
            return;
        if (!IsLightShown(LightId))
            ShowLight(LightId);
        StopFollow(position, false);
        if (Lights.ContainsKey(LightId))
            PositionToFollower.Add(position, new(Timing.RunCoroutine(LightMove(LightId, position)), LightId));
    }
    

    public static void StopFollow(IPosition position, bool shouldHide = true)
    {
        if (!PositionToFollower.TryGetValue(position, out var coroutineHandle))
            return;
        PositionToFollower.Remove(position);
        Timing.KillCoroutines(coroutineHandle.Key);
        if (shouldHide)
            HideLight(coroutineHandle.Value);
    }

    public static void StopFollowAndStartFollow(IPosition oldFollower, IPosition newFollower)
    {
        if (!PositionToFollower.TryGetValue(oldFollower, out var follower))
            return;
        PositionToFollower.Remove(oldFollower);
        Timing.KillCoroutines(follower.Key);
        StartFollow(follower.Value, newFollower);
    }

    public static void StopFollowAndStartNew(IPosition oldFollower, IPosition newFollower, ref int LightId, LightConfig lightConfig, bool makeNewIfNotExists = true)
    {
        StopFollow(oldFollower);
        if (!Lights.ContainsKey(LightId) && makeNewIfNotExists)
            LightId = MakeLight(newFollower.Position, lightConfig);
        if (Lights.ContainsKey(LightId))
            StartFollow(LightId, newFollower);
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

    private static IEnumerator<float> LightMove(int LightId, IPosition position)
    {
        yield return 0;
        while (Lights.ContainsKey(LightId) && PositionToFollower.ContainsKey(position))
        {
            Lights[LightId].Position = position.Position;
            yield return 0;
        }
        yield break;
    }
}
