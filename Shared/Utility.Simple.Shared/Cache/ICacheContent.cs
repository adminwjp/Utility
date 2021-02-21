using System;
using System.Threading;
#if !(NET20 || NET30 || NET35)
using System.Threading.Tasks;
#endif

namespace Utility.Cache
{
    #region cache interface
    /// <summary> cache interface </summary>
    public interface  ICacheContent:IDisposable
    {
        /// <summary>according cache item key get cache item </summary>
        /// <typeparam name="T">cache object type</typeparam>
        /// <param name="key">cache item key</param>
        /// <returns>return get cache object</returns>
         T Get<T>(string key);

        /// <summary>according cache item key set cache item</summary>
        /// <typeparam name="T">cache object type</typeparam>
        /// <param name="key">cache item key</param>
        /// <param name="t">cache object</param>
        /// <param name="expire">set expire time</param>
        /// <returns>return  true for set cache item succcess,if or return false.</returns>
        bool Set<T>(string key, T t, DateTime expire);

        /// <summary> remove a cache item</summary>
        /// <param name="key">cache item key</param>
        /// <returns>return  true for remove a cache item succcess,if or return false.</returns>
        bool Remove(string key);

#if !(NET20 || NET30 || NET35)
        /// <summary>according cache item key get cache item </summary>
        /// <typeparam name="T">cache object type</typeparam>
        /// <param name="key">cache item key</param>
        /// <param name="cancellationToken">samephore</param>
        /// <returns>return get cache object</returns>
        Task<T> GetAsync<T>(string key, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>according cache item key set cache item</summary>
        /// <typeparam name="T">cache object type</typeparam>
        /// <param name="key">cache item key</param>
        /// <param name="t">cache object</param>
        /// <param name="expire">set expire time</param>
        /// <param name="cancellationToken">samephore</param>
        /// <returns>return  true for set cache item succcess,if or return false.</returns>
       Task<bool> SetAsync<T>(string key, T t, DateTime expire, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary> remove a cache item</summary>
        /// <param name="key">cache item key</param>
        /// <param name="cancellationToken">samephore</param>
        /// <returns>return  true for remove a cache item succcess,if or return false.</returns>
        Task<bool> RemoveAsync(string key, CancellationToken cancellationToken = default(CancellationToken));
#endif

    }
    #endregion cache interface
}
