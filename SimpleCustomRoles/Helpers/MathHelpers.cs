using SimpleCustomRoles.RoleYaml;
using SimpleCustomRoles.RoleYaml.Enums;

namespace SimpleCustomRoles.Helpers;

public static class MathHelpers
{
    public static void MathWithValue(this MathValue mathValue, ref float inValue)
    {
        inValue = mathValue.Math.MathWithFloat(inValue, mathValue.Value);
    }

    public static void MathWithValue(this MathValueInt mathValue, ref int inValue)
    {
        inValue = mathValue.Math.MathWithInt(inValue, mathValue.Value);
    }

    public static float MathWithFloat(this MathOption mathOption, float inFloat, float myValue)
    {
        return mathOption switch
        {
            MathOption.None => inFloat,
            MathOption.Set => myValue,
            MathOption.Add => inFloat + myValue,
            MathOption.Subtract => inFloat - myValue,
            MathOption.Multiply => inFloat * myValue,
            MathOption.Divide => inFloat / myValue,
            _ => inFloat,
        };
    }

    public static int MathWithInt(this MathOption mathOption, int inInt, int myValue)
    {
        return mathOption switch
        {
            MathOption.None => inInt,
            MathOption.Set => myValue,
            MathOption.Add => inInt + myValue,
            MathOption.Subtract => inInt - myValue,
            MathOption.Multiply => inInt * myValue,
            MathOption.Divide => inInt / myValue,
            _ => inInt,
        };
    }
}
