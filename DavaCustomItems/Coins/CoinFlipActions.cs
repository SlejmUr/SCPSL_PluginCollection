using Exiled.API.Features;

namespace DavaCustomItems.Coins;

internal class CoinFlipActions
{
    public static void RunActions(Player player, bool IsTails, CoinRarityType rarityType)
    {
        switch (rarityType)
        {
            case CoinRarityType.Common:
                Common_Flip(player, IsTails);
                break;
            case CoinRarityType.Regular:
                Regular_Flip(player, IsTails);
                break;
            case CoinRarityType.Rare:
                Rare_Flip(player, IsTails);
                break;
            case CoinRarityType.Legendary:
                Legendary_Flip(player, IsTails);
                break;
            case CoinRarityType.WorstOfThemAll:
                break;
            case CoinRarityType.OnlyNegative:
                break;
            default:
                break;
        }
    }

    static void Common_Flip(Player player, bool IsTails)
    {

    }

    static void Regular_Flip(Player player, bool IsTails)
    {

    }

    static void Rare_Flip(Player player, bool IsTails)
    {

    }

    static void Legendary_Flip(Player player, bool IsTails)
    {

    }
}