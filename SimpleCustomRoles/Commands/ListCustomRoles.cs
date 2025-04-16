using CommandSystem;
using System;

namespace SimpleCustomRoles.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class ListCustomRoles : ICommand
{
    public string Command => "listscr";

    public string[] Aliases => ["listsimplecustomrole"];

    public string Description => "List the Custom Role Names";
    public bool SanitizeResponse => true;
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        response = "Roles: \n";
        foreach (var item in Main.Instance.RolesLoader.RoleInfos)
        {
            response += $"{item.RoleName} [{item.DisplayRoleName}]\n";
        }
        response = response.Remove(response.Length - 2);
        return true;
    }
}
