using CommandSystem;

namespace DavaCustomItems.Commands.LightCommands;

[CommandHandler(typeof(BaseLightCommand))]
public class DeleteLightCommand : ICommand, IUsageProvider
{
    public string Command => "delete";

    public string[] Aliases => ["d"];

    public string Description => "Removing the Custom Light";

    public string[] Usage => ["LightId"];

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (arguments.Count != 1)
        {
            response = "Are you missing an argument?";
            return false;
        }
        response = "";
        return false;
    }
}
