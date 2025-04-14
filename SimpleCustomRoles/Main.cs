using Exiled.API.Features;
using MEC;
using Respawning;
using SimpleCustomRoles.Handler;
using SimpleCustomRoles.RoleInfo;
using SimpleCustomRoles.SSS;
using System;
using System.Collections.Generic;

namespace SimpleCustomRoles;

internal class Main : Plugin<Config>
{
    public static Main Instance { get; private set; }

    #region Plugin Info
    public override string Author => "SlejmUr";
    public override string Name => "SimpleCustomRoles";
    public override string Prefix => "SimpleCustomRoles";
    public override Version Version => new(0,3, 1);
    #endregion

    public RolesLoader RolesLoader;
    public Dictionary<string, CustomRoleInfo> PlayerCustomRole;
    public List<CustomRoleInfo> RegularRoles;
    public List<CustomRoleInfo> InWaveRoles;
    public List<CustomRoleInfo> AfterDeathRoles;
    public List<CustomRoleInfo> SPC_SpecificRoles;
    public List<CustomRoleInfo> EscapeRoles;
    public override void OnEnabled()
    {
        Instance = this;
        HelperTxts.WriteAll();
        RolesLoader = new RolesLoader();
        Timing.CallDelayed(5, Logic.Init);

        Exiled.Events.Handlers.Server.WaitingForPlayers += CreateAndInit_Handler.WaitingForPlayers;
        Exiled.Events.Handlers.Server.RoundStarted += CreateAndInit_Handler.RoundStarted;

        Exiled.Events.Handlers.Player.Escaping += TheHandler.Escaping;
        Exiled.Events.Handlers.Player.Died += TheHandler.Died;
        Exiled.Events.Handlers.Player.UsingItem += TheHandler.UsingItem;
        Exiled.Events.Handlers.Player.DroppingItem += TheHandler.DroppingItem;
        Exiled.Events.Handlers.Player.ChangingSpectatedPlayer += TheHandler.ChangingSpectatedPlayer;
        Exiled.Events.Handlers.Player.Hurting += TheHandler.Hurting;
        Exiled.Events.Handlers.Player.ChangingRole += TheHandler.ChangingRole;
        Exiled.Events.Handlers.Player.Dying += TheHandler.Dying;

        Exiled.Events.Handlers.Scp049.FinishingRecall += SCP_049_Handler.FinishingRecall;

        Exiled.Events.Handlers.Scp0492.ConsumingCorpse += SCP_0492_Handler.ConsumingCorpse;

        Exiled.Events.Handlers.Scp096.AddingTarget += SCP_096_Handler.AddingTarget;
        Exiled.Events.Handlers.Scp096.TryingNotToCry += SCP_096_Handler.TryingNotToCry;
        Exiled.Events.Handlers.Scp096.Charging += SCP_096_Handler.Charging;
        Exiled.Events.Handlers.Scp096.Enraging += SCP_096_Handler.Enraging;
        Exiled.Events.Handlers.Scp096.StartPryingGate += SCP_096_Handler.StartPryingGate;

        Exiled.Events.Handlers.Scp173.PlacingTantrum += SCP_173_Handler.PlacingTantrum;
        Exiled.Events.Handlers.Scp173.UsingBreakneckSpeeds += SCP_173_Handler.UsingBreakneckSpeeds;

        Exiled.Events.Handlers.Scp330.InteractingScp330 += SCP_330_Handler.InteractingScp330;
        Exiled.Events.Handlers.Scp330.EatingScp330 += SCP_330_Handler.EatingScp330;
        Exiled.Events.Handlers.Scp330.DroppingScp330 += SCP_330_Handler.DroppingScp330;

        WaveManager.OnWaveSpawned += CreateAndInit_Handler.RespawnManager_ServerOnRespawned;

        base.OnEnabled();
    }

    public override void OnDisabled()
    {

        Exiled.Events.Handlers.Server.WaitingForPlayers -= CreateAndInit_Handler.WaitingForPlayers;
        Exiled.Events.Handlers.Server.RoundStarted -= CreateAndInit_Handler.RoundStarted;

        Exiled.Events.Handlers.Player.Escaping -= TheHandler.Escaping;
        Exiled.Events.Handlers.Player.Died -= TheHandler.Died;
        Exiled.Events.Handlers.Player.UsingItem -= TheHandler.UsingItem;
        Exiled.Events.Handlers.Player.DroppingItem -= TheHandler.DroppingItem;
        Exiled.Events.Handlers.Player.ChangingSpectatedPlayer -= TheHandler.ChangingSpectatedPlayer;
        Exiled.Events.Handlers.Player.Hurting -= TheHandler.Hurting;
        Exiled.Events.Handlers.Player.ChangingRole -= TheHandler.ChangingRole;
        Exiled.Events.Handlers.Player.Dying -= TheHandler.Dying;

        Exiled.Events.Handlers.Scp049.FinishingRecall -= SCP_049_Handler.FinishingRecall;

        Exiled.Events.Handlers.Scp0492.ConsumingCorpse -= SCP_0492_Handler.ConsumingCorpse;

        Exiled.Events.Handlers.Scp096.AddingTarget -= SCP_096_Handler.AddingTarget;
        Exiled.Events.Handlers.Scp096.TryingNotToCry -= SCP_096_Handler.TryingNotToCry;
        Exiled.Events.Handlers.Scp096.Charging -= SCP_096_Handler.Charging;
        Exiled.Events.Handlers.Scp096.Enraging -= SCP_096_Handler.Enraging;
        Exiled.Events.Handlers.Scp096.StartPryingGate -= SCP_096_Handler.StartPryingGate;

        Exiled.Events.Handlers.Scp173.PlacingTantrum -= SCP_173_Handler.PlacingTantrum;
        Exiled.Events.Handlers.Scp173.UsingBreakneckSpeeds -= SCP_173_Handler.UsingBreakneckSpeeds;;

        Exiled.Events.Handlers.Scp330.InteractingScp330 -= SCP_330_Handler.InteractingScp330;
        Exiled.Events.Handlers.Scp330.EatingScp330 -= SCP_330_Handler.EatingScp330;
        Exiled.Events.Handlers.Scp330.DroppingScp330 -= SCP_330_Handler.DroppingScp330;

        WaveManager.OnWaveSpawned -= CreateAndInit_Handler.RespawnManager_ServerOnRespawned;

        Logic.UnInit();
        RolesLoader.Dispose();
        RolesLoader = null;
        Instance = null;

        base.OnDisabled();
    }
}
