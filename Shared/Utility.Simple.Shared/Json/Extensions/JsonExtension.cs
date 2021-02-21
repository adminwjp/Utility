#if !(NET20 || NET30)
using Newtonsoft.Json;
using System;

namespace Utility.Json.Extensions
{
    /// <summary>json扩展类 </summary>
    public static class JsonExtension
    {

        /// <summary> json 字符串 转对象 </summary>
        /// <param name="json">json 字符串</param>
        /// <returns></returns>
        public static T ToObject<T>(this string json) => JsonHelper.ToObject<T>(json);

        /// <summary>  对象  转  json 字符串 </summary>
        /// <param name="json">对象</param>
        public static object ToObject(this string json) => JsonHelper.ToObject(json);

        /// <summary>  对象  转  json 字符串</summary>
        /// <param name="obj">对象</param>
        public static string ToJson(this object obj) => JsonHelper.ToJson(obj);

        /// <summary> json 字符串 转对象 </summary>
        /// <param name="json">json 字符串</param>
        /// <param name="jsonSerializer"></param>
        /// <returns></returns>
        public static object ToObject(this string json, JsonSerializerSettings jsonSerializer) => JsonHelper.ToObject(json,jsonSerializer);

        /// <summary> 对象  转  json 字符串 </summary>
        /// <param name="obj">对象</param>
        /// <param name="jsonSerializer"></param>
        /// <returns></returns>
        public static string ToJson(this object obj, JsonSerializerSettings jsonSerializer) => JsonHelper.ToJson(obj, jsonSerializer);

        /// <summary> 对象  转  json 字符串 </summary>
        /// <param name="json">字符串</param>
        /// <param name="jsonSerializer"></param>
        /// <returns></returns>
        public static T ToObject<T>(this string json, JsonSerializerSettings jsonSerializer) => JsonHelper.ToObject<T>(json, jsonSerializer);

                /// <summary> 对象  转  json 字符串 </summary>
        /// <param name="json">字符串</param>
        /// <param name="type"></param>
        /// <param name="jsonSerializer"></param>
        /// <returns></returns>
        public static object ToObject(this string json,Type type,  JsonSerializerSettings jsonSerializer)  => JsonHelper.ToObject(json,type, jsonSerializer);

    }
}
#endif