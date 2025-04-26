using PlayerRoles;
using UnityEngine;
using VoiceChat;

namespace SimpleCustomRoles.RoleYaml;

public class FpcInfo
{
    public Vector3 Scale { get; set; } = Vector3.one;
    public Vector3 FakeScale { get; set; } = Vector3.one;
    public RoleTypeId Appearance { get; set; } = RoleTypeId.None;
    public VoiceChatChannel VoiceChatChannel { get; set; } = VoiceChatChannel.None;
}
