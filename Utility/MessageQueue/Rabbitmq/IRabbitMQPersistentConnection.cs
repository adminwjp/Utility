#if NET45 ||  NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using RabbitMQ.Client;
using System;

namespace Utility.MessageQueue
{
    /// <summary>
    /// RabbitMQ 连接 接口
    /// </summary>
    public interface IRabbitMQPersistentConnection
        : IDisposable
    {

        /// <summary>
        /// RabbitMQ 是否 连接
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// 重试连接
        /// </summary>
        /// <returns></returns>
        bool TryConnect();

        /// <summary>
        /// 创建  IModel
        /// </summary>
        /// <returns></returns>
        IModel CreateModel();
    }
}
#endif