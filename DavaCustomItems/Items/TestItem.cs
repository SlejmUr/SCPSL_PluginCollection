using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;

namespace DavaCustomItems.Items;

[CustomItem(ItemType.KeycardO5)]
public class Test05Light : BaseLightItem
{
    public override uint Id { get; set; } = 898;
    public override string Name { get; set; } = "Test05Light";
    public override string Description { get; set; }
    public override float Weight { get; set; }
    public override SpawnProperties SpawnProperties { get; set; }

    public override void Init()
    {
        base.Init();
    }

}
