#if NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using Utility.AspNet.Consul;
using Utility.AspNetCore.Filter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Utility.Consul;
using Utility.AspNetCore.Extensions;
using Utility.AspNetCore.Consul;
using Utility.AspNetCore.Zipkin;

namespace Utility.AspNetCore
{
    /// <summary>
    /// asp.netcore 3.0 +  autofac
    /// </summary>
    public class BaseHostStart: BaseStart
    {
        public BaseHostStart(IConfiguration configuration) : base(configuration)
        {
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual void ConfigureServices(IServiceCollection services)
        {
            base.CustomConfigureServices(services);
        }
        // ConfigureContainer is where you can register things directly
        // with Autofac. This runs after ConfigureServices so the things
        // here will override registrations made in ConfigureServices.
        // Don't build the container; that gets done for you by the factory.
        public virtual void ConfigureContainer(ContainerBuilder builder)
        {
            // Register your own things directly with Autofac here. Don't
            // call builder.Populate(), that happens in AutofacServiceProviderFactory
            // for you.
        }
    }
    /// <summary>
    /// asp.netcore 1.0 - 2.2 autofac
    /// </summary>
    public class BaseWebHostStart: BaseStart
    {
        public BaseWebHostStart(IConfiguration configuration):base(configuration)
        {
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual IServiceProvider ConfigureServices(IServiceCollection services)
        {
            base.CustomConfigureServices(services);
            var builder = GetContainerBuilder();
            builder.Populate(services);
            return new AutofacServiceProvider(builder.Build());
        }
        protected virtual ContainerBuilder GetContainerBuilder()
        {
            return null;
        }
    }
    public class BaseStart
    {
        public BaseStart(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        protected string SwaggerVersion { get; set; } = "V1";//要统一
        protected string SwaggerTitle { get; set; } = "SwaggerTitle";
        protected string Title { get; set; } = "AspNetCore";
        public IConfiguration Configuration { get; }
        protected Type[] FilterTypes { get; set; }

        
        protected virtual void CustomConfigureServices(IServiceCollection services)
        {
            services.AddRegisterService(Configuration, ServiceConfig.Flag);
            AspNetCore.Extensions.ServiceCollectionExtensions.AddApiVersioning(services);
            var mvcBuilder = AspNetCore.Extensions.ServiceCollectionExtensions.AddFilter(services, FilterTypes);
#if NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0
            AspNetCore.Extensions.MvcBuilderExtensions.AddJson(mvcBuilder).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddControllers().AddControllersAsServices();
#else
            AspNetCore.Extensions.MvcBuilderExtensions.AddJson(mvcBuilder).SetCompatibilityVersion(CompatibilityVersion.Latest);
            services.AddMvcCore();
#endif
            //    .ConfigureApiBehaviorOptions(options =>
            //{
            //    options.SuppressConsumesConstraintForFormFileParameters = true;
            //    options.SuppressInferBindingSourcesForParameters = true;
            //    options.SuppressModelStateInvalidFilter = true;
            //    options.SuppressMapClientErrors = true;
            //    options.ClientErrorMapping[404].Link =
            //        "https://httpstatuses.com/404";
            //});
            AspNetCore.Extensions.ServiceCollectionExtensions.AddSwagger<EmptySwaggerOperationFilter>(services, SwaggerVersion, SwaggerTitle);
            AspNetCore.Extensions.ServiceCollectionExtensions.AddApiModelValidate(services);
        }

       

#if NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
#pragma warning disable CS0618 // 类型或成员已过时
        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env, Microsoft.Extensions.Hosting.IApplicationLifetime lifetime, ILoggerFactory loggerFactory)
#pragma warning restore CS0618 // 类型或成员已过时
        {
#else
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
#pragma warning disable CS0618 // 类型或成员已过时
        public virtual void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env, Microsoft.Extensions.Hosting.IApplicationLifetime lifetime, ILoggerFactory loggerFactory)
#pragma warning restore CS0618 // 类型或成员已过时
        {
#endif
            if (env.IsDevelopment())
            {
               
            }
            app.UseStaticFiles();
            Use(app);
            StartHelper.ApplicationStarted = lifetime.ApplicationStarted;
            StartHelper.ApplicationStopped = lifetime.ApplicationStopped;
            StartHelper.ApplicationStopping = lifetime.ApplicationStopping;
            //生命 周期  eureka consul  Steeltoe 框架已管理
            app.UseService(Configuration, ServiceConfig.Flag);
            app.UseZipkin(loggerFactory,
                        lifetime, Configuration);
           
             
            AspNetCore.Extensions.ApplicationBuilderExtensions.Use(app,env,Title);
        }
        protected virtual void Use(IApplicationBuilder app)
        {

        }
    }
}
#endif