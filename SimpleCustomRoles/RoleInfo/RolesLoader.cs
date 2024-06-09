using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using Exiled.API.Features;
using Exiled.API.Enums;
using PlayerRoles;

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
                    if (file.Contains(".disable") || file.Contains(".d"))
                        continue;
                    if (file.Contains(".yml"))
                    {
                        RoleInfos.Add(Deserialize(File.ReadAllText(file)));
                    }
                }
            }
            else
            {
                Directory.CreateDirectory(Dir);
                File.WriteAllText(Dir + "/Template.yml.d", Serialize(CreateTemplate()));
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
                RoleToSpawnAs = RoleTypeId.Scientist,
                RoleToReplace = RoleTypeId.ClassD,
                InventoryItems = new List<ItemType>()
                {
                     ItemType.KeycardScientist, ItemType.Medkit, ItemType.Adrenaline, ItemType.Coin
                },
                Effects = new List<Effect>()
                {
                     new Effect()
                     {
                         EffectType = EffectType.DamageReduction,
                         Duration = 100,
                         Intensity = 3
                     }
                },
                Location = new Location()
                {
                    UseDefault = false,
                    LocationSpawnPriority = LocationSpawnPriority.SpawnZone,
                    SpawnRooms = new List<RoomType>()
                    {
                        RoomType.EzCafeteria, RoomType.EzVent, RoomType.HczArmory
                    },
                    SpawnZones = new List<ZoneType>()
                    {
                        ZoneType.Entrance
                    }
                },
                Advanced = new Advanced()
                {
                    RoleAppearance = RoleTypeId.ClassD,
                    Damager = new Damager()
                    {
                        DamageDict = new Dictionary<DamageType, Damager.SubDamager>()
                        {
                            {
                                DamageType.Revolver, new Damager.SubDamager()
                                {
                                    Damage = 120,
                                    IsSet = true,
                                    IsAddition = false
                                }
                            },
                            {
                                DamageType.Jailbird, new Damager.SubDamager()
                                {
                                    Damage = 1200,
                                    IsSet = false,
                                    IsAddition = true
                                }
                            }
                        }
                    },
                    DeadBy = new DeadBy()
                    {
                        IsConfigurated = false,
                        KillerRole = RoleTypeId.None,
                        KillerTeam = Team.OtherAlive,
                        RoleAfterKilled = RoleTypeId.None,
                        RoleNameRandom = new List<string>(),
                        RoleNameToRespawnAs = "yeeet"
                    },
                    OpenDoorsNextToSpawn = false,
                    BypassEnabled = false,
                    CanChargeJailBird = true,
                    Candy = new CandyStuff()
                    {
                        CandiesToGive = new List<InventorySystem.Items.Usables.Scp330.CandyKindID>(),
                        GlobalCanDropCandy = true,
                        CanTakeCandy = true,
                        GlobalCanEatCandy = true,
                        MaxTakeCandy = 2000,
                        SpecialCandy = new Dictionary<InventorySystem.Items.Usables.Scp330.CandyKindID, CandyStuff.CandySpecific>()
                        {
                            {
                                InventorySystem.Items.Usables.Scp330.CandyKindID.Pink, new CandyStuff.CandySpecific()
                                {
                                    CanDropCandy = true,
                                    CanEatCandy = false
                                }
                            },
                            {
                                InventorySystem.Items.Usables.Scp330.CandyKindID.Red, new CandyStuff.CandySpecific()
                                {
                                    CanDropCandy = false,
                                    CanEatCandy = true
                                }
                            }
                        },

                    },
                    CanEscape = false,
                    ColorHex = "#ffffff",
                    RoleAfterEscape = RoleTypeId.None,
                    Scale = new V3()
                    {
                        X = 1,
                        Y = 1,
                        Z = 1
                    }
                },
                Hint = new HintStuff()
                {
                    SpawnBroadcast = "You spawned as TEMP",
                    SpawnBroadcastDuration = 10,
                    SpawnHint = "Do your stuff!",
                    SpawnHintDuration = 10,
                },
                Ammos = new Dictionary<AmmoType, ushort>()
                {
                    { AmmoType.Nato762, 3  }
                },
                DeniedUsingItems = new List<ItemType>()
                {
                    ItemType.Coin
                },
                SCP_Specific = new SCP_Specific()
                {
                    SCP_Specific_Role = false,
                    SCP_049 = new SCP_Specific._049()
                    {
                        CanRecall = true,
                        RoleAfterKilled = RoleTypeId.None,
                        RoleNameRandom = new List<string>() { "random1", "random2" },
                        RoleNameToRespawnAs = "always_role"
                    },
                    SCP_0492 = new SCP_Specific._0492()
                    {
                        CanConsumeCorpse = true,
                        CanSpawnIfNoCustom094 = false,
                        ChanceForSpawn = 0
                    },
                    SCP_096 = new SCP_Specific._096()
                    {
                        CanTrigger096 = true
                    }
                },
                DisplayRoleName = "TEMPORARY",
                CannotDropItems = new List<ItemType>()
                { 
                    ItemType.Coin
                },
                SpawnWaveSpecific = new SpawnWaveSpecific() 
                { 
                    MinimumTeamMemberRequired = 3,
                    SkipMinimumCheck = true,
                    Team = Respawning.SpawnableTeamType.ChaosInsurgency
                },
                UsedAfterDeath = false,
                CustomItemIds = new List<uint>(),
                HealthModifiers = new HealthMod(),
                HealthReplacer = new HealthReplace(),
                ReplaceFromTeam = Team.ClassD,
            };

        }
    }
}
