using InventorySystem.Items.Usables.Scp330;
using LabApi.Loader;
using SimpleCustomRoles.RoleYaml;

namespace SimpleCustomRoles.RoleInfo;

public static class RolesLoader
{
    public static List<CustomRoleBaseInfo> RoleInfos = [];
    internal static string Dir = Path.Combine(Main.Instance.GetConfigDirectory().FullName, "Roles");

    public static void Load()
    {
        RoleInfos.Clear();
        if (Directory.Exists(Dir))
        {
            File.WriteAllText(Path.Combine(Dir, "Template.yml.d"), CustomYaml.Serializer.Serialize(CreateTemplate()));
            foreach (var file in Directory.GetFiles(Dir))
            {
                if (file.Contains(".disable") || file.Contains(".d"))
                    continue;
                if (!file.Contains(".yml"))
                    continue;
                CL.Debug(file, Main.Instance.Config.Debug);
                RoleInfos.Add(CustomYaml.Deserializer.Deserialize<CustomRoleBaseInfo>(File.ReadAllText(file)));
            }

        }
        else
        {
            Directory.CreateDirectory(Dir);
            File.WriteAllText(Path.Combine(Dir, "Template.yml.d"), CustomYaml.Serializer.Serialize(CreateTemplate()));
        }
        RoleInfos = [.. RoleInfos.OrderBy(x => x.Spawn.SpawnChance)];
    }

    public static void Clear()
    {
        RoleInfos.Clear();
    }


    public static CustomRoleBaseInfo CreateTemplate()
    {
        return new()
        {
            Effects =
            [
                new()
                {
                    EffectName = "test",
                    Duration = 5,
                    Intensity = 55,
                    Removable = true,
                }
            ],
            Inventory = new()
            {
                Ammos = new()
                {
                    { ItemType.Ammo12gauge, 40 },
                    { ItemType.Ammo44cal, 40 },
                },
                Items =
                [
                    ItemType.ArmorCombat,
                    ItemType.GunE11SR
                ]
            },
            Candy = new()
            {
                Candies =
                [
                    CandyKindID.Red,
                    CandyKindID.Pink
                ]
            },
            Deniable = new()
            {
                Items = new()
                {
                    {
                        ItemType.GunE11SR,
                        new()
                        {
                            CanDrop = true,
                            CanUse = true,
                        }
                    },
                    {
                        ItemType.Medkit,
                        new()
                        {
                            CanDrop = true,
                            CanUse = false,
                        }
                    }
                },
                Candies = new()
                {
                    {
                        CandyKindID.Pink,
                        new()
                        {
                            CanDrop = true,
                            CanUse = true,
                        }
                    },
                }
            },
            Escape = new()
            {
                CanEscape = true,
                ConfigToRole = new()
                {
                    {
                        new()
                        {
                            EscapeRole = PlayerRoles.RoleTypeId.Scp173,
                            ShouldBeCuffer = true,
                        },
                        new()
                        {
                            RoleType =  PlayerRoles.RoleTypeId.ClassD
                        }
                    }
                }
            },
            KillerToNewRole = new()
            {
                {
                    new()
                    {
                        KillerRole = PlayerRoles.RoleTypeId.Scp106
                    },
                    new()
                    {
                        RoleType =  PlayerRoles.RoleTypeId.ClassD
                    }
                }
            },
            Damage = new()
            {
                DamageDealt = new()
                {
                    {
                        new()
                        {
                            DamageType = DamageType.Firearm,
                            DamageSubType = DamageSubType.WeaponType,
                            SubType = ItemType.GunRevolver
                        },
                        new()
                        {
                            Math = MathOption.Set,
                            Value = 5
                        }
                    }
                }
            }
        };
    }
}
