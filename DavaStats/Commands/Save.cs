using CommandSystem;
using System;

namespace DavaStats.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class Save : ICommand
    {
        public string Command => "SaveStats";

        public string[] Aliases => new string[] { "savestats" };

        public string Description => "Save all stats that currently holding.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Main.Instance.Database.Save(Main.Instance.Statistic.AllStats);
            response = "OK!";
            return true;
        }
    }
}
