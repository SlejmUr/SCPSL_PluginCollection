using CommandSystem;
using Exiled.Permissions.Extensions;

namespace ItemSpawner.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class SpawnCommand : ICommand
{
    public string Command => "ItemSpawn";

    public string[] Aliases => ["GroundItem"];

    public string Description => "Spawning an item (ItemType, SpawmType, Settings, AdvancedSettings)";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (!sender.CheckPermission("itemspawn"))
        {
            response = "You dont have permission to edit setting for the event!";
            return false;
        }


        if (arguments.Count < 3)
        {
            response = "You forget something!";
            return false;
        }

        if (!Enum.TryParse(arguments.At(0), out ItemType item))
        {
            response = "ItemType Cannot be parsed!";
            return false;
        }
        if (!Enum.TryParse(arguments.At(1), out SpawnType spawnType))
        {
            response = "SpawnType Cannot be parsed!";
            return false;
        }
        return LootSpawner.Spawn(item, spawnType, arguments, out response);
    }
}
