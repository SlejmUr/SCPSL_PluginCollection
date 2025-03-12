namespace DavaCustomItems.Configs;

public sealed class NameConfig
{
    public string ActionName { get; set; }
    public bool IsTails { get; set; }
    public string ExtraSettingsParameter { get; set; }
    public bool UseWeight { get; set; }

    public NameConfig()
    {

    }

    public NameConfig(string actionName, bool isTails, string extraSettings, bool useWeight)
    {
        ActionName = actionName;
        IsTails = isTails;
        ExtraSettingsParameter = extraSettings;
        UseWeight = useWeight;
    }
}

public sealed class CoinExtraConfig
{
    public float CoinBrakeChance { get; set; } = 0.10f;
    public int MaxFlipping { get; set; } = 3;

    public Dictionary<NameConfig, int> NameAndWeight { get; set; } = [];

    public Dictionary<string, List<object>> ExtraSettings { get; set; } = [];

    public Dictionary<string, Dictionary<List<object>, int>> ExtraSettingsAndWeight { get; set; } = [];
}
