#if NET45 || NET451 || NET452 || NET46 ||NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Utility.Redis
{

    /// <summary>
    /// 
    /// </summary>
    public class StackExchangeCache:IRedisCache
    {
        /// <summary>
        /// 
        /// </summary>
        protected ConnectionMultiplexer ConnectionMultiplexer;

        /// <summary>
        /// 
        /// </summary>

        protected IDatabase RedisClient;

        /// <summary>
        /// 
        /// </summary>

        protected CommandFlags Presistence = StackExchangeHelper.GetPresistenceCommandFlags();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        public StackExchangeCache(string host)
        {
            this.ConnectionMultiplexer = StackExchangeHelper.Connection(host);
            this.RedisClient = ConnectionMultiplexer.GetDatabase(0);
        }
        /// <summary>
        /// add string
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="expiresAt"></param>
        /// <returns></returns>
        public virtual bool AddString(string key, string val, DateTime? expiresAt = null)
        {
            return StackExchangeHelper.Add(RedisClient, key, val, DataStrucactorFlag.String, expiresAt);
        }

        /// <summary>
        /// add string 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="expiresAt"></param>
        /// <returns></returns>
        public virtual bool ReplaceString(string key, string val, DateTime? expiresAt = null)
        {
            return StackExchangeHelper.ReplaceString(RedisClient, key, val, expiresAt);
        }

        /// <summary>
        /// get string
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual string GetString(string key)
        {
            return StackExchangeHelper.GetString(RedisClient, key);
        }
        /// <summary>
        /// add string presistence
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="expiresAt"></param>
        /// <returns></returns>
        public virtual bool AddStringPresistence(string key, string val, DateTime? expiresAt = null)
        {
            return StackExchangeHelper.Add(RedisClient, key, val, DataStrucactorFlag.String, expiresAt,null, Presistence);
        }

        /// <summary>
        /// add string presistence
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="expiresAt"></param>
        /// <returns></returns>

        public virtual bool ReplaceStringPresistence(string key, string val, DateTime? expiresAt = null)
        {
            return StackExchangeHelper.ReplaceString(RedisClient, key, val, expiresAt, Presistence);
        }


        /// <summary>
        /// key expire
        /// </summary>
        /// <param name="key"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public virtual bool Expire(string key, int seconds)
        {
            return StackExchangeHelper.Expire(RedisClient, key, seconds);
        }

        /// <summary>
        /// key expire presistence
        /// </summary>
        /// <param name="key"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public virtual bool ExpirePresistence(string key, int seconds)
        {
            return StackExchangeHelper.Expire(RedisClient, key, seconds,Presistence);
        }


        /// <summary>
        /// remove key 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual bool RemoveKey(string key)
        {
            return StackExchangeHelper.RemoveKey(RedisClient, key);
        }

        /// <summary>
        /// remove key presistence
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual bool RemoveKeyPresistence(string key)
        {
            return StackExchangeHelper.RemoveKey(RedisClient, key, Presistence);
        }

        /// <summary>
        /// add list 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public virtual bool AddList(string key, string val)
        {
            return StackExchangeHelper.Add(RedisClient, key, val, DataStrucactorFlag.List);
        }

        /// <summary>
        /// remove all list
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual bool RemoveList(string key)
        {
            return StackExchangeHelper.RemoveKey(RedisClient, key);
        }

        /// <summary>
        /// remove first list item,but return item
        /// </summary>
        /// <param name="key"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public virtual string RemoveFirstList(string key, TimeSpan? timeout)
        {
            return StackExchangeHelper.RemoveFirstList(RedisClient, key, timeout);
        }

        /// <summary>
        /// remove last list item,but return item
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual string RemoveLastList(string key)
        {
            return StackExchangeHelper.RemoveLaseList(RedisClient, key,null);
        }

        /// <summary>
        /// add list
        /// </summary>
        /// <param name="key"></param>
        /// <param name="vals"></param>
        /// <returns></returns>
        public virtual bool AddList(string key, List<string> vals)
        {
            return StackExchangeHelper.Add(RedisClient, key, vals, DataStrucactorFlag.List);
        }

        /// <summary>
        /// add list presistence
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public virtual bool AddListPresistence(string key, string val)
        {
            return StackExchangeHelper.Add(RedisClient, key, val, DataStrucactorFlag.List,null,null,Presistence);
        }

        /// <summary>
        /// remove all list
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual bool RemoveListPresistence(string key)
        {
            return StackExchangeHelper.RemoveKey(RedisClient, key, Presistence);
        }

        /// <summary>
        /// remove first list item,but return item
        /// </summary>
        /// <param name="key"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public virtual string RemoveFirstListPresistence(string key, TimeSpan? timeout)
        {
            return StackExchangeHelper.RemoveFirstList(RedisClient, key, timeout,Presistence);
        }

        /// <summary>
        /// remove last list item,but return item
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual string RemoveLastListPresistence(string key)
        {
            return StackExchangeHelper.RemoveLaseList(RedisClient, key,null, Presistence);
        }

        /// <summary>
        /// pop list but not remove
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual string PopList(string key)
        {
            return StackExchangeHelper.Pop(RedisClient, key, DataStrucactorFlag.List);
        }

        /// <summary>
        ///get list 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public virtual List<string> GetList(string key, int start = -1, int end = -1)
        {
            return StackExchangeHelper.Get(RedisClient, key,  DataStrucactorFlag.List, start,end);
        }
        /// <summary>
        /// add list presistence
        /// </summary>
        /// <param name="key"></param>
        /// <param name="vals"></param>
        /// <returns></returns>
        public virtual bool AddListPresistence(string key, List<String> vals)
        {
            return StackExchangeHelper.Add(RedisClient, key, vals, DataStrucactorFlag.List,Presistence);
        }

        /// <summary>
        /// add set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public virtual bool AddSet(string key, string val)
        {
            return StackExchangeHelper.Add(RedisClient, key, val, DataStrucactorFlag.Set);
        }

        /// <summary>
        ///get set 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual HashSet<string> GetSet(string key)
        {
            return new HashSet<string>(StackExchangeHelper.Get(RedisClient, key, DataStrucactorFlag.Set, -1, -1));
        }
        /// <summary>
        /// remove set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public virtual bool RemoveSet(string key, string val)
        {
            return StackExchangeHelper.Remove(RedisClient, key, val, DataStrucactorFlag.Set);
        }

        /// <summary>
        /// remove set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public virtual bool RemoveSetPresistence(string key, string val)
        {
            return StackExchangeHelper.Remove(RedisClient, key,val, DataStrucactorFlag.Set,Presistence);
        }

        /// <summary>
        ///  pop set but not remove
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual string PopSet(string key)
        {
            return StackExchangeHelper.Pop(RedisClient, key, DataStrucactorFlag.Set);
        }

        /// <summary>
        /// add set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="vals"></param>
        /// <returns></returns>
        public virtual bool AddSet(string key, List<String> vals)
        {
            return StackExchangeHelper.Add(RedisClient, key, vals, DataStrucactorFlag.Set);
        }

        /// <summary>
        /// add set presistence
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public virtual bool AddSetPresistence(string key, string val)
        {
            return StackExchangeHelper.Add(RedisClient, key, val, DataStrucactorFlag.Set,null,null, Presistence);
        }

        /// <summary>
        /// add set presistence
        /// </summary>
        /// <param name="key"></param>
        /// <param name="vals"></param>
        /// <returns></returns>
        public virtual bool AddSetPresistence(string key, List<String> vals)
        {
            return StackExchangeHelper.Add(RedisClient, key, vals, DataStrucactorFlag.Set,Presistence);
        }

        /// <summary>
        /// add sortedset 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public virtual bool AddSortedSet(string key, string val)
        {
            return StackExchangeHelper.Add(RedisClient, key, val, DataStrucactorFlag.SortedSet);
        }

        /// <summary>
        ///get set 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public virtual List<string> GetSortedSet(string key, int start = -1, int end = -1)
        {
            return StackExchangeHelper.Get(RedisClient, key, DataStrucactorFlag.SortedSet, start, end);
        }
        /// <summary>
        /// remove soetedset
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public virtual bool RemoveSortedSet(string key, string val)
        {
            return StackExchangeHelper.Remove(RedisClient, key, val, DataStrucactorFlag.SortedSet);
        }

        /// <summary>
        /// remove sortedset set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public virtual bool RemoveSortedSetPresistence(string key, string val)
        {
            return StackExchangeHelper.Remove(RedisClient, key,val, DataStrucactorFlag.SortedSet,Presistence);
        }

        /// <summary>
        ///  pop set but not remove
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual string PopSortedSet(string key)
        {
            return StackExchangeHelper.Pop(RedisClient, key, DataStrucactorFlag.SortedSet);
        }

        /// <summary>
        /// add sorted set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="vals"></param>
        /// <returns></returns>
        public virtual bool AddSortedSet(string key, List<String> vals)
        {
            return StackExchangeHelper.Add(RedisClient, key, vals, DataStrucactorFlag.SortedSet);
        }

        /// <summary>
        /// add sorted set presistence
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public virtual bool AdddSortedSetPresistence(string key, string val)
        {
            return StackExchangeHelper.Add(RedisClient, key, val, DataStrucactorFlag.SortedSet,null,null, Presistence);
        }

        /// <summary>
        /// add sortedset presistence
        /// </summary>
        /// <param name="key"></param>
        /// <param name="vals"></param>
        /// <returns></returns>
        public virtual bool AdddSortedSetPresistence(string key, List<string> vals)
        {
            return StackExchangeHelper.Add(RedisClient, key, vals, DataStrucactorFlag.SortedSet,Presistence);
        }

        /// <summary>
        /// add hash
        /// </summary>
        /// <param name="hashId"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public virtual bool AddHash(string hashId, string key, string val)
        {
            return StackExchangeHelper.Add(RedisClient, key, val, DataStrucactorFlag.Hash, null, hashId);
        }

        /// <summary>
        /// add hash
        /// </summary>
        /// <param name="hashId"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public virtual bool AddHashPresistence(string hashId, string key, string val)
        {
            return StackExchangeHelper.Add(RedisClient, key,val,DataStrucactorFlag.Hash,null,hashId,Presistence);
        }

        /// <summary>
        /// get all hash key
        /// </summary>
        /// <param name="hashId"></param>
        /// <returns></returns>
        public virtual List<string> GetHashKey(string hashId)
        {
            return StackExchangeHelper.GetHashKey(RedisClient, hashId);
        }

        /// <summary>
        /// get all hash val
        /// </summary>
        /// <param name="hashId"></param>
        /// <returns></returns>
        public virtual List<string> GetHashValue(string hashId)
        {
            return StackExchangeHelper.GetHashValue(RedisClient, hashId);
        }

        /// <summary>
        /// according key get  hash val
        /// </summary>
        /// <param name="hashId"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual string GetHashValue(string hashId, string key)
        {
           return StackExchangeHelper.GetHashValue(RedisClient, hashId,key);
        }
        /// <summary>
        /// remote   hash  key
        /// </summary>
        /// <param name="hashId"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual  bool RemoveHash( string hashId, string key)
        {
            return StackExchangeHelper.RemoveHash(RedisClient, hashId, key);
        }
        /// <summary>
        /// remote   hash  key
        /// </summary>
        /// <param name="hashId"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual  bool RemoveHashPresistence(string hashId, string key)
        {
            return StackExchangeHelper.RemoveHash(RedisClient,hashId, key, Presistence);
        }
        /// <summary>
        /// subscribe
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="ac"></param>
        public virtual void Subscribe(string channel, Action<string, byte[]> ac)
        {
            StackExchangeHelper.Subscribe(ConnectionMultiplexer, channel, ac);
        }

        /// <summary>
        /// subscribe
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="ac"></param>
        public virtual void SubscribePresistence(string channel, Action<string, byte[]> ac)
        {
            StackExchangeHelper.Subscribe(ConnectionMultiplexer, channel, ac,Presistence);
        }
        /// <summary>
        /// publish
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="msg"></param>
        public virtual void Publish(string channel, byte[] msg)
        {
            StackExchangeHelper.Publish(RedisClient, channel, msg);
        }

        /// <summary>
        /// publish
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="msg"></param>
        public virtual void PublishPresistence(string channel, byte[] msg)
        {
            StackExchangeHelper.Publish(RedisClient, channel, msg,Presistence);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Dispose()
        {
            ConnectionMultiplexer.Dispose();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class StackExchangeHelper
    {
        /// <summary>
        /// 获取redis数据库链接
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public static ConnectionMultiplexer Connection(string host)
        {
            return ConnectionMultiplexer.Connect(host);
        }


        /// <summary>
        /// add string or set or sortedset or hash
        /// </summary>
        /// <param name="redisClient"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="dataStrucactorFlag"></param>
        /// <param name="expiresAt"></param>
        /// <param name="hashId">hash use</param>
        /// <param name="commandFlags"></param>
        /// <returns></returns>
        public static bool Add(IDatabase redisClient, string key, string val, DataStrucactorFlag dataStrucactorFlag = DataStrucactorFlag.String, DateTime? expiresAt = null, string hashId = "",CommandFlags commandFlags= CommandFlags.None)
        {
            var res = false;
            try
            {
                if (dataStrucactorFlag == DataStrucactorFlag.String)
                {
                    res = redisClient.SetAdd(key, val, commandFlags);
                }
                else if (dataStrucactorFlag == DataStrucactorFlag.List)
                {
                    res = redisClient.ListLeftPush(key, val, When.Always, commandFlags) > 0;
                }
                if (dataStrucactorFlag == DataStrucactorFlag.Set)
                {
                    res = redisClient.SetAdd(key, val, commandFlags);
                }
                else if (dataStrucactorFlag == DataStrucactorFlag.SortedSet)
                {
                    res = redisClient.SortedSetAdd(key, val, -1, commandFlags);
                }
                else if (dataStrucactorFlag == DataStrucactorFlag.Hash)
                {
                    res = redisClient.HashSet(hashId, key, val, When.Always, commandFlags);
                }
            }
            finally
            {
                if (res && expiresAt.HasValue)
                {
                    res = redisClient.KeyExpire(key, expiresAt.Value);
                }
            }
            return res;
        }

        /// <summary>
        /// add string  
        /// </summary>
        /// <param name="redisClient"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="expiresAt"></param>
        /// <param name="commandFlags"></param>
        /// <returns></returns>
        public static bool ReplaceString(IDatabase redisClient, string key, string val, DateTime? expiresAt = null, CommandFlags commandFlags = CommandFlags.None)
        {
            string res=  redisClient.StringSetRange(key,0, val, commandFlags);
            if (expiresAt.HasValue)
            {
                return redisClient.KeyExpire(key, expiresAt.Value, commandFlags);
            }
            return  true;
        }


        /// <summary>
        /// key expire
        /// </summary>
        /// <param name="redisClient"></param>
        /// <param name="key"></param>
        /// <param name="seconds"></param>
        /// <param name="commandFlags"></param>
        /// <returns></returns>
        public static bool Expire(IDatabase redisClient, string key, int seconds, CommandFlags commandFlags = CommandFlags.None)
        {
            return redisClient.KeyExpire(key, DateTime.Now.AddSeconds(seconds), commandFlags);
        }

        /// <summary>
        /// remove key 
        /// </summary>
        /// <param name="redisClient"></param>
        /// <param name="key"></param>
        /// <param name="commandFlags"></param>
        /// <returns></returns>
        public static bool RemoveKey(IDatabase redisClient, string key, CommandFlags commandFlags = CommandFlags.None)
        {
            return redisClient.KeyDelete(key, commandFlags);
        }



        /// <summary>
        /// get string or set or sortedset
        /// </summary>
        /// <param name="redisClient"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetString(IDatabase redisClient, string key)
        {
            return redisClient.StringGet(key);
        }



        /// <summary>
        /// remove first list item,but return item
        /// </summary>
        /// <param name="redisClient"></param>
        /// <param name="key"></param>
        /// <param name="timeout"></param>
        /// <param name="commandFlags"></param>
        /// <returns></returns>
        public static string RemoveFirstList(IDatabase redisClient, string key, TimeSpan? timeout, CommandFlags commandFlags = CommandFlags.None)
        {
            string val = redisClient.ListLeftPop(key);
            var res=redisClient.ListRemove(key, val, 0, commandFlags);
            if (res>0&&timeout.HasValue)
            {
                 redisClient.KeyExpire(key, DateTime.Now.Add(timeout.Value));
            }
            return val;
        }
        /// <summary>
        /// remove lase list item,but return item
        /// </summary>
        /// <param name="redisClient"></param>
        /// <param name="key"></param>
        /// <param name="timeout"></param>
        /// <param name="commandFlags"></param>
        /// <returns></returns>
        public static string RemoveLaseList(IDatabase redisClient, string key, TimeSpan? timeout, CommandFlags commandFlags = CommandFlags.None)
        {
            string val = redisClient.ListRightPop(key);
            var res = redisClient.ListRemove(key, val, 0, commandFlags);
            if (res > 0 && timeout.HasValue)
            {
                redisClient.KeyExpire(key, DateTime.Now.Add(timeout.Value));
            }
            return val;
        }

        /// <summary>
        /// add list or set or sortedset
        /// </summary>
        /// <param name="redisClient"></param>
        /// <param name="key"></param>
        /// <param name="vals"></param>
        /// <param name="dataStrucactorFlag"></param>
        /// <param name="commandFlags"></param>
        /// <returns></returns>
        public static bool Add(IDatabase redisClient, string key, List<string> vals, DataStrucactorFlag dataStrucactorFlag = DataStrucactorFlag.List, CommandFlags commandFlags = CommandFlags.None)
        {
            if (dataStrucactorFlag == DataStrucactorFlag.List)
            {
                long res=0;
                foreach (var item in vals)
                {
                    res+=redisClient.ListRightPush(key, item,When.Always,commandFlags);
                }
                return res>0;
            }
            if (dataStrucactorFlag == DataStrucactorFlag.Set)
            {
                bool res = false;
                foreach (var item in vals)
                {
                    var temp = redisClient.SetAdd(key, item,  commandFlags);
                    if (!res)
                    {
                        res = temp;
                    }
                }
                return res;
            }
            if (dataStrucactorFlag == DataStrucactorFlag.SortedSet)
            {
                bool res = false;
                foreach (var item in vals)
                {
                    var temp = redisClient.SortedSetAdd(key, item,-1, commandFlags);
                    if (!res)
                    {
                        res = temp;
                    }
                }
                return res;
            }
            return false;
        }


        /// <summary>
        /// pop list or set or sortedset but not remove
        /// </summary>
        /// <param name="redisClient"></param>
        /// <param name="key"></param>
        /// <param name="dataStrucactorFlag"></param>
        /// <returns></returns>
        public static string Pop(IDatabase redisClient, string key, DataStrucactorFlag dataStrucactorFlag = DataStrucactorFlag.List)
        {
            if (dataStrucactorFlag == DataStrucactorFlag.List)
            {
                return redisClient.ListLeftPop(key);
            }
            if (dataStrucactorFlag == DataStrucactorFlag.Set)
            {
                return redisClient.SetPop(key);
            }
            if (dataStrucactorFlag == DataStrucactorFlag.SortedSet)
            {
#if NET45 || NET451 || NET452 || NET46
                var vals = redisClient.SortedSetRangeByRank(key, 0, 1);
                return  vals != null && vals.Length > 0 ? (string)vals[0] : null; 
#else
                var res = redisClient.SortedSetPop(key, Order.Ascending);
                return res.HasValue ? (string)res.Value.Element : null;
#endif

            }
            return null;
        }

        /// <summary>
        ///get list or set or sortedset 
        /// </summary>
        /// <param name="redisClient"></param>
        /// <param name="key"></param>
        /// <param name="dataStrucactorFlag"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static List<string> Get(IDatabase redisClient, string key, DataStrucactorFlag dataStrucactorFlag = DataStrucactorFlag.List, int start = -1, int end = -1)
        {
            RedisValue[] redisValues=null; 
            if (dataStrucactorFlag == DataStrucactorFlag.List)
            {
                if (start != -1&&start>0
                    &&end!=-1||end>0)
                {
                    redisValues= redisClient.ListRange(key, start, end);
                }
                else
                {
                    redisValues = redisClient.ListRange(key);
                }
            }
            if (dataStrucactorFlag == DataStrucactorFlag.Set)
            {
                long length = redisClient.SetLength(key);
                if (length > 0)
                {
#if NET45 || NET451 || NET452 || NET46
                     redisValues = new RedisValue[] { redisClient.SetPop(key) };
#else
                    redisValues = redisClient.SetPop(key, length);
#endif
                }
            }
            if (dataStrucactorFlag == DataStrucactorFlag.SortedSet)
            {
                if (start == -1 && end != -1)
                {
                    redisValues = redisClient.SortedSetRangeByRank(key, start, end);
                }
                else
                {
                    redisValues = redisClient.SortedSetRangeByRank(key);
                }
            }
            if(redisValues!=null&& redisValues.Length > 0)
            {
                List<string> res = new List<string>(redisValues.Length);
                for (int i = 0; i < redisValues.Length; i++)
                {
                    res.Add(redisValues[i]);
                }
                return res;
            }
            return null;
        }

        /// <summary>
        /// remove  list or set or sortedset
        /// </summary>
        /// <param name="redisClient"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="dataStrucactorFlag"></param>
        /// <param name="commandFlags"></param>
        /// <returns></returns>
        public static bool Remove(IDatabase redisClient, string key, string val, DataStrucactorFlag dataStrucactorFlag = DataStrucactorFlag.List, CommandFlags commandFlags = CommandFlags.None)
        {
            if (dataStrucactorFlag == DataStrucactorFlag.List)
            {
                return redisClient.ListRemove(key, val,0, commandFlags) > 0;
            }
            if (dataStrucactorFlag == DataStrucactorFlag.Set)
            {
                redisClient.SetRemove(key, val, commandFlags);
                return true;
            }
            if (dataStrucactorFlag == DataStrucactorFlag.SortedSet)
            {
                return redisClient.SortedSetRemove(key, val, commandFlags);
            }
            return false;
        }

        /// <summary>
        /// get all hash key
        /// </summary>
        /// <param name="redisClient"></param>
        /// <param name="hashId"></param>
        /// <returns></returns>
        public static List<string> GetHashKey(IDatabase redisClient, string hashId)
        {
            RedisValue[] redisValues =  redisClient.HashKeys(hashId);
            if (redisValues != null && redisValues.Length > 0)
            {
                List<string> res = new List<string>(redisValues.Length);
                for (int i = 0; i < redisValues.Length; i++)
                {
                    res.Add(redisValues[i]);
                }
                return res;
            }
            return null;
        }

        /// <summary>
        /// get all hash val
        /// </summary>
        /// <param name="redisClient"></param>
        /// <param name="hashId"></param>
        /// <returns></returns>
        public static List<string> GetHashValue(IDatabase redisClient, string hashId)
        {
            RedisValue[] redisValues = redisClient.HashValues(hashId);
            if (redisValues != null && redisValues.Length > 0)
            {
                List<string> res = new List<string>(redisValues.Length);
                for (int i = 0; i < redisValues.Length; i++)
                {
                    res.Add(redisValues[i]);
                }
                return res;
            }
            return null;
        }

        /// <summary>
        /// get  hash val
        /// </summary>
        /// <param name="redisClient"></param>
        /// <param name="hashId"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetHashValue(IDatabase redisClient, string hashId, string key)
        {
            return redisClient.HashGet(hashId, key);
        }

        /// <summary>
        /// remote   hash  key
        /// </summary>
        /// <param name="redisClient"></param>
        /// <param name="hashId"></param>
        /// <param name="key"></param>
        /// <param name="commandFlags"></param>
        /// <returns></returns>
        public static bool RemoveHash(IDatabase redisClient, string hashId, string key, CommandFlags commandFlags = CommandFlags.None)
        {
            return redisClient.HashDelete(hashId, key, commandFlags);
        }

        /// <summary>
        /// subscribe
        /// </summary>
        /// <param name="redisClient"></param>
        /// <param name="channelName"></param>
        /// <param name="ac"></param>
        /// <param name="commandFlags"></param>
        public static void Subscribe(ConnectionMultiplexer redisClient, string channelName, Action<string, byte[]> ac, CommandFlags commandFlags = CommandFlags.None)
        {
            ISubscriber subscriber = redisClient.GetSubscriber();
#if NET45 || NET451 || NET452 || NET46
            subscriber.Subscribe(channelName,(it,msg)=>{
                ac?.Invoke(it,msg);
                Console.WriteLine(msg);
            }, commandFlags);
#else
            ChannelMessageQueue channel = subscriber.Subscribe(new RedisChannel(channelName, RedisChannel.PatternMode.Auto), commandFlags);
            channel.OnMessage(it => {
                ac?.Invoke(it.Channel,it.Message);
                Console.WriteLine(it.Message);
            });
#endif
        }
        /// <summary>
        /// publish
        /// </summary>
        /// <param name="redisClient"></param>
        /// <param name="channel"></param>
        /// <param name="msg"></param>
        /// <param name="commandFlags"></param>
        public static long Publish(IDatabase redisClient, string channel, byte[] msg, CommandFlags commandFlags = CommandFlags.None)
        {
            var res = redisClient.Publish(new RedisChannel(channel, RedisChannel.PatternMode.Auto), msg);
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="channelName"></param>
        /// <param name="msg"></param>
        /// <param name="commandFlags"></param>
        /// <returns></returns>
        public static long Publish(ConnectionMultiplexer connection, string channelName, byte[] msg, CommandFlags commandFlags = CommandFlags.None)
        {
            ISubscriber subscriber = connection.GetSubscriber();
            return subscriber.Publish(new RedisChannel(channelName, RedisChannel.PatternMode.Auto), msg, commandFlags);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static CommandFlags GetPresistenceCommandFlags()
        {
            CommandFlags commandFlags =
#if NET45 || NET451 || NET452 || NET46
CommandFlags.PreferSlave;
#else
CommandFlags.PreferReplica;
#endif
            return commandFlags;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static IDatabase Database(ConnectionMultiplexer connection, int db =0)
        {
            return connection.GetDatabase(db);
        }


      


        /// <summary>
        /// 关闭练级
        /// </summary>
        /// <param name="connection"></param>
        public static void Close(ConnectionMultiplexer connection)
        {
            if (connection != null)
            {
                connection.Dispose();
                connection = null;
            }
        }

    }
}
#endif