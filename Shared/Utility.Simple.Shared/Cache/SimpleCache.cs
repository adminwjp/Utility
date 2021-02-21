#if !( NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
using System;
using System.Collections.Generic;
using System.Threading;

namespace Utility.Cache
{
    /// <summary>
    /// simpale cache,based on key value collection store
    /// </summary>
    public class SimpleCache: BaseCache, ICacheContent,IDisposable
    {
        //  public static readonly SimpleCache Empty = new SimpleCache(new Dictionary<string, ValueEntry>());//prevent thread start-up
        internal IDictionary<string, ValueEntry> Cache;//cache collection
        private  Thread thread ;//thread
#if !(NET20 || NET30 || NET35)
        /// <summary>
        /// samephore
        /// </summary>
        private readonly CancellationTokenSource cancellationToken = new CancellationTokenSource();
#else
        /// <summary>
        /// samephore
        /// </summary>
        protected bool IsCancellationRequested { get; set; }
#endif

        /// <summary>
        ///new  SimpleCache(new Dictionary &lt;string,ValueEntry&gt;());
        /// </summary>

        public static ICacheContent Create()
        {
            return new SimpleCache(new Dictionary<string, ValueEntry>());
        }

        /// <summary>
        /// has param constractor IDictionary &lt;string,ValueEntry&gt;
        /// </summary>
        /// <param name="cache" >IDictionary &lt;string,ValueEntry&gt;</param>

        internal SimpleCache(IDictionary<string, ValueEntry> cache)
        {
            Cache = cache;
            thread = new Thread((token) => {
#if !(NET20 || NET30 || NET35)
                CancellationTokenSource cancellation = (CancellationTokenSource)token;
                while (!cancellation.IsCancellationRequested)
#else
                bool cancellation = (bool)token;
                while (!cancellation)
#endif
                {
                    foreach (var item in Cache)
                    {
                        if (item.Value.Expire < DateTime.Now.AddMilliseconds(-5))
                        {
                            cache.Remove(item.Key);
                            break;
                        }
                    }
                    Thread.Sleep(500);
                }
            });
            thread.IsBackground = true;
#if !(NET20 || NET30 || NET35)
            thread.Start(cancellationToken);
#else
            thread.Start(IsCancellationRequested);
#endif
        }

        /// <summary>
        /// stop thread
        /// </summary>
        private void Stop()
        {
#if !(NET20 || NET30 || NET35)
            cancellationToken.Cancel();
#else
            IsCancellationRequested = true;
#endif
#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)

            try
            {
                thread.Abort();
            }
            catch 
            {

            }
#endif
        }

        /// <summary>
        /// cache item entity
        /// </summary>
        internal class ValueEntry
        {
            /// <summary>
            /// cache item value 
            /// </summary>
            public object Value { get; set; }
            /// <summary>
            /// cache item expire time
            /// </summary>
            public DateTime Expire { get; set; }
        }

        /// <summary>according cache item key get cache item </summary>
        /// <typeparam name="T">cache object type</typeparam>
        /// <param name="key">cache item key</param>
        /// <returns>return get cache object</returns>
        public override T Get<T>(string key)
        {
            if (Cache.ContainsKey(key)) return (T)Cache[key].Value;
            return default;
        }

        /// <summary>according cache item key set cache item</summary>
        /// <typeparam name="T">cache object type</typeparam>
        /// <param name="key">cache item key</param>
        /// <param name="t">cache object</param>
        /// <param name="expire">set expire time</param>
        /// <returns>return  true for set cache item succcess,if or return false.</returns>
        public override bool Set<T>(string key, T t, DateTime expire)
        {
            ValueEntry valueEntry = null;
            if (Cache.ContainsKey(key))
            {
                valueEntry = Cache[key];
            }
            else
            {
                valueEntry = new ValueEntry();
                Cache.Add(key,valueEntry);
            }
            valueEntry.Value = t;
            valueEntry.Expire = expire;
            return true;
        }

        /// <summary> remove a cache item</summary>
        /// <param name="key">cache item key</param>
        /// <returns>return  true for remove a cache item succcess,if or return false.</returns>
        public override bool Remove(string key)
        {
            if (Cache.ContainsKey(key))
            {
                return Cache.Remove(key);
            }
            return false;
        }

        /// <summary>
        /// stop thread and cache clean
        /// </summary>
        public virtual void Dispose()
        {
            Stop();
            Cache.Clear();
        }
    }
}
#endif