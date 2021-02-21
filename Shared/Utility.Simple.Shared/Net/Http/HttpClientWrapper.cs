#if !( NET20 || NET30 || NET35 || NET40 || NETSTANDARD1_0)
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Linq;
using Utility.Net;
using System.IO;
using Utility.Logs;

namespace Utility.Net.Http
{
    /// <summary>
    /// 
    /// </summary>
    public class HttpClientWrapper:IHttpCollect
    { 
        /// <summary>
        /// 内部类
        /// </summary>
        class InnerHttpClient
        {
            ///<summary>
            ///声明并初始化
            /// </summary>
            public static readonly HttpClientWrapper Instace = new HttpClientWrapper();
        }

        /// <summary>
        /// 获取默认实例对象 饿汉式 单例
        /// </summary>
        public static HttpClientWrapper Instance => InnerHttpClient.Instace;

        /// <summary>
        /// HttpClient通用一般请求方法 
        /// </summary>
        /// <param name="httpRequest">HttpRequestMessage对象</param>
        /// <param name="referer">referer地址</param>
        /// <returns>返回 http string </returns>
        public static string Result(HttpRequestMessage httpRequest, string referer = "")
        {
            using (HttpClient httpClient =new HttpClient())
            {
                if (!string.IsNullOrEmpty(referer))
                {
                    httpRequest.Headers.Referrer = new Uri(referer);
                }
                HttpResponseMessage httpContent = httpClient.SendAsync(httpRequest).Result;
                return httpContent.IsSuccessStatusCode ? httpContent.Content.ReadAsStringAsync().Result : string.Empty;
            }
        }

        /// <summary>
        /// 通用 请求 带cookie
        /// </summary>
        /// <param name="httpEntity"></param>
        /// <returns></returns>
        public virtual HttpResult Http(HttpEntity httpEntity)
        {
            return HttpRequest(httpEntity);
        }

        /// <summary>
        /// http get 请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="referer">请求来源</param>
        /// <returns></returns>
        public virtual string GetString(string url, string referer = "") => Get(url, referer);

        /// <summary>
        /// /http post form 请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="param">请求参数</param>
        /// <param name="referer">请求来源</param>
        /// <returns></returns>
        public virtual  string PostString(string url, Dictionary<string, string> param, string referer = "") => Post(url, param, referer);

        /// <summary>
        /// post json
        /// <para>不支持 net 2.0- net 3.5</para>
        /// </summary>
        /// <param name="url">url地址</param>
        /// <param name="json">post请求参数</param>
        /// <param name="referer">referer地址</param>
        /// <returns></returns>
        public  virtual string PostJson(string url, string json, string referer = "") => PostJsonToString(url, json, referer);

        /// <summary>
        /// /http put json 请求
        /// <para>不支持 net 2.0- net 3.5</para>
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="json">请求参数</param>
        /// <param name="referer">请求来源</param>
        /// <returns></returns>
        public virtual string PutJson(string url, string json, string referer = "") => PutJsonToString(url, json, referer);

        /// <summary>
        /// down
        /// <para>不支持 net 2.0- net 3.5</para>
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="referer">请求来源</param>
        /// <returns></returns>
        public virtual byte[] DownFile(string url, string referer = "") => Down(url, referer);

        /// <summary>
        /// http 请求
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="param">参数</param>
        /// <param name="referer">referer</param>
        /// <param name="method">方法</param>
        /// <param name="contentType">contentType</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public virtual string Http(string url, string param, string referer, HttpMethod method, string contentType, Encoding encoding = null) => HttpString(url, param, referer, method, contentType, encoding);

        /// <summary>
        /// post form 
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="param">参数</param>
        /// <param name="referer">referer</param>
        /// <returns></returns>
        public virtual string PostString(string url, string param, string referer = "")
        {
            var data = (Dictionary<string, string>)new RequestParamParse().Parse(param);
            return PostString(url, data, referer);
        }

#region 静态方法
        /// <summary>
        /// get 请求
        /// <para>不支持 net 2.0- net 3.5</para>
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="referer">请求来源</param>
        /// <returns></returns>
        public static string Get(string url, string referer = "")
        {
            return Result(new HttpRequestMessage()
            {
                RequestUri = new Uri(url),
                Method = System.Net.Http.HttpMethod.Get
            }, referer);
        }

        /// <summary>
        /// /http post form 请求
        /// <para>不支持 net 2.0- net 3.5</para>
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="param">请求参数</param>
        /// <param name="referer">请求来源</param>
        /// <returns></returns>
        public static string Post(string url, Dictionary<string, string> param, string referer = "") => Result(new HttpRequestMessage()
        {
            RequestUri = new Uri(url),
            Method = System.Net.Http.HttpMethod.Post,
            Content = param.Any() ? null : new FormUrlEncodedContent(param)
        }, referer);

        /// <summary>
        /// post json
        /// <para>不支持 net 2.0- net 3.5</para>
        /// </summary>
        /// <param name="url">url地址</param>
        /// <param name="json">post请求参数</param>
        /// <param name="referer">referer地址</param>
        /// <returns></returns>
        public static string PostJsonToString(string url, string json, string referer = "") => HttpString(url,json,referer, HttpMethod.POST,ContentTypeConstant.APPLICATION_JSON);

        /// <summary>
        /// http 
        /// </summary>
        /// <param name="url">url地址</param>
        /// <param name="param">post请求参数</param>
        /// <param name="referer">referer地址</param>
        /// <param name="method">方法</param>
        /// <param name="contentType"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string HttpString(string url, string param, string referer, HttpMethod method, string contentType, Encoding encoding=null)
        {
#if NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
            System.Net.Http.HttpMethod httpMethod = HttpMethodAdapter.Parse(method);
#else
            System.Net.Http.HttpMethod httpMethod = System.Net.Http.HttpMethod.Get;
#endif
            return Result(new HttpRequestMessage()
            {
                RequestUri = new Uri(url),
                Method = httpMethod,
                Content = string.IsNullOrEmpty(param) ? null : new StringContent(param, encoding ?? Encoding.UTF8, contentType)
            }, referer);
        }
        /// <summary>
        /// /http put json 请求
        /// <para>不支持 net 2.0- net 3.5</para>
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="json">请求参数</param>
        /// <param name="referer">请求来源</param>
        public static string PutJsonToString(string url, string json, string referer = "") => HttpString(url, json, referer, HttpMethod.PUT, ContentTypeConstant.APPLICATION_JSON);

        /// <summary>
        /// down
        /// <para>不支持 net 2.0- net 3.5</para>
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="referer">请求来源</param>
        /// <returns></returns>
        public static byte[] Down(string url, string referer = "")
        {
            using (HttpClient httpClient=new HttpClient())
            {
                HttpRequestMessage httpRequest = new HttpRequestMessage()
                {
                    RequestUri = new Uri(url),
                    Method = System.Net.Http.HttpMethod.Get
                };
                if (!string.IsNullOrEmpty(referer)) httpRequest.Headers.Referrer = new Uri(referer);
                return httpClient.SendAsync(httpRequest).Result.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult();
            }
        }

      private static ILog log= new DefaultLog<HttpClientWrapper>();

        /// <summary>
        /// 通用 请求 带cookie
        /// </summary>
        /// <param name="httpEntity"></param>
        /// <returns></returns>
        public static HttpResult HttpRequest(HttpEntity httpEntity)
        {
            using (HttpClient httpClient=new HttpClient())
            {
                HttpRequestMessage httpRequest = new HttpRequestMessage()
                {
                    RequestUri = new Uri(httpEntity.Url)
                };
#if NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
                httpRequest.Method = HttpMethodAdapter.Parse(httpEntity.Method);
#endif
                if (httpEntity.Method == HttpMethod.POST)
                {
                    if (!string.IsNullOrEmpty(httpEntity.Param)&&httpEntity.IsStringParam)
                    {
                        byte[] p = Encoding.UTF8.GetBytes(httpEntity.Param);
                        httpRequest.Content=new ByteArrayContent(p, 0, p.Length);
                    }else if (httpEntity.ParamBytes != null)
                    {
                        byte[] p = httpEntity.ParamBytes;
                       httpRequest.Content=new ByteArrayContent(p, 0, p.Length);
                    }
                }
                if (!string.IsNullOrEmpty(httpEntity.Referer)) httpRequest.Headers.Referrer = new Uri(httpEntity.Referer);
                try
                {
#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
                    IEnumerable<string> cookieStrs = CookieHelper.GetCookieString(httpEntity.Cookie);
                    if (cookieStrs.Any())
                    {
                        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Cookie", cookieStrs); //请求带cookie
                    }
#endif
                    HttpResponseMessage httpContent = httpClient.SendAsync(httpRequest).Result;
                    HttpResult httpResult = new HttpResult();
                    if (httpContent.IsSuccessStatusCode)
                    {
#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
                       if (httpContent.Headers != null)
                            if (httpContent.Headers.TryGetValues("Set-Cookie", out IEnumerable<string> cookies)) CookieHelper.UpdateCookie(cookies, httpResult.Cookie);
#endif
                       httpResult.Header = httpContent.Headers;
                        switch (httpEntity.Result)
                        {
                            case HttpRetunnResult.String:
                                Encoding encoding = null;
                                if (!httpEntity.IsCustomEncoding)
                                {
                                    var charsets = httpContent.Headers.GetValues("CharacterSet");
                                    if (charsets != null)
                                    {
                                        foreach (var charset in charsets)
                                        {
                                            encoding = !string.IsNullOrEmpty(charset) ?
                                  (charset.Contains("8859") || charset != "ISO-8859-1") ? Encoding.GetEncoding(EncdoingConstant.ISO88591) : Encoding.GetEncoding(charset)
                                  : httpEntity.ReciveEncoding;
                                            if (encoding != null) break;
                                        }
                                    }
                                    encoding = encoding ?? Encoding.UTF8;
                                }
                                else
                                {
                                    encoding = httpEntity.ReciveEncoding ?? Encoding.UTF8;
                                }
                                using (var sr = new StreamReader(httpContent.Content.ReadAsStreamAsync().Result, encoding))
                                {
                                    httpResult.StringResult = sr.ReadToEnd();
                                }
                                break;
                            case HttpRetunnResult.Byte:
                                httpResult.ByteResult = httpContent.Content.ReadAsByteArrayAsync().Result;
                                break;
                            case HttpRetunnResult.Stream:
                                httpResult.Stream = httpContent.Content.ReadAsStreamAsync().Result;
                                break;
                        }
                        return httpResult;
                    }
                    else return null;
                }
                catch (Exception e)
                {          
                    log.LogException(LogLevel.Error, "http request error!", e);
                    return new HttpResult() { Exception = e };//请求异常
                }
            }
        }

#endregion  静态方法
    }
}
#endif
