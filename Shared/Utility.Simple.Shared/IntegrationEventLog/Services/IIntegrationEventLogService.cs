#if !(NET20 || NET30 || NET35 || NET10 || NET11)
using System;
using System.Collections.Generic;
#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
using System.Data.Common;
#endif
using System.Threading.Tasks;
using Utility.EventBus.Events;

namespace Utility.IntegrationEventLog.Services
{
    /// <summary>
    /// 集成 事件 日志服务 接口
    /// </summary>
    public interface IIntegrationEventLogService
    {
        /// <summary>
        /// 集成事件日志发布
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<IntegrationEventLogEntry>> RetrieveEventLogsPendingToPublishAsync();


        /// <summary>
        /// 集成事件日志 保存
        /// </summary>
        /// <param name="event">事件</param>
        /// <param name="transaction">事务</param>
        /// <returns></returns>
#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
        Task SaveEventAsync(IntegrationEvent @event, DbTransaction transaction);
#endif

        /// <summary>
        /// 创建集成事件日志 发布
        /// </summary>
        /// <param name="eventId">事件 id</param>
        /// <returns></returns>
        Task MarkEventAsPublishedAsync(string eventId);


        /// <summary>
        /// 创建集成事件日志 进程发布
        /// </summary>
        /// <param name="eventId">事件 id</param>
        /// <returns></returns>
        Task MarkEventAsInProgressAsync(string eventId);

        /// <summary>
        /// 创建集成事件日志 保存失败
        /// </summary>
        /// <param name="eventId">事件 id</param>
        /// <returns></returns>
        Task MarkEventAsFailedAsync(string eventId);
    }
}
#endif