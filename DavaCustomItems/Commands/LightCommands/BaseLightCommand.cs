using CommandSystem.Commands.RemoteAdmin;
using CommandSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavaCustomItems.Commands.LightCommands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
internal class BaseLightCommand : ParentCommand
{
    public override string Command => "lightmanager";

    public override string[] Aliases => ["lm"];

    public override string Description => "Managing custom Light Sources around facility";

    public override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        response = "Invalid subcommand. Available commands: .";
        return true;
    }

    public override void LoadGeneratedCommands()
    {
        
    }
}
