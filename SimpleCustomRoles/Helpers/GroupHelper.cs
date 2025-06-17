using SimpleCustomRoles.RoleGroup;
using SimpleCustomRoles.RoleYaml;

namespace SimpleCustomRoles.Helpers;

public static class GroupHelper
{
    public static bool CanSpawn(string roleGroup, ref List<CustomRoleBaseInfo> customRoles)
    {
        var group = GetGroup(roleGroup);
        if (group == null)
            return true;
        if (group.MaxRole != -1)
        {
            int currentRoleCount = customRoles.Count(x => x.Rolegroup == roleGroup);
            return currentRoleCount < group.MaxRole;
        }
        return true;
    }

    public static void DenyCustomRoles(string roleGroup, ref List<CustomRoleBaseInfo> customRoles)
    {
        var role = GetGroup(roleGroup);
        if (role == null)
            return;
        if (customRoles.Count(x => x.Rolegroup == roleGroup) > 1)
        {
            customRoles.RemoveAll(x => role.GroupsToDeny.Contains(x.Rolegroup));
            customRoles.RemoveAll(x => role.RolesToDeny.Contains(x.Rolename));
        }
    }

    public static RoleBaseGroup GetGroup(string roleGroup)
    {
        if (string.IsNullOrEmpty(roleGroup))
            return null;
        return Main.Instance.RoleGroups.FirstOrDefault(x => x.Name == roleGroup);
    }
}
