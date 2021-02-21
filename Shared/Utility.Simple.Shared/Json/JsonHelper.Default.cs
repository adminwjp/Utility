namespace Utility.Json
{
    /// <summary>json 公共类 </summary>
    public partial class JsonHelper
    {
        /// <summary>
        /// json 接口
        /// </summary>
        public static IJson Json { get; set; }


        /// <summary> json 字符串 转对象 </summary>
        /// <param name="json">json 字符串</param>
        /// <returns></returns>
        public static object ToObject(string json) => Json.ToObject(json);

        /// <summary>  对象  转  json 字符串 </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static string ToJson(object obj) => Json.ToJson(obj);

        /// <summary>  对象  转  json 字符串 </summary>
        /// <param name="json">字符串</param>
        public static T ToObject<T>(string json) =>  Json.ToObject<T>(json);
    }
}
