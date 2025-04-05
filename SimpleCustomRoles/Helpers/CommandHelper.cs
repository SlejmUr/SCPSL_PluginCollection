using LabApi.Features.Wrappers;

namespace SimpleCustomRoles.Helpers;

public static class CommandHelper
{

    public static void RunCommand(string commandName, string args)
    {
        if (!string.IsNullOrEmpty(commandName))
        {
            // Call event
            Server.RunCommand($"{commandName} {args}");
        }
    }
}
