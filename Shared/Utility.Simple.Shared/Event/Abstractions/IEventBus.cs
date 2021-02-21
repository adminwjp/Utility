#if !(NET20 || NET30 ||  NET35  || NET10 || NET11)
using Utility.EventBus.Events;
using System;

namespace Utility.EventBus.Abstractions
{
    /// <summary>
    /// 事件总线
    /// </summary>
    public interface IEventBus
    {
        /// <summary>
        /// 发布事件
        /// </summary>
        /// <param name="event">集成事件</param>
        void Publish(IntegrationEvent @event);

        /// <summary>
        /// 订阅事件
        /// </summary>
        /// <typeparam name="T">集成事件<see cref="IntegrationEvent"/></typeparam>
        /// <typeparam name="TH">集成事件处理 <see cref="IIntegrationEventHandler{T}"/></typeparam>
        void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;

        /// <summary>
        /// 订阅动态事件
        /// </summary>
        /// <typeparam name="TH">动态事件处理<see cref="IDynamicIntegrationEventHandler"/></typeparam>
        /// <param name="eventName">事件类型名称</param>
        void SubscribeDynamic<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler;

        /// <summary>
        /// 取消订阅动态事件
        /// </summary>
        /// <typeparam name="TH"></typeparam>
        /// <param name="eventName"></param>
        void UnsubscribeDynamic<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler;

        /// <summary>
        /// 取消订阅事件
        /// </summary>
        /// <typeparam name="T">集成事件 <see cref="IntegrationEvent"/></typeparam>
        /// <typeparam name="TH">事件处理 <see cref="IIntegrationEventHandler{T}"/></typeparam>
        void Unsubscribe<T, TH>()
            where TH : IIntegrationEventHandler<T>
            where T : IntegrationEvent;
    }
}
#endif
