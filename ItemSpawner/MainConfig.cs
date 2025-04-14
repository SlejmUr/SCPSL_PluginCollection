using Exiled.API.Interfaces;

namespace ItemSpawner;

public class MainConfig : IConfig
{
    public bool IsEnabled { get; set; } = true;
    public bool Debug { get; set; }
}
