using Newtonsoft.Json;
using System;

namespace Utility.Json
{
    /// <summary>
    ///Newtonsoft  实现
    /// </summary>
    public class NewtonsoftJson:IJson
    {
        /// <summary>
        /// NewtonsoftJson
        /// </summary>
        public static readonly NewtonsoftJson Empty = new NewtonsoftJson();
        /// <summary> json 字符串 转对象 </summary>
        /// <param name="json">json 字符串</param>
        /// <returns></returns>
        public  virtual object ToObject(string json) => JsonToObject(json);

        /// <summary>  对象  转  json 字符串 </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public virtual  string ToJson(object obj) => ObjectToJson(obj);

        /// <summary>  对象  转  json 字符串 </summary>
        /// <param name="json">字符串</param>
        public  virtual T ToObject<T>(string json) => JsonToObject<T>(json);

        /// <summary> json 字符串 转对象 </summary>
        /// <param name="json">json 字符串</param>
        /// <returns></returns>
        public static object JsonToObject(string json) => string.IsNullOrEmpty(json) ? null : JsonConvert.DeserializeObject(json, new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });

        /// <summary>  对象  转  json 字符串 </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static string ObjectToJson(object obj) => obj == null ? "{}" : JsonConvert.SerializeObject(obj,
            Formatting.None,
           new JsonSerializerSettings()
           {
               ReferenceLoopHandling = ReferenceLoopHandling.Ignore
           });

        /// <summary>  对象  转  json 字符串 </summary>
        /// <param name="json">字符串</param>
        public static T JsonToObject<T>(string json) => string.IsNullOrEmpty(json) ? default(T) : JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });
    }

    /// <summary>json 公共类 </summary>
    public partial class JsonHelper
    {
        static JsonHelper()
        {
#if !(NET10 || NET20 || NET30 || NET35 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
            JsonHelper.Json = DataContractJsonSerializerJson.Json;
#endif
            JsonHelper.Json = NewtonsoftJson.Empty;
        }
        /// <summary>  a_b_c ABC </summary>
        public static  readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings()
        {
            //忽略循环引用
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            //使用 ab_c AbC  实际 AbC  ab_c 
            ContractResolver = JsonContractResolver.ObjectResolverJson,
            //JsonContractResolver.JsonResolverObject,
                                                                       //设置时间格式
            DateFormatString = "yyyy-MM-dd HH:mm:ss"
        };

        /// <summary> json 字符串 转对象</summary>
        /// <param name="json">json 字符串</param>
        /// <param name="jsonSerializer"></param>
        /// <returns></returns>
        public static object ToObject(string json,JsonSerializerSettings jsonSerializer) => string.IsNullOrEmpty(json) ? null : JsonConvert.DeserializeObject(json,jsonSerializer);

        /// <summary> 对象  转  json 字符串 </summary>
        /// <param name="obj">对象</param>
        /// <param name="jsonSerializer"></param>
        /// <returns></returns>
        public static string ToJson(object obj, JsonSerializerSettings jsonSerializer) => obj == null ? "{}" : JsonConvert.SerializeObject(obj, jsonSerializer);

        /// <summary> 对象  转  json 字符串 </summary>
        /// <param name="json">字符串</param>
        /// <param name="jsonSerializer"></param>
        /// <returns></returns>
        public static T ToObject<T>(string json, JsonSerializerSettings jsonSerializer) => string.IsNullOrEmpty(json) ? default(T) : JsonConvert.DeserializeObject<T>(json,jsonSerializer);

        /// <summary> 对象  转  json 字符串 </summary>
        /// <param name="json">字符串</param>
        /// <param name="type"></param>
        /// <param name="jsonSerializer"></param>
        /// <returns></returns>
        public static object ToObject(string json,Type type,  JsonSerializerSettings jsonSerializer) => string.IsNullOrEmpty(json) ? null : JsonConvert.DeserializeObject(json, type, jsonSerializer);
    }
}
