using CommandSystem;
using Exiled.API.Extensions;
using Exiled.API.Features;
using RemoteAdmin;
using System;
using System.Linq;

namespace SimpleCustomRoles.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class TempShowEffect : ICommand
    {
        public string Command => "showeffects";

        public string[] Aliases => new string[] { "showeffects", "geteffects", "scr_ef" };

        public string Description => "Show your current Effects";

        public bool SanitizeResponse => true;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (sender is PlayerCommandSender)
            {
                var pcs = sender as PlayerCommandSender;
                var player = Player.List.Where(x => x.UserId == pcs.SenderId).FirstOrDefault();
                if (player == null)
                {
                    response = "Must be coming from Player!";
                    return false;
                }
                response = "Your effects: \n";
                foreach (var effect in player.ActiveEffects)
                {
                    string effectName = EffectTypeExtension.GetEffectType(effect).ToString();
                    response += effectName + $" (d: {effect.Duration} i:{effect.Intensity})" + "\n";
                }
                return true;

            }
            response = "Must be coming from Player!";
            return false;
        }
    }
}
