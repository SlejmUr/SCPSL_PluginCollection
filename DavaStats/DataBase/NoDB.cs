using DavaStats.Jsons;
using System.Collections.Generic;
using System.IO;

namespace DavaStats.DataBase
{
    public class NoDB : IDatabase
    {
        public Dictionary<string, AllStats> Load()
        {
            return new Dictionary<string, AllStats>();
        }

        public void Save(Dictionary<string, AllStats> kv)
        {

        }
    }
}
