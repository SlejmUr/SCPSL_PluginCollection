using Exiled.Events.EventArgs.Scp079;
using System;
using System.Linq;

namespace DavaStats.Handlers
{
    internal class SCP_079_Handler
    {
        public static void ChangingCamera(ChangingCameraEventArgs args)
        {
            if (args.Player.DoNotTrack && args.IsAllowed)
                return;
            var id = args.Player.UserId;
            var stat = Main.Instance.Statistic.GetStatForPlayer(id);
            stat.SCPStats.SCP_079.ChangingCamera++;
            Main.Instance.Statistic.AddStatForPlayer(id, stat);
        }

        public static void ChangingSpeakerStatus(ChangingSpeakerStatusEventArgs args)
        {
            if (args.Player.DoNotTrack && args.IsAllowed)
                return;
            var id = args.Player.UserId;
            var stat = Main.Instance.Statistic.GetStatForPlayer(id);
            stat.SCPStats.SCP_079.ChangingSpeakerStatus++;
            if (!stat.SCPStats.SCP_079.SpeakerChangeInRoom.ContainsKey(args.Room.Name))
            {
                stat.SCPStats.SCP_079.SpeakerChangeInRoom.Add(args.Room.Name, 0);
            }
            stat.SCPStats.SCP_079.SpeakerChangeInRoom[args.Room.Name]++;
            Main.Instance.Statistic.AddStatForPlayer(id, stat);
        }

        public static void ElevatorTeleporting(ElevatorTeleportingEventArgs args)
        {
            if (args.Player.DoNotTrack && args.IsAllowed)
                return;
            var id = args.Player.UserId;
            var stat = Main.Instance.Statistic.GetStatForPlayer(id);
            stat.SCPStats.SCP_079.ElevatorTeleporting++;
            if (!stat.SCPStats.SCP_079.ElevatorTeleportWithName.ContainsKey(args.Lift.Name))
            {
                stat.SCPStats.SCP_079.ElevatorTeleportWithName.Add(args.Lift.Name, 0);
            }
            stat.SCPStats.SCP_079.ElevatorTeleportWithName[args.Lift.Name]++;
            Main.Instance.Statistic.AddStatForPlayer(id, stat);
        }

        public static void GainingExperience(GainingExperienceEventArgs args)
        {
            if (args.Player.DoNotTrack && args.IsAllowed)
                return;
            var id = args.Player.UserId;
            var stat = Main.Instance.Statistic.GetStatForPlayer(id);
            stat.SCPStats.SCP_079.GainingExperienceTime++;
            stat.SCPStats.SCP_079.GainingExperienceAmount += (ulong)args.Amount;
            if (!stat.SCPStats.SCP_079.ExperienceGainRoleType.ContainsKey(args.RoleType))
            {
                stat.SCPStats.SCP_079.ExperienceGainRoleType.Add(args.RoleType, 0);
            }
            stat.SCPStats.SCP_079.ExperienceGainRoleType[args.RoleType]++;
            if (!stat.SCPStats.SCP_079.ExperienceGainType.ContainsKey(args.GainType))
            {
                stat.SCPStats.SCP_079.ExperienceGainType.Add(args.GainType, 0);
            }
            stat.SCPStats.SCP_079.ExperienceGainType[args.GainType]++;
            Main.Instance.Statistic.AddStatForPlayer(id, stat);
        }

        public static void GainingLevel(GainingLevelEventArgs args)
        {
            if (args.Player.DoNotTrack && args.IsAllowed)
                return;
            var id = args.Player.UserId;
            var stat = Main.Instance.Statistic.GetStatForPlayer(id);
            stat.SCPStats.SCP_079.GainingLevel++;
            if (!stat.SCPStats.SCP_079.GainingLevel_LevelMap.ContainsKey(args.NewLevel))
            {
                stat.SCPStats.SCP_079.GainingLevel_LevelMap.Add(args.NewLevel, 0);
            }
            stat.SCPStats.SCP_079.GainingLevel_LevelMap[args.NewLevel]++;
            Main.Instance.Statistic.AddStatForPlayer(id, stat);
        }

        public static void InteractingTesla(InteractingTeslaEventArgs args)
        {
            if (args.Player.DoNotTrack)
                return;
            var id = args.Player.UserId;
            var stat = Main.Instance.Statistic.GetStatForPlayer(id);
            stat.SCPStats.SCP_079.InteractingTesla++;
            Main.Instance.Statistic.AddStatForPlayer(id, stat);
        }

        public static void LockingDown(LockingDownEventArgs args)
        {
            if (args.Player.DoNotTrack)
                return;
            var id = args.Player.UserId;
            var stat = Main.Instance.Statistic.GetStatForPlayer(id);
            stat.SCPStats.SCP_079.LockingDown++;
            if (!stat.SCPStats.SCP_079.LockedInRoom.ContainsKey(args.Room.Name))
            {
                stat.SCPStats.SCP_079.LockedInRoom.Add(args.Room.Name, 0);
            }
            stat.SCPStats.SCP_079.LockedInRoom[args.Room.Name]++;
            Main.Instance.Statistic.AddStatForPlayer(id, stat);
        }

        public static void Pinging(PingingEventArgs args)
        {
            if (args.Player.DoNotTrack)
                return;
            var id = args.Player.UserId;
            var stat = Main.Instance.Statistic.GetStatForPlayer(id);
            stat.SCPStats.SCP_079.Pinging++;
            if (!stat.SCPStats.SCP_079.Pings.ContainsKey(args.Type))
            {
                stat.SCPStats.SCP_079.Pings.Add(args.Type, 0);
            }
            stat.SCPStats.SCP_079.Pings[args.Type]++;
            Main.Instance.Statistic.AddStatForPlayer(id, stat);
        }

        public static void Recontained(RecontainedEventArgs args)
        {
            if (args.Player.DoNotTrack)
                return;
            var id = args.Player.UserId;
            var stat = Main.Instance.Statistic.GetStatForPlayer(id);
            stat.SCPStats.SCP_079.Recontained++;
            Main.Instance.Statistic.AddStatForPlayer(id, stat);
        }

        public static void RoomBlackout(RoomBlackoutEventArgs args)
        {
            if (args.Player.DoNotTrack)
                return;
            var id = args.Player.UserId;
            var stat = Main.Instance.Statistic.GetStatForPlayer(id);
            stat.SCPStats.SCP_079.RoomBlackout++;
            Main.Instance.Statistic.AddStatForPlayer(id, stat);
        }

        public static void TriggeringDoor(TriggeringDoorEventArgs args)
        {
            if (args.Player.DoNotTrack)
                return;
            var id = args.Player.UserId;
            var stat = Main.Instance.Statistic.GetStatForPlayer(id);
            stat.SCPStats.SCP_079.TriggeringDoor++;
            Main.Instance.Statistic.AddStatForPlayer(id, stat);
        }

        public static void ZoneBlackout(ZoneBlackoutEventArgs args)
        {
            if (args.Player.DoNotTrack)
                return;
            var id = args.Player.UserId;
            var stat = Main.Instance.Statistic.GetStatForPlayer(id);
            stat.SCPStats.SCP_079.ZoneBlackout++;
            Main.Instance.Statistic.AddStatForPlayer(id, stat);
        }
    }
}
