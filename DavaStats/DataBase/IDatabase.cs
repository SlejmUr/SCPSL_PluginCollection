using DavaStats.Jsons;
using System.Collections.Generic;

namespace DavaStats.DataBase
{
    public interface IDatabase
    {
        void Save(Dictionary<string, AllStats> kv);

        Dictionary<string, AllStats> Load();
    }
}
