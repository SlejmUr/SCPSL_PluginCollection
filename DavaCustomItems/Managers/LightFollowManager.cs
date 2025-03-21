using DavaCustomItems.Configs;
using Exiled.API.Interfaces;
using MEC;

namespace DavaCustomItems.Managers;

public static class LightFollowManager
{
    static Dictionary<IPosition, KeyValuePair<CoroutineHandle, int>> PositionToFollower = [];

    /// <summary>
    /// Make a new Light with the config as <paramref name="lightConfig"/> and position too <paramref name="position"/>, if success start following the <paramref name="position"/>
    /// </summary>
    /// <param name="position"></param>
    /// <param name="lightConfig"></param>
    /// <returns>A LightId</returns>
    public static int MakeLightAndFollow(IPosition position, LightConfig lightConfig)
    {
        int id = LightManager.MakeLight(position.Position, lightConfig);
        if (id == -1)
            return id;
        StartFollow(id, position);
        return id;
    }

    /// <summary>
    /// Start following the <paramref name="position"/> with using Light source as <paramref name="LightId"/>
    /// </summary>
    /// <param name="LightId"></param>
    /// <param name="position"></param>
    public static void StartFollow(int LightId, IPosition position)
    {
        if (LightId == -1)
            return;
        if (!LightManager.IsLightShown(LightId))
            LightManager.ShowLight(LightId);
        StopFollow(position, false);
        if (!LightManager.IsLightExists(LightId))
            return;
        PositionToFollower.Add(position, new(Timing.RunCoroutine(LightMove(LightId, position)), LightId));
    }

    /// <summary>
    /// Stop following <paramref name="position"/> and return the LightId
    /// </summary>
    /// <param name="position"></param>
    /// <param name="shouldHide">Optionally should hide the Light</param>
    /// <returns>A LightId</returns>
    public static int StopFollow(IPosition position, bool shouldHide = true)
    {
        if (!PositionToFollower.TryGetValue(position, out var coroutineHandle))
            return -1;
        PositionToFollower.Remove(position);
        Timing.KillCoroutines(coroutineHandle.Key);
        if (shouldHide)
            LightManager.HideLight(coroutineHandle.Value);
        return coroutineHandle.Value;
    }

    /// <summary>
    /// Stop following <paramref name="oldFollower"/> and start following <paramref name="newFollower"/>
    /// </summary>
    /// <param name="oldFollower"></param>
    /// <param name="newFollower"></param>
    public static void StopFollowAndStartFollow(IPosition oldFollower, IPosition newFollower)
    {
        int id = StopFollow(oldFollower, false);
        if (id == -1)
            return;
        StartFollow(id, newFollower);
    }


    private static IEnumerator<float> LightMove(int LightId, IPosition position)
    {
        yield return 0;
        while (LightManager.IsLightExists(LightId) && PositionToFollower.ContainsKey(position))
        {
            LightManager.SetLightPos(LightId, position.Position);
            yield return 0;
        }
        yield break;
    }
}
