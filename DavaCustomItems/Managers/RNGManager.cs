namespace DavaCustomItems.Managers;

public static class RNGManager
{
    public static Random RNG = new();

    public static T GetRandomWeight<T>(this Dictionary<T, int> dic, T default_val = default)
    {
        var sum = dic.Values.Sum();
        int chance = RNG.Next(1, sum + 1);
        T return_t = default_val;
        foreach (var kv in dic)
        {
            if (chance <= kv.Value)
            {
                return_t = kv.Key;
                break;
            }
            chance -= kv.Value;
        }
        return return_t;
    }

    public static float GetRandom(float min = float.MinValue, float max = float.MinValue)
    {
        double range = (double)min - (double)max;
        double sample = RNG.NextDouble();
        double scaled = (sample * range) + min;
        float f = (float)scaled;
        return f;
    }

    public static T GetRandomWeight<T>(this Dictionary<T, int> dic, Func<KeyValuePair<T, int>, bool> predicate, T default_val = default)
    {
        var dic2 = dic.Where(predicate).ToDictionary(x => x.Key, x => x.Value);
        var sum = dic2.Values.Sum();
        int chance = RNG.Next(1, sum + 1);
        T return_t = default_val;
        foreach (var kv in dic2)
        {
            if (chance <= kv.Value)
            {
                return_t = kv.Key;
                break;
            }
            chance -= kv.Value;
        }
        return return_t;
    }
}
