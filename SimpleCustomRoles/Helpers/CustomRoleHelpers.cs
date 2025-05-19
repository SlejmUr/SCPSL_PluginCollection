using LabApi.Features.Stores;
using LabApi.Features.Wrappers;
using LabApiExtensions.Extensions;
using MEC;
using SimpleCustomRoles.RoleInfo;
using SimpleCustomRoles.RoleYaml;

namespace SimpleCustomRoles.Helpers;

public static class CustomRoleHelpers
{
    public static bool IsShouldSpawn(CustomRoleBaseInfo roleInfo)
    {
        if (roleInfo.Spawn.MinimumPlayers == -1 && roleInfo.Spawn.MaximumPlayers == -1)
            return true;
        if (roleInfo.Spawn.MinimumPlayers == -1 && roleInfo.Spawn.MaximumPlayers >= Server.PlayerCount)
            return true;
        if (roleInfo.Spawn.MinimumPlayers <= Server.PlayerCount && roleInfo.Spawn.MaximumPlayers >= Server.PlayerCount)
            return true;
        if (roleInfo.Spawn.MinimumPlayers <= Server.PlayerCount && roleInfo.Spawn.MaximumPlayers == -1)
            return true;
        return false;
    }

    public static bool SetNewRole(Player player, NewRoleInfo newRoleInfo)
    {
        if (newRoleInfo.RoleType != PlayerRoles.RoleTypeId.None)
        {
            player.SetRole(newRoleInfo.RoleType, PlayerRoles.RoleChangeReason.None, newRoleInfo.Flags);
            return true;
        }
        CustomRoleBaseInfo customRoleInfo = null;
        if (!string.IsNullOrEmpty(newRoleInfo.Name))
            customRoleInfo = RolesLoader.RoleInfos.Where(x => x.Rolename == newRoleInfo.Name).FirstOrDefault();
        else if (newRoleInfo.Random.Count != 0)
            customRoleInfo = RolesLoader.RoleInfos.Where(x => x.Rolename == newRoleInfo.Random.RandomItem()).FirstOrDefault();
        if (customRoleInfo == null)
            return false;
        SetFromCMD(player, customRoleInfo);
        return true;
    }

    public static void SetFromCMD(Player player, CustomRoleBaseInfo customRoleInfo)
    {
        UnSetCustomInfoToPlayer(player);
        Timing.CallDelayed(0.1f, () => { SetCustomInfoToPlayer(player, customRoleInfo); });
    }

    public static void SetCustomInfoToPlayer(Player player, CustomRoleBaseInfo customRoleInfo)
    {
        if (player == null)
            return;
        CL.Debug($"SetCustomInfoToPlayer: {player.UserId} Role: {customRoleInfo.Rolename}", Main.Instance.Config.Debug);
        if (Contains(player))
            return;

        var roleStore = CustomDataStore.GetOrAdd<CustomRoleInfoStorage>(player);
        roleStore.Role = customRoleInfo;
        roleStore.Apply();
        Events.TriggerRoleAdded(player, customRoleInfo);
        CL.Debug($"SetCustomInfoToPlayer: {player.UserId} Role: {customRoleInfo.Rolename} Success", Main.Instance.Config.Debug);
    }

    public static void UnSetCustomInfoToPlayer(Player player, bool dontResetRole = false)
    {
        if (player == null)
            return;
        if (!Contains(player))
            return;
        var rolestorage = CustomDataStore.GetOrAdd<CustomRoleInfoStorage>(player);
        rolestorage.DontResetRole = dontResetRole;
        Events.TriggerRoleRemoved(player, rolestorage.Role);
        CustomDataStore.Destroy<CustomRoleInfoStorage>(player);
    }

    public static bool TryGetCustomRole(Player player, out CustomRoleBaseInfo customRoleInfo)
    {
        customRoleInfo = null;
        if (player == null)
            return false;
        customRoleInfo = CustomDataStore.GetOrAdd<CustomRoleInfoStorage>(player).Role;
        return customRoleInfo != null;
    }

    public static bool Contains(Player player)
    {
        if (!TryGetCustomRole(player, out var role))
            return false;
        return role != null;
    }

    public static IEnumerable<Player> GetPlayers()
    {
        var players = CustomDataStoreManagerExtended.GetAll<CustomRoleInfoStorage>();
        return players.Where(x=>x.Value is CustomRoleInfoStorage st && st != null && st.Role != null).Select(x=>x.Key);
    }

    public static Dictionary<Player, CustomRoleBaseInfo> GetPlayerAndRoles()
    {
        var players = CustomDataStoreManagerExtended.GetAll<CustomRoleInfoStorage>();
        return players.Where(x => x.Value is CustomRoleInfoStorage st && st != null && st.Role != null).ToDictionary(x => x.Key, x => ((CustomRoleInfoStorage)x.Value).Role);
    }

    public static IEnumerable<CustomRoleBaseInfo> GetCurrentCustomRoles()
    {
        var players = CustomDataStoreManagerExtended.GetAll<CustomRoleInfoStorage>();
        return players.Where(x => x.Value is CustomRoleInfoStorage st && st != null && st.Role != null).Select(x => ((CustomRoleInfoStorage)x.Value).Role);
    }
}
