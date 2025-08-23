using LabApiExtensions.Extensions;
using PlayerStatsSystem;
using SimpleCustomRoles.RoleYaml;

namespace SimpleCustomRoles.Helpers;

public static class DamageHelper
{
    // TODO: Make log for this.

    public static float CalculateDamage(this Dictionary<DamageMaker, MathValueFloat> dict, DamageHandlerBase damageHandlerBase, float baseDamage, DamageType damageType)
    {
        float newDamage = baseDamage;
        var DamageTypeEnum = dict.Where(x => x.Key.DamageType == damageType);
        if (DamageTypeEnum.Count() == 0)
        {
            return newDamage;
        }


        if (DamageTypeEnum.Any(x => x.Key.DamageSubType is DamageSubType.None))
        {
            var damageFirst = DamageTypeEnum.FirstOrDefault();
            if (damageFirst.Value == default)
            {
                return newDamage;
            }
            return damageFirst.Value.MathCalculation(newDamage);
        }

        foreach (var item in DamageTypeEnum.Select(x => x.Key.DamageSubType))
        {
            var obj = damageHandlerBase.GetObjectBySubType(item);
            if (obj == null)
                continue;
            var sent = DamageTypeEnum.
                Where(x => x.Key.DamageSubType == item && x.Key.SubType.ToString() == obj.ToString());
            if (sent.Any())
            {
                var first = sent.First();
                if (first.Value == null)
                    continue;
                newDamage = first.Value.MathCalculation(newDamage);
            }
        }
        return newDamage;
    }
}
