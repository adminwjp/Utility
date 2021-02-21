#if NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Utility.AspNetCore.Filter;
using Swashbuckle.AspNetCore.SwaggerGen;
using Utility.AspNetCore.Data;
using Steeltoe.Discovery.Client;
#if NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using Microsoft.Extensions.Diagnostics.HealthChecks;
#endif
using Microsoft.Extensions.Configuration;

namespace Utility.AspNetCore.Extensions
{
    /// <summary>
    /// ServiceCollection 扩展类
    /// </summary>
    public static class ServiceCollectionExtensions
    {

        /// <summary>
        /// 添加 注册服务
        /// Steeltoe 支持 eureka(优先级高) consul 默认 只给 eureka 配置 要么 2 选 1
        /// consul 单独实现
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="flag"></param>
        public static void AddRegisterService(this IServiceCollection services,IConfiguration configuration,ServiceFlag flag)
        {
            switch (flag)
            {
                case ServiceFlag.None:
                    break;
                case ServiceFlag.Eureka:
                    services.AddDiscoveryClient(configuration);//注册微服务 Eureka or  consul 优先级 eureka 高 没有配置则不启动 

                    break;
                case ServiceFlag.Consul:
                    services.Configure<Utility.Consul.ConsulEntity>(configuration.GetSection("Consul"));
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
        /// 数据库健康检测
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connectionString"></param>
        /// <param name="name"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
#if NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0
        public static IHealthChecksBuilder UseHealthCheck(this IServiceCollection services, string  connectionString,string name,DbFlag flag) 
        {
            var hcBuilder = services.AddHealthChecks();
            //健康检查
            hcBuilder.AddCheck("self", () => HealthCheckResult.Healthy());
            switch (flag)
            {
                case DbFlag.MySql:
                    hcBuilder
                   .AddMySql(
                       connectionString,
                       name: $"{name}-mysql-check",
                       tags: new string[] { $"{name} mysql" });
                    break;
                case DbFlag.SqlServer:
                    hcBuilder
                   .AddSqlServer(
                       connectionString,
                       name: $"{name}-sqlserver-check",
                       tags: new string[] { $"{name} sqlserver" });
                    break;
                case DbFlag.Oracle:
                    hcBuilder
                      .AddOracle(
                          connectionString,
                       name: $"{name}-oracle-check",
                       tags: new string[] { $"{name} oracle" });
                    break;
                case DbFlag.Postgre:
                    hcBuilder
                    .AddNpgSql(
                        connectionString,
                       name: $"{name}-postgre-check",
                       tags: new string[] { $"{name} postgre" });
                    break;
                case DbFlag.Sqlite:
                    hcBuilder
                    .AddSqlite(
                         connectionString,
                       name: $"{name}-sqlite-check",
                       tags: new string[] { $"{name} sqlite" });
                    break;
                default:
                    break;
            }
            //hcBuilder
            //       .AddRedis(
            //           Configuration.GetConnectionString("RedisConnectionString"),
            //           name: $"{name}-redis-check",
            //           tags: new string[] { $"{name}-redis" });
            //hcBuilder
            //      .AddElasticsearch(
            //           Configuration.GetConnectionString("ElasticsearchConnectionString"),
            //          name: $"{name}-es-check",
            //          tags: new string[] { $"{name}-es" });

            // prevent from mapping "sub" claim to nameidentifier.
            //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

            //var identityUrl = Configuration.GetValue<string>("IdentityUrl");

            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            //}).AddJwtBearer(options =>
            //{
            //    options.Authority = identityUrl;
            //    options.RequireHttpsMetadata = false;
            //    options.Audience = "email";
            //});
			return hcBuilder;
        }
#endif
       
        /// <summary>
        /// 创建 asp.net core ioc
        /// </summary>
        /// <returns></returns>
        public static IServiceCollection Create()
        {
            return new ServiceCollection();
        }
    
        /// <summary>
        /// 使用时 必须注入HttpClient 使用
        /// </summary>
        /// <param name="services"></param>
        public static void AddHttpClient(this IServiceCollection services)
        {
#if !NETCOREAPP2_0
            HttpClientFactoryServiceCollectionExtensions.AddHttpClient(services);
#else
            Microsoft.Extensions.DependencyInjection.HttpServiceCollectionExtensions.AddHttpContextAccessor(services);
#endif
        }
        
        /// <summary>
        /// 注入 过滤器
        /// </summary>
        /// <param name="services"></param>
        /// <param name="filters"></param>
        /// <returns></returns>
        public static IMvcBuilder AddFilter(this IServiceCollection services,Type[] filters=null)
        {
#if NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0
            return services.AddMvc(options =>
             {
                // options.ModelBinderProviders.Insert(0, new CustomModelBinderProvider());
                //options.InputFormatters.Insert(0, new XDocumentInputFormatter());
                 options.Conventions.Add(new ApiControllerVersionConvention());
                 options.Filters.Add<HttpGlobalExceptionFilter>();
                 if(filters!=null)
                 {
                     foreach (var item in filters)
                     {
                         options.Filters.Add(item);
                     }
                 }
                 options.Filters.Add<APIResultFilter>();
             });
#else
            return null;
#endif
        }

        /// <summary>
        /// 设置 iis request 流 接受
        /// </summary>
        /// <param name="services"></param>
        public static void AddIISServerOptions(this IServiceCollection services)
        {
#if NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0
            //https://www.cnblogs.com/cmt/p/11347507.html
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
#else

#endif
        }
     
        /// <summary>
        /// 添加 api 模型 验证 自动取消 需要手动调用
        /// </summary>
        /// <param name="services"></param>
        public static void AddApiModelValidate(this IServiceCollection services)
        {
            //禁用默认行为
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
        }

        /// <summary>
        /// services.AddSingleton &lt; Swashbuckle.AspNetCore.SwaggerGen.ISchemaGenerator, Swashbuckle.AspNetCore.SwaggerGen.JsonSchemaGenerator &gt; ();
        ///<para> Register the Swagger generator, defining 1 or more Swagger documents</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        /// <param name="verion"></param>
        /// <param name="title"></param>
        public static void AddSwaggerV1<T>(this IServiceCollection services, string verion, string title) where T : IOperationFilter, IDocumentFilter
        {
            /**参数结果循环引用，会导致电脑司机 无效 一直加载中 解决方案 修改源码 逻辑有点问题 asp.net core 控制器参数 start*/
            //#if NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
#if NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
            services.AddSingleton<IActionDescriptorCollectionProvider, CustomActionDescriptorCollectionProvider>();
            services.AddSingleton<IApiDescriptionGroupCollectionProvider, CustomApiDescriptionGroupCollectionProvider>();
            foreach (var item in services)
            {
                if (item.ServiceType.FullName == "Microsoft.AspNetCore.Mvc.ApiExplorer.DefaultApiDescriptionProvider")
                {
                    services.Remove(item);
                    break;
                }

            }
            services.AddSingleton<IApiDescriptionProvider, CustomApiDescriptionProvider>();
#endif

            services.AddSwagger<T>(verion,title);
        }

        /// <summary>
        /// services.AddSingleton &lt; Swashbuckle.AspNetCore.SwaggerGen.ISchemaGenerator, Swashbuckle.AspNetCore.SwaggerGen.JsonSchemaGenerator &gt; ();
        ///<para> Register the Swagger generator, defining 1 or more Swagger documents</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        /// <param name="verion"></param>
        /// <param name="title"></param>
        public static void AddSwagger<T>(this IServiceCollection services, string verion, string title) where T : IOperationFilter, IDocumentFilter
        {
            
            /**参数结果循环引用，会导致电脑司机 无效 一直加载中 解决方案 修改源码 逻辑有点问题 asp.net core 控制器参数 end*/

            services.AddSwaggerGen(c =>
            {
                //使用过滤器单独对某些API接口实施认证
                c.OperationFilter<T>();
                //参数结果循环引用，会导致电脑司机 无效 一直加载中
                c.SwaggerDoc(verion, new OpenApiInfo { Title = title, Version = verion });
                // Set the comments path for the Swagger JSON and UI.
                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //c.IncludeXmlComments(xmlPath);
            });

        }

        /// <summary>
        /// 设置 api 版本
        /// </summary>
        /// <param name="services"></param>
        /// <param name="apiVersion"></param>
        public static void AddApiVersioning(this IServiceCollection services, ApiVersion apiVersion=null)
        {
            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;//return versions in a response header
                options.DefaultApiVersion = apiVersion??new ApiVersion(1, 0);//default version select 
                options.AssumeDefaultVersionWhenUnspecified = true;//if not specifying an api version,show the default version
            });
        }

        /// <summary>
        /// 设置 cors
        /// </summary>
        /// <param name="services"></param>
        public static void AddCors(this IServiceCollection services)
        {
#if NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                         builder =>
                         {
                             //builder.AllowAnyOrigin().AllowAnyMethod(); ;
                             builder.AllowAnyHeader();
                             builder.AllowAnyMethod();
                             builder.AllowAnyOrigin();
                             //builder.AllowCredentials();
                         });

            });
#else

#endif
        }
    }
}
#endif