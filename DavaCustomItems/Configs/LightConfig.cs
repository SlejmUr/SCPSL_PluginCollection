using UnityEngine;

namespace DavaCustomItems.Configs;

public class LightConfig
{
    public bool ShouldFollowPlayer { get; set; }
    public bool ShouldMakeLight { get; set; }
    public bool IsRainbow { get; set; }
    //public bool ShouldHideInitially { get; set; }
    public float Intensity { get; set; }
    public float Range { get; set; }
    public float SpotAngle { get; set; }
    public float InnerSpotAngle { get; set; }
    public float ShadowStrength { get; set; }
    public Color Color { get; set; } = Color.white;
    public LightShape LightShape { get; set; } = LightShape.Box;
    public LightType LightType { get; set; } = LightType.Area;
    public LightShadows ShadowType { get; set; } = LightShadows.None;
    public byte MovementSmoothing { get; set; } = 60;
    public Vector3 Scale { get; set; } = Vector3.one;
}
