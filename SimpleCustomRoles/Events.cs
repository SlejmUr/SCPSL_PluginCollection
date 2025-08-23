using LabApi.Features.Wrappers;
using SimpleCustomRoles.RoleYaml;

namespace SimpleCustomRoles;

public static class Events
{
    public static event Action<Player, CustomRoleBaseInfo> OnRoleAdded;
    public static event Action<Player, CustomRoleBaseInfo> OnRoleRemoved;

    public static event Action<Player, CustomRoleBaseInfo, Player> OnRoleSpectated;

    public static event Action<Player, CustomRoleBaseInfo, TypeWrapper<bool>> OnShowHint;
    public static event Action<Player, CustomRoleBaseInfo, TypeWrapper<bool>> OnShowBroadcast;
    public static event Action<Player, CustomRoleBaseInfo, TypeWrapper<bool>> OnShowBroadcastAll;

    internal static void TriggerRoleAdded(Player player, CustomRoleBaseInfo role)
        => OnRoleAdded?.Invoke(player, role);

    internal static void TriggerRoleRemoved(Player player, CustomRoleBaseInfo role)
        => OnRoleRemoved?.Invoke(player, role);

    internal static void TriggerRoleSpectated(Player player, CustomRoleBaseInfo role, Player spectator)
        => OnRoleSpectated?.Invoke(player, role, spectator);

    internal static void TriggerShowHint(Player player, CustomRoleBaseInfo role, ref bool runOriginal)
    {
        TypeWrapper<bool> runOriginalHelper = runOriginal;
        OnShowHint?.Invoke(player, role, runOriginalHelper);
        runOriginal = runOriginalHelper;
    }

    internal static void TriggerShowBroadcast(Player player, CustomRoleBaseInfo role, ref bool runOriginal)
    {
        TypeWrapper<bool> runOriginalHelper = runOriginal;
        OnShowBroadcast?.Invoke(player, role, runOriginalHelper);
        runOriginal = runOriginalHelper;
    }

    internal static void TriggerShowBroadcastAll(Player player, CustomRoleBaseInfo role, ref bool runOriginal)
    {
        TypeWrapper<bool> runOriginalHelper = runOriginal;
        OnShowBroadcastAll?.Invoke(player, role, runOriginalHelper);
        runOriginal = runOriginalHelper;
    }
}
