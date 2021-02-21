#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
using System;
using Utility.Validate;

namespace Utility.Cache
{
    /// <summary>
    /// not support net 2.0 - 3.0 netcoreapp 1.0 - 1.2  netstandard 1.0 - 1.6 IMemcachedClient implement
    /// </summary>
    public sealed class EnyimMemcachedContentReflect : BaseCache, ICacheContent
    {
        private readonly object _memcachedClient;//declare IMemcachedClient object
        private readonly Type _type;
        /// <summary>
        /// 有时可能找不到 怎么解决了 需要手动 确定
        /// </summary>
        public static Type IMemcachedClientType { get; set; }
        /// <summary>
        /// has param constractor 
        /// </summary>
        /// <param name="memcachedClient">IMemcachedClient object,not relfect of every implement way not equal . new ()</param>
        /// <exception cref="DllNotFoundException">EnyimMemcachedCore</exception>
        public EnyimMemcachedContentReflect(object memcachedClient)
        {
            ValidateHelper.ValidateArgumentObjectNull("memcachedClient", memcachedClient);
            _memcachedClient =memcachedClient;
            _type = memcachedClient.GetType();
            this.IsSupport = _type.IsAssignableFrom(Type.GetType("Enyim.Caching.IMemcachedClient,EnyimMemcachedCore"));
            if (!this.IsSupport)
            {
                //throw new DllNotFoundException("EnyimMemcachedCore");
            }
        }
        /// <summary>
        /// support EnyimMemcached ?
        /// </summary>
        public bool IsSupport { get; private set; }

        /// <summary>according cache item key get cache item </summary>
        /// <typeparam name="T">cache object type</typeparam>
        /// <param name="key">cache item key</param>
        /// <returns>return get cache object</returns>
        public override T Get<T>(string key)
        {
            if (!IsSupport) return default(T);
            return (T)_type.GetMethod("Get").Invoke(_memcachedClient, new object[] { key });
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
            Array array = System.Enum.GetValues(Type.GetType("Enyim.Caching.Memcached.StoreMode,EnyimMemcachedCore"));
            object val = 2;
            foreach (var item in array)
            {
                if (item.ToString() == "Set")
                {
                    val = item;
                    break;
                }
            }
            return (bool)_type.GetMethod("Store").Invoke(_memcachedClient, new object[] { val, key, t, expire });
        }

        /// <summary> remove a cache item</summary>
        /// <param name="key">cache item key</param>
        /// <returns>return  true for remove a cache item succcess,if or return false.</returns>
        public override bool Remove(string key)
        {
            if (!IsSupport) return false;
            return (bool)_type.GetMethod("Remove").Invoke(_memcachedClient, new object[] { key });
        }

        /// <summary>
        /// do nothing 
        /// </summary>
        public void Dispose()
        {

        }

    }
}
#endif