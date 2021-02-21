#if NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using Autofac.Extensions.DependencyInjection;
using System.Net;
using System.Runtime.InteropServices;
using Winton.Extensions.Configuration.Consul;
using System.Threading;

namespace Utility.AspNetCore
{
    public class StartHelper
    {

       /// <summary>
        /// Triggered when the application host has fully started and is about to wait for
        /// a graceful shutdown.
        /// </summary>
        public static  CancellationToken ApplicationStarted
        {
            get; set;
        }

         /// <summary>
        ///  Triggered when the application host is performing a graceful shutdown. All requests
        /// should be complete at this point. Shutdown will block until this event completes.
        /// </summary>
        public static CancellationToken ApplicationStopped
        {
            get; set;
        }

        //     may still be in flight. Shutdown will block until this event completes.  
        /// <summary>
        ///   Triggered when the application host is performing a graceful shutdown.Requests
        ///  may still be in flight. Shutdown will block until this event completes.  
        /// </summary>
        public static CancellationToken ApplicationStopping
        {
            get;set;
        }

        /// <summary>
        /// 设置服务端口
        /// </summary>
        public static int WindowServicePort { get; set; }
        /// <summary>
        /// win service 启动
        /// </summary>
        public static bool IsWindowService { get; set; }

        /// <summary>
        /// 启动
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="title"></param>
        /// <param name="args"></param>
        public static void Start<T>(string title, string[] args) where T : class
        {
            Console.Title = title;
            var config =LogHelper.Initial();
            ////https://autofaccn.readthedocs.io/en/latest/integration/aspnetcore.html#asp-net-core-3-0-and-generic-hosting
#if NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0
            CreateHostBuilder<T>(config, args).Run();
#else
            CreateWebHostBuilder<T>(config, args).Run();
#endif
        }


        /// <summary>
        /// host 启动
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configuration"></param>
        /// <param name="args"></param>
        /// <param name="apb"></param>
        /// <returns></returns>

#if NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0
        public static IHost CreateHostBuilder<T>(IConfiguration configuration, string[] args,bool apb=false) where T : class
        {
          
            //WebHost 1.1-2.0+
            //Host3.0+
            // ASP.NET Core 3.0+:
            // The UseServiceProviderFactory call attaches the
            // Autofac provider to the generic hosting mechanism.
            var host = Host.CreateDefaultBuilder(args)
                // .ConfigureServices(it=>it.AddAutofac())
                ;
            if (IsWindowService&&RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                host = host.UseWindowsService();
            }
            host= host
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    Use<T>(configuration, webBuilder);
                });
               //注意 必须放在后面 
               //InvalidCastException: Unable to cast object of type 'Microsoft.Extensions.DependencyInjection.ServiceCollection' to type 'Autofac.ContainerBuilder'.
               //最新版本可以不需要
               if(!apb){
                 host=host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
               }
               return host.Build();
          }
#endif

        /// <summary>
        /// 启动 配置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configuration"></param>
        /// <param name="webBuilder"></param>
        public static IWebHostBuilder Use<T>(IConfiguration configuration, IWebHostBuilder webBuilder) where T : class
        {

            if (IsWindowService)
            {
                webBuilder.ConfigureKestrel(serverOptions =>
                {
                    serverOptions.Listen(IPAddress.Any, WindowServicePort);
                    serverOptions.Limits.MaxRequestBodySize = null;
                });
            }
          
            webBuilder.ConfigureAppConfiguration((hostingContext, config) =>

            {
                //asp.net core 5.0 怎么进不来 弄错了
                //https://www.cnblogs.com/Vincent-yuan/p/11186196.html 优先级 太慢 启动时需要
                //Files(appsettings.json, appsettings.{ Environment}.json, { Environment}
                //是应用当前的运行环境)
                //Azure Key Vault
                //User secrets(Secret Manager)(仅用在开发环境)
                //Environment variables
                //Command - line arguments
                //config.AddEFConfiguration( options => options.UseInMemoryDatabase("InMemoryDb"));

                var env = hostingContext.HostingEnvironment;
                //hostingContext.Configuration = config.Build();
                // string consul_url = hostingContext.Configuration["Consul_Url"];
                string consul_url = configuration["ConsulUrl"];
                string key = configuration["ConsulKey"];
                Console.WriteLine(consul_url);
                Console.WriteLine(key);
                Console.WriteLine(env.ApplicationName);
                Console.WriteLine(env.EnvironmentName);
                config.AddConsul(key,
                    options =>
                    {
                        options.Optional = true;
                        options.ReloadOnChange = true;
                        options.OnLoadException = exceptionContext => { exceptionContext.Ignore = true; };
                        options.ConsulConfigurationOptions = cco => { cco.Address = new Uri(consul_url); };

                    }
                );
                hostingContext.Configuration = config.Build();
                configuration = hostingContext.Configuration;//更新 否则 consul 失效
            });

            webBuilder.CaptureStartupErrors(false)
            .UseDefaultServiceProvider(options => { options.ValidateScopes = false; })
            //.UseApplicationInsights()
            .UseContentRoot(Directory.GetCurrentDirectory())
             .UseIISIntegration()
            .UseConfiguration(configuration)
            //.UseUrls(configuration["applicationUrl"].Split(new char[] { ';' }))
            .UseSerilog()
            .UseStartup<T>();
            return webBuilder;
        }

        /// <summary>
        /// web  host 启动
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configuration"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IWebHost CreateWebHostBuilder<T>(IConfiguration configuration, string[] args) where T : class =>
                
                Use<T>(configuration, WebHost.CreateDefaultBuilder(args))
                .Build();
    }

}
#endif
