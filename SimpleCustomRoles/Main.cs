using Exiled.API.Features;
using Respawning;
using SimpleCustomRoles.Handler;
using SimpleCustomRoles.RoleInfo;
using System;
using System.Collections.Generic;

namespace SimpleCustomRoles
{
    internal class Main : Plugin<Config>
    {
        public static Main Instance { get; private set; }

        #region Plugin Info
        public override string Author => "SlejmUr";
        public override string Name => "SimpleCustomRoles";
        public override string Prefix => "SimpleCustomRoles";
        public override Version Version => new Version(0,2,0);
        #endregion

        public RolesLoader RolesLoader;
        public List<CustomRoleInfo> PlayersRolled;
        public List<CustomRoleInfo> SpawningRoles;
        public Dictionary<string, CustomRoleInfo> PlayerCustomRole;
        public List<CustomRoleInfo> AfterDeathRoles;
        public List<CustomRoleInfo> ScpSpecificRoles;

        public override void OnEnabled()
        {
            Instance = this;
            HelperTxts.WriteAll();
            RolesLoader = new RolesLoader();

            Exiled.Events.Handlers.Server.WaitingForPlayers += TheHandler.WaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundStarted += TheHandler.RoundStarted;
            Exiled.Events.Handlers.Player.Escaping += TheHandler.Escaping;
            Exiled.Events.Handlers.Player.Died += TheHandler.Died;
            Exiled.Events.Handlers.Scp049.FinishingRecall += SCP_049_Handler.FinishingRecall;
            Exiled.Events.Handlers.Scp0492.ConsumingCorpse += SCP_0492_Handler.ConsumingCorpse;
            Exiled.Events.Handlers.Scp096.AddingTarget += SCP_096_Handler.AddingTarget;
            Exiled.Events.Handlers.Player.UsingItem += TheHandler.UsingItem;
            Exiled.Events.Handlers.Player.DroppingItem += TheHandler.DroppingItem;
            Exiled.Events.Handlers.Item.ChargingJailbird += TheHandler.ChargingJailbird;

            RespawnManager.ServerOnRespawned += TheHandler.RespawnManager_ServerOnRespawned;


            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= TheHandler.WaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundStarted -= TheHandler.RoundStarted;
            Exiled.Events.Handlers.Player.Escaping -= TheHandler.Escaping;
            Exiled.Events.Handlers.Player.Died -= TheHandler.Died;
            Exiled.Events.Handlers.Scp049.FinishingRecall -= SCP_049_Handler.FinishingRecall;
            Exiled.Events.Handlers.Scp0492.ConsumingCorpse -= SCP_0492_Handler.ConsumingCorpse;
            Exiled.Events.Handlers.Scp096.AddingTarget -= SCP_096_Handler.AddingTarget;
            Exiled.Events.Handlers.Player.UsingItem -= TheHandler.UsingItem;
            Exiled.Events.Handlers.Player.DroppingItem -= TheHandler.DroppingItem;
            Exiled.Events.Handlers.Item.ChargingJailbird -= TheHandler.ChargingJailbird;

            RespawnManager.ServerOnRespawned -= TheHandler.RespawnManager_ServerOnRespawned;

            RolesLoader.Dispose();
            RolesLoader = null;
            Instance = null;

            base.OnDisabled();
        }
    }
}
