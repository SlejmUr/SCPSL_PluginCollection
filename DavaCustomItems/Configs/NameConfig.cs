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

    public NameConfig(string actionName, bool isTails)
    {
        ActionName = actionName;
        IsTails = isTails;
        ExtraSettingsParameter = string.Empty;
        UseWeight = false;
    }
}