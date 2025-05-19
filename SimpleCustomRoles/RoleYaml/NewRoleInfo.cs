using PlayerRoles;

namespace SimpleCustomRoles.RoleYaml;

public class NewRoleInfo
{
    public RoleSpawnFlags Flags { get; set; } = RoleSpawnFlags.None;
    public RoleTypeId RoleType { get; set; } = RoleTypeId.None;
    public string Name { get; set; } = string.Empty;
    public List<string> Random { get; set; } = [];

    public override string ToString()
    {
        return $"Flags: {Flags} RoleType: {RoleType} Name: {Name}, Randoms: {Random.Count}";
    }
}
