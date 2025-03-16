using CommandSystem;
using DavaCustomItems.Managers;

namespace DavaCustomItems.Commands.LightCommands;

[CommandHandler(typeof(BaseLightCommand))]
public class GetLightPosCommand : ICommand, IUsageProvider
{
    public string Command => "lightpos";

    public string[] Aliases => ["lp"];

    public string Description => "Get custom Light Source position";

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
        var light = LightManager.GetLight(l_id);
        if (light == null)
        {
            response = "No light found!!";
            return false;
        }
        response = light.Position.ToString();
        return true;
    }
}
