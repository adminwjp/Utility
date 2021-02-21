#if NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Utility.AspNetCore.Infrastructure
{
    /// <summary>
    /// 本地化设置 格式
    /// </summary>
    public class JsonStringLocalizerFactory : IStringLocalizerFactory
    {
        private readonly string _applicationName;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly LocalizationOptions _options;
        /// <summary>
        /// 本地化设置 格式
        /// </summary>
        /// <param name="hostingEnvironment"></param>
        /// <param name="localizationOptions"></param>
        public JsonStringLocalizerFactory(IHostingEnvironment hostingEnvironment, IOptions<LocalizationOptions> localizationOptions)
        {
            if (localizationOptions == null)
            {
                throw new ArgumentNullException(nameof(localizationOptions));
            }
            this._hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
            this._options = localizationOptions.Value;
            this._applicationName = hostingEnvironment.ApplicationName;
        }

        public IStringLocalizer Create(Type resourceSource)
        {
            TypeInfo typeInfo = IntrospectionExtensions.GetTypeInfo(resourceSource);
            //Assembly assembly = typeInfo.Assembly;
            //AssemblyName assemblyName = new AssemblyName(assembly.FullName);

            string baseResourceName = typeInfo.FullName;
            baseResourceName = TrimPrefix(baseResourceName, _applicationName + ".");

            return new JsonStringLocalizer(_hostingEnvironment, _options, baseResourceName, null);
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            location = location ?? _applicationName;

            string baseResourceName = baseName;
            baseResourceName = TrimPrefix(baseName, location + ".");

            return new JsonStringLocalizer(_hostingEnvironment, _options, baseResourceName, null);
        }

        private static string TrimPrefix(string name, string prefix)
        {
            if (name.StartsWith(prefix, StringComparison.Ordinal))
            {
                return name.Substring(prefix.Length);
            }

            return name;
        }
    }
}
#endif