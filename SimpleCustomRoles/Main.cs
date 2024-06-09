using Exiled.API.Features;
using Exiled.CustomRoles;
using Respawning;
using SimpleCustomRoles.Handler;
using SimpleCustomRoles.RoleInfo;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace SimpleCustomRoles
{
    internal class Main : Plugin<Config>
    {
        public static Main Instance { get; private set; }

        #region Plugin Info
        public override string Author => "SlejmUr";
        public override string Name => "SimpleCustomRoles";
        public override string Prefix => "SimpleCustomRoles";
        public override Version Version => new Version(0,2,5);
        #endregion

        public RolesLoader RolesLoader;
        public List<CustomRoleInfo> PlayersRolled;
        public List<CustomRoleInfo> SpawningRoles;
        public Dictionary<string, CustomRoleInfo> PlayerCustomRole;
        public List<CustomRoleInfo> AfterDeathRoles;
        public List<CustomRoleInfo> ScpSpecificRoles;
        //sList<Exiled.CustomRoles.API.Features.CustomRole> customRoles = new List<Exiled.CustomRoles.API.Features.CustomRole>();
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
            Exiled.Events.Handlers.Player.ChangingSpectatedPlayer += TheHandler.ChangingSpectatedPlayer;

            Exiled.Events.Handlers.Scp330.InteractingScp330 += SCP_330_Handler.InteractingScp330;
            Exiled.Events.Handlers.Scp330.EatingScp330 += SCP_330_Handler.EatingScp330;
            Exiled.Events.Handlers.Scp330.DroppingScp330 += SCP_330_Handler.DroppingScp330;

            Exiled.Events.Handlers.Player.Hurting += TheHandler.Hurting;

            RespawnManager.ServerOnRespawned += TheHandler.RespawnManager_ServerOnRespawned;

            //customRoles.Add(new Bomber_Zombie());
            //Exiled.CustomRoles.API.Extensions.Register(customRoles);

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
            Exiled.Events.Handlers.Player.ChangingSpectatedPlayer -= TheHandler.ChangingSpectatedPlayer;

            Exiled.Events.Handlers.Scp330.InteractingScp330 -= SCP_330_Handler.InteractingScp330;
            Exiled.Events.Handlers.Scp330.EatingScp330 -= SCP_330_Handler.EatingScp330;

            Exiled.Events.Handlers.Player.Hurting -= TheHandler.Hurting;

            RespawnManager.ServerOnRespawned -= TheHandler.RespawnManager_ServerOnRespawned;

            RolesLoader.Dispose();
            RolesLoader = null;
            Instance = null;

            //Exiled.CustomRoles.API.Extensions.Unregister(customRoles);

            base.OnDisabled();
        }
    }
}
