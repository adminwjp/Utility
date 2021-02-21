using System;
using System.Threading;
#if !(NET20 || NET30 || NET35)
using System.Threading.Tasks;
#endif

namespace Utility.Cache
{
    /// <summary>
    /// cache abstract base class
    /// </summary>
    public abstract class BaseCache
    {
        /// <summary>according cache item key get cache item </summary>
        /// <typeparam name="T">cache object type</typeparam>
        /// <param name="key">cache item key</param>
        /// <returns>return get cache object</returns>
        public abstract T Get<T>(string key);

        /// <summary>according cache item key set cache item</summary>
        /// <typeparam name="T">cache object type</typeparam>
        /// <param name="key">cache item key</param>
        /// <param name="t">cache object</param>
        /// <param name="expire">set expire time</param>
        /// <returns>return  true for set cache item succcess,if or return false.</returns>
        public abstract bool Set<T>(string key, T t, DateTime expire);

        /// <summary> remove a cache item</summary>
        /// <param name="key">cache item key</param>
        /// <returns>return  true for remove a cache item succcess,if or return false.</returns>
        public abstract bool Remove(string key);

#if !(NET20 || NET30 || NET35)
        /// <summary>according cache item key get cache item </summary>
        /// <typeparam name="T">cache object type</typeparam>
        /// <param name="key">cache item key</param>
        /// <param name="cancellationToken">samephore</param>
        /// <returns>return get cache object</returns>
        public virtual Task<T> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested) return new Task<T>(() => default(T));
            var obj = Get<T>(key);
            return new Task<T>(() => obj);
        }

        /// <summary>according cache item key set cache item</summary>
        /// <typeparam name="T">cache object type</typeparam>
        /// <param name="key">cache item key</param>
        /// <param name="t">cache object</param>
        /// <param name="expire">set expire time</param>
        /// <param name="cancellationToken">samephore</param>
        /// <returns>return  true for set cache item succcess,if or return false.</returns>
        public virtual Task<bool> SetAsync<T>(string key, T t, DateTime expire, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested) return new Task<bool>(() => false);
            TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>(cancellationToken);
            taskCompletionSource.SetResult(Set<T>(key, t, expire));
            return taskCompletionSource.Task;
        }
        /// <summary> remove a cache item</summary>
        /// <param name="key">cache item key</param>
        /// <param name="cancellationToken">samephore</param>
        /// <returns>return  true for remove a cache item succcess,if or return false.</returns>
        public virtual Task<bool> RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested) return new Task<bool>(() => false);
            // return Task.Factory.StartNew(() => Remove(key), cancellationToken);
            var res = Remove(key);
            return new Task<bool>(() => res);
        }
#endif
    }
}
