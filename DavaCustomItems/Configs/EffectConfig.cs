using Exiled.API.Enums;

namespace DavaCustomItems.Configs;

public sealed class EffectConfig
{
    public EffectType EffectType { get; set; }
    public float Duration { get; set; }
    public byte Intensity { get; set; }

    public EffectConfig()
    {

    }

    public EffectConfig(EffectType effectType)
    {
        EffectType = effectType;
        Duration = 0;
        Intensity = 1;
    }

    public EffectConfig(EffectType effectType, float duration, byte intensity)
    {
        EffectType = effectType;
        Duration = duration;
        Intensity = intensity;
    }
}
