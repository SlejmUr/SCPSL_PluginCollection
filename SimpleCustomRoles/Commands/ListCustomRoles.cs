using CommandSystem;
using SimpleCustomRoles.RoleInfo;

namespace SimpleCustomRoles.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class ListCustomRoles : ICommand
{
    public string Command => "listscr";

    public string[] Aliases => ["listsimplecustomrole", "scr_list"];

    public string Description => "List the Custom Role Names";
    public bool SanitizeResponse => true;
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        response = "Roles:";
        foreach (var item in RolesLoader.RoleInfos)
        {
            response += $"\n{item.Rolename} [{item.Display.RARoleName}]";
        }
        return true;
    }
}
