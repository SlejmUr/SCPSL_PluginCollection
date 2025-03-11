using DavaCustomItems.Configs;
using MEC;
using UnityEngine;

namespace DavaCustomItems.Managers;

public static class RainbowLightManager
{
    static Dictionary<int, CoroutineHandle> RainbowLight = [];

    static Dictionary<RainbowLightType, Func<int, RainbowLightConfig, IEnumerator<float>>> LighTypeToCoroutine = new()
    {
        { RainbowLightType.None, LightRainbowNone },
        { RainbowLightType.Breathing, LightRainbowBreathing },
        { RainbowLightType.Rave, LightRainbowRave }
    };

    public static void Init()
    {
        LightManager.LightAdded += LMLightAdded;
    }

    public static void UnInit()
    {
        LightManager.LightAdded -= LMLightAdded;
    }

    private static void LMLightAdded(int LightId, LightConfig config)
    {
        if (config.RainbowConfig.RainbowType == RainbowLightType.None)
            return;
        MakeRainbow(LightId, config.RainbowConfig);
    }

    public static void MakeRainbow(int LightId, RainbowLightConfig rainbowConfig)
    {
        if (!LightManager.IsLightExists(LightId))
            return;
        RainbowLight.Add(LightId, Timing.RunCoroutine(LighTypeToCoroutine[rainbowConfig.RainbowType].Invoke(LightId, rainbowConfig)));
    }

    public static void StopRainbow(int LightId)
    {
        if (!RainbowLight.TryGetValue(LightId, out CoroutineHandle coroutineHandle))
            return;
        RainbowLight.Remove(LightId);
        Timing.KillCoroutines(coroutineHandle);
    }

    private static IEnumerator<float> LightRainbowNone(int LightId, RainbowLightConfig rainbowConfig)
    {
        yield break;
    }

    private static IEnumerator<float> LightRainbowBreathing(int LightId, RainbowLightConfig rainbowConfig)
    {
        yield return 0;
        byte red = 0;
        byte green = 0;
        byte blue = 0;
        while (LightManager.IsLightExists(LightId))
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
            LightManager.SetLightColor(LightId, new Color32(red, green, blue, rainbowConfig.ByteAlpha));
            yield return 0;
        }
        yield break;
    }

    // step 50 is good
    // step 100 is slow
    // step 10 is so fast
    private static IEnumerator<float> LightRainbowRave(int LightId, RainbowLightConfig rainbowConfig)
    {
        yield return 0;
        int maxSteps = rainbowConfig.Speed;
        int step = 0;
        float r = 0;
        float g = 0;
        float b = 0;
        while (LightManager.IsLightExists(LightId))
        {
            if (step > maxSteps)
                step = 0;
            float h = (float)step / maxSteps;
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
            LightManager.SetLightColor(LightId, new(r, g, b, rainbowConfig.FloatAlpha));
            step++;
            yield return 0;
        }
        yield break;
    }
}
