#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
namespace Utility.Net.Http
{
    using System;
    using System.Net;
#if !(NETSTANDARD1_0  || NETSTANDARD1_1 || NETSTANDARD1_2)
    using System.Security.Cryptography.X509Certificates;
    using System.Net.Security;
#endif
    using System.Text;
    using System.IO;
#if !NETSTANDARD1_0
    using System.IO.Compression;
#endif
    using System.Collections.Generic;
    using Utility.Net;
    using Utility.IO;
    using Utility.Logs;

    /// <summary>HttpWebRequest包装类 </summary>
    public sealed class DefaultHttpCollect : IHttpCollect
    {
        /// <summary>
        /// 内部类
        /// </summary>
        class InnerHttpCollect
        {
            /// <summary>
            /// 声明并初始化
            /// </summary>
            public readonly static DefaultHttpCollect Instace = new DefaultHttpCollect();
        }
        private static ILog log= new DefaultLog<DefaultHttpCollect>();
        /// <summary>
        /// HttpWebRequest包装类
        /// </summary>
        public DefaultHttpCollect()
        {
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback((object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors) =>
            {
                return true; //总是接受  
            });
            ServicePointManager.DefaultConnectionLimit = DefaultConnectionLimit;
            ServicePointManager.MaxServicePointIdleTime = MaxServicePointIdleTime;
            ServicePointManager.MaxServicePoints = MaxServicePoints;
#pragma warning disable CS0618 // 类型或成员已过时
            //#if !(NET20 || NET30 ||NET35 || NET40)
            //            //The requested security protocol is not supported.
            //            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            //#else
            //            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls; //需要安装 System.Net.Http
            //#endif

#if !NET20 && !NET30 && !NET35 && !NET40 && !NETCOREAPP1_0 && !NETCOREAPP1_1 && !NETCOREAPP1_2
#if !NETCOREAPP3_1 //其他版本没测试
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
#endif
#else
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;
#endif

#pragma warning restore CS0618 // 类型或成员已过时

        }
        /// <summary>
        /// 默认连接数
        /// </summary>
        public  int DefaultConnectionLimit { get; set; } = 2000;
        /// <summary>
        /// 最大空闲时间
        /// </summary>
        public  int MaxServicePointIdleTime { get; set; } = 50;
        /// <summary>
        /// 最大服务点
        /// </summary>
        public  int MaxServicePoints { get; set; } = 100;

        /// <summary>
        /// 获取默认实例对象 饿汉式 单例
        /// </summary>
        public static DefaultHttpCollect Instace => InnerHttpCollect.Instace;

        /// <summary> 
        /// http  请求 HttpWebRequest带Cookie
        /// <para>
        ///request 不支持 netcoreapp1.0 -1.2   netstandard1.0-1.6
        /// </para>
        /// <para>
        /// response cookie 不支持 netstandard1.0-1.4
        /// </para>
        /// </summary>
        /// <param name="httpEntity">请求参数</param>
        /// <returns>请求结果</returns>
        public HttpResult Http(HttpEntity httpEntity)
        {
            return HttpCollect(httpEntity);
        }


        /// <summary>
        /// 通用请求方法
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="referer">请求来源</param>
        /// <param name="param">请求参数</param>
        /// <param name="method">请求方法</param>
        /// <param name="contentType">请求类型</param>
        /// <param name="encoding">编码</param>
        /// <exception cref="Exception"></exception>
        /// <returns></returns>
        public  string Http(string url, string param, string referer, HttpMethod method, string contentType, Encoding encoding) => HttpString(url, param, referer, method, contentType, encoding);

        /// <summary>
        /// http get 请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="referer">请求来源</param>
        /// <exception cref="Exception"></exception>
        /// <returns></returns>
        public   string GetString(string url, string referer = "") => Get(url, referer);

        /// <summary>
        ///http post form 请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="param">请求参数</param>
        /// <param name="referer">请求来源</param>
        /// <returns></returns>
        public   string PostString(string url, string param, string referer = "") => Post(url, param, referer);

        /// <summary>
        /// /http post form 请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="param">请求参数</param>
        /// <param name="referer">请求来源</param>
        /// <exception cref="Exception"></exception>
        /// <returns></returns>
        public  string PostString(string url, Dictionary<string, string> param, string referer = "") => Post(url, param, referer);

        /// <summary> post json</summary>
        /// <param name="url">url地址</param>
        /// <param name="json">post请求参数</param>
        /// <param name="referer">referer地址</param>
        /// <exception cref="Exception"></exception>
        /// <returns></returns>
        public  string PostJson(string url, string json, string referer = "") => PostJsonToString(url, json, referer);

        /// <summary>
        /// /http put json 请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="json">请求参数</param>
        /// <param name="referer">请求来源</param>
        /// <exception cref="Exception"></exception>
        /// <returns></returns>
        public  string PutJson(string url, string json, string referer = "") => PutJsonToString(url, json, referer);

        /// <summary>
        /// down
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="referer">请求来源</param>
        /// <exception cref="Exception"></exception>
        /// <returns></returns>
        public  byte[] DownFile(string url, string referer = "")
        {
            return Down(url, referer);
        }

        #region 静态方法
        /// <summary>
        /// 通用 请求 带cookie
        /// </summary>
        /// <param name="httpEntity"></param>
        /// <exception cref="Exception"></exception>
        /// <returns></returns>
        public static HttpResult HttpRequest(HttpEntity httpEntity) => HttpCollect(httpEntity);

        /// <summary>
        /// get 请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="referer">请求来源</param>
        /// <exception cref="Exception"></exception>
        /// <returns></returns>
        public static string Get(string url, string referer = "") => HttpString(url, referer, string.Empty, HttpMethod.GET, string.Empty, null);

        /// <summary>
        /// /http post form 请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="param">请求参数</param>
        /// <param name="referer">请求来源</param>
        /// <exception cref="Exception"></exception>
        /// <returns></returns>
        public static string Post(string url, string param, string referer = "") => HttpString(url, param, referer, HttpMethod.POST, ContentTypeConstant.APPLICATION_X_WWW_FORM_URLENCODED, null);

        /// <summary>
        /// /http post form 请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="param">请求参数</param>
        /// <param name="referer">请求来源</param>
        /// <exception cref="Exception"></exception>
        /// <returns></returns>
        public static string Post(string url, Dictionary<string, string> param, string referer = "") {
            var requestParamParse = new RequestParamParse();
            string json = requestParamParse.Parse(param);
            return Post(url, json, referer);
        }

        /// <summary>
        /// post json
        /// </summary>
        /// <param name="url">url地址</param>
        /// <param name="json">post请求参数</param>
        /// <param name="referer">referer地址</param>
        /// <exception cref="Exception"></exception>
        /// <returns></returns>
        public static string PostJsonToString(string url, string json, string referer = "") => HttpString(url, json, referer, HttpMethod.POST, ContentTypeConstant.APPLICATION_JSON, null);

        /// <summary>
        /// /http put json 请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="json">请求参数</param>
        /// <param name="referer">请求来源</param>
        /// <exception cref="Exception"></exception>
        public static string PutJsonToString(string url, string json, string referer = "") =>HttpString(url, json, referer, HttpMethod.PUT, ContentTypeConstant.APPLICATION_JSON, null);

        /// <summary>
        /// down
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="referer">请求来源</param>
        /// <exception cref="Exception"></exception>
        /// <returns></returns>
        public static byte[] Down(string url, string referer = "")
        {
            var httpEntity = new HttpEntity()
            {
                Url = url,
                Referer = referer,
                Method = HttpMethod.GET,
                IsCookie = false,
                Result = HttpRetunnResult.Byte
            };
            var httpResult = HttpCollect(httpEntity);
            if (httpResult.Exception != null) throw httpResult.Exception;
            return httpResult.ByteResult;
        }

        /// <summary>
        /// 通用请求方法
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="referer">请求来源</param>
        /// <param name="param">请求参数</param>
        /// <param name="method">请求方法</param>
        /// <param name="contentType">请求类型</param>
        /// <param name="encoding">编码</param>
        /// <exception cref="Exception"></exception>
        /// <returns></returns>
        public static string HttpString(string url, string param, string referer, HttpMethod method, string contentType, Encoding encoding)
        {
            HttpEntity httpEntity = new HttpEntity() { Url = url, Referer = referer, Method = method, Param = param, ContentType = contentType, IsCustomEncoding = encoding == null ? false : true, ReciveEncoding = encoding, IsCookie = false, Result = HttpRetunnResult.String };
            HttpResult result = HttpCollect(httpEntity);
            if (result.Exception != null) throw result.Exception;
            return result.StringResult;
        }

        /// <summary> 
        /// http  请求 HttpWebRequest带Cookie
        /// <para>
        ///request 不支持 netcoreapp1.0 -1.2   netstandard1.0-1.6
        /// </para>
        /// <para>
        /// response cookie 不支持 netstandard1.0-1.4
        /// </para>
        /// </summary>
        /// <param name="httpEntity">请求参数</param>
        /// <returns>请求结果</returns>
        private static HttpResult HttpCollect(HttpEntity httpEntity)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            HttpResult httpResult = new HttpResult();
            try
            {
                request = (HttpWebRequest)HttpWebRequest.Create(httpEntity.Url);
                if (!string.IsNullOrEmpty(httpEntity.Accept)) request.Accept = httpEntity.Accept;
                if (!string.IsNullOrEmpty(httpEntity.ContentType)) request.ContentType = httpEntity.ContentType;
                //证书认证
                if (!string.IsNullOrEmpty(httpEntity.Certificate))
                {
                    X509Store store = new X509Store("MY", httpEntity.StoreLocation);
                    store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                    X509Certificate2Collection x509s = store.Certificates as X509Certificate2Collection;
                    X509Certificate2 x509 = null;
                    if (x509s != null)
                    {
                        foreach (X509Certificate2 item in x509s)
                        {
                            if (item.IssuerName.Name == httpEntity.Certificate)
                            {
                                x509 = item;
                                break;
                            }
                        }
                    }
                    if (x509 != null) request.ClientCertificates.Add(x509);
                }
                if (!string.IsNullOrEmpty(httpEntity.IpProxy)) request.Proxy = new WebProxy(httpEntity.IpProxy);
                request.AllowAutoRedirect = httpEntity.AllowAutoRedirect;
                if (!string.IsNullOrEmpty(httpEntity.UserAgent)) request.UserAgent = httpEntity.UserAgent;
                if (httpEntity.Version != null)
                {
                    //Only HTTP/1.0 and HTTP/1.1 version requests are currently supported. (Parameter 'value')
                    request.ProtocolVersion = httpEntity.Version;//不支持 2.0 支持话 需要 安装 System.Net.Http 包 ,不知其他包是否影响
                    //request.ProtocolVersion = HttpVersion.Version11;
                }
                request.KeepAlive = httpEntity.KeepAlive;
                if (httpEntity.Headers != null && httpEntity.Headers.Count > 0)
                {
                    WebHeaderCollection head = new WebHeaderCollection();
                    foreach (string item in httpEntity.Headers.Keys)
                    {
                        head.Set(item, httpEntity.Headers[item]);
                    }
                    request.Headers = head;
                }
                request.Timeout = httpEntity.Timeout;
                if (!string.IsNullOrEmpty(httpEntity.Referer)) request.Referer = httpEntity.Referer;
                request.ReadWriteTimeout = httpEntity.ReadWriteTimeout;
#if !(NET20 || NET30 || NET35)
                if (!string.IsNullOrEmpty(httpEntity.Host)) request.Host = httpEntity.Host;
#endif
#if !(NET20 || NET30 || NET35 || NET40)
                request.ContinueTimeout = httpEntity.ContinueTimeout;
#endif
                request.CookieContainer = httpEntity.Cookie;
                request.Method = httpEntity.Method.ToString();
                if (httpEntity.Method == HttpMethod.POST)
                {
                    if (!string.IsNullOrEmpty(httpEntity.Param)&&httpEntity.IsStringParam)
                    {
                        byte[] p = Encoding.UTF8.GetBytes(httpEntity.Param);
                        request.GetRequestStream().Write(p, 0, p.Length);
                    }else if (httpEntity.ParamBytes != null)
                    {
                        byte[] p = httpEntity.ParamBytes;
                        request.GetRequestStream().Write(p, 0, p.Length);
                    }
                }
#if !(NET20 || NET30 || NET35 || NET40)
                request.ServerCertificateValidationCallback = (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => { return true; };
#endif
                response = request.GetResponse() as HttpWebResponse;
                return httpResult = Http(httpEntity, response);
            }
            catch (Exception e)
            {
                log.LogException(LogLevel.Error, "http request error!", e);
                httpResult.Exception = e;
                return httpResult;
            }
            finally
            {
                if (response != null)
                {

#if !(NET20 || NET30 || NET35 || NET40)
                    response.Dispose();
#else
                    response.Close();
#endif
                    response = null;
                }
                if (request != null)
                {
                    request.Abort();
                    request = null;
                }
                if (httpEntity.IsCookie) httpResult.Cookie = httpEntity.Cookie;
            }
        }

        /// <summary>
        /// http response 返回响应结果
        /// <para>
        /// cookie 不支持 netstandard1.0-1.6
        /// </para>
        /// </summary>
        /// <param name="httpEntity"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        private static HttpResult Http(HttpEntity httpEntity, HttpWebResponse response)
        {
            if (httpEntity.IsCookie) CookieHelper.AddOrUpdateCookie(httpEntity.Cookie, response.Cookies);
            HttpResult result = new HttpResult()
            {
                Stream = response.GetResponseStream(),
                CharacterSet = response.CharacterSet,
                ContentType = response.ContentType.ToLower(),
                Headers = response.Headers
            };
            try
            {
                switch (httpEntity.Result)
                {
                    case HttpRetunnResult.String:
                        {
                            Encoding encoding = null;
                            if (!httpEntity.IsCustomEncoding)
                            {
                                encoding = !string.IsNullOrEmpty(response.CharacterSet) ?
                               (response.CharacterSet.Contains("8859") || response.CharacterSet != "ISO-8859-1") ? Encoding.GetEncoding(EncdoingConstant.ISO88591) : Encoding.GetEncoding(response.CharacterSet)
                               : httpEntity.ReciveEncoding;
                            }
                            else
                            {
                                encoding = httpEntity.ReciveEncoding;
                            }
                            encoding = encoding ?? EncdoingConstant.GetEncoding();
                            if (response.ContentType.Contains("gzip"))
                            {
                                using (GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress))
                                {
                                    result.StringResult = new StreamReader(gzip, encoding).ReadToEnd();
                                }
                            }
                            else if (response.ContentType.Contains("deflate"))
                            {
                                using (DeflateStream deflate = new DeflateStream(response.GetResponseStream(), CompressionMode.Decompress))
                                {
                                    result.StringResult = new StreamReader(deflate, encoding).ReadToEnd();
                                }
                            }
                            else
                            {
                                using (Stream str = response.GetResponseStream())
                                {
                                    result.StringResult = new StreamReader(str, encoding).ReadToEnd();
                                }
                            }
                        }
                        break;
                    case HttpRetunnResult.Byte:
                        {
                            result.ByteResult = StreamHelper.GetBuffer(response.GetResponseStream());
                        }
                        break;
                    case HttpRetunnResult.Stream:
                        result.Stream = response.GetResponseStream();
                        break;
                }
            }
            catch (Exception e)
            {
                log.LogException(LogLevel.Error, "http response error!", e);
                result.Exception = e;
            }
            return result;
        }
        #endregion  静态方法
    }
}
#endif