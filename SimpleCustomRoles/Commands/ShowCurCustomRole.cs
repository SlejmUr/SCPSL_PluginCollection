using CommandSystem;
using SimpleCustomRoles.Helpers;
using SimpleCustomRoles.RoleInfo;

namespace SimpleCustomRoles.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class ShowCurCustomRole : ICommand
{
    public string Command => "scrcur";

    public string[] Aliases => ["simplecustomrolecurrently", "scr_current", "scr_match"];

    public string Description => "List currently what player is which role.";

    public bool SanitizeResponse => true;

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (!sender.CheckPermission(PlayerPermissions.PlayersManagement))
        {
            response = "You dont have permission!";
            return false;
        }
        response = "Currently Playing roles:\n";
        var players = CustomRoleHelpers.GetPlayers();
        if (players.Count() == 0)
        {
            response += $"There is no Custom Roles\n";
        }
        else
        {
            foreach (var player in players)
            {
                if (CustomRoleHelpers.TryGetCustomRole(player, out var role) && role != null)
                    response += $"{role.Rolename} [{role.DisplayRolename}]: [Id]{player.PlayerId} [Name]{player.Nickname}\n";
            }
        }

        return true;
    }
}
