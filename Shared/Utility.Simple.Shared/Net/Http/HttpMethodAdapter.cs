namespace Utility.Net.Http
{
    /// <summary>
    /// http 请求 方法 转换
    /// </summary>
    public class HttpMethodAdapter
    {
        /// <summary>
        /// http 请求 方法 转换
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
#if NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_1
        public static HttpMethod Parse(string method) => method switch
        {
            "GET" => HttpMethod.GET,
            "PUT" => HttpMethod.PUT,
            "POST" => HttpMethod.POST,
            "DELETE" => HttpMethod.DELETE,
            "HEAD" => HttpMethod.HEAD,
            "OPTIONS" => HttpMethod.OPTIONS,
            "TRACE" => HttpMethod.TRACE,
            _ => HttpMethod.GET
        };
        public static System.Net.Http.HttpMethod Parse(HttpMethod method) => method switch
        {
            HttpMethod.GET=> System.Net.Http.HttpMethod.Get,
            HttpMethod.PUT=> System.Net.Http.HttpMethod.Put,
            HttpMethod.POST=> System.Net.Http.HttpMethod.Post,
            HttpMethod.DELETE => System.Net.Http.HttpMethod.Delete,
            HttpMethod.HEAD => System.Net.Http.HttpMethod.Head,
            HttpMethod.OPTIONS => System.Net.Http.HttpMethod.Options,
            HttpMethod.TRACE => System.Net.Http.HttpMethod.Trace,
            _ =>  System.Net.Http.HttpMethod.Get
        };
#else
        public static HttpMethod Parse(string method)
        {
            switch (method)
            {
                case "GET": return HttpMethod.GET;
                case "PUT": return HttpMethod.PUT;
                case "POST": return HttpMethod.POST;
                case "DELETE": return HttpMethod.DELETE;
                case "HEAD": return HttpMethod.HEAD;
                case "OPTIONS": return HttpMethod.OPTIONS;
                case "TRACE": return HttpMethod.TRACE;
                default: return HttpMethod.GET;
            }
        }
#if NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
        /// <summary>
        /// http 请求 方法 转换
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public static System.Net.Http.HttpMethod Parse(HttpMethod method)
        {
            switch (method)
            {
                case HttpMethod.GET: return System.Net.Http.HttpMethod.Get;
                case HttpMethod.PUT: return System.Net.Http.HttpMethod.Put;
                case HttpMethod.POST: return System.Net.Http.HttpMethod.Post;
                case HttpMethod.DELETE: return System.Net.Http.HttpMethod.Delete;
                case HttpMethod.HEAD: return System.Net.Http.HttpMethod.Head;
                case HttpMethod.OPTIONS: return System.Net.Http.HttpMethod.Options;
                case HttpMethod.TRACE: return System.Net.Http.HttpMethod.Trace;
                default: return System.Net.Http.HttpMethod.Get;
            }
        }
#endif
#endif
    }
}

