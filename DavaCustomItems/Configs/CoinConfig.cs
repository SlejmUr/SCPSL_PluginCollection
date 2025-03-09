namespace DavaCustomItems.Configs;

public sealed class CoinConfig
{
    // public CoinRarityType CoinRarity { get; set; }
    public uint Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public CoinExtraConfig ExtraConfig { get; set; } = new();
}
