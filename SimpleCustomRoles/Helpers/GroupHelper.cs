namespace SimpleCustomRoles.Helpers;

public static class GroupHelper
{
    public static bool CanSpawn(string roleGroup)
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
            var currentRoleCount = CustomRoleHelpers.GetCurrentCustomRoles().Count(x => x.Rolegroup == group.Name);
            if (currentRoleCount <= group.MaxRole)
            {
                return false;
            }
        }
        return true;
    }
}
