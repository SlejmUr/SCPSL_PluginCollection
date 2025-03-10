using DavaCustomItems.Configs;
using DavaCustomItems.Managers;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups.Projectiles;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.Features;
using MEC;
using PlayerRoles;
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

        Actions.Add(new CoinAction("GivePositiveEffect", (player, config, actionName) =>
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


        Actions.Add(new CoinAction("ImmenseFortitude", (player, config, actionName) =>
        {
            player.MaxHealth += 150;
            player.Health += 150;
            player.ShowHint("The coin blesses you with great strength. You have gained an extremely potent pool of health and damage reduction.", 10);

            player.EnableEffect(EffectType.DamageReduction, 0, true);
            player.ChangeEffectIntensity(EffectType.DamageReduction, 5); // in %

            player.SessionVariables["ImmenseFortitude173"] = true;

            CustomEventHandler<HurtingEventArgs> handler = null;
            handler = (ev) =>
            {
                if (ev.Attacker != null && ev.Attacker.Role.Type == RoleTypeId.Scp173 && player.SessionVariables.ContainsKey("ImmenseFortitude173"))
                {
                    ev.Player.SessionVariables["ImmenseFortitude173"] = false;
                    ev.Amount = Mathf.Min(ev.Amount, 150); // Limit damage to 150
                }
                Exiled.Events.Handlers.Player.Hurting.Unsubscribe(handler);
            };
            Exiled.Events.Handlers.Player.Hurting.Subscribe(handler);
        }));

        Actions.Add(new CoinAction("TrueRessurection", (player, config, actionName) =>
        {
            player.ShowHint("The coin gives you a spare lease on life. If you are to die, you will come back to life… But only once…", 10);
            player.SessionVariables["TrueResurrection"] = true;
            Map.Broadcast(10, $"{player.Nickname} has flipped the Legendary coin and was given a second Life.");

            CustomEventHandler<DyingEventArgs> handler = null;
            handler = (ev) =>
            {
                if (ev.Player.SessionVariables.ContainsKey("TrueResurrection") && (bool)ev.Player.SessionVariables["TrueResurrection"])
                {
                    ev.IsAllowed = false; // Prevent the player from dying
                    ev.Player.SessionVariables["TrueResurrection"] = false; // Remove the effect

                    // Respawn the player at their point of death
                    Vector3 deathPosition = ev.Player.Position;

                    Timing.CallDelayed(0.1f, () =>
                    {
                        ev.Player.Position = deathPosition;
                        ev.Player.Health = ev.Player.MaxHealth;
                        ev.Player.EnableEffect(EffectType.Invisible, 10);
                        ev.Player.EnableEffect(EffectType.SpawnProtected, 2);
                        ev.Player.EnableEffect(EffectType.Scp207, 10); // Temporary speed boost

                        ev.Player.ShowHint("The coin has Let you Live!", 5);

                    });
                }
                Exiled.Events.Handlers.Player.Dying.Unsubscribe(handler);
            };
            Exiled.Events.Handlers.Player.Dying.Subscribe(handler);
        }));

        Actions.Add(new CoinAction("Necromancy", (player, config, actionName) =>
        {
            Map.Broadcast(5, $"{player.Nickname} has flipped the Legendary Coin and respawned everyone as their minions");
            player.ShowHint("The coin's power grants you an army of minions loyal to you!", 5);

            foreach (var p in Player.List.Where(p => !p.IsAlive))
            {
                p.Role = player.Role;

                p.Health = 50;
                p.MaxHealth = 50;

                p.Scale = new Vector3(0.5f, 0.5f, 0.5f);

                p.Position = player.Position;

                var jailbird = new Jailbird();
                jailbird.TotalCharges = 100_000;
                jailbird.Give(p);
                p.CurrentItem = jailbird;
                // TODO: Figure out how to unsubscibe from it.
                CustomEventHandler<DroppingItemEventArgs> handler = null;
                handler = (ev) =>
                {
                    if (ev.Item == jailbird)
                    {
                        ev.IsAllowed = false;
                    }
                };
                Exiled.Events.Handlers.Player.DroppingItem.Subscribe(handler);

                


            }
        }));
    }
}
