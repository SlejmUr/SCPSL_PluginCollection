using CommandSystem;

namespace DavaCustomItems.Commands.LightCommands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class BaseLightCommand : ParentCommand
{
    public override string Command => "lightmanager";

    public override string[] Aliases => ["lm", "clm", "customlightmanager"];

    public override string Description => "Managing custom Light Sources around facility";

    public override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        response = "Invalid subcommand. Available commands: create, delete, list.";
        return true;
    }

    public override void LoadGeneratedCommands()
    {
        this.RegisterCommand(new CreateLightCommand());
        this.RegisterCommand(new DeleteLightCommand());
        this.RegisterCommand(new ListLightCommand());
    }
}
