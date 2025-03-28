using DavaCustomItems.Configs;
using DavaCustomItems.Items;
using DavaCustomItems.Managers;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups.Projectiles;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using InventorySystem.Items.Usables.Scp330;
using MEC;
using PlayerRoles;
using System.Linq;
using UnityEngine;

namespace DavaCustomItems.Coins;

public sealed class CoinAction(string actionName, Action<Player, CoinExtraConfig, List<object>> runAction)
{
    public static List<CoinAction> Actions { get; set; } = [];

    public string ActionName = actionName;
    public Action<Player /* player */, CoinExtraConfig /* config */, List<object> /* ExtraSettings */> RunAction = runAction;

    public static void Init()
    {
        Actions.Add(new CoinAction("NoAction", (player, config, extraSettings) =>
        {

        }));

        Actions.Add(new CoinAction("GiveItem", (player, config, extraSettings) =>
        {
            if (extraSettings.IsEmpty())
            {
                player.ShowHint("Please report this error To a developer", 5);
                return;
            }
            ItemType itemType = ObjectConvertManager.ParseToEnum(extraSettings.RandomItem(), ItemType.None);
            var citem = Item.Create(itemType);
            citem.CreatePickup(player.Position);
            player.ShowHint("You were given a random item", 3);

        }));

        Actions.Add(new CoinAction("GivePositiveEffect", (player, config, extraSettings) =>
        {
            if (extraSettings.IsEmpty())
            {
                player.ShowHint("Please report this error To a developer", 5);
                return;
            }
            EffectConfig effect = ObjectConvertManager.ParseToEffectConfig(extraSettings.RandomItem(), new());
            player.EnableEffect(effect.EffectType, effect.Intensity, effect.Duration, true);
            player.ShowHint($"You were given {effect.EffectType} Temporarily", 3);
        }));

        Actions.Add(new CoinAction("GiveNegativeEffect", (player, config, extraSettings) =>
        {
            if (extraSettings.IsEmpty())
            {
                player.ShowHint("Please report this error To a developer", 5);
                return;
            }   
            EffectConfig effect = ObjectConvertManager.ParseToEffectConfig(extraSettings.RandomItem(), new());
            player.EnableEffect(effect.EffectType, effect.Intensity, effect.Duration, true);
            player.ShowHint($"You were given {effect.EffectType} Temporarily ", 3);
        }));

        Actions.Add(new CoinAction("GiveMixedEffect", (player, config, extraSettings) =>
        {
            if (extraSettings.IsEmpty())
            {
                player.ShowHint("Please report this error To a developer", 5);
                return;
            }
            EffectConfig effect = ObjectConvertManager.ParseToEffectConfig(extraSettings.RandomItem(), new());
            player.EnableEffect(effect.EffectType, effect.Intensity, effect.Duration, true);
            player.ShowHint($"You were given {effect.EffectType} Temporarily", 3);
        }));
        
        Actions.Add(new CoinAction("MoreHealth", (player, config, extraSettings) =>
        {
            if (extraSettings.IsEmpty())
            {
                player.ShowHint("Please report this error To a developer", 5);
                return;
            }
            int result = ObjectConvertManager.ParseToInt(extraSettings.RandomItem());
            player.Health += result;
            player.ShowHint("You got some health!", 5);
        }));

        Actions.Add(new CoinAction("LoseHealth", (player, config, extraSettings) =>
        {
            if (extraSettings.IsEmpty())
            {
                player.ShowHint("Please report this error To a developer", 5);
                return;
            }
            int item = ObjectConvertManager.ParseToInt(extraSettings.RandomItem());
            player.Health -= item;
            player.ShowHint("You lost some health!", 5);
        }));

        Actions.Add(new CoinAction("1hp", (player, config, extraSettings) =>
        {
            player.Health = 1;
            player.ShowHint("You Are 1HP!!", 5);
        }));

        Actions.Add(new CoinAction("MedicalKit", (player, config, extraSettings) =>
        {
            if (extraSettings.IsEmpty())
            {
                player.ShowHint("Please report this error To a developer", 5);
                return;
            }
            foreach (var item in extraSettings)
            {
                ItemType itemType = ObjectConvertManager.ParseToEnum(extraSettings.RandomItem(), ItemType.None);
                var citem = Item.Create(itemType);
                citem.CreatePickup(player.Position);
            }
            player.ShowHint("You received some medical kit!", 5);
        }));

        Actions.Add(new CoinAction("LoseItems", (player, config, extraSettings) =>
        {
            player.ClearItems();
            player.ShowHint("Oops we removed all your items!", 5);
        }));

        Actions.Add(new CoinAction("TpToSCP", (player, config, extraSettings) => // rare = also give 1s of slowness
        {
            var scp = Player.List.Where(x => x.IsScp && x.Role.Type != RoleTypeId.Scp079);
            if (!scp.Any())
            {
                player.ShowHint("No SCP to Teleport to... Lucky...", 5);
                return;
            }
            player.Teleport(scp.GetRandomValue());
            player.ShowHint("Teleported to SCP!", 5);

            if (!extraSettings.IsEmpty() && (bool)extraSettings[0])
            {
                player.EnableEffect(EffectType.Slowness, 20, 2);
            }
        }));

        Actions.Add(new CoinAction("TpToRandomPlayer", (player, config, extraSettings) =>
        {
            var scp = Player.List.Where(x => x.IsAlive && !x.IsScp);
            if (!scp.Any())
            {
                player.ShowHint("No person to Teleport to... Lucky...", 5);
                return;
            }
            player.Teleport(scp.GetRandomValue());
            player.ShowHint("Teleported to a random user!", 5);
        }));


        Actions.Add(new CoinAction("Blackout", (player, config, extraSettings) =>
        {
            if (extraSettings.IsEmpty())
            {
                player.ShowHint("Please report this error To a developer", 5);
                return;
            }
            Map.TurnOffAllLights(ObjectConvertManager.ParseToInt(extraSettings[0])); 
            player.ShowHint("You cut the lights out", 5);
        }));

        Actions.Add(new CoinAction("RandomKeycard", (player, config, extraSettings) =>
        {
            if (extraSettings.IsEmpty())
            {
                player.ShowHint("Please report this error To a developer", 5);
                return;
            }
            ItemType itemType = ObjectConvertManager.ParseToEnum(extraSettings.RandomItem(), ItemType.None);
            var citem = Item.Create(itemType);
            citem.CreatePickup(player.Position);
            player.ShowHint($"You got a random Keycard ({itemType})", 5);
        }));

        Actions.Add(new CoinAction("RandomWeapon", (player, config, extraSettings) =>
        {
            if (extraSettings.IsEmpty())
            {
                player.ShowHint("Please report this error To a developer", 5);
                return;
            }
            ItemType itemType = ObjectConvertManager.ParseToEnum(extraSettings.RandomItem(), ItemType.None);
            var citem = Item.Create(itemType);
            citem.CreatePickup(player.Position);
            player.ShowHint($"You Got a random Weapon ({itemType})", 5);
        }));

        Actions.Add(new CoinAction("Freeze", (player, config, extraSettings) =>
        {
            player.EnableEffect(EffectType.Slowness, 100, 5);
            player.EnableEffect(EffectType.Hypothermia, 1, 10);
            player.ShowHint("You Were Frozen for 10 Seconds", 5);
        }));

        Actions.Add(new CoinAction("Gnome", (player, config, extraSettings) =>
        {
            player.Scale = new Vector3(0.75f, 0.75f, 0.75f);
            player.ShowHint("You Have Become a Gnome", 5);
        }));

        Actions.Add(new CoinAction("TallMan", (player, config, extraSettings) =>
        {
            player.Scale = new Vector3(1.0f, 1.15f, 1.0f);
            player.ShowHint("You Have Become a Tall Man!", 5);
        }));

        Actions.Add(new CoinAction("WideMan", (player, config, extraSettings) =>
        {
            player.Scale = new Vector3(1.15f, 1.0f, 1.15f);
            player.ShowHint("You Have Become a Wide", 5);
        }));

        Actions.Add(new CoinAction("NormalMan", (player, config, extraSettings) =>
        {
            player.Scale = Vector3.one;
            player.ShowHint("You Have Become normal!", 5);
        }));

        Actions.Add(new CoinAction("GiveXP", (player, config, extraSettings) =>
        {
            if (extraSettings.IsEmpty())
            {
                player.ShowHint("Please report this error To a developer", 5);
                return;
            }
            int xp = ObjectConvertManager.ParseToInt(extraSettings[0]);
            Server.ExecuteCommand($"xp give {xp} {player.Id}", ServerConsole.Scs);
            player.ShowHint($"You Got {xp} XP from the Coin!", 5);
        }));

        Actions.Add(new CoinAction("GrenadeDrop", (player, config, extraSettings) => // make optionable (via for loop for amount of gernades
        {
            var grenade = new ExplosiveGrenade(ItemType.GrenadeHE);
            grenade.SpawnActive(player.Position);
            player.ShowHint("Oops, Watch your feet!", 5);
        })); 

        Actions.Add(new CoinAction("DisappearingAct", (player, config, extraSettings) =>
        {
            FlashGrenade flash = new() 
            {
                FuseTime = 1
            };
            flash.SpawnActive(player.Position);

            var accessibleRooms = Room.List.Where(room => 
            room.Zone != ZoneType.LightContainment && 
            room.Type != RoomType.HczTesla &&
            room.Type != RoomType.HczTestRoom &&
            room.Type != RoomType.Pocket && 
            room.Type != RoomType.Unknown &&
            room.RoomName != MapGeneration.RoomName.EzEvacShelter &&
            room.RoomShape == MapGeneration.RoomShape.Straight).ToList();

            if (Warhead.IsDetonated)  // make higher in the room 
            {
                accessibleRooms = Room.Get(ZoneType.Surface).ToList();
            }
            var randomRoom = accessibleRooms[RNGManager.RNG.Next(accessibleRooms.Count)];

            player.Teleport(randomRoom);
            player.ShowHint("You Disappeared! (into another room)", 5);

            if (!extraSettings.IsEmpty() && ObjectConvertManager.ParseToBool(extraSettings[0]))
            {
                player.EnableEffect(EffectType.Invisible, 1, 5);
            }
        }));

        Actions.Add(new CoinAction("FoodPoisoning", (player, config, extraSettings) => 
        {
            player.ShowHint("You Shit Yourself!", 3);
            player.PlaceTantrum();
        }));

        Actions.Add(new CoinAction("SourTooth", (player, config, extraSettings) =>
        {
            player.ShowHint("You Got some VERY sour Candy!", 3);
            player.TryAddCandy(CandyKindID.Pink);
        }));

        Actions.Add(new CoinAction("SweetTooth", (player, config, extraSettings) =>
        {
            if (extraSettings.IsEmpty())
            {
                player.ShowHint("Please report this error To a developer", 5);
                return;
            }
            CandyKindID candy = ObjectConvertManager.ParseToEnum(extraSettings.RandomItem(), CandyKindID.None);
            player.ShowHint("You Got some Sweet Candy!", 3);
            player.TryAddCandy(candy);
        }));

        Actions.Add(new CoinAction("ThrowableSpawn", (player, config, extraSettings) =>
        {
            if (extraSettings.IsEmpty())
            {
                player.ShowHint("Please report this error To a developer", 5);
                return;
            }
            ProjectileType randomItem = ObjectConvertManager.ParseToEnum(extraSettings.RandomItem(), ProjectileType.None);
            Projectile.CreateAndSpawn(randomItem, player.Position, player.Rotation, true, player);
            player.ShowHint($"You got hit with a {randomItem}!", 3);
        }));

        Actions.Add(new CoinAction("NeverQuit", (player, config, extraSettings) =>
        {
            var normal = CustomItem.Get((uint)CustomItemsEnum.NormalCoin);
            var rare = CustomItem.Get((uint)CustomItemsEnum.RareCoin);
            var legendary = CustomItem.Get((uint)CustomItemsEnum.LegendaryCoin);
            var serial = player.CurrentItem.Serial;
            if (normal.TrackedSerials.Contains(serial))
            {
                //if the player has normal coin
                for (int i = 0; i < 2; i++)
                {
                    var coin = CustomItem.Get((uint)CustomItemsEnum.NormalCoin);
                    coin.Give(player);
                }
            }
            else if (rare.TrackedSerials.Contains(serial))
            {
                //if the player has rare coin
                for (int i = 0; i < 2; i++)
                {
                    var coin = CustomItem.Get((uint)CustomItemsEnum.RareCoin);
                    coin.Give(player);
                }
            }
            else
            {
                // we return if neither!
                player.ShowHint("Please report this error To a developer", 5);
                return;
            }

            player.ShowHint("TRUE GAMBLERS NEVER QUIT!", 5);

        }));

        Actions.Add(new CoinAction("AutoNuke", (player, config, extraSettings) =>
        {
            Warhead.Start();
            player.ShowHint("You have activated the Nuke!", 5);
        }));

        Actions.Add(new CoinAction("MaxHP", (player, config, extraSettings) =>
        {
            if (extraSettings.IsEmpty())
            {
                player.ShowHint("Please report this error To a developer", 5);
                return;
            }
            int item = ObjectConvertManager.ParseToInt(extraSettings.RandomItem());
            player.MaxHealth += item;
            player.ShowHint("You suddenly feel stronger, your Max HP increased!", 5);
        }));


        Actions.Add(new CoinAction("MinHP", (player, config, extraSettings) =>
        {
            if (extraSettings.IsEmpty())
            {
                player.ShowHint("Please report this error To a developer", 5);
                return;
            }
            int item = ObjectConvertManager.ParseToInt(extraSettings.RandomItem());
            player.MaxHealth -= item;
            player.ShowHint("You suddenly feel weaker, your Max HP decreased!", 5);
        }));

        Actions.Add(new CoinAction("Jackpot", (player, config, extraSettings) =>
        {
            var normal = CustomItem.Get((uint)CustomItemsEnum.NormalCoin);
            var rare = CustomItem.Get((uint)CustomItemsEnum.RareCoin);
            var legendary = CustomItem.Get((uint)CustomItemsEnum.LegendaryCoin);
            var serial = player.CurrentItem.Serial;
            if (normal.TrackedSerials.Contains(serial))
            {
                player.RemoveItem(player.CurrentItem);
                rare.Give(player);
            }
            else if (rare.TrackedSerials.Contains(serial))
            {
                player.RemoveItem(player.CurrentItem);
                legendary.Give(player);
            }
            else
            {
                player.ShowHint("Please report this error To a developer", 5);
                return;
            }

            player.ShowHint("YOU HIT THE JACKPOT!! Your coin was upgraded!", 5);

        }));

        Actions.Add(new CoinAction("Exposed", (player, config, extraSettings) =>
        {
            var Randomplayer = Player.List.Where(x => x.IsAlive && !x.IsScp).GetRandomValue();

            player.ShowHint("You’ve just exposed someone’s location, I hope they don’t mind.", 5);

            if (Randomplayer != null)
            {
                if (!extraSettings.IsEmpty() && ObjectConvertManager.ParseToBool(extraSettings[0]))
                {
                    string playername = Randomplayer.Nickname;
                    string playerClass = Randomplayer.Role.Type.ToString();
                    string playerZone = Randomplayer.CurrentRoom.Type.ToString();

                    string message = $"Coin: {playername} ({playerClass}) is located in {playerZone}";
                    Map.Broadcast(5, message);
                }
                else
                {
                    string playerClass = Randomplayer.Role.Type.ToString();
                    string playerZone = Randomplayer.CurrentRoom.Type.ToString();

                    string message = $"Coin: A {playerClass} is located in {playerZone}";
                    Map.Broadcast(5, message);
                }
            }


        }));

        Actions.Add(new CoinAction("SPEEED", (player, config, extraSettings) =>
        {
            player.ShowHint("SPEEEEEED BOOST", 5);

            if (!extraSettings.IsEmpty() && ObjectConvertManager.ParseToBool(extraSettings[0]))
            {
                player.EnableEffect(EffectType.MovementBoost, 80, 4);
            }
            else
            {
                player.EnableEffect(EffectType.MovementBoost, 50, 7);
            }

        }));

        Actions.Add(new CoinAction("StarPower", (player, config, extraSettings) => //not implement yet
        {
            player.ShowHint("You got a Star Power (Not affiliated with anything)", 5);

            player.EnableEffect(EffectType.MovementBoost, 60, 5);

            int oldId = LightFollowManager.StopFollow(player); // remove if has any follower
            // add Breathing light to user
            int id = LightManager.MakeLight(player.Position, SetLightConfigs.Breathing_LightConfig);

            Timing.CallDelayed(5.5f, () => 
            {
                LightFollowManager.StopFollow(player);
                LightManager.RemoveLight(id);
                if (oldId != -1)
                {
                    LightFollowManager.StartFollow(oldId, player);
                }
            });
        }));

        Actions.Add(new CoinAction("Timeout", (player, config, extraSettings) => //not implement yet
        {
            player.ShowHint("You have been put in time out! Naughty, naughty…", 5);

            var playerPosition = player.Position;
            player.Position = new Vector3(40, 1015 ,-30);

            Timing.CallDelayed(5, () =>
            {
                player.Position = playerPosition;
            });
        }));

        Actions.Add(new CoinAction("SideSwapper", (player, config, extraSettings) => //not implement yet
        {
            if (extraSettings.IsEmpty())
            {
                player.ShowHint("Please report this error To a developer", 5);
                return;
            }
            int duration = ObjectConvertManager.ParseToInt(extraSettings.RandomItem());
            var pos = player.Position;
            RoleTypeId originalRole = player.Role.Type;
            Team originalTeam = player.Role.Team;

            List<RoleTypeId> possibleNewRole =
            [
                RoleTypeId.ClassD,
                RoleTypeId.Scientist,
                RoleTypeId.FacilityGuard,
                RoleTypeId.NtfPrivate,
                RoleTypeId.NtfSergeant,
                RoleTypeId.NtfCaptain, 
                RoleTypeId.NtfSpecialist,
                RoleTypeId.ChaosConscript,
                RoleTypeId.ChaosRifleman,
                RoleTypeId.ChaosRepressor,
                RoleTypeId.ChaosMarauder,
                RoleTypeId.Scp0492 
            ];
            RoleTypeId newRole = possibleNewRole.Where(role => player.Role.Team != originalTeam).GetRandomValue();;

            player.Role.Set(newRole, SpawnReason.ForceClass, RoleSpawnFlags.None);
            player.Position = pos;
            player.ShowHint($"You have temporarily turned into a {newRole}!", 5);

            Timing.CallDelayed(duration, () =>
            {
                var pos = player.Position;
                player.Role.Set(originalRole, SpawnReason.ForceClass, RoleSpawnFlags.None);
                player.Position = pos;
                player.ShowHint("You have reverted back to your original class!", 5);
            });
        }));

        Actions.Add(new CoinAction("Lootbox", (player, config, extraSettings) =>
        {
            // drop all their items
            player.Handcuff();
            player.RemoveHandcuffs();

            int itemCount = 5; // Number of random items to drop
            for (int i = 0; i < itemCount; i++)
            {
                ItemType itemType = ObjectConvertManager.ParseToEnum(extraSettings.RandomItem(), ItemType.None);
                var citem = Item.Create(itemType);
                citem.CreatePickup(player.Position);
            }
            player.ShowHint("You have become the micro transation", 5);
        }));

        Actions.Add(new CoinAction("InsultToInjury", (player, config, extraSettings) =>
        {
            var randomPlayer = Player.List.GetRandomValue(p => p != player);
            if (randomPlayer == null)
            {
                player.ShowHint("You could not insult anyone", 5);
                return;
            }
            string name = randomPlayer.Nickname ?? "Nobody??";
            if (extraSettings.IsEmpty())
            {
                player.ShowHint("Please Report this Error To a Developer", 5);
                return;
            }

            int damage = ObjectConvertManager.ParseToInt(extraSettings.RandomItem());

            if (randomPlayer.IsScp)
                damage *= 5;

            player.Hurt(damage);

            string[] insults =
            [
                $"{name} thinks you smell but doesn’t want to tell you",
                $"{name} said some terrible things about you…",
                $"haha, oh sorry, {name} just told a really funny joke about you",
                $"{name} wanted you to feel pain…",
                $"“Yeah I think {player.Nickname} is dumb” - {name}",
                $"I think you should uninstall - {name}",
            ];

            string insult = insults[RNGManager.RNG.Next(insults.Length)];
            randomPlayer.ShowHint($"Coin: {insult} ... oof", 5);
            player.ShowHint($"You just insulted {name}", 5);
        }));

        Actions.Add(new CoinAction("SpreadTheLove", (player, config, extraSettings) =>
        {
            var randomPlayer = Player.List.Where(p => p.IsAlive && p != player).GetRandomValue();
            if (randomPlayer == null)
            {
                player.ShowHint("There is no one alive to spread the love to!", 5);
                return;
            }
            var normal = CustomItem.Get((uint)CustomItemsEnum.NormalCoin);
            var rare = CustomItem.Get((uint)CustomItemsEnum.RareCoin);
            var serial = player.CurrentItem.Serial;
            Item coinItem = null;

            if (normal.TrackedSerials.Contains(serial))
            {
                coinItem = Item.Create(normal.Type);
                normal.Give(randomPlayer, coinItem);
            }
            else if (rare.TrackedSerials.Contains(serial))
            {
                coinItem = Item.Create(rare.Type);
                rare.Give(randomPlayer, coinItem);
            }
            else
            {
                player.ShowHint("Please report this error To a developer", 5);
                return;
            }

            if (coinItem != null)
            {
                randomPlayer.CurrentItem = coinItem;
            }

            player.ShowHint("Spread the gambling love like it’s a disease!", 5);
            randomPlayer.ShowHint("You were given a coin by another like minded gambler!", 5);
        }));

        Actions.Add(new CoinAction("SITREP", (player, config, extraSettings) =>
        {
            var scpSubjects = Player.List.Where(p => p.IsScp && p.Role.Type != RoleTypeId.Scp0492).ToList();
            int scpCount = scpSubjects.Count;

            foreach (var scp in scpSubjects)
            {
                scp.ShowHint("Coin has Exposed You!", 5);
            }

            if (scpCount == 0)
            {
                Cassie.Message("NO SCP FOUND ", false, false);
            }
            else
            {
                string scpList = string.Join(" ", scpSubjects.Select(p => "SCP " + p.Role.Type.ToString().ToUpper().Replace("SCP", string.Empty).Aggregate("", (c, i) => c + i + ' ')));
                Cassie.Message($"AWAITINGRECONTAINMENT {scpCount} SCP, {scpList}", false, false);
            }

            player.ShowHint("I hope this helps! - the Coin", 5);
        }));

        /*Actions.Add(new CoinAction("QuestGiver", (player, config, extraSettings) =>
        {
              var rooms = Room.List.Where(room => room.Zone != ZoneType.Pocket && room.Zone != ZoneType.Unknown).ToList();
              var targetRoom = rooms.GetRandomValue();

              player.ShowHint($"Coin Quest: Visit {targetRoom.Name} to be given bountiful rewards!", 5);

              void OnPlayerEnterRoom(whatever to enter room event arg ev)
              {
                  if (ev.Player == player && ev.NewRoom == targetRoom)
                  {
                      player.ShowHint("You completed the Quest!!", 5);

                      // Grant two random positive effects
                      for (int i = 0; i < 2; i++)
                      {
                          EffectConfig effect = ObjectConvertManager.ParseToEffectConfig(extraSettings.RandomItem(), new());
                          player.EnableEffect(effect.EffectType, effect.Intensity, effect.Duration, true);
                          player.ShowHint($"You were given {effect.EffectType} for completing the quest!", 5);
                      }

                      Exiled.Events.Handlers.Player. -= OnPlayerEnterRoom;
                  }
              }

              Exiled.Events.Handlers.Player. += OnPlayerEnterRoom;
        })); 
        */

        Actions.Add(new CoinAction("BecomeScp", (player, config, extraSettings) =>
        {
            List<RoleTypeId> scpRoles = new List<RoleTypeId>
    {
        RoleTypeId.Scp049,
        RoleTypeId.Scp0492,
        RoleTypeId.Scp096,
        RoleTypeId.Scp106,
        RoleTypeId.Scp173,
        RoleTypeId.Scp939
    };

            RoleTypeId randomScpRole = scpRoles.GetRandomValue();
            player.Role.Set(randomScpRole, SpawnReason.ForceClass, RoleSpawnFlags.None);
            player.ShowHint($"You have become {randomScpRole}!", 5);
        }));


        // Legendary Coin Actions Below

        Actions.Add(new CoinAction("ShufflePlayers", (player, config, extraSettings) => 
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
                    Map.Broadcast(5, $"{player.Nickname} flipped the <color=orange>Legendary</color> and shuffled everyone’s positions.");
                });
            });
        }));

        Actions.Add(new CoinAction("GreatGamble", (player, config, extraSettings) =>
        {
            Map.Broadcast(10, $"{player.Nickname} flipped the <color=orange>Legendary</color> coin and unleashed a gambling cataclysm.");
            Map.ShowHint("A great gamble is upon us… Everyone has been gifted with a coin.", 5); 

            foreach (var p in Player.List.Where(p=> p.IsAlive))
            {
                var coin = CustomItem.Get((uint)CustomItemsEnum.NormalCoin);
                var item = Item.Create(coin.Type);
                coin.Give(p, item);
                p.CurrentItem = item;
            }

            for (int i = 0; i < 3; i++)
            {
                var coin = CustomItem.Get((uint)CustomItemsEnum.NormalCoin);
                coin.Give(player);
            }
        }));


        Actions.Add(new CoinAction("ImmenseFortitude", (player, config, extraSettings) =>
        {
            player.MaxHealth += 150;
            player.Health += 150;
            player.ShowHint("The coin blesses you with great strength. You have gained an extremely potent pool of health and damage reduction.", 10);
            Map.Broadcast(10, $"{player.Nickname} flipped the <color=orange>Legendary</color> coin and Gained Immense Fortitude.");

            player.EnableEffect(EffectType.DamageReduction, 50, 0);

            player.SessionVariables["ImmenseFortitude173"] = true;

            void handler(HurtingEventArgs ev)
            {
                if (ev.Attacker != null && ev.Attacker.Role.Type == RoleTypeId.Scp173 && player.SessionVariables.ContainsKey("ImmenseFortitude173"))
                {
                    ev.Player.SessionVariables["ImmenseFortitude173"] = false;
                    ev.Amount = 50;
                }
                Exiled.Events.Handlers.Player.Hurting.Unsubscribe(handler);
            }

            Exiled.Events.Handlers.Player.Hurting.Subscribe(handler);
        }));

        Actions.Add(new CoinAction("TrueResurrection", (player, config, extraSettings) =>
        {
            player.ShowHint("The coin gives you a spare lease on life. If you are to die, you will come back to life… But only once…", 10);
            player.SessionVariables["TrueResurrection"] = true;
            // removed because players doesnt need to know
            Map.Broadcast(10, $"{player.Nickname} has flipped the <color=orange>Legendary</color> coin and was given a second Life.");
            Vector3 deathPosition = player.Position;
            void dying(DyingEventArgs ev)
            {
                if (ev.Player.SessionVariables.ContainsKey("TrueResurrection") && (bool)ev.Player.SessionVariables["TrueResurrection"])
                {
                    deathPosition = player.Position;
                    Exiled.Events.Handlers.Player.Dying.Unsubscribe(dying);
                }
            }

            Exiled.Events.Handlers.Player.Dying.Subscribe(dying);

            void died(DiedEventArgs ev)
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
            }
            Exiled.Events.Handlers.Player.Died.Subscribe(died);
        }));

        Actions.Add(new CoinAction("Necromancy", (player, config, extraSettings) =>
        {
            Map.Broadcast(5, $"{player.Nickname} has flipped the <color=orange>Legendary</color> Coin and respawned everyone as their minions");
            player.ShowHint("The coin's power grants you an army of minions loyal to you!", 5);

            foreach (var p in Player.List.Where(p => !p.IsAlive))
            {
                Log.Info(p);
                p.Role.Set(player.Role);

                p.Health = 50;
                p.MaxHealth = 50;

                p.Scale = new Vector3(0.5f, 0.5f, 0.5f);

                p.Position = player.Position;
                Jailbird jailbird = new();
                jailbird.ChargeDamage /= 2;
                jailbird.MeleeDamage /= 2;
                jailbird.TotalCharges = -100_000;
                jailbird.Give(p);
                p.CurrentItem = jailbird;

                void handler(DroppingItemEventArgs ev)
                {
                    if (ev.Item != jailbird)
                        return;
                    ev.IsAllowed = false;
                }

                Exiled.Events.Handlers.Player.DroppingItem.Subscribe(handler);

                void dying(DyingEventArgs ev)
                {
                    if (ev.Player != p)
                        return;

                    p.MaxHealth = 100;

                    p.Scale = Vector3.one;

                    Timing.CallDelayed(1, () =>
                    {
                        jailbird.Break();
                        Exiled.Events.Handlers.Player.Dying.Unsubscribe(dying);
                        Exiled.Events.Handlers.Player.DroppingItem.Unsubscribe(handler);
                    });
                }

                Exiled.Events.Handlers.Player.Dying.Subscribe(dying);
            }
        }));

        Actions.Add(new CoinAction("DrugCocktail", (player, config, extraSettings) =>
        {
            Map.Broadcast(5, $"{player.Nickname} has flipped the <color=orange>Legendary</color> and Has been given a Cocktail of effects");
            player.ShowHint("The coin blesses you with euphoria in the form of every drug known to mankind.", 5);
            player.EnableEffect(EffectType.Invigorated, 1, 0);
            player.EnableEffect(EffectType.BodyshotReduction, 5, 0);
            player.EnableEffect(EffectType.DamageReduction, 5, 0);
            player.ChangeEffectIntensity(EffectType.MovementBoost, 10, 0);
            player.EnableEffect(EffectType.RainbowTaste, 1, 0);
            player.EnableEffect(EffectType.Vitality, 1, 0);
            player.EnableEffect(EffectType.SilentWalk, 1, 0);
        }));

        Actions.Add(new CoinAction("CursedDrugCocktail", (player, config, extraSettings) =>
        {
            Map.Broadcast(5, $"{player.Nickname} has flipped the <color=orange>Legendary</color> and Has been given a Cursed Cocktail of effects");
            player.ShowHint("The coin cursed you with euphoria in the form of every drug known to mankind.", 5);
            player.EnableEffect(EffectType.AmnesiaVision, 1, 180);
            player.EnableEffect(EffectType.Burned, 1, 180);
            player.EnableEffect(EffectType.Concussed, 1, 180);
            player.EnableEffect(EffectType.Stained, 1, 180);
            player.EnableEffect(EffectType.InsufficientLighting, 1, 180);
            player.EnableEffect(EffectType.Slowness, 2, 180);
            player.EnableEffect(EffectType.SinkHole, 1, 180);
        }));

        Actions.Add(new CoinAction("ExperimentalItem", (player, config, extraSettings) =>
        {
            Map.Broadcast(5, $"{player.Nickname} has flipped the <color=orange>Legendary</color> Coin and Has Gained a Unique Item");

            int randomNumber = RNGManager.RNG.Next(1, 4);
            string itemName = string.Empty;

            switch (randomNumber)
            {
                case 1:
                    var Nerfgun = CustomItem.Get((uint)CustomItemsEnum.NerfGun);
                    Nerfgun.Give(player);
                    break;
                case 2:
                    var Swappergun = CustomItem.Get((uint)CustomItemsEnum.SwapperGun);
                    Swappergun.Give(player);
                    break;
                case 3:
                    var NoGogglesVest = CustomItem.Get((uint)CustomItemsEnum.NoGogglesArmor);
                    NoGogglesVest.Give(player);
                    break;
                case 4:
                    var BrokenLamps = CustomItem.Get((uint)CustomItemsEnum.BrokenLamp);
                    BrokenLamps.Give(player);
                    break;
                default:
                    break;
            }

            player.ShowHint("The coin blesses you with a special item!", 5);

        }));

        Actions.Add(new CoinAction("Combustion", (player, config, extraSettings) =>
        {
            foreach (var p in Player.List.Where(p => p.IsAlive))
            {
                p.ShowHint("The coin blesses you with a grenade!", 5);

                var grenade = new ExplosiveGrenade(ItemType.GrenadeHE);
                grenade.SpawnActive(p.Position);
                
            }
            Map.Broadcast(5, $"{player.Nickname} has flipped the <color=orange>Legendary</color> Coin and has Exploded Everyone");
        }));

        Actions.Add(new CoinAction("HighAwareness", (player, config, extraSettings) =>
        {
            Map.Broadcast(5, $"{player.Nickname} has flipped the <color=orange>Legendary</color> Coin and Became Omnipresent");

            player.EnableEffect(EffectType.SoundtrackMute, 1, 0);
            player.EnableEffect(EffectType.Scp1344, 1, 0);

            //Start a coroutine so that it repeats every 60 seconds
            //Timing.WaitForSeconds(60);

            var coroutineHandle = Timing.RunCoroutine(_DisplayEnemyLocations());

            void OnPlayerDied(DiedEventArgs ev)
            {
                if (ev.Player == player)
                {
                    Timing.KillCoroutines(coroutineHandle);
                    Exiled.Events.Handlers.Player.Died -= OnPlayerDied;
                }
            }

            Exiled.Events.Handlers.Player.Died += OnPlayerDied;

            IEnumerator<float> _DisplayEnemyLocations()
            {
                while (true)
                {
                    yield return Timing.WaitForSeconds(60);

                    // remove after thing
                    if (!player.IsAlive)
                        yield break;
                    // TODO: Optimize this
                    var list = Player.List.Where(p=> p != player);
                    int scpCount = list.Count(p => p.IsScp);
                    int ciCount = list.Count(p => p.Role.Team == Team.ChaosInsurgency);
                    int dclassCount = list.Count(p => p.Role.Team == Team.ClassD);
                    int scientistCount = list.Count(p => p.Role.Team == Team.Scientists);
                    int foundationCount = list.Count(p => p.Role.Team == Team.FoundationForces);


                    int scpLCZ = list.Count(p => p.IsScp && p.CurrentRoom.Zone == ZoneType.LightContainment);
                    int scpHCZ = list.Count(p => p.IsScp && p.CurrentRoom.Zone == ZoneType.HeavyContainment);
                    int scpEZ = list.Count(p => p.IsScp && p.CurrentRoom.Zone == ZoneType.Entrance);
                    int scpSurface = list.Count(p => p.IsScp && p.CurrentRoom.Zone == ZoneType.Surface);

                    int ciLCZ = list.Count(p => p.Role.Team == Team.ChaosInsurgency && p.CurrentRoom.Zone == ZoneType.LightContainment);
                    int ciHCZ = list.Count(p => p.Role.Team == Team.ChaosInsurgency && p.CurrentRoom.Zone == ZoneType.HeavyContainment);
                    int ciEZ = list.Count(p => p.Role.Team == Team.ChaosInsurgency && p.CurrentRoom.Zone == ZoneType.Entrance);
                    int ciSurface = list.Count(p => p.Role.Team == Team.ChaosInsurgency && p.CurrentRoom.Zone == ZoneType.Surface);

                    int dclassLCZ = list.Count(p => p.Role.Team == Team.ClassD && p.CurrentRoom.Zone == ZoneType.LightContainment);
                    int dclassHCZ = list.Count(p => p.Role.Team == Team.ClassD && p.CurrentRoom.Zone == ZoneType.HeavyContainment);
                    int dclassEZ = list.Count(p => p.Role.Team == Team.ClassD && p.CurrentRoom.Zone == ZoneType.Entrance);
                    int dclassSurface = list.Count(p => p.Role.Team == Team.ClassD && p.CurrentRoom.Zone == ZoneType.Surface);

                    int scientistLCZ = list.Count(p => p.Role.Team == Team.Scientists && p.CurrentRoom.Zone == ZoneType.LightContainment);
                    int scientistHCZ = list.Count(p => p.Role.Team == Team.Scientists && p.CurrentRoom.Zone == ZoneType.HeavyContainment);
                    int scientistEZ = list.Count(p => p.Role.Team == Team.Scientists && p.CurrentRoom.Zone == ZoneType.Entrance);
                    int scientistSurface = list.Count(p => p.Role.Team == Team.Scientists && p.CurrentRoom.Zone == ZoneType.Surface);

                    int mtfLCZ = list.Count(p => p.Role.Team == Team.FoundationForces && p.CurrentRoom.Zone == ZoneType.LightContainment);
                    int mtfHCZ = list.Count(p => p.Role.Team == Team.FoundationForces && p.CurrentRoom.Zone == ZoneType.HeavyContainment);
                    int mtfEZ = list.Count(p => p.Role.Team == Team.FoundationForces && p.CurrentRoom.Zone == ZoneType.Entrance);
                    int mtfSurface = list.Count(p => p.Role.Team == Team.FoundationForces && p.CurrentRoom.Zone == ZoneType.Surface);

                    string hintMessage = "You can sense:\n";

                    if (scpCount > 0)
                    {
                        hintMessage += $"{scpCount} SCP’s: ";
                        if (scpLCZ > 0) hintMessage += $"{scpLCZ} in LCZ, ";
                        if (scpHCZ > 0) hintMessage += $"{scpHCZ} in HCZ, ";
                        if (scpEZ > 0) hintMessage += $"{scpEZ} in EZ, ";
                        if (scpSurface > 0) hintMessage += $"{scpSurface} on Surface";
                        hintMessage = hintMessage.TrimEnd(',', ' ') + "\n";
                    }

                    if (ciCount > 0)
                    {
                        hintMessage += $"{ciCount} CI: ";
                        if (ciLCZ > 0) hintMessage += $"{ciLCZ} in LCZ, ";
                        if (ciHCZ > 0) hintMessage += $"{ciHCZ} in HCZ, ";
                        if (ciEZ > 0) hintMessage += $"{ciEZ} in EZ, ";
                        if (ciSurface > 0) hintMessage += $"{ciSurface} on Surface";
                        hintMessage = hintMessage.TrimEnd(',', ' ') + "\n";
                    }

                    if (dclassCount > 0)
                    {
                        hintMessage += $"{dclassCount} D-Class: ";
                        if (dclassLCZ > 0) hintMessage += $"{dclassLCZ} in LCZ, ";
                        if (dclassHCZ > 0) hintMessage += $"{dclassHCZ} in HCZ, ";
                        if (dclassEZ > 0) hintMessage += $"{dclassEZ} in EZ, ";
                        if (dclassSurface > 0) hintMessage += $"{dclassSurface} on Surface";
                        hintMessage = hintMessage.TrimEnd(',', ' ') + "\n";
                    }

                    if (scientistCount > 0)
                    {
                        hintMessage += $"{scientistCount} Scientists: ";
                        if (scientistLCZ > 0) hintMessage += $"{scientistLCZ} in LCZ, ";
                        if (scientistHCZ > 0) hintMessage += $"{scientistHCZ} in HCZ, ";
                        if (scientistEZ > 0) hintMessage += $"{scientistEZ} in EZ, ";
                        if (scientistSurface > 0) hintMessage += $"{scientistSurface} on Surface";
                        hintMessage = hintMessage.TrimEnd(',', ' ') + "\n";
                    }

                    if (foundationCount > 0)
                    {
                        hintMessage += $"{foundationCount} Foundation: ";
                        if (mtfLCZ > 0) hintMessage += $"{mtfLCZ} in LCZ, ";
                        if (mtfHCZ > 0) hintMessage += $"{mtfHCZ} in HCZ, ";
                        if (mtfEZ > 0) hintMessage += $"{mtfEZ} in EZ, ";
                        if (mtfSurface > 0) hintMessage += $"{mtfSurface} on Surface";
                        hintMessage = hintMessage.TrimEnd(',', ' ') + "\n";
                    }

                    player.ShowHint(hintMessage, 10);

                }
            }
        }));

        Actions.Add(new CoinAction("RocketMan", (player, config, extraSettings) =>
        {
            player.ShowHint("The coins unstable power unleashes calamity on you", 5);

            for(int i = 0; i < 3; i++) // replace 3 with variable later
            {
                var grenade = new ExplosiveGrenade(ItemType.GrenadeHE);
                {
                    float randomNumber = RNGManager.RNG.Next(3, 5);
                    grenade.FuseTime = randomNumber; 
                } 
                grenade.SpawnActive(player.Position);
            }
            for (int i = 0; i < 2; i++) // replace 2 with variable later
            {
                FlashGrenade flash = (FlashGrenade)Item.Create(ItemType.GrenadeFlash);
                
                flash.SpawnActive(player.Position);
                {
                    float randomNumber = RNGManager.RNG.Next(3, 5);
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

        Actions.Add(new CoinAction("BlessingsAbound", (player, config, extraSettings) =>
        {
            if (extraSettings.IsEmpty())
            {
                player.ShowHint("Please report this error To a developer", 5);
                return;
            }
            Map.Broadcast(5, $"{player.Nickname} has flipped a <color=orange>Legendary</color> Coin and Blessed Everyone");
            player.ShowHint("The coin blesses everyone with a positive effect", 5);

            foreach (var p in Player.List.Where(p => p.IsAlive))
            {
                EffectConfig effect = ObjectConvertManager.ParseToEffectConfig(extraSettings.RandomItem(), new());

                p.EnableEffect(effect.EffectType, effect.Intensity, 180, true);
                p.ShowHint($"You were given {effect.EffectType} for 2 minutes thanks to the powers of the coin!", 5);
            }
        }));


    }
}