using CommandSystem;
using Exiled.API.Features;
using RemoteAdmin;
using SimpleCustomRoles.Handler;
using System;
using System.Linq;

namespace SimpleCustomRoles.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class SetMyCustomRole : ICommand
    {
        public string Command => "setscr";

        public string[] Aliases => new string[] { "setsimplecustomrole" };

        public string Description => "Set your custom role with a given roleName";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (sender is PlayerCommandSender)
            {
                if (!sender.CheckPermission( PlayerPermissions.PlayersManagement ))
                {
                    response = "You dont have permission!";
                    return false;
                }
                var args = arguments.ToList();
                if (args.Count == 0)
                {
                    response = "You forgot to add RoleName!";
                    return false;
                }
                var name = args[0];

                var pcs = sender as PlayerCommandSender;
                var player = Player.List.Where(x => x.UserId == pcs.SenderId).FirstOrDefault();
                if (player == null)
                {
                    response = "Must be coming from Player!";
                    return false;
                }
                var role = Main.Instance.RolesLoader.RoleInfos.Where(x => x.RoleName == name).FirstOrDefault();
                if (role == null)
                {
                    response = $"Role with name {name} not exist!";
                    return false;
                }
                TheHandler.SetFromCMD(player, role);
                response = $"You set yourself as a role: {name}!";
                return true;

            }
            response = "Must be coming from Player!";
            return false;
        }
    }
}
