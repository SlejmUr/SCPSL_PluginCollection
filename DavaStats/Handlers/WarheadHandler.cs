using Exiled.Events.EventArgs.Warhead;

namespace DavaStats.Handlers
{
    public class WarheadHandler
    {
        public static void ChangingLeverStatus(ChangingLeverStatusEventArgs changingLeverStatusEventArgs)
        {
            if (changingLeverStatusEventArgs.Player.DoNotTrack)
                return;
            var id = changingLeverStatusEventArgs.Player.UserId;
            var state = changingLeverStatusEventArgs.CurrentState;
            var stat = Main.Instance.Statistic.GetStatForPlayer(id);
            if (state)
                stat.WarheadStat.ActivationCount++;
             else
                stat.WarheadStat.DeactivationCount++;
            Main.Instance.Statistic.AddStatForPlayer(id, stat);
        }
        public static void Starting(StartingEventArgs startingEventArgs)
        {
            if (startingEventArgs.IsAuto)
                return;
            if (startingEventArgs.Player.DoNotTrack)
                return;
            var id = startingEventArgs.Player.UserId;
            var stat = Main.Instance.Statistic.GetStatForPlayer(id);
            stat.WarheadStat.StartingCount++;
            Main.Instance.Statistic.AddStatForPlayer(id, stat);
        }
        public static void Stopping(StoppingEventArgs stoppingEventArgs)
        {
            if (stoppingEventArgs.Player.DoNotTrack)
                return;
            var id = stoppingEventArgs.Player.UserId;
            var stat = Main.Instance.Statistic.GetStatForPlayer(id);
            stat.WarheadStat.StoppingCount++;
            Main.Instance.Statistic.AddStatForPlayer(id, stat);
        }
    }
}
