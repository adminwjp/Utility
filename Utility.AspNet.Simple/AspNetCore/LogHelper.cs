#if  NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.File;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Winton.Extensions.Configuration.Consul;

namespace Utility.AspNetCore
{
    /// <summary>
    /// asp.net core 日志 帮助类
    /// </summary>
    public class LogHelper
    {
        /// <summary>
        /// 日志写入方式
        /// </summary>
        public static LogFlag Flag = LogFlag.File;
        /// <summary>
        /// 动态配置服务地址 硬代码编辑麻烦
        /// </summary>
       // public static string Address { get; set; }
        /// <summary>
        /// 初始化 日志配置
        /// </summary>
        /// <returns></returns>
        public static IConfiguration Initial()
        {
            IConfigurationBuilder configurationBuilder = Builder(true);
            return LogConfig(configurationBuilder.Build());
        }

        /// <summary>
        /// 默认 初始化 配置文件
        /// </summary>
        /// <param name="dynamic"></param>
        /// <returns></returns>
        public static IConfigurationBuilder Builder(bool dynamic=false)
        {
            string text = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (string.IsNullOrEmpty(text))
            {
                text = "Development";
            }

            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings." + text + ".json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            //加载动态配置 则设置端口无效
            //if (dynamic)
            //{
            //    configurationBuilder = DynamicConfig(configurationBuilder, ServiceConfig.Flag,StartHelper.Key,Address);
            //}
            return configurationBuilder;
        }
        /// <summary>
        /// 动态 配置
        /// </summary>
        /// <param name="configurationBuilder"></param>
        /// <param name="flag"></param>
        /// <param name="address"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static IConfigurationBuilder DynamicConfig(IConfigurationBuilder configurationBuilder,ServiceFlag flag,string key,string address)
        {
            switch (flag)
            {
                case ServiceFlag.None:
                    break;
                case ServiceFlag.Eureka:
                    break;
                case ServiceFlag.Consul:
                    configurationBuilder = configurationBuilder.AddConsul(key,
                                 options =>
                                 {
                                     options.Optional = true;
                                     options.ReloadOnChange = true;
                                     options.OnLoadException = exceptionContext => { exceptionContext.Ignore = true; };
                                     options.ConsulConfigurationOptions = cco => { cco.Address = new Uri(address); };

                                 }
                             );
                    break;
                case ServiceFlag.Zookeeper:
                    break;
                case ServiceFlag.ServiceFabric:
                    break;
                case ServiceFlag.Redis:
                    break;
                case ServiceFlag.Rabbitmq:
                    break;
                case ServiceFlag.Kubernetes:
                    break;
                default:
                    break;
            }
            return configurationBuilder;
        }

        /// <summary>
        /// 日志 配置
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IConfiguration LogConfig(IConfiguration configuration)
        {
        
            if(Flag== LogFlag.File)
            {
                Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
							 .MinimumLevel.Override("Default", LogEventLevel.Information)
							 .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                             .MinimumLevel.Override("System", LogEventLevel.Warning)
							 .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                             .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                             .Enrich.FromLogContext()
                             //file
                             .WriteTo.File(Path.Combine("logs", "log.txt"), LogEventLevel.Verbose,
                             "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}", null,
                             1073741824L, null, buffered: false, shared: false, null, RollingInterval.Hour,
                             rollOnFileSizeLimit: false, 31)
							 .WriteTo.Console(LogEventLevel.Verbose,
                             "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
                             null, null, null, AnsiConsoleTheme.Literate).ReadFrom.Configuration(configuration)

                             .CreateLogger();
            }
            else
            {
                Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
							.MinimumLevel.Override("Default", LogEventLevel.Information)
							.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
							.MinimumLevel.Override("System", LogEventLevel.Warning)
							.MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
							.MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
							.Enrich.FromLogContext()
							// es
							.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(configuration.GetConnectionString("ElasticsearchConnectionString"))) // for the docker-compose implementation
							{
							AutoRegisterTemplate = true,
							OverwriteTemplate = true,
							DetectElasticsearchVersion = true,
							AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
							NumberOfReplicas = 1,
							NumberOfShards = 2,
							//BufferBaseFilename = "./buffer",
							RegisterTemplateFailure = RegisterTemplateRecovery.FailSink,
							FailureCallback = e => Console.WriteLine("Unable to submit event " + e.MessageTemplate),
							EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog |
							EmitEventFailureHandling.WriteToFailureSink |
							EmitEventFailureHandling.RaiseCallback,
							#pragma warning disable CS0618 // 类型或成员已过时
							FailureSink = new FileSink("./fail-{Date}.txt", new JsonFormatter(), null, null)
							#pragma warning restore CS0618 // 类型或成员已过时
							})
							.WriteTo.Console(LogEventLevel.Verbose,
							"[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
							null, null, null, AnsiConsoleTheme.Literate).ReadFrom.Configuration(configuration)
							.CreateLogger();
            }
            return configuration;
        }
    }

    /// <summary>
    /// 日志写入方式
    /// </summary>
    [Flags]
    public enum LogFlag
    {
        /// <summary>
        /// 文件
        /// </summary>
        File,
        /// <summary>
        /// ElaticSearch
        /// </summary>
        Es
    }
}
#endif