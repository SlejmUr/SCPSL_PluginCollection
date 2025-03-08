using DavaCustomItems.Coins;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.CustomItems.API.Features;

namespace DavaCustomItems;

public sealed class Main : Plugin<Config>
{
    public static Main Instance { get; private set; }
    #region Plugin Info
    public override string Author => "SlejmUr";
    public override string Name => "CustomItems";
    public override string Prefix => "CustomItems";
    public override Version Version => new(0, 1);
    public override PluginPriority Priority => PluginPriority.Lowest;
    #endregion

    public override void OnEnabled()
    {
        if (!Config.IsEnabled)
            return;
        Instance = this;

        foreach (var item in Config.CoinRarityConfigs)
        {
            BaseCustomCoin coin = new()
            {
                Id = item.Value.Id,
                Description = item.Value.Description,
                Name = item.Value.Name,
                Rarity = item.Key
            };
            coin.TryRegister();
        }

        CustomItem.RegisterItems([typeof(BaseCustomCoin)], true, false);
        base.OnEnabled();
    }

    public override void OnDisabled()
    {
        if (!Config.IsEnabled)
            return;
        Instance = this;
        CustomItem.UnregisterItems();
        base.OnDisabled();
    }
}
