using Exiled.API.Enums;
using Exiled.API.Features;
using PlayerRoles;
using PlayerRoles.PlayableScps.Scp079;
using System.Collections.Generic;

namespace DavaStats.Jsons.SCPStats
{
    public class SCP_079Stat
    {
        public ulong ChangingCamera { get; set; } = 0;
        public ulong GainingExperienceTime { get; set; } = 0;
        public ulong GainingExperienceAmount { get; set; } = 0;
        public ulong GainingLevel { get; set; } = 0;
        public ulong InteractingTesla { get; set; } = 0;
        public ulong TriggeringDoor { get; set; } = 0;
        public ulong LockingDown { get; set; } = 0;
        public ulong ChangingSpeakerStatus { get; set; } = 0;
        public ulong Recontained { get; set; } = 0;
        public ulong Pinging { get; set; } = 0;
        public ulong RoomBlackout { get; set; } = 0;
        public ulong ZoneBlackout { get; set; } = 0;
        public ulong ElevatorTeleporting { get; set; } = 0;

        public Dictionary<string, ulong> SpeakerChangeInRoom { get; set; } = new Dictionary<string, ulong>();
        public Dictionary<string, ulong> LockedInRoom { get; set; } = new Dictionary<string, ulong>();
        public Dictionary<int, ulong> GainingLevel_LevelMap { get; set; } = new Dictionary<int, ulong>();
        public Dictionary<string, ulong> ElevatorTeleportWithName { get; set; } = new Dictionary<string, ulong>();
        public Dictionary<Scp079HudTranslation, ulong> ExperienceGainType { get; set; } = new Dictionary<Scp079HudTranslation, ulong>();
        public Dictionary<RoleTypeId, ulong> ExperienceGainRoleType { get; set; } = new Dictionary<RoleTypeId, ulong>();
        public Dictionary<PingType, ulong> Pings { get; set; } = new Dictionary<PingType, ulong>();
    }
}
