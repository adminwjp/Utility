#if NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0|| NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility.Redis
{
    /// <summary>
    /// redis chache
    /// </summary>
    public interface IRedisCache:IDisposable
    {
        /// <summary>
        /// add string
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="expiresAt"></param>
        /// <returns></returns>
        bool AddString(string key, string val, DateTime? expiresAt = null);

        /// <summary>
        /// add string 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="expiresAt"></param>
        /// <returns></returns>
        bool ReplaceString(string key, string val, DateTime? expiresAt = null);

        /// <summary>
        /// get string
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string GetString(string key);
        /// <summary>
        /// add string presistence
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="expiresAt"></param>
        /// <returns></returns>
        bool AddStringPresistence(string key, string val, DateTime? expiresAt = null);

        /// <summary>
        /// add string presistence
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="expiresAt"></param>
        /// <returns></returns>

        bool ReplaceStringPresistence(string key, string val, DateTime? expiresAt = null);


        /// <summary>
        /// key expire
        /// </summary>
        /// <param name="key"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        bool Expire(string key, int seconds);

        /// <summary>
        /// key expire presistence
        /// </summary>
        /// <param name="key"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        bool ExpirePresistence(string key, int seconds);


        /// <summary>
        /// remove key 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool RemoveKey(string key);

        /// <summary>
        /// remove key presistence
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool RemoveKeyPresistence(string key);

        /// <summary>
        /// add list 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        bool AddList(string key, string val);

        /// <summary>
        /// remove all list
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool RemoveList(string key);

        /// <summary>
        /// remove first list item,but return item
        /// </summary>
        /// <param name="key"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        string RemoveFirstList(string key, TimeSpan? timeout);

        /// <summary>
        /// remove last list item,but return item
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string RemoveLastList(string key);

        /// <summary>
        /// add list
        /// </summary>
        /// <param name="key"></param>
        /// <param name="vals"></param>
        /// <returns></returns>
        bool AddList(string key, List<string> vals);

        /// <summary>
        /// add list presistence
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        bool AddListPresistence(string key, string val);

        /// <summary>
        /// remove all list
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
         bool RemoveListPresistence(string key);

        /// <summary>
        /// remove first list item,but return item
        /// </summary>
        /// <param name="key"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        string RemoveFirstListPresistence(string key, TimeSpan? timeout);

        /// <summary>
        /// remove last list item,but return item
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string RemoveLastListPresistence(string key);

        /// <summary>
        /// pop list but not remove
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string PopList(string key);

        /// <summary>
        ///get list 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        List<string> GetList(string key, int start = -1, int end = -1);

        /// <summary>
        /// add list presistence
        /// </summary>
        /// <param name="key"></param>
        /// <param name="vals"></param>
        /// <returns></returns>
        bool AddListPresistence(string key, List<String> vals);

        /// <summary>
        /// add set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        bool AddSet(string key, string val);

        /// <summary>
        ///get set 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        HashSet<string> GetSet(string key);

        /// <summary>
        /// remove set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        bool RemoveSet(string key, string val);

        /// <summary>
        /// remove set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        bool RemoveSetPresistence(string key, string val);

        /// <summary>
        ///  pop set but not remove
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string PopSet(string key);

        /// <summary>
        /// add set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="vals"></param>
        /// <returns></returns>
        bool AddSet(string key, List<String> vals);

        /// <summary>
        /// add set presistence
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        bool AddSetPresistence(string key, string val);

        /// <summary>
        /// add set presistence
        /// </summary>
        /// <param name="key"></param>
        /// <param name="vals"></param>
        /// <returns></returns>
        bool AddSetPresistence(string key, List<String> vals);

        /// <summary>
        /// add sortedset 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        bool AddSortedSet(string key, string val);

        /// <summary>
        ///get set 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
         List<string> GetSortedSet(string key, int start = -1, int end = -1);

        /// <summary>
        /// remove soetedset
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        bool RemoveSortedSet(string key, string val);

        /// <summary>
        /// remove sortedset set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        bool RemoveSortedSetPresistence(string key, string val);

        /// <summary>
        ///  pop set but not remove
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string PopSortedSet(string key);

        /// <summary>
        /// add sorted set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="vals"></param>
        /// <returns></returns>
        bool AddSortedSet(string key, List<String> vals);

        /// <summary>
        /// add sorted set presistence
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        bool AdddSortedSetPresistence(string key, string val);

        /// <summary>
        /// add sortedset presistence
        /// </summary>
        /// <param name="key"></param>
        /// <param name="vals"></param>
        /// <returns></returns>
        bool AdddSortedSetPresistence(string key, List<string> vals);

        /// <summary>
        /// add hash
        /// </summary>
        /// <param name="hashId"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
         bool AddHash(string hashId, string key, string val);

        /// <summary>
        /// add hash
        /// </summary>
        /// <param name="hashId"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        bool AddHashPresistence(string hashId, string key, string val);

        /// <summary>
        /// get all hash key
        /// </summary>
        /// <param name="hashId"></param>
        /// <returns></returns>
        List<string> GetHashKey(string hashId);

        /// <summary>
        /// get all hash val
        /// </summary>
        /// <param name="hashId"></param>
        /// <returns></returns>
        List<string> GetHashValue(string hashId);

        /// <summary>
        /// according key get  hash val
        /// </summary>
        /// <param name="hashId"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        string GetHashValue(string hashId, string key);

        /// <summary>
        /// remote   hash  key
        /// </summary>
        /// <param name="hashId"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        bool RemoveHash(string hashId, string key);

        /// <summary>
        /// remote   hash  key
        /// </summary>
        /// <param name="hashId"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        bool RemoveHashPresistence(string hashId, string key);

        /// <summary>
        /// subscribe
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="ac"></param>
        void Subscribe(string channel, Action<string, byte[]> ac);

        /// <summary>
        /// subscribe
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="ac"></param>
        void SubscribePresistence(string channel, Action<string, byte[]> ac);

        /// <summary>
        /// publish
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="msg"></param>
        void Publish(string channel, byte[] msg);

        /// <summary>
        /// publish
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="msg"></param>
         void PublishPresistence(string channel, byte[] msg);
    }

  
}
#endif