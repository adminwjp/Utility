#if NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0
using System.Text.Json;
using Utility.Utils;

namespace Utility.Json
{
    public class JsonPropertyNamingPolicy : JsonNamingPolicy
    {
        private StringFormat _stringFormat = StringFormat.None;

        public static readonly JsonPropertyNamingPolicy Empty = new JsonPropertyNamingPolicy();

        /// <summary>大写转小写 例如: ABC a_b_c</summary>
        public static readonly JsonNamingPolicy JsonResolverObject = new JsonPropertyNamingPolicy(StringFormat.InitialLetterLowerCaseUpper);

        /// <summary> 小写转大写 例如: a_b_c ABC </summary>
        public static readonly JsonNamingPolicy ObjectResolverJson = new JsonPropertyNamingPolicy(StringFormat.InitialLetterUpperCaseLower);


        /// <summary>无参 构造函数 </summary>
        public JsonPropertyNamingPolicy()
        {

        }

        /// <summary>有参 构造函数 </summary>
        /// <param name="stringFormat">转换格式</param>
        public JsonPropertyNamingPolicy(StringFormat stringFormat)
        {
            this._stringFormat = stringFormat;
        }

       /// <summary>属性名称解析 </summary>
        public static new JsonNamingPolicy CamelCase
        {
            get { return Empty; }
        }

        /// <summary>属性名称解析 </summary>
        /// <param name="name">属性名称</param>
        /// <returns>返回解析后属性名称</returns>
        public override string ConvertName(string name)
        {
            return StringUtils.Parse(name, this._stringFormat);
        }

    }
}
#endif