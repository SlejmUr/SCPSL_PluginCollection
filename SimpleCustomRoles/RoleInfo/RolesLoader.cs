using PlayerRoles;
using LabApi.Loader;
using MapGeneration;
using LabApi.Loader.Features.Yaml;
using UnityEngine;

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
            File.WriteAllText(Path.Combine(Dir, "Template.yml.d"), HelperTxts.TheYML_PRE_Comment + "\n" + YamlConfigParser.Serializer.Serialize(CreateTemplate()));
            File.WriteAllText(Path.Combine(Dir, "Template.yml"), YamlConfigParser.Serializer.Serialize(CreateTemplate()));
            foreach (var file in Directory.GetFiles(Dir))
            {
                if (file.Contains(".disable") || file.Contains(".d"))
                    continue;
                if (!file.Contains(".yml"))
                    continue;
                CL.Debug(file, Main.Instance.Config.Debug);
                RoleInfos.Add(YamlConfigParser.Deserializer.Deserialize<CustomRoleInfo>(File.ReadAllText(file)));
            }

        }
        else
        {
            Directory.CreateDirectory(Dir);
            File.WriteAllText(Path.Combine(Dir, "Template.yml.d"), YamlConfigParser.Serializer.Serialize(CreateTemplate()));
        }
        Main.Instance.RolesLoader.RoleInfos = [.. Main.Instance.RolesLoader.RoleInfos.OrderBy(x=>x.SpawnChance)];
    }

    public void Clear()
    {
        RoleInfos.Clear();
    }


    public CustomRoleInfo CreateTemplate()
    {
        return new()
        {
            Rolename = "Temp",
            DisplayColor = "#ffffff",
            RoleType = CustomRoleType.Regular,
            SpawnAmount = 1,
            SpawnChance = 0,
            RoleToSpawn = RoleTypeId.Scientist,
            ReplaceRole = RoleTypeId.ClassD,
            Inventory = new Inventory()
            {
                Items =
                [
                     ItemType.KeycardScientist, ItemType.Medkit, ItemType.Adrenaline, ItemType.Coin
                ],
                Ammos = new()
                {
                    { ItemType.Ammo762x39, 3  }
                },
                CustomIds = []
            },
            Effects =
            [
                 new Effect()
                 {
                     EffectName = "DamageReduction",
                     Duration = 100,
                     Intensity = 3
                 },
                 new Effect()
                 {
                     Removable = false,
                     EffectName = "MovementBoost",
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
                DeniedUsingItems =
                [
                    ItemType.Coin
                ],
                CannotDropItems =
                [
                    ItemType.Coin
                ],
                Appearance = RoleTypeId.ClassD,
                Damager = new()
                {
                    DamageReceived =
                    {
                        { 
                            new Damager.DamageMaker()
                            {
                                DamageType = Helpers.DamageHelper.DamageType.Firearm,
                                DamageSubType = Helpers.DamageHelper.SubType.AmmoType,
                                SubType = ItemType.Ammo556x45
                            }, 
                            new()
                            { 
                                Math = MathOption.Set,
                                Value = 0
                            }
                        },
                        {
                            new Damager.DamageMaker()
                            {
                                DamageType = Helpers.DamageHelper.DamageType.Explosion,
                                DamageSubType = Helpers.DamageHelper.SubType.ExplosionType,
                                SubType = ExplosionType.PinkCandy
                            },
                            new()
                            {
                                Math = MathOption.Set,
                                Value = 0
                            }
                        }
                    },
                    DamageSent =
                    {
                        {
                            new Damager.DamageMaker()
                            {
                                DamageType = Helpers.DamageHelper.DamageType.Snowball,
                                DamageSubType = Helpers.DamageHelper.SubType.None,
                                SubType = null
                            },
                            new()
                            {
                                Math = MathOption.Set,
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
                    AfterDeath = RoleTypeId.None,
                    Random = ["choserandomthis","chooserandom2"],
                    RoleName = "yeeet"
                },
                OpenDoorsNextToSpawn = false,
                Bypass = false,
                Candy = new()
                {
                    Candies =
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
                Scale = Vector3.one,
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
            Hint = new HintClass()
            {
                Broadcast = "You spawned as TEMP",
                BroadcastDuration = 10,
                Hint = "Do your stuff!",
                HintDuration = 10,
            },
            ScpSpecific = new()
            {
                Scp049 = new()
                {
                    CanRecall = true,
                    AfterDeath = RoleTypeId.None,
                    Random = ["random1", "random2"],
                    RoleName = "always_role"
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
                        Math = MathOption.Add,
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
            DisplayRolename = "TEMPORARY",
            SpawnWave = new() 
            { 
                MinRequired = 3,
                SkipCheck = true,
                Faction = Faction.FoundationEnemy
            },
            Health = new()
            { 
                Health = new()
                { 
                    Math = MathOption.Add,
                    Value = 30
                },
                Ahp = new()
                {
                    Math = MathOption.Set,
                    Value = 10
                },
                HumeShield = new()
                {
                    Math = MathOption.Multiply,
                    Value = 2
                },
            },
            Events = new(),
            ReplaceTeam = Team.ClassD,
        };

    }
}
