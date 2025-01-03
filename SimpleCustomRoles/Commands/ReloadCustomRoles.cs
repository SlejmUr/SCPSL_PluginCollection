﻿using CommandSystem;
using SimpleCustomRoles.Handler;
using System;

namespace SimpleCustomRoles.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class ReloadCustomRoles : ICommand
{
    public string Command => "reloadscr";

    public string[] Aliases => new string[] { "reloadsimplecustomrole" };

    public string Description => "Reload the Custom Role Names";

    public bool SanitizeResponse => true;

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (!sender.CheckPermission(PlayerPermissions.PlayersManagement))
        {
            response = "You dont have permission!";
            return false;
        }
        TheHandler.ReloadRoles();
        response = "Roles Reloaded!";
        return true;
    }
}
