using DavaCustomItems.Coins;
using DavaCustomItems.Configs;
using Exiled.API.Interfaces;

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
                ExtraConfig = new()
                {
                    CoinBrakeChance = 0.01f,
                    /*
                    NameAndWeight =
                    {
                        { new("...", true),5 },
                        { new("...", false),5 }
                    },
                    GiveNegativeEffectWeight =
                    {
                        {  new EffectConfig(Exiled.API.Enums.EffectType.Corroding, 20, 20), 30 }
                    }
                    */
                }
            }
        },
        {
            CoinRarityType.SuperUnlucky,
            new()
            {
                Id = 501,
                Name = "Negative Coin",
                Description = "This make it the only bad happens."
            }
        },
        {
            CoinRarityType.TEST,
            new()
            {
                Id = 499,
                Name = "TEST Coin",
                Description = "TEST.",
                ExtraConfig = new()
                {
                    MaxFlipping = 10
                }
            }
        },
        {
            CoinRarityType.Normal,
            new()
            {
                Id = 5002,
                Name = "Normal Coin",
                Description = "todo",
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
