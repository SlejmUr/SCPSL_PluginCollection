using InventorySystem.Items.Usables.Scp330;
using System.ComponentModel;

namespace SimpleCustomRoles.RoleYaml;

public class CandyInfo
{
    [Description("Candies to give to the player when spawned. Check CandyKindIDs.txt")]
    public List<CandyKindID> Candies { get; set; } = [];

    [Description("Can the user Take candies from the bowl.")]
    public bool CanTakeCandy { get; set; } = true;

    [Description("Max candies can taken from the bowl.")]
    public int MaxTakeCandy { get; set; } = 2;

    [Description("Player can eat any Candy")]
    public bool GlobalCanEatCandy { get; set; } = true;

    [Description("Player can drop any Candy")]
    public bool GlobalCanDropCandy { get; set; } = true;

    [Description("Show how many candies can the user get.")]
    public bool ShowCandyLeft { get; set; } = false;
}
