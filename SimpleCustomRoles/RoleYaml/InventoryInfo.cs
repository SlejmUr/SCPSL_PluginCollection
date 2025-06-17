using System.ComponentModel;

namespace SimpleCustomRoles.RoleYaml;

public class InventoryInfo
{
    public bool Clear { get; set; } = true;
    public List<ItemType> Items { get; set; } = [];
    public Dictionary<ItemType, ushort> Ammos { get; set; } = [];
    public List<uint> CustomIds { get; set; } = [];
    public List<string> CustomNames { get; set; } = [];
    [Description("Disables RemoveEverythingExceedingLimits function.")]
    public bool Keep { get; set; } = false;
}
