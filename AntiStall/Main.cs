using Exiled.API.Features;
using PluginAPI.Core;
using System;

namespace AntiStall
{
    public class Main : Plugin<Config>
    {
        public static Main Instance { get; private set; }
        #region Plugin Info
        public override string Author => "SlejmUr";
        public override string Name => "AntiStall";
        public override string Prefix => "AntiStall";
        public override Version Version => new Version(0, 0, 1);
        #endregion

        public override void OnEnabled()
        {
            Instance = this;
            if (this.Config.IsEnabled)
            {
                Exiled.Events.Handlers.Player.Spawned += TheHandler.Spawned;
                Exiled.Events.Handlers.Player.Died += TheHandler.Died;
            }
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Instance = null;
            if (this.Config.IsEnabled)
            {
                Exiled.Events.Handlers.Player.Spawned -= TheHandler.Spawned;
                Exiled.Events.Handlers.Player.Died -= TheHandler.Died;
            }
            base.OnDisabled();
        }
    }
}
