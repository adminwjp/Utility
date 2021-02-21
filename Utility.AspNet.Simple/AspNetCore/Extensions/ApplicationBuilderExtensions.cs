#if NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Steeltoe.Discovery.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility.AspNetCore.Consul;

namespace Utility.AspNetCore.Extensions
{
    /// <summary>
    /// ApplicationBuilder 扩展类
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
      

        /// <summary>
        /// 添加 注册服务 生命周期
        /// Steeltoe 支持 eureka(优先级高) consul 默认 只给 eureka 配置 要么 2 选 1
        /// consul 单独实现
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configuration"></param>
        /// <param name="flag"></param>
        public static void UseService(this IApplicationBuilder app, IConfiguration configuration, ServiceFlag flag)
        {
            switch (flag)
            {
                case ServiceFlag.None:
                    break;
                case ServiceFlag.Eureka:
                    app.UseDiscoveryClient();//注册微服务 Eureka or  consul 优先级 eureka 高 没有配置则不启动 
                    break;
                case ServiceFlag.Consul:
                    app.UseConsul(configuration);
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
        }


        /// <summary>
        /// 创建 ApplicationBuilder
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
#if NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0
        public static IApplicationBuilder Create(this IServiceCollection services)
        {
            IServiceProvider GetProviderFromFactory(IServiceCollection collection)
            {
                var provider = collection.BuildServiceProvider();
                var factory = provider.GetService<IServiceProviderFactory<IServiceCollection>>();

                if (factory != null && !(factory is DefaultServiceProviderFactory))
                {
                    using (provider)
                    {
                        return factory.CreateServiceProvider(factory.CreateBuilder(collection));
                    }
                }

                return provider;
            }
            IApplicationBuilder builder = new ApplicationBuilder(GetProviderFromFactory(services));
            return builder;
        }
#endif

        /// <summary>
        /// 使用 cors swagger  apiversion
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="name"></param>

#if NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0
        public static void Use(this IApplicationBuilder app, IWebHostEnvironment env,string name)
        {
            //方案1  IServiceCollection 不需要配置
            app.UseCors(options =>
            {
                options.AllowAnyHeader();
                options.AllowAnyMethod();
                options.AllowAnyOrigin();
                //options.AllowCredentials();
            });
            //方案2 IServiceCollection 需要配置
            //app.UseCors("AllowAllOrigins");

            // app.UseAuthorization();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            //要在应用的根 (http://localhost:<port>/) 处提供 Swagger UI，请将 RoutePrefix 属性设置为空字符串
            app.UseSwaggerUI(c =>
            {
                ////要统一 SwaggerVersion
                c.SwaggerEndpoint("/swagger/V1/swagger.json", name);
                c.RoutePrefix = string.Empty;
            });
            app.UseApiVersioning();
            app.UseHttpsRedirection();
            var cachePeriod = env.IsDevelopment() ? "600" : "604800";
            app.UseStaticFiles(new StaticFileOptions
            {
                //FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "MyStaticFiles")),
                //RequestPath = "/StaticFiles",
                OnPrepareResponse = ctx =>
                {
                    // Requires the following import:
                    // using Microsoft.AspNetCore.Http;
                    ctx.Context.Response.Headers.Append("Cache-Control", $"public, max-age={cachePeriod}");
                }
            });
            //app.UseCookiePolicy();
            // app.UseAuthentication();
            // app.UseAuthorization();   
            app.UseRouting();
            //aspnet core > = 3.0
            app.UseEndpoints(options =>
            {
                //iis 不支持 
                options.MapAreaControllerRoute(
                  name: "area",
                  areaName: "areas",
                  pattern: "{area:exists}/{controller}/{action}/{id?}"
                );
                options.MapControllerRoute(
                  name: "default",
                  pattern: "{controller}/{action}/{id?}"
                );
                options.MapControllers();
                options.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
#else
        public static void Use(this IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env, string name)
        {
            // app.UseAuthorization();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            //要在应用的根 (http://localhost:<port>/) 处提供 Swagger UI，请将 RoutePrefix 属性设置为空字符串
            app.UseSwaggerUI(c =>
            {
                ////要统一 SwaggerVersion
                c.SwaggerEndpoint("/swagger/V1/swagger.json", name);
                c.RoutePrefix = string.Empty;
            });
#if NETCOREAPP2_2 ||  NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0
            app.UseApiVersioning();
#endif
            var cachePeriod = env.IsDevelopment() ? "600" : "604800";
            app.UseStaticFiles(new StaticFileOptions
            {
                //FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "MyStaticFiles")),
                //RequestPath = "/StaticFiles",
                OnPrepareResponse = ctx =>
                {
                    // Requires the following import:
                    // using Microsoft.AspNetCore.Http;
                    ctx.Context.Response.Headers.Append("Cache-Control", $"public, max-age={cachePeriod}");
                }
            });
            app.UseRouter(options =>
            {
                //iis 不支持 
                options.MapAreaRoute(
                  name: "area",
                  areaName: "areas",
                  template: "{area:exists}/{controller}/{action}/{id?}"
                );
                options.MapRoute(
                  name: "default",
                  template: "{controller}/{action}/{id?}"
                );
            });
        }
#endif
    }
}
#endif