using UnityEngine;

namespace SimpleCustomRoles.RoleYaml;

public class StatsInfo
{
    public MathValue Health { get; set; } = new();
    public MathValue MaxHealth { get; set; } = new();
    public MathValue Ahp { get; set; } = new();
    public MathValue MaxAhp { get; set; } = new();
    public MathValue HumeShield { get; set; } = new();
    public MathValue MaxHumeShield { get; set; } = new();
    public Vector3 Gravity { get; set; } = new Vector3(0f, -19.6f, 0f);
    public MathValue MaxStamina { get; set; } = new();
}
