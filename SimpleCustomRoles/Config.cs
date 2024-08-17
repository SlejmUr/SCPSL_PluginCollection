using Exiled.API.Interfaces;
using System;

namespace SimpleCustomRoles
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; }
        public bool Debug { get; set; }
        public bool IsPaused { get; set; }
        public ushort SpectatorBroadcastTime { get; set; } = 7;
    }
}
