using Exiled.API.Features;
using Exiled.Events.EventArgs.Server;
using Exiled.Events.Features;
using System.IO;
using System.Linq;

namespace DavaStats.Handlers
{
    public class ServerHandler
    {
        public static void RoundStarted()
        {
            foreach (var item in Player.List.Where(x => !x.DoNotTrack))
            {
                Log.Info(item.Id + " = " + item.UserId);
                var stat = Main.Instance.Statistic.GetStatForPlayer(item.UserId);
                stat.ServerStat.RoundsStarted++;
                Main.Instance.Statistic.AddStatForPlayer(item.UserId, stat);
            }
        }

        public static void WaitingForPlayers()
        {

        }

        public static void RespawningTeam(RespawningTeamEventArgs respawningTeamEventArgs)
        {
            foreach (var item in respawningTeamEventArgs.Players)
            {
                var stat = Main.Instance.Statistic.GetStatForPlayer(item.UserId);
                stat.ServerStat.Respawned++;
                // Cheap check if what role you spawn in
                if (respawningTeamEventArgs.SpawnQueue.Where(x=>
                    x == PlayerRoles.RoleTypeId.NtfPrivate ||
                    x == PlayerRoles.RoleTypeId.NtfCaptain ||
                    x == PlayerRoles.RoleTypeId.NtfSergeant ||
                    x == PlayerRoles.RoleTypeId.NtfSpecialist).Any())
                {
                    stat.ServerStat.RespawnedAsNTF++;
                }
                else
                {
                    stat.ServerStat.RespawnedAsChaos++;
                }
                Main.Instance.Statistic.AddStatForPlayer(item.UserId, stat);
            }
        }

        public static void EndingRound(EndingRoundEventArgs endingRoundEventArgs)
        {
            if (!endingRoundEventArgs.IsRoundEnded)
                return;
            foreach (var item in Player.List.Where(x => !x.DoNotTrack))
            {
                var stat = Main.Instance.Statistic.GetStatForPlayer(item.UserId);
                stat.ServerStat.RoundsFinished++;
                Main.Instance.Statistic.AddStatForPlayer(item.UserId, stat);
            }
            foreach (var item in Player.List.Where(x => !x.DoNotTrack && x.IsAlive))
            {
                var stat = Main.Instance.Statistic.GetStatForPlayer(item.UserId);
                stat.ServerStat.RoundsAlive++;
                Main.Instance.Statistic.AddStatForPlayer(item.UserId, stat);
            }
            Log.Info("Saving!");
            Main.Instance.Database.Save(Main.Instance.Statistic.AllStats);

            var path = Exiled.Loader.PathExtensions.GetPath(Main.Instance);
            //SAVE!!!
        }
    }
}
