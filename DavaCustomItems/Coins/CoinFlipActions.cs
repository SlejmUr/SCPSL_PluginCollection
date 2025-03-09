using DavaCustomItems.Configs;
using DavaCustomItems.Managers;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;

namespace DavaCustomItems.Coins;

internal class CoinFlipActions
{
    // Global RNG (Random Number Generator) so we dont need to instanciate every time.
    // System. prefix because we dont know if we add "using UnityEngine;" here.
    public static System.Random RNG = new();

    public static void RunActions(Player player, bool IsTails, CoinRarityType rarityType, CoinExtraConfig extraConfig)
    {
        
        switch (rarityType)
        {
            case CoinRarityType.SuperUnluckyCoin:  // NOT YET IMPLEMENTED
                SuperUnlucky_Flip(player, IsTails, extraConfig);
                break;
            case CoinRarityType.UnluckyCoin: // NOT YET IMPLEMENTED
                Unlucky_Flip(player, IsTails, extraConfig);
                break;
            case CoinRarityType.NormalCoin:
                Normal_Flip(player, IsTails, extraConfig);
                break;
            case CoinRarityType.RareCoin:
                Rare_Flip(player, IsTails, extraConfig);
                break;
            case CoinRarityType.LegendaryCoin:
                Legendary_Flip(player, IsTails, extraConfig);
                break;
            case CoinRarityType.TEST:
                TEST_Flip(player, IsTails, extraConfig);
                break;
            default:
                break;
        }
    }

    static void TEST_Flip(Player player, bool IsTails, CoinExtraConfig extraConfig) // NOT YET IMPLEMENTED
    {
        string ConfigName = extraConfig.NameAndWeight.GetRandomWeight(string.Empty);
        Log.Info($"ConfigName : {ConfigName}");
        var effect = CoinAction.Actions.FirstOrDefault(x => x.ActionName == ConfigName);
        if (effect.ActionName != ConfigName)
            return; // ?
        effect.RunAction(player, extraConfig, ConfigName);
    }

    static void SuperUnlucky_Flip(Player player, bool IsTails, CoinExtraConfig extraConfig) // NOT YET IMPLEMENTED
    {

    }

    static void Unlucky_Flip(Player player, bool IsTails, CoinExtraConfig extraConfig)// NOT YET IMPLEMENTED
    {
        
    }


    static void Normal_Flip(Player player, bool IsTails, CoinExtraConfig extraConfig)
    {
        int randomNumber = RNG.Next(1, 26); // Random number for X number of effects to occur

        if (IsTails) // Bad Effects
        {
            player.ShowHint($"Your coin landed on Tails!", 5);

            switch (randomNumber)
            {
                case 1:
                    player.Health -= 30; 
                    break;
                case 2:
                    player.Health = 1; 
                    break;
                case 3:
                    player.EnableEffect(EffectType.Bleeding, 10); 
                    break;
                case 4:
                    // Drop an active grenade at player
                    var grenade = new ExplosiveGrenade(ItemType.GrenadeHE);
                    grenade.FuseTime = 3f;
                    grenade.SpawnActive(player.Position);
                    break;
                case 5:
                    // Spawn SCP018 at player
                    var scp018 = new Scp018(ItemType.SCP018);
                    scp018.SpawnActive(player.Position); // wrong but ok fornow
                    break;
                default:
                    player.ShowHint("Nothing happened.", 5);
                    break;
            }
        }
        else
        {
            player.ShowHint($"Your coin landed on Heads!", 5);

            switch (randomNumber)
            {
                case 1:
                    player.Health += 10; // Give 10 extra health
                    break;
                case 2:
                    Server.ExecuteCommand("giveXP", player.Sender); // wrong but will fix
                    break;
                case 3:
                    player.EnableEffect(EffectType.Scp207, 10);
                    break;
                case 4:
                    player.MaxHealth += 20; 
                    break;
                case 5:
                    player.HumeShield += 50;
                    break;
                default:
                    player.ShowHint("Nothing happened.", 5);
                    break;
                /*
                6: Give Medical Kit
                7: TP to Escape
                8: Give Candy
                9: Give Random Item
                10: Gnome player
                11: Wide player
                12: Give Ghostlight
                13: Give Keycard
                14: G
                 */
            }
        }

        // Chance to break the coin
        // 10% chance to break - will make config variable. maybe diffrent by type? Legendary break less than normal
        // - Done
        CheckCoinBreak(player, extraConfig);
    }

    static void Rare_Flip(Player player, bool IsTails, CoinExtraConfig extraConfig)
    {

    }

    static void Legendary_Flip(Player player, bool IsTails, CoinExtraConfig extraConfig)
    {

    }

    static void CheckCoinBreak(Player player, CoinExtraConfig extraConfig)
    {
        if (RNG.NextDouble() < extraConfig.CoinBrakeChance)
        {
            player.RemoveItem(player.CurrentItem);
            player.ShowHint("Your coin broke!", 5);
        }
    }
}