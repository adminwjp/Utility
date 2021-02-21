#if !(NET20 || NET30 ||  NET35  || NET10 || NET11)
using System.Threading.Tasks;

namespace Utility.EventBus.Abstractions
{
    /// <summary>
    /// 动态集成事件处理 接口
    /// </summary>
    public interface IDynamicIntegrationEventHandler
    {
        /// <summary>
        /// 事件处理
        /// </summary>
        /// <param name="eventData">动态集成事件数据</param>
        /// <returns></returns>
        Task Handle(dynamic eventData);
    }
}
#endif
