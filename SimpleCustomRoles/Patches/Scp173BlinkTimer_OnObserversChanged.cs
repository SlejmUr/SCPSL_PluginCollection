using HarmonyLib;
using LabApi.Features.Wrappers;
using PlayerRoles;
using PlayerRoles.PlayableScps.Scp173;
using PlayerRoles.Subroutines;
using SimpleCustomRoles.Helpers;
using System.Reflection.Emit;
using static HarmonyLib.AccessTools;

namespace SimpleCustomRoles.Patches;

[HarmonyPatch(typeof(Scp173BlinkTimer), nameof(Scp173BlinkTimer.OnObserversChanged))]
internal static class Scp173BlinkTimer_OnObserversChanged
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
			new(OpCodes.Callvirt, PropertyGetter(typeof(SubroutineBase), nameof(SubroutineBase.Role))),
            
            // <value>
            new(OpCodes.Ldc_R4, const_value),

            // BlinkCooldown(this.Owner, <value>)
            new(OpCodes.Call, Method(typeof(Scp173BlinkTimer_OnObserversChanged), nameof(BlinkCooldown), [typeof(PlayerRoleBase), typeof(float)])),
			]);

        return code;
	}

	internal static float BlinkCooldown(PlayerRoleBase roleBase, float currentValue)
	{
		if (!roleBase.TryGetOwner(out ReferenceHub hub))
			return currentValue;
		Player player = Player.Get(hub);
		if (CustomRoleHelpers.TryGetCustomRole(player, out var role) && role != null)
			return role.Scp.Scp173.BlinkCooldown.MathWithValue(currentValue);
		return currentValue;
	}
}
