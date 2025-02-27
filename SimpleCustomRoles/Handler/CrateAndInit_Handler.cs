using LabApi.Features.Wrappers;
using MEC;
using SimpleCustomRoles.RoleInfo;
using SimpleCustomRoles.SSS;

namespace SimpleCustomRoles.Handler;

internal class CrateAndInit_Handler
{
    public static void ReloadRoles()
    {
        Main.Instance.RegularRoles = [];
        Main.Instance.PlayerCustomRole = [];
        Main.Instance.InWaveRoles = [];
        Main.Instance.AfterDeathRoles = [];
        Main.Instance.SPC_SpecificRoles = [];
        Main.Instance.RolesLoader.Load();
        foreach (var item in Main.Instance.RolesLoader.RoleInfos)
        {
            if (item.RoleType == CustomRoleType.AfterDead)
            {
                Main.Instance.AfterDeathRoles.Add(item);
                continue;
            }
            for (int i = 0; i < item.SpawnAmount; i++)
            {
                if (item.RoleType == CustomRoleType.SPC_Specific)
                    Main.Instance.SPC_SpecificRoles.Add(item);
                if (item.RoleType == CustomRoleType.InWave)
                    Main.Instance.InWaveRoles.Add(item);
            }

        }
    }

    public static void WaitingForPlayers()
    {
        Main.Instance.PlayerCustomRole = [];
        Main.Instance.RegularRoles = [];
        Main.Instance.InWaveRoles = [];
        Main.Instance.AfterDeathRoles = [];
        Main.Instance.SPC_SpecificRoles = [];
        Main.Instance.EscapeRoles = [];
        Main.Instance.RolesLoader.Load();
        if (Main.Instance.Config.Debug)
            CL.Info("Loading custom roles!");
        foreach (var item in Main.Instance.RolesLoader.RoleInfos)
        {
            if (item.RoleType == CustomRoleType.AfterDead)
            {
                if (Main.Instance.Config.Debug)
                    CL.Info($"After Death Role added: " + item.RoleName);
                Main.Instance.AfterDeathRoles.Add(item);
                continue;
            }
            if (item.RoleType == CustomRoleType.Escape)
            {
                if (Main.Instance.Config.Debug)
                    CL.Info($"Escape Role added: " + item.RoleName);
                Main.Instance.EscapeRoles.Add(item);
                continue;
            }
            for (int i = 0; i < item.SpawnAmount; i++)
            {
                bool IsSpawning = false;
                var random = RandomGenerator.GetInt16(1, 10000, true);
                if (random <= item.SpawnChance)
                {
                    IsSpawning = true;
                    if (item.RoleType == CustomRoleType.SPC_Specific)
                        Main.Instance.SPC_SpecificRoles.Add(item);
                    else
                        Main.Instance.RegularRoles.Add(item);
                }
                if (Main.Instance.Config.Debug)
                    CL.Info($"Rolled chance: {random}/{item.SpawnChance} for Role {item.RoleName}. Role is " + (IsSpawning ? "" : "NOT ") + "spawning.");
            }
        }
        CL.Info("Loading custom roles finished!");
    }

    public static void RoundStarted()
    {
        if (Main.Instance.Config.IsPaused)
            return;
        foreach (var item in Main.Instance.RegularRoles)
        {
            if (item.RoleType == CustomRoleType.InWave)
            {
                Main.Instance.InWaveRoles.Add(item);
                continue;
            }
            Player player = null;
            if (item.RoleToReplace == PlayerRoles.RoleTypeId.None && item.ReplaceFromTeam != PlayerRoles.Team.Dead)
            {
                player = Player.List.Where(x => x.Team == item.ReplaceFromTeam).ToList().RandomItem();
            }
            else
            {
                player = Player.List.Where(x => x.Role == item.RoleToReplace).ToList().RandomItem();
            }
            if (player == null)
                continue;
            if (Main.Instance.Config.Debug)
                CL.Info("Player Selected to spawn: " + player.UserId);
            RoleSetter.SetCustomInfoToPlayer(player, item);
        }
        Main.Instance.RegularRoles.Clear();
    }
}
