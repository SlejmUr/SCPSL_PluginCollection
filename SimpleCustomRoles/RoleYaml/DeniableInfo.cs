using InventorySystem.Items.Usables.Scp330;

namespace SimpleCustomRoles.RoleYaml;

public class DeniableInfo
{
    public Dictionary<CandyKindID, Deniable> Candies { get; set; } = [];
    public Dictionary<ItemType, Deniable> Items { get; set; } = [];
}
