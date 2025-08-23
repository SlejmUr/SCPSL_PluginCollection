using HarmonyLib;
using LabApi.Features.Wrappers;
using PlayerRoles.PlayableScps.Scp049;
using PlayerRoles.Subroutines;
using SimpleCustomRoles.Helpers;
using System.Reflection.Emit;
using static HarmonyLib.AccessTools;

namespace SimpleCustomRoles.Patches;
// DISABLED FUCK IL CODE

[HarmonyPatch(typeof(Scp049ResurrectAbility), nameof(Scp049ResurrectAbility.CheckBeginConditions))]
internal static class Scp049ResurrectAbility_CheckBeginConditions
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
            new(OpCodes.Call, Method(typeof(Scp049ResurrectAbility_CheckBeginConditions), nameof(Human), [typeof(ReferenceHub), typeof(float)])),
            ]);

        // remove the full code.
        code.Remove(inst);

        // get the current value.
        var index2 = code.FindLastIndex(x => x.opcode == OpCodes.Ldc_R4);
        inst = code[index2];
        const_value = inst.operand;

        // add after the field loaded
        code.InsertRange(index2, [
            // this.Owner
            new CodeInstruction(OpCodes.Ldarg_0).MoveLabelsFrom(inst),
            new(OpCodes.Callvirt, PropertyGetter(typeof(StandardSubroutine<Scp049Role>), nameof(StandardSubroutine<Scp049Role>.Owner))),
            
            // <value>
            new(OpCodes.Ldc_R4, const_value),

            // Cooldown(this.Owner, <value>)
            new(OpCodes.Call, Method(typeof(Scp049ResurrectAbility_CheckBeginConditions), nameof(Corpse), [typeof(ReferenceHub), typeof(float)])),
            ]);

        // remove the full code.
        code.Remove(inst);

        return code;
    }

    internal static float Human(ReferenceHub referenceHub, float currentValue)
    {
        Player player = Player.Get(referenceHub);
        if (CustomRoleHelpers.TryGetCustomRole(player, out var role) && role != null)
            return role.Scp.Scp049.ResurrectHumanCorpseDuration.MathCalculation(currentValue);
        return currentValue;
    }

    internal static float Corpse(ReferenceHub referenceHub, float currentValue)
    {
        Player player = Player.Get(referenceHub);
        if (CustomRoleHelpers.TryGetCustomRole(player, out var role) && role != null)
            return role.Scp.Scp049.ResurrectTargetCorpseDuration.MathCalculation(currentValue);
        return currentValue;
    }
}
