using Exiled.API.Enums;

namespace DavaCustomItems.Configs;

public sealed class CoinExtraConfig
{
    public float CoinBrakeChance { get; set; } = 0.10f;
    public int MaxFlipping { get; set; } = 3;

    // TODO: Make separate from tails and heads, <(string ActionName, bool IsTails), int> ?
    public Dictionary<string, int> NameAndWeight { get; set; } = new()
    {
        // Some events not implemented yet. 
        { "GiveItem", 25 },
        { "GivePositiveEffect", 15 },
        { "GiveNegativeEffect", 15 },
        { "GiveMixedEffect", 15 },
        { "MoreHealth", 10 },
        { "LoseHealth", 10 },
        { "ThrowableSpawn", 10 },
        { "MedicalKit", 40 },
        { "ExecuteCommand", 1 },
        { "ExecuteServerCommand", 1 },
        { "ShufflePlayers", 1 },
        { "GreatGamble", 1 },
        { "LoseItems", 1 },
        { "TpToSCP", 1 },
        { "TpToRandomPlayer", 1 },
        { "NoAction", 100 }
    };

    public Dictionary<string, string> NameToHint { get; set; } = new()
    {
        { "GiveItem", "You got {0}!" },
        { "GiveEffect", "You got {0} effect!" }
    };

    public Dictionary<ItemType, int> GiveItemWeight { get; set; } = new()
    {
        {  ItemType.Adrenaline, 10 },
        {  ItemType.Radio, 20 }
    };

    public Dictionary<EffectConfig, int> GivePositiveEffectWeight { get; set; } = new()
    {
        { new(EffectType.RainbowTaste), 10 },
        { new(EffectType.MovementBoost, 100, 100), 5 }
    };

    public Dictionary<EffectConfig, int> GiveNegativeEffectWeight { get; set; } = new();

    public Dictionary<EffectConfig, int> GiveMixedEffectWeight { get; set; } = new();

    public Dictionary<int, int> MoreHealthWeight { get; set; } = new()
    {
        { 10, 10 },
        { 30, 3 },
        { 5, 30 },
    };

    public Dictionary<ProjectileType, int> ThrowableSpawnWeight { get; set; } = new()
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
