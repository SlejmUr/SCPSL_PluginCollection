using HarmonyLib;
using LabApi.Features.Wrappers;
using PlayerRoles.PlayableScps.Scp049;
using PlayerRoles.Subroutines;
using SimpleCustomRoles.Helpers;
using System.Reflection.Emit;
using static HarmonyLib.AccessTools;

namespace SimpleCustomRoles.Patches;

[HarmonyPatch(typeof(Scp049AttackAbility), nameof(Scp049AttackAbility.CanFindTarget))]
[HarmonyPatch(typeof(Scp049AttackAbility), nameof(Scp049AttackAbility.IsTargetValid))]
internal static class Scp049AttackAbility_Distance
{
    internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        List<CodeInstruction> code = [.. instructions];
        // get the current value.
        var index = code.FindIndex(x => x.opcode == OpCodes.Ldc_R4);
        var inst = code[index];
        var const_value = inst.operand;

        // add after the field loaded
        code.InsertRange(index, [
            // this.Owner
            new(OpCodes.Ldarg_0),
            new(OpCodes.Callvirt, PropertyGetter(typeof(StandardSubroutine<Scp049Role>), nameof(StandardSubroutine<Scp049Role>.Owner))),
            
            // <value>
            new(OpCodes.Ldc_R4, const_value),

            // Cooldown(this.Owner, <value>)
            new(OpCodes.Call, Method(typeof(Scp049AttackAbility_Distance), nameof(Distance), [typeof(ReferenceHub), typeof(float)])),
            ]);

        // remove the full code.
        code.Remove(inst);

        return code;
    }

    internal static float Distance(ReferenceHub referenceHub, float currentValue)
    {
        Player player = Player.Get(referenceHub);
        if (CustomRoleHelpers.TryGetCustomRole(player, out var role) && role != null)
            return role.Scp.Scp049.AttackDistance.MathCalculation(currentValue);
        return currentValue;
    }
}
