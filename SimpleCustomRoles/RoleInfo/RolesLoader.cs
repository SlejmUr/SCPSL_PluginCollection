using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using Exiled.API.Features;
using Exiled.API.Enums;
using PlayerRoles;
using System.Linq;

namespace SimpleCustomRoles.RoleInfo;

public class RolesLoader
{
    public List<CustomRoleInfo> RoleInfos;
    internal string Dir = Path.Combine(Paths.Configs, "SimpleCustomRoles");

    public void Load()
    {
        RoleInfos = new List<CustomRoleInfo>();
        if (Directory.Exists(Dir))
        {
            File.WriteAllText(Dir + "/Template.yml.d", HelperTxts.TheYML_PRE_Comment + "\n" + Serialize(CreateTemplate()));
            File.WriteAllText(Dir + "/Template.yml", Serialize(CreateTemplate()));
            foreach (var file in Directory.GetFiles(Dir))
            {
                if (file.Contains(".disable") || file.Contains(".d"))
                    continue;
                if (file.Contains(".yml"))
                {
                    if (Main.Instance.Config.Debug)
                    {
                        Log.Info(file);
                    }
                    RoleInfos.Add(Deserialize(File.ReadAllText(file)));
                }
            }
        }
        else
        {
            Directory.CreateDirectory(Dir);
            File.WriteAllText(Dir + "/Template.yml.d", Serialize(CreateTemplate()));
        }
        Main.Instance.RolesLoader.RoleInfos = Main.Instance.RolesLoader.RoleInfos.OrderBy(x=>x.SpawnChance).ToList();
    }

    public void Dispose()
    {
        RoleInfos.Clear();
    }

    public string Serialize(CustomRoleInfo customRole)
    {
        var serializer = new SerializerBuilder()
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .WithTypeInspector(inner => new CommentGatheringTypeInspector(inner))
        .WithEmissionPhaseObjectGraphVisitor(args => new CommentsObjectGraphVisitor(args.InnerVisitor))
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
            RoleDisplayColorHex = "#ffffff",
            RoleType = CustomRoleType.Regular,
            SpawnAmount = 1,
            SpawnChance = 0,
            RoleToSpawnAs = RoleTypeId.Scientist,
            RoleToReplace = RoleTypeId.ClassD,
            Inventory = new Inventory()
            {
                InventoryItems = new List<ItemType>()
                {
                     ItemType.KeycardScientist, ItemType.Medkit, ItemType.Adrenaline, ItemType.Coin
                },
                Ammos = new Dictionary<AmmoType, ushort>()
                {
                    { AmmoType.Nato762, 3  }
                },
                DeniedUsingItems = new List<ItemType>()
                {
                    ItemType.Coin
                },
                CannotDropItems = new List<ItemType>()
                {
                    ItemType.Coin
                },
                CustomItemIds = new List<uint>()
            },
            Effects = new List<Effect>()
            {
                 new Effect()
                 {
                     EffectType = EffectType.DamageReduction,
                     Duration = 100,
                     Intensity = 3
                 },
                 new Effect()
                 {
                     CanRemovedWithSCP500 = false,
                     EffectType = EffectType.MovementBoost,
                     Duration = 433,
                     Intensity = 12
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
                    DamageReceivedDict = new Dictionary<DamageType, ValueSetter>()
                    {
                        {
                            DamageType.Revolver, new ValueSetter()
                            {
                                Value = 120,
                                SetType = MathOption.Set
                            }
                        },
                        {
                            DamageType.Jailbird, new ValueSetter()
                            {
                                Value = 1200,
                                SetType = MathOption.Add
                            }
                        }
                    },
                    DamageSentDict = new Dictionary<DamageType, ValueSetter>()
                    {
                        {
                            DamageType.ParticleDisruptor, new ValueSetter()
                            {
                                Value = 120,
                                SetType = MathOption.Multiply
                            }
                        },
                        {
                            DamageType.Com15, new ValueSetter()
                            {
                                Value = 1200,
                                SetType = MathOption.Add
                            }
                        }
                    },
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
                BypassModeEnabled = false,
                CanChargeJailBird = true,
                Candy = new CandyStuff()
                {
                    CandiesToGive = new List<InventorySystem.Items.Usables.Scp330.CandyKindID>()
                    {
                        InventorySystem.Items.Usables.Scp330.CandyKindID.Pink
                    },
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
                CanTrigger096 = true,
                Scale = new V3()
                {
                    X = 1,
                    Y = 1,
                    Z = 1
                },
                FriendlyFire = new List<Advanced.FF>()
                {
                    new Advanced.FF()
                    { 
                        RoleType = RoleTypeId.ClassD,
                        Value = 40
                    }
                },
                Escaping = new Advanced.Escape()
                {
                    CanEscape = false,
                    RoleAfterEscape = new Dictionary<EscapeScenario, RoleTypeId>()
                    {
                        { EscapeScenario.Scientist, RoleTypeId.NtfSpecialist  },
                        { EscapeScenario.CuffedScientist, RoleTypeId.ChaosConscript  }
                    },
                    RoleNameAfterEscape = new Dictionary<EscapeScenario, string>()
                    {
                        { EscapeScenario.Scientist, "test"  },
                        { EscapeScenario.CuffedScientist, "test2"  }
                    }
                }
            },
            Hint = new HintStuff()
            {
                SpawnBroadcast = "You spawned as TEMP",
                SpawnBroadcastDuration = 10,
                SpawnHint = "Do your stuff!",
                SpawnHintDuration = 10,
            },
            Scp_Specific = new SCP_Specific()
            {
                Scp049 = new SCP_Specific._049()
                {
                    CanRecall = true,
                    RoleAfterKilled = RoleTypeId.None,
                    RoleNameRandom = new List<string>() { "random1", "random2" },
                    RoleNameToRespawnAs = "always_role"
                },
                Scp0492 = new SCP_Specific._0492()
                {
                    CanConsumeCorpse = true,
                    CanSpawnIfNoCustom094 = false,
                    ChanceForSpawn = 0
                },
                Scp096 = new SCP_Specific._096()
                {
                    DoorToNotPryOn = new List<DoorType>()
                    { 
                        DoorType.Scp096,
                    },
                    Enraging = new ValueSetter()
                    {
                        SetType = MathOption.Add,
                        Value = 100
                    },
                    CanCharge = false,
                    CanPry = true,
                    CanTryingNotToCry = true,
                },
                Scp173 = new SCP_Specific._173()
                { 
                    CanPlaceTantrum = true,
                    CanUseBreakneckSpeed = true,
                },
                Scp079 = new SCP_Specific._079()
                { 
                    ChangingCameraCost = new SCP_Specific._079.PowerCostSet()
                    { 
                        SetType = MathOption.Set,
                        AuxiliaryPowerCost = 0,
                    },
                    GainingXP = new List<SCP_Specific._079.GainXP>()
                    { 
                        new SCP_Specific._079.GainXP()
                        { 
                            IsAllowed = true,
                            SetType = MathOption.Set,
                            XPAmount = 32,
                            HudTranslation = PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.PingLocation,
                            RoleType = RoleTypeId.None
                        }
                    }
                }
            },
            DisplayRoleName = "TEMPORARY",
            SpawnWaveSpecific = new SpawnWaveSpecific() 
            { 
                MinimumTeamMemberRequired = 3,
                SkipMinimumCheck = true,
                Team = Respawning.SpawnableTeamType.ChaosInsurgency
            },
            Health = new HealthClass()
            { 
                Health = new ValueSetter()
                { 
                    SetType = MathOption.Add,
                    Value = 30
                },
                Ahp = new ValueSetter()
                {
                    SetType = MathOption.Set,
                    Value = 10
                },
                HumeShield = new ValueSetter()
                {
                    SetType = MathOption.Multiply,
                    Value = 2
                },
            },
            EventCaller = new EventCaller()
            { 
                
            },
            ReplaceFromTeam = Team.ClassD,
        };

    }
}
