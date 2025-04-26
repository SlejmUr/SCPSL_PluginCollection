using Interactables.Interobjects;
using LabApi.Features.Wrappers;
using MEC;
using PlayerRoles;

namespace SimpleCustomRoles.Helpers;

internal static class AppearanceSync
{
    static readonly Dictionary<Player, (RoleTypeId role, List<Player> players)> PlayerToSpyRole = [];
    static CoroutineHandle handle;

    public static void Start()
    {
        handle = Timing.RunCoroutine(Sync());
    }

    public static void Stop()
    {
        Timing.KillCoroutines(handle);
    }

    public static void AddPlayer(Player player, RoleTypeId role)
    {
        lock (PlayerToSpyRole)
        {
            PlayerToSpyRole.Add(player, (role, []));
        }
    }

    public static void RemovePlayer(Player player)
    {
        lock (PlayerToSpyRole)
        {
            PlayerToSpyRole.Remove(player);
        }
    }

    public static void ForceSync(Player player)
    {
        lock (PlayerToSpyRole)
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
            lock (PlayerToSpyRole)
            {
                for (int i = 0; i < PlayerToSpyRole.Count; i++)
                {
                    var kv = PlayerToSpyRole.ElementAt(i);
                    // skip in elevator
                    if (ElevatorChamber.AllChambers.Any(x => x.WorldspaceBounds.Contains(kv.Key.Position)))
                        continue;
                    List<Player> playersToSync = [.. Player.List.Where(x => x.IsReady && x != kv.Key)];
                    if (kv.Value.players.Count != 0)
                        playersToSync = [.. playersToSync.Except(kv.Value.players)];
                    if (playersToSync.Count == 0)
                        continue;
                    //Log.Info("Sync to Ids: " + string.Join(", ", playersToSync.Select(x=>x.Id)));
                    kv.Key.ChangeAppearance(kv.Value.role, playersToSync);
                    kv.Value.players.AddRange(playersToSync);
                    PlayerToSpyRole[kv.Key] = kv.Value;
                }
            }
            yield return 10f;
        }
    }
}
