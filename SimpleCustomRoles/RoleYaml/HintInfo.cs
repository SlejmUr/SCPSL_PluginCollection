using System.ComponentModel;

namespace SimpleCustomRoles.RoleYaml;

public class HintInfo
{
    [Description("Suggestion: Say that what the user spawned as. Can use something like <color=#ededb4><b>TEMP</b></color>\\n\\tFav role")]
    public string Broadcast { get; set; } = string.Empty;

    [Description("Suggestion: Set as 15, so when you spawned most of the UI is gonna get obscured. After 10 or 8 second it will be removed/hidden.")]
    public ushort BroadcastDuration { get; set; } = 0;

    [Description("Suggestion: Any hints to display.")]
    public string Hint { get; set; } = string.Empty;

    [Description("Suggestion: Set as 15, so when you spawned most of the UI is gonna get obscured. After 10 or 8 second it will be removed/hidden.")]
    public float HintDuration { get; set; } = 0;

    [Description("Broadcast to All users.")]
    public string BroadcastAll { get; set; } = string.Empty;

    [Description("Suggestion: Set as 15, so when you spawned most of the UI is gonna get obscured. After 10 or 8 second it will be removed/hidden.")]
    public ushort BroadcastAllDuration { get; set; } = 0;
}
