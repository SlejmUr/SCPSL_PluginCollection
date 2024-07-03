using DavaStats.DataBase;
using DavaStats.Handlers;
using Exiled.API.Features;
using System;

namespace DavaStats
{
    internal class Main : Plugin<Config>
    {
        public static Main Instance { get; private set; }
        #region Plugin Info
        public override string Author => "SlejmUr";
        public override string Name => "DavaStats";
        public override string Prefix => "DavaStats";
        public override Version Version => new Version(0,2,3);
        #endregion
        public IDatabase Database { get; private set; }
        public StatisticStuff Statistic { get; private set; }
        public override void OnEnabled()
        {
            Instance = this;

            Database = new NoDB();
            if (Config.UseFileStorage)
                Database = new FileDB();
            if (Config.UseLiteDBStorage)
                Database = new SQLDB();

            Statistic = new StatisticStuff();
            Statistic.AllStats = Database.Load();

            Exiled.Events.Handlers.Server.WaitingForPlayers += ServerHandler.WaitingForPlayers;
            Exiled.Events.Handlers.Server.EndingRound += ServerHandler.EndingRound;
            Exiled.Events.Handlers.Server.RoundStarted += ServerHandler.RoundStarted;
            Exiled.Events.Handlers.Server.RespawningTeam += ServerHandler.RespawningTeam;

            Exiled.Events.Handlers.Warhead.ChangingLeverStatus += WarheadHandler.ChangingLeverStatus;
            Exiled.Events.Handlers.Warhead.Starting += WarheadHandler.Starting;
            Exiled.Events.Handlers.Warhead.Stopping += WarheadHandler.Stopping;

            Exiled.Events.Handlers.Player.ActivatingGenerator += PlayerEventsHandler.ActivatingGenerator;
            Exiled.Events.Handlers.Player.Died += PlayerEventsHandler.Died;
            Exiled.Events.Handlers.Player.EnteringPocketDimension += PlayerEventsHandler.EnteringPocketDimension;
            Exiled.Events.Handlers.Player.EscapingPocketDimension += PlayerEventsHandler.EscapingPocketDimension;
            Exiled.Events.Handlers.Player.Escaping += PlayerEventsHandler.Escaping;
            Exiled.Events.Handlers.Player.FailingEscapePocketDimension += PlayerEventsHandler.FailingEscapePocketDimension; 
            Exiled.Events.Handlers.Player.FlippingCoin += PlayerEventsHandler.FlippingCoin;
            Exiled.Events.Handlers.Player.Handcuffing += PlayerEventsHandler.Handcuffing;
            Exiled.Events.Handlers.Player.Hurt += PlayerEventsHandler.Hurt;
            Exiled.Events.Handlers.Player.IntercomSpeaking += PlayerEventsHandler.IntercomSpeaking;
            Exiled.Events.Handlers.Player.ItemAdded += PlayerEventsHandler.ItemAdded;
            Exiled.Events.Handlers.Player.ItemRemoved += PlayerEventsHandler.ItemRemoved;
            Exiled.Events.Handlers.Player.MakingNoise += PlayerEventsHandler.MakingNoise;
            Exiled.Events.Handlers.Player.ReceivingEffect += PlayerEventsHandler.ReceivingEffect;
            Exiled.Events.Handlers.Player.RemovingHandcuffs += PlayerEventsHandler.RemovingHandcuffs;
            Exiled.Events.Handlers.Player.StoppingGenerator += PlayerEventsHandler.StoppingGenerator;
            Exiled.Events.Handlers.Player.ThrownProjectile += PlayerEventsHandler.ThrownProjectile;
            Exiled.Events.Handlers.Player.TriggeringTesla += PlayerEventsHandler.TriggeringTesla;
            Exiled.Events.Handlers.Player.UsingItemCompleted += PlayerEventsHandler.UsingItemCompleted;

            Exiled.Events.Handlers.Item.KeycardInteracting += ItemHandler.KeycardInteracting;
            Exiled.Events.Handlers.Item.Swinging += ItemHandler.Swinging;

            Exiled.Events.Handlers.Scp049.SendingCall += SCP_049_Handler.SendingCall;
            Exiled.Events.Handlers.Scp049.ActivatingSense += SCP_049_Handler.ActivatingSense;
            Exiled.Events.Handlers.Scp049.FinishingRecall += SCP_049_Handler.FinishingRecall;
            Exiled.Events.Handlers.Scp049.Attacking += SCP_049_Handler.Attacking;

            Exiled.Events.Handlers.Scp0492.ConsumedCorpse += SCP_0492_Handler.ConsumedCorpse;

            Exiled.Events.Handlers.Scp079.ChangingCamera += SCP_079_Handler.ChangingCamera;
            Exiled.Events.Handlers.Scp079.ChangingSpeakerStatus += SCP_079_Handler.ChangingSpeakerStatus;
            Exiled.Events.Handlers.Scp079.ElevatorTeleporting += SCP_079_Handler.ElevatorTeleporting;
            Exiled.Events.Handlers.Scp079.GainingExperience += SCP_079_Handler.GainingExperience;
            Exiled.Events.Handlers.Scp079.GainingLevel += SCP_079_Handler.GainingLevel;
            Exiled.Events.Handlers.Scp079.InteractingTesla += SCP_079_Handler.InteractingTesla;
            Exiled.Events.Handlers.Scp079.LockingDown += SCP_079_Handler.LockingDown;
            Exiled.Events.Handlers.Scp079.Pinging += SCP_079_Handler.Pinging;
            Exiled.Events.Handlers.Scp079.Recontained += SCP_079_Handler.Recontained;
            Exiled.Events.Handlers.Scp079.RoomBlackout += SCP_079_Handler.RoomBlackout;
            Exiled.Events.Handlers.Scp079.TriggeringDoor += SCP_079_Handler.TriggeringDoor;
            Exiled.Events.Handlers.Scp079.ZoneBlackout += SCP_079_Handler.ZoneBlackout;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {

            Exiled.Events.Handlers.Server.WaitingForPlayers -= ServerHandler.WaitingForPlayers;
            Exiled.Events.Handlers.Server.EndingRound -= ServerHandler.EndingRound;
            Exiled.Events.Handlers.Server.RoundStarted -= ServerHandler.RoundStarted;
            Exiled.Events.Handlers.Server.RespawningTeam -= ServerHandler.RespawningTeam;

            Exiled.Events.Handlers.Warhead.ChangingLeverStatus -= WarheadHandler.ChangingLeverStatus;
            Exiled.Events.Handlers.Warhead.Starting -= WarheadHandler.Starting;
            Exiled.Events.Handlers.Warhead.Stopping -= WarheadHandler.Stopping;

            Exiled.Events.Handlers.Player.ActivatingGenerator -= PlayerEventsHandler.ActivatingGenerator;
            Exiled.Events.Handlers.Player.Died -= PlayerEventsHandler.Died;
            Exiled.Events.Handlers.Player.EnteringPocketDimension -= PlayerEventsHandler.EnteringPocketDimension;
            Exiled.Events.Handlers.Player.EscapingPocketDimension -= PlayerEventsHandler.EscapingPocketDimension;
            Exiled.Events.Handlers.Player.Escaping -= PlayerEventsHandler.Escaping;
            Exiled.Events.Handlers.Player.FailingEscapePocketDimension -= PlayerEventsHandler.FailingEscapePocketDimension;
            Exiled.Events.Handlers.Player.FlippingCoin -= PlayerEventsHandler.FlippingCoin;
            Exiled.Events.Handlers.Player.Handcuffing -= PlayerEventsHandler.Handcuffing;
            Exiled.Events.Handlers.Player.Hurt -= PlayerEventsHandler.Hurt;
            Exiled.Events.Handlers.Player.IntercomSpeaking -= PlayerEventsHandler.IntercomSpeaking;
            Exiled.Events.Handlers.Player.ItemAdded -= PlayerEventsHandler.ItemAdded;
            Exiled.Events.Handlers.Player.ItemRemoved -= PlayerEventsHandler.ItemRemoved;
            Exiled.Events.Handlers.Player.MakingNoise -= PlayerEventsHandler.MakingNoise;
            Exiled.Events.Handlers.Player.ReceivingEffect -= PlayerEventsHandler.ReceivingEffect;
            Exiled.Events.Handlers.Player.RemovingHandcuffs -= PlayerEventsHandler.RemovingHandcuffs;
            Exiled.Events.Handlers.Player.StoppingGenerator -= PlayerEventsHandler.StoppingGenerator;
            Exiled.Events.Handlers.Player.ThrownProjectile -= PlayerEventsHandler.ThrownProjectile;
            Exiled.Events.Handlers.Player.TriggeringTesla -= PlayerEventsHandler.TriggeringTesla;
            Exiled.Events.Handlers.Player.UsingItemCompleted -= PlayerEventsHandler.UsingItemCompleted;

            Statistic = null;
            Database = null;
            Instance = null;


            base.OnDisabled();
        }
    }
}
