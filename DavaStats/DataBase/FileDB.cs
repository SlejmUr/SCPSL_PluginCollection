using DavaStats.Jsons;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace DavaStats.DataBase
{
    public class FileDB : IDatabase
    {
        public Dictionary<string, AllStats> Load()
        {
            var path = Exiled.Loader.PathExtensions.GetPath(Main.Instance);
            path = path.Replace(".dll", "");
            Directory.CreateDirectory(path + "/davasave");
            if (File.Exists(path + "/davasave/all.json"))
            {
                var alljson = File.ReadAllText(path + "/davasave/all.json");
                var ret = JsonConvert.DeserializeObject<Dictionary<string, AllStats>>(alljson);
                if (ret != null)
                    return ret;
            }
            return new Dictionary<string, AllStats>();
        }

        public void Save(Dictionary<string, AllStats> kv)
        {
            var path = Exiled.Loader.PathExtensions.GetPath(Main.Instance);
            path = path.Replace(".dll", "");
            var json = JsonConvert.SerializeObject(kv);
            Directory.CreateDirectory(path + "/davasave");
            File.WriteAllText(path + "/davasave/all.json", json);
        }
    }
}
