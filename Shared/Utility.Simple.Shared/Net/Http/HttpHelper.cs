#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
using System.IO;
using Utility.Validate;
#if !(NET20 || NET30 || NET35 )
using System.Threading.Tasks;
#endif
using System.Text;

#if !(NET20 || NET30 || NET35 || NET40 || NET45 || NET451 || NET452 || NET46 || NET472 || NET48 || NET481 || NET482 || NETSTANDARD1_0)
using System.Net.Http;
#endif

namespace Utility.Net.Http
{
    /// <summary>
    /// http 公共类
    /// </summary>
    public static class HttpHelper
    {

#if !(NET20 || NET30 || NET35 || NET40 || NET45 || NET451 || NET452 || NET46 || NET472 || NET48 || NET481 || NET482 || NETSTANDARD1_0)
        /// <summary>
        /// http get 请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <returns></returns>
        public static Task<string> GetAsync(string url)
        {
            ValidateHelper.ValidateArgumentNull("url", url);
            using (HttpClient httpClient = new HttpClient())
            {
                return httpClient.GetStringAsync(url);
            }
        }
        /// <summary>
        /// 对象HttpClient
        /// </summary>
        public static readonly HttpClient HttpClient = new HttpClient();
#endif
        /// <summary>
        /// http get 请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <returns></returns>
        public static string Get(string url)
        {
            ValidateHelper.ValidateArgumentNull("url", url);
#if !(NET20 || NET30 || NET35 || NET40 || NET45 || NET451 || NET452 || NET46 || NET472 || NET48 || NET481 || NET482 || NETSTANDARD1_0)
            return HttpClientWrapper.Get(url);
#else
            return DefaultHttpCollect.Get(url);
#endif
        }
        /// <summary>
        /// http get 请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <returns></returns>
        public static string GetLocation(string url)
        {
            ValidateHelper.ValidateArgumentNull("url", url);
#if !(NET20 || NET30 || NET35 || NET40 || NET45 || NET451 || NET452 || NET46 || NET472 || NET48 || NET481 || NET482 || NETSTANDARD1_0)
           return HttpClientWrapper.HttpRequest(new HttpEntity() { Url = url, Method = HttpMethod.GET }).Location;
#else
           return DefaultHttpCollect.HttpRequest(new HttpEntity() { Url=url,Method=HttpMethod.GET}).Location;
#endif
        }
#if !(NET20 || NET30 || NET35 || NET40 || NET45 || NET451 || NET452 || NET46 || NET472 || NET48 || NET481 || NET482 || NETSTANDARD1_0)
        /// <summary>
        /// http get 请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="param">请求参数</param>
        /// <returns></returns>
        public static Task<string> PostJsonAsync(string url, string param)
        {
            ValidateHelper.ValidateArgumentNull("url", url);
            param = param ?? string.Empty;
            using (HttpClient httpClient = new HttpClient())
            {
                using (HttpContent httpContent = new StringContent(param, Encoding.UTF8))
                {
                    return httpClient.PostAsync(url, httpContent).Result.Content.ReadAsStringAsync();
                }
            }
        }
#endif
        /// <summary>
        /// http get 请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="json">请求参数</param>
        /// <returns></returns>
        public static string PostJson(string url, string json)
        {
            ValidateHelper.ValidateArgumentNull("url", url);
            json = json ?? string.Empty;
#if !(NET20 || NET30 || NET35 || NET40 || NET45 || NET451 || NET452 || NET46 || NET472 || NET48 || NET481 || NET482 || NETSTANDARD1_0)
          return HttpClientWrapper.PostJsonToString(url,json);
#else
            return DefaultHttpCollect.PostJsonToString(url,json);
#endif
        }

        /// <summary>
        /// post from 表单 操作
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string PostForm(string url, string param,string encoding="utf-8")
        {
            ValidateHelper.ValidateArgumentNull("url", url);
            param = param ?? string.Empty;
#if !(NET20 || NET30 || NET35 || NET40 || NET45 || NET451 || NET452 || NET46 || NET472 || NET48 || NET481 || NET482 || NETSTANDARD1_0)
            using (HttpClient httpClient = new HttpClient())
            {
                using (HttpContent httpContent = new StringContent(param, Encoding.UTF8, ContentTypeConstant.APPLICATION_X_WWW_FORM_URLENCODED))
                {
                    var res = httpClient.PostAsync(url, httpContent).Result;
                    using (var sr = new StreamReader(res.Content.ReadAsStreamAsync().Result, Encoding.GetEncoding(encoding)))
                    {
                        return sr.ReadToEnd();
                    }
                   // return httpClient.PostAsync(url, httpContent).Result.Content.ReadAsStringAsync().Result;
                }
                
            }
#else
            return DefaultHttpCollect.Post(url, param);
#endif
        }
#if ! (NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2)
        /// <summary>
        /// http 上传文件
        /// </summary>
        /// <param name="url"></param>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        public static string Upload(string url, FileInfo fileInfo)
        {
            ValidateHelper.ValidateArgumentNull("url", url);
            ValidateHelper.ValidateArgumentObjectNull("fileInfo", fileInfo);
            return string.Empty;
        }
        /// <summary>
        /// http 上传文件 未 实现  上传 文件 对方实现接口 不同 导致 操作不同
        /// </summary>
        /// <param name="url"></param>
        /// <param name="stream">文件信息</param>
        /// <returns></returns>
        public static string Upload(string url, Stream stream)
        {
            ValidateHelper.ValidateArgumentNull("url", url);
            ValidateHelper.ValidateArgumentObjectNull("stream", stream);
            return string.Empty;
        }
#endif
        /// <summary>
        /// 下载 
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <returns></returns>
        public static string WebClientDownload(string url)
        {
            ValidateHelper.ValidateArgumentNull("url", url);
#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0  || NETSTANDARD1_1 || NETSTANDARD1_2 || !NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
            using (System.Net.WebClient webClient = new System.Net.WebClient())
            {
                return webClient.DownloadString(url);
            }
#else
            return string.Empty;
#endif
        }
    }
}
#endif