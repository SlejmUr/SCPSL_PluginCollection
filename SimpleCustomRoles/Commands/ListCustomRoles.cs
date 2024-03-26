using CommandSystem;
using RemoteAdmin;
using System;

namespace SimpleCustomRoles.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class ListCustomRoles : ICommand
    {
        public string Command => "listscr";

        public string[] Aliases => new string[] { "listsimplecustomrole" };

        public string Description => "List the Custom Role Names";
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = "Roles: \n";
            foreach (var item in Main.Instance.RolesLoader.RoleInfos)
            {
                response += $"{item.RoleName}, ";
            }
            response = response.Remove(response.Length - 2);
            return true;
        }
    }
}
