using DavaCustomItems.Configs;
using DavaCustomItems.Managers;
using DavaCustomItems.PassiveItem;
using DavaCustomItems.Weapons;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups.Projectiles;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.Features;
using MEC;
using PlayerRoles;
using UnityEngine;

namespace DavaCustomItems.Coins;

public sealed class CoinAction
{
    public static List<CoinAction> Actions { get; set; } = [];

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

        Actions.Add(new CoinAction("GiveNegativeEffect", (player, config, actionName) =>
        {
            var effectToAdd = config.GiveNegativeEffectWeight.GetRandomWeight();
            if (effectToAdd == null)
                return;
            player.EnableEffect(effectToAdd.EffectType, effectToAdd.Intensity, effectToAdd.Duration, true);
            var hint = config.NameToHint.Get(actionName, string.Empty);
            if (hint != string.Empty)
                player.ShowHint(string.Format(hint, effectToAdd.EffectType));
        }));

        Actions.Add(new CoinAction("GiveMixedEffect", (player, config, actionName) =>
        {
            var effectToAdd = config.GiveMixedEffectWeight.GetRandomWeight();
            if (effectToAdd == null)
                return;
            player.EnableEffect(effectToAdd.EffectType, effectToAdd.Intensity, effectToAdd.Duration, true);
            var hint = config.NameToHint.Get(actionName, string.Empty);
            if (hint != string.Empty)
                player.ShowHint(string.Format(hint, effectToAdd.EffectType));
        }));

        Actions.Add(new CoinAction("MoreHealth", (player, config, actionName) =>
        {
            var healtToAdd = config.MoreHealthWeight.GetRandomWeight();
            player.Health += healtToAdd;
            player.ShowHint("You got some health!", 5);
        }));

        Actions.Add(new CoinAction("LoseHealth", (player, config, actionName) =>
        {
            var healtToAdd = config.LoseHealthWeight.GetRandomWeight();
            player.Health -= healtToAdd;
            player.ShowHint("You lost some health!", 5);
        }));

        Actions.Add(new CoinAction("MedicalKit", (player, config, actionName) =>
        {
            foreach (var item in config.MedicalKit)
            {
                var citem = Item.Create(item);
                citem.CreatePickup(player.Position);
            }
            player.ShowHint("You received some medical kit!", 5);
        }));

        Actions.Add(new CoinAction("LoseItems", (player, config, actionName) =>
        {
            player.ClearItems();
            player.ShowHint("Oops we removed all your items!", 5);
        }));

        Actions.Add(new CoinAction("TpToSCP", (player, config, actionName) =>
        {
            player.Teleport(Player.List.Where(x => x.IsScp && x.Role.Type != RoleTypeId.Scp079).GetRandomValue());
            player.ShowHint("Teleported to SCP!", 5);
        }));

        Actions.Add(new CoinAction("TpToRandomPlayer", (player, config, actionName) =>
        {
            player.Teleport(Player.List.Where(x => x.IsAlive && x.Role.Type != RoleTypeId.Scp079).GetRandomValue());
            player.ShowHint("Teleported to a random user!", 5);
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
            foreach (var p in Player.List.Where(p => p != player && p.IsAlive && p.Role.Type != RoleTypeId.Scp079))
            {
                p.ShowHint("You can sense reality forcing you somewhere else…", 5);
            }
            Timing.CallDelayed(3, () =>
            {
                List<Player> players = Player.List.Where(p=> p.IsAlive && p.Role.Type != RoleTypeId.Scp079).ToList();
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
                var coin = BaseCustomCoin.Get(Main.Instance.Config.CoinRarityConfigs.Get(CoinRarityType.Normal).Id);
                var item = Item.Create(coin.Type);
                coin.Give(p, item);
                p.CurrentItem = item;
            }

            for (int i = 0; i < 3; i++)
            {
                var coin = BaseCustomCoin.Get(Main.Instance.Config.CoinRarityConfigs.Get(CoinRarityType.Normal).Id);
                coin.Give(player);
            }
        }));


        Actions.Add(new CoinAction("ImmenseFortitude", (player, config, actionName) =>
        {
            player.MaxHealth += 150;
            player.Health += 150;
            player.ShowHint("The coin blesses you with great strength. You have gained an extremely potent pool of health and damage reduction.", 10);

            player.EnableEffect(EffectType.DamageReduction, 10, 0);

            player.SessionVariables["ImmenseFortitude173"] = true;

            CustomEventHandler<HurtingEventArgs> handler = null;
            handler = (ev) =>
            {
                if (ev.Attacker != null && ev.Attacker.Role.Type == RoleTypeId.Scp173 && player.SessionVariables.ContainsKey("ImmenseFortitude173"))
                {
                    ev.Player.SessionVariables["ImmenseFortitude173"] = false;
                    ev.Amount = 50;
                }
                Exiled.Events.Handlers.Player.Hurting.Unsubscribe(handler);
            };
            Exiled.Events.Handlers.Player.Hurting.Subscribe(handler);
        }));

        Actions.Add(new CoinAction("TrueResurrection", (player, config, actionName) =>
        {
            player.ShowHint("The coin gives you a spare lease on life. If you are to die, you will come back to life… But only once…", 10);
            player.SessionVariables["TrueResurrection"] = true;
            Map.Broadcast(10, $"{player.Nickname} has flipped the Legendary coin and was given a second Life.");
            Vector3 deathPosition = player.Position;
            CustomEventHandler<DyingEventArgs> dying = null;
            dying = (ev) => 
            {
                if (ev.Player.SessionVariables.ContainsKey("TrueResurrection") && (bool)ev.Player.SessionVariables["TrueResurrection"])
                {
                    deathPosition = player.Position;
                    Exiled.Events.Handlers.Player.Dying.Unsubscribe(dying);
                }
            };
            Exiled.Events.Handlers.Player.Dying.Subscribe(dying);


            CustomEventHandler<DiedEventArgs> died = null;
            died = (ev) =>
            {
                if (ev.Player.SessionVariables.ContainsKey("TrueResurrection") && (bool)ev.Player.SessionVariables["TrueResurrection"])
                {
                    ev.Player.SessionVariables["TrueResurrection"] = false; // Remove the effect

                    Timing.CallDelayed(0.1f, () =>
                    {
                        ev.Player.Role.Set(ev.TargetOldRole, SpawnReason.ForceClass, RoleSpawnFlags.None);
                        ev.Player.Position = deathPosition;
                        ev.Player.Health = ev.Player.MaxHealth;
                        ev.Player.EnableEffect(EffectType.Invisible, 1, 3);
                        ev.Player.EnableEffect(EffectType.SpawnProtected, 1, 2);
                        ev.Player.EnableEffect(EffectType.Scp207, 1, 10); // Temporary speed boost

                        ev.Player.ShowHint("The coin has Let you Live!", 5);

                    });
                    Exiled.Events.Handlers.Player.Died.Unsubscribe(died);
                }
            };
            Exiled.Events.Handlers.Player.Died.Subscribe(died);
        }));

        Actions.Add(new CoinAction("Necromancy", (player, config, actionName) =>
        {
            Map.Broadcast(5, $"{player.Nickname} has flipped the Legendary Coin and respawned everyone as their minions");
            player.ShowHint("The coin's power grants you an army of minions loyal to you!", 5);

            foreach (var p in Player.List.Where(p => !p.IsAlive))
            {
                Log.Info(p);
                p.Role.Set(player.Role);

                p.Health = 50;
                p.MaxHealth = 50;

                p.Scale = new Vector3(0.5f, 0.5f, 0.5f);

                p.Position = player.Position;
                var jailbird = new Jailbird();
                jailbird.ChargeDamage /= 2;
                jailbird.MeleeDamage /= 2;
                jailbird.TotalCharges = -1_000;
                jailbird.Give(p);
                p.CurrentItem = jailbird;
                // TODO: Figure out how to unsubscibe from it.
                CustomEventHandler<DroppingItemEventArgs> handler = null;
                handler = (ev) =>
                {
                    if (ev.Item != jailbird)
                        return;
                    ev.IsAllowed = false;
                };
                Exiled.Events.Handlers.Player.DroppingItem.Subscribe(handler);

                CustomEventHandler<DyingEventArgs> dying = null;
                dying = (ev) =>
                {
                    if (ev.Player == p)
                    {
                        jailbird.Break();
                        Exiled.Events.Handlers.Player.Dying.Unsubscribe(dying);
                    }
                };
                Exiled.Events.Handlers.Player.Dying.Subscribe(dying);

            }
        }));

        Actions.Add(new CoinAction("DrugCocktail", (player, config, actionName) =>
        {
            Map.Broadcast(5, $"{player.Nickname} has flipped the Legendary Coin and Has been given a Cocktail of effects");
            player.ShowHint("The coin blesses you with euphoria in the form of every drug known to mankind.", 5);
            player.EnableEffect(EffectType.Invigorated, 1, 0);
            player.EnableEffect(EffectType.BodyshotReduction, 5, 0);
            player.EnableEffect(EffectType.DamageReduction, 5, 0);
            player.ChangeEffectIntensity(EffectType.MovementBoost, 10, 0);
            player.EnableEffect(EffectType.RainbowTaste, 1, 0);
            player.EnableEffect(EffectType.Vitality, 1, 0);
            player.EnableEffect(EffectType.SilentWalk, 1, 0);

        }));

        Actions.Add(new CoinAction("CursedDrugCocktail", (player, config, actionName) =>
        {
            Map.Broadcast(5, $"{player.Nickname} has flipped the Legendary Coin and Has been given a Cocktail of effects");
            player.ShowHint("The coin blesses you with euphoria in the form of every drug known to mankind.", 5);
            player.EnableEffect(EffectType.AmnesiaVision, 1, 180);
            player.EnableEffect(EffectType.Burned, 1, 180);
            player.EnableEffect(EffectType.Concussed, 1, 180);
            player.EnableEffect(EffectType.Stained, 1, 180);
            player.EnableEffect(EffectType.InsufficientLighting, 1, 180);
            player.EnableEffect(EffectType.Slowness, 2, 180);
            player.EnableEffect(EffectType.SinkHole, 1, 180);
        }));

        Actions.Add(new CoinAction("ExperimentalItem", (player, config, actionName) =>
        {
            Map.Broadcast(5, $"{player.Nickname} has flipped the Legendary Coin and Has Gained a Unique Item");

            int randomNumber = RNGManager.RNG.Next(1, 4);
            string itemName = string.Empty;

            switch (randomNumber)
            {
                case 1:
                    var Nerfgun = NerfGun.Get(600);
                    Nerfgun.Give(player);
                    break;
                case 2:
                    var Swappergun = SwapperGun.Get(9000);
                    Swappergun.Give(player);
                    break;
                case 3:
                    var NoGogglesVest = NoGogglesArmor.Get(700);
                    NoGogglesVest.Give(player);
                    break;
                case 4:
                    var BrokenLamps = BrokenLamp.Get(701);
                    BrokenLamps.Give(player);
                    break;
                default:
                    break;
            }

            player.ShowHint("The coin blesses you with a special item!", 5);

        }));

        Actions.Add(new CoinAction("Combustion", (player, config, actionName) =>
        {
            foreach (var p in Player.List.Where(p => p.IsAlive))
            {
                p.ShowHint("The coin blesses you with a grenade!", 5);

                var grenade = new ExplosiveGrenade(ItemType.GrenadeHE);
                grenade.SpawnActive(p.Position);
                
            }
            Map.Broadcast(5, $"{player.Nickname} has flipped the Legendary Coin and has Exploded Everyone");
        }));

        Actions.Add(new CoinAction("awareness", (player, config, actionName) =>
        {
            Map.Broadcast(5, $"{player.Nickname} has flipped the Legendary Coin and Became Omnipresent");
            //Start a coroutine
            // once every muinite
            // scan all players
            // display to user what zone they are in 
            // repeat
        }));

        Actions.Add(new CoinAction("RocketMan", (player, config, actionName) =>
        {
            player.ShowHint("The coins unstable power unleashes calamity on you", 5);

            for(int i = 0; i < 3; i++) // replace 3 with variable later
            {
                var grenade = new ExplosiveGrenade(ItemType.GrenadeHE);
                {
                    float randomNumber = RNGManager.GetRandom(3.0f, 5.0f);
                    grenade.FuseTime = randomNumber;
                } 
                grenade.SpawnActive(player.Position);
            }
            for (int i = 0; i < 2; i++) // replace 2 with variable later
            {
                FlashGrenade flash = (FlashGrenade)FlashGrenade.Create(ItemType.GrenadeFlash);
                
                flash.SpawnActive(player.Position);
                {
                    float randomNumber = RNGManager.GetRandom(3.0f, 5.0f);
                    flash.FuseTime = randomNumber;
                }
                flash.SpawnActive(player.Position);
            }
            for (int i = 0; i < 2; i++) // replace 2 with variable later
            {
                var SCP018 = new Scp018(ItemType.SCP018);
                SCP018.SpawnActive(player.Position);
            }

        }));
    }
}