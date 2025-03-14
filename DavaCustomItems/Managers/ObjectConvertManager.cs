using DavaCustomItems.Configs;

namespace DavaCustomItems.Managers;

public static class ObjectConvertManager
{
    public static EffectConfig ParseToEffectConfig(object obj, EffectConfig default_value = default)
    {
        if (typeof(EffectConfig) == obj.GetType())
            return (EffectConfig)obj;
        if (typeof(Dictionary<object, object>) == obj.GetType())
        {
            return new EffectConfig(obj);
        }
        return default_value;
    }

    public static int ParseToInt(object obj, int default_value = default)
    {
        if (typeof(int) == obj.GetType())
            return (int)obj;
        if (typeof(string) == obj.GetType())
        {
            string str_obj = (string)obj;
            if (int.TryParse(str_obj, out int res))
                return res;
        }
        return default_value;
    }

    public static bool ParseToBool(object obj, bool default_value = default)
    {
        if (typeof(bool) == obj.GetType())
            return (bool)obj;
        if (typeof(string) == obj.GetType())
        {
            string str_obj = (string)obj;
            if (bool.TryParse(str_obj, out bool res))
                return res;
        }
        return default_value;
    }

    public static T ParseToEnum<T>(object obj, T default_value = default) where T : struct
    {
        if (typeof(T) == obj.GetType())
            return (T)obj;
        if (typeof(string) == obj.GetType())
        {
            string str_obj = (string)obj;
            if (Enum.TryParse(str_obj, true, out T result))
                return result;
            else
                return default_value;

        }
        return default_value;
    }
}
