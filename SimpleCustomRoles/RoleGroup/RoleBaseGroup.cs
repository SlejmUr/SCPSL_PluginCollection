namespace SimpleCustomRoles.RoleGroup;

public class RoleBaseGroup
{
    public string Name { get; set; }
    public int MaxRole { get; set; } = -1;
    public List<string> GroupsToDeny { get; set; } = [];
    public List<string> RolesToDeny { get; set; } = [];
}
