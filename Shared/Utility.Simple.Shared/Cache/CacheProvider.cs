using System;

namespace Utility.Cache
{
    /// <summary>
    ///abstract cache provider
    /// </summary>
    public abstract class CacheProvider : IDisposable
    {
        /// <summary> 
        /// no param constractor 
        /// </summary>
        public CacheProvider()
        {

        }

        /// <summary>
        /// has param constractor 
        /// </summary>
        /// <param name="cacheContent">ICacheContent</param>
        public CacheProvider(ICacheContent cacheContent)
        {
            Cache = cacheContent;
        }

        /// <summary>
        /// destractor
        /// </summary>
        ~CacheProvider()
        {
            Dispose();
        }

#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)

        /// <summary>
        ///default injection  memory cache
        /// </summary>
        public static CacheProvider Create()
        {
            return new ObjectCacheProvider();
        }
#endif

        /// <summary>
        ///injection   cache
        /// </summary>
        /// <param name="cacheContent">cache object</param>
        public static CacheProvider ObjectCacheProvider(ICacheContent cacheContent)
        {
            return new ObjectCacheProvider(cacheContent);
        }

        /// <summary>
        /// cache
        /// </summary>
        public ICacheContent Cache { get;private set; }

        /// <summary>
        /// set cache, set injection
        /// </summary>
        /// <param name="cacheContent">缓存</param>
        public virtual void Set(ICacheContent cacheContent)
        {
            if (Cache != null)
            {
                Cache = null;
            }
            Cache = cacheContent;
        }

        /// <summary>
        ///  set cache, set injection,be based on reflect implement
        /// </summary>
        /// <param name="type">cache type</param>
        ///<exception cref="NotSupportedException"></exception>
        public virtual void Set(Type type)
        {
#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
            if (type.IsAssignableFrom(typeof(ICacheContent)))
            {
                if (Cache != null)
                {
                    Cache = null;
                }
                Cache = (ICacheContent)Activator.CreateInstance(type);
            }
#else
            throw new NotSupportedException(); 
#endif
        }


        /// <summary>
        /// release resources
        /// </summary>
        public virtual void Dispose()
        {
            Cache?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
