using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySpider.util
{
    public class Redishelper : IDisposable
    {
        private static string redisIP;
        private static ConnectionMultiplexer conn;
        private static IDatabase db;
        private static Redishelper redis;
        private static readonly object syncObject = new object();

        private Redishelper(string redisip)
        {
            redisIP = redisip;
            conn = ConnectionMultiplexer.Connect(redisIP);
            db = conn.GetDatabase();
        }

        public static Redishelper GetRedis(string redisip)
        {
            if (redis == null)
            {
                lock (syncObject)
                {
                    if (redis == null)
                    {
                        redis = new Redishelper(redisip);
                    }
                }
            }
            return redis;
        }

        public bool StringSet(RedisKey key, RedisValue value)
        {
            return db.StringSet(key, value);
        }

        public RedisValue StringGet(RedisKey key)
        {
            return db.StringGet(key);
        }

        public bool Remove(RedisKey key)
        {
            return db.KeyDelete(key);
        }

        public bool HashSet<T>(string key, string hashfiled, IEnumerable<T> t)
        {
            var value = JsonConvert.SerializeObject(t);
            return db.HashSet(key, hashfiled, value);
        }

        /// <summary>
        /// 存储数据到hash表
        /// </summary>
        public bool HashSet<T>(string key, string hashfiled, T t)
        {
            var value = JsonConvert.SerializeObject(t);
            return db.HashSet(key, hashfiled, value);
        }

        public bool IsExistsHashKey(string key, string hashfiled)
        {
            return db.HashExists(key, hashfiled);
        }

        public T GetHashValue<T>(string key, string hashfiled)
        {
            return JsonConvert.DeserializeObject<T>(db.HashGet(key, hashfiled));
        }

        public string GetHashValue(string key, string hashfiled)
        {
            return db.HashGet(key, hashfiled);
        }

        public IEnumerable<T> GetHashValuelist<T>(string key, string hashfiled)
        {
            return JsonConvert.DeserializeObject<IEnumerable<T>>(db.HashGet(key, hashfiled));
        }

        public IEnumerable<RedisValue> GetHashKeys(string key)
        {
            return db.HashKeys(key);
        }

        public IEnumerable<T> GetHashValue<T>(string key)
        {
            return db.HashGetAll(key).Select(x => JsonConvert.DeserializeObject<T>(x.Value));
        }

        public bool HashDelete(string key, string hashfiled)
        {
            return db.HashDelete(key, hashfiled);
        }

        /// <summary>  
        /// 释放资源  
        /// </summary>  
        public void Dispose()
        {
            if (conn != null)
            {
                conn.Close();//关闭连接  
                conn.Dispose();//释放资源 
            }
            //强制垃圾回收  
            GC.Collect();
        }

    }
}
