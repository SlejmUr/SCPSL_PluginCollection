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

    public EffectConfig(EffectType effectType, byte intensity = 1, float duration = 0f)
    {
        EffectType = effectType;
        Intensity = intensity;
        Duration = duration;
    }
}
