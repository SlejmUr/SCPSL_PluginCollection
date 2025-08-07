using CommandSystem;

namespace SimpleCustomRoles.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class PauseCustomRole : ICommand
{
    public string Command => "pausescr";

    public string[] Aliases => ["pausecustomrole", "scr_pause"];

    public string Description => "Pause or resume custom roles";

    public bool SanitizeResponse => true;

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (arguments.Count == 1)
        {
            var arg0 = arguments.Array[1];

            if (arg0 == "off" || arg0 == "false" || arg0 == "0")
            {
                Main.Instance.Config.IsPaused = true;
                response = "Custom Roles spawn are now paused";
                return true;
            }
            if (arg0 == "on" || arg0 == "true" || arg0 == "1")
            {
                Main.Instance.Config.IsPaused = false;
                response = "Custom Roles spawn are now resumed";
                return true;
            }

        }
        else
        {
            Main.Instance.Config.IsPaused = !Main.Instance.Config.IsPaused;
            response = "Custom Roles spawn are now " + (Main.Instance.Config.IsPaused ? "paused" : "resumed");
            return true;
        }
        response = "Something off";
        return false;
    }
}
