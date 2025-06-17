using LabApi.Events.Handlers;
using LabApi.Features;
using LabApi.Loader.Features.Plugins;

namespace RemoteKC;

internal class Main : Plugin
{
    #region Plugin Info
    public override string Author => "SlejmUr";
    public override string Name => "RemoteKC";
    public override Version Version => new(0, 0, 1);
    public override string Description => "Adding Remote Keycard.";
    public override Version RequiredApiVersion => new(LabApiProperties.CompiledVersion);
    #endregion

    public override void Enable()
    {
        PlayerEvents.InteractingDoor += Events.InteractingDoor;
        PlayerEvents.UnlockingGenerator += Events.UnlockingGenerator;
        PlayerEvents.UnlockingWarheadButton += Events.UnlockingWarheadButton;
        PlayerEvents.InteractingLocker += Events.InteractingLocker;
    }

    public override void Disable()
    {
        PlayerEvents.InteractingDoor -= Events.InteractingDoor;
        PlayerEvents.UnlockingGenerator -= Events.UnlockingGenerator;
        PlayerEvents.UnlockingWarheadButton -= Events.UnlockingWarheadButton;
        PlayerEvents.InteractingLocker -= Events.InteractingLocker;
    }
}
