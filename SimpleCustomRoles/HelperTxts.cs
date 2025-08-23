using InventorySystem.Items.Usables.Scp330;
using LabApi.Loader;
using MapGeneration;
using PlayerRoles;
using PlayerRoles.PlayableScps.Scp3114;
using PlayerRoles.PlayableScps.Scp939;
using PlayerStatsSystem;
using SimpleCustomRoles.RoleYaml.Enums;

namespace SimpleCustomRoles;

internal class HelperTxts
{
    static string Dir => Path.Combine(Path.Combine(Main.Instance.GetConfigDirectory().FullName, "Roles"), "HelpTXT");
    static readonly Dictionary<string, string> TxtPair = new()
    {
        { "DamageType", string.Join("\r\n", Enum.GetNames(typeof(DamageType))) },
        { "DamageSubType", string.Join("\r\n", Enum.GetNames(typeof(DamageSubType))) },
        { "UniversalDamageSubType", string.Join("\r\n", Enum.GetNames(typeof(DamageUniversalType))) },
        { "ItemType", string.Join("\r\n", Enum.GetNames(typeof(ItemType))) },
        { "Scp096DamageType", string.Join("\r\n", Enum.GetNames(typeof(Scp096DamageHandler.AttackType))) },
        { "Scp049DamageType", string.Join("\r\n", Enum.GetNames(typeof(Scp049DamageHandler.AttackType))) },
        { "MicroHidFiringMode", string.Join("\r\n", Enum.GetNames(typeof(InventorySystem.Items.MicroHID.Modules.MicroHidFiringMode))) },
        { "ExplosionType", string.Join("\r\n", Enum.GetNames(typeof(ExplosionType))) },
        { "DisruptorFiringState", string.Join("\r\n", Enum.GetNames(typeof(InventorySystem.Items.Firearms.Modules.DisruptorActionModule.FiringState))) },
        { "Scp939DamageType", string.Join("\r\n", Enum.GetNames(typeof(Scp939DamageType))) },
        { "Scp3114DamageType", string.Join("\r\n", Enum.GetNames(typeof(Scp3114DamageHandler.HandlerType))) },
        { "MathOption", string.Join("\r\n", Enum.GetNames(typeof(MathOption))) },
        { "CustomRoleType", string.Join("\r\n", Enum.GetNames(typeof(CustomRoleType))) },
        { "RoleTypeId", string.Join("\r\n", Enum.GetNames(typeof(RoleTypeId))) },
        { "Team", string.Join("\r\n", Enum.GetNames(typeof(Team))) },
        { "Faction", string.Join("\r\n", Enum.GetNames(typeof(Faction))) },
        { "LocationSpawnPriority", string.Join("\r\n", Enum.GetNames(typeof(LocationSpawnPriority))) },
        { "FacilityZone", string.Join("\r\n", Enum.GetNames(typeof(FacilityZone))) },
        { "RoomName", string.Join("\r\n", Enum.GetNames(typeof(RoomName))) },
        { "EscapeScenarioType", string.Join("\r\n", Enum.GetNames(typeof(Escape.EscapeScenarioType))) },
        { "CandyKindID", string.Join("\r\n", Enum.GetNames(typeof(CandyKindID))) },
        //{ "", "" },
    };
    public static void WriteAll()
    {
        if (!Directory.Exists(Dir))
            Directory.CreateDirectory(Dir);
        foreach (var item in TxtPair)
        {
            var txt = Path.Combine(Dir, item.Key);
            if (!File.Exists(txt + ".txt"))
            {
                File.WriteAllText(txt + ".txt", item.Value);
            }
        }
    }

    public static string TheYML_PRE_Comment = "# Welcome to small in-file documentation for the SimpleCustomRoles.\r\n#\r\n# REQUIRED: Required under normal condition\r\n# NOTDEAD: NOT required if the roleType is AfterDead\r\n# WAVE: Required if roleType is InWave\r\n# TEAMREPLACE: Required if replaceFromTeam is Dead OR roleToReplace is None!\r\n#\r\n# ARRAYS:\r\n# something: [] is an array, to set a values to it use like:\r\n#\r\n# something:\r\n#   - MyValue\r\n#\r\n# ONLY \"effects\" has a class inside and not a single value, set more as copy - paste it\r\n#\r\n# DICTONARY:\r\n# ammos: {} is an dictionary, to set key,value to it use like:\r\n#\r\n# ammos:\r\n#   MyKey: 40\r\n#\r\n#\r\n# RoleTypeIds: None | means it gonna skip it.\r\n# Team: Dead | means it gonna skip it. (Sorry!)\r\n#\r\n#";
}
