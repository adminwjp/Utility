using System;
using System.Threading;
#if !(NET20 || NET30 || NET35)
using System.Threading.Tasks;
#endif

namespace Utility.Cache
{
    /// <summary> cacche provider </summary>
    public class ObjectCacheProvider : CacheProvider
    {
#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)

        /// <summary>default injection  memory cache </summary>
        public ObjectCacheProvider() : this(new CacheContentReflect())
        {

        }
#endif

        /// <summary> injection cache </summary>
        /// <param name="cacheContent">cache obejct</param>
        public ObjectCacheProvider(ICacheContent cacheContent) : base(cacheContent)
        {
        }

        /// <summary>according cache item key set cache item.if exists  update,if or add</summary>
        /// <param name="key">cache item key</param>
        /// <param name="value">cache object</param>
        /// <param name="expire">set expire time</param>
        /// <returns>return  true for set cache item succcess,if or return false.</returns>
        public void Add(string key, object value, DateTime expire) => Cache.Set(key, value, expire);

        /// <summary> remove a cache item</summary>
        /// <param name="key">cache item key</param>
        /// <returns>return  true for remove a cache item succcess,if or return false.</returns>
        public void Remove(string key) => Cache.Remove(key);

        /// <summary>according cache item key get cache item </summary>
        /// <typeparam name="T">cache object type</typeparam>
        /// <param name="key">cache item key</param>
        /// <returns>return get cache object</returns>
        public T Get<T>(string key) => Cache.Get<T>(key);

#if !(NET20 || NET30 || NET35)

        /// <summary>according cache item key get cache item </summary>
        /// <typeparam name="T">cache object type</typeparam>
        /// <param name="key">cache item key</param>
        /// <param name="cancellationToken">samephore</param>
        /// <returns>return get cache object</returns>
        public Task<T> GetAsync<T>(string key, CancellationToken cancellationToken = default) => Cache.GetAsync<T>(key, cancellationToken);

        /// <summary>according cache item key set cache item. if exists  update,if or add</summary>
        /// <typeparam name="T">cache object type</typeparam>
        /// <param name="key">cache item key</param>
        /// <param name="value">cache object</param>
        /// <param name="expire">set expire time</param>
        /// <param name="cancellationToken">samephore</param>
        /// <returns>return  true for set cache item succcess,if or return false.</returns>
        public Task<bool> AddAsync<T>(string key, T value, DateTime expire, CancellationToken cancellationToken = default) => Cache.SetAsync<T>(key, value, expire, cancellationToken);

        /// <summary> remove a cache item</summary>
        /// <param name="key">cache item key</param>
        /// <param name="cancellationToken">samephore</param>
        /// <returns>return  true for remove a cache item succcess,if or return false.</returns>
        public Task<bool> RemoveAsync(string key, CancellationToken cancellationToken = default) => Cache.RemoveAsync(key,cancellationToken);

#endif
    }
}
