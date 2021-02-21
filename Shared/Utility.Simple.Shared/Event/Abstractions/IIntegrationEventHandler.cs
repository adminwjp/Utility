#if !(NET20 || NET30 ||  NET35  || NET10 || NET11)
using Utility.EventBus.Events;
using System.Threading.Tasks;

namespace Utility.EventBus.Abstractions
{
    /// <summary>
    /// 集成事件处理
    /// </summary>
    public interface IIntegrationEventHandlerAsync<in TIntegrationEvent> 
        where TIntegrationEvent: IntegrationEvent
    {
        /// <summary>
        /// 订阅 处理
        /// </summary>
        /// <param name="event">集成事件</param>
        /// <returns></returns>
        Task HandleAsync(TIntegrationEvent @event);
    }

    /// <summary>
    /// 集成事件处理
    /// </summary>
    public interface IIntegrationEventHandler<in TIntegrationEvent> 
        where TIntegrationEvent : IntegrationEvent
    {    
        /// <summary>
         /// 订阅 处理
         /// </summary>
         /// <param name="event">集成事件</param>
         /// <returns></returns>
        void Handle(TIntegrationEvent @event);
    }
}
#endif