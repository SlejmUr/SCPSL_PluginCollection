using DavaCustomItems.Configs;
using DavaCustomItems.Managers;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups.Projectiles;
using MEC;
using UnityEngine;

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
        Actions.Add(new CoinAction("NoAction", (player, config, actionName) =>
        {

        }));

        Actions.Add(new CoinAction("GiveItem", (player, config, actionName) => 
        {
            ItemType itemToAdd = config.GiveItemWeight.GetRandomWeight(ItemType.None);
            player.AddItem(itemToAdd);
            var hint = config.NameToHint.Get(actionName, string.Empty);
            if (hint != string.Empty)
                player.ShowHint(string.Format(hint, itemToAdd));
        }));

        Actions.Add(new CoinAction("GiveEffect", (player, config, actionName) =>
        {
            var effectToAdd = config.GivePositiveEffectWeight.GetRandomWeight();
            if (effectToAdd == null)
                return;
            player.EnableEffect(effectToAdd.EffectType, effectToAdd.Intensity, effectToAdd.Duration, true);
            var hint = config.NameToHint.Get(actionName, string.Empty);
            if (hint != string.Empty)
                player.ShowHint(string.Format(hint, effectToAdd.EffectType));
        }));

        Actions.Add(new CoinAction("ThrowableSpawn", (player, config, actionName) =>
        {
            var randomItem = config.ThrowableSpawnWeight.GetRandomWeight(ProjectileType.None);
            if (randomItem == ProjectileType.None)
                return;
            Projectile.CreateAndSpawn(randomItem, player.Position, player.Rotation, true, player);
            var hint = config.NameToHint.Get(actionName, string.Empty);
            if (hint != string.Empty)
                player.ShowHint(hint);
        }));

        Actions.Add(new CoinAction("ShufflePlayers", (player, config, actionName) =>
        {
            player.ShowHint("The coin's power grows unstable, everyone’s positions will swap.", 5);
            foreach (var p in Player.List.Where(p => p != player))
            {
                p.ShowHint("You can sense reality forcing you somewhere else…", 5);
            }
            Timing.CallDelayed(3, () =>
            {
                List<Player> players = Player.List.ToList();
                List<Vector3> positions = players.Select(p => p.Position).ToList();
                positions.ShuffleList(RNGManager.RNG);

                for (int i = 0; i < players.Count; i++)
                {
                    players[i].Position = positions[i];
                }
                Timing.CallDelayed(2, () =>
                {
                    Map.Broadcast(5, $"{player.Nickname} flipped the Legendary Coin and shuffled everyone’s positions.");
                });
            });
        }));

        Actions.Add(new CoinAction("GreatGamble", (player, config, actionName) =>
        {
            Map.Broadcast(10, $"{player.Nickname} flipped the Legendary coin and unleashed a gambling cataclysm.");
            Map.ShowHint("A great gamble is upon us… Everyone has been gifted with a coin.", 5);

            foreach (var p in Player.List)
            {
                var coin = BaseCustomCoin.Get(Main.Instance.Config.CoinRarityConfigs.Get(CoinRarityType.NormalCoin).Id);
                var item = Item.Create(coin.Type);
                coin.Give(p, item);
                p.CurrentItem = item;
            }

            for (int i = 0; i < 3; i++)
            {
                var coin = BaseCustomCoin.Get(Main.Instance.Config.CoinRarityConfigs.Get(CoinRarityType.NormalCoin).Id);
                coin.Give(player);
            }
        }));
    }
}
