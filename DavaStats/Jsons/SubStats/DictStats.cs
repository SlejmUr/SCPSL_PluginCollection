using CustomPlayerEffects;
using Exiled.API.Enums;
using System.Collections.Generic;

namespace DavaStats.Jsons.SubStats
{
    public class DictStats
    {
        public Dictionary<StatusEffectBase.EffectClassification, ulong> TimesOfEffect { get; set; }
        public Dictionary<ProjectileType, ulong> ProjectileThrown { get; set; }
        public Dictionary<ItemType, ulong> ItemUsed { get; set; }
        public Dictionary<ItemType, ulong> ItemAdded { get; set; }
        public Dictionary<ItemType, ulong> ItemRemoved { get; set; }
        public Dictionary<DamageType, ulong> DeathBy { get; set; }
        public Dictionary<EscapeScenario, ulong> EscapedAs { get; set; }
    }
}
