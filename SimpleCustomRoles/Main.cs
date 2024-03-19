using Exiled.API.Features;
using Respawning;
using SimpleCustomRoles.Handler;
using SimpleCustomRoles.RoleInfo;
using System;

namespace SimpleCustomRoles
{
    internal class Main : Plugin<Config>
    {
        public static Main Instance { get; private set; }

        #region Plugin Info
        public override string Author => "SlejmUr";
        public override string Name => "SimpleCustomRoles";
        public override string Prefix => "SimpleCustomRoles";
        public override Version Version => new Version(0,1);
        #endregion

        public RolesLoader RolesLoader;

        public override void OnEnabled()
        {;
            Instance = this;
            HelperTxts.WriteAll();
            RolesLoader = new RolesLoader();

            Exiled.Events.Handlers.Server.WaitingForPlayers += TheHandler.WaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundStarted += TheHandler.RoundStarted;
            Exiled.Events.Handlers.Player.Escaping += TheHandler.Escaping;
            RespawnManager.ServerOnRespawned += TheHandler.RespawnManager_ServerOnRespawned;


            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= TheHandler.WaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundStarted -= TheHandler.RoundStarted;
            Exiled.Events.Handlers.Player.Escaping -= TheHandler.Escaping;
            RespawnManager.ServerOnRespawned -= TheHandler.RespawnManager_ServerOnRespawned;
            RolesLoader.Dispose();
            RolesLoader = null;
            Instance = null;

            base.OnDisabled();
        }
    }
}
