using PlayerRoles;

namespace SimpleCustomRoles.RoleYaml;

public class WaveInfo
{
    public Faction Faction { get; set; } = Faction.Unclassified;
    public int MinRequired { get; set; } = 0;
    public bool SkipCheck { get; set; } = false;
    public bool RemoveAfterSpawn { get; set; } = false;
}
