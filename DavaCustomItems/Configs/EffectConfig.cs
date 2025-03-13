using Exiled.API.Enums;
using Exiled.API.Features;

namespace DavaCustomItems.Configs;

public sealed class EffectConfig
{
    public EffectType EffectType { get; set; }
    public float Duration { get; set; }
    public byte Intensity { get; set; }

    public EffectConfig()
    {

    }

    public EffectConfig(EffectType effectType, byte intensity = 1, float duration = 0f)
    {
        EffectType = effectType;
        Intensity = intensity;
        Duration = duration;
    }

    public EffectConfig(object obj)
    {
        if (obj is not Dictionary<object, object> dict)
            return;
        foreach (var item in dict)
        {
            string kstr = (string)item.Key;
            string vstr = (string)item.Value;
            if (kstr.Contains("effect_type"))
            {
                if (!Enum.TryParse(vstr, true, out EffectType result))
                    continue;
                EffectType = result;
            }
            if (kstr.Contains("duration"))
            {
                if (!float.TryParse(vstr, out var result))
                    continue;
                Duration = result;
            }
            if (kstr.Contains("intensity"))
            {
                if (!byte.TryParse(vstr, out var result))
                    continue;
                Intensity = result;
            }
        }
    }
}
