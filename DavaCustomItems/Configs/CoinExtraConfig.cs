using Exiled.API.Enums;

namespace DavaCustomItems.Configs;

public sealed class CoinExtraConfig
{
    public float CoinBrakeChance { get; set; } = 0.10f;
    public int MaxFlipping { get; set; } = 3;
    public Dictionary<string, int> NameAndWeight { get; set; } = new()
    {
        { "GiveItem", 25 },
        { "GiveEffect", 15 },
        { "MoreHealth", 10 },
        { "LoseHealth", 10 },
        { "ThrowableSpawn", 10 },
        { "MedicalKit", 40 },
        { "ExecuteCommand", 1 },
        { "ExecuteServerCommand", 1 },
    };
    public Dictionary<string, string> NameToHint { get; set; } = new()
    {
        { "GiveItem", "You got {0}!" },
        { "GiveEffect", "You got {0} effect!" }
    };

    public Dictionary<ItemType, int> ItemChance { get; set; } = new()
    {
        {  ItemType.Adrenaline, 10 },
        {  ItemType.Radio, 20 }
    };

    public Dictionary<EffectConfig, int> EffectChance { get; set; } = new()
    {
        { new(EffectType.RainbowTaste), 10 },
        { new(EffectType.Scanned), 20 },
        { new(EffectType.MovementBoost, 100, 100), 5 }
    };
    public Dictionary<int, int> MoreHealthChance { get; set; } = new()
    {
        { 10, 10 },
        { 30, 3 },
        { 5, 30 },
    };

    public Dictionary<ProjectileType, int> ThrowableSpawnChance { get; set; } = new()
    {
        { ProjectileType.FragGrenade, 10 },
        { ProjectileType.Scp018, 3 },
        { ProjectileType.Flashbang, 30 },
    };

    public List<ItemType> MedicalKit { get; set; } =
    [
        ItemType.Medkit, ItemType.Painkillers
    ];

}
