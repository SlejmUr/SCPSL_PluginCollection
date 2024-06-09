using CommandSystem;
using Exiled.API.Features;
using InventorySystem.Items.Firearms.Attachments;
using Newtonsoft.Json;
using RemoteAdmin;
using System;
using System.Linq;

namespace DavaStats.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class ShowMyStat : ICommand
    {
        public string Command => "showmystat";

        public string[] Aliases => new string[] { "showstat" };

        public string Description => "Show My Stats";

        public bool SanitizeResponse => true;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var pcs = sender as PlayerCommandSender;
            var stat = Main.Instance.Statistic.GetStatForPlayer(pcs.SenderId);

            var list = arguments.ToList();
            if (list.Count != 0)
            {
                var statpage = list[0];
                if (!int.TryParse(statpage, out int res))
                {
                    goto RSP;
                }
                switch (res)
                {
                    case 1:
                        response = "\n-----------\n" +
                            "Pocket Dimension Stats:\n" +
                            "Entered / Escaped / Died: " + stat.PlayerStat.PockedDim.EnteredCount + " / " + stat.PlayerStat.PockedDim.EscapedCount + " / " + stat.PlayerStat.PockedDim.DiedCount + "\n" +
                            "Coin Flip Stats:\n" +
                            "Total / Heads / Tails: " + stat.PlayerStat.CoinFlip.CoinflipCount + " / " + stat.PlayerStat.CoinFlip.CoinflipHeads + " / " + stat.PlayerStat.CoinFlip.CoinflipTails + "\n" +
                            "Generator Stats:\n" +
                            "Activated / Deactivated: " + stat.PlayerStat.Gen.GenActivated + " / " + stat.PlayerStat.Gen.GenDeactivated + "\n" +
                            "Cuffing Stats:\n" +
                            "Cuffed / Uncuffed Times: " + stat.PlayerStat.Cuff.Cuffed + " / " + stat.PlayerStat.Cuff.Uncuffed + "\n" +
                            "Other Stats:\n" +
                            "Noise Made: " + stat.PlayerStat.Funny.MadeNoise + "\n" +
                            "Intercom Speaking Times: " + stat.PlayerStat.Funny.IntercomSpeaking + "\n" +
                            "Tesla Triggered: " + stat.PlayerStat.Funny.TeslaTrigger + "\n" +
                            "-----------\n" +
                            "(You are currently looking at Page 1)";
                        return true;
                    case 2:
                        {
                            response = "\n-----------\n" +
                                "Death By: (JSON FORM!)\n";
                            var dead = JsonConvert.SerializeObject(stat.PlayerStat.DictStats.DeathBy, new JsonSerializerSettings()
                            {
                                Converters =
                                {
                                    new Newtonsoft.Json.Converters.StringEnumConverter()
                                }
                            });
                            response += dead;
                            response +=
                                "\n-----------\n" +
                                "(You are currently looking at Page 2)";
                            return true;
                        }
                    case 3:
                        {
                            response = "\n-----------\n" +
                                "Escaped As: (JSON FORM!)\n";
                            var dead = JsonConvert.SerializeObject(stat.PlayerStat.DictStats.EscapedAs, new JsonSerializerSettings()
                            {
                                Converters =
                                {
                                    new Newtonsoft.Json.Converters.StringEnumConverter()
                                }
                            });
                            response += dead;
                            response +=
                                "\n-----------\n" +
                                "(You are currently looking at Page 3)";
                            return true;
                        }
                    case 4:
                        {
                            response = "\n-----------\n" +
                                "Projectile Thrown: (JSON FORM!)\n";
                            var dead = JsonConvert.SerializeObject(stat.PlayerStat.DictStats.ProjectileThrown, new JsonSerializerSettings()
                            {
                                Converters =
                                {
                                    new Newtonsoft.Json.Converters.StringEnumConverter()
                                }
                            });
                            response += dead;
                            response +=
                                "\n-----------\n" +
                                "(You are currently looking at Page 4)";
                            return true;
                        }
                    case 5:
                        {
                            response = "\n-----------\n" +
                                "Times Of Effects: (JSON FORM!)\n";
                            var dead = JsonConvert.SerializeObject(stat.PlayerStat.DictStats.TimesOfEffect, new JsonSerializerSettings()
                            {
                                Converters =
                                {
                                    new Newtonsoft.Json.Converters.StringEnumConverter()
                                }
                            });
                            response += dead;
                            response +=
                                "\n-----------\n" +
                                "(You are currently looking at Page 5)";
                            return true;
                        }
                    case 6:
                        {
                            response = "\n-----------\n" +
                                "Item Added: (JSON FORM!)\n";
                            var dead = JsonConvert.SerializeObject(stat.PlayerStat.DictStats.ItemAdded, new JsonSerializerSettings()
                            {
                                Converters =
                                {
                                    new Newtonsoft.Json.Converters.StringEnumConverter()
                                }
                            });
                            response += dead;
                            response +=
                                "\n-----------\n" +
                                "(You are currently looking at Page 6)";
                            return true;
                        }
                    case 7:
                        {
                            response = "\n-----------\n" +
                                "Item Removed: (JSON FORM!)\n";
                            var dead = JsonConvert.SerializeObject(stat.PlayerStat.DictStats.ItemRemoved, new JsonSerializerSettings()
                            {
                                Converters =
                                {
                                    new Newtonsoft.Json.Converters.StringEnumConverter()
                                }
                            });
                            response += dead;
                            response +=
                                "\n-----------\n" +
                                "(You are currently looking at Page 7)";
                            return true;
                        }
                    case 8:
                        {
                            response = "\n-----------\n" +
                                "Item Used: (JSON FORM!)\n";
                            var dead = JsonConvert.SerializeObject(stat.PlayerStat.DictStats.ItemUsed, new JsonSerializerSettings()
                            {
                                Converters =
                                {
                                    new Newtonsoft.Json.Converters.StringEnumConverter()
                                }
                            });
                            response += dead;
                            response +=
                                "\n-----------\n" +
                                "(You are currently looking at Page 8)";
                            return true;
                        }
                    default:
                        break;
                }
            }
            RSP:
            response =
    "\n-----------\n" +
    "Warhead Stats:\n" +
    "Started/Stopped: " + stat.WarheadStat.StartingCount + " / " + stat.WarheadStat.StoppingCount + "\n" +
    "Activation/Deactivation: " + stat.WarheadStat.ActivationCount + " / " + stat.WarheadStat.DeactivationCount + "\n" +
    "-----------\n" +
    "Round Stats:\n" +
    "Rounds Started/Finished: " + stat.ServerStat.RoundsStarted + " / " + stat.ServerStat.RoundsFinished + "\n" +
    "Rounds Alive/Respawned: " + stat.ServerStat.RoundsAlive + " / " + stat.ServerStat.Respawned + "\n" +
    "Respawned as NTF / Chaos: " + stat.ServerStat.RespawnedAsNTF + " / " + stat.ServerStat.RespawnedAsChaos + "\n" +
    "-----------\n" +
    "Usual Stats:\n" +
    "Damage Received/Dealt: " + stat.PlayerStat.Usual.DamageReceived + " / " + stat.PlayerStat.Usual.DamageDone + "\n" +
    "Died Times: " + stat.PlayerStat.Usual.DiedTimes + "\n" +
    "Escaped Times: " + stat.PlayerStat.Usual.EscapedTimes + "\n" +
    "-----------\n" +
    "For More Stats use .showstat 1 (max is 8)";
            return true;
        }
    }
}
