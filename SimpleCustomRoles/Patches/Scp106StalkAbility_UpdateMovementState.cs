using HarmonyLib;
using LabApi.Features.Wrappers;
using PlayerRoles.PlayableScps.Scp106;
using PlayerRoles.Subroutines;
using SimpleCustomRoles.Helpers;
using System.Reflection.Emit;
using static HarmonyLib.AccessTools;

namespace SimpleCustomRoles.Patches;

[HarmonyPatch(typeof(Scp106StalkAbility), nameof(Scp106StalkAbility.UpdateMovementState))]
internal static class Scp106StalkAbility_UpdateMovementState
{
    internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        List<CodeInstruction> code = [.. instructions];
        // get the current value.
        var index = code.FindIndex(x => x.opcode == OpCodes.Ldc_R4);
        var inst = code[index];
        var const_value = inst.operand;
        // remove the full code.
        code.Remove(inst);

        // add after the field loaded
        code.InsertRange(index, [
            // this.Owner
            new(OpCodes.Ldarg_0),
            new(OpCodes.Callvirt, PropertyGetter(typeof(StandardSubroutine<Scp106Role>), nameof(StandardSubroutine<Scp106Role>.Owner))),
            
            // <value>
            new(OpCodes.Ldc_R4, const_value),

            // StalkCostStationary(this.Owner, <value>)
            new(OpCodes.Call, Method(typeof(Scp106StalkAbility_UpdateMovementState), nameof(MovementRange), [typeof(ReferenceHub), typeof(float)])),
            ]);

        index = code.FindIndex(index, x => x.opcode == OpCodes.Ldc_R8);
        inst = code[index];
        const_value = inst.operand;
        // remove the full code.
        code.Remove(inst);

        // add after the field loaded
        code.InsertRange(index, [
            // this.Owner
            new(OpCodes.Ldarg_0),
            new(OpCodes.Callvirt, PropertyGetter(typeof(StandardSubroutine<Scp106Role>), nameof(StandardSubroutine<Scp106Role>.Owner))),
            
            // <value>
            new(OpCodes.Ldc_R8, const_value),

            // MovementTimer(this.Owner, <value>)
            new(OpCodes.Call, Method(typeof(Scp106StalkAbility_UpdateMovementState), nameof(MovementTimer), [typeof(ReferenceHub), typeof(float)])),

            // (double)
            new(OpCodes.Conv_R8)
            ]);

        index = code.FindIndex(index, x => x.opcode == OpCodes.Ldc_R4);
        inst = code[index];
        const_value = inst.operand;
        // remove the full code.
        code.Remove(inst);

        // add after the field loaded
        code.InsertRange(index, [
            // this.Owner
            new(OpCodes.Ldarg_0),
            new(OpCodes.Callvirt, PropertyGetter(typeof(StandardSubroutine<Scp106Role>), nameof(StandardSubroutine<Scp106Role>.Owner))),
            
            // <value>
            new(OpCodes.Ldc_R4, const_value),

            // StalkVigorRegeneration(this.Owner, <value>)
            new(OpCodes.Call, Method(typeof(Scp106StalkAbility_UpdateMovementState), nameof(StalkVigorRegeneration), [typeof(ReferenceHub), typeof(float)])),
            ]);

        return code;
    }

    internal static float MovementRange(ReferenceHub referenceHub, float currentValue)
    {
        Player player = Player.Get(referenceHub);
        if (CustomRoleHelpers.TryGetCustomRole(player, out var role) && role != null)
            return role.Scp.Scp106.MovementRange.MathCalculation(currentValue);
        return currentValue;
    }

    internal static float MovementTimer(ReferenceHub referenceHub, float currentValue)
    {
        Player player = Player.Get(referenceHub);
        if (CustomRoleHelpers.TryGetCustomRole(player, out var role) && role != null)
            return role.Scp.Scp106.MovementTimer.MathCalculation(currentValue);
        return currentValue;
    }

    internal static float StalkVigorRegeneration(ReferenceHub referenceHub, float currentValue)
    {
        Player player = Player.Get(referenceHub);
        if (CustomRoleHelpers.TryGetCustomRole(player, out var role) && role != null)
            return role.Scp.Scp106.StalkVigorRegeneration.MathCalculation(currentValue);
        return currentValue;
    }
}
