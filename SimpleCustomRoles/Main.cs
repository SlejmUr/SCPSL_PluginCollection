using LabApi.Events.Handlers;
using LabApi.Features;
using LabApi.Loader.Features.Plugins;
using Respawning;
using SimpleCustomRoles.Handler;
using SimpleCustomRoles.RoleInfo;

namespace SimpleCustomRoles;

internal class Main : Plugin<Config>
{
    public static Main Instance { get; private set; }

    #region Plugin Info
    public override string Author => "SlejmUr";
    public override string Name => "SimpleCustomRoles";
    public override Version Version => new(0,4, 0);
    public override string Description => "Add simple YAML Support for creating custom roles.";
    public override Version RequiredApiVersion => new(LabApiProperties.CompiledVersion);
    #endregion
    public RolesLoader RolesLoader;
    public Dictionary<string, CustomRoleInfo> PlayerCustomRole;
    public List<CustomRoleInfo> RegularRoles;
    public List<CustomRoleInfo> InWaveRoles;
    public List<CustomRoleInfo> AfterDeathRoles;
    public List<CustomRoleInfo> SPC_SpecificRoles;
    public List<CustomRoleInfo> EscapeRoles;

    public override void Enable()
    {
        Instance = this;
        HelperTxts.WriteAll();
        RolesLoader = new RolesLoader();

        ServerEvents.WaitingForPlayers += CrateAndInit_Handler.WaitingForPlayers;
        ServerEvents.RoundRestarted += CrateAndInit_Handler.RoundStarted;

        PlayerEvents.Escaping += TheHandler.Escaping;
        PlayerEvents.Death += TheHandler.Died;
        PlayerEvents.UsingItem += TheHandler.UsingItem;
        PlayerEvents.DroppingItem += TheHandler.DroppingItem;
        PlayerEvents.ChangedSpectator += TheHandler.ChangingSpectatedPlayer;
        PlayerEvents.Hurting += TheHandler.Hurting;
        PlayerEvents.ChangingRole += TheHandler.ChangingRole;
        PlayerEvents.Dying += TheHandler.Dying;
        Scp049Events.ResurrectingBody += SCP_049_Handler.FinishingRecall;

        // LabAPI doesnt have consuming corpse as 049-2
        // Exiled.Events.Handlers.Scp0492.ConsumingCorpse += SCP_0492_Handler.ConsumingCorpse;

        Scp096Events.AddingTarget += SCP_096_Handler.AddingTarget;
        Scp096Events.TryingNotToCry += SCP_096_Handler.TryingNotToCry;
        Scp096Events.Charging += SCP_096_Handler.Charging;
        Scp096Events.Enraging += SCP_096_Handler.Enraging;
        Scp096Events.PryingGate += SCP_096_Handler.StartPryingGate;

        Scp173Events.CreatingTantrum += SCP_173_Handler.PlacingTantrum;
        Scp173Events.BreakneckSpeedChanging += SCP_173_Handler.UsingBreakneckSpeeds;

        /*
        Exiled.Events.Handlers.Scp330.InteractingScp330 += SCP_330_Handler.InteractingScp330;
        Exiled.Events.Handlers.Scp330.EatingScp330 += SCP_330_Handler.EatingScp330;
        Exiled.Events.Handlers.Scp330.DroppingScp330 += SCP_330_Handler.DroppingScp330;
        */
        WaveManager.OnWaveSpawned += TheHandler.RespawnManager_ServerOnRespawned;
    }

    public override void Disable()
    {
        ServerEvents.WaitingForPlayers -= CrateAndInit_Handler.WaitingForPlayers;
        ServerEvents.RoundRestarted -= CrateAndInit_Handler.RoundStarted;

        PlayerEvents.Escaping -= TheHandler.Escaping;
        PlayerEvents.Death -= TheHandler.Died;
        PlayerEvents.UsingItem -= TheHandler.UsingItem;
        PlayerEvents.DroppingItem -= TheHandler.DroppingItem;
        PlayerEvents.ChangedSpectator -= TheHandler.ChangingSpectatedPlayer;
        PlayerEvents.Hurting -= TheHandler.Hurting;
        PlayerEvents.ChangingRole -= TheHandler.ChangingRole;
        PlayerEvents.Dying -= TheHandler.Dying;
        Scp049Events.ResurrectingBody -= SCP_049_Handler.FinishingRecall;

        // LabAPI doesnt have consuming corpse as 049-2
        // Exiled.Events.Handlers.Scp0492.ConsumingCorpse += SCP_0492_Handler.ConsumingCorpse;

        Scp096Events.AddingTarget -= SCP_096_Handler.AddingTarget;
        Scp096Events.TryingNotToCry -= SCP_096_Handler.TryingNotToCry;
        Scp096Events.Charging -= SCP_096_Handler.Charging;
        Scp096Events.Enraging -= SCP_096_Handler.Enraging;
        Scp096Events.PryingGate -= SCP_096_Handler.StartPryingGate;

        Scp173Events.CreatingTantrum -= SCP_173_Handler.PlacingTantrum;
        Scp173Events.BreakneckSpeedChanging -= SCP_173_Handler.UsingBreakneckSpeeds;

        /*
        Exiled.Events.Handlers.Scp330.InteractingScp330 += SCP_330_Handler.InteractingScp330;
        Exiled.Events.Handlers.Scp330.EatingScp330 += SCP_330_Handler.EatingScp330;
        Exiled.Events.Handlers.Scp330.DroppingScp330 += SCP_330_Handler.DroppingScp330;
        */

        WaveManager.OnWaveSpawned -= TheHandler.RespawnManager_ServerOnRespawned;

        RolesLoader.Dispose();
        RolesLoader = null;
        Instance = null;
    }
}
