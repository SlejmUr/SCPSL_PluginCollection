using CommandSystem;
using LabApi.Features.Wrappers;
using SimpleCustomRoles.Helpers;
using SimpleCustomRoles.RoleInfo;
using Utils;

namespace SimpleCustomRoles.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class SetOthersCustomRole : ICommand
{
    public string Command => "setoscr";

    public string[] Aliases => ["setotherssimplecustomrole", "scr_seto"];

    public string Description => "Set others custom role with a given roleName\n Usage: setoscr RoleName PlayerId.\nTo set others back use '.' as a name";

    public bool SanitizeResponse => true;

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
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

        if (args.Count == 1)
        {
            response = "You forgot to add PlayerId!";
            return false;
        }
        string name = args[0];

        var list = RAUtils.ProcessPlayerIdOrNamesList(arguments, 1, out _, false);
        bool allplayerSuccess = false;
        string rsp = string.Empty;
        foreach (var item in list)
        {
            allplayerSuccess = SetIdToRole(name, item.PlayerId, out rsp);
            if (!allplayerSuccess)
            {
                break;
            }
        }
        if (!allplayerSuccess)
        {
            response = "Some player couldnt be set as a role! Error:" + rsp;
            return false;
        }
        else
        {
            response = "All PlayerIds set to role!";
            return true;
        }
    }

    public bool SetIdToRole(string rolename, int playerId, out string response)
    {
        var player = Player.List.Where(x => x.PlayerId == playerId).FirstOrDefault();
        if (player == null)
        {
            response = "Must be a Player!";
            return false;
        }
        // Set roles back.
        //CL.Info(rolename + " " + (rolename[0] == '.') + " " + (rolename == "."));
        if (rolename == ".")
        {
            CustomRoleHelpers.UnSetCustomInfoToPlayer(player);
            response = $"You unset {player.PlayerId}!";
            return true;
        }
        else
        {
            var role = RolesLoader.RoleInfos.FirstOrDefault(x => x.Rolename == rolename);
            if (role == default)
            {
                response = $"Role with name {rolename} not exist!";
                return false;
            }
            CustomRoleHelpers.SetFromCMD(player, role);
            response = $"You set {player.PlayerId} as a role: {rolename}[{role.Display.RARoleName}]!";
            return true;
        }

    }
}
