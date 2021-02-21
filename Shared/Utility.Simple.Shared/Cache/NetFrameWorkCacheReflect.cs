#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
using System;
using System.Globalization;

namespace Utility.Cache
{
    /// <summary>
    /// supprt net or netcore cache ,be based on reflect.
    /// </summary>
    public partial class CacheContentReflect
    {
        /// <summary>
        /// System.Web.HttpRuntime.Cache reflect implement 
        /// </summary>
        private class NetFrameWorkCacheReflect : BaseCache, ICacheContent
        {
            private readonly /*System.Web.Caching.Cache*/ object _objCache;//cache object : System.Web.HttpRuntime.Cache

            private const string HttpRuntimeFullName = "System.Web.HttpRuntime,System.Web";//static variable value belonging to class full name
            public NetFrameWorkCacheReflect()
            {
                //_objCache = System.Web.HttpRuntime.Cache;
                Type type = Type.GetType(HttpRuntimeFullName);
                this.IsSupport = type != null;
                if (this.IsSupport)
                {
                    _objCache = type.GetProperty("Cache").GetValue(null, null);//get static variable value :System.Web.HttpRuntime.Cache
                }
            }

            public bool IsSupport { get; private set; }

            /// <summary>be based on reflect implement according cache item key get cache item </summary>
            /// <typeparam name="T">cache object type</typeparam>
            /// <param name="key">cache item key</param>
            /// <returns>return get cache object</returns>
            public override T Get<T>(string key)
            {
                if (!this.IsSupport) return default(T);//not found
                return (T)_objCache.GetType().GetMethod("Get", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).Invoke(_objCache,
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance, null, new object[] { key }, CultureInfo.CurrentCulture);
                // return (T)_objCache.Get(key);
            }

            /// <summary>be based on reflect implement according cache item key set cache item</summary>
            /// <typeparam name="T">cache object type</typeparam>
            /// <param name="key">cache item key</param>
            /// <param name="t">cache object</param>
            /// <param name="expire">set expire time</param>
            /// <returns>return  true for set cache item succcess,if or return false.</returns>
            public override bool Set<T>(string key, T t, DateTime expire)
            {
                if (!this.IsSupport) return false;//not found 
                var obj = Get<T>(key);
                if (obj != null)
                {
                    Remove(key);
                }
                _objCache.GetType().GetMethod("Insert", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).Invoke(_objCache,
       System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance, null, new object[] { key, t, null, DateTime.Now, new TimeSpan((expire - DateTime.Now).Ticks) }, CultureInfo.CurrentCulture);
                //_objCache.Insert(key, t, null, DateTime.Now, new TimeSpan((expire - DateTime.Now).Ticks));
                return true;
            }

            /// <summary>be based on reflect implement remove a cache item</summary>
            /// <param name="key">cache item key</param>
            /// <returns>return  true for remove a cache item succcess,if or return false.</returns>
            public override bool Remove(string key)
            {
                if (!this.IsSupport) return false;//not found 
                _objCache.GetType().GetMethod("Remove", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).Invoke(_objCache,
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance, null, new object[] { key }, CultureInfo.CurrentCulture);
                //_objCache.Remove(key);
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