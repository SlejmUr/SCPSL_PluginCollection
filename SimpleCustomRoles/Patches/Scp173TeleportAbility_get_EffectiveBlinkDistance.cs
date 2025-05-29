using HarmonyLib;
using LabApi.Features.Wrappers;
using PlayerRoles.PlayableScps.Scp173;
using PlayerRoles.Subroutines;
using SimpleCustomRoles.Helpers;
using System.Reflection.Emit;
using static HarmonyLib.AccessTools;

namespace SimpleCustomRoles.Patches;

[HarmonyPatch(typeof(Scp173TeleportAbility), "get_EffectiveBlinkDistance")]
internal static class Scp173TeleportAbility_get_EffectiveBlinkDistance
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
			new(OpCodes.Callvirt, PropertyGetter(typeof(StandardSubroutine<Scp173Role>), nameof(StandardSubroutine<Scp173Role>.Owner))),
            
            // <value>
            new(OpCodes.Ldc_R4, const_value),

            // BlinkDistance(this.Owner, <value>)
            new(OpCodes.Call, Method(typeof(Scp173TeleportAbility_get_EffectiveBlinkDistance), nameof(BlinkDistance), [typeof(ReferenceHub), typeof(float)])),
            ]);

        index = code.FindIndex(x => x.opcode == OpCodes.Br_S) + 1;
        inst = code[index];
        const_value = inst.operand;
        // remove the full code.
        code.Remove(inst);

        // add after the field loaded
        code.InsertRange(index, [
            // this.Owner
            new(OpCodes.Ldarg_0),
            new(OpCodes.Callvirt, PropertyGetter(typeof(StandardSubroutine<Scp173Role>), nameof(StandardSubroutine<Scp173Role>.Owner))),
            
            // <value>
            new(OpCodes.Ldc_R4, const_value),

            // BreakneckDistanceMultiplier(this.Owner, <value>)
            new(OpCodes.Call, Method(typeof(Scp173TeleportAbility_get_EffectiveBlinkDistance), nameof(BreakneckDistanceMultiplier), [typeof(ReferenceHub), typeof(float)])),
            ]);

        return code;
	}

	internal static float BlinkDistance(ReferenceHub referenceHub, float currentValue)
	{
		Player player = Player.Get(referenceHub);
		if (CustomRoleHelpers.TryGetCustomRole(player, out var role) && role != null)
			return role.Scp.Scp173.BlinkDistance.MathWithValue(currentValue);
		return currentValue;
	}

    internal static float BreakneckDistanceMultiplier(ReferenceHub referenceHub, float currentValue)
    {
        Player player = Player.Get(referenceHub);
        if (CustomRoleHelpers.TryGetCustomRole(player, out var role) && role != null)
            return role.Scp.Scp173.BreakneckDistanceMultiplier.MathWithValue(currentValue);
        return currentValue;
    }
}
