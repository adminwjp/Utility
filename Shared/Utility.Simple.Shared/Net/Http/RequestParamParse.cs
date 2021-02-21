using System.Collections.Generic;
using System.Text;
#if !(NET20 || NET30)
using System.Linq;
#endif

namespace Utility.Net.Http
{
    /// <summary>
    /// post 参数 转换
    /// </summary>
    public class RequestParamParse
    {
        /// <summary>
        /// 键值 转换 为 post from 格式
        /// </summary>
        /// <param name="param">键值</param>
        /// <returns></returns>
        public string Parse(IDictionary<string,string> param)
        {
            string json = string.Empty;
#if !(NET20 || NET30 || NET35)
            json = string.Join("&", param.Select(it => $"{it.Key}={it.Value}"));
#else
            StringBuilder builder = new StringBuilder();
            foreach (var item in param)
            {
                builder.AppendFormat("{0}={1}&", item.Key, item.Value);
            }
             json = builder.ToString().TrimEnd('&');
#endif
            return json;
        }
        /// <summary>
        ///   post from 格式 转换 为  键值
        /// </summary>
        /// <param name="param">post from 格式</param>
        /// <returns></returns>
        public IDictionary<string, string> Parse(string param)
        {
            if (!string.IsNullOrEmpty(param))
            {
                Dictionary<string, string> data = new Dictionary<string, string>();
                foreach (var item in param.Split('&'))
                {
                    var strs = item.Split('=');
                    data.Add(strs[0], strs[1]);
                }
                return data;
            }
            return null;
        }
    }
}
