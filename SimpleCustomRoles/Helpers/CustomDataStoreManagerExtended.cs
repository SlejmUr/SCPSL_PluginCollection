using LabApi.Features.Stores;
using LabApi.Features.Wrappers;

namespace SimpleCustomRoles.Helpers;

public static class CustomDataStoreManagerExtended
{
    public static void EnsureExists<TStore>() where TStore : CustomDataStore
    {
        if (!CustomDataStoreManager.IsRegistered<TStore>())
            CustomDataStoreManager.RegisterStore<TStore>();
    }

    public static IReadOnlyCollection<Player> GetPlayers<TStore>() where TStore : CustomDataStore
    {
        Type type = typeof(TStore);

        if (!CustomDataStoreManager.IsRegistered<TStore>())
            return [];

        if (!CustomDataStore.StoreInstances.ContainsKey(type))
            return [];

        return CustomDataStore.StoreInstances[type].Keys;
    }

    public static Dictionary<Player, CustomDataStore> GetAll<TStore>() where TStore : CustomDataStore
    {
        Type type = typeof(TStore);

        if (!CustomDataStoreManager.IsRegistered<TStore>())
            return [];

        if (!CustomDataStore.StoreInstances.ContainsKey(type))
            return [];

        return CustomDataStore.StoreInstances[type];
    }
}
