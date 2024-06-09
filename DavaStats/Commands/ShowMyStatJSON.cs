using CommandSystem;
using Newtonsoft.Json;
using RemoteAdmin;
using System;

namespace DavaStats.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class ShowMyStatJSON : ICommand
    {
        public string Command => "ShowMyStatJSON";

        public string[] Aliases => new string[] { "showstatjson" };

        public string Description => "Show My Stats as in json form";

        public bool SanitizeResponse => true;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var pcs = sender as PlayerCommandSender;
            var stat = Main.Instance.Statistic.GetStatForPlayer(pcs.SenderId);
            response = JsonConvert.SerializeObject(stat, new JsonSerializerSettings()
            { 
                Converters = 
                {
                    new Newtonsoft.Json.Converters.StringEnumConverter()
                }
            });
            return true;
        }
    }
}
