using HarmonyLib;
using LabApi.Features.Wrappers;
using Mirror;
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
	[HarmonyPrefix]
	public static bool Prefix(Scp173BlinkTimer __instance, int current)
	{
        if (!__instance.Role.TryGetOwner(out ReferenceHub hub))
            return true;
        Player player = Player.Get(hub);
		if (!CustomRoleHelpers.TryGetCustomRole(player, out var role))
            return true;
		if (role.Scp.Scp173.BlinkCooldown.Math == LabApiExtensions.Enums.MathOption.None)
			return true;
        __instance._initialStopTime = NetworkTime.time;
        __instance._totalCooldown = role.Scp.Scp173.BlinkCooldown.MathCalculation(3f);
        __instance._endSustainTime = (current > 0) ? (-1.0) : (NetworkTime.time + role.Scp.Scp173.BlinkSustainTime.MathCalculation(2.0f));
        __instance.ServerSendRpc(true);
        return false;
	}
	/*
	internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
	{
		List<CodeInstruction> code = [.. instructions];
		// get the current value.
		var index = code.FindIndex(10, x => x.opcode == OpCodes.Ldc_R4);
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
			return role.Scp.Scp173.BlinkCooldown.MathCalculation(currentValue);
		return currentValue;
	}
	*/
}
