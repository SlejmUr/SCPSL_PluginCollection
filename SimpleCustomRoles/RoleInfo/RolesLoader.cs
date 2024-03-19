using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using Exiled.API.Features;

namespace SimpleCustomRoles.RoleInfo
{
    public class RolesLoader
    {
        public List<CustomRoleInfo> RoleInfos;
        internal string Dir = Path.Combine(Paths.Configs, "SimpleCustomRoles");

        public void Load()
        {
            RoleInfos = new List<CustomRoleInfo>();
            if (Directory.Exists(Dir))
            {
                File.WriteAllText(Dir + "/Template.yml", Serialize(CreateTemplate()));
                foreach (var file in Directory.GetFiles(Dir))
                {
                    if (file.Contains(".yml"))
                    {
                        Log.Info(file + " Loaded as custom role!");
                        RoleInfos.Add(Deserialize(File.ReadAllText(file)));
                    }
                }
            }
            else
            {
                Directory.CreateDirectory(Dir);
                File.WriteAllText(Dir + "/Template.yml", Serialize(CreateTemplate()));
            }
        }

        public void Dispose()
        {
            RoleInfos.Clear();
        }

        public string Serialize(CustomRoleInfo customRole)
        {
            var serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
            var yaml = serializer.Serialize(customRole);
            return yaml;
        }


        public CustomRoleInfo Deserialize(string txt)
        {
            var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
            var customRoleInfo = deserializer.Deserialize<CustomRoleInfo>(txt);
            return customRoleInfo;
        }


        public CustomRoleInfo CreateTemplate()
        {
            return new CustomRoleInfo()
            { 
                RoleName = "Temp",
                SpawnAmount = 1,
                SpawnChance = 0,
                ReplaceInSpawnWave = false,
                RoleToSpawnAs = PlayerRoles.RoleTypeId.Scientist,
                RoleToReplace = PlayerRoles.RoleTypeId.ClassD,
                InventoryItems = new List<ItemType>()
                { 
                     ItemType.KeycardScientist, ItemType.Medkit, ItemType.Adrenaline, ItemType.Coin
                },
                Effects = new List<Effect>()
                { 
                     new Effect()
                     {
                         EffectType = Exiled.API.Enums.EffectType.DamageReduction,
                         Duration = 100,
                         Intensity = 3
                     }
                },
                Location = new Location()
                { 
                    UseDefault = false,
                    LocationSpawnPriority = LocationSpawnPriority.SpawnZone,
                    SpawnRooms = new List<Exiled.API.Enums.RoomType>()
                    {
                        Exiled.API.Enums.RoomType.EzCafeteria, Exiled.API.Enums.RoomType.EzVent, Exiled.API.Enums.RoomType.HczArmory
                    },
                    SpawnZones = new List<Exiled.API.Enums.ZoneType>()
                    { 
                        Exiled.API.Enums.ZoneType.Entrance
                    }
                },
                Advanced = new Advanced()
                {
                   RoleAppearance = PlayerRoles.RoleTypeId.ClassD
                },
                Hint = new HintStuff()
                { 
                    SpawnBroadcast = "You spawned as TEMP",
                    SpawnBroadcastDuration = 10,
                    SpawnHint = "Do your stuff!",
                    SpawnHintDuration = 10,
                },
                Ammos = new Dictionary<Exiled.API.Enums.AmmoType, ushort>()
                {
                    {  Exiled.API.Enums.AmmoType.Nato9, 30 }
                }
            };

        }
    }
}
