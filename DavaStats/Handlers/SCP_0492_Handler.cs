using Exiled.Events.EventArgs.Scp0492;

namespace DavaStats.Handlers
{
    internal class SCP_0492_Handler
    {
        public static void ConsumedCorpse(ConsumedCorpseEventArgs args)
        {
            if (args.Player.DoNotTrack)
                return;
            var id = args.Player.UserId;
            var stat = Main.Instance.Statistic.GetStatForPlayer(id);
            stat.SCPStats.SCP_0492.CorseConsumed++;
            Main.Instance.Statistic.AddStatForPlayer(id, stat);
        }
    }
}
