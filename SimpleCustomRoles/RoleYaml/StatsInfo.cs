using UnityEngine;

namespace SimpleCustomRoles.RoleYaml;

public class StatsInfo
{
    public MathValueFloat Health { get; set; } = new();
    public MathValueFloat MaxHealth { get; set; } = new();
    public MathValueFloat Ahp { get; set; } = new();
    public MathValueFloat MaxAhp { get; set; } = new();
    public MathValueFloat HumeShield { get; set; } = new();
    public MathValueFloat MaxHumeShield { get; set; } = new();
    public Vector3 Gravity { get; set; } = new(0f, -19.6f, 0f);
    public MathValueFloat MaxStamina { get; set; } = new();
}
