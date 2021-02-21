#if  NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0  || NETCOREAPP3_1|| NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using Microsoft.Azure.ServiceBus;
using System;

namespace Utility.ServiceBus
{
    /// <summary>
    /// ServiceBus 连接
    /// </summary>
    public class DefaultServiceBusPersisterConnection : IServiceBusPersisterConnection
    {
        private readonly ServiceBusConnectionStringBuilder _serviceBusConnectionStringBuilder;
        private ITopicClient _topicClient;
        bool _disposed;

        /// <summary>
        /// ServiceBus 连接
        /// </summary>
        /// <param name="serviceBusConnectionStringBuilder"></param>
        public DefaultServiceBusPersisterConnection(ServiceBusConnectionStringBuilder serviceBusConnectionStringBuilder)
        {
            _serviceBusConnectionStringBuilder = serviceBusConnectionStringBuilder ??
                throw new ArgumentNullException(nameof(serviceBusConnectionStringBuilder));
            _topicClient = new TopicClient(_serviceBusConnectionStringBuilder, RetryPolicy.Default);
        }

        /// <summary>
        /// 连接接口
        /// </summary>
        public ServiceBusConnectionStringBuilder ServiceBusConnectionStringBuilder => _serviceBusConnectionStringBuilder;


        /// <summary>
        /// 创建  ITopicClient
        /// </summary>
        /// <returns></returns>
        public virtual ITopicClient CreateModel()
        {
            if (_topicClient.IsClosedOrClosing)
            {
                _topicClient = new TopicClient(_serviceBusConnectionStringBuilder, RetryPolicy.Default);
            }

            return _topicClient;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public virtual void Dispose()
        {
            if (_disposed) return;
            _topicClient.CloseAsync();
            _disposed = true;
        }
    }
}
#endif
