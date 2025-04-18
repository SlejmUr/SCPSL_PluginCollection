using Exiled.API.Extensions;
using Exiled.API.Features;
using Interactables.Interobjects;
using MEC;
using PlayerRoles;

namespace SimpleCustomRoles.RoleInfo;

// This class used to sync to all player the change.
internal class FixSpy
{
    public static Dictionary<Player, (RoleTypeId, List<Player>)> PlayerToSpyRole = [];
    static CoroutineHandle handle;

    public static void StartSync()
    {
        handle = Timing.RunCoroutine(_Sync());
    }

    public static void Stop()
    {
        Timing.KillCoroutines(handle);
    }

    static IEnumerator<float> _Sync()
    {
        yield return 1f;
        while (true)
        {
            for (int i = 0; i < PlayerToSpyRole.Count; i++)
            {
                var kv = PlayerToSpyRole.ElementAt(i);
                // skip in elevator
                if (ElevatorChamber.AllChambers.Any(x => x.WorldspaceBounds.Contains(kv.Key.Position)))
                    continue;
                List<Player> playersToSync = Player.List.ToList();
                if (kv.Value.Item2.Count != 0)
                    playersToSync = Player.List.Except(kv.Value.Item2).ToList();
                if (playersToSync.Count == 0)
                    continue;
                kv.Key.ChangeAppearance(kv.Value.Item1, playersToSync);
                kv.Value.Item2.AddRange(playersToSync);
                PlayerToSpyRole[kv.Key] = kv.Value;
            }
            yield return 10f;
        }
    }
}
