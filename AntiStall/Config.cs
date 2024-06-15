using Exiled.API.Interfaces;

namespace AntiStall
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; }
        public bool Debug { get; set; }

        public string DontStallMSG { get; set; } = "Please don't stall! Move!";

        public int DontStallMGSTime { get; set; } = 3;
    }
}
