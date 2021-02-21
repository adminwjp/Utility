#if !(NET20 || NET30 ||  NET35  || NET10 || NET11)
using System;
using System.Collections.Generic;
#if !(NET20 || NET30|| NET10 || NET11)
using System.Linq;
#endif
using Utility.EventBus.Abstractions;
using Utility.EventBus.Events;

namespace Utility.EventBus
{
    /// <summary>
    /// 基于内存 事件总线 订阅管理
    /// </summary>
    public partial class InMemoryEventBusSubscriptionsManager : IEventBusSubscriptionsManager
    {


        private readonly Dictionary<string, List<SubscriptionInfo>> _handlers;//订阅信息

        private readonly List<Type> _eventTypes;//集成事件处理类型

        /// <summary>
        /// 集成事件移除事件
        /// </summary>
#if !(NET40)
        public event EventHandler<string> OnEventRemoved;
#else
        public event Action<object,string> OnEventRemoved;
#endif

        /// <summary>
        /// 基于内存 事件总线 订阅管理
        /// </summary>
        public InMemoryEventBusSubscriptionsManager()
        {
            _handlers = new Dictionary<string, List<SubscriptionInfo>>();
            _eventTypes = new List<Type>();
        }

        /// <summary>
        /// 事件总线 是否为空
        /// </summary>
        public virtual bool IsEmpty => !(_handlers.Keys != null && _handlers.Keys.Count > 0);//!_handlers.Keys.Any();

        /// <summary>
        /// 清除所有订阅 信息
        /// </summary>
        public virtual void Clear() => _handlers.Clear();

        /// <summary>
        /// 添加 动态事件处理订阅
        /// </summary>
        /// <typeparam name="TH">动态集成事件处理<see cref="IDynamicIntegrationEventHandler"/></typeparam>
        /// <param name="eventName">事件名称 即  事件类型全名</param>
        public virtual void AddDynamicSubscription<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler
        {
            DoAddSubscription(typeof(TH), eventName, isDynamic: true);
        }

        /// <summary>
        /// 添加 事件处理订阅
        /// </summary>
        /// <typeparam name="T">集成事件<see cref="IntegrationEvent"/></typeparam>
        /// <typeparam name="TH">集成事件处理 <see cref="IIntegrationEventHandler{T}"/></typeparam>
        public virtual void AddSubscription<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = GetEventKey<T>();

            DoAddSubscription(typeof(TH), eventName, isDynamic: false);

            if (!_eventTypes.Contains(typeof(T)))
            {
                _eventTypes.Add(typeof(T));
            }
        }

        /// <summary>
        /// 添加订阅 信息到缓存中
        /// </summary>
        /// <param name="handlerType">集成事件处理 类型</param>
        /// <param name="eventName">事件类型全名 </param>
        /// <param name="isDynamic">是否是 动态集成事件处理</param>
        private void DoAddSubscription(Type handlerType, string eventName, bool isDynamic)
        {
            if (!HasSubscriptionsForEvent(eventName))
            {
                _handlers.Add(eventName, new List<SubscriptionInfo>());
            }
//#if !(NET20 || NET30 || NET10 || NET11)
//            if (_handlers[eventName].Any(s => s.HandlerType == handlerType))
//            {
//                throw new ArgumentException(
//                    $"Handler Type {handlerType.Name} already registered for '{eventName}'", nameof(handlerType));
//            }
//#endif
            foreach (var item in _handlers[eventName])
            {
                if (item.HandlerType == handlerType)
                {
                    throw new ArgumentException(
                   $"Handler Type {handlerType.Name} already registered for '{eventName}'", nameof(handlerType));
                }
            }
            if (isDynamic)
            {
                _handlers[eventName].Add(SubscriptionInfo.Dynamic(handlerType));
            }
            else
            {
                _handlers[eventName].Add(SubscriptionInfo.Typed(handlerType));
            }
        }


        /// <summary>
        /// 移除动态订阅
        /// </summary>
        /// <typeparam name="TH">动态集成事件处理<see cref="IDynamicIntegrationEventHandler"/></typeparam>
        /// <param name="eventName">事件名称 即  事件类型全名</param>
        public virtual void RemoveDynamicSubscription<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler
        {
            var handlerToRemove = FindDynamicSubscriptionToRemove<TH>(eventName);
            DoRemoveHandler(eventName, handlerToRemove);
        }

        /// <summary>
        /// 移除订阅
        /// </summary>
        /// <typeparam name="T">集成事件<see cref="IntegrationEvent"/></typeparam>
        /// <typeparam name="TH">集成事件处理<see cref="IIntegrationEventHandler{T}"/></typeparam>
        public virtual void RemoveSubscription<T, TH>()
            where TH : IIntegrationEventHandler<T>
            where T : IntegrationEvent
        {
            var handlerToRemove = FindSubscriptionToRemove<T, TH>();
            var eventName = GetEventKey<T>();
            DoRemoveHandler(eventName, handlerToRemove);
        }


        /// <summary>
        /// 根据事件名称 移除 订阅信息
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="subsToRemove">订阅信息</param>
        private void DoRemoveHandler(string eventName, SubscriptionInfo subsToRemove)
        {
            if (subsToRemove != null)
            {
                _handlers[eventName].Remove(subsToRemove);
                if (!(_handlers[eventName] != null && _handlers[eventName].Count > 0))
                //if (!_handlers[eventName].Any())
                {
                    _handlers.Remove(eventName);
                    var eventType = GetEventTypeByName(eventName);
                    if (eventType != null)
                    {
                        _eventTypes.Remove(eventType);
                    }
                    RaiseOnEventRemoved(eventName);
                }

            }
        }

        /// <summary>
        /// 根据集成事件 获取所有订阅信息
        /// </summary>
        /// <typeparam name="T">集成事件 <see cref="IntegrationEvent"/></typeparam>
        /// <returns></returns>
        public virtual IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : IntegrationEvent
        {
            var key = GetEventKey<T>();
            return GetHandlersForEvent(key);
        }

        /// <summary>
        /// 根据事件名称 获取所有订阅信息
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <returns></returns>
        public virtual IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName) => _handlers[eventName];

        /// <summary>
        /// 集成事件 移除 事件触发
        /// </summary>
        /// <param name="eventName"></param>
        private void RaiseOnEventRemoved(string eventName)
        {

            var handler = OnEventRemoved;
            if (handler != null)
            {
                OnEventRemoved(this, eventName);
            }
        }

        /// <summary>
        /// 根据 事件名称 获取 事件 订阅 第一个消息
        /// </summary>
        /// <typeparam name="TH">动态集成事件处理<see cref="IDynamicIntegrationEventHandler"/></typeparam>
        /// <param name="eventName">事件名称 即  事件类型名称</param>
        /// <returns></returns>
        private SubscriptionInfo FindDynamicSubscriptionToRemove<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler
        {
            return DoFindSubscriptionToRemove(eventName, typeof(TH));
        }


        /// <summary>
        /// 根据事件类型 获取 事件 订阅 第一个消息
        /// </summary>
        /// <typeparam name="T">集成事件<see cref="IntegrationEvent"/></typeparam>
        /// <typeparam name="TH">集成事件处理<see cref="IIntegrationEventHandler{T}"/></typeparam>
        /// <returns></returns>
        private SubscriptionInfo FindSubscriptionToRemove<T, TH>()
             where T : IntegrationEvent
             where TH : IIntegrationEventHandler<T>
        {
            var eventName = GetEventKey<T>();
            return DoFindSubscriptionToRemove(eventName, typeof(TH));
        }

        /// <summary>
        /// 根据 事件名称 获取 事件 订阅 第一个消息
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="handlerType">集成事件处理 类型</param>
        /// <returns></returns>
        private SubscriptionInfo DoFindSubscriptionToRemove(string eventName, Type handlerType)
        {
            if (!HasSubscriptionsForEvent(eventName))
            {
                return null;
            }

            // return _handlers[eventName].SingleOrDefault(s => s.HandlerType == handlerType);
            var subscription = (SubscriptionInfo)null;
            foreach (var item in _handlers[eventName])
            {
                if (item.HandlerType == handlerType)
                {
                    subscription = item;
                    break;
                }
            }
            return subscription;
        }

        /// <summary>
        ///根据订阅事件 是否存在订阅事件
        /// </summary>
        /// <typeparam name="T">集成事件 <see cref="IntegrationEvent"/></typeparam>
        /// <returns></returns>
        public virtual bool HasSubscriptionsForEvent<T>() where T : IntegrationEvent
        {
            var key = GetEventKey<T>();
            return HasSubscriptionsForEvent(key);
        }

        /// <summary>
        /// 根据事件名称 是否存在订阅事件
        /// </summary>
        /// <param name="eventName">事件名称 即  事件类型名称</param>
        /// <returns></returns>
        public virtual bool HasSubscriptionsForEvent(string eventName) => _handlers.ContainsKey(eventName);

        /// <summary>
        /// 根据事件名称 获取事件类型
        /// </summary>
        /// <param name="eventName">事件名称 即  事件类型名称</param>
        /// <returns></returns>
        public virtual Type GetEventTypeByName(string eventName)
        {
           // return _eventTypes.SingleOrDefault(t => t.Name == eventName);
            var eventType = (Type)null;
            foreach (var item in _eventTypes)
            {
                if (item.Name == eventName)
                {
                    eventType = item;
                    break;
                }
            }
            return eventType;
        }



        /// <summary>
        /// 根据集成事件 获取事件名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual string GetEventKey<T>()
        {
            return typeof(T).Name;
        }
    }
}
#endif