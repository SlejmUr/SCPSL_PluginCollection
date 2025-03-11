using Exiled.API.Enums;

namespace DavaCustomItems.Configs;

public sealed class CoinExtraConfig
{
    public float CoinBrakeChance { get; set; } = 0.10f;
    public int MaxFlipping { get; set; } = 3;

    // TODO: Make separate from tails and heads, <(string ActionName, bool IsTails), int> ?
    public Dictionary<KeyValuePair<string, bool>, int> NameAndWeight { get; set; } = new()
    {
        // Some events not implemented yet. 
        { new("GiveItem", true), 25 },
        { new("GivePositiveEffect", true), 15 },
        { new("GiveNegativeEffect", false), 15 },
        { new("GiveMixedEffect", true), 15 },
        { new("GiveMixedEffect", false), 15 },
        { new("MoreHealth", true), 10 },
        { new("LoseHealth", false), 10 },
        { new("ThrowableSpawn", true), 10 },
        { new("MedicalKit", true), 40 },
        //{ new("ExecuteCommand", true), 1 },
        //{ new("ExecuteServerCommand", true), 1 },
        { new("ShufflePlayers", true), 1 },
        { new("GreatGamble", true), 1 },
        { new("LoseItems", false), 1 },
        { new("TpToSCP", true), 1 },
        { new("TpToRandomPlayer", true), 1 },
        { new("ImmenseFortitude", true), 10 },
        { new("TrueRessurection", true), 10 },
        { new("Necromancy", true), 10 },
        { new("NoAction", true), 1 }
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

    public Dictionary<int, int> LoseHealthWeight { get; set; } = new()
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
