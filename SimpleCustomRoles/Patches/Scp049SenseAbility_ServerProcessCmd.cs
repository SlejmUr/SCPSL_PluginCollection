using HarmonyLib;
using LabApi.Features.Wrappers;
using PlayerRoles.PlayableScps.Scp049;
using PlayerRoles.Subroutines;
using SimpleCustomRoles.Helpers;
using System.Reflection.Emit;
using static HarmonyLib.AccessTools;

namespace SimpleCustomRoles.Patches;

[HarmonyPatch(typeof(Scp049SenseAbility), nameof(Scp049SenseAbility.ServerProcessCmd))]
internal static class Scp049SenseAbility_ServerProcessCmd
{
    internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        List<CodeInstruction> code = [.. instructions];

        // get the current value.
        var index = code.FindIndex(x => x.opcode == OpCodes.Ldc_R8);
        var inst = code[index];
        var const_value = code[index].operand;

        // add after the field loaded
        code.InsertRange(index, [
            // this.Owner
            new(OpCodes.Ldarg_0),
            new(OpCodes.Callvirt, PropertyGetter(typeof(StandardSubroutine<Scp049Role>), nameof(StandardSubroutine<Scp049Role>.Owner))),
            
            // <value>
            new(OpCodes.Ldc_R8, const_value),

            // Cooldown(this.Owner, <value>)
            new(OpCodes.Call, Method(typeof(Scp049SenseAbility_ServerProcessCmd), nameof(Cooldown), [typeof(ReferenceHub), typeof(float)])),
            
            // (double)
            new(OpCodes.Conv_R8)
            ]);

        // remove the full code.
        code.Remove(inst);

        // get the current value.
        var index2 = code.FindLastIndex(x => x.opcode == OpCodes.Ldc_R8);
        inst = code[index2];
        const_value = code[index2].operand;


        // add after the field loaded
        code.InsertRange(index2, [
            // this.Owner
            new(OpCodes.Ldarg_0),
            new(OpCodes.Callvirt, PropertyGetter(typeof(StandardSubroutine<Scp049Role>), nameof(StandardSubroutine<Scp049Role>.Owner))),
            
            // <value>
            new(OpCodes.Ldc_R8, const_value),

            // Cooldown(this.Owner, <value>)
            new(OpCodes.Call, Method(typeof(Scp049SenseAbility_ServerProcessCmd), nameof(Duration), [typeof(ReferenceHub), typeof(float)])),
            
            // (double)
            new(OpCodes.Conv_R8)
            ]);

        // remove the full code.
        code.Remove(inst);

        return code;
    }

    internal static float Cooldown(ReferenceHub referenceHub, float currentValue)
    {
        Player player = Player.Get(referenceHub);
        if (CustomRoleHelpers.TryGetCustomRole(player, out var role) && role != null)
            return role.Scp.Scp049.SenseAttemptFailCooldown.MathCalculation(currentValue);
        return currentValue;
    }

    internal static float Duration(ReferenceHub referenceHub, float currentValue)
    {
        Player player = Player.Get(referenceHub);
        if (CustomRoleHelpers.TryGetCustomRole(player, out var role) && role != null)
            return role.Scp.Scp049.SenseEffectDuration.MathCalculation(currentValue);
        return currentValue;
    }
}
