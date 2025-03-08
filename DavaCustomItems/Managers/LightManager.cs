using MEC;
using Exiled.API.Features;
using UnityEngine;
using DavaCustomItems.Configs;
using DavaCustomItems.Components;
using TLight = Exiled.API.Features.Toys.Light;

namespace DavaCustomItems.Managers;

public static class LightManager
{
    static Dictionary<int, TLight> Lights = [];
    static Dictionary<Player, CoroutineHandle> PlayerToFollower = [];

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
        //ligth.Color = lightConfig.Color;
        ligth.LightShape = lightConfig.LightShape;
        ligth.LightType = lightConfig.LightType;
        ligth.ShadowType = lightConfig.ShadowType;
        ligth.MovementSmoothing = lightConfig.MovementSmoothing;
        //ligth.Scale = lightConfig.Scale;
        int id = Lights.Count;
        id++;
        var lcomponent = ligth.GameObject.AddComponent<LightConfigComponent>();
        lcomponent.LightConfig = lightConfig;
        lcomponent.LightId = id;
        Lights.Add(id, ligth);
        return id;

    }
    
    public static void LightFollowPlayer(int LightId, Player player)
    {
        StopFollowPlayer(player);
        if (Lights.ContainsKey(LightId))
            PlayerToFollower.Add(player, Timing.RunCoroutine(LightMove(LightId, player)));
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

    public static void StopFollowPlayer(Player player)
    {
        if (!PlayerToFollower.TryGetValue(player, out CoroutineHandle coroutineHandle))
            return;
        PlayerToFollower.Remove(player);
        Timing.KillCoroutines(coroutineHandle);
        
    }

    public static void SetNewLightPos(int LightId, Vector3 pos)
    {
        if (!Lights.TryGetValue(LightId, out TLight light))
            return;
        light.Position = pos;
    }

    private static IEnumerator<float> LightMove(int LightId, Player player)
    {
        yield return 0;
        while (Lights.ContainsKey(LightId) && PlayerToFollower.ContainsKey(player))
        {
            TLight light = Lights[LightId];
            light.Position = player.Position;
            yield return 0;
        }
        yield break;
    }
}
