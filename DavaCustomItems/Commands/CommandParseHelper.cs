using DavaCustomItems.Configs;
using UnityEngine;

namespace DavaCustomItems.Commands;

public static class CommandParseHelper
{
    public static bool ParseVector3(string arg, char separator, out Vector3 vector, out string response)
    {
        vector = Vector3.zero;
        if (!arg.Contains(separator))
        {
            response = "No separator found in input!";
            return false;
        }
        var splitted = arg.Split(separator);
        if (splitted.Length != 3)
        {
            response = "For Vector3 you need 3 separate Values!";
            return false;
        }
        for (int i = 0; i < 3; i++)
        {
            if (!float.TryParse(splitted[i], out float result))
            {
                response = $"In {i} cannot parse as float!";
                return false;
            }
            vector[i] = result;
        }
        response = string.Empty; 
        return true;
    }

    public static bool ParseColor(string arg, char separator, out Color color, out string response)
    {
        color = Color.clear;
        if (!arg.Contains(separator))
        {
            response = "No separator found in input!";
            return false;
        }
        var splitted = arg.Split(separator);
        if (!(splitted.Length == 4 || splitted.Length == 3))
        {
            response = $"For Color you need 3 or 4 separate Values! {splitted.Length}";
            return false;
        }
        for (int i = 0; i < splitted.Length; i++)
        {
            if (!float.TryParse(splitted[i], out float result))
            {
                response = $"In {i} cannot parse as float!";
                return false;
            }
            color[i] = result;
        }
        response = string.Empty;
        return true;
    }

    public enum InputType
    {
        Unknown,
        Text,
        Float,
        Number,
        Bool,
        Color,
        Vector3,
        Enum
    }

    public static InputType GetInputTypeForLight(string arg)
    {
        if (arg.Contains("follow"))
            return InputType.Bool;
        if (arg.Contains("intensity") || arg.Contains("range") || arg.Contains("spotrange") || arg.Contains("innerspotrange") || arg.Contains("shadowstrength"))
            return InputType.Float;
        if (arg.Contains("movementsmoothing"))
            return InputType.Number;
        if (arg.Contains("color"))
            return InputType.Color;
        if (arg.Contains("scale"))
            return InputType.Vector3;
        if (arg.Contains("lightshape") || arg.Contains("lighttype") || arg.Contains("shadowtype"))
            return InputType.Enum;
        return InputType.Unknown;
    }

    public static int TryParseToEnum(string arg, string str_input)
    {
        if (arg.Contains("lightshape"))
        {
            if (Enum.TryParse(str_input, true, out LightShape result))
                return (int)result;
        }
        if (arg.Contains("lighttype"))
        {
            if (Enum.TryParse(str_input, true, out LightType result))
                return (int)result;
        }
        if (arg.Contains("shadowtype"))
        {
            if (Enum.TryParse(str_input, true, out LightShadows result))
                return (int)result;
        }
        return 0;
    }

    public static void SetSettingForLight(ref LightConfig config, string name, object value)
    {
        if (name.Contains("follow"))
            config.ShouldFollowPlayer = (bool)value;
        if (name.Contains("intensity"))
            config.Intensity = (float)value;
        if (name.Contains("range"))
            config.Range = (float)value;
        if (name.Contains("spotrange"))
            config.SpotAngle = (float)value;
        if (name.Contains("innerspotrange"))
            config.InnerSpotAngle = (float)value;
        if (name.Contains("shadowstrength"))
            config.ShadowStrength = (float)value;
        if (name.Contains("movementsmoothing"))
            config.MovementSmoothing = (byte)value;
        if (name.Contains("color"))
            config.Color = (Color)value;
        if (name.Contains("scale"))
            config.Scale = (Vector3)value;
        if (name.Contains("lightshape"))
            config.LightShape = (LightShape)(int)value;
        if (name.Contains("lighttype"))
            config.LightType = (LightType)(int)value;
        if (name.Contains("shadowtype"))
            config.ShadowType = (LightShadows)(int)value;
    }
}
/*
 * args would be like
 * follow:true,
 * intensity:10,
 * range:15,
 * spotrange:10
 * innerspotrange:1,
 * shadowstrength:3,
 * color:0.4-0.7-0.9-0.5, OR color:0.4-0.7-0.9,
 * lightshape:box,
 * lighttype:area
 * shadowtype:soft,
 * movementsmoothing:60,
 * scale:1-1-1,
 * 
 */
