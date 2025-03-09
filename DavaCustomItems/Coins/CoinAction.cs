using DavaCustomItems.Configs;
using DavaCustomItems.Managers;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Pickups.Projectiles;

namespace DavaCustomItems.Coins;

public sealed class CoinAction
{
    public static List<CoinAction> Actions { get; set; } = new();

    public string ActionName = string.Empty;
    public Action<Player /* player */, CoinExtraConfig /* config */, string /* actionName */> RunAction = null;

    public CoinAction(string actionName, Action<Player, CoinExtraConfig, string> runAction)
    {
        ActionName = actionName;
        RunAction = runAction;
    }

    public static void Init()
    {
        Actions.Add(new CoinAction("GiveItem", (player, config, actionName) => 
        {
            ItemType itemToAdd = config.ItemChance.GetRandomWeight(ItemType.None);
            player.AddItem(itemToAdd);
            var hint = config.NameToHint.Get(actionName, string.Empty);
            if (hint != string.Empty)
                player.ShowHint(string.Format(hint, itemToAdd));
        }));

        Actions.Add(new CoinAction("GiveEffect", (player, config, actionName) =>
        {
            var effectToAdd = config.EffectChance.GetRandomWeight();
            if (effectToAdd == null)
                return;
            player.EnableEffect(effectToAdd.EffectType, effectToAdd.Intensity, effectToAdd.Duration, true);
            var hint = config.NameToHint.Get(actionName, string.Empty);
            if (hint != string.Empty)
                player.ShowHint(string.Format(hint, effectToAdd.EffectType));
        }));

        Actions.Add(new CoinAction("ThrowableSpawn", (player, config, actionName) =>
        {
            var randomItem = config.ThrowableSpawnChance.GetRandomWeight(ProjectileType.None);
            if (randomItem == ProjectileType.None)
                return;
            Projectile.CreateAndSpawn(randomItem, player.Position, player.Rotation, true, player);
            var hint = config.NameToHint.Get(actionName, string.Empty);
            if (hint != string.Empty)
                player.ShowHint(hint);
        }));
    }
}
