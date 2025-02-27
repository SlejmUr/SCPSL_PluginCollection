using Exiled.API.Features;
using Hints;
using System.Linq;
using UserSettings.ServerSpecific;

namespace SimpleCustomRoles.SSS;

internal class Logic
{
    static SSKeybindSetting showRolekb;
    static ServerSpecificSettingBase[] Settings;

    public static void Init()
    {
        Settings =
        [
            new SSGroupHeader("Simple Custom Roles"),
            showRolekb = new SSKeybindSetting(null,"Show my Role Info Again", UnityEngine.KeyCode.L)
        ];
        ServerSpecificSettingsSync.ServerOnSettingValueReceived += ServerOnSettingValueReceived;
    }

    public static void UnInit()
    {
        ServerSpecificSettingsSync.ServerOnSettingValueReceived -= ServerOnSettingValueReceived;
    }

    public static void SetToCustomRole(Player player)
    {
        var tmp_old = ServerSpecificSettingsSync.DefinedSettings.ToList();
        ServerSpecificSettingsSync.DefinedSettings =
        [
            ..tmp_old,
            ..Settings
        ];
        ServerSpecificSettingsSync.SendToPlayer(player.ReferenceHub);
    }

    public static void UnSet(Player player)
    {
        var tmp_old = ServerSpecificSettingsSync.DefinedSettings.ToList();
        tmp_old.RemoveAll(x => Settings.Contains(x));
        ServerSpecificSettingsSync.DefinedSettings = tmp_old.ToArray();
        ServerSpecificSettingsSync.SendToPlayer(player.ReferenceHub);
    }

    private static void ServerOnSettingValueReceived(ReferenceHub hub, ServerSpecificSettingBase @base)
    {
        if (!Main.Instance.PlayerCustomRole.TryGetValue(hub.authManager.UserId, out var customRoleInfo))
            return;

        if (string.IsNullOrEmpty(customRoleInfo.Hint.SpawnHint)) // role does not have any spawning hints
            return;

        if (@base is SSKeybindSetting { SyncIsPressed: true } keybindSetting && keybindSetting.SettingId == showRolekb.SettingId)
        {
            hub.hints.Show(new TextHint(customRoleInfo.Hint.SpawnHint,
            [
                new StringHintParameter(customRoleInfo.Hint.SpawnHint)
            ], null, customRoleInfo.Hint.SpawnHintDuration));
        }

    }
}
