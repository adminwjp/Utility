#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
using System;
using Utility.Validate;

namespace Utility.Cache
{
    /// <summary>
    /// supprt net or netcore cache ,be based on reflect.
    /// </summary>
    public partial class CacheContentReflect : BaseCache, ICacheContent
    {
        /// <summary>
        /// Microsoft.Extensions.Caching.Memory.MemoryCache,Microsoft.Extensions.Caching.Memorye reflect implement 
        /// </summary>
        private class NetCoreCacheReflect : BaseCache, ICacheContent
        {
            private const string TypeName = "Microsoft.Extensions.Caching.Memory.MemoryCache,Microsoft.Extensions.Caching.Memory";
            private readonly Type type;
            private readonly object _objCache;//declare IMemoryCache object

            /// <summary> 
            /// no param constractor 
            /// </summary>
            /// <exception cref="DllNotFoundException">Microsoft.Extensions.Caching.Memory</exception>
            /// <example>
            /// _objCache = Activator.CreateInstance(type, new object[] { Activator.CreateInstance(Type.GetType("Microsoft.Extensions.Caching.Memory.MemoryCacheOptions,Microsoft.Extensions.Caching.Memory")) });
            /// </example>
            public NetCoreCacheReflect()
            {
                type = Type.GetType(TypeName);
                this.IsSupport = type != null;
                if (!this.IsSupport) return;//throw new DllNotFoundException("Microsoft.Extensions.Caching.Memory");
                _objCache = Activator.CreateInstance(type, new object[] { Activator.CreateInstance(Type.GetType("Microsoft.Extensions.Caching.Memory.MemoryCacheOptions,Microsoft.Extensions.Caching.Memory")) });
            }

            /// <summary>
            /// has param constractor net core 
            /// </summary>
            /// <param name="memoryCache">Microsoft.Extensions.Caching.Memory.IMemoryCache,Microsoft.Extensions.Caching.Memory</param>
            /// <exception cref="DllNotFoundException">Microsoft.Extensions.Caching.Memory</exception>
            public NetCoreCacheReflect(object memoryCache)
            {
                ValidateHelper.ValidateArgumentObjectNull("memoryCache", memoryCache);
                type = memoryCache.GetType();
                Type origionType = Type.GetType("Microsoft.Extensions.Caching.Memory.IMemoryCache,Microsoft.Extensions.Caching.Memory");
#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
                this.IsSupport = origionType != null && type.IsAssignableFrom(origionType);
                if (!this.IsSupport) return;// throw new DllNotFoundException("Microsoft.Extensions.Caching.Memory");
#endif
                _objCache = memoryCache;
            }

            public bool IsSupport { get; private set; }

            /// <summary>according cache item key get cache item </summary>
            /// <typeparam name="T">cache object type</typeparam>
            /// <param name="key">cache item key</param>
            /// <returns>return get cache object</returns>
            public override T Get<T>(string key)
            {
                if (!IsSupport) return default(T);
                T val = default(T);
                type.GetMethod("TryGetValue").Invoke(_objCache, new object[] { key, val });
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
                if (!IsSupport) return false;
                var obj = Get<T>(key);
                if (obj != null)
                {
                    Remove(key);
                }
                object entry = type.GetMethod("CreateEntry").Invoke(_objCache, new object[] { key });
                entry.GetType().GetProperty("Value").SetValue(entry, t, null);
                entry.GetType().GetProperty("AbsoluteExpiration").SetValue(entry, expire, null);
                return true;
            }

            /// <summary> remove a cache item</summary>
            /// <param name="key">cache item key</param>
            /// <returns>return  true for remove a cache item succcess,if or return false.</returns>
            public override bool Remove(string key)
            {
                if (!IsSupport) return false;
                type.GetMethod("Remove").Invoke(_objCache, new object[] { key });
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
}
#endif