using DavaCustomItems.Coins;
using DavaCustomItems.Items;
using DavaCustomItems.Managers;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.CustomItems.API.Features;

namespace DavaCustomItems;

public sealed class Main : Plugin<Config>
{
    public static Main Instance { get; private set; }
    #region Plugin Info
    public override string Author => "SlejmUr,Falout01";
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
        RainbowLightManager.Init();
        LightSerialManager.Init();
        CoinAction.Init();
        foreach (var item in Config.CoinRarityConfigs)
        {
            if (item.Key == CoinRarityType.None)
                continue;
            BaseCustomCoin coin = new()
            {
                Id = (uint)CustomItemsEnum.None + (uint)item.Key,
                Description = item.Value.Description,
                Name = item.Value.Name,
                Rarity = item.Key,
                ExtraConfig = item.Value.ExtraConfig,
                CoinPickupHint = item.Value.CoinPickUpHint,
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
        Instance = null;
        RainbowLightManager.UnInit();
        LightSerialManager.UnInit();
        CustomItem.UnregisterItems();
        base.OnDisabled();
    }
}
