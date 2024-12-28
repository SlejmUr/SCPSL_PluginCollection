using CommandSystem;
using Exiled.API.Features;
using RemoteAdmin;
using SimpleCustomRoles.RoleInfo;
using System;
using System.Linq;

namespace SimpleCustomRoles.Commands;

[CommandHandler(typeof(ClientCommandHandler))]
[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class OptOutCustomRole : ICommand
{
    public string Command => "optoutscr";

    public string[] Aliases => new string[] { "optoutscr" };

    public string Description => "Opting out from current Custom Role";

    public bool SanitizeResponse => true;

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (sender is PlayerCommandSender pcs)
        {
            var player = Player.List.Where(x => x.UserId == pcs.SenderId).FirstOrDefault();
            if (player == null)
            {
                response = "Must be coming from Player!";
                return false;
            }
            RoleSetter.UnSetCustomInfoToPlayer(player);
            response = "Sucessfully opted out from Custom Roles";
            return true;

        }
        response = "Must be coming from Player!";
        return false;
    }
}
