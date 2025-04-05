using LabApi.Features.Stores;
using LabApi.Features.Wrappers;
using MEC;
using SimpleCustomRoles.RoleInfo;

namespace SimpleCustomRoles.Helpers;

public static class CustomRoleHelpers
{
    public static bool IsShouldSpawn(CustomRoleInfo roleInfo)
    {
        if (roleInfo.MinimumPlayers == -1 && roleInfo.MaximumPlayers == -1)
            return true;
        if (roleInfo.MinimumPlayers == -1 && roleInfo.MaximumPlayers >= Server.PlayerCount)
            return true;
        if (roleInfo.MinimumPlayers <= Server.PlayerCount && roleInfo.MaximumPlayers >= Server.PlayerCount)
            return true;
        if (roleInfo.MinimumPlayers <= Server.PlayerCount && roleInfo.MaximumPlayers == -1)
            return true;
        return false;
    }

    public static void SetFromCMD(Player player, CustomRoleInfo customRoleInfo)
    {
        UnSetCustomInfoToPlayer(player);
        Timing.CallDelayed(0.5f, () => { SetCustomInfoToPlayer(player, customRoleInfo); });
    }

    public static void SetCustomInfoToPlayer(Player player, CustomRoleInfo customRoleInfo)
    {
        if (player == null)
            return;
        CL.Info("SetCustomInfoToPlayer: " + player.UserId + " Role: " + customRoleInfo.Rolename);
        if (Contains(player))
            return;

        var roleStore = CustomDataStore.GetOrAdd<CustomRoleInfoStorage>(player);
        roleStore.Role = customRoleInfo;
        roleStore.Apply();

         CL.Info("SetCustomInfoToPlayer: " + player.UserId + " Role: " + customRoleInfo.Rolename + " Success");
    }

    public static void UnSetCustomInfoToPlayer(Player player)
    {
        if (player == null)
            return;
        if (Contains(player))
            CustomDataStore.Destroy<CustomRoleInfoStorage>(player);
    }

    public static bool TryGetCustomRole(Player player, out CustomRoleInfo customRoleInfo)
    {
        customRoleInfo = null;
        if (player == null)
            return false;
        customRoleInfo = CustomDataStore.GetOrAdd<CustomRoleInfoStorage>(player).Role;
        return customRoleInfo != null;
    }

    public static bool Contains(Player player)
    {
        return TryGetCustomRole(player, out var role) && role != null;
    }

    public static IEnumerable<Player> GetPlayers()
    {
        var players = CustomDataStoreManagerExtended.GetAll<CustomRoleInfoStorage>();
        return players.Where(x=>x.Value is CustomRoleInfoStorage st && st != null && st.Role != null).Select(x=>x.Key);
    }
}
