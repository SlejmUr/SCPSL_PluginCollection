namespace DavaCustomItems.Configs;

public sealed class CoinConfig
{
    public uint Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string CoinPickUpHint { get; set; }
    public CoinExtraConfig ExtraConfig { get; set; } = new();
}
