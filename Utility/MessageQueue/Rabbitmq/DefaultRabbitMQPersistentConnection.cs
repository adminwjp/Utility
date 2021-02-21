#if NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.IO;
using System.Net.Sockets;
using Utility.Logs;

namespace Utility.MessageQueue
{
    /// <summary>
    /// RabbitMQ 连接
    /// </summary>
    public class DefaultRabbitMQPersistentConnection
       : IRabbitMQPersistentConnection
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly ILog<DefaultRabbitMQPersistentConnection> _logger;
        private readonly int _retryCount;
        IConnection _connection;
        bool _disposed;
        readonly object sync_root = new object();

        /// <summary>
        /// RabbitMQ 连接
        /// </summary>
        /// <param name="connectionFactory"></param>
        /// <param name="logger"></param>
        /// <param name="retryCount"></param>
        public DefaultRabbitMQPersistentConnection(IConnectionFactory connectionFactory, ILog<DefaultRabbitMQPersistentConnection> logger, int retryCount = 5)
        {
            this._connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            this._logger = logger;
            this._retryCount = retryCount;
        }

        /// <summary>
        /// RabbitMQ 是否 连接
        /// </summary>
        public virtual bool IsConnected
        {
            get
            {
                return this._connection != null && this._connection.IsOpen && !this._disposed;
            }
        }

        /// <summary>
        /// 创建  IModel
        /// </summary>
        /// <returns></returns>
        public virtual IModel CreateModel()
        {
            if (!this.IsConnected)
            {
                throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");
            }
            return this._connection.CreateModel();
        }

        /// <summary>
        /// 关闭 连接
        /// </summary>
        public virtual void Dispose()
        {
            if (this._disposed) return;
            this._disposed = true;
            try
            {
                this._connection.Dispose();
            }
            catch (IOException ex)
            {
                this._logger.Log(LogLevel.Warn,ex.ToString());
            }
        }

        /// <summary>
        /// 重试连接
        /// </summary>
        /// <returns></returns>
        public virtual bool TryConnect()
        {
            this._logger.Log(LogLevel.Info,"RabbitMQ Client is trying to connect");
            lock (this.sync_root)
            {
                var policy = RetryPolicy.Handle<SocketException>()
                    .Or<BrokerUnreachableException>()
                    .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                    {
                        this._logger.LogException(LogLevel.Warn,string.Format("RabbitMQ Client could not connect after {0}s ({1})", $"{time.TotalSeconds:n1}", ex.Message),ex);
                    }
                );
                policy.Execute(() =>
                {
                    this._connection = _connectionFactory.CreateConnection();
                });

                if (IsConnected)
                {
                    this._connection.ConnectionShutdown += OnConnectionShutdown;
                    this._connection.CallbackException += OnCallbackException;
                    this._connection.ConnectionBlocked += OnConnectionBlocked;
                    this._logger.LogFormat(LogLevel.Info,"RabbitMQ Client acquired a persistent connection to '{0}' and is subscribed to failure events", _connection.Endpoint.HostName);
                    return true;
                }
                else
                {
                    this._logger.Log(LogLevel.Info,"FATAL ERROR: RabbitMQ connections could not be created and opened");
                    return false;
                }
            }
        }

        /// <summary>
        /// 连接中断重连
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
        {
            if (this._disposed) return;
            this._logger.Log(LogLevel.Warn,"A RabbitMQ connection is shutdown. Trying to re-connect...");
            this.TryConnect();
        }

        /// <summary>
        /// 连接异常重连
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnCallbackException(object sender, CallbackExceptionEventArgs e)
        {
            if (this._disposed) return;
            this._logger.Log(LogLevel.Warn,"A RabbitMQ connection throw exception. Trying to re-connect...");
            this.TryConnect();
        }

        /// <summary>
        /// 连接已关闭重连
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnConnectionShutdown(object sender, ShutdownEventArgs reason)
        {
            if (this._disposed) return;
            this._logger.Log(LogLevel.Warn,"A RabbitMQ connection is on shutdown. Trying to re-connect...");
            this.TryConnect();
        }
    }
}
#endif