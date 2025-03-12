using CommandSystem;
using DavaCustomItems.Managers;
using Exiled.API.Features;
using UnityEngine;

namespace DavaCustomItems.Commands.LightCommands;

[CommandHandler(typeof(BaseLightCommand))]
public class ListLightCommand : ICommand, IUsageProvider
{
    public string Command => "list";

    public string[] Aliases => ["l"];

    public string Description => "List all custom Light Source";

    public string[] Usage => [];

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        response = "Light Ids: " + string.Join(", ", LightManager.GetLightIds());
        return true;
    }
}
