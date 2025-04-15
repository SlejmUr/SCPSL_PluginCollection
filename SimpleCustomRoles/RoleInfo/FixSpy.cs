using Exiled.API.Extensions;
using Exiled.API.Features;
using MEC;
using PlayerRoles;

namespace SimpleCustomRoles.RoleInfo;

// This class used to sync to all player the change.
internal class FixSpy
{
    public static Dictionary<Player, RoleTypeId> PlayerToSpyRole = [];
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
            foreach (var player in PlayerToSpyRole)
            {
                player.Key.ChangeAppearance(player.Value, true);
            }
            yield return 5f;
        }
    }
}
