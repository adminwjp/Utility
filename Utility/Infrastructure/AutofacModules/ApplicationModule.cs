#if NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1  || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using Autofac;
using System.Reflection;
using Utility.Application.IntegrationEvents;
using Utility.EventBus.Abstractions;

namespace Utility.Infrastructure.AutofacModules
{
    /// <summary>
    /// autofac 集成事件 模块
    /// </summary>

    public class ApplicationModule: Autofac.Module
    {
        /// <summary>
        /// 默认 注入  集成事件 处理模块
        /// 注入  集成事件 模块
        /// </summary>
        /// <param name="builder"></param>
        protected virtual void DefaultLoad(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IIntegrationEventHandler<>).GetTypeInfo().Assembly)
           .AsClosedTypesOf(typeof(IIntegrationEventHandler<>));
            builder.RegisterAssemblyTypes(typeof(IntegrationEventService<,,>).GetTypeInfo().Assembly)
         .AsClosedTypesOf(typeof(IIntegrationEventService));
        }

    }
}
#endif