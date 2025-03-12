using CommandSystem;
using DavaCustomItems.Coins;
using DavaCustomItems.Managers;
using Exiled.API.Features;

namespace DavaCustomItems.Commands.CoinCommands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class RunCoinActionsCommand : ICommand, IUsageProvider
{
    public string Command => "dci_runcoinaction";

    public string[] Aliases => ["dci_rca", "dci_cf"];

    public string Description => "Run a coin actions";

    public string[] Usage => ["ActionName", "PlayerId", "CoinRarity", "IsTails"];

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (!sender.CheckPermission(PlayerPermissions.PlayersManagement))
        {
            response = "Missing permission! (PlayerManagement)";
            return false;
        }
        if (arguments.Count != 4)
        {
            response = "Wrong argument count! Use with <ActionName> <PlayerId> <CoinRarity> <IsTails>";
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
        bool isTails = bool.Parse(arguments.At(3));
        var config = Main.Instance.Config.CoinRarityConfigs[rarityType].ExtraConfig;
        var name_weight = config.NameAndWeight.Keys.First(x=>x.IsTails == isTails && x.ActionName == coinAction.ActionName);
        List<object> settings = [];

        // getting the extra settings var from it.
        if (name_weight.UseWeight)
        {
            if (config.ExtraSettingsAndWeight.TryGetValue(name_weight.ExtraSettingsParameter, out var dict2))
                settings = dict2.GetRandomWeight();
        }
        else
        {
            if (config.ExtraSettings.TryGetValue(name_weight.ExtraSettingsParameter, out var dict2))
                settings = dict2;
        }
        coinAction.RunAction(player, config, settings);
        response = "Coin Action has been run!";
        return true;
    }
}