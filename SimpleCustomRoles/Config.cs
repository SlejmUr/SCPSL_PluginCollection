using System.ComponentModel;

namespace SimpleCustomRoles;

public class Config
{
    public bool Debug { get; set; }
    public bool IsPaused { get; set; }
    public ushort SpectatorBroadcastTime { get; set; } = 7;
    public bool UsePlayerPercent { get; set; }
    public float SpawnRateMultiplier { get; set; } = 1f;

    [Description("For CustomItemsAPI use \"/lci give {0} {1}\" for exiled use \"/ci give {1} {0}\"")]
    public string CustomItemCommand { get; set; } = "/lci give {0} {1}";
    public bool CustomItemUseName { get; set; } = true;
}
