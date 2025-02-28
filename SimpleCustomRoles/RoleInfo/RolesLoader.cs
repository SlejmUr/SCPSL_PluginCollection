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
        RoleInfos = [];
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
        Main.Instance.RolesLoader.RoleInfos = [.. Main.Instance.RolesLoader.RoleInfos.OrderBy(x=>x.SpawnChance)];
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
        return new()
        {
            RoleName = "Temp",
            RoleDisplayColorHex = "#ffffff",
            RoleType = CustomRoleType.Regular,
            SpawnAmount = 1,
            SpawnChance = 0,
            RoleToSpawnAs = RoleTypeId.Scientist,
            RoleToReplace = RoleTypeId.ClassD,
            Inventory = new()
            {
                InventoryItems =
                [
                     ItemType.KeycardScientist, ItemType.Medkit, ItemType.Adrenaline, ItemType.Coin
                ],
                Ammos = new Dictionary<AmmoType, ushort>()
                {
                    { AmmoType.Nato762, 3  }
                },
                DeniedUsingItems =
                [
                    ItemType.Coin
                ],
                CannotDropItems =
                [
                    ItemType.Coin
                ],
                CustomItemIds = []
            },
            Effects =
            [
                 new()
                 {
                     EffectType = EffectType.DamageReduction,
                     Duration = 100,
                     Intensity = 3
                 },
                 new()
                 {
                     CanRemovedWithSCP500 = false,
                     EffectType = EffectType.MovementBoost,
                     Duration = 433,
                     Intensity = 12
                 }
            ],
            Location = new Location()
            {
                UseDefault = false,
                LocationSpawnPriority = LocationSpawnPriority.SpawnZone,
                SpawnRooms =
                [
                    RoomType.EzCafeteria, RoomType.EzVent, RoomType.HczArmory
                ],
                SpawnZones =
                [
                    ZoneType.Entrance
                ],
                ExludeSpawnRooms = [RoomType.EzCollapsedTunnel]
            },
            Advanced = new()
            {
                RoleAppearance = RoleTypeId.ClassD,
                Damager = new()
                {
                    DamageReceivedDict = new Dictionary<DamageType, ValueSetter>()
                    {
                        {
                            DamageType.Revolver, new()
                            {
                                Value = 120,
                                SetType = MathOption.Set
                            }
                        },
                        {
                            DamageType.Jailbird, new()
                            {
                                Value = 1200,
                                SetType = MathOption.Add
                            }
                        }
                    },
                    DamageSentDict = new Dictionary<DamageType, ValueSetter>()
                    {
                        {
                            DamageType.ParticleDisruptor, new()
                            {
                                Value = 120,
                                SetType = MathOption.Multiply
                            }
                        },
                        {
                            DamageType.Com15, new()
                            {
                                Value = 1200,
                                SetType = MathOption.Add
                            }
                        }
                    },
                },
                DeadBy = new()
                {
                    IsConfigurated = false,
                    KillerRole = RoleTypeId.None,
                    KillerTeam = Team.OtherAlive,
                    RoleAfterKilled = RoleTypeId.None,
                    RoleNameRandom = [],
                    RoleNameToRespawnAs = "yeeet"
                },
                OpenDoorsNextToSpawn = false,
                BypassModeEnabled = false,
                Candy = new()
                {
                    CandiesToGive =
                    [
                        InventorySystem.Items.Usables.Scp330.CandyKindID.Pink
                    ],
                    GlobalCanDropCandy = true,
                    CanTakeCandy = true,
                    GlobalCanEatCandy = true,
                    MaxTakeCandy = 2000,
                    SpecialCandy = new Dictionary<InventorySystem.Items.Usables.Scp330.CandyKindID, CandyStuff.CandySpecific>()
                    {
                        {
                            InventorySystem.Items.Usables.Scp330.CandyKindID.Pink, new()
                            {
                                CanDropCandy = true,
                                CanEatCandy = false
                            }
                        },
                        {
                            InventorySystem.Items.Usables.Scp330.CandyKindID.Red, new()
                            {
                                CanDropCandy = false,
                                CanEatCandy = true
                            }
                        }
                    },

                },
                CanTrigger096 = true,
                Scale = new()
                {
                    X = 1,
                    Y = 1,
                    Z = 1
                },
                FriendlyFire =
                [
                    new()
                    { 
                        RoleType = RoleTypeId.ClassD,
                        Value = 40
                    }
                ],
                Escaping = new()
                {
                    CanEscape = false,
                    RoleAfterEscape = new()
                    {
                        { EscapeScenario.Scientist, RoleTypeId.NtfSpecialist  },
                        { EscapeScenario.CuffedScientist, RoleTypeId.ChaosConscript  }
                    },
                    RoleNameAfterEscape = new()
                    {
                        { EscapeScenario.Scientist, "test"  },
                        { EscapeScenario.CuffedScientist, "test2"  }
                    }
                }
            },
            Hint = new()
            {
                SpawnBroadcast = "You spawned as TEMP",
                SpawnBroadcastDuration = 10,
                SpawnHint = "Do your stuff!",
                SpawnHintDuration = 10,
            },
            Scp_Specific = new()
            {
                Scp049 = new()
                {
                    CanRecall = true,
                    RoleAfterKilled = RoleTypeId.None,
                    RoleNameRandom = ["random1", "random2"],
                    RoleNameToRespawnAs = "always_role"
                },
                Scp0492 = new()
                {
                    CanConsumeCorpse = true,
                    CanSpawnIfNoCustom094 = false,
                    ChanceForSpawn = 0
                },
                Scp096 = new()
                {
                    DoorToNotPryOn =
                    [
                        DoorType.Scp096,
                    ],
                    Enraging = new()
                    {
                        SetType = MathOption.Add,
                        Value = 100
                    },
                    CanCharge = false,
                    CanPry = true,
                    CanTryingNotToCry = true,
                },
                Scp173 = new()
                { 
                    CanPlaceTantrum = true,
                    CanUseBreakneckSpeed = true,
                },
                Scp079 = new()
                { 
                    ChangingCameraCost = new()
                    { 
                        SetType = MathOption.Set,
                        AuxiliaryPowerCost = 0,
                    },
                    GainingXP =
                    [
                        new()
                        { 
                            IsAllowed = true,
                            SetType = MathOption.Set,
                            XPAmount = 32,
                            HudTranslation = PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.PingLocation,
                            RoleType = RoleTypeId.None
                        }
                    ]
                }
            },
            DisplayRoleName = "TEMPORARY",
            SpawnWaveSpecific = new() 
            { 
                MinimumTeamMemberRequired = 3,
                SkipMinimumCheck = true,
                Faction = Faction.FoundationEnemy
            },
            Health = new()
            { 
                Health = new()
                { 
                    SetType = MathOption.Add,
                    Value = 30
                },
                Ahp = new()
                {
                    SetType = MathOption.Set,
                    Value = 10
                },
                HumeShield = new()
                {
                    SetType = MathOption.Multiply,
                    Value = 2
                },
            },
            EventCaller = new()
            { 
                
            },
            ReplaceFromTeam = Team.ClassD,
        };

    }
}
