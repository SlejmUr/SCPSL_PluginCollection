using PlayerRoles;
using SimpleCustomRoles.RoleYaml.Enums;
using SimpleCustomRoles.RoleYaml.SCP;

namespace SimpleCustomRoles.RoleYaml;

public class CustomRoleBaseInfo
{
    public string Rolename { get; set; }
    public CustomRoleType RoleType { get; set; } = CustomRoleType.Regular;
    public RoleTypeId RoleToSpawn { get; set; } = RoleTypeId.None;
    public RoleTypeId ReplaceRole { get; set; } = RoleTypeId.None;
    public Team ReplaceTeam { get; set; } = Team.Dead;
    public string Rolegroup { get; set; } = string.Empty;
    public SpawnInfo Spawn { get; set; } = new();
    public DisplayInfo Display { get; set; } = new();
    public WaveInfo Wave { get; set; } = new();
    public StatsInfo Stats { get; set; } = new();
    public List<EffectInfo> Effects { get; set; } = [];
    public LocationInfo Location { get; set; } = new();
    public InventoryInfo Inventory { get; set; } = new();
    public HintInfo Hint { get; set; } = new();
    public CandyInfo Candy { get; set; } = new();
    public DeniableInfo Deniable { get; set; } = new();
    public FpcInfo Fpc { get; set; } = new();
    public DamageInfo Damage { get; set; } = new();
    public EscapeInfo Escape { get; set; } = new();
    public ScpInfo Scp { get; set; } = new();
    public Dictionary<KillerRoleInfo, NewRoleInfo> KillerToNewRole { get; set; } = [];
    public ExtraInfo Extra { get; set; } = new();
    public PocketInfo Pocket { get; set; } = new();
}
