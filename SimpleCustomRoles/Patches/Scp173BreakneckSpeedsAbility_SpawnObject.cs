using HarmonyLib;
using LabApi.Features.Wrappers;
using PlayerRoles.PlayableScps.Scp173;
using PlayerRoles.Subroutines;
using SimpleCustomRoles.Helpers;
using System.Reflection.Emit;
using static HarmonyLib.AccessTools;

namespace SimpleCustomRoles.Patches;

[HarmonyPatch(typeof(Scp173BreakneckSpeedsAbility), nameof(Scp173BreakneckSpeedsAbility.SpawnObject))]
internal static class Scp173BreakneckSpeedsAbility_SpawnObject
{
    internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        List<CodeInstruction> code = [.. instructions];
        // get the current value.
        var index = code.FindIndex(x => x.opcode == OpCodes.Ldc_R8);
        var inst = code[index];
        var const_value = inst.operand;
        // remove the full code.
        code.Remove(inst);

        // add after the field loaded
        code.InsertRange(index, [
            // this.Owner
            new(OpCodes.Ldarg_0),
            new(OpCodes.Callvirt, PropertyGetter(typeof(StandardSubroutine<Scp173Role>), nameof(StandardSubroutine<Scp173Role>.Owner))),
            
            // <value>
            new(OpCodes.Ldc_R8, const_value),

            // BreakneckRechargeTime(this.Owner, <value>)
            new(OpCodes.Call, Method(typeof(Scp173BreakneckSpeedsAbility_SpawnObject), nameof(BreakneckRechargeTime), [typeof(ReferenceHub), typeof(double)])),
            ]);

        return code;
    }

    internal static double BreakneckRechargeTime(ReferenceHub referenceHub, double currentValue)
    {
        Player player = Player.Get(referenceHub);
        if (CustomRoleHelpers.TryGetCustomRole(player, out var role) && role != null)
            return role.Scp.Scp173.BreakneckRechargeTime.MathCalculation((float)currentValue);
        return currentValue;
    }
}
