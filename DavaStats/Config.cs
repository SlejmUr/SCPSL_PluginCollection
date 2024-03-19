using Exiled.API.Interfaces;
using System;

namespace DavaStats
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; }
        public bool Debug { get; set; }
        public bool UseFileStorage { get; set; } = false;
        public bool UseLiteDBStorage { get; set; } = true;
    }
}
