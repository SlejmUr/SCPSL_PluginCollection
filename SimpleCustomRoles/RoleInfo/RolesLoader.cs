using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using PlayerRoles;
using LabApi.Loader;
using MapGeneration;

namespace SimpleCustomRoles.RoleInfo;

public class RolesLoader
{
    public List<CustomRoleInfo> RoleInfos;
    internal string Dir = Path.Combine(Main.Instance.GetConfigDirectory().FullName, "Roles");

    public void Load()
    {
        RoleInfos = [];
        if (Directory.Exists(Dir))
        {
            File.WriteAllText(Path.Combine(Dir, "Template.yml.d"), HelperTxts.TheYML_PRE_Comment + "\n" + Serialize(CreateTemplate()));
            File.WriteAllText(Path.Combine(Dir, "Template.yml"), Serialize(CreateTemplate()));
            foreach (var file in Directory.GetFiles(Dir))
            {
                if (file.Contains(".disable") || file.Contains(".d"))
                    continue;
                if (file.Contains(".yml"))
                {
                    if (Main.Instance.Config.Debug)
                    {
                        CL.Debug(file);
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
        return serializer.Serialize(customRole);
    }


    public CustomRoleInfo Deserialize(string txt)
    {
        var deserializer = new DeserializerBuilder()
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .Build();
        return deserializer.Deserialize<CustomRoleInfo>(txt);
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
            Inventory = new Inventory()
            {
                InventoryItems =
                [
                     ItemType.KeycardScientist, ItemType.Medkit, ItemType.Adrenaline, ItemType.Coin
                ],
                Ammos = new()
                {
                    { ItemType.Ammo762x39, 3  }
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
                 new Effect()
                 {
                     EffectTypeName = "DamageReduction",
                     Duration = 100,
                     Intensity = 3
                 },
                 new Effect()
                 {
                     CanRemovedWithSCP500 = false,
                     EffectTypeName = "MovementBoost",
                     Duration = 433,
                     Intensity = 12
                 }
            ],
            Location = new()
            {
                UseDefault = false,
                LocationSpawnPriority = LocationSpawnPriority.SpawnZone,
                SpawnRooms = [RoomName.EzOfficeStoried, RoomName.EzEvacShelter, RoomName.HczArmory],
                SpawnZones = [FacilityZone.Entrance],
                ExludeSpawnRooms = [RoomName.Pocket]
            },
            Advanced = new()
            {
                RoleAppearance = RoleTypeId.ClassD,
                Damager = new()
                {
                    DamageReceivedDict =
                    {
                        { 
                            new Damager.DamageMaker()
                            {
                                DamageType = Helpers.DamageHelper.DamageType.Firearm,
                                DamageSubType = Helpers.DamageHelper.SubType.AmmoType,
                                subType = ItemType.Ammo556x45
                            }, 
                            new()
                            { 
                                SetType = MathOption.Set,
                                Value = 0
                            }
                        },
                        {
                            new Damager.DamageMaker()
                            {
                                DamageType = Helpers.DamageHelper.DamageType.Explosion,
                                DamageSubType = Helpers.DamageHelper.SubType.ExplosionType,
                                subType = ExplosionType.PinkCandy
                            },
                            new()
                            {
                                SetType = MathOption.Set,
                                Value = 0
                            }
                        }
                    },
                    DamageSentDict =
                    {
                        {
                            new Damager.DamageMaker()
                            {
                                DamageType = Helpers.DamageHelper.DamageType.Snowball,
                                DamageSubType = Helpers.DamageHelper.SubType.None,
                                subType = null
                            },
                            new()
                            {
                                SetType = MathOption.Set,
                                Value = 0
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
                    RoleNameRandom = ["choserandomthis","chooserandom2"],
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
                    SpecialCandy = new()
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
                Scale = new()
                {
                    X = 1,
                    Y = 1,
                    Z = 1
                },
                FriendlyFire =
                [
                    new Advanced.FF()
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
                        { Escape.EscapeScenarioType.Scientist, RoleTypeId.NtfSpecialist  },
                        { Escape.EscapeScenarioType.CuffedScientist, RoleTypeId.ChaosConscript  }
                    },
                    RoleNameAfterEscape = new()
                    {
                        { Escape.EscapeScenarioType.Scientist, "test"  },
                        { Escape.EscapeScenarioType.CuffedScientist, "test2"  }
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
                        "Hcz096",
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
                        new SCP_Specific._079.GainXP()
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
            EventCaller = new(),
            ReplaceFromTeam = Team.ClassD,
        };

    }
}
