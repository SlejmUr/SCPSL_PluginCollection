using PlayerRoles.PlayableScps.Scp1507;
using PlayerRoles.PlayableScps.Scp3114;
using PlayerRoles.PlayableScps.Scp939;
using PlayerStatsSystem;
using SimpleCustomRoles.RoleInfo;
using static SimpleCustomRoles.Helpers.DamageHelper;
using static SimpleCustomRoles.RoleInfo.Damager;

namespace SimpleCustomRoles.Helpers;

public class DamageHelper
{
    public enum DamageType
    {
        None = 0,
        Recontainment,
        Firearm,
        Warhead,
        Universal,
        Scp,
        Scp096,
        Scp049,
        MicroHid,
        Custom,
        Explosion,
        Scp018,
        Disruptor,
        Jailbird,
        Scp939,
        Scp3114,
        Scp1507,
        Scp956,
        Snowball
    }

    public enum SubType
    {
        None = 0,
        UniversalSubType,
        WeaponType,
        AmmoType,
        Scp069_AttackType,
        Scp049_AttackType,
        MicroHidFiringMode,
        ExplosionType,
        Disruptor_FiringState,
        Scp939_AttackType,
        Scp3114_AttackType,
    }

    public enum UniversalSubType
    {
        None = -1,
        Recontained,
        Warhead,
        Scp049,
        Unknown,
        Asphyxiated,
        Bleeding,
        Falldown,
        PocketDecay,
        Decontamination,
        Poisoned,
        Scp207,
        SeveredHands,
        MicroHID,
        Tesla,
        Explosion,
        Scp096,
        Scp173,
        Scp939Lunge,
        Zombie,
        BulletWounds,
        Crushed,
        UsedAs106Bait,
        FriendlyFireDetector,
        Hypothermia,
        CardiacArrest,
        Scp939Other,
        Scp3114Slap,
        MarshmallowMan,
        Scp1344,
        Scp1507Peck,

    }

    public static Dictionary<SubType, Type> SubTypeToType = new()
    {
        { SubType.None, null },
        { SubType.UniversalSubType, typeof(UniversalSubType) },
        { SubType.WeaponType, typeof(ItemType) },
        { SubType.AmmoType, typeof(ItemType) },
        { SubType.Scp069_AttackType, typeof(Scp096DamageHandler.AttackType) },
        { SubType.Scp049_AttackType, typeof(Scp049DamageHandler.AttackType) },
        { SubType.MicroHidFiringMode, typeof(InventorySystem.Items.MicroHID.Modules.MicroHidFiringMode) },
        { SubType.ExplosionType, typeof(ExplosionType) },
        { SubType.Disruptor_FiringState, typeof(InventorySystem.Items.Firearms.Modules.DisruptorActionModule.FiringState) },
        { SubType.Scp939_AttackType, typeof(Scp939DamageType) },
        { SubType.Scp3114_AttackType, typeof(Scp3114DamageHandler.HandlerType) },
    };

    public static DamageType GetDamageType(DamageHandlerBase handlerBase)
    {
        switch (handlerBase)
        {
            case RecontainmentDamageHandler:
                return DamageType.Recontainment;
            case FirearmDamageHandler:
                return DamageType.Firearm;
            case WarheadDamageHandler:
                return DamageType.Warhead;
            case UniversalDamageHandler:
                return DamageType.Universal;
            case ScpDamageHandler scpDamage:
                {
                    return scpDamage switch
                    {
                        Scp096DamageHandler => DamageType.Scp096,
                        Scp049DamageHandler => DamageType.Scp049,
                        _ => DamageType.Scp,
                    };
                }
            case MicroHidDamageHandler:
                return DamageType.MicroHid;
            case CustomReasonDamageHandler:
                return DamageType.Custom;
            case ExplosionDamageHandler:
                return DamageType.Explosion;
            case Scp018DamageHandler:
                return DamageType.Scp018;
            case DisruptorDamageHandler:
                return DamageType.Disruptor;
            case JailbirdDamageHandler:
                return DamageType.Jailbird;
            case Scp939DamageHandler:
                return DamageType.Scp939;
            case Scp3114DamageHandler:
                return DamageType.Scp3114;
            case Scp1507DamageHandler:
                return DamageType.Scp1507;
            case Scp956DamageHandler:
                return DamageType.Scp956;
            case SnowballDamageHandler:
                return DamageType.Snowball;
            default:
                return DamageType.None;
        }
    }

    public static float GetDamageValue(DamageHandlerBase handlerBase)
    {
        if (handlerBase is StandardDamageHandler standardDamage)
        {
            return standardDamage.Damage;
        }
        return -1;
    }

    public static void SetDamageValue(DamageHandlerBase handlerBase, float damage)
    {
        if (handlerBase is StandardDamageHandler standardDamage)
        {
            standardDamage.Damage = damage;
        }
    }

    public static object GetObjectBySubType(DamageHandlerBase handlerBase, SubType subType)
    {
        switch (handlerBase)
        {
            case FirearmDamageHandler firearm:
                {
                    if (subType == SubType.AmmoType)
                        return firearm.AmmoType;
                    if (subType == SubType.WeaponType)
                        return firearm.WeaponType;
                }
                return null;
            case Scp096DamageHandler scp096Damage:
                {
                    if (subType == SubType.Scp069_AttackType)
                        return scp096Damage._attackType;
                }
                return null;
            case Scp049DamageHandler scp049Damage:
                {
                    if (subType == SubType.Scp049_AttackType)
                        return scp049Damage.DamageSubType;
                }
                return null;
            case MicroHidDamageHandler microHidDamage:
                {
                    if (subType == SubType.MicroHidFiringMode)
                        return microHidDamage.FiringMode;
                }
                return null;
            case ExplosionDamageHandler explosionDamage:
                {
                    if (subType == SubType.ExplosionType)
                        return explosionDamage.ExplosionType;
                }
                return null;
            case DisruptorDamageHandler disruptorDamage:
                {
                    if (subType == SubType.Disruptor_FiringState)
                        return disruptorDamage.FiringState;
                }
                return null;
            case Scp939DamageHandler scp939Damage:
                {
                    if (subType == SubType.Scp939_AttackType)
                        return scp939Damage.Scp939DamageType;
                }
                return null;
            case Scp3114DamageHandler scp3114Damage:
                {
                    if (subType == SubType.Scp939_AttackType)
                        return scp3114Damage.Subtype;
                }
                return null;
            case UniversalDamageHandler universalDamage:
                {
                    if (subType == SubType.UniversalSubType)
                        return (UniversalSubType)(int)universalDamage.TranslationId;
                }
                return null;
            default:
                return null;
        }
    }


    public static float CalculateDamage(Dictionary<DamageMaker, ValueSetter> dict, DamageHandlerBase damageHandlerBase, float baseDamage, DamageType damageType)
    {
        float newDamage = baseDamage;
        foreach (var item in dict.
               Where(x => x.Key.DamageType == damageType).
               Select(x => x.Key.DamageSubType))
        {
            var obj = GetObjectBySubType(damageHandlerBase, item);
            if (obj == null)
                continue;
            var sent = dict.
                Where(x => x.Key.DamageType == damageType && x.Key.DamageSubType == item).
                Where(x => x.Key.SubType.ToString() == obj.ToString());
            if (sent.Any())
            {
                var first = sent.First();
                if (first.Value == null)
                    continue;
                newDamage = first.Value.Math.MathWithFloat(newDamage, first.Value.Value);
            }
        }
        return newDamage;
    }
}
