namespace DavaCustomItems.Configs;

public sealed class CoinConfig
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string CoinPickUpHint { get; set; }
    public CoinExtraConfig ExtraConfig { get; set; } = new();
}
