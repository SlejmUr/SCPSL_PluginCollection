using CommandSystem;
using DavaCustomItems.Coins;

namespace DavaCustomItems.Commands.CoinCommands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class ListCoinActionsCommand : ICommand
{
    public string Command => "dci_listcoinactions";

    public string[] Aliases => ["dci_lca"];

    public string Description => "List all available coin actions";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        response = string.Join(", ", CoinAction.Actions.Select(x => x.ActionName));
        return true;
    }
}
