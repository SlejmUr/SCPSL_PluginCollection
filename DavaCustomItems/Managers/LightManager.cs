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
    static Dictionary<IPosition, CoroutineHandle> PositionToFollower = [];
    static Dictionary<int, CoroutineHandle> RainbowLight = [];

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
        if (lightConfig.IsRainbow)
            MakeRainbow(id);
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
        StopRainbow(LightId);

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

    public static void MakeRainbow(int LightId)
    {
        if (!Lights.TryGetValue(LightId, out TLight light))
            return;
        RainbowLight.Add(LightId, Timing.RunCoroutine(LightRainbow(LightId)));
    }

    public static void StopRainbow(int LightId)
    {
        if (!RainbowLight.TryGetValue(LightId, out CoroutineHandle coroutineHandle))
            return;
        RainbowLight.Remove(LightId);
        Timing.KillCoroutines(coroutineHandle);
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

    private static IEnumerator<float> LightRainbow(int LightId) // Breathing Effect like
    {
        yield return 0;
        byte red = 0;
        byte green = 0;
        byte blue = 0;
        while (Lights.ContainsKey(LightId))
        {
            if (red == 0 && green == 0 && blue == 0)
                red = 255;
            else if (red == 0 && green < 255 && blue == 255)
                green++;
            else if (red == 0 && green == 255 && blue > 0)
                blue--;
            else if (red == 255 && green == 0 && blue < 255)
                blue++;
            else if (red == 255 && green > 0 && blue == 0)
                green--;
            else if (red > 0 && green == 0 && blue == 255)
                red--;
            else if (red < 255 && green == 255 && blue == 0)
                red++;
            Lights[LightId].Color = new Color32(red, green, blue, 1);
            yield return 0;
        }
        yield break;
    }

    private static IEnumerator<float> LightRainbow_Fast(int LightId) // "Rave" Effect like
    {
        yield return 0;
        // step 50 is good
        // step 100 is slow
        // step 10 is so fast
        int numOfSteps = 50;
        int step = 0;
        float r = 0;
        float g = 0;
        float b = 0;
        while (Lights.ContainsKey(LightId))
        {
            if (step > numOfSteps)
                step = 0;
            float h = (float)step / numOfSteps;
            int i = (int)(h * 6);
            float f = h * 6.0f - i;
            float q = 1 - f;
            switch (i % 6)
            {
                case 0:
                    r = 1;
                    g = f;
                    b = 0;
                    break;
                case 1:
                    r = q;
                    g = 1;
                    b = 0;
                    break;
                case 2:
                    r = 0;
                    g = 1;
                    b = f;
                    break;
                case 3:
                    r = 0;
                    g = q;
                    b = 1;
                    break;
                case 4:
                    r = f;
                    g = 0;
                    b = 1;
                    break;
                case 5:
                    r = 1;
                    g = 0;
                    b = q;
                    break;
            }
            Lights[LightId].Color = new Color(r, g, b, 0.6f);
            step++;
            yield return 0;
        }
        yield break;
    }
}
