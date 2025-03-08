using DavaCustomItems.Coins;
using DavaCustomItems.Configs;
using Exiled.API.Interfaces;
using UnityEngine;

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
                Color = Color.red,
                Intensity = 50,
                MovementSmoothing = 50,
                Range = 10,
                Scale = Vector3.one,
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
                Description = "Legendary Coin now give you much better loot and chance and stuff"
            }
        },
        {
            CoinRarityType.OnlyNegative,
            new()
            {
                Id = 501,
                Name = "Negative Coin",
                Description = "This make it the only bad happens."
            }
        }
    };
}
