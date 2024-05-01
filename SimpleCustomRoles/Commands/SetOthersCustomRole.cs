using CommandSystem;
using Exiled.API.Features;
using RemoteAdmin;
using SimpleCustomRoles.RoleInfo;
using System;
using System.Linq;

namespace SimpleCustomRoles.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class SetOthersCustomRole : ICommand
    {
        public string Command => "setoscr";

        public string[] Aliases => new string[] { "setsimplecustomrole" };

        public string Description => "Set others custom role with a given roleName";

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

                if (args.Count == 1)
                {
                    response = "You forgot to add PlayerId!";
                    return false;
                }
                var name = args[0];

                var playerId = args[1];
                if (!int.TryParse(playerId, out int player_int_id))
                {
                    response = "PlayerId must be int!";
                    return false;
                }

                var pcs = sender as PlayerCommandSender;
                var player = Player.List.Where(x => x.Id == player_int_id).FirstOrDefault();
                if (player == null)
                {
                    response = "Must be a Player!";
                    return false;
                }
                var role = Main.Instance.RolesLoader.RoleInfos.Where(x => x.RoleName == name).FirstOrDefault();
                if (role == null)
                {
                    response = $"Role with name {name} not exist!";
                    return false;
                }
                RoleSetter.SetFromCMD(player, role);
                response = $"You set {player.Id} as a role: {name}!";
                return true;

            }
            response = "Must be coming from Player!";
            return false;
        }
    }
}
