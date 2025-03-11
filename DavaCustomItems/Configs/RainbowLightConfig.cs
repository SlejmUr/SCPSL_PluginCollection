using System.ComponentModel;

namespace DavaCustomItems.Configs;

public sealed class RainbowLightConfig
{
    /// <summary>
    /// Rainbow Light Type
    /// </summary>
    public RainbowLightType RainbowType { get; set; } = RainbowLightType.None;

    /// <summary>
    /// Less mean faster, More means slower
    /// </summary>
    [Description("Less mean faster, More means slower")]
    public int Speed { get; set; } = 50;

    /// <summary>
    /// Used in only Breathing
    /// </summary>
    [Description("Used in only Breathing")]
    public byte ByteAlpha { get; set; } = 1;

    /// <summary>
    /// Used in only Rave
    /// </summary>
    [Description("Used in only Rave")]
    public float FloatAlpha { get; set; } = 0.6f;
}

public enum RainbowLightType
{
    None,
    Breathing,
    Rave
}