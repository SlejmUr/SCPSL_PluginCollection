using CommandSystem;
using DavaCustomItems.Coins;
using Exiled.API.Features;

namespace DavaCustomItems.Commands.CoinCommands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class RunCoinActionsCommand : ICommand, IUsageProvider
{
    public string Command => "dci_runcoinaction";

    public string[] Aliases => ["dci_rca", "dci_cf"];

    public string Description => "Run a coin actions";

    public string[] Usage => ["ActionName", "PlayerId", "CoinRarity"];

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (!sender.CheckPermission(PlayerPermissions.PlayersManagement))
        {
            response = "Missing permission! (PlayerManagement)";
            return false;
        }
        if (arguments.Count != 3)
        {
            response = "Wrong argument count! Use with <ActionName> <PlayerId> <CoinRarity>";
            return false;
        }

        string actionName = arguments.At(0);
        var coinAction = CoinAction.Actions.FirstOrDefault(x => x.ActionName == actionName);
        if (coinAction == default)
        {
            response = "No Action found with this name!";
            return false;
        }

        string playerid = arguments.At(1);
        Player player = null;
        if (int.TryParse(playerid, out int playerid_int))
        {
            player = Player.Get(playerid_int);
        }
        else
        {
            player = Player.Get(playerid);
        }
        if (player == null)
        {
            response = "User not found!";
            return false;
        }

        string CoinRarity = arguments.At(2);
        if (!Enum.TryParse(CoinRarity, true, out CoinRarityType rarityType))
        {
            response = "Coin rarity cannot be parsed!";
            return false;
        }

        var config = Main.Instance.Config.CoinRarityConfigs[rarityType].ExtraConfig;
        coinAction.RunAction(player, config, coinAction.ActionName);
        response = "Coin Action has been run!";
        return true;
    }
}