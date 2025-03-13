using CommandSystem;
using Exiled.API.Features;
using RemoteAdmin;

namespace DavaCustomItems.Commands.CoinCommands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class GetPosCommand : ICommand
{
    public string Command => "getpos";

    public string[] Aliases => ["getpos"];

    public string Description => "getpos";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (sender is not PlayerCommandSender pcs)
        {
            response = "not player";
            return false;
        }
        response = Player.Get(pcs.PlayerId).Position.ToString();
        return true;
    }
}
