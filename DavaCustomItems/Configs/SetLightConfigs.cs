namespace DavaCustomItems.Configs;

public static class SetLightConfigs
{
    public static LightConfig Rave_LightConfig => new()
    { 
        ShouldFollowPlayer = true,
        ShouldFollowPickup = true,
        ShouldMakeLight = true,
        ShouldShowLightOnSpawn = true,
        Intensity = 10,
        Range = 5,
        RainbowConfig = new()
        {
            RainbowType = RainbowLightType.Rave,
            FloatAlpha = 0.6f,
            Speed = 100
        }
    };

    public static LightConfig Breathing_LightConfig => new()
    {
        ShouldFollowPlayer = true,
        ShouldFollowPickup = true,
        ShouldMakeLight = true,
        ShouldShowLightOnSpawn = true,
        Intensity = 10,
        Range = 5,
        RainbowConfig = new()
        {
            RainbowType = RainbowLightType.Breathing,
            ByteAlpha = 1,
        }
    };
}
