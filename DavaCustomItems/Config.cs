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
                IsRainbow = true,
                Intensity = 10,
                Range = 5
            }
        }
    };

    public Dictionary<CoinRarityType /* Rarity */, CoinConfig /* Coin Config */> CoinRarityConfigs { get; set; } = new()
    {
        { 
            CoinRarityType.LegendaryCoin,
            new()
            {
                Id = 500,
                Name = "Legendary Coin",
                Description = "Legendary Coin now give you much better loot and chance and stuff",
                ExtraConfig = new()
                {
                    CoinBrakeChance = 0.01f,
                }
            }
        },
        {
            CoinRarityType.SuperUnluckyCoin,
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
            CoinRarityType.NormalCoin,
            new()
            {
                Id = 5002,
                Name = "Normal Coin",
                Description = "todo",
                ExtraConfig = new()
                {

                }
            }
        }
    };
}
