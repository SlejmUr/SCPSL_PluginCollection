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
                ShouldFollowPickup = true,
                ShouldFollowPlayer = true,
                ShouldMakeLight = true,
                ShouldShowLightOnSpawn = true,
                Color = new(1, 0.5f, 0, 1),
                Intensity = 30,
                Range = 5,
            }
        },
        {
            "Rare Coin",
            new()
            {
                ShouldFollowPickup = true,
                ShouldFollowPlayer = true,
                ShouldMakeLight = true,
                ShouldShowLightOnSpawn = true,
                Color = UnityEngine.Color.blue,
                Intensity = 7,
                Range = 3,
            }
        },
        {
            "Test05Light",
            new()
            {
                ShouldFollowPickup = true,
                ShouldFollowPlayer = true,
                ShouldMakeLight = true,
                ShouldShowLightOnSpawn = true,
                Color = UnityEngine.Color.red,
                Intensity = 10,
                Range = 5,
                RainbowConfig = new()
                {
                    RainbowType = RainbowLightType.Rave,
                    ByteAlpha = 10,
                    Speed = 100
                }
            }
        }
    };

    public Dictionary<CoinRarityType /* Rarity */, CoinConfig /* Coin Config */> CoinRarityConfigs { get; set; } = new()
    {        
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
                        { new( "ShufflePlayers", false), 3 },
                        { new( "ShufflePlayers", true), 3 },
                        { new( "GreatGamble", true), 3 },
                        { new( "GreatGamble", false), 3 },
                        { new( "ImmenseFortitude", true), 6 },
                        { new( "ImmenseFortitude", false), 6 },
                        { new( "TrueResurrection", true), 2 },
                        { new( "TrueResurrection", false), 2 },
                        { new( "Necromancy", false), 2 },
                        { new( "Necromancy", true), 2 },
                        { new( "DrugCocktail", false), 6 },
                        { new( "DrugCocktail", true), 6 },
                        { new( "CursedDrugCocktail", false), 2 },
                        { new( "CursedDrugCocktail", true), 2 },
                        { new( "ExperimentalItem", false), 3 },
                        { new( "ExperimentalItem", true), 3 },
                        { new( "Combustion", false), 5 },
                        { new( "Combustion", true), 5 },
                        { new( "HighAwareness", false), 3 },
                        { new( "HighAwareness", true), 3 },
                        { new( "BlessingsAbound", false, "BlessingEffects", false), 3 },
                        { new( "BlessingsAbound", true, "BlessingEffects", false), 3 },
                        { new( "RocketMan", false), 5 },
                        { new( "RocketMan", true), 5 },
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
                        { new( "GiveItem", true, "GiveItem", false), 4 },
                        { new( "GivePositiveEffect", true, "GiveEffect", false), 6 },
                        { new( "MoreHealth", true, "MoreHealth", false), 6 },
                        { new( "SweetTooth", true, "SweetTooth", true), 4 },
                        { new( "SourTooth", true), 2 },
                        { new( "MedicalKit", true, "MedicalKit", false), 6 },
                        { new( "GiveXP", true, "GiveXP", false), 6 },
                        { new( "RandomWeapon", true, "RandomWeapon", false), 4 },
                        { new( "RandomKeycard", true, "RandomKeycard", false), 4 },
                        { new( "MaxHP", true, "MaxHP", false), 6 },
                        { new( "NeverQuit", true), 3 },
                        { new( "Jackpot", true), 2 },
                        { new( "SPEEED", true, "SPEEED", false), 4 },
                        { new( "Exposed", true, "Exposed", false), 2 },
                        { new( "Lootbox", true, "Lootbox", false), 2},
                        { new( "SpreadTheLove", true), 3 },
                        
                        // Bad
                        { new( "ThrowableSpawn", false, "ThrowableSpawn", false), 4 },
                        { new( "GiveNegativeEffect", false, "GiveBadEffect", false), 4 },
                        { new( "LoseHealth", false, "LoseHealth", false), 4 },
                        { new( "1hp", false), 1 },
                        { new( "FoodPoisoning", false), 3 },
                        { new( "GrenadeDrop", false), 4 },
                        { new( "LoseItems", false), 1 },
                        { new( "TpToSCP", false), 1 },
                        { new( "Freeze", false), 2 },
                        { new( "MinHP", false, "MinHP", false), 1 },
                        { new( "AutoNuke", false), 1 },
                        { new( "Timeout", false), 1 },
                        { new( "InsultToInjury", false, "InsultToInjury", false), 2 },
                        

                        // Mixed
                        { new( "GiveMixedEffect", true, "GiveMixedEffect", false), 5 },
                        { new( "GiveMixedEffect", false, "GiveMixedEffect", false), 5 },
                        { new( "DisappearingAct", true, "DisappearingAct", false), 5 },
                        { new( "DisappearingAct", false, "DisappearingAct", false), 5 },
                        { new( "Gnome", true), 5 },
                        { new( "Gnome", false), 5 },
                        { new( "TallMan", true), 5 },
                        { new( "TallMan", false), 5 },
                        { new( "NormalMan", true), 5 },
                        { new( "NormalMan", false), 5 },
                        { new( "WideMan", true), 5 },
                        { new( "WideMan", false), 5 },
                        { new( "Blackout", true, "Blackout", false), 3 },
                        { new( "Blackout", false, "Blackout", false), 3 },
                        { new( "TpToRandomPlayer", true), 3 },
                        { new( "TpToRandomPlayer", false), 3 },
                        { new( "SideSwapper", false, "SideSwapper", false), 1 },
                        { new( "SideSwapper", true, "SideSwapper", false), 1 },

                    },
                    ExtraSettings =
                    {
                        {
                            "GiveItem",
                            [ItemType.SCP500, ItemType.SCP2176, ItemType.Jailbird]
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
                            [ItemType.GunCrossvec, ItemType.GunAK, ItemType.GunLogicer, ItemType.GunFRMG0]
                        },
                        {
                            "RandomKeycard",
                            [ItemType.KeycardO5, ItemType.KeycardChaosInsurgency, ItemType.KeycardFacilityManager]
                        },
                        {
                            "Blackout",
                            [20]
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
                        },
                        {
                              "SideSwapper",
                            [20]
                        },
                        {
                            "GiveXP",
                            [200]
                        },
                        {
                            "DisappearingAct",
                            [true]
                        },
                        {
                            "Lootbox",
                            [ItemType.SCP500, ItemType.Lantern, ItemType.KeycardFacilityManager, ItemType.GunE11SR, ItemType.GunA7]
                        },
                        {
                            "InsultToInjury",
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
                        { new( "GiveItem", true, "GiveItem", false), 3 },
                        { new( "GivePositiveEffect", true, "GiveEffect", false), 5 },
                        { new( "MoreHealth", true, "MoreHealth", false), 5 },
                        { new( "SweetTooth", true, "SweetTooth", true), 3 },
                        { new( "SourTooth", true), 1 },
                        { new( "MedicalKit", true, "MedicalKit", false), 6 },
                        { new( "GiveXP", true, "GiveXP", false), 5 },
                        { new( "RandomWeapon", true, "RandomWeapon", false), 3 },
                        { new( "RandomKeycard", true, "RandomKeycard", false), 3 },
                        { new( "MaxHP", true, "MaxHP", false), 5 },
                        { new( "NeverQuit", true), 2 },
                        { new( "Jackpot", true), 1 },
                        { new( "SPEEED", true, "SPEEED", false), 3 },
                        { new( "Exposed", true, "Exposed", false), 1 },
                        { new( "Lootbox", true, "Lootbox", false), 1 },
                        { new( "SpreadTheLove", true), 2 },
                        
                        // Bad
                        { new( "ThrowableSpawn", false, "ThrowableSpawn", false), 5 },
                        { new( "GiveNegativeEffect", false, "GiveBadEffect", false), 5 },
                        { new( "LoseHealth", false, "LoseHealth", false), 1 },
                        { new( "1hp", false), 1 },
                        { new( "FoodPoisoning", false), 5 },
                        { new( "GrenadeDrop", false), 5 },
                        { new( "LoseItems", false), 2 },
                        { new( "TpToSCP", false), 3 },
                        { new( "Freeze", false), 3 },
                        { new( "MinHP", false, "MinHP", false), 1 },
                        { new( "AutoNuke", false), 1 },
                        { new( "Timeout", false), 1 },
                        { new( "InsultToInjury", false, "InsultToInjury", false), 3 },

                        // Mixed
                        { new( "GiveMixedEffect", true, "GiveMixedEffect", false), 5 },
                        { new( "GiveMixedEffect", false, "GiveMixedEffect", false), 5 },
                        { new( "DisappearingAct", true), 1 },
                        { new( "DisappearingAct", false), 1 },
                        { new( "Gnome", true), 5 },
                        { new( "Gnome", false), 5 },
                        { new( "TallMan", true), 5 },
                        { new( "TallMan", false), 5 },
                        { new( "WideMan", true), 5 },
                        { new( "WideMan", false), 5 },
                        { new( "NormalMan", true), 5 },
                        { new( "NormalMan", false), 5 },
                        { new( "Blackout", true, "Blackout", false), 3 },
                        { new( "Blackout", false, "Blackout", false), 3 },
                        { new( "TpToRandomPlayer", true), 3 },
                        { new( "TpToRandomPlayer", false), 3 },
                        { new( "SideSwapper", false, "SideSwapper", false), 1 },
                        { new( "SideSwapper", true, "SideSwapper", false), 1 },
                    },
                    ExtraSettings =
                    {
                        {
                            "GiveItem",
                            [ItemType.Adrenaline, ItemType.KeycardZoneManager, ItemType.SCP500, ItemType.SCP2176]
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
                            [ItemType.GunCOM18, ItemType.GunA7, ItemType.GunRevolver]
                        },
                        {
                            "RandomKeycard",
                            [ItemType.KeycardScientist, ItemType.KeycardMTFOperative, ItemType.KeycardContainmentEngineer]
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
                        },
                        {
                            "GiveXP",
                            [100]

                        },
                        {
                            "DisappearingAct",
                            [false]
                        },
                        {
                            "Lootbox",
                            [ItemType.Adrenaline, ItemType.Flashlight, ItemType.KeycardMTFOperative, ItemType.GunFSP9, ItemType.GunA7]
                        },
                        {
                            "InsultToInjury",
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
                    ReplaceNormalCoinAmount = 100,
                    ItemsToReplace =
                    {
                        { ItemType.SCP500, 3 }
                    },
                    ReplaceCoinChance = 100,
                    ReplaceItemChance = 100
                },  
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
