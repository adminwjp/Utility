#if !(NET10 || NET11 || NET20 || NET30 || NET35 || NET40 ||NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 ||NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 ||NETSTANDARD1_3 )
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using zipkin4net;
using zipkin4net.Tracers;
using zipkin4net.Tracers.Zipkin;
using zipkin4net.Transport.Http;

namespace Utility.Zipkin
{
    /// <summary>
    ///  http aspnet.core 跟踪
    /// </summary>
    public static class ZipkinHelper
    {

        /// <summary>
        /// 启动 http aspnet.core 跟踪
        /// </summary>
        /// <param name="url"></param>
        /// <param name="tracingLogger"></param>
        public static void Start(string url,zipkin4net.ILogger tracingLogger)
        {
            TraceManager.SamplingRate = 1.0f;//记录数据密度,1.0 代表所有记录
            var httpZipkinSender = new HttpZipkinSender(url, "application/json");
            var zipkinTracer = new ZipkinTracer(httpZipkinSender, new JSONSpanSerializer(), new Statistics());//注册zipkin
            var consoleTracer = new ConsoleTracer();//控制台输出
            TraceManager.RegisterTracer(zipkinTracer);//注册
            TraceManager.RegisterTracer(consoleTracer);//控制台输入日志
            TraceManager.Start(tracingLogger);//放到内存中的数据
        }


        /// <summary>
        /// 启动 http 跟踪
        /// </summary>
        /// <param name="url"></param>
        /// <param name="title"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        public static async Task<string> GetAsync(string url,string title,List<KeyValuePair<string,string>> header=null)
        {
            using (HttpClient httpClient = new HttpClient(new TracingHandler(title)))
            {
                if (header != null)
                {
                    foreach (KeyValuePair<string, string> item in header)
                    {
                        httpClient.DefaultRequestHeaders.Add(item.Key, item.Value);
                    }
                }
                return await httpClient.GetStringAsync(url);
            }
        }

        /// <summary>
        ///停止 http aspnet.core 跟踪
        /// </summary>
        public static void Stop()
        {
            TraceManager.Stop();
        }
    }
}
#endif