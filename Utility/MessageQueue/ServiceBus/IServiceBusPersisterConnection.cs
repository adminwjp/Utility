#if NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
namespace Utility.ServiceBus
{
    using Microsoft.Azure.ServiceBus;
    using System;

    /// <summary>
    /// ServiceBus 连接 接口
    /// </summary>
    public interface IServiceBusPersisterConnection : IDisposable
    {
        /// <summary>
        /// 连接接口
        /// </summary>
        ServiceBusConnectionStringBuilder ServiceBusConnectionStringBuilder { get; }

        /// <summary>
        /// 创建  ITopicClient
        /// </summary>
        /// <returns></returns>
        ITopicClient CreateModel();
    }
}
#endif