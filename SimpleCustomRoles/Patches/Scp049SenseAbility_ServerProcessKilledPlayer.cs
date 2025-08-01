﻿using HarmonyLib;
using LabApi.Features.Wrappers;
using PlayerRoles.PlayableScps.Scp049;
using PlayerRoles.Subroutines;
using SimpleCustomRoles.Helpers;
using System.Reflection.Emit;
using static HarmonyLib.AccessTools;

namespace SimpleCustomRoles.Patches;

[HarmonyPatch(typeof(Scp049SenseAbility), nameof(Scp049SenseAbility.ServerProcessKilledPlayer))]
internal static class Scp049SenseAbility_ServerProcessKilledPlayer
{
    internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        List<CodeInstruction> code = [.. instructions];
        // get the current value.
        var index = code.FindIndex(x=>x.opcode == OpCodes.Ldc_R8);
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
            new(OpCodes.Call, Method(typeof(Scp049SenseAbility_ServerProcessKilledPlayer), nameof(Cooldown), [typeof(ReferenceHub), typeof(float)])),
            
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
            return role.Scp.Scp049.SenseBaseCooldown.MathWithValue(currentValue);
        return currentValue;
    }
}
