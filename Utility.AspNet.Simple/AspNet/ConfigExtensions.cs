#if NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Utility.AspNet
{
    /// <summary>
    /// 配置 
    /// </summary>
    public static  class ConfigExtensions
    {
        /// <summary>
        /// 默认 返回 xml 
        /// </summary>
        public static void AddJson()
        {
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();//使用 json 默认 xml
        }

        /// <summary>
        /// 返回 json 格式 ABC a_b_c
        /// </summary>
        /// <param name="config"></param>
        public static void ConfigureApi(this HttpConfiguration config)

        {
            var jsonFormatter = new JsonMediaTypeFormatter();
            var settings = jsonFormatter.SerializerSettings;

            IsoDateTimeConverter timeConverter = new IsoDateTimeConverter();
            //这里使用自定义日期格式
            timeConverter.DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";
            settings.Converters.Add(timeConverter);


            settings.ContractResolver = Utility.Json.JsonContractResolver.ObjectResolverJson;
            jsonFormatter.SerializerSettings = settings;
            config.Services.Replace(typeof(IContentNegotiator), new JsonContentNegotiator(jsonFormatter));
        }
        public class JsonContentNegotiator : IContentNegotiator
        {
            private readonly JsonMediaTypeFormatter _jsonFormatter;


            public JsonContentNegotiator(JsonMediaTypeFormatter formatter)
            {
                _jsonFormatter = formatter;
            }


            public ContentNegotiationResult Negotiate(Type type, HttpRequestMessage request, IEnumerable<MediaTypeFormatter> formatters)
            {
                var result = new ContentNegotiationResult(_jsonFormatter, new MediaTypeHeaderValue("application/json"));
                return result;
            }
        }

    }
}
#endif
