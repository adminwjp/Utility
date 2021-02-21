#if NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
namespace Utility.EventBus
{
    using Autofac;
    using Utility.EventBus.Abstractions;
    using Utility.EventBus.Events;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using Utility.ServiceBus;
    using Utility.Logs;

    /// <summary>
    /// 基于 ServiceBus 事件 总线
    /// </summary>
    public class EventBusServiceBus : IEventBus
    {
        private readonly DefaultServiceBus _defaultServiceBus;
        private readonly ILog<EventBusServiceBus> _logger;
        private readonly IEventBusSubscriptionsManager _subsManager;
        private readonly ILifetimeScope _autofac;
        private readonly string AUTOFAC_SCOPE_NAME = "event_bus";
        private const string INTEGRATION_EVENT_SUFIX = "IntegrationEvent";

        /// <summary>
        /// 基于 ServiceBus 事件 总线
        /// </summary>
        /// <param name="serviceBusPersisterConnection"></param>
        /// <param name="logger"></param>
        /// <param name="subsManager"></param>
        /// <param name="subscriptionClientName"></param>
        /// <param name="autofac"></param>
        public EventBusServiceBus(IServiceBusPersisterConnection serviceBusPersisterConnection,
            ILog<EventBusServiceBus> logger, IEventBusSubscriptionsManager subsManager, string subscriptionClientName,
            ILifetimeScope autofac)
        {
            _defaultServiceBus = new DefaultServiceBus(serviceBusPersisterConnection, autofac.Resolve<ILog<DefaultServiceBus>>(), subscriptionClientName, ProcessEvent);
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _subsManager = subsManager ?? new InMemoryEventBusSubscriptionsManager();
            _autofac = autofac;
        }

        /// <summary>
        /// 发布事件
        /// </summary>
        /// <param name="event">集成事件</param>
        public virtual void Publish(IntegrationEvent @event)
        {
            var eventName = @event.GetType().Name.Replace(INTEGRATION_EVENT_SUFIX, "");
            var jsonMessage = JsonConvert.SerializeObject(@event);
            var body = Encoding.UTF8.GetBytes(jsonMessage);
            _defaultServiceBus.Publish(Guid.NewGuid().ToString(), body, eventName);
        }

        /// <summary>
        /// 订阅动态事件
        /// </summary>
        /// <typeparam name="TH">动态事件处理<see cref="IDynamicIntegrationEventHandler"/></typeparam>
        /// <param name="eventName">事件类型名称</param>
        public virtual void SubscribeDynamic<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler
        {
            _logger.Log(Logs.LogLevel.Info,$"Subscribing to dynamic event {eventName} with {nameof(TH)}");
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
            var eventName = typeof(T).Name.Replace(INTEGRATION_EVENT_SUFIX, "");

            var containsKey = _subsManager.HasSubscriptionsForEvent<T>();
            if (!containsKey)
            {
                _defaultServiceBus.Subscribe(eventName);
            }
            _logger.Log(Logs.LogLevel.Info, $"Subscribing to  event {eventName} with {nameof(TH)}");
            _subsManager.AddSubscription<T, TH>();
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
            var eventName = typeof(T).Name.Replace(INTEGRATION_EVENT_SUFIX, "");
            _defaultServiceBus.Unsubscribe(eventName);
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
            _logger.Log(Logs.LogLevel.Info, $"Unsubscribing from dynamic  event {eventName} ");

            _subsManager.RemoveDynamicSubscription<TH>(eventName);
        }

        /// <summary>
        /// 释放资源 清空订阅信息
        /// </summary>
        public virtual void Dispose()
        {
            _subsManager.Clear();
        }

        /// <summary>
        /// 基于 ServiceBus 消息队列 事件处理
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private async Task<bool> ProcessEvent(string eventName, string message)
        {
            var processed = false;
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
                processed = true;
            }
            return processed;
        }
    }
}
#endif