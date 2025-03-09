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
}
