using LabApi.Features.Wrappers;
using SimpleCustomRoles.Helpers;
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
            showRolekb = new SSKeybindSetting(null, "Show my Role Info Again", UnityEngine.KeyCode.L)
        ];
        List<ServerSpecificSettingBase> settingBases = [];
        if (ServerSpecificSettingsSync.DefinedSettings != null)
        {
            settingBases = [.. ServerSpecificSettingsSync.DefinedSettings];
        }
        settingBases.AddRange(Settings);
        ServerSpecificSettingsSync.DefinedSettings = [.. settingBases];
        ServerSpecificSettingsSync.ServerOnSettingValueReceived += ServerOnSettingValueReceived;
        ServerSpecificSettingsSync.SendToAll();
    }

    public static void UnInit()
    {
        ServerSpecificSettingsSync.DefinedSettings = [];
        ServerSpecificSettingsSync.ServerOnSettingValueReceived -= ServerOnSettingValueReceived;
        ServerSpecificSettingsSync.SendToAll();
    }

    private static void ServerOnSettingValueReceived(ReferenceHub hub, ServerSpecificSettingBase @base)
    {
        Player player = Player.Get(hub);
        if (!CustomRoleHelpers.TryGetCustomRoleStorage(player, out var storage))
            return;

        if (@base is SSKeybindSetting { SyncIsPressed: true } keybindSetting && keybindSetting.SettingId == showRolekb.SettingId)
        {
            storage.SetHints();
        }

    }
}
