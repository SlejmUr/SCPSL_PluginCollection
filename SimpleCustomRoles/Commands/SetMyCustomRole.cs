using CommandSystem;
using LabApi.Features.Wrappers;
using RemoteAdmin;
using SimpleCustomRoles.Helpers;
using SimpleCustomRoles.RoleInfo;

namespace SimpleCustomRoles.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class SetMyCustomRole : ICommand
{
    public string Command => "setscr";

    public string[] Aliases => ["setsimplecustomrole", "scr_set"];

    public string Description => "Set your custom role with a given roleName";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (sender is not PlayerCommandSender pcs)
        {
            response = "Must be coming from Player!";
            return false;
        }
        if (!sender.CheckPermission(PlayerPermissions.PlayersManagement))
        {
            response = "You dont have permission!";
            return false;
        }
        var args = arguments.ToList();
        if (args.Count == 0)
        {
            response = "You forgot to add RoleName!";
            return false;
        }
        var name = args[0];
        var player = Player.List.Where(x => x.UserId == pcs.SenderId).FirstOrDefault();
        if (player == null)
        {
            response = "Must be coming from Player!";
            return false;
        }
        var role = RolesLoader.RoleInfos.Where(x => x.Rolename == name).FirstOrDefault();
        if (role == null)
        {
            response = $"Role with name {name} not exist!";
            return false;
        }
        CustomRoleHelpers.SetFromCMD(player, role);
        response = $"You set yourself as a role: {name}!";
        return true;
    }
}
