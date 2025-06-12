using SimpleCustomRoles.RoleYaml;

namespace SimpleCustomRoles.Helpers;

public static class GroupHelper
{
    public static bool CanSpawn(string roleGroup, ref List<CustomRoleBaseInfo> customRoles)
    {
        if (string.IsNullOrEmpty(roleGroup))
        {
            return true;
        }
        var group = Main.Instance.RoleGroups.FirstOrDefault(x => x.Name == roleGroup);
        if (group == null)
            return true;
        if (group.MaxRole != -1)
        {
            int currentRoleCount = customRoles.Count(x => x.Rolegroup == group.Name);
            return currentRoleCount < group.MaxRole;
        }
        return true;
    }
}
