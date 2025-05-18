using LabApi.Features;
using LabApi.Loader.Features.Plugins;

namespace AntiStall;

public class Main : Plugin<Config>
{
    public static Main Instance { get; private set; }
    #region Plugin Info
    public override string Author => "SlejmUr";
    public override string Name => "AntiStall";
    public override Version Version => new Version(0, 0, 1);

    public override string Description => "Makes player not stalling in one place";

    public override Version RequiredApiVersion => new(LabApiProperties.CompiledVersion);
    #endregion


    public override void Enable()
    {

    }

    public override void Disable()
    {

    }
}
