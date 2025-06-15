using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Wrappers;
using MEC;

namespace AntiStall;

internal class TheHandler : CustomEventsHandler
{
    public static Dictionary<string, CoroutineHandle> PlayerToCR = [];

    public override void OnPlayerDeath(PlayerDeathEventArgs ev)
    {
        if (PlayerToCR.ContainsKey(ev.Player.UserId))
        {
            Timing.KillCoroutines(PlayerToCR[ev.Player.UserId]);
            PlayerToCR.Remove(ev.Player.UserId);
        }
    }

    public override void OnPlayerSpawned(PlayerSpawnedEventArgs ev)
    {
        if (PlayerToCR.ContainsKey(ev.Player.UserId))
        {
            Timing.KillCoroutines(PlayerToCR[ev.Player.UserId]);
            PlayerToCR.Remove(ev.Player.UserId);
        }
        Room last_room = ev.Player.Room;
        PlayerToCR.Add(ev.Player.UserId, Timing.RunCoroutine(AntiStaller(ev.Player, last_room)));
    }

    public static IEnumerator<float> AntiStaller(Player player, Room room)
    {
        int max_stall = Main.Instance.Config.StallMin; // this is 4 min +/- some ms
        int stall = 0;
        bool IsNotifiedSent = false;
        int renotify = Main.Instance.Config.StallResend; // arbitrary number
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
            if (player.IsSCP && Main.Instance.Config.UseOnSCP)
                break;
            if (player.Room != room)
            {
                stall = 0;
                IsNotifiedSent = false;
                renotify = Main.Instance.Config.StallResend;
            }
            if (stall > max_stall && (!IsNotifiedSent || stall > renotify))
            {
                if (stall > renotify)
                {
                    player.SendHint(Main.Instance.Config.DontStallMSG_Resend, Main.Instance.Config.DontStallMGSTime_Resend);
                }
                else
                {
                    player.SendHint(Main.Instance.Config.DontStallMSG, Main.Instance.Config.DontStallMGSTime);
                }
                IsNotifiedSent = true;
                renotify += Main.Instance.Config.StallAddResend;
            }
            if (room != player.Room)
                room = player.Room;
            stall++;
            yield return Timing.WaitForSeconds(0.5f);
        }
    }
}
