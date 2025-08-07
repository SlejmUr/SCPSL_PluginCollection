using HarmonyLib;
using LabApi.Features.Wrappers;
using PlayerRoles.PlayableScps.Scp106;
using PlayerRoles.Subroutines;
using SimpleCustomRoles.Helpers;
using System.Reflection.Emit;
using static HarmonyLib.AccessTools;

namespace SimpleCustomRoles.Patches;

[HarmonyPatch(typeof(Scp106StalkAbility), nameof(Scp106StalkAbility.UpdateServerside))]
internal static class Scp106StalkAbility_UpdateServerside
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
            new(OpCodes.Callvirt, PropertyGetter(typeof(StandardSubroutine<Scp106Role>), nameof(StandardSubroutine<Scp106Role>.Owner))),
            
            // <value>
            new(OpCodes.Ldc_R4, const_value),

            // StalkCostStationary(this.Owner, <value>)
            new(OpCodes.Call, Method(typeof(Scp106StalkAbility_UpdateServerside), nameof(StalkCostStationary), [typeof(ReferenceHub), typeof(float)])),
            ]);

        // remove the full code.
        code.Remove(inst);

        index = code.FindIndex(index + 1, x => x.opcode == OpCodes.Ldc_R4);
        inst = code[index];
        const_value = inst.operand;

        // add after the field loaded
        code.InsertRange(index, [
            // this.Owner
            new(OpCodes.Ldarg_0),
            new(OpCodes.Callvirt, PropertyGetter(typeof(StandardSubroutine<Scp106Role>), nameof(StandardSubroutine<Scp106Role>.Owner))),
            
            // <value>
            new(OpCodes.Ldc_R4, const_value),

            // StalkCostMoving(this.Owner, <value>)
            new(OpCodes.Call, Method(typeof(Scp106StalkAbility_UpdateServerside), nameof(StalkCostMoving), [typeof(ReferenceHub), typeof(float)])),
            ]);

        // remove the full code.
        code.Remove(inst);

        return code;
    }

    internal static float StalkCostStationary(ReferenceHub referenceHub, float currentValue)
    {
        Player player = Player.Get(referenceHub);
        if (CustomRoleHelpers.TryGetCustomRole(player, out var role) && role != null)
            return role.Scp.Scp106.StalkCostStationary.MathCalculation(currentValue);
        return currentValue;
    }

    internal static float StalkCostMoving(ReferenceHub referenceHub, float currentValue)
    {
        Player player = Player.Get(referenceHub);
        if (CustomRoleHelpers.TryGetCustomRole(player, out var role) && role != null)
            return role.Scp.Scp106.StalkCostMoving.MathCalculation(currentValue);
        return currentValue;
    }
}
