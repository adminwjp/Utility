#if NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Utility.Logs;

namespace Utility.MessageQueue
{

    /// <summary>
    /// 使用浏览器打开http://localhost:15672 访问Rabbit Mq的管理控制台，
    /// 使用刚才创建的账号登陆系统即可。
    /// Rabbit MQ 管理后台，可以更好的可视化方式查看RabbitMQ服务器实例的状态
    /// RabbitMQ
    /// </summary>

    public class DefaultRabbitMQ:IDisposable
    {
        private readonly IRabbitMQPersistentConnection _persistentConnection;
        private readonly ILog<DefaultRabbitMQ> _logger;
        private readonly int _retryCount;
        private IModel _consumerChannel;
        
        /// <summary>
        /// 消费者 
        /// </summary>
        public IModel ConsumerChannel { get{ return this._consumerChannel; } }
        Func<string, string,Task> _processEvent;

        /// <summary>
        /// RabbitMQ
        /// </summary>
        /// <param name="persistentConnection"></param>
        /// <param name="logger"></param>
        /// <param name="processEvent"></param>
        /// <param name="retryCount"></param>
        public DefaultRabbitMQ(IRabbitMQPersistentConnection persistentConnection, ILog<DefaultRabbitMQ> logger, Func<string, string,Task> processEvent, int retryCount = 5)
        {
            this._persistentConnection = persistentConnection ?? throw new ArgumentNullException(nameof(persistentConnection));
            this._logger =logger;
            this._processEvent = processEvent;
            this._retryCount = retryCount;
        }

        /// <summary>
        /// 消费端(客服端) 消费 数据
        /// </summary>
        /// <param name="queueName">队列名称(不是 交换机 to 队列名称)</param>
        /// <param name="exchange">交换机 默认 null</param>
        /// <param name="type">交换机类型  direct headers fanout topic 默认 direct 暂时没用 </param>
        public virtual void Initial(string queueName, string exchange, string type = "direct")
        {
            this._consumerChannel = CreateConsumerChannel(queueName,exchange,type);
        }
        /// <summary>
        /// 删除 交换机 绑定 队列 和 to 队列 不会删除 交换机 
        /// 不存在 交换机 会 创建 
        /// 存在 不存在队列 则 异常
        /// </summary>
        /// <param name="queueName">队列</param>
        /// <param name="exchange">交换机</param>
        /// <param name="routingKey">to 队列</param>
        public virtual void Removed(string queueName,string exchange,string routingKey)
        {
            if (!this._persistentConnection.IsConnected)
            {
                this._persistentConnection.TryConnect();
            }
            using (var channel = _persistentConnection.CreateModel())
            {
#if NET45
                channel.QueueUnbind(queue: queueName, exchange: exchange,  routingKey: routingKey,arguments:null);
#else
                 channel.QueueUnbind(queue: queueName, exchange: exchange,  routingKey: routingKey);
#endif
            }
        }
        /// <summary>
        /// 创建 交换机 绑定 队列 和 to 队列
        /// 前提必须存在 交换机
        /// </summary>
        /// <param name="queueName">to 队列</param>
        /// <param name="exchange">交换机</param>
        /// <param name="routingKey">交换机 队列</param>
        public virtual void Subscription(string queueName, string exchange, string routingKey)
        {
            if (!this._persistentConnection.IsConnected)
            {
                this._persistentConnection.TryConnect();
            }
            using (var channel = _persistentConnection.CreateModel())
            {
                channel.QueueBind(queue: queueName,  exchange: exchange,routingKey: routingKey);
            }
        }
        /// <summary>
        /// 生产者 生产 数据
        /// </summary>
        /// <param name="exchange">交换机(要存在)</param>
        /// <param name="routingKey">队列名称(交换机  队列名称 不是 那个队列名称)</param>
        /// <param name="body">内容</param>
        public virtual void Publish(string exchange,string routingKey, byte[] body)
        {
            if (!this._persistentConnection.IsConnected)
            {
                this._persistentConnection.TryConnect();
            }
            var policy = RetryPolicy.Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetry(this._retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                {
                    this._logger.LogException(LogLevel.Warn,string.Format("Could not publish event: {0} after {1}s ({2})", exchange, $"{time.TotalSeconds:n1}", ex.Message),ex);
                });
            using (var channel = this._persistentConnection.CreateModel())
            {
                policy.Execute(() =>
                {
                    var properties = channel.CreateBasicProperties();
                    properties.DeliveryMode =2; // persistent
                    properties.Persistent = true;
                    channel.BasicPublish(exchange: exchange, routingKey: routingKey, mandatory: true,  basicProperties: properties,body: body);
                });
            }
        }
        /// <summary>
        /// 创建消费者(客户端) 消费 数据
        /// </summary>
        /// <param name="queueName">队列</param>
        /// <param name="exchange">交换机 默认 "" 不使用交换机</param>
        /// <param name="type">fanout direct topic header 默认 direct</param>
        /// <returns></returns>
        private  IModel CreateConsumerChannel(string queueName, string exchange,string type= "direct")
        {
            if (!this._persistentConnection.IsConnected)
            {
                this._persistentConnection.TryConnect();
            }
            var channel = this._persistentConnection.CreateModel();
            //if(!string.IsNullOrEmpty(exchange))
            //{
            //    channel.ExchangeDeclare(exchange: exchange, type: type); //最好不要用 好像没 影响  error 
            //}
            channel.QueueDeclare(queue: queueName,   durable: true,  exclusive: false, autoDelete: false, arguments: null);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received +=async  (model, ea) =>
            {
                var eventName = ea.RoutingKey;
#if NET45 || NET451 || NET452 || NET46
                    var message = Encoding.UTF8.GetString(ea.Body);
#else
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
#endif
                await this._processEvent(eventName, message);
                channel.BasicAck(ea.DeliveryTag, multiple: false);//手动确认后 数据 不会 存在
            };
#if NET45
              channel.BasicConsume(queue: queueName,  noAck: false,consumer: consumer);
#else
               channel.BasicConsume(queue: queueName,  autoAck: false,consumer: consumer);
#endif
            channel.CallbackException += (sender, ea) =>
            {
                this._consumerChannel.Dispose();
                this._consumerChannel = CreateConsumerChannel(queueName,exchange);
            };
            return channel;
        }

        /// <summary>
        /// 释放资源 消费者释放
        /// </summary>
        public virtual void Dispose()
        {
            if (this._consumerChannel != null)
            {
                this._consumerChannel.Dispose();
            }
        }
    }
}
#endif
