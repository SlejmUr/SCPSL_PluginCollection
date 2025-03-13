namespace DavaCustomItems.Configs;

public sealed class CoinExtraConfig
{
    public bool Replace914Coins { get; set; }
    public int ReplaceNormalCoinAmount { get; set; }
    public Dictionary<ItemType /* ItemType */, int/* Count */> ItemsToReplace { get; set; } = [];
    public float CoinBrakeChance { get; set; } = 0.10f;
    public int MaxFlipping { get; set; } = 3;
    public int ReplaceItemChance { get; set; } 
    public int ReplaceCoinChance { get; set; } 
    public double CoolDown { get; set; } = 5;
    public Dictionary<NameConfig, int> NameAndWeight { get; set; } = [];
    public Dictionary<string, List<object>> ExtraSettings { get; set; } = [];
    public Dictionary<string, Dictionary<List<object>, int>> ExtraSettingsAndWeight { get; set; } = [];
}
