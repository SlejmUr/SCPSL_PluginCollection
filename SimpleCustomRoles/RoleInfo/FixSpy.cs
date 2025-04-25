using Exiled.API.Extensions;
using Exiled.API.Features;
using Interactables.Interobjects;
using MEC;
using PlayerRoles;

namespace SimpleCustomRoles.RoleInfo;

// This class used to sync to all player the change.
internal class FixSpy
{
    static Dictionary<Player, (RoleTypeId role, List<Player> players)> PlayerToSpyRole = [];
    static object locker = new();
    static CoroutineHandle handle;

    public static void StartSync()
    {
        handle = Timing.RunCoroutine(Sync());
    }

    public static void Stop()
    {
        Timing.KillCoroutines(handle);
    }

    public static void AddPlayer(Player player, RoleTypeId role)
    {
        lock (locker)
        {
            PlayerToSpyRole.Add(player, (role, []));
        }
    }

    public static void RemovePlayer(Player player)
    {
        lock (locker)
        {
            PlayerToSpyRole.Remove(player);
        }
    }

    public static void ForceSync(Player player)
    {
        lock (locker)
        {
            for (int i = 0; i < PlayerToSpyRole.Count; i++)
            {
                var kv = PlayerToSpyRole.ElementAt(i);
                if (kv.Key == player)
                    continue;
                kv.Value.players.Remove(player);
                PlayerToSpyRole[kv.Key] = kv.Value;
            }
        }
    }

    static IEnumerator<float> Sync()
    {
        yield return 1f;
        while (true)
        {
            lock (locker)
            {
                for (int i = 0; i < PlayerToSpyRole.Count; i++)
                {
                    var kv = PlayerToSpyRole.ElementAt(i);
                    // skip in elevator
                    if (ElevatorChamber.AllChambers.Any(x => x.WorldspaceBounds.Contains(kv.Key.Position)))
                        continue;
                    List<Player> playersToSync = [.. Player.List.Where(x => x.IsConnected && x != kv.Key)];
                    if (kv.Value.players.Count != 0)
                        playersToSync = [.. playersToSync.Except(kv.Value.players)];
                    if (playersToSync.Count == 0)
                        continue;
                    kv.Key.ChangeAppearance(kv.Value.role, playersToSync);
                    kv.Value.players.AddRange(playersToSync);
                    PlayerToSpyRole[kv.Key] = kv.Value;
                }
            }
            yield return 10f;
        }
    }
}
