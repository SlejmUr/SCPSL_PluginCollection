using HarmonyLib;
using LabApi.Features.Wrappers;
using PlayerRoles.PlayableScps.Scp049;
using PlayerRoles.Subroutines;
using SimpleCustomRoles.Helpers;
using System.Reflection.Emit;
using static HarmonyLib.AccessTools;

namespace SimpleCustomRoles.Patches;

[HarmonyPatch(typeof(Scp049ResurrectAbility), nameof(Scp049ResurrectAbility.CheckMaxResurrections))]
internal static class Scp049ResurrectAbility_CheckMaxResurrections
{
    internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        List<CodeInstruction> code = [.. instructions];
        // get the current value.
        var index = code.FindIndex(x => x.opcode == OpCodes.Ldc_I4_2);
        var inst = code[index];

        // add after the field loaded
        code.InsertRange(index, [
            // this.Owner
            new(OpCodes.Ldarg_0),
            new(OpCodes.Callvirt, PropertyGetter(typeof(StandardSubroutine<Scp049Role>), nameof(StandardSubroutine<Scp049Role>.Owner))),
            
            // <value>
            new(OpCodes.Ldc_I4_2),

            // Cooldown(this.Owner, <value>)
            new(OpCodes.Call, Method(typeof(Scp049ResurrectAbility_CheckMaxResurrections), nameof(MaxResurrection), [typeof(ReferenceHub), typeof(int)])),
            ]);

        // remove the full code.
        code.Remove(inst);

        // get the current value.
        var index2 = code.FindLastIndex(x => x.opcode == OpCodes.Ldc_I4_2);
        inst = code[index2];

        // add after the field loaded
        code.InsertRange(index2, [
            // this.Owner
            new CodeInstruction(OpCodes.Ldarg_0),
            new(OpCodes.Callvirt, PropertyGetter(typeof(StandardSubroutine<Scp049Role>), nameof(StandardSubroutine<Scp049Role>.Owner))),
            
            // <value>
            new(OpCodes.Ldc_I4_2),

            // Cooldown(this.Owner, <value>)
            new(OpCodes.Call, Method(typeof(Scp049ResurrectAbility_CheckMaxResurrections), nameof(MaxResurrection), [typeof(ReferenceHub), typeof(int)])),
            ]);

        // remove the full code.
        code.Remove(inst);
        return code;
    }

    internal static int MaxResurrection(ReferenceHub referenceHub, int currentValue)
    {
        Player player = Player.Get(referenceHub);
        if (CustomRoleHelpers.TryGetCustomRole(player, out var role) && role != null)
            return role.Scp.Scp049.ResurrectMaxResurrection.MathCalculation(currentValue);
        return currentValue;
    }
}
