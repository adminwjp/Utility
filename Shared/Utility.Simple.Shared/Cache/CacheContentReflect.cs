#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
using System;

namespace Utility.Cache
{
    /// <summary>
    /// be based on net framework  System.Web.Caching.Cache cache, be based on net  core   Microsoft.Extensions.Caching.Memory.MemoryCache cache
    /// </summary>
    public partial class CacheContentReflect : BaseCache, ICacheContent
    {
        private static readonly NetFrameWorkCacheReflect _netFrameWorkCache = new NetFrameWorkCacheReflect();
        private readonly NetCoreCacheReflect _netCoreCache;


        /// <summary> 
        /// no param constractor 
        /// </summary>
        public CacheContentReflect()
        {
            this.IsSupport=_netFrameWorkCache.IsSupport;
            if (this.IsSupport)
            {
                return;
            }
            _netCoreCache = new NetCoreCacheReflect();
            this.IsSupport = _netCoreCache.IsSupport;
        }

        /// <summary>
        /// has param constractor net core 
        /// </summary>
        /// <param name="memoryCache">Microsoft.Extensions.Caching.Memory.IMemoryCache,Microsoft.Extensions.Caching.Memory</param>
        /// <exception cref="DllNotFoundException">Microsoft.Extensions.Caching.Memory</exception>
        public CacheContentReflect(object memoryCache)
        {
            this.IsSupport = _netFrameWorkCache.IsSupport;
            if (this.IsSupport)
            {
                return;
            }
            _netCoreCache = new NetCoreCacheReflect(memoryCache);
            this.IsSupport = _netCoreCache.IsSupport;
        }
       /// <summary>
       /// is support true 
       /// </summary>
        public bool IsSupport { get; private set; }

        /// <summary>according cache item key get cache item </summary>
        /// <typeparam name="T">cache object type</typeparam>
        /// <param name="key">cache item key</param>
        /// <returns>return get cache object</returns>
        public override T Get<T>(string key)
        {
            if (!IsSupport) return default(T);
            if (_netCoreCache != null)
            {
               return _netCoreCache.Get<T>(key);
            }
            else
            {
                return _netFrameWorkCache.Get<T>(key);
            }
        }

        /// <summary>according cache item key set cache item</summary>
        /// <typeparam name="T">cache object type</typeparam>
        /// <param name="key">cache item key</param>
        /// <param name="t">cache object</param>
        /// <param name="expire">set expire time</param>
        /// <returns>return  true for set cache item succcess,if or return false.</returns>
        public override bool Set<T>(string key, T t, DateTime expire)
        {
            if (!IsSupport) return false;
            if (_netCoreCache != null)
            {
                return _netCoreCache.Set(key,t,expire);
            }
            else
            {
                return _netFrameWorkCache.Set(key, t, expire);
            }
        }

        /// <summary> remove a cache item</summary>
        /// <param name="key">cache item key</param>
        /// <returns>return  true for remove a cache item succcess,if or return false.</returns>
        public override bool Remove(string key)
        {
            if (!IsSupport) return false;
            if (_netCoreCache != null)
            {
                return _netCoreCache.Remove(key);
            }
            else
            {
                return _netFrameWorkCache.Remove(key);
            }
        }

        /// <summary>
        /// release resources
        /// </summary>
        public void Dispose()
        {
            this.IsSupport = _netFrameWorkCache.IsSupport;
            if (this.IsSupport)
            {
                _netFrameWorkCache.Dispose();
                return;
            }
            _netCoreCache.Dispose();
        }
    }
}
#endif