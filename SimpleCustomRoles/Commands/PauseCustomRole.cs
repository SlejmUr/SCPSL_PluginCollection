using CommandSystem;
using System;

namespace SimpleCustomRoles.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class PauseCustomRole : ICommand
    {
        public string Command => "pausescr";

        public string[] Aliases => new string[] { "pausecustomrole" };

        public string Description => "Show your current Effects";

        public bool SanitizeResponse => true;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Main.Instance.Config.IsPaused = !Main.Instance.Config.IsPaused;
            response = "Custom Roles spawn are now " + (Main.Instance.Config.IsPaused ? "paused" : "resumed");
            return false;
        }
    }
}
