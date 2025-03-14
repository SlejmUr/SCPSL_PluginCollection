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
            "Rare Coin",
            new()
            {
                ShouldFollowPlayer = true,
                ShouldMakeLight = true,
                ShouldShowLightOnSpawn = true,
                Color = UnityEngine.Color.blue,
                Intensity = 10,
                Range = 5,
            }
        },
        {
            "Super Unlucky Coin",
            new()
            {
                ShouldFollowPlayer = true,
                ShouldMakeLight = true,
                ShouldShowLightOnSpawn = true,
                Color = UnityEngine.Color.red,
                Intensity = 10,
                Range = 5,
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
                CoinPickUpHint = "You picked up the <color=orange>LEGENDARY</color> Coin",
                ExtraConfig = new()
                {
                    CoinBrakeChance = 0.01f,
                    MaxFlipping = 1,
                    NameAndWeight =
                    {
                        { new( "ShufflePlayers", false), 1},
                        { new( "ShufflePlayers", true), 1},
                        { new( "GreatGamble", true), 1 },
                        { new( "GreatGamble", false), 1 },
                        { new( "ImmenseFortitude", true), 1 },
                        { new( "ImmenseFortitude", false), 1 },
                        { new( "TrueResurrection", true), 1 },
                        { new( "TrueResurrection", false), 1 },
                        { new( "Necromancy", false), 1 },
                        { new( "Necromancy", true), 1 },
                        { new( "DrugCocktail", false), 1 },
                        { new( "DrugCocktail", true), 1 },
                        { new( "CursedDrugCocktail", false), 1 },
                        { new( "CursedDrugCocktail", true), 1 },
                        { new( "ExperimentalItem", false), 1 },
                        { new( "ExperimentalItem", true), 1 },
                        { new( "Combustion", false), 1 },
                        { new( "Combustion", true), 1 },
                        { new( "HighAwareness", false), 1 },
                        { new( "HighAwareness", true), 1 },
                        { new( "BlessingsAbound", false, "BlessingEffects", false), 1 },
                        { new( "BlessingsAbound", true, "BlessingEffects", false), 1 },
                        { new( "RocketMan", false), 1 },
                        { new( "RocketMan", true), 1 },
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
                CoinPickUpHint = "You picked up the <color=blue>RARE</color> Coin",
                ExtraConfig = new()
                {
                    NameAndWeight =
                    {
                        // Good
                        { new( "GiveItem", true, "GiveItem", false), 1 },
                        { new( "GivePositiveEffect", true, "GiveEffect", false), 1 },
                        { new( "MoreHealth", true, "MoreHealth", false), 1 },
                        { new( "SweetTooth", true, "SweetTooth", true), 1 },
                        { new( "SourTooth", true), 1 },
                        { new( "MedicalKit", true, "MedicalKit", false), 1 },
                        { new( "GiveXP", true, "GiveXP", false), 1 },
                        { new( "RandomWeapon", true, "RandomWeapon", false), 1 },
                        { new( "RandomKeycard", true, "RandomKeycard", false), 1 },
                        { new( "MaxHP", true, "MaxHP", false), 1 },
                        { new( "NeverQuit", true), 1 },
                        { new( "Jackpot", true), 1 },
                        { new( "SPEEED", true, "SPEEED", false), 1 },
                        
                        // Bad
                        { new( "ThrowableSpawn", false, "ThrowableSpawn", false), 1 },
                        { new( "GiveNegativeEffect", false, "GiveBadEffect", false), 1 },
                        { new( "LoseHealth", false, "LoseHealth", false), 1 },
                        { new( "1hp", false), 1 },
                        { new( "FoodPoisoning", false), 1 },
                        { new( "GrenadeDrop", false), 1 },
                        { new( "LoseItems", false), 1 },
                        { new( "TpToSCP", false), 1 },
                        { new( "Freeze", false), 1 },
                        { new( "MinHP", false, "MinHP", false), 1 },
                        { new( "AutoNuke", false), 1 },
                        { new( "Timeout", false), 1 },

                        // Mixed
                        { new( "GiveMixedEffect", true, "GiveMixedEffect", false), 1 },
                        { new( "GiveMixedEffect", false, "GiveMixedEffect", false), 1 },
                        { new( "DisappearingAct", true), 1 },
                        { new( "DisappearingAct", false), 1 },
                        { new( "Gnome", true), 1 },
                        { new( "Gnome", false), 1 },
                        { new( "TallMan", true), 1 },
                        { new( "TallMan", false), 1 },
                        { new( "NormalMan", true), 1 },
                        { new( "NormalMan", false), 1 },
                        { new( "WideMan", true), 1 },
                        { new( "WideMan", false), 1 },
                        { new( "Blackout", true, "Blackout", false), 1 },
                        { new( "Blackout", false, "Blackout", false), 1 },
                        { new( "TpToRandomPlayer", true), 1 },
                        { new( "TpToRandomPlayer", false), 1 },
                        { new( "Exposed", false, "Exposed", false), 1 },
                        { new( "Exposed", true, "Exposed", false), 1 },

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
                        {
                            "Exposed",
                            [true]
                        },
                        {
                            "SPEEED",
                            [true]
                        }
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
                    ReplaceCoinChance = 2,
                    ReplaceItemChance = 10
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
                CoinPickUpHint = "You picked up a Coin",
                ExtraConfig = new() 
                {
                    NameAndWeight =
                    {
                        // Good
                        { new( "GiveItem", true, "GiveItem", false), 1 },
                        { new( "GivePositiveEffect", true, "GiveEffect", false), 1 },
                        { new( "MoreHealth", true, "MoreHealth", false), 1 },
                        { new( "SweetTooth", true, "SweetTooth", true), 1 },
                        { new( "SourTooth", true), 1 },
                        { new( "MedicalKit", true, "MedicalKit", false), 1 },
                        { new( "GiveXP", true, "GiveXP", false), 1 },
                        { new( "RandomWeapon", true, "RandomWeapon", false), 1 },
                        { new( "RandomKeycard", true, "RandomKeycard", false), 1 },
                        { new( "MaxHP", true, "MaxHP", false), 1 },
                        { new( "NeverQuit", true), 1 },
                        { new( "Jackpot", true), 1 },
                        { new( "SPEEED", true, "SPEEED", false), 1 },
                        
                        // Bad
                        { new( "ThrowableSpawn", false, "ThrowableSpawn", false), 1 },
                        { new( "GiveNegativeEffect", false, "GiveBadEffect", false), 1 },
                        { new( "LoseHealth", false, "LoseHealth", false), 1 },
                        { new( "1hp", false), 1 },
                        { new( "FoodPoisoning", false), 1 },
                        { new( "GrenadeDrop", false), 1 },
                        { new( "LoseItems", false), 1 },
                        { new( "TpToSCP", false), 1 },
                        { new( "Freeze", false), 1 },
                        { new( "MinHP", false, "MinHP", false), 1 },
                        { new( "AutoNuke", false), 1 },
                        { new( "Timeout", false), 1 },

                        // Mixed
                        { new( "GiveMixedEffect", true, "GiveMixedEffect", false), 1 },
                        { new( "GiveMixedEffect", false, "GiveMixedEffect", false), 1 },
                        { new( "DisappearingAct", true), 1 },
                        { new( "DisappearingAct", false), 1 },
                        { new( "Gnome", true), 1 },
                        { new( "Gnome", false), 1 },
                        { new( "TallMan", true), 1 },
                        { new( "TallMan", false), 1 },
                        { new( "WideMan", true), 1 },
                        { new( "WideMan", false), 1 },
                        { new( "NormalMan", true), 1 },
                        { new( "NormalMan", false), 1 },
                        { new( "Blackout", true, "Blackout", false), 1 },
                        { new( "Blackout", false, "Blackout", false), 1 },
                        { new( "TpToRandomPlayer", true), 1 },
                        { new( "TpToRandomPlayer", false), 1 },
                        { new( "Exposed", false, "Exposed", false), 1 },
                        { new( "Exposed", true, "Exposed", false), 1 },
                        { new( "SideSwapper", false, "SideSwapper", false), 1 },
                        { new( "SideSwapper", true, "SideSwapper", false), 1 },
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
                        {
                            "Exposed",
                            [false]
                        },
                        {
                            "SPEEED",
                            [false]
                        },
                        {
                            "SideSwapper",
                            [10]
                        }
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
                    },
                    ReplaceCoinChance = 98,
                    ReplaceItemChance = 90
                },  
            }
        },
        {
            CoinRarityType.Unlucky,
            new()
            {
                Id = 503,
                Name = "Unlucky Coin",
                Description = "Unlucky coin, only bad things happen.",
                ExtraConfig = new()
                {
                    NameAndWeight =
                    {
                        { new( "ThrowableSpawn", false, "ThrowableSpawn", false), 1 },
                        { new( "GiveNegativeEffect", false, "GiveBadEffect", false), 1 },
                        { new( "LoseHealth", false, "LoseHealth", false), 1 },
                        { new( "1hp", false), 1 },
                        { new( "FoodPoisoning", false), 1 },
                        { new( "GrenadeDrop", false), 1 },
                        { new( "LoseItems", false), 1 },
                        { new( "TpToSCP", false), 1 },
                        { new( "Freeze", false), 1 },
                        { new( "MinHP", false, "MinHP", false), 1 },
                        { new( "AutoNuke", false), 1 },
                        { new( "Timeout", false), 1 },

                        // Mixed
                        { new( "GiveMixedEffect", true, "GiveMixedEffect", false), 1 },
                        { new( "GiveMixedEffect", false, "GiveMixedEffect", false), 1 },
                        { new( "DisappearingAct", true), 1 },
                        { new( "DisappearingAct", false), 1 },
                        { new( "Gnome", true), 1 },
                        { new( "Gnome", false), 1 },
                        { new( "TallMan", true), 1 },
                        { new( "TallMan", false), 1 },
                        { new( "NormalMan", true), 1 },
                        { new( "NormalMan", false), 1 },
                        { new( "WideMan", true), 1 },
                        { new( "WideMan", false), 1 },
                        { new( "Blackout", true, "Blackout", false), 1 },
                        { new( "Blackout", false, "Blackout", false), 1 },
                        { new( "TpToRandomPlayer", true), 1 },
                        { new( "TpToRandomPlayer", false), 1 },
                        { new( "Exposed", false, "Exposed", false), 1 },
                        { new( "Exposed", true, "Exposed", false), 1 },
                        { new( "SideSwapper", false, "SideSwapper", false), 1 },
                        { new( "SideSwapper", true, "SideSwapper", false), 1 },
                    },
                    ExtraSettings =
                    {
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
                            "LoseHealth",
                            [15,30]
                        },
                        {
                            "Blackout",
                            [30]
                        },
                        {
                            "TpToSCP",
                            [true]
                        },
                        {
                            "MinHP",
                            [20, 40]
                        },
                        {
                            "Exposed",
                            [true]
                        },
                        {
                            "SideSwapper",
                            [20]
                        }
                    },
                }
            }
        },
        {
            CoinRarityType.SuperUnlucky,
            new()
            {
                Id = 504,
                Name = "Super Unlucky Coin",
                Description = "REALLY UNLUCKY COIN.",
                ExtraConfig = new()
                {
                    NameAndWeight =
                    {
                        { new( "ShufflePlayers", false), 1},
                        { new( "ShufflePlayers", true), 1},
                        { new( "CursedDrugCocktail", false), 1 },
                        { new( "CursedDrugCocktail", true), 1 },
                        { new( "Combustion", false), 1 },
                        { new( "Combustion", true), 1 },
                        { new( "RocketMan", false), 1 },
                        { new( "RocketMan", true), 1 },
                    },
                }
            }
        },
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
