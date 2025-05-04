namespace SimpleCustomRoles.Helpers;

public static class GroupHelper
{
    public static bool CanSpawn(string roleGroup)
    {
        if (string.IsNullOrEmpty(roleGroup))
        {
            return false;
        }
        var group = Main.Instance.RoleGroups.FirstOrDefault(x => x.Name == roleGroup);
        if (group == null)
            return false;
        if (group.MaxRole != -1)
        {
            var currentRoleCount = CustomRoleHelpers.GetCurrentCustomRoles().Count(x => x.Rolegroup == group.Name);
            if (group.MaxRole == currentRoleCount)
            {
                return true;
            }
        }
        return false;
    }
}
