using System.ComponentModel;

namespace SimpleCustomRoles.RoleYaml.SCP;

public class Scp0492Info
{
    public bool CanConsumeCorpse { get; set; } = true;
    public bool CanSpawnIfNoCustom094 { get; set; } = false;
    public int ChanceForSpawn { get; set; } = 0;

    public MathValueFloat ConsumeHealth { get; set; } = new();

    [Description("Able to eat corpses that are Already Consumed, while other zombie consuming and when on full health.")]
    public bool ForceEat { get; set; } = false;
}
