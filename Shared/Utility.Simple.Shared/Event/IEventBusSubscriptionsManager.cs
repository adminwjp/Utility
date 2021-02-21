#if !(NET20 || NET30 ||  NET35  || NET10 || NET11)
using Utility.EventBus.Abstractions;
using Utility.EventBus.Events;
using System;
using System.Collections.Generic;
using static Utility.EventBus.InMemoryEventBusSubscriptionsManager;

namespace Utility.EventBus
{
    /// <summary>
    /// 事件总线 订阅管理
    /// </summary>
    public interface IEventBusSubscriptionsManager
    {
        /// <summary>
        /// 事件总线 是否为空
        /// </summary>
        bool IsEmpty { get; }

#if !(NET40)
        /// <summary>
        /// 集成事件移除事件
        /// </summary>
        event EventHandler<string> OnEventRemoved;
#endif

        /// <summary>
        /// 添加 动态事件处理订阅
        /// </summary>
        /// <typeparam name="TH">动态集成事件处理<see cref="IDynamicIntegrationEventHandler"/></typeparam>
        /// <param name="eventName">事件名称 即  事件类型名称</param>
        void AddDynamicSubscription<TH>(string eventName)
           where TH : IDynamicIntegrationEventHandler;

        /// <summary>
        /// 添加 事件处理订阅
        /// </summary>
        /// <typeparam name="T">集成事件<see cref="IntegrationEvent"/></typeparam>
        /// <typeparam name="TH">集成事件处理 <see cref="IIntegrationEventHandler{T}"/></typeparam>
        void AddSubscription<T, TH>()
           where T : IntegrationEvent
           where TH : IIntegrationEventHandler<T>;

        /// <summary>
        /// 移除订阅
        /// </summary>
        /// <typeparam name="T">集成事件<see cref="IntegrationEvent"/></typeparam>
        /// <typeparam name="TH">集成事件处理<see cref="IIntegrationEventHandler{T}"/></typeparam>
        void RemoveSubscription<T, TH>()
             where TH : IIntegrationEventHandler<T>
             where T : IntegrationEvent;

        /// <summary>
        /// 移除动态订阅
        /// </summary>
        /// <typeparam name="TH">动态集成事件处理<see cref="IDynamicIntegrationEventHandler"/></typeparam>
        /// <param name="eventName">事件名称 即  事件类型名称</param>
        void RemoveDynamicSubscription<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler;

        /// <summary>
        ///根据订阅事件 是否存在订阅事件
        /// </summary>
        /// <typeparam name="T">集成事件 <see cref="IntegrationEvent"/></typeparam>
        /// <returns></returns>
        bool HasSubscriptionsForEvent<T>() where T : IntegrationEvent;

        /// <summary>
        /// 根据事件名称 是否存在订阅事件
        /// </summary>
        /// <param name="eventName">事件名称 即  事件类型名称</param>
        /// <returns></returns>
        bool HasSubscriptionsForEvent(string eventName);

        /// <summary>
        /// 根据事件名称 获取事件类型
        /// </summary>
        /// <param name="eventName">事件名称 即  事件类型名称</param>
        /// <returns></returns>
        Type GetEventTypeByName(string eventName);

        /// <summary>
        /// 清除所有订阅 信息
        /// </summary>
        void Clear();

        /// <summary>
        /// 根据集成事件 获取所有订阅信息
        /// </summary>
        /// <typeparam name="T">集成事件 <see cref="IntegrationEvent"/></typeparam>
        /// <returns></returns>
        IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : IntegrationEvent;

        /// <summary>
        /// 根据事件名称 获取所有订阅信息
        /// </summary>
        /// <param name="eventName">事件名称 即  事件类型名称</param>
        /// <returns></returns>
        IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName);

        /// <summary>
        /// 根据集成事件 获取事件名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        string GetEventKey<T>();
    }
}
#endif