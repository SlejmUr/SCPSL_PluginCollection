using System.ComponentModel;

namespace SimpleCustomRoles.RoleYaml;

public class SpawnInfo
{
    [Description("Role spawning chance. 0 means NEVER, min 1, max 10000 [10 000] (so 0.01 = 1, 60 = 6000 [6 000])")]
    public int SpawnChance { get; set; } = 0;

    [Description("Role spawn ammount")]
    public int SpawnAmount { get; set; } = 0;

    [Description("Minimum player count this role should spawn (-1 means no minimum!)")]
    public int MinimumPlayers { get; set; } = -1;

    [Description("Maximum player count this role should spawn (-1 means no maximum!)")]
    public int MaximumPlayers { get; set; } = -1;

    [Description("Denying editing the Spawn Chance by any means.")]
    public bool DenyChance { get; set; } = false;
}
