using HarmonyLib;
using LabApi.Features.Wrappers;
using PlayerRoles.PlayableScps.Scp106;
using PlayerRoles.Subroutines;
using SimpleCustomRoles.Helpers;
using System.Reflection.Emit;
using static HarmonyLib.AccessTools;

namespace SimpleCustomRoles.Patches;

[HarmonyPatch(typeof(Scp106Attack), nameof(Scp106Attack.ServerShoot))]
internal static class Scp106Attack_ServerShoot
{
    internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        List<CodeInstruction> code = [.. instructions];
        // get the current value.
        var index = code.FindIndex(30, x => x.opcode == OpCodes.Ldc_R4);
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

            // VigorBonus(this.Owner, <value>)
            new(OpCodes.Call, Method(typeof(Scp106Attack_ServerShoot), nameof(VigorBonus), [typeof(ReferenceHub), typeof(float)])),
            ]);

        // get the current value.
        index = code.FindIndex(65, x => x.opcode == OpCodes.Ldc_R4);
        inst = code[index];
        const_value = inst.operand;
        // remove the full code.
        code.Remove(inst);

        // add after the field loaded
        code.InsertRange(index, [
            // this.Owner
            new(OpCodes.Ldarg_0),
            new(OpCodes.Callvirt, PropertyGetter(typeof(StandardSubroutine<Scp106Role>), nameof(StandardSubroutine<Scp106Role>.Owner))),
            
            // <value>
            new(OpCodes.Ldc_R4, const_value),

            // CorrodingEffect(this.Owner, <value>)
            new(OpCodes.Call, Method(typeof(Scp106Attack_ServerShoot), nameof(CorrodingEffect), [typeof(ReferenceHub), typeof(float)])),
            ]);

        // get the current value.
        index = code.FindIndex(100, x => x.opcode == OpCodes.Ldc_R4);
        inst = code[index];
        const_value = inst.operand;
        // remove the full code.
        code.Remove(inst);

        // add after the field loaded
        code.InsertRange(index, [
            // this.Owner
            new(OpCodes.Ldarg_0),
            new(OpCodes.Callvirt, PropertyGetter(typeof(StandardSubroutine<Scp106Role>), nameof(StandardSubroutine<Scp106Role>.Owner))),
            
            // <value>
            new(OpCodes.Ldc_R4, const_value),

            // VigorBonus(this.Owner, <value>)
            new(OpCodes.Call, Method(typeof(Scp106Attack_ServerShoot), nameof(VigorBonus), [typeof(ReferenceHub), typeof(float)])),
            ]);

        return code;
    }

    internal static float VigorBonus(ReferenceHub referenceHub, float currentValue)
    {
        Player player = Player.Get(referenceHub);
        if (CustomRoleHelpers.TryGetCustomRole(player, out var role) && role != null)
            return role.Scp.Scp106.VigorBonus.MathCalculation(currentValue);
        return currentValue;
    }
    internal static float CorrodingEffect(ReferenceHub referenceHub, float currentValue)
    {
        Player player = Player.Get(referenceHub);
        if (CustomRoleHelpers.TryGetCustomRole(player, out var role) && role != null)
            return role.Scp.Scp106.CorrodingEffect.MathCalculation(currentValue);
        return currentValue;
    }
}
