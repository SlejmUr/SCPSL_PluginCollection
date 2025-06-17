using CommandSystem;
using LabApi.Features.Wrappers;
using RemoteAdmin;
using SimpleCustomRoles.Helpers;

namespace SimpleCustomRoles.Commands;

[CommandHandler(typeof(ClientCommandHandler))]
[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class OptOutCustomRole : ICommand
{
    public string Command => "optoutscr";

    public string[] Aliases => ["optoutscr", "scr_quit"];

    public string Description => "Opting out from current Custom Role";

    public bool SanitizeResponse => true;

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (sender is not PlayerCommandSender pcs)
        {
            response = "Must be coming from Player!";
            return false;
        }
        var player = Player.List.Where(x => x.UserId == pcs.SenderId).FirstOrDefault();
        if (player == null)
        {
            response = "Must be coming from Player!";
            return false;
        }
        CustomRoleHelpers.UnSetCustomInfoToPlayer(player);
        response = "Sucessfully opted out from Custom Roles";
        return true;
    }
}
