using Exiled.Events.EventArgs.Player;
using Exiled.API.Features;
using MEC;
using System.Collections.Generic;

namespace AntiStall
{
    internal class TheHandler
    {
        public static Dictionary<string, CoroutineHandle> PlayerToCR = new Dictionary<string, CoroutineHandle>();

        public static void Died(DiedEventArgs diedEventArgs)
        {
            if (PlayerToCR.ContainsKey(diedEventArgs.Player.UserId))
            {
                Timing.KillCoroutines(PlayerToCR[diedEventArgs.Player.UserId]);
                PlayerToCR.Remove(diedEventArgs.Player.UserId);
            }
        }

        public static void Spawned(SpawnedEventArgs spawnedEventArgs)
        {
            if (PlayerToCR.ContainsKey(spawnedEventArgs.Player.UserId))
            {
                Timing.KillCoroutines(PlayerToCR[spawnedEventArgs.Player.UserId]);
                PlayerToCR.Remove(spawnedEventArgs.Player.UserId);
            }
            Room last_room = spawnedEventArgs.Player.CurrentRoom;
            PlayerToCR.Add(spawnedEventArgs.Player.UserId, Timing.RunCoroutine(AntiStaller(spawnedEventArgs.Player, last_room)));
        }

        public static IEnumerator<float> AntiStaller(Player player, Room room)
        {
            int max_stall = 480; // this is 4 min +/- some ms
            int stall = 0;
            bool IsNotifiedSent = false;
            int renotify = 540; // arbitrary number
            int notifyagain_time = 12; // should be 10s +/- some ms difference
            while (true)
            {
                if (player == null)
                    break;
                if (!player.IsAlive)
                    break;
                if (player.IsGodModeEnabled)
                    break;
                if (Warhead.IsDetonated)
                    break;
                if (player.CurrentRoom != room)
                {
                    stall = 0;
                    IsNotifiedSent = false;
                    renotify = 400;
                    notifyagain_time = 10;
                }
                if (stall > max_stall && (!IsNotifiedSent || stall > renotify))
                {
                    player.ShowHint(Main.Instance.Config.DontStallMSG, Main.Instance.Config.DontStallMGSTime);
                    IsNotifiedSent = true;
                    renotify += notifyagain_time;
                }
                if (room != player.CurrentRoom)
                    room = player.CurrentRoom;
                stall++;
                yield return Timing.WaitForSeconds(0.5f);
            }
        }
    }
}
