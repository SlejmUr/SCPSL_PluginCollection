using CommandSystem;
using Exiled.API.Features;
using System;
using System.Linq;

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
        if (Main.Instance.PlayerCustomRole.Count == 0)
        {
            response += $"There is no Custom Roles\n";
        }
        else
        {
            foreach (var role in Main.Instance.PlayerCustomRole)
            {
                var player = Player.List.Where(x => x.UserId == role.Key).FirstOrDefault();
                if (player != null)
                    response += $"{role.Value.RoleName} [{role.Value.DisplayRoleName}]: [Id]{player.Id} [Name]{player.DisplayNickname}\n";
            }
        }

        return true;
    }
}
