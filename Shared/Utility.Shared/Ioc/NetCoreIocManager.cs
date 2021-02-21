#if NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Utility.Ioc
{
    public class NetCoreIocManager : IIocManager
    {
    //注意：nuget 包里找不到,但使用本地包可以
#if NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0
        //public static IApplicationBuilder Create(IServiceCollection services)
        //{
        //    IServiceProvider GetProviderFromFactory(IServiceCollection collection)
        //    {
        //        var provider = collection.BuildServiceProvider();
        //        var factory = provider.GetService<IServiceProviderFactory<IServiceCollection>>();

        //        if (factory != null && !(factory is DefaultServiceProviderFactory))
        //        {
        //            using (provider)
        //            {
        //                return factory.CreateServiceProvider(factory.CreateBuilder(collection));
        //            }
        //        }
        //        return provider;
        //    }
        //    IApplicationBuilder builder = new ApplicationBuilder(GetProviderFromFactory(services));
        //    return builder;
        //}
#endif

        public NetCoreIocManager()
        {
            Services=new ServiceCollection();
        } 
        public NetCoreIocManager(IApplicationBuilder builder):this()
        {
            Builder=builder;
        }
         public NetCoreIocManager(IServiceCollection services)
        {
            Services=services;
        }
          public NetCoreIocManager(IServiceCollection services,IApplicationBuilder builder):this(builder)
        {
            Builder=builder;
        }
        public IServiceCollection Services {get;private set;}
        public IApplicationBuilder Builder{ get; private set;}
        public virtual void AddScoped<T, ImplT>() where ImplT : class
        {
            Services.AddScoped(typeof(T),typeof(ImplT));
        }

        public virtual void AddTransient<T, ImplT>() where ImplT : class
        {
             Services.AddTransient(typeof(T),typeof(ImplT));
        }

        public virtual T Get<T>()
        {
            return Builder==null?Services.BuildServiceProvider().GetService<T>(): Builder.ApplicationServices.GetRequiredService<T>();
        }

        public virtual void SingleInstance<T, ImplT>() where ImplT : class
        {
             Services.AddSingleton(typeof(T),typeof(ImplT));
        }
    }
}
#endif