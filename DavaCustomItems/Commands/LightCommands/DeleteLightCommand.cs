using CommandSystem;
using DavaCustomItems.Managers;

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
        var lid = arguments.At(0);
        if (lid.ToLower().Contains("all"))
        {
            var lights = LightManager.GetLightIds();
            foreach (var item in lights)
            {
                LightManager.RemoveLight(item);
            }
            response = "All Light removed!";
            return true;
        }
        if (!int.TryParse(lid, out int l_id))
        {
            response = "Cannot parse to int!";
            return false;
        }
        LightManager.RemoveLight(l_id);
        response = "Light removed!";
        return true;
    }
}
