using System;

namespace Utility.Net.Http
{
    /// <summary>
    /// 协议检测
    /// </summary>
    public class ProtocolCheck
    {

        /// <summary>
        /// 地址 协议检测
        /// </summary>
        /// <param name="url">地址</param>
        public static void Check(string url)
        {
            if (!string.IsNullOrEmpty(url)&& !(url.StartsWith("http:") || url.StartsWith("https:"))) 
                throw new NotSupportedException(" request url error,support http or https protocol");
        }
    }
}
