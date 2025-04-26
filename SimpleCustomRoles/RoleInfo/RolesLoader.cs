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
            File.WriteAllText(Path.Combine(Dir, "Template.yml.d"), HelperTxts.TheYML_PRE_Comment + "\n" + CustomYaml.Serializer.Serialize(CreateTemplate()));
            File.WriteAllText(Path.Combine(Dir, "Template.yml"), CustomYaml.Serializer.Serialize(CreateTemplate()));
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
        return new();
    }
}
