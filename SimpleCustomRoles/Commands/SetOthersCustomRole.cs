using CommandSystem;
using Exiled.API.Features;
using RemoteAdmin;
using SimpleCustomRoles.RoleInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using Utils;

namespace SimpleCustomRoles.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class SetOthersCustomRole : ICommand
    {
        public string Command => "setoscr";

        public string[] Aliases => new string[] { "setotherssimplecustomrole" };

        public string Description => "Set others custom role with a given roleName";

        public bool SanitizeResponse => true;

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
                string name = args[0];

                var list = RAUtils.ProcessPlayerIdOrNamesList(arguments, 1, out var array, false);
                bool allplayerSuccess = false;
                string rsp = string.Empty;
                foreach (var item in list)
                {
                    allplayerSuccess = SetIdToRole(name, item.PlayerId, out rsp);
                    if (!allplayerSuccess)
                    {
                        break;
                    }
                }
                if (!allplayerSuccess)
                {
                    response = "Some player couldnt be set as a role! Error:" + rsp;
                    return false;
                }
                else
                {
                    response = "All PlayerIds set to role!";
                    return true;
                }
            }
            response = "Must be coming from Player!";
            return false;
        }

        public bool SetIdToRole(string rolename, int playerId, out string response)
        {
            var player = Player.List.Where(x => x.Id == playerId).FirstOrDefault();
            if (player == null)
            {
                response = "Must be a Player!";
                return false;
            }
            var role = Main.Instance.RolesLoader.RoleInfos.Where(x => x.RoleName == rolename).FirstOrDefault();
            if (role == null)
            {
                response = $"Role with name {rolename} not exist!";
                return false;
            }
            RoleSetter.SetFromCMD(player, role);
            response = $"You set {player.Id} as a role: {rolename}[{role.DisplayRoleName}]!";
            return true;
        }
    }
}
