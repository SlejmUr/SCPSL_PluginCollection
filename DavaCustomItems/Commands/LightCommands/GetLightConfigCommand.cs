using CommandSystem;
using DavaCustomItems.Managers;

namespace DavaCustomItems.Commands.LightCommands;

[CommandHandler(typeof(BaseLightCommand))]
public class GetLightConfigCommand : ICommand, IUsageProvider
{
    public string Command => "lightconfig";

    public string[] Aliases => ["lc"];

    public string Description => "Get custom Light Source Config";

    public string[] Usage => ["LightId"];

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (arguments.Count != 1)
        {
            response = "Are you missing an argument?";
            return false;
        }
        var lid = arguments.At(0);
        if (!int.TryParse(lid, out int l_id))
        {
            response = "Cannot parse to int!";
            return false;
        }
        response = LightManager.GetLightConfig(l_id).ToString();
        return true;
    }
}
