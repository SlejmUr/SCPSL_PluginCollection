using PlayerRoles;
using SimpleCustomRoles.RoleYaml;
using System.ComponentModel;

namespace SimpleCustomRoles;

public class Config
{
    public bool Debug { get; set; }
    public bool UseGlobalDir { get; set; }
    public bool IsPaused { get; set; }
    public ushort SpectatorBroadcastTime { get; set; } = 7;
    public bool UsePlayerPercent { get; set; }
    public float SpawnRateMultiplier { get; set; } = 1f;

    [Description("For CustomItemsAPI use \"/lci give {0} {1}\" for exiled use \"/ci give {0} {1}\"")]
    public string CustomItemCommand { get; set; } = "/lci give {0} {1}";
    public bool CustomItemUseName { get; set; } = true;

    public Dictionary<EscapeConfig, RoleTypeId> EscapeConfigs { get; set; } = new()
    {
        {
            new()
            {
                EscapeRole = RoleTypeId.ChaosConscript,
                ShouldBeCuffer = true,
            },
            RoleTypeId.NtfSpecialist
        },
        {
            new()
            {
                EscapeRole = RoleTypeId.ChaosMarauder,
                ShouldBeCuffer = true,
            },
            RoleTypeId.NtfSpecialist
        },
        {
            new()
            {
                EscapeRole = RoleTypeId.ChaosRepressor,
                ShouldBeCuffer = true,
            },
            RoleTypeId.NtfSpecialist
        },
        {
            new()
            {
                EscapeRole = RoleTypeId.ChaosRifleman,
                ShouldBeCuffer = true,
            },
            RoleTypeId.NtfSpecialist
        },
        {
            new()
            {
                EscapeRole = RoleTypeId.NtfCaptain,
                ShouldBeCuffer = true,
            },
            RoleTypeId.ChaosMarauder
        },
        {
            new()
            {
                EscapeRole = RoleTypeId.NtfPrivate,
                ShouldBeCuffer = true,
            },
            RoleTypeId.ChaosConscript
        },
        {
            new()
            {
                EscapeRole = RoleTypeId.NtfSergeant,
                ShouldBeCuffer = true,
            },
            RoleTypeId.ChaosConscript
        },
        {
            new()
            {
                EscapeRole = RoleTypeId.NtfSpecialist,
                ShouldBeCuffer = true,
            },
            RoleTypeId.ChaosConscript
        },
    };
}

