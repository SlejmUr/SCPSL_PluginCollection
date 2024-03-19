using DavaStats.Jsons;
using Exiled.API.Features;
using LiteDB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace DavaStats.DataBase
{
    public class SQLDB : IDatabase
    {
        public Dictionary<string, AllStats> Load()
        {
            var path = Exiled.Loader.PathExtensions.GetPath(Main.Instance);
            path = path.Replace(".dll","");
            Directory.CreateDirectory(path + "/davasave");
            var db = new LiteDatabase(path + "/davasave/DavaStats.db");
            var col = db.GetCollection<DB_Save>("Stats");
            var things = col.FindAll();
            var ret = new Dictionary<string, AllStats>();
            foreach (var item in things)
            {
                ret.Add(item.UserId, item.Stats);
            }
            db.Dispose();
            return ret;
        }

        public void Save(Dictionary<string, AllStats> kv)
        {
            var path = Exiled.Loader.PathExtensions.GetPath(Main.Instance);
            path = path.Replace(".dll", "");
            Directory.CreateDirectory(path + "/davasave");
            var db = new LiteDatabase(path + "/davasave/DavaStats.db");
            var col = db.GetCollection<DB_Save>("Stats");
            foreach (var item in kv)
            {
                Log.Info("Saving user: " + item.Key);
                col.Upsert(new DB_Save()
                { 
                    UserId = item.Key,
                    Stats = item.Value
                });
            }
            db.Commit();
            db.Dispose();
        }

        class DB_Save
        {
            [BsonId]
            public string UserId { get; set; }
            public AllStats Stats { get; set; }
        }
    }
}
