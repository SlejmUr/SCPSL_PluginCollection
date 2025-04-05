namespace SimpleCustomRoles;

public class Config
{
    public bool Debug { get; set; }
    public bool IsPaused { get; set; }
    public ushort SpectatorBroadcastTime { get; set; } = 7;
    public bool UsePlayerPercent { get; set; }
    public float SpawnRateMultiplier { get; set; } = 1f;
}
