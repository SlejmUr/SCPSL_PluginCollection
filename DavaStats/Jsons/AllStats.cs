using DavaStats.Jsons.SCPStats;

namespace DavaStats.Jsons
{
    public class AllStats
    {
        public WarheadStat WarheadStat { get; set; } = new WarheadStat();
        public ServerStat ServerStat { get; set; } = new ServerStat();
        public PlayerStat PlayerStat { get; set; } = new PlayerStat();
        public ItemStats ItemStats { get; set; } = new ItemStats();
        public CoreSCPStat SCPStats { get; set; } = new CoreSCPStat();
    }
}
