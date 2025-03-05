using Hints;
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
        List<ServerSpecificSettingBase> settingBases = [];
        if (ServerSpecificSettingsSync.DefinedSettings != null)
        {
            settingBases = ServerSpecificSettingsSync.DefinedSettings.ToList();
        }
        settingBases.AddRange(Settings);
        ServerSpecificSettingsSync.DefinedSettings = settingBases.ToArray();
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
        if (!Main.Instance.PlayerCustomRole.TryGetValue(hub.authManager.UserId, out var customRoleInfo))
            return;

        if (@base is SSKeybindSetting { SyncIsPressed : true } keybindSetting && keybindSetting.SettingId == showRolekb.SettingId)
        {
            if (!string.IsNullOrEmpty(customRoleInfo.Hint.SpawnHint)) // role does not have any spawning hints
            {
                hub.hints.Show(new TextHint(customRoleInfo.Hint.SpawnHint,
                [
                    new StringHintParameter(customRoleInfo.Hint.SpawnHint)
                ], null, customRoleInfo.Hint.SpawnHintDuration));
            }
            if (!string.IsNullOrEmpty(customRoleInfo.Hint.SpawnBroadcast)) // role does not have any broadcast hints
            {
                Broadcast.Singleton.TargetAddElement(hub.connectionToClient, customRoleInfo.Hint.SpawnBroadcast, customRoleInfo.Hint.SpawnBroadcastDuration, Broadcast.BroadcastFlags.Normal);
            }
        }
            
    }
}
