using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;

namespace DavaStats.Handlers
{
    internal class PlayerEventsHandler
    {
        #region PockedDim
        public static void EnteringPocketDimension(EnteringPocketDimensionEventArgs enteringPocketDimensionEventArgs)
        {
            if (enteringPocketDimensionEventArgs.Player.DoNotTrack)
                return;
            var id = enteringPocketDimensionEventArgs.Player.UserId;
            var stat = Main.Instance.Statistic.GetStatForPlayer(id);
            stat.PlayerStat.PockedDim.EnteredCount++;
            Main.Instance.Statistic.AddStatForPlayer(id, stat);
        }
        public static void EscapingPocketDimension(EscapingPocketDimensionEventArgs escapingPocketDimensionEventArgs)
        {
            if (escapingPocketDimensionEventArgs.Player.DoNotTrack)
                return;
            var id = escapingPocketDimensionEventArgs.Player.UserId;
            var stat = Main.Instance.Statistic.GetStatForPlayer(id);
            stat.PlayerStat.PockedDim.EscapedCount++;
            Main.Instance.Statistic.AddStatForPlayer(id, stat);
        }
        public static void FailingEscapePocketDimension(FailingEscapePocketDimensionEventArgs failingEscapePocketDimensionEventArgs)
        {
            if (failingEscapePocketDimensionEventArgs.Player.DoNotTrack)
                return;
            var id = failingEscapePocketDimensionEventArgs.Player.UserId;
            var stat = Main.Instance.Statistic.GetStatForPlayer(id);
            stat.PlayerStat.PockedDim.DiedCount++;
            Main.Instance.Statistic.AddStatForPlayer(id, stat);
        }
        #endregion
        #region Generator
        public static void ActivatingGenerator(ActivatingGeneratorEventArgs activatingGeneratorEventArgs)
        {
            if (activatingGeneratorEventArgs.Player.DoNotTrack)
                return;
            var id = activatingGeneratorEventArgs.Player.UserId;
            var stat = Main.Instance.Statistic.GetStatForPlayer(id);
            stat.PlayerStat.Gen.GenActivated++;
            Main.Instance.Statistic.AddStatForPlayer(id, stat);
        }
        public static void StoppingGenerator(StoppingGeneratorEventArgs stoppingGeneratorEventArgs)
        {
            if (stoppingGeneratorEventArgs.Player.DoNotTrack)
                return;
            var id = stoppingGeneratorEventArgs.Player.UserId;
            var stat = Main.Instance.Statistic.GetStatForPlayer(id);
            stat.PlayerStat.Gen.GenDeactivated++;
            Main.Instance.Statistic.AddStatForPlayer(id, stat);
        }
        #endregion
        #region Funny
        public static void FlippingCoin(FlippingCoinEventArgs flippingCoinEventArgs)
        {
            if (flippingCoinEventArgs.Player.DoNotTrack)
                return;
            var id = flippingCoinEventArgs.Player.UserId;
            var stat = Main.Instance.Statistic.GetStatForPlayer(id);
            stat.PlayerStat.CoinFlip.CoinflipCount++;
            if (flippingCoinEventArgs.IsTails)
            {
                stat.PlayerStat.CoinFlip.CoinflipTails++;
            }
            else
            {
                stat.PlayerStat.CoinFlip.CoinflipHeads++;
            }
            Main.Instance.Statistic.AddStatForPlayer(id, stat);
        }
        public static void IntercomSpeaking(IntercomSpeakingEventArgs intercomSpeakingEventArgs)
        {
            if (intercomSpeakingEventArgs.Player.DoNotTrack)
                return;
            var id = intercomSpeakingEventArgs.Player.UserId;
            var stat = Main.Instance.Statistic.GetStatForPlayer(id);
            stat.PlayerStat.Funny.IntercomSpeaking++;
            Main.Instance.Statistic.AddStatForPlayer(id, stat);

        }
        public static void MakingNoise(MakingNoiseEventArgs makingNoiseEventArgs)
        {
            if (makingNoiseEventArgs.Player.DoNotTrack)
                return;
            var id = makingNoiseEventArgs.Player.UserId;
            var stat = Main.Instance.Statistic.GetStatForPlayer(id);
            stat.PlayerStat.Funny.MadeNoise++;
            Main.Instance.Statistic.AddStatForPlayer(id, stat);
        }
        public static void ReceivingEffect(ReceivingEffectEventArgs receivingEffectEventArgs)
        {
            if (receivingEffectEventArgs == null)
                return;
            if (receivingEffectEventArgs.Player.DoNotTrack)
                return;
            if (!receivingEffectEventArgs.Player.IsAlive)
                return;
            var classification = receivingEffectEventArgs.Effect.Classification;
            var id = receivingEffectEventArgs.Player.UserId;
            var stat = Main.Instance.Statistic.GetStatForPlayer(id);
            if (!stat.PlayerStat.DictStats.TimesOfEffect.TryGetValue(classification, out var value))
            {
                stat.PlayerStat.DictStats.TimesOfEffect.Add(classification, 0);
            }
            stat.PlayerStat.DictStats.TimesOfEffect[classification]++;
            Main.Instance.Statistic.AddStatForPlayer(id, stat);
        }
        public static void TriggeringTesla(TriggeringTeslaEventArgs triggeringTeslaEventArgs)
        {
            if (triggeringTeslaEventArgs.Player.DoNotTrack)
                return;
            if (!triggeringTeslaEventArgs.IsTriggerable)
                return;
            var id = triggeringTeslaEventArgs.Player.UserId;
            var stat = Main.Instance.Statistic.GetStatForPlayer(id);
            stat.PlayerStat.Funny.TeslaTrigger++;
            Main.Instance.Statistic.AddStatForPlayer(id, stat);
        }
        public static void ThrownProjectile(ThrownProjectileEventArgs thrownProjectileEventArgs)
        {
            if (thrownProjectileEventArgs.Player.DoNotTrack)
                return;
            var projectileType = thrownProjectileEventArgs.Projectile.ProjectileType;
            var id = thrownProjectileEventArgs.Player.UserId;
            var stat = Main.Instance.Statistic.GetStatForPlayer(id);
            if (!stat.PlayerStat.DictStats.ProjectileThrown.TryGetValue(projectileType, out var value))
            {
                stat.PlayerStat.DictStats.ProjectileThrown.Add(projectileType, 0);
            }
            stat.PlayerStat.DictStats.ProjectileThrown[projectileType]++;
            Main.Instance.Statistic.AddStatForPlayer(id, stat);
        }
        #endregion
        #region Cuff
        public static void Handcuffing(HandcuffingEventArgs handcuffingEventArgs)
        {
            if (handcuffingEventArgs.Target.DoNotTrack)
                return;
            var id = handcuffingEventArgs.Target.UserId;
            var stat = Main.Instance.Statistic.GetStatForPlayer(id);
            stat.PlayerStat.Cuff.Cuffed++;
            Main.Instance.Statistic.AddStatForPlayer(id, stat);
        }
        public static void RemovingHandcuffs(RemovingHandcuffsEventArgs removingHandcuffsEventArgs)
        {
            if (removingHandcuffsEventArgs.Target.DoNotTrack)
                return;
            var id = removingHandcuffsEventArgs.Target.UserId;
            var stat = Main.Instance.Statistic.GetStatForPlayer(id);
            stat.PlayerStat.Cuff.Uncuffed++;
            Main.Instance.Statistic.AddStatForPlayer(id, stat);
        }
        #endregion
        #region ItemStuff
        public static void ItemAdded(ItemAddedEventArgs itemAddedEventArgs)
        {
            if (itemAddedEventArgs.Player.DoNotTrack)
                return;
            var itemType = itemAddedEventArgs.Item.Type;
            var id = itemAddedEventArgs.Player.UserId;
            var stat = Main.Instance.Statistic.GetStatForPlayer(id);
            if (!stat.PlayerStat.DictStats.ItemAdded.TryGetValue(itemType, out var value))
            {
                stat.PlayerStat.DictStats.ItemAdded.Add(itemType, 0);
            }
            stat.PlayerStat.DictStats.ItemAdded[itemType]++;
            Main.Instance.Statistic.AddStatForPlayer(id, stat);

        }
        public static void ItemRemoved(ItemRemovedEventArgs itemRemovedEventArgs)
        {
            if (itemRemovedEventArgs.Player.DoNotTrack)
                return;
            var itemType = itemRemovedEventArgs.Item.Type;
            var id = itemRemovedEventArgs.Player.UserId;
            var stat = Main.Instance.Statistic.GetStatForPlayer(id);
            if (!stat.PlayerStat.DictStats.ItemRemoved.TryGetValue(itemType, out var value))
            {
                stat.PlayerStat.DictStats.ItemRemoved.Add(itemType, 0);
            }
            stat.PlayerStat.DictStats.ItemRemoved[itemType]++;
            Main.Instance.Statistic.AddStatForPlayer(id, stat);
        }

        public static void UsingItemCompleted(UsingItemCompletedEventArgs usingItemCompletedEventArgs)
        {
            if (usingItemCompletedEventArgs.Player.DoNotTrack)
                return;
            var itemType = usingItemCompletedEventArgs.Item.Type;
            var id = usingItemCompletedEventArgs.Player.UserId;
            var stat = Main.Instance.Statistic.GetStatForPlayer(id);
            if (!stat.PlayerStat.DictStats.ItemUsed.TryGetValue(itemType, out var value))
            {
                stat.PlayerStat.DictStats.ItemUsed.Add(itemType, 0);
            }
            stat.PlayerStat.DictStats.ItemUsed[itemType]++;
            Main.Instance.Statistic.AddStatForPlayer(id, stat);
        }
        #endregion
        #region Damage
        public static void Hurt(HurtEventArgs hurtEventArgs)
        {
            if (hurtEventArgs.Player.DoNotTrack)
                return;
            if (hurtEventArgs.Attacker == null)
            {
                return;
            }
            if (hurtEventArgs.Attacker.DoNotTrack)
                return;
            if (hurtEventArgs.HandlerOutput == PlayerStatsSystem.DamageHandlerBase.HandlerOutput.Damaged || hurtEventArgs.HandlerOutput == PlayerStatsSystem.DamageHandlerBase.HandlerOutput.Death)
            {
                //atk
                var AttackerId = hurtEventArgs.Attacker.UserId;
                var stat = Main.Instance.Statistic.GetStatForPlayer(AttackerId);
                stat.PlayerStat.Usual.DamageDone += hurtEventArgs.Amount;
                Main.Instance.Statistic.AddStatForPlayer(AttackerId, stat);

                //whogotdmg
                var DmgReceiver = hurtEventArgs.Player.UserId;
                stat = Main.Instance.Statistic.GetStatForPlayer(DmgReceiver);
                stat.PlayerStat.Usual.DamageReceived += hurtEventArgs.Amount;
                Main.Instance.Statistic.AddStatForPlayer(DmgReceiver, stat);
            }
        }
        #endregion
        #region Usual / Die/Escape
        public static void Died(DiedEventArgs diedEventArgs)
        {
            if (diedEventArgs.Player.DoNotTrack)
                return;
            var damageType = diedEventArgs.DamageHandler.Type;
            var id = diedEventArgs.Player.UserId;
            var stat = Main.Instance.Statistic.GetStatForPlayer(id);
            if (!stat.PlayerStat.DictStats.DeathBy.TryGetValue(damageType, out var value))
            {
                stat.PlayerStat.DictStats.DeathBy.Add(damageType, 0);
            }
            stat.PlayerStat.DictStats.DeathBy[damageType]++;
            stat.PlayerStat.Usual.DiedTimes++;
            Main.Instance.Statistic.AddStatForPlayer(id, stat);

        }

        public static void Escaping(EscapingEventArgs escapingEventArgs)
        {
            if (escapingEventArgs.Player.DoNotTrack)
                return;
            var escape = escapingEventArgs.EscapeScenario;
            var id = escapingEventArgs.Player.UserId;
            var stat = Main.Instance.Statistic.GetStatForPlayer(id);
            if (!stat.PlayerStat.DictStats.EscapedAs.TryGetValue(escape, out var value))
            {
                stat.PlayerStat.DictStats.EscapedAs.Add(escape, 0);
            }
            stat.PlayerStat.DictStats.EscapedAs[escape]++;
            stat.PlayerStat.Usual.EscapedTimes++;
            Main.Instance.Statistic.AddStatForPlayer(id, stat);
        }
        #endregion

    }
}
