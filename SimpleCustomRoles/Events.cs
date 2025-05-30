using LabApi.Features.Wrappers;
using SimpleCustomRoles.RoleYaml;

namespace SimpleCustomRoles;

public static class Events
{
    public static Action<Player, CustomRoleBaseInfo> OnRoleAdded;
    public static Action<Player, CustomRoleBaseInfo> OnRoleRemoved;

    public static Action<Player, CustomRoleBaseInfo, Player> OnRoleSpectated;


    internal static void TriggerRoleAdded(Player player, CustomRoleBaseInfo role)
    {
        OnRoleAdded?.Invoke(player, role);
    }

    internal static void TriggerRoleRemoved(Player player, CustomRoleBaseInfo role)
    {
        OnRoleRemoved?.Invoke(player, role);
    }

    internal static void TriggerRoleSpectated(Player player, CustomRoleBaseInfo role, Player spectator)
    {
        OnRoleSpectated?.Invoke(player, role, spectator);
    }
}
