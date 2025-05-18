using System.ComponentModel;

namespace AntiStall;

public class Config
{
    public bool IsEnabled { get; set; }
    public bool Debug { get; set; }

    public string DontStallMSG { get; set; } = "Please don't stall! Move!";
    public int DontStallMGSTime { get; set; } = 3;

    public string DontStallMSG_Resend { get; set; } = "You are over 4 minute in the same room! Move!";
    public int DontStallMGSTime_Resend { get; set; } = 3;


    [Description("Mimimum time to be in the same room. (120 is a minute)")]
    public int StallMin { get; set; } = 480;

    [Description("Time after the DontStallMSG_Resend should activate.")]
    public int StallResend { get; set; } = 520;

    [Description("Time adding to StallResend after its being sent.")]
    public int StallAddResend { get; set; } = 20;

    [Description("Should SCPs can get the message")]
    public bool UseOnSCP { get; set; } = false;
}
