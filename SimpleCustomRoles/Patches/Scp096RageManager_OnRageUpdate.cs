using HarmonyLib;
using LabApi.Features.Wrappers;
using PlayerRoles.PlayableScps.Scp096;
using PlayerRoles.Subroutines;
using SimpleCustomRoles.Helpers;
using System.Reflection.Emit;
using static HarmonyLib.AccessTools;

namespace SimpleCustomRoles.Patches;

[HarmonyPatch(typeof(Scp096RageManager), nameof(Scp096RageManager.OnRageUpdate))]
internal static class Scp096RageManager_OnRageUpdate
{
    internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {


        List<CodeInstruction> code = [.. instructions];
        // get the current value.
        var index = code.FindIndex(x => x.opcode == OpCodes.Ldc_R4);
        var inst = code[index];
        var const_value = code[index].operand;

        // add after the field loaded
        code.InsertRange(index, [
            // this.Owner
            new CodeInstruction(OpCodes.Ldarg_0).MoveLabelsFrom(inst),
            new(OpCodes.Callvirt, PropertyGetter(typeof(StandardSubroutine<Scp096Role>), nameof(StandardSubroutine<Scp096Role>.Owner))),
            
            // <value>
            new(OpCodes.Ldc_R4, const_value),

            // CalmingShieldMultiplier(this.Owner, <value>)
            new(OpCodes.Call, Method(typeof(Scp096RageManager_OnRageUpdate), nameof(CalmingShieldMultiplier), [typeof(ReferenceHub), typeof(float)])),
            ]);

        // remove the full code.
        code.Remove(inst);

        var index2 = code.FindIndex(index + 5, x => x.opcode == OpCodes.Ldc_R4);
        var inst2 = code[index2];
        var const_value2 = inst2.operand;

        // add after the field loaded
        code.InsertRange(index2, [
            // this.Owner
            new CodeInstruction(OpCodes.Ldarg_0).MoveLabelsFrom(inst2),
            new(OpCodes.Callvirt, PropertyGetter(typeof(StandardSubroutine<Scp096Role>), nameof(StandardSubroutine<Scp096Role>.Owner))),
            
            // <value>
            new(OpCodes.Ldc_R4, const_value2),

            // EnragingShieldMultiplier(this.Owner, <value>)
            new(OpCodes.Call, Method(typeof(Scp096RageManager_OnRageUpdate), nameof(EnragingShieldMultiplier), [typeof(ReferenceHub), typeof(float)])),
            ]);

        // remove the full code.
        code.Remove(inst2);

        return code;
    }

    internal static float CalmingShieldMultiplier(ReferenceHub referenceHub, float currentValue)
    {
        Player player = Player.Get(referenceHub);
        if (CustomRoleHelpers.TryGetCustomRole(player, out var role) && role != null)
            return role.Scp.Scp096.CalmingShieldMultiplier.MathCalculation(currentValue);
        return currentValue;
    }

    internal static float EnragingShieldMultiplier(ReferenceHub referenceHub, float currentValue)
    {
        Player player = Player.Get(referenceHub);
        if (CustomRoleHelpers.TryGetCustomRole(player, out var role) && role != null)
            return role.Scp.Scp096.EnragingShieldMultiplier.MathCalculation(currentValue);
        return currentValue;
    }
}