using DavaCustomItems.Coins;
using DavaCustomItems.Configs;
using Exiled.API.Enums;
using Exiled.API.Interfaces;
using InventorySystem.Items.Usables.Scp330;

namespace DavaCustomItems;

public sealed class Config : IConfig
{
    public bool IsEnabled { get; set; } = true;
    public bool Debug { get; set; }
    public Dictionary<string /* ItemName */, LightConfig /* Light to Emit */> LightConfigs { get; set; } = new()
    {
        { 
            "Legendary Coin", 
            new()
            {
                ShouldFollowPlayer = true,
                ShouldMakeLight = true,
                ShouldShowLightOnSpawn = true,
                Color = new(1, 0.5f, 0, 1),
                Intensity = 40,
                Range = 10,
            }
        },
        {
            "TEST Coin",
            new()
            {   
                ShouldFollowPlayer = true,
                ShouldMakeLight = true,
                Intensity = 10,
                Range = 5,
                RainbowConfig = new()
                {
                    RainbowType = RainbowLightType.Breathing,
                    FloatAlpha = 1
                }
            }
        },
        {
            "Rare Coin",
            new()
            {
                ShouldFollowPlayer = true,
                ShouldMakeLight = true,
                Color = UnityEngine.Color.blue,
                Intensity = 10,
                Range = 5,
            }
        }
    };

    public Dictionary<CoinRarityType /* Rarity */, CoinConfig /* Coin Config */> CoinRarityConfigs { get; set; } = new()
    {        
        {
            CoinRarityType.TEST,
            new()
            {
                Id = 499,
                Name = "TEST Coin",
                Description = "TEST.",
                ExtraConfig = new()
                {
                    MaxFlipping = 1
                }
            }
        },
        { 
            CoinRarityType.Legendary,
            new()
            {
                Id = 500,
                Name = "Legendary Coin",
                Description = "Legendary Coin now give you much better loot and chance and stuff",
                ExtraConfig = new()
                {
                    CoinBrakeChance = 0.01f,
                    MaxFlipping = 1,
                    NameAndWeight =
                    {
                        { new( "ShufflePlayers", false, string.Empty, false), 1},
                        { new( "ShufflePlayers", true, string.Empty, false), 1},
                        { new( "GreatGamble", true,  string.Empty, false), 1 },
                        { new( "GreatGamble", false,  string.Empty, false), 1 },
                        { new( "ImmenseFortitude", true,  string.Empty, false), 1 },
                        { new( "ImmenseFortitude", false,  string.Empty, false), 1 },
                        { new( "TrueResurrection", true,  string.Empty, true), 1 },
                        { new( "TrueResurrection", false,  string.Empty, true), 1 },
                        { new( "Necromancy", false,  string.Empty, true), 1 },
                        { new( "Necromancy", true,  string.Empty, true), 1 },
                        { new( "DrugCocktail", false,  string.Empty, true), 1 },
                        { new( "DrugCocktail", true,  string.Empty, true), 1 },
                        { new( "CursedDrugCocktail", false,  string.Empty, true), 1 },
                        { new( "CursedDrugCocktail", true,  string.Empty, true), 1 },
                        { new( "ExperimentalItem", false,  string.Empty, true), 1 },
                        { new( "ExperimentalItem", true,  string.Empty, true), 1 },
                        { new( "Combustion", false,  string.Empty, true), 1 },
                        { new( "Combustion", true,  string.Empty, true), 1 },
                        { new( "HighAwareness", false,  string.Empty, true), 1 },
                        { new( "HighAwareness", true,  string.Empty, true), 1 },
                        { new( "BlessingsAbound", false, "BlessingEffects", false), 1 },
                        { new( "BlessingsAbound", true, "BlessingEffects", false), 1 },
                        { new( "RocketMan", false, string.Empty, false), 1 },
                        { new( "RocketMan", true, string.Empty, false), 1 },
                    },
                    ExtraSettings =
                    {
                        {
                            "BlessingEffects", 
                            new()
                            { 
                                new EffectConfig(EffectType.MovementBoost, 30),
                                new EffectConfig(EffectType.RainbowTaste, 30),
                                new EffectConfig(EffectType.DamageReduction, 30),
                            } 
                        }
                    },
                }
            }
        },
        {
            CoinRarityType.Rare,
            new()
            {
                Id = 501,
                Name = "Rare Coin",
                Description = "Rare coin!",
                ExtraConfig = new()
                {
                    NameAndWeight =
                    {
                        // Good
                        { new( "GiveItem", true, "GiveItem", false), 1 },
                        { new( "GivePositiveEffect", true, "GiveEffect", false), 1 },
                        { new( "MoreHealth", true, "MoreHealth", false), 1 },
                        { new( "SweetTooth", true, "SweetTooth", true), 1 },
                        { new( "SourTooth", true, string.Empty, true), 1 },
                        { new( "MedicalKit", true, "MedicalKit", false), 1 },
                        { new( "GiveXP", true, "GiveXP", false), 1 },
                        { new( "RandomWeapon", true, "RandomWeapon", false), 1 },
                        { new( "RandomKeycard", true, "RandomKeycard", false), 1 },
                        { new( "MaxHP", true, "MaxHP", false), 1 },
                        { new( "NeverQuit", true, string.Empty, false), 1 },
                        { new( "Jackpot", true, string.Empty, false), 1 },
                        
                        // Bad
                        { new( "ThrowableSpawn", false, "ThrowableSpawn", false), 1 },
                        { new( "GiveNegativeEffect", false, "GiveBadEffect", false), 1 },
                        { new( "LoseHealth", false, "LoseHealth", false), 1 },
                        { new( "1hp", false, string.Empty ,false), 1 },
                        { new( "FoodPoisoning", false, string.Empty, false), 1 },
                        { new( "GrenadeDrop", false, string.Empty, false), 1 },
                        { new( "LoseItems", false, string.Empty , false), 1 },
                        { new( "TpToSCP", false, string.Empty , false), 1 },
                        { new( "Freeze", false, string.Empty, false), 1 },
                        { new( "MinHP", false, "MinHP", false), 1 },
                        { new( "AutoNuke", false, string.Empty, false), 1 },

                        // Mixed
                        { new( "GiveMixedEffect", true, "GiveMixedEffect", false), 1 },
                        { new( "GiveMixedEffect", false, "GiveMixedEffect", false), 1 },
                        { new( "DisappearingAct", true, string.Empty, false), 1 },
                        { new( "DisappearingAct", false, string.Empty, false), 1 },
                        { new( "Gnome", true, string.Empty, false), 1 },
                        { new( "Gnome", false, string.Empty, false), 1 },
                        { new( "TallMan", true, string.Empty, false), 1 },
                        { new( "TallMan", false, string.Empty, false), 1 },
                        { new( "NormalMan", true, string.Empty, false), 1 },
                        { new( "NormalMan", false, string.Empty, false), 1 },
                        { new( "WideMan", true, string.Empty, false), 1 },
                        { new( "WideMan", false, string.Empty, false), 1 },
                        { new( "Blackout", true, "Blackout", false), 1 },
                        { new( "Blackout", false, "Blackout", false), 1 },
                        { new( "TpToRandomPlayer", true, string.Empty, false), 1 },
                        { new( "TpToRandomPlayer", false, string.Empty, false), 1 },
                    },
                    ExtraSettings =
                    {
                        {
                            "GiveItem",
                            [ItemType.Adrenaline, ItemType.Radio, ItemType.Painkillers, ItemType.KeycardZoneManager]
                        },
                        {
                            "GiveEffect",
                            new()
                            {
                                new EffectConfig(EffectType.MovementBoost, 10, 10),
                                new EffectConfig(EffectType.SilentWalk, 10, 10),
                                new EffectConfig(EffectType.RainbowTaste, 10, 10)
                            }
                        },
                        {
                            "GiveBadEffect",
                            new()
                            {
                                new EffectConfig(EffectType.Scanned, 10, 10),
                                new EffectConfig(EffectType.CardiacArrest, 10, 10),
                                new EffectConfig(EffectType.Bleeding, 10, 10)
                            }
                        },
                        {
                            "GiveMixedEffect",
                            new()
                            {
                                new EffectConfig(EffectType.Deafened, 10, 10),
                                new EffectConfig(EffectType.Concussed, 10, 10),
                                new EffectConfig(EffectType.Ghostly, 10, 10)
                            }
                        },
                        {
                            "ThrowableSpawn",
                            [ProjectileType.FragGrenade, ProjectileType.Scp018, ProjectileType.Flashbang]
                        },
                        {
                            "MoreHealth",
                            [30,45]
                        },
                        {
                            "LoseHealth",
                            [15,30]
                        },
                        {
                            "MedicalKit",
                            [ItemType.Adrenaline, ItemType.Medkit, ItemType.SCP500]
                        },
                        {
                            "RandomWeapon",
                            [ItemType.GunCrossvec, ItemType.GunAK, ItemType.GunLogicer]
                        },
                        {
                            "RandomKeycard",
                            [ItemType.KeycardO5, ItemType.KeycardChaosInsurgency, ItemType.KeycardFacilityManager]
                        },
                        {
                            "Blackout",
                            [10]
                        },
                        {
                            "TpToSCP",
                            [true]
                        },
                        {
                            "MaxHP",
                            [20]
                        },
                        {
                            "MinHP",
                            [20]
                        },
                    },
                    ExtraSettingsAndWeight =
                    {
                        {
                            "SweetTooth",
                            new()
                            {
                                {
                                    new()
                                    {
                                        CandyKindID.Red,CandyKindID.Rainbow
                                    },
                                    4
                                },
                                {
                                    new()
                                    {
                                        CandyKindID.Yellow
                                    },
                                    14
                                }
                            }
                        }
                    }
                }
            }
        },
        {
            CoinRarityType.Normal,
            new()
            {
                Id = 502,
                Name = "Normal Coin",
                Description = "The Basic Coin",
                ExtraConfig = new()
                {
                    NameAndWeight =
                    {
                        // Good
                        { new( "GiveItem", true, "GiveItem", false), 1 },
                        { new( "GivePositiveEffect", true, "GiveEffect", false), 1 },
                        { new( "MoreHealth", true, "MoreHealth", false), 1 },
                        { new( "SweetTooth", true, "SweetTooth", true), 1 },
                        { new( "SourTooth", true, string.Empty, true), 1 },
                        { new( "MedicalKit", true, "MedicalKit", false), 1 },
                        { new( "GiveXP", true, "GiveXP", false), 1 },
                        { new( "RandomWeapon", true, "RandomWeapon", false), 1 },
                        { new( "RandomKeycard", true, "RandomKeycard", false), 1 },
                        { new( "MaxHP", true, "MaxHP", false), 1 },
                        { new( "NeverQuit", true, string.Empty, false), 1 },
                        { new( "Jackpot", true, string.Empty, false), 1 },
                        
                        // Bad
                        { new( "ThrowableSpawn", false, "ThrowableSpawn", false), 1 },
                        { new( "GiveNegativeEffect", false, "GiveBadEffect", false), 1 },
                        { new( "LoseHealth", false, "LoseHealth", false), 1 },
                        { new( "1hp", false, string.Empty ,false), 1 },
                        { new( "FoodPoisoning", false, string.Empty, false), 1 },
                        { new( "GrenadeDrop", false, string.Empty, false), 1 },
                        { new( "LoseItems", false, string.Empty , false), 1 },
                        { new( "TpToSCP", false, string.Empty , false), 1 },
                        { new( "Freeze", false, string.Empty, false), 1 },
                        { new( "MinHP", false, "MinHP", false), 1 },
                        { new( "AutoNuke", false, string.Empty, false), 1 },

                        // Mixed
                        { new( "GiveMixedEffect", true, "GiveMixedEffect", false), 1 },
                        { new( "GiveMixedEffect", false, "GiveMixedEffect", false), 1 },
                        { new( "DisappearingAct", true, string.Empty, false), 1 },
                        { new( "DisappearingAct", false, string.Empty, false), 1 },
                        { new( "Gnome", true, string.Empty, false), 1 },
                        { new( "Gnome", false, string.Empty, false), 1 },
                        { new( "TallMan", true, string.Empty, false), 1 },
                        { new( "TallMan", false, string.Empty, false), 1 },
                        { new( "WideMan", true, string.Empty, false), 1 },
                        { new( "WideMan", false, string.Empty, false), 1 },
                        { new( "NormalMan", true, string.Empty, false), 1 },
                        { new( "NormalMan", false, string.Empty, false), 1 },
                        { new( "Blackout", true, "Blackout", false), 1 },
                        { new( "Blackout", false, "Blackout", false), 1 },
                        { new( "TpToRandomPlayer", true, string.Empty, false), 1 },
                        { new( "TpToRandomPlayer", false, string.Empty, false), 1 },
                    },
                    ExtraSettings =
                    {
                        {
                            "GiveItem",
                            [ItemType.Adrenaline, ItemType.Radio, ItemType.Painkillers, ItemType.KeycardZoneManager]
                        },
                        {
                            "GiveEffect",
                            new()
                            {
                                new EffectConfig(EffectType.MovementBoost, 10, 10),
                                new EffectConfig(EffectType.SilentWalk, 10, 10),
                                new EffectConfig(EffectType.RainbowTaste, 10, 10)
                            }
                        },
                        {
                            "GiveBadEffect",
                            new()
                            {
                                new EffectConfig(EffectType.Scanned, 10, 10),
                                new EffectConfig(EffectType.CardiacArrest, 10, 10),
                                new EffectConfig(EffectType.Bleeding, 10, 10)
                            }
                        },
                        {
                            "GiveMixedEffect",
                            new()
                            {
                                new EffectConfig(EffectType.Deafened, 10, 10),
                                new EffectConfig(EffectType.Concussed, 10, 10),
                                new EffectConfig(EffectType.Ghostly, 10, 10)
                            }
                        },
                        {
                            "ThrowableSpawn",
                            [ProjectileType.FragGrenade, ProjectileType.Scp018, ProjectileType.Flashbang]
                        },
                        {
                            "MoreHealth",
                            [15,30]
                        },
                        {
                            "LoseHealth",
                            [15,30]
                        },
                        {
                            "MedicalKit",
                            [ItemType.Adrenaline, ItemType.Medkit]
                        },
                        {
                            "RandomWeapon",
                            [ItemType.GunCOM18, ItemType.GunCrossvec, ItemType.GunRevolver]
                        },
                        {
                            "RandomKeycard",
                            [ItemType.KeycardScientist, ItemType.KeycardChaosInsurgency, ItemType.KeycardFacilityManager]
                        },
                        {
                            "Blackout",
                            [5]
                        },
                        {
                            "MaxHP",
                            [10]
                        },
                        {
                            "MinHP",
                            [10]
                        },
                    },
                    ExtraSettingsAndWeight =
                    {
                        {
                            "SweetTooth",
                            new()
                            {
                                {
                                    new()
                                    {
                                        CandyKindID.Red,CandyKindID.Rainbow
                                    },
                                    4
                                },
                                {
                                    new()
                                    {
                                        CandyKindID.Yellow
                                    },
                                    14
                                }
                            }
                        }
                    },
                    Replace914Coins = true,
                    ReplaceNormalCoinAmount = 5,
                    ItemsToReplace =
                    {
                        { ItemType.SCP500, 3 }
                    }
                }
            }
        }
    };
}

/*
Legendary Coin Global Broadcast Prefix "{player.Nickname} has flipped the Legendary Coin"
Legendary Coin Global Broadcast Duration "5"

shuffle Global Broadcast Message
shuffle Delay before moved to the next player "3"
Shuffle Player Hint 
Shuffle Player Hint Duration "5"
Shuffle Victim Hint
Shuffle Victim Hint duration 

GreatGamble Map Broadcast + duration
GreatGamble map hint + duration
GreatGamble Player Hint + duration
GreatGamble Victim hint
 
 */
