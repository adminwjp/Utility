#if !NET20 && !NET30 && !NET35
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility.EventBus.Events;

namespace Utility.Application.IntegrationEvents
{
    /// <summary>
    /// 集成事件服务接口
    /// </summary>
    public interface IIntegrationEventService
    {
        /// <summary>
        /// 发布事件 到事件总线
        /// </summary>
        /// <returns></returns>
        Task PublishEventsThroughEventBusAsync();

        /// <summary>
        /// 添加 或 保存事件
        /// </summary>
        /// <param name="evt"></param>
        /// <returns></returns>
        Task AddAndSaveEventAsync(IntegrationEvent evt);
    }
}
#endif