
using Newtonsoft.Json.Serialization;
using Utility.Utils;

namespace Utility.Json
{
    /// <summary> json 转换  </summary>
    public class JsonContractResolver: DefaultContractResolver
    {
       private  StringFormat _stringFormat = StringFormat.None;

        /// <summary>大写转小写 例如: ABC a_b_c</summary>
        public static readonly IContractResolver JsonResolverObject = new JsonContractResolver(StringFormat.InitialLetterLowerCaseUpper);

        /// <summary> 小写转大写 例如: a_b_c ABC </summary>
        public static readonly IContractResolver ObjectResolverJson = new JsonContractResolver(StringFormat.InitialLetterUpperCaseLower);



        /// <summary>无参 构造函数 </summary>
        public JsonContractResolver()
        {

        }

        /// <summary>有参 构造函数 </summary>
        /// <param name="stringFormat">转换格式</param>
        public JsonContractResolver(StringFormat stringFormat)
        {
            this._stringFormat = stringFormat;
        }

        /// <summary>属性名称解析 </summary>
        /// <param name="propertyName">属性名称</param>
        /// <returns>返回解析后属性名称</returns>
        protected override string ResolvePropertyName(string propertyName)
        {
            return StringUtils.Parse(propertyName, this._stringFormat);
        }
    }
}
