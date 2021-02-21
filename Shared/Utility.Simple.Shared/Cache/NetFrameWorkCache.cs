#if NET35 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48
//#if NET20 || NET30 || NET35 || NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48
using System;

namespace Utility.Cache
{
    /// <summary>
    /// net cache 
    /// </summary>
    public class NetFrameWorkCache : BaseCache, ICacheContent
    {
        private readonly System.Web.Caching.Cache  _objCache;

        /// <summary>
        /// no param constractor
        /// </summary>
        public NetFrameWorkCache()
        {
            _objCache = System.Web.HttpRuntime.Cache;
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsSupport { get; private set; }

        /// <summary>according cache item key get cache item </summary>
        /// <typeparam name="T">cache object type</typeparam>
        /// <param name="key">cache item key</param>
        /// <returns>return get cache object</returns>
        public override T Get<T>(string key)
        {
            return (T)_objCache.Get(key);
        }

        /// <summary>according cache item key set cache item</summary>
        /// <typeparam name="T">cache object type</typeparam>
        /// <param name="key">cache item key</param>
        /// <param name="t">cache object</param>
        /// <param name="expire">set expire time</param>
        /// <returns>return  true for set cache item succcess,if or return false.</returns>
        public override bool Set<T>(string key, T t, DateTime expire)
        {
            var obj = Get<T>(key);
            if (obj != null)
            {
                Remove(key);
            }
            _objCache.Insert(key, t, null, DateTime.Now, new TimeSpan((expire - DateTime.Now).Ticks));
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