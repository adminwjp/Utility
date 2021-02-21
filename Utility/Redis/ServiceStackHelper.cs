#if NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility.Utils;

namespace Utility.Redis
{
    /// <summary>
    /// 
    /// </summary>
    public class ServiceStackCache:IRedisCache
    {
        /// <summary>
        /// 
        /// </summary>
        public  RedisClient RedisClient { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="port"></param>
        /// <param name="pwd"></param>
        /// <param name="db"></param>
        public ServiceStackCache(string connectionString, int port, string pwd, int db = 0)
        {
            this.RedisClient = ServiceStackHelper.GetRedis(connectionString, port, pwd, db);
        }
        /// <summary>
        /// add string
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="expiresAt"></param>
        /// <returns></returns>
        public virtual bool AddString( string key,string val, DateTime? expiresAt=null)
        {
            return ServiceStackHelper.Add(RedisClient,key, val,DataStrucactorFlag.String, expiresAt);
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
            return ServiceStackHelper.ReplaceString(RedisClient,  key, val, expiresAt);
        }

        /// <summary>
        /// get string
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual string GetString(string key)
        {
            return ServiceStackHelper.GetString(RedisClient,  key);
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
            return ServiceStackHelper.AddPresistence(RedisClient, key,val,DataStrucactorFlag.String,expiresAt);
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
            return ServiceStackHelper.ReplaceStringPresistence(RedisClient,  key, val, expiresAt);
        }


        /// <summary>
        /// key expire
        /// </summary>
        /// <param name="key"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public virtual bool Expire(string key,  int seconds)
        {
            return ServiceStackHelper.Expire(RedisClient,  key, seconds);
        }

        /// <summary>
        /// key expire presistence
        /// </summary>
        /// <param name="key"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public virtual bool ExpirePresistence( string key, int seconds)
        {
            return ServiceStackHelper.ExpirePresistence(RedisClient,  key, seconds);
        }


        /// <summary>
        /// remove key 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual bool RemoveKey(string key)
        {
            return ServiceStackHelper.RemoveKey(RedisClient,  key, DataStrucactorFlag.String);
        }

        /// <summary>
        /// remove key presistence
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual bool RemoveKeyPresistence(string key)
        {
            return ServiceStackHelper.RemoveKeyPresistence(RedisClient, key, DataStrucactorFlag.String);
        }

        /// <summary>
        /// add list 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public virtual bool AddList(string key,string val)
        {
            return ServiceStackHelper.Add(RedisClient,  key, val, DataStrucactorFlag.List);
        }

        /// <summary>
        /// remove all list
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual bool RemoveList(string key)
        {
            return ServiceStackHelper.RemoveKey(RedisClient, key, DataStrucactorFlag.List);
        }

        /// <summary>
        /// remove first list item,but return item
        /// </summary>
        /// <param name="key"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public virtual string RemoveFirstList(string key,TimeSpan? timeout)
        {
            return ServiceStackHelper.RemoveFirstList(RedisClient,  key, timeout);
        }

        /// <summary>
        /// remove last list item,but return item
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual string RemoveLastList(string key)
        {
            return ServiceStackHelper.RemoveLastList(RedisClient, key);
        }

        /// <summary>
        /// add list
        /// </summary>
        /// <param name="key"></param>
        /// <param name="vals"></param>
        /// <returns></returns>
        public virtual bool AddList( string key, List<string> vals)
        {
            return ServiceStackHelper.Add(RedisClient, key,vals, DataStrucactorFlag.List);
        }

        /// <summary>
        /// add list presistence
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public virtual bool AddListPresistence( string key, string val)
        {
            return ServiceStackHelper.AddPresistence(RedisClient, key, val, DataStrucactorFlag.List);
        }

        /// <summary>
        /// remove all list
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual bool RemoveListPresistence(string key)
        {
            return ServiceStackHelper.RemoveKeyPresistence(RedisClient, key, DataStrucactorFlag.List);
        }

        /// <summary>
        /// remove first list item,but return item
        /// </summary>
        /// <param name="key"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public virtual string RemoveFirstListPresistence(string key, TimeSpan? timeout)
        {
            return ServiceStackHelper.RemoveFirstListPresistence(RedisClient, key, timeout);
        }

        /// <summary>
        /// remove last list item,but return item
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual string RemoveLastListPresistence( string key)
        {
            return ServiceStackHelper.RemoveLastListPresistence(RedisClient, key);
        }

        /// <summary>
        /// pop list but not remove
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual string PopList(string key)
        {
            return ServiceStackHelper.Pop(RedisClient, key,DataStrucactorFlag.List);
        }

        /// <summary>
        ///get list 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public virtual List<string> GetList(string key,int start=-1,int end=-1)
        {
            return ServiceStackHelper.Get(RedisClient, key, DataStrucactorFlag.List,start,end);
        }
        /// <summary>
        /// add list presistence
        /// </summary>
        /// <param name="key"></param>
        /// <param name="vals"></param>
        /// <returns></returns>
        public virtual bool AddListPresistence( string key, List<String> vals)
        {
            return ServiceStackHelper.AddPresistence(RedisClient, key,vals, DataStrucactorFlag.List);
        }

        /// <summary>
        /// add set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public virtual bool AddSet(string key, string val)
        {
            return ServiceStackHelper.Add(RedisClient, key, val, DataStrucactorFlag.Set);
        }

        /// <summary>
        ///get set 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public  virtual HashSet<string> GetSet(string key)
        {
            var datas = ServiceStackHelper.Get(RedisClient, key, DataStrucactorFlag.Set);
            return datas!=null&&datas.Count>0?new HashSet<string>(datas):null;
        }
        /// <summary>
        /// remove set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public virtual bool RemoveSet(string key, string val)
        {
            return ServiceStackHelper.Remove(RedisClient, key,val, DataStrucactorFlag.Set);
        }

        /// <summary>
        /// remove set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public virtual bool RemoveSetPresistence(string key, string val)
        {
            return ServiceStackHelper.RemovePresistence(RedisClient, key, val, DataStrucactorFlag.Set);
        }

        /// <summary>
        ///  pop set but not remove
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual string PopSet(string key)
        {
            return ServiceStackHelper.Pop(RedisClient, key,  DataStrucactorFlag.Set);
        }

        /// <summary>
        /// add set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="vals"></param>
        /// <returns></returns>
        public virtual bool AddSet(string key, List<String> vals)
        {
            return ServiceStackHelper.Add(RedisClient, key,vals, DataStrucactorFlag.Set);
        }

        /// <summary>
        /// add set presistence
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public virtual bool AddSetPresistence(string key, string val)
        {
            return ServiceStackHelper.AddPresistence(RedisClient, key, val, DataStrucactorFlag.Set);
        }

        /// <summary>
        /// add set presistence
        /// </summary>
        /// <param name="key"></param>
        /// <param name="vals"></param>
        /// <returns></returns>
        public virtual bool AddSetPresistence(string key, List<String> vals)
        {
            return ServiceStackHelper.AddPresistence(RedisClient, key, vals, DataStrucactorFlag.Set);
        }

        /// <summary>
        /// add sortedset 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public virtual bool AddSortedSet( string key, string val)
        {
            return ServiceStackHelper.AddPresistence(RedisClient, key, val, DataStrucactorFlag.SortedSet);
        }

        /// <summary>
        ///get set 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public virtual List<string> GetSortedSet(string key,int start=-1,int end=-1)
        {
            return ServiceStackHelper.Get(RedisClient, key, DataStrucactorFlag.SortedSet,start,end);
        }
        /// <summary>
        /// remove soetedset
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public virtual bool RemoveSortedSet(string key, string val)
        {
            return ServiceStackHelper.Remove(RedisClient, key,val, DataStrucactorFlag.SortedSet);
        }

        /// <summary>
        /// remove sortedset set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public virtual bool RemoveSortedSetPresistence( string key, string val)
        {
            return ServiceStackHelper.RemovePresistence(RedisClient, key, val, DataStrucactorFlag.SortedSet);
        }

        /// <summary>
        ///  pop set but not remove
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual string PopSortedSet( string key)
        {
            return ServiceStackHelper.Pop(RedisClient, key,DataStrucactorFlag.SortedSet);
        }

        /// <summary>
        /// add sorted set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="vals"></param>
        /// <returns></returns>
        public virtual bool AddSortedSet(string key, List<String> vals)
        {
            return ServiceStackHelper.Add(RedisClient, key,vals, DataStrucactorFlag.SortedSet);
        }

        /// <summary>
        /// add sorted set presistence
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public virtual bool AdddSortedSetPresistence( string key, string val)
        {
            return ServiceStackHelper.AddPresistence(RedisClient, key, val, DataStrucactorFlag.SortedSet);
        }

        /// <summary>
        /// add sortedset presistence
        /// </summary>
        /// <param name="key"></param>
        /// <param name="vals"></param>
        /// <returns></returns>
        public virtual bool AdddSortedSetPresistence(string key, List<string> vals)
        {
            return ServiceStackHelper.AddPresistence(RedisClient, key, vals, DataStrucactorFlag.SortedSet);
        }

        /// <summary>
        /// add hash
        /// </summary>
        /// <param name="hashId"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public virtual bool AddHash(string hashId, string key,string val)
        {
            return ServiceStackHelper.Add(RedisClient, key, val, DataStrucactorFlag.Hash, null, hashId);
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
            return ServiceStackHelper.AddPresistence(RedisClient, key,val,DataStrucactorFlag.Hash,null, hashId);
        }

        /// <summary>
        /// get all hash key
        /// </summary>
        /// <param name="hashId"></param>
        /// <returns></returns>
        public virtual List<string> GetHashKey( string hashId)
        {
            return ServiceStackHelper.GetHashKey(RedisClient, hashId);
        }

        /// <summary>
        /// get all hash val
        /// </summary>
        /// <param name="hashId"></param>
        /// <returns></returns>
        public virtual List<string> GetHashValue(string hashId)
        {
            return ServiceStackHelper.GetHashValue(RedisClient, hashId);
        }

        /// <summary>
        /// according key get  hash val
        /// </summary>
        /// <param name="hashId"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public  virtual string GetHashValue(string hashId, string key)
        {
            return ServiceStackHelper.GetHashValue(RedisClient, hashId, key);
        }

        /// <summary>
        /// remote   hash  key
        /// </summary>
        /// <param name="hashId"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual bool RemoveHash( string hashId, string key)
        {
            return ServiceStackHelper.RemoveHash(RedisClient, hashId, key);
        }
        /// <summary>
        /// remote   hash  key
        /// </summary>
        /// <param name="hashId"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual bool RemoveHashPresistence(string hashId, string key)
        {
            return ServiceStackHelper.RemoveHashPresistence(RedisClient, hashId, key);
        }


        /// <summary>
        /// subscribe
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="ac"></param>
        public virtual void Subscribe(string channel, Action<string, byte[]> ac)
        {
            ServiceStackHelper.Subscribe(RedisClient, channel, ac);
        }

        /// <summary>
        /// subscribe
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="ac"></param>
        public virtual void SubscribePresistence(string channel, Action<string, byte[]> ac)
        {
            ServiceStackHelper.SubscribePresistence(RedisClient, channel, ac);
        }
        /// <summary>
        /// publish
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="msg"></param>
        public virtual void Publish(string channel, byte[] msg)
        {
            ServiceStackHelper.Publish(RedisClient, channel, msg);
        }

        /// <summary>
        /// publish
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="msg"></param>
        public virtual void PublishPresistence(string channel, byte[] msg)
        {
             ServiceStackHelper.PublishPresistence(RedisClient, channel, msg);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Dispose()
        {
            RedisClient.Dispose();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum DataStrucactorFlag
    {
        /// <summary>
        /// 
        /// </summary>
        String,
        /// <summary>
        /// 
        /// </summary>
        List,
        /// <summary>
        /// 
        /// </summary>
        Set,
        /// <summary>
        /// 
        /// </summary>
        SortedSet,
        /// <summary>
        /// 
        /// </summary>
        Hash
    }


    /// <summary>
    /// 
    /// </summary>
    public class ServiceStackHelper
    {  

        /// <summary>获取redis数据库链接 </summary>
        public  static RedisClient GetRedis(string connectionString,int port,string pwd,int db = 0)
        {
            RedisClient redisClient = new RedisClient(connectionString, port, pwd, db);
            return redisClient;
        }

        /// <summary>
        /// add string or set or sortedset
        /// </summary>
        /// <param name="redisClient"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="dataStrucactorFlag"></param>
        /// <param name="expiresAt"></param>
        /// <param name="hashId">hash use</param>
        /// <returns></returns>
        public static bool Add(RedisClient redisClient,  string key, string val, DataStrucactorFlag dataStrucactorFlag= DataStrucactorFlag.String, DateTime? expiresAt = null, string hashId="")
        {
            if(dataStrucactorFlag== DataStrucactorFlag.String)
            {
                return expiresAt.HasValue ? redisClient.Add(key, val, expiresAt.Value) : redisClient.Add(key, val);
            }
            if (dataStrucactorFlag == DataStrucactorFlag.List)
            {
                redisClient.AddItemToList(key, val);
                if (expiresAt.HasValue)
                {
                  return  Expire(redisClient, key, (int)DateUtils.TotalSeconds(expiresAt.Value));
                }
                return true;
            }
            if (dataStrucactorFlag == DataStrucactorFlag.Set)
            {
               redisClient.AddItemToSet(key, val);
                if (expiresAt.HasValue)
                {
                    return Expire(redisClient, key, (int)DateUtils.TotalSeconds(expiresAt.Value));
                }
                return true;
            }
            if (dataStrucactorFlag == DataStrucactorFlag.SortedSet)
            {
                bool res= redisClient.AddItemToSortedSet(key, val);
                if (res&& expiresAt.HasValue)
                {
                    return Expire(redisClient, key, (int)DateUtils.TotalSeconds(expiresAt.Value));
                }
                return res;
            }
         
            if (dataStrucactorFlag == DataStrucactorFlag.Hash)
            {
                bool res=redisClient.SetEntryInHash(hashId,key, val);
                if (res&&expiresAt.HasValue)
                {
                   return Expire(redisClient, key, (int)DateUtils.TotalSeconds(expiresAt.Value));
                }
                return res; 
            }
            return false;
        }

        /// <summary>
        /// add string or set or sortedset presistence
        /// </summary>
        /// <param name="redisClient"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="dataStrucactorFlag"></param>
        /// <param name="expiresAt"></param>
        /// <param name="hashId">hash use</param>
        /// <returns></returns>
        public static bool AddPresistence(RedisClient redisClient, string key, string val, DataStrucactorFlag dataStrucactorFlag = DataStrucactorFlag.String, DateTime? expiresAt = null, string hashId = "")
        {
            try
            {
               return Add(redisClient, key, val, dataStrucactorFlag, expiresAt,hashId);
            }
            finally
            {
                redisClient.Save();
            }
        }

        /// <summary>
        /// add string  
        /// </summary>
        /// <param name="redisClient"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="expiresAt"></param>
        /// <returns></returns>
        public static bool ReplaceString(RedisClient redisClient,  string key, string val, DateTime? expiresAt = null)
        {
            return expiresAt.HasValue ? redisClient.Replace(key, val, expiresAt.Value) : redisClient.Replace(key, val);
        }

        /// <summary>
        /// add string presistence
        /// </summary>
        /// <param name="redisClient"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="expiresAt"></param>
        /// <returns></returns>

        public static bool ReplaceStringPresistence(RedisClient redisClient, string key, string val, DateTime? expiresAt = null)
        {
            try
            {
                return ReplaceString(redisClient,  key, val, expiresAt);
            }
            finally
            {
                redisClient.Save();
            }
        }

        /// <summary>
        /// key expire
        /// </summary>
        /// <param name="redisClient"></param>
        /// <param name="key"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static bool Expire(RedisClient redisClient, string key, int seconds)
        {
            return redisClient.Expire(key, seconds);
        }

        /// <summary>
        /// key expire presistence
        /// </summary>
        /// <param name="redisClient"></param>
        /// <param name="key"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static bool ExpirePresistence(RedisClient redisClient,string key, int seconds)
        {
            try
            {
                return Expire(redisClient, key, seconds);
            }
            finally
            {
                redisClient.Save();
            }
        }



        /// <summary>
        /// remove key 
        /// </summary>
        /// <param name="redisClient"></param>
        /// <param name="key"></param>
        /// <param name="dataStrucactorFlag"></param>
        /// <returns></returns>
        public static bool RemoveKey(RedisClient redisClient,  string key,DataStrucactorFlag dataStrucactorFlag= DataStrucactorFlag.String)
        {
            if (dataStrucactorFlag == DataStrucactorFlag.String)
            {
                return redisClient.Remove(key);
            }
            if (dataStrucactorFlag == DataStrucactorFlag.Set)
            {
                return redisClient.Remove(key);
            }
            if (dataStrucactorFlag == DataStrucactorFlag.SortedSet)
            {
                return redisClient.Remove(key);
            }
            if (dataStrucactorFlag == DataStrucactorFlag.List)
            {
                redisClient.RemoveAllFromList(key);
                return true;
            }
            return redisClient.Remove(key);
        }

        /// <summary>
        /// remove key presistence
        /// </summary>
        /// <param name="redisClient"></param>
        /// <param name="key"></param>
        /// <param name="dataStrucactorFlag"></param>
        /// <returns></returns>
        public static bool RemoveKeyPresistence(RedisClient redisClient,  string key, DataStrucactorFlag dataStrucactorFlag = DataStrucactorFlag.String)
        {
            try
            {
                return RemoveKey(redisClient, key,dataStrucactorFlag);
            }
            finally
            {
                redisClient.Save();
            }
        }


        /// <summary>
        /// get string or set or sortedset
        /// </summary>
        /// <param name="redisClient"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetString(RedisClient redisClient, string key)
        {
            return redisClient.Get<string>(key);
        }



        /// <summary>
        /// remove first list item,but return item
        /// </summary>
        /// <param name="redisClient"></param>
        /// <param name="key"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static string RemoveFirstList(RedisClient redisClient, string key, TimeSpan? timeout)
        {
            if (timeout.HasValue)
            {
                return redisClient.BlockingRemoveStartFromList(key, timeout);
            }
            return redisClient.RemoveStartFromList(key);
        }

        /// <summary>
        /// remove last list item,but return item
        /// </summary>
        /// <param name="redisClient"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string RemoveLastList(RedisClient redisClient, string key)
        {
            return redisClient.RemoveEndFromList(key);
        }

        /// <summary>
        /// add list or set or sortedset
        /// </summary>
        /// <param name="redisClient"></param>
        /// <param name="key"></param>
        /// <param name="vals"></param>
        /// <param name="dataStrucactorFlag"></param>
        /// <returns></returns>
        public static bool Add(RedisClient redisClient,string key, List<String> vals, DataStrucactorFlag dataStrucactorFlag = DataStrucactorFlag.List)
        {
            if (dataStrucactorFlag == DataStrucactorFlag.List)
            {
                redisClient.AddRangeToList(key, vals);
                return true;
            }
            if (dataStrucactorFlag == DataStrucactorFlag.Set)
            {
                redisClient.AddRangeToSet(key, vals);
                return true;
            }
            if (dataStrucactorFlag == DataStrucactorFlag.SortedSet)
            {
                redisClient.AddRangeToSortedSet(key, vals,-1);
                return true;
            }
            return false;
        }

        /// <summary>
        /// remove first list item,but return item
        /// </summary>
        /// <param name="redisClient"></param>
        /// <param name="key"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static string RemoveFirstListPresistence(RedisClient redisClient,string key, TimeSpan? timeout)
        {
            try
            {
                return RemoveFirstList(redisClient, key, timeout);
            }
            finally
            {
                redisClient.Save();
            }
        }

        /// <summary>
        /// remove last list item,but return item
        /// </summary>
        /// <param name="redisClient"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string RemoveLastListPresistence(RedisClient redisClient, string key)
        {
            try
            {
                return RemoveLastList(redisClient,  key);
            }
            finally
            {
                redisClient.Save();
            }
        }

        /// <summary>
        /// pop list or set or sortedset but not remove
        /// </summary>
        /// <param name="redisClient"></param>
        /// <param name="key"></param>
        /// <param name="dataStrucactorFlag"></param>
        /// <returns></returns>
        public static string Pop(RedisClient redisClient, string key, DataStrucactorFlag dataStrucactorFlag = DataStrucactorFlag.List)
        {
            if (dataStrucactorFlag == DataStrucactorFlag.List)
            {
                return redisClient.PopItemFromList(key);
            }
            if (dataStrucactorFlag == DataStrucactorFlag.Set)
            {
                return redisClient.PopItemFromSet(key);
            }
            if (dataStrucactorFlag == DataStrucactorFlag.SortedSet)
            {
                return redisClient.PopItemWithLowestScoreFromSortedSet(key);
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
        public static List<string> Get(RedisClient redisClient, string key,DataStrucactorFlag dataStrucactorFlag= DataStrucactorFlag.List, int start = -1, int end = -1)
        {
            if (dataStrucactorFlag == DataStrucactorFlag.List)
            {
                if (start != -1 && start > 0
                  && end != -1 || end > 0)
                {
                    return redisClient.GetRangeFromList(key, start, end);
                }
                return redisClient.GetAllItemsFromList(key);
            }
            if (dataStrucactorFlag == DataStrucactorFlag.Set)
            {
                return redisClient.GetAllItemsFromSet(key).ToList();
            }
            if (dataStrucactorFlag == DataStrucactorFlag.SortedSet)
            {
                if (start != -1 && start > 0
                  && end != -1 || end > 0)
                {
                    return redisClient.GetRangeFromSortedSet(key, start, end);
                }
                return redisClient.GetAllItemsFromSortedSet(key);
            }

            return null;
        }
        /// <summary>
        /// add list or set or sortedset presistence
        /// </summary>
        /// <param name="redisClient"></param>
        /// <param name="key"></param>
        /// <param name="vals"></param>
        /// <param name="dataStrucactorFlag"></param>
        /// <returns></returns>
        public static bool AddPresistence(RedisClient redisClient,  string key, List<string> vals, DataStrucactorFlag dataStrucactorFlag = DataStrucactorFlag.List)
        {
            try
            {
                return Add(redisClient, key, vals, dataStrucactorFlag);
            }
            finally
            {
                redisClient.Save();
            }
        }

        /// <summary>
        /// remove  list or set or sortedset
        /// </summary>
        /// <param name="redisClient"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="dataStrucactorFlag"></param>
        /// <returns></returns>
        public static bool Remove(RedisClient redisClient,  string key, string val,DataStrucactorFlag dataStrucactorFlag= DataStrucactorFlag.List)
        {
            if(dataStrucactorFlag == DataStrucactorFlag.List)
            {
               return redisClient.RemoveItemFromList(key,val)>0;
            }
            if (dataStrucactorFlag == DataStrucactorFlag.Set)
            {
                redisClient.RemoveItemFromSet(key, val);
                return true;
            }
            if (dataStrucactorFlag == DataStrucactorFlag.SortedSet)
            {
               return redisClient.RemoveItemFromSortedSet(key, val);
            }
            return false;
        }

        /// <summary>
        /// remove list or set or sortedset presistence
        /// </summary>
        /// <param name="redisClient"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="dataStrucactorFlag"></param>
        /// <returns></returns>
        public static bool RemovePresistence(RedisClient redisClient,string key, string val, DataStrucactorFlag dataStrucactorFlag = DataStrucactorFlag.List)
        {
            try
            {
                return Remove(redisClient,  key, val, dataStrucactorFlag);
            }
            finally
            {
                redisClient.Save();
            }
        }
        /// <summary>
        /// get all hash key
        /// </summary>
        /// <param name="redisClient"></param>
        /// <param name="hashId"></param>
        /// <returns></returns>
        public static List<string> GetHashKey(RedisClient redisClient, string hashId)
        {
            return redisClient.GetHashKeys(hashId);
        }

        /// <summary>
        /// get all hash val
        /// </summary>
        /// <param name="redisClient"></param>
        /// <param name="hashId"></param>
        /// <returns></returns>
        public static List<string> GetHashValue(RedisClient redisClient, string hashId)
        {
            return redisClient.GetHashValues(hashId);
        }

        /// <summary>
        /// get  hash val
        /// </summary>
        /// <param name="redisClient"></param>
        /// <param name="hashId"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetHashValue(RedisClient redisClient, string hashId,string key)
        {
            return redisClient.GetValueFromHash(hashId, key);
        }
        /// <summary>
        /// remote   hash  key
        /// </summary>
        /// <param name="redisClient"></param>
        /// <param name="hashId"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool RemoveHash(RedisClient redisClient, string hashId, string key)
        {
            return redisClient.RemoveEntryFromHash(hashId, key);
        }

        /// <summary>
        /// remote   hash  key
        /// </summary>
        /// <param name="redisClient"></param>
        /// <param name="hashId"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool RemoveHashPresistence(RedisClient redisClient, string hashId, string key)
        {
            try
            {
                return redisClient.RemoveEntryFromHash(hashId, key);
            }
            finally
            {
                redisClient.Save();
            }
        }

        /// <summary>
        /// subscribe
        /// </summary>
        /// <param name="redisClient"></param>
        /// <param name="channel"></param>
        /// <param name="ac"></param>
        public static void Subscribe(RedisClient redisClient,string channel, Action<string, byte[]> ac)
        {
            var msgs = redisClient.Subscribe(new string[] { channel });
            if (msgs != null && msgs.Length == 0)
            {
                ac?.Invoke(channel, msgs[0]);
            }
        }

        /// <summary>
        /// subscribe presistence
        /// </summary>
        /// <param name="redisClient"></param>
        /// <param name="channel"></param>
        /// <param name="ac"></param>
        public static void SubscribePresistence (RedisClient redisClient, string channel, Action<string, byte[]> ac)
        {
            try
            {
                Subscribe(redisClient, channel, ac);
            }
            finally
            {
                redisClient.Save();
            }
        }

        /// <summary>
        /// publish
        /// </summary>
        /// <param name="redisClient"></param>
        /// <param name="channel"></param>
        /// <param name="msg"></param>
        public static void Publish(RedisClient redisClient,string channel, byte[] msg)
        {
            var res = redisClient.Publish(channel, msg);
            //Console.WriteLine(res);
        }

        /// <summary>
        /// publish  presistence
        /// </summary>
        /// <param name="redisClient"></param>
        /// <param name="channel"></param>
        /// <param name="msg"></param>
        public static void PublishPresistence(RedisClient redisClient, string channel, byte[] msg)
        {
            try
            {
                Publish(redisClient,  channel, msg);
            }
            finally
            {
                redisClient.Save();
            }
        }
    }
}
#endif