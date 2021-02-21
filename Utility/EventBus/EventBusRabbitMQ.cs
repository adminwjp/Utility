#if NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0|| NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using Autofac;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.EventBus.Abstractions;
using Utility.EventBus.Events;
using Utility.EventBus.Extensions;
using Utility.Logs;
using Utility.MessageQueue;

namespace Utility.EventBus
{
    /// <summary>
    /// 基于 rabbit 事件 总线
    /// </summary>
    public class EventBusRabbitMQ : IEventBus, IDisposable
    {
        const string BROKER_NAME = "event_bus";
        private readonly DefaultRabbitMQ _defaultRabbitMQ;
        private readonly ILog<EventBusRabbitMQ> _logger;
        private readonly IEventBusSubscriptionsManager _subsManager;
        private readonly ILifetimeScope _autofac;
        private readonly string AUTOFAC_SCOPE_NAME = "event_bus";
        private  string _queueName;

        /// <summary>
        /// 基于 rabbit 事件 总线
        /// </summary>
        /// <param name="persistentConnection"></param>
        /// <param name="logger"></param>
        /// <param name="autofac"></param>
        /// <param name="subsManager"></param>
        /// <param name="queueName"></param>
        /// <param name="retryCount"></param>
        public EventBusRabbitMQ(IRabbitMQPersistentConnection persistentConnection, ILog<EventBusRabbitMQ> logger,
            ILifetimeScope autofac, IEventBusSubscriptionsManager subsManager, string queueName = null, int retryCount = 5)
        {
            _defaultRabbitMQ = new DefaultRabbitMQ(persistentConnection, autofac.Resolve<ILog<DefaultRabbitMQ>>(), processEvent: ProcessEvent, retryCount);
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _subsManager = subsManager ?? new InMemoryEventBusSubscriptionsManager();
            _autofac = autofac;
            _queueName = queueName;
            _subsManager.OnEventRemoved += SubsManager_OnEventRemoved;
        }

        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventName"></param>
        private void SubsManager_OnEventRemoved(object sender, string eventName)
        {
            _defaultRabbitMQ.Removed(queueName: _queueName,
                    exchange: BROKER_NAME,
                    routingKey: eventName);
            if (_subsManager.IsEmpty)
            {
                _queueName = string.Empty;
                _defaultRabbitMQ.ConsumerChannel?.Close();
            }
        }

        /// <summary>
        /// 发布事件
        /// </summary>
        /// <param name="event">集成事件</param>
        public virtual void Publish(IntegrationEvent @event)
        {
            var message = JsonConvert.SerializeObject(@event);
            var body = Encoding.UTF8.GetBytes(message);
            var eventName = @event.GetType().Name;
            _defaultRabbitMQ.Publish(BROKER_NAME, eventName,body);
        }

        /// <summary>
        /// 订阅动态事件
        /// </summary>
        /// <typeparam name="TH">动态事件处理<see cref="IDynamicIntegrationEventHandler"/></typeparam>
        /// <param name="eventName">事件类型名称</param>
        public virtual void SubscribeDynamic<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler
        {
            _logger.Log(LogLevel.Info,$"Subscribing to dynamic event {eventName} with {typeof(TH).GetGenericTypeName()}");

            DoInternalSubscription(eventName);
            _subsManager.AddDynamicSubscription<TH>(eventName);
        }

        /// <summary>
        /// 订阅事件
        /// </summary>
        /// <typeparam name="T">集成事件<see cref="IntegrationEvent"/></typeparam>
        /// <typeparam name="TH">集成事件处理 <see cref="IIntegrationEventHandler{T}"/></typeparam>
        public virtual void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = _subsManager.GetEventKey<T>();
            DoInternalSubscription(eventName);

            _logger.Log(LogLevel.Info, $"Subscribing to  event {eventName} with {typeof(TH).GetGenericTypeName()}");

            _subsManager.AddSubscription<T, TH>();
        }

        /// <summary>
        /// 订阅事件
        /// </summary>
        /// <param name="eventName">事件类型名称</param>
        private void DoInternalSubscription(string eventName)
        {
            var containsKey = _subsManager.HasSubscriptionsForEvent(eventName);
            if (!containsKey)
            {
                _defaultRabbitMQ.Subscription(queueName: _queueName,
                   exchange: BROKER_NAME,
                   routingKey: eventName);
            }
        }

        /// <summary>
        /// 取消订阅事件
        /// </summary>
        /// <typeparam name="T">集成事件 <see cref="IntegrationEvent"/></typeparam>
        /// <typeparam name="TH">事件处理 <see cref="IIntegrationEventHandler{T}"/></typeparam>
        public virtual void Unsubscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = _subsManager.GetEventKey<T>();
            _logger.Log(LogLevel.Info, $"Unsubscribing from event {eventName} ");

            _subsManager.RemoveSubscription<T, TH>();
        }

        /// <summary>
        /// 取消订阅动态事件
        /// </summary>
        /// <typeparam name="TH"></typeparam>
        /// <param name="eventName"></param>
        public virtual void UnsubscribeDynamic<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler
        {
            _subsManager.RemoveDynamicSubscription<TH>(eventName);
        }

        /// <summary>
        /// 释放资源 消费者关闭 订阅信息清理
        /// </summary>
        public virtual void Dispose()
        {
            if (_defaultRabbitMQ.ConsumerChannel != null)
            {
                _defaultRabbitMQ.ConsumerChannel.Dispose();
            }

            _subsManager.Clear();
        }

        /// <summary>
        /// 基于 rabbitmq 消息对应 事件处理
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private async Task ProcessEvent(string eventName, string message)
        {
            if (_subsManager.HasSubscriptionsForEvent(eventName))
            {
                using (var scope = _autofac.BeginLifetimeScope(AUTOFAC_SCOPE_NAME))
                {
                    var subscriptions = _subsManager.GetHandlersForEvent(eventName);
                    foreach (var subscription in subscriptions)
                    {
                        if (subscription.IsDynamic)
                        {
                            var handler = scope.ResolveOptional(subscription.HandlerType) as IDynamicIntegrationEventHandler;
                            if (handler == null) continue;
                            dynamic eventData = JObject.Parse(message);
                            await handler.Handle(eventData);
                        }
                        else
                        {
                            var handler = scope.ResolveOptional(subscription.HandlerType);
                            if (handler == null) continue;
                            var eventType = _subsManager.GetEventTypeByName(eventName);
                            var integrationEvent = JsonConvert.DeserializeObject(message, eventType);
                            var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
                            await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
                        }
                    }
                }
            }
        }
    }
}
#endif