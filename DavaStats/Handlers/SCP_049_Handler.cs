using Exiled.Events.EventArgs.Scp049;

namespace DavaStats.Handlers
{
    public class SCP_049_Handler
    {
        public static void SendingCall(SendingCallEventArgs args)
        {
            if (args.Player.DoNotTrack && args.IsAllowed)
                return;
            var id = args.Player.UserId;
            var stat = Main.Instance.Statistic.GetStatForPlayer(id);
            stat.SCPStats.SCP_049.CallsSent++;
            Main.Instance.Statistic.AddStatForPlayer(id, stat);
        }

        public static void ActivatingSense(ActivatingSenseEventArgs args)
        {
            if (args.Player.DoNotTrack && args.IsAllowed)
                return;
            var id = args.Player.UserId;
            var stat = Main.Instance.Statistic.GetStatForPlayer(id);
            stat.SCPStats.SCP_049.SenseActivated++;
            Main.Instance.Statistic.AddStatForPlayer(id, stat);
        }

        public static void FinishingRecall(FinishingRecallEventArgs args)
        {
            if (args.Player.DoNotTrack && args.IsAllowed)
                return;
            var id = args.Player.UserId;
            var stat = Main.Instance.Statistic.GetStatForPlayer(id);
            stat.SCPStats.SCP_049.Recalled++;
            Main.Instance.Statistic.AddStatForPlayer(id, stat);
        }

        public static void Attacking(AttackingEventArgs args)
        {
            if (args.Player.DoNotTrack && args.IsAllowed)
                return;
            var id = args.Player.UserId;
            var stat = Main.Instance.Statistic.GetStatForPlayer(id);
            stat.SCPStats.SCP_049.Attacking++;
            Main.Instance.Statistic.AddStatForPlayer(id, stat);
        }
    }
}
