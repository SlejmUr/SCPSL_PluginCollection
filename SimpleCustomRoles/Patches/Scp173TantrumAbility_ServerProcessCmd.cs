using HarmonyLib;
using LabApi.Features.Wrappers;
using PlayerRoles.PlayableScps.Scp173;
using PlayerRoles.Subroutines;
using SimpleCustomRoles.Helpers;
using System.Reflection.Emit;
using static HarmonyLib.AccessTools;

namespace SimpleCustomRoles.Patches;

[HarmonyPatch(typeof(Scp173TantrumAbility), nameof(Scp173TantrumAbility.ServerProcessCmd))]
internal static class Scp173TantrumAbility_ServerProcessCmd
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

            // TantrumCooldown(this.Owner, <value>)
            new(OpCodes.Call, Method(typeof(Scp173TantrumAbility_ServerProcessCmd), nameof(TantrumCooldown), [typeof(ReferenceHub), typeof(float)])),

			// (double)
            new(OpCodes.Conv_R8)
            ]);

        return code;
    }

    internal static float TantrumCooldown(ReferenceHub referenceHub, float currentValue)
    {
        Player player = Player.Get(referenceHub);
        if (CustomRoleHelpers.TryGetCustomRole(player, out var role) && role != null)
            return role.Scp.Scp173.TantrumCooldown.MathCalculation(currentValue);
        return currentValue;
    }
}
