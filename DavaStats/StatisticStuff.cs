using DavaStats.Jsons;
using DavaStats.Jsons.SubStats;
using System.Collections.Generic;


namespace DavaStats
{
    public class StatisticStuff
    {
        public Dictionary<string, AllStats> AllStats = new Dictionary<string, AllStats>();

        public AllStats GetStatForPlayer(string id)
        {
            if (AllStats.ContainsKey(id))
            {
                return AllStats[id];
            }
            var allstats = new AllStats()
            {
                WarheadStat = new WarheadStat()
                { 
                    StartingCount = 0,
                    StoppingCount = 0,
                    DeactivationCount = 0,
                    ActivationCount = 0,
                },
                ServerStat = new ServerStat()
                {
                    Respawned = 0,
                    RespawnedAsChaos = 0,
                    RoundsStarted = 0,
                    RespawnedAsNTF = 0,
                    RoundsAlive = 0,
                    RoundsFinished = 0
                },
                PlayerStat = new PlayerStat()
                {
                    CoinFlip = new CoinFlip()
                    { 
                        CoinflipCount = 0,
                        CoinflipHeads = 0,
                        CoinflipTails = 0
                    },
                    PockedDim = new PockedDim()
                    {
                        DiedCount = 0,
                        EnteredCount = 0,
                        EscapedCount = 0
                    },
                    Gen = new Gen()
                    { 
                        GenActivated = 0,
                        GenDeactivated = 0
                    },
                    Cuff = new Cuff()
                    {
                        Cuffed = 0,
                        Uncuffed = 0
                    },
                    Funny = new Funny()
                    { 
                        IntercomSpeaking = 0,
                        MadeNoise = 0,
                        TeslaTrigger = 0
                    },
                    DictStats = new DictStats()
                    {
                        DeathBy = new Dictionary<Exiled.API.Enums.DamageType, ulong>(),
                        EscapedAs = new Dictionary<Exiled.API.Enums.EscapeScenario, ulong>(),
                        ItemAdded = new Dictionary<ItemType, ulong>(),
                        ItemRemoved = new Dictionary<ItemType, ulong>(),
                        ItemUsed = new Dictionary<ItemType, ulong>(),
                        ProjectileThrown = new Dictionary<Exiled.API.Enums.ProjectileType, ulong>(),
                        TimesOfEffect = new Dictionary<CustomPlayerEffects.StatusEffectBase.EffectClassification, ulong>(),
                    },
                    Usual = new Usual()
                    {
                        DamageDone = 0,
                        DamageReceived = 0,
                        DiedTimes = 0,
                        EscapedTimes = 0
                    },
                },
            };
            AllStats.Add(id, allstats);
            return allstats;
        }

        public void AddStatForPlayer(string id, AllStats allStats)
        {
            AllStats[id] = allStats;
        }
    }
}
