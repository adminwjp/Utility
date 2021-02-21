#if (NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_2)
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Options;
using System;
using Utility.Validate;

namespace Utility.Cache
{
    /// <summary>
    /// Microsoft.Extensions.Caching.Memory.MemoryCache,Microsoft.Extensions.Caching.Memorye reflect implement 
    /// </summary>
    public class NetCoreCache : BaseCache, ICacheContent
    {
        private readonly IMemoryCache _objCache;//declare IMemoryCache object

        /// <summary> 
        /// no param constractor 
        /// </summary>
        /// <exception cref="DllNotFoundException">Microsoft.Extensions.Caching.Memory</exception>
        /// <example>
        /// _objCache = Activator.CreateInstance(type, new object[] { Activator.CreateInstance(Type.GetType("Microsoft.Extensions.Caching.Memory.MemoryCacheOptions,Microsoft.Extensions.Caching.Memory")) });
        /// </example>
        public NetCoreCache()
        {
            _objCache = new MemoryCache(new MemoryCacheOptions() { Clock= new SystemClock(),SizeLimit=10*1024});
        }

        /// <summary>
        /// has param constractor net core 
        /// </summary>
        /// <param name="memoryCache">Microsoft.Extensions.Caching.Memory.IMemoryCache,Microsoft.Extensions.Caching.Memory</param>
        /// <exception cref="DllNotFoundException">Microsoft.Extensions.Caching.Memory</exception>
        public NetCoreCache(IMemoryCache memoryCache)
        {
            ValidateHelper.ValidateArgumentObjectNull("memoryCache", memoryCache);
            _objCache = memoryCache;
        }
           
        /// <summary>
        /// is support
        /// </summary>
        public bool IsSupport { get; private set; }=true;

        /// <summary>according cache item key get cache item </summary>
        /// <typeparam name="T">cache object type</typeparam>
        /// <param name="key">cache item key</param>
        /// <returns>return get cache object</returns>
        public override T Get<T>(string key)
        {
            _objCache.TryGetValue(key,out T val);
            return val;
        }

        /// <summary>according cache item key set cache item</summary>
        /// <typeparam name="T">cache object type</typeparam>
        /// <param name="key">cache item key</param>
        /// <param name="t">cache object</param>
        /// <param name="expire">set expire time</param>
        /// <returns>return  true for set cache item succcess,if or return false.</returns>
        public override bool Set<T>(string key, T t, DateTime expire)
        {
            _objCache.Set(key, t,expire);
            return true;
        }

        /// <summary> remove a cache item</summary>
        /// <param name="key">cache item key</param>
        /// <returns>return  true for remove a cache item succcess,if or return false.</returns>
        public override bool Remove(string key)
        {
            _objCache.Remove(key);
            return true;
        }

        /// <summary>
        /// do nothing
        /// </summary>
        public virtual void Dispose()
        {

        }
    }
}
#endif