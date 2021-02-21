using System;
using System.Collections.Generic;
using System.Text;
using Utility.Validate;

namespace Utility.Net.Http
{
    /// <summary>   http entity </summary>
    public class HttpEntity
    {
        /// <summary>
        /// http 访问 版本  直接 新 版本 需要 安装 库 system.net.htttp 
        /// </summary>
        public static Version HttpVersion=
#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
#if NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_1
            System.Net.HttpVersion.Version20;//不支持 2.0 支持话 需要 安装 System.Net.Http 包 ,不知其他包是否影响
#else
             System.Net.HttpVersion.Version11;
#endif
#else
             new Version(1,0);
#endif


        /// <summary>
        /// no param constractor 
        /// </summary>
        public HttpEntity()
        {
            this.Clear();
        }
        /// <summary>
        /// initial property value
        /// </summary>
        public void Clear()
        {
            this._url = string.Empty;
            this._referer = string.Empty;
            this.Certificate = string.Empty;
#if  !(NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 )
            this.StoreLocation = System.Security.Cryptography.X509Certificates.StoreLocation.CurrentUser;
#endif
            this.AllowAutoRedirect = true;
            this.ContentType = ContentTypeConstant.APPLICATION_X_WWW_FORM_URLENCODED;
            this.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3325.181 Safari/537.36";
            this.Accept = "*/*";
            this.ContinueTimeout = 5000;
            this.Cookie = this.Cookie ?? new System.Net.CookieContainer();
            this.Timeout = 5000;
            this.Expect100Continue = false;
            this.IpProxy = string.Empty;
            this.ReadWriteTimeout = 5000;
            this.Method = HttpMethod.GET;
            this.KeepAlive = false;
            this.Host = string.Empty;
            this.Headers = this.Headers ?? new Dictionary<string, string>();
            this.Param = string.Empty;
            this.Result = HttpRetunnResult.String;
            this.ReciveEncoding = EncdoingConstant.GetEncoding();
            this.SendEncoding = this.ReciveEncoding;
#if !(NET20  || NET30 || NET35)
            this.CancelltionToken = this.CancelltionToken ?? new System.Threading.CancellationTokenSource();
#endif
            this.IsCustomEncoding = false;
            this.Version = HttpEntity.HttpVersion;
        }

        private string _url;//request url
        private string _referer;//request referer
        ///<summary>request url</summary>
        public string Url
        {
            get
            {
                ValidateHelper.ValidateArgumentNull("url", this._url);
                return this._url;
            }
            set
            {

                if (!string.IsNullOrEmpty(value))
                {
                    ProtocolCheck.Check(value);
                }
                this._url = value;
            }
        }
        ///<summary>request referer</summary>
        public string Referer
        {
            get { return this._referer; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    ProtocolCheck.Check(value);
                }
                this._referer = value;
            }
        }
        
        ///<summary>certificate address</summary>
        public string Certificate { get; set; }
#if !(NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 )
        ///<summary>store address of certificate store</summary>
        public System.Security.Cryptography.X509Certificates.StoreLocation StoreLocation { get; set; }
#endif
        ///<summary> wether automatic redirect is allowed . allowed by default</summary>
        public bool AllowAutoRedirect { get; set; }
        ///<summary>receiving type</summary>
        public string ContentType { get; set; }
        ///<summary>user agent</summary>
        public string UserAgent { get; set; }
        /// <summary>
        /// accept protocol
        /// </summary>
        public string Accept { get; set; }
        ///<summary>用户请求继续超时时间,默认5秒</summary>
        public int ContinueTimeout { get; set; }
        ///<summary>用户请求cookie,默认实例化</summary>
        public System.Net.CookieContainer Cookie { get; set; }
        ///<summary>用户请求超时时间,默认5秒</summary>
        public int Timeout { get; set; }
        ///<summary>用户请求是否握手,默认false</summary>
        public bool Expect100Continue { get; set; }
        ///<summary>用户请求ip信息</summary>
        public string IpProxy { get; set; }
        ///<summary>用户请求读写超时时间,默认5秒</summary>
        public int ReadWriteTimeout { get; set; }
        ///<summary>用户请求方式,，默认get</summary>
        public HttpMethod Method { get; set; }
        ///<summary>用户请求是否长连接,默认false</summary>
        public bool KeepAlive { get; set; }
        ///<summary>用户请求Host</summary>
        public string Host { get; set; }
        ///<summary>用户请求Headers,默认null</summary>
        public Dictionary<string, string> Headers { get; set; }
        ///<summary>用户请求参数</summary>
        public string Param { get; set; }
        ///<summary>默认用户请求参数 为 string 类型 </summary>
        public bool IsStringParam { get; set; } = true;
        ///<summary>用户请求参数 文件流 </summary>
        public byte[] ParamBytes { get; set; }
        /// <summary>
        /// 返回类型
        /// </summary>
        public HttpRetunnResult Result { get; set; }
        ///<summary>用户请求结果返回编码类型,默认utf-8</summary>
        public Encoding ReciveEncoding { get; set; }
        ///<summary>用户请求结果返回编码类型,默认utf-8</summary>
        public Encoding SendEncoding { get; set; }
#if !(NET20  || NET30 || NET35)
        ///<sumary>任务并行控制</sumary>
        public System.Threading.CancellationTokenSource CancelltionToken { get; set; }
#endif
        /// <summary>
        /// allow custom response encoding ?
        /// </summary>
        public bool IsCustomEncoding { get; set; }
        /// <summary>
        /// version
        /// </summary>
        public Version Version { get; set; }
        /// <summary>
        /// wether need cookie . default true
        /// </summary>
        public bool IsCookie { get; set; } = true;
    }
}
