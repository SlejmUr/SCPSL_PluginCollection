using Exiled.API.Enums;
using Exiled.API.Features;
using InventorySystem.Items.Usables.Scp330;
using PlayerRoles;
using SimpleCustomRoles.RoleInfo;
using System;
using System.Collections.Generic;
using System.IO;

namespace SimpleCustomRoles;

internal class HelperTxts
{
    static readonly string Dir = Path.Combine(Path.Combine(Paths.Configs, "SimpleCustomRoles"), "HelpTXT");
    static readonly Dictionary<string, string> TxtPair = new()
    {
        { "EffectType", string.Join("\r\n", Enum.GetNames(typeof(EffectType))) },
        { "ItemType", string.Join("\r\n", Enum.GetNames(typeof(ItemType))) },
        { "LocationSpawnPrioritys", string.Join("\r\n", Enum.GetNames(typeof(LocationSpawnPriority))) },
        { "RoleTypeId", string.Join("\r\n", Enum.GetNames(typeof(RoleTypeId))) },
        { "RoomType", string.Join("\r\n", Enum.GetNames(typeof(RoomType))) },
        { "ZoneType", string.Join("\r\n", Enum.GetNames(typeof(ZoneType))) },
        { "AmmoType", string.Join("\r\n", Enum.GetNames(typeof(AmmoType))) },
        { "SpawnableFaction", string.Join("\r\n", Enum.GetNames(typeof(SpawnableFaction))) },
        { "Team", string.Join("\r\n", Enum.GetNames(typeof(Team))) },
        { "CandyKindID", string.Join("\r\n", Enum.GetNames(typeof(CandyKindID))) },
        { "DamageType", string.Join("\r\n", Enum.GetNames(typeof(DamageType))) },
        { "CustomRoleType", string.Join("\r\n", Enum.GetNames(typeof(CustomRoleType))) },
        { "EscapeScenario", string.Join("\r\n", Enum.GetNames(typeof(EscapeScenario))) },
        { "MathOption", string.Join("\r\n", Enum.GetNames(typeof(MathOption))) },
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
