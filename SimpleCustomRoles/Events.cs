using LabApi.Features.Wrappers;
using SimpleCustomRoles.RoleYaml;

namespace SimpleCustomRoles;

public static class Events
{
    public static Action<Player, CustomRoleBaseInfo> OnRoleAdded;
    public static Action<Player, CustomRoleBaseInfo> OnRoleRemoved;


    internal static void TriggerRoleAdded(Player player, CustomRoleBaseInfo role)
    {
        OnRoleAdded?.Invoke(player, role);
    }

    internal static void TriggerRoleRemoved(Player player, CustomRoleBaseInfo role)
    {
        OnRoleRemoved?.Invoke(player, role);
    }
}
