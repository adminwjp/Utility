#if NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utility.AspNet.Zipkin;
using zipkin4net.Middleware;

namespace Utility.AspNetCore.Zipkin
{
    /// <summary>
    /// http aspnet.core 跟踪
    /// </summary>
    public static class ZipkinExtensions
    {
         public static zipkin4net.ILogger TracingLogger{get;set;}
#pragma warning disable CS0618 // 类型或成员已过时
        /// <summary>
        /// 启动 http aspnet.core 跟踪
        /// </summary>
        /// <param name="applicationBuilder"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="lifetime"></param>
        /// <param name="url"></param>
        /// <param name="name"></param>
        public static void UseZipkin(this IApplicationBuilder applicationBuilder,ILoggerFactory loggerFactory, 
            Microsoft.Extensions.Hosting.IApplicationLifetime lifetime,string url,string name)
#pragma warning restore CS0618 // 类型或成员已过时
        {
            StartHelper.ApplicationStarted.Register(() =>
            {
                TracingLogger = TracingLogger ?? new TracingLogger(loggerFactory, "zipkin4net");//内存数据
                ZipkinHelper.Start(url, TracingLogger);
                applicationBuilder.UseTracing(name);
            });
            //启动成功 才能停止
            StartHelper.ApplicationStopped.Register(()=> {
                ZipkinHelper.Stop();
            });
        }

#pragma warning disable CS0618 // 类型或成员已过时
        /// <summary>
        /// 启动 http aspnet.core 跟踪
        /// </summary>
        /// <param name="applicationBuilder"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="lifetime"></param>
        /// <param name="configuration"></param>
        public static void UseZipkin(this IApplicationBuilder applicationBuilder, ILoggerFactory loggerFactory,
            Microsoft.Extensions.Hosting.IApplicationLifetime lifetime,IConfiguration configuration)
#pragma warning restore CS0618 // 类型或成员已过时
        {
            if (bool.TryParse(configuration["EnableZipkin"], out bool enableZipkin)&& !enableZipkin)
            {
                return;
            }
            UseZipkin(applicationBuilder, loggerFactory,
                         lifetime, configuration["Zipkin:Address"], configuration["Zipkin:Name"]);
        }
     

        /// <summary>
        /// 启动 http 跟踪
        /// </summary>
        /// <param name="url"></param>
        /// <param name="title"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        public static  Task<string> GetAsync(string url,string title,List<KeyValuePair<string,string>> header=null)
        {
            return ZipkinHelper.GetAsync(url, title, header);
        }
    }
}
#endif