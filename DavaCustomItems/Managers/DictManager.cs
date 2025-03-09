namespace DavaCustomItems.Managers;

public static class DictManager
{
    public static T Get<X, T>(this Dictionary<X, T> dic, X key, T default_t = default)
    {
        if (dic.TryGetValue(key, out T val))
            return val;
        return default_t;
    }
}
