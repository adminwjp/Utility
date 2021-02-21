using System.IO;
using System.Net;

namespace Utility.Net.Http
{
    /// <summary>HTTP 请求返回结果</summary>
    public class HttpResult
    {
        private string location;

        /// <summary>
        /// 无参构造函数
        /// </summary>
        public HttpResult()
        {
            this.Clear();
        }
        /// <summary>
        /// 初始化数据
        /// </summary>
        public void Clear()
        {
            this.Cookie = null;
            this.StringResult = string.Empty;
            this.ByteResult = null;
            this.Location = string.Empty;
            this.CharacterSet = string.Empty;
            this.ContentType = string.Empty;
            this.Stream = null;
            this.Exception = null;
        }
        /// <summary> 请求Cookie </summary>
        public CookieContainer Cookie { get; set; }
        /// <summary> 返回结果 string 类型 </summary>
        public string StringResult { get; set; }
        /// <summary> 返回结果 byte 类型 </summary>
        public byte[] ByteResult { get; set; }
        /// <summary> 重定向地址</summary>
        public string Location { 
            get {
                if (string.IsNullOrEmpty(location))
                {

#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
                    if (Headers != null)
                    {
                        var vals= Headers.GetValues("Location");
                        return vals != null && vals.Length > 0 ? vals[0] : string.Empty;
                    }
#endif

#if !(NET20 || NET30 || NET35 || NET40 || NETSTANDARD1_0)
                    if (Header != null)
                    {
                        var vals = Header.GetValues("Location");
                        if (vals != null)
                        {
                            foreach (var item in vals)
                            {
                                return item;
                            }
                        }
                    }
#endif
                }
                return string.Empty;
            }
            set => location = value;
        }
        /// <summary>输出编码格式</summary>
        public string CharacterSet { get; set; }
        /// <summary>输出编码类型</summary>
        public string ContentType { get; set; }
        /// <summary> 输出流 </summary>
        public Stream Stream { get; set; }
#if !(NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
        /// <summary> HttpWebResponse 输出头 </summary>
        public WebHeaderCollection Headers { get; set; }
#endif
#if !(NET20 || NET30 || NET35 || NET40 || NETSTANDARD1_0)
        //#if !(NET10 || NET11 || NET20 || NET30 || NET35 || NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NETSTANDARD1_0)
        /// <summary> HttpClient 输出头 </summary>
        public System.Net.Http.Headers.HttpResponseHeaders Header { get; set; }
#endif
        /// <summary> 异常信息 </summary>
        public System.Exception Exception { get; set; }
    }
}
