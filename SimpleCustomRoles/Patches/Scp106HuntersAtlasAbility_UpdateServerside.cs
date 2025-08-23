using HarmonyLib;
using LabApi.Features.Wrappers;
using PlayerRoles.PlayableScps.Scp106;
using PlayerRoles.Subroutines;
using SimpleCustomRoles.Helpers;
using System.Reflection.Emit;
using static HarmonyLib.AccessTools;

namespace SimpleCustomRoles.Patches;

[HarmonyPatch(typeof(Scp106HuntersAtlasAbility), nameof(Scp106HuntersAtlasAbility.UpdateServerside))]
internal static class Scp106HuntersAtlasAbility_UpdateServerside
{
    internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        List<CodeInstruction> code = [.. instructions];
        // get the current value.
        var index = code.FindIndex(25, x => x.opcode == OpCodes.Ldc_R4);
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

            // VigorCostPerMinute(this.Owner, <value>)
            new(OpCodes.Call, Method(typeof(Scp106HuntersAtlasAbility_UpdateServerside), nameof(VigorCostPerMinute), [typeof(ReferenceHub), typeof(float)])),
            ]);

        return code;
    }

    internal static float VigorCostPerMinute(ReferenceHub referenceHub, float currentValue)
    {
        Player player = Player.Get(referenceHub);
        if (CustomRoleHelpers.TryGetCustomRole(player, out var role) && role != null)
            return role.Scp.Scp106.VigorCostPerMinute.MathCalculation(currentValue);
        return currentValue;
    }
}
