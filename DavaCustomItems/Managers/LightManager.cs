using MEC;
using UnityEngine;
using DavaCustomItems.Configs;
using DavaCustomItems.Components;
using TLight = Exiled.API.Features.Toys.Light;
using Exiled.API.Interfaces;

namespace DavaCustomItems.Managers;

public static class LightManager
{
    static Dictionary<int, TLight> Lights = [];
    static Dictionary<IPosition, CoroutineHandle> PositionToFollower = [];

    public static int MakeLight(Vector3 Position, LightConfig lightConfig)
    {
        if (!lightConfig.ShouldMakeLight)
            return -1;
        var ligth = TLight.Create(Position, Vector3.zero, lightConfig.Scale, true, lightConfig.Color);
        ligth.Intensity = lightConfig.Intensity;
        ligth.Range = lightConfig.Range;
        ligth.SpotAngle = lightConfig.SpotAngle;
        ligth.InnerSpotAngle = lightConfig.InnerSpotAngle;
        ligth.ShadowStrength = lightConfig.ShadowStrength;
        ligth.LightShape = lightConfig.LightShape;
        ligth.LightType = lightConfig.LightType;
        ligth.ShadowType = lightConfig.ShadowType;
        ligth.MovementSmoothing = lightConfig.MovementSmoothing;
        int id = Lights.Count;
        id++;
        var lcomponent = ligth.GameObject.AddComponent<LightConfigComponent>();
        lcomponent.LightConfig = lightConfig;
        lcomponent.LightId = id;
        Lights.Add(id, ligth);
        return id;

    }
    public static bool IsLightExists(int LightId)
    {
        return Lights.ContainsKey(LightId);
    }


    public static void RemoveLight(int LightId)
    {
        if (!Lights.TryGetValue(LightId, out TLight light))
            return;
        light.Destroy();
        Lights.Remove(LightId);

    }

    public static void HideLight(int LightId)
    {
        if (!Lights.TryGetValue(LightId, out TLight light))
            return;
        light.UnSpawn();
    }

    public static void ShowLight(int LightId)
    {
        if (!Lights.TryGetValue(LightId, out TLight light))
            return;
        light.Spawn();
    }

    public static int MakeLightAndFollow(IPosition position, LightConfig lightConfig)
    {
        int id = MakeLight(position.Position, lightConfig);
        if (id == -1)
            return id;
        LightFollow(id, position);
        return id;
    }

    public static void LightFollow(int LightId, IPosition position)
    {
        StopFollow(position);
        if (Lights.ContainsKey(LightId))
            PositionToFollower.Add(position, Timing.RunCoroutine(LightMove(LightId, position)));
    }
    

    public static void StopFollow(IPosition position)
    {
        if (!PositionToFollower.TryGetValue(position, out CoroutineHandle coroutineHandle))
            return;
        PositionToFollower.Remove(position);
        Timing.KillCoroutines(coroutineHandle);
        
    }

    public static void StopFollowAndStartNew(IPosition oldFollower, IPosition newFollower, ref int LightId, LightConfig lightConfig, bool makeNewIfNotExists = true)
    {
        StopFollow(oldFollower);
        if (!Lights.ContainsKey(LightId) && makeNewIfNotExists)
            LightId = MakeLight(newFollower.Position, lightConfig);
        if (Lights.ContainsKey(LightId))
            LightFollow(LightId, newFollower);
    }

    public static void SetNewLightPos(int LightId, Vector3 pos)
    {
        if (!Lights.TryGetValue(LightId, out TLight light))
            return;
        light.Position = pos;
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
