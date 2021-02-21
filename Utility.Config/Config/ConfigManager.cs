using Consul;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Config
{
    public class ConfigManager
    {
        public static string ConsulAddress { get; set; } = "http://127.0.0.1:8500/";
        public static string GetByConsul(string key)
        {
            using (var client = new ConsulClient(it => { it.Address = new Uri(ConsulAddress); }))
            {
                var kvPair = client.KV.Get(key).Result;
                if (kvPair.Response != null && kvPair.Response.Value != null)
                {
                    return Encoding.UTF8.GetString(kvPair.Response.Value, 0, kvPair.Response.Value.Length);
                }
                return string.Empty;
            }
        }
        public static bool SetByConsul(string key, byte[] val)
        {
            using (var client = new ConsulClient(it => { it.Address = new Uri(ConsulAddress); }))
            {
                return client.KV.Put(new KVPair(key) { Value = val }).Result.Response;
            }
        }

        public static void SetConfig(string name,ConfigEntity config)
        {
            using (var client = new ConsulClient(it => {
                it.Address = new Uri(ConfigManager.ConsulAddress);
            }))
            {
                string key = $"{name}/{name}";
                string json = System.Text.Json.JsonSerializer.Serialize(config);
                client.KV.Put(new KVPair(key)
                {
                    Value = Encoding.UTF8.GetBytes(json)
                });

                key = $"{name}/{DbFlag.MySql}ConnectionString";
                client.KV.Put(new KVPair(key)
                {
                    Value = Encoding.UTF8.GetBytes(config.
               ConnectionStrings.MySqlConnectionString)
                });

                key = $"{name}/{DbFlag.SqlServer}ConnectionString";
                client.KV.Put(new KVPair(key)
                {
                    Value = Encoding.UTF8.GetBytes(config.
               ConnectionStrings.SqlServerConnectionString)
                });

                key = $"{name}/{DbFlag.Sqlite}ConnectionString";
                client.KV.Put(new KVPair(key)
                {
                    Value = Encoding.UTF8.GetBytes(config.
               ConnectionStrings.SqliteConnectionString)
                });

                key = $"{name}/{DbFlag.Postgre}ConnectionString";
                client.KV.Put(new KVPair(key)
                {
                    Value = Encoding.UTF8.GetBytes(config.
               ConnectionStrings.PostgreConnectionString)
                });

                key = $"{name}/{DbFlag.Oracle}ConnectionString";
                client.KV.Put(new KVPair(key)
                {
                    Value = Encoding.UTF8.GetBytes(config.
               ConnectionStrings.OracleConnectionString)
                });
            }
        }
       
    }

}
