#if !(NET20 || NET30 ||  NET35  || NET10 || NET11)
using System;

namespace Utility.EventBus
{
    /// <summary>
    /// 基于内存 事件总线订阅管理
    /// </summary>
    public partial class InMemoryEventBusSubscriptionsManager : IEventBusSubscriptionsManager
    {
        /// <summary>
        /// 订阅信息
        /// </summary>
        public class SubscriptionInfo
        {
            /// <summary>
            /// 是否是 动态类型
            /// </summary>
            public bool IsDynamic { get; }
           
            /// <summary>
            ///集成事件处理类型
            /// </summary>
            public Type HandlerType{ get; }

            /// <summary>
            /// 订阅信息
            /// </summary>
            /// <param name="isDynamic">是否是 动态类型</param>
            /// <param name="handlerType">集成事件处理类型</param>
            private SubscriptionInfo(bool isDynamic, Type handlerType)
            {
                IsDynamic = isDynamic;
                HandlerType = handlerType;
            }

            /// <summary>
            /// 动态订阅信息
            /// </summary>
            /// <param name="handlerType">集成事件处理类型</param>
            /// <returns></returns>
            public static SubscriptionInfo Dynamic(Type handlerType)
            {
                return new SubscriptionInfo(true, handlerType);
            }

            /// <summary>
            /// 普通订阅信息
            /// </summary>
            /// <param name="handlerType">集成事件处理类型</param>
            /// <returns></returns>
            public static SubscriptionInfo Typed(Type handlerType)
            {
                return new SubscriptionInfo(false, handlerType);
            }
        }
    }
}
#endif