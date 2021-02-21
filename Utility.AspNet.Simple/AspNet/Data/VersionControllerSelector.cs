#if NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.Routing;

namespace Utility.AspNet.Data
{
    internal sealed class HttpControllerTypeCache
    {
        private readonly HttpConfiguration _configuration;

        private readonly Lazy<Dictionary<string, ILookup<string, Type>>> _cache;

        internal Dictionary<string, ILookup<string, Type>> Cache => _cache.Value;

        public HttpControllerTypeCache(HttpConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }

            _configuration = configuration;
            _cache = new Lazy<Dictionary<string, ILookup<string, Type>>>(InitializeCache);
        }

        public  ICollection<Type> GetControllerTypes(string controllerName)
        {
            if (string.IsNullOrEmpty(controllerName))
            {
                throw new ArgumentNullException("controllerName");
            }

            HashSet<Type> hashSet = new HashSet<Type>();
            if (_cache.Value.TryGetValue(controllerName, out ILookup<string, Type> value))
            {
                foreach (IGrouping<string, Type> item in value)
                {
                    hashSet.UnionWith(item);
                }

                return hashSet;
            }

            return hashSet;
        }

        private Dictionary<string, ILookup<string, Type>> InitializeCache()
        {
            IAssembliesResolver assembliesResolver = _configuration.Services.GetAssembliesResolver();
            return _configuration.Services.GetHttpControllerTypeResolver().GetControllerTypes(assembliesResolver).GroupBy((Type t) => t.Name.Substring(0, t.Name.Length - DefaultHttpControllerSelector.ControllerSuffix.Length), StringComparer.OrdinalIgnoreCase)
                .ToDictionary((IGrouping<string, Type> g) => g.Key, (IGrouping<string, Type> g) => g.ToLookup((Type t) => t.Namespace ?? string.Empty, StringComparer.OrdinalIgnoreCase), StringComparer.OrdinalIgnoreCase);
        }
    }
    /// <summary>
    /// 参考 https://www.cnblogs.com/hdwgxz/p/7856619.html
    /// </summary>
    public class VersionControllerSelector : DefaultHttpControllerSelector
    {
        private const string Version = "version";
        private const string ControllerKey = "controller";
        private readonly HttpControllerTypeCache _controllerTypeCache;
        private readonly Lazy<ConcurrentDictionary<string, HttpControllerDescriptor>> _controllerInfoCache;
        private HttpConfiguration configuration;
        public VersionControllerSelector(HttpConfiguration configuration)
            : base(configuration)
        {
            if (configuration == null)
            {
                throw new  ArgumentNullException("configuration");
            }

            _controllerInfoCache = new Lazy<ConcurrentDictionary<string, HttpControllerDescriptor>>(InitializeControllerInfoCache);
            this.configuration = configuration;
            _controllerTypeCache = new HttpControllerTypeCache(configuration);
        }
        public override string GetControllerName(HttpRequestMessage request)
        {
            return SelectController(request).ControllerName;
        }
        private ConcurrentDictionary<string, HttpControllerDescriptor> InitializeControllerInfoCache()
        {
            ConcurrentDictionary<string, HttpControllerDescriptor> concurrentDictionary = new ConcurrentDictionary<string, HttpControllerDescriptor>(StringComparer.OrdinalIgnoreCase);
            HashSet<string> hashSet = new HashSet<string>();
            foreach (KeyValuePair<string, ILookup<string, Type>> item in _controllerTypeCache.Cache)
            {
                string key = item.Key;
                foreach (IGrouping<string, Type> item2 in item.Value)
                {
                    foreach (Type item3 in item2)
                    {
                        if (concurrentDictionary.Keys.Contains(key))
                        {
                            hashSet.Add(key);
                            break;
                        }

                        concurrentDictionary.TryAdd(key, new HttpControllerDescriptor(configuration, key, item3));
                    }
                }
            }

            foreach (string item4 in hashSet)
            {
                concurrentDictionary.TryRemove(item4, out HttpControllerDescriptor _);
            }

            return concurrentDictionary;
        }
        // Get a value from the route data, if present.
        private static T GetRouteVariable<T>(IHttpRouteData routeData, string name)
        {
            object result = null;
            if (routeData.Values.TryGetValue(name, out result))
            {
                return (T)result;
            }
            return default(T);
        }

        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            if (request == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            IHttpRouteData routeData = request.GetRouteData();
            if (routeData == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            // Get the namespace and controller variables from the route data.
            string version = GetRouteVariable<string>(routeData, Version);
            if (version == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            string controllerName = GetRouteVariable<string>(routeData, ControllerKey);
            if (controllerName == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            // 匹配控制器
            HttpControllerDescriptor controllerDescriptor;
            if (_controllerInfoCache.Value.TryGetValue(controllerName, out controllerDescriptor))
            {
                
                return controllerDescriptor;
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }
    }

}
#endif