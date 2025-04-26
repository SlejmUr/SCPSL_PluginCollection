using PlayerRoles.PlayableScps.Scp1507;
using PlayerRoles.PlayableScps.Scp3114;
using PlayerRoles.PlayableScps.Scp939;
using PlayerStatsSystem;
using SimpleCustomRoles.RoleYaml;
using SimpleCustomRoles.RoleYaml.Enums;

namespace SimpleCustomRoles.Helpers;

public static class DamageHelper
{

    public static Dictionary<DamageSubType, Type> SubTypeToType = new()
    {
        { DamageSubType.None, null },
        { DamageSubType.UniversalSubType, typeof(DamageUniversalType) },
        { DamageSubType.WeaponType, typeof(ItemType) },
        { DamageSubType.AmmoType, typeof(ItemType) },
        { DamageSubType.Scp069_AttackType, typeof(Scp096DamageHandler.AttackType) },
        { DamageSubType.Scp049_AttackType, typeof(Scp049DamageHandler.AttackType) },
        { DamageSubType.MicroHidFiringMode, typeof(InventorySystem.Items.MicroHID.Modules.MicroHidFiringMode) },
        { DamageSubType.ExplosionType, typeof(ExplosionType) },
        { DamageSubType.Disruptor_FiringState, typeof(InventorySystem.Items.Firearms.Modules.DisruptorActionModule.FiringState) },
        { DamageSubType.Scp939_AttackType, typeof(Scp939DamageType) },
        { DamageSubType.Scp3114_AttackType, typeof(Scp3114DamageHandler.HandlerType) },
    };

    public static DamageType GetDamageType(this DamageHandlerBase handlerBase)
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

    public static float GetDamageValue(this DamageHandlerBase handlerBase)
    {
        if (handlerBase is StandardDamageHandler standardDamage)
        {
            return standardDamage.Damage;
        }
        return -1;
    }

    public static void SetDamageValue(this DamageHandlerBase handlerBase, float damage)
    {
        if (handlerBase is StandardDamageHandler standardDamage)
        {
            standardDamage.Damage = damage;
        }
    }

    public static object GetObjectBySubType(this DamageHandlerBase handlerBase, DamageSubType subType)
    {
        switch (handlerBase)
        {
            case FirearmDamageHandler firearm:
                {
                    if (subType == DamageSubType.AmmoType)
                        return firearm.AmmoType;
                    if (subType == DamageSubType.WeaponType)
                        return firearm.WeaponType;
                }
                return null;
            case Scp096DamageHandler scp096Damage:
                {
                    if (subType == DamageSubType.Scp069_AttackType)
                        return scp096Damage._attackType;
                }
                return null;
            case Scp049DamageHandler scp049Damage:
                {
                    if (subType == DamageSubType.Scp049_AttackType)
                        return scp049Damage.DamageSubType;
                }
                return null;
            case MicroHidDamageHandler microHidDamage:
                {
                    if (subType == DamageSubType.MicroHidFiringMode)
                        return microHidDamage.FiringMode;
                }
                return null;
            case ExplosionDamageHandler explosionDamage:
                {
                    if (subType == DamageSubType.ExplosionType)
                        return explosionDamage.ExplosionType;
                }
                return null;
            case DisruptorDamageHandler disruptorDamage:
                {
                    if (subType == DamageSubType.Disruptor_FiringState)
                        return disruptorDamage.FiringState;
                }
                return null;
            case Scp939DamageHandler scp939Damage:
                {
                    if (subType == DamageSubType.Scp939_AttackType)
                        return scp939Damage.Scp939DamageType;
                }
                return null;
            case Scp3114DamageHandler scp3114Damage:
                {
                    if (subType == DamageSubType.Scp939_AttackType)
                        return scp3114Damage.Subtype;
                }
                return null;
            case UniversalDamageHandler universalDamage:
                {
                    if (subType == DamageSubType.UniversalSubType)
                        return (DamageUniversalType)(int)universalDamage.TranslationId;
                }
                return null;
            default:
                return null;
        }
    }


    public static float CalculateDamage(this Dictionary<DamageMaker, MathValue> dict, DamageHandlerBase damageHandlerBase, float baseDamage, DamageType damageType)
    {
        float newDamage = baseDamage;
        var DamageTypeEnum = dict.Where(x => x.Key.DamageType == damageType);
        foreach (var item in
               DamageTypeEnum.Select(x => x.Key.DamageSubType))
        {
            var obj = GetObjectBySubType(damageHandlerBase, item);
            if (obj == null)
                continue;
            var sent = DamageTypeEnum.
                Where(x => x.Key.DamageSubType == item && x.Key.SubType.ToString() == obj.ToString());
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
