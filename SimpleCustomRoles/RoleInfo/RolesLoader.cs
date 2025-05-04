using InventorySystem.Items.Usables.Scp330;
using LabApi.Loader;
using SimpleCustomRoles.RoleYaml;

namespace SimpleCustomRoles.RoleInfo;

public class RolesLoader
{
    public List<CustomRoleBaseInfo> RoleInfos;
    internal string Dir = Path.Combine(Main.Instance.GetConfigDirectory().FullName, "Roles");

    public void Load()
    {
        RoleInfos = [];
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
        Main.Instance.RolesLoader.RoleInfos = [.. Main.Instance.RolesLoader.RoleInfos.OrderBy(x=>x.Spawn.SpawnChance)];
    }

    public void Clear()
    {
        RoleInfos.Clear();
    }


    public CustomRoleBaseInfo CreateTemplate()
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
                        ItemType.GunFSP9,
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
            }
        };
    }
}
