using Exiled.Events.EventArgs.Item;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavaStats.Handlers
{
    internal class ItemHandler
    {
        public static void Swinging(SwingingEventArgs args)
        {
            if (args.Player.DoNotTrack && args.IsAllowed)
                return;
            var id = args.Player.UserId;
            var stat = Main.Instance.Statistic.GetStatForPlayer(id);
            stat.ItemStats.KeyCardInteractions++;
            Main.Instance.Statistic.AddStatForPlayer(id, stat);
        }

        public static void KeycardInteracting(KeycardInteractingEventArgs args)
        {
            if (args.Player.DoNotTrack && args.IsAllowed)
                return;
            var id = args.Player.UserId;
            var stat = Main.Instance.Statistic.GetStatForPlayer(id);
            stat.ItemStats.KeyCardInteractions++;
            Main.Instance.Statistic.AddStatForPlayer(id, stat);
        }
    }
}
