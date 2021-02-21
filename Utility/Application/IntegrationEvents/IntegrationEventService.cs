#if NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0|| NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
#if NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;
#endif
#if NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48
using System.Data.Entity;
#endif
using System;
using System.Data.Common;
using System.Threading.Tasks;
using Utility.Domain.Entities;
using Utility.EventBus.Abstractions;
using Utility.EventBus.Events;
using Utility.Infrastructure;
using Utility.IntegrationEventLog;
using Utility.IntegrationEventLog.Services;
using Utility.Logs;

namespace Utility.Application.IntegrationEvents
{
    /// <summary>
    /// 集成事件服务
    /// </summary>
    public class IntegrationEventService<EventService,Context, Entity> : IIntegrationEventService
        where EventService: IntegrationEventService<EventService, Context, Entity>
          where Context : BaseDbContext<Context, Entity>
        where Entity : class, IDomainEvent
    {
        /// <summary>
        /// 程序集名称
        /// </summary>
        public static string AppName { get; set; } = "Test";
        private readonly Func<DbConnection, IIntegrationEventLogService> integrationEventLogServiceFactory;
        private readonly IEventBus eventBus;
        private readonly Context context;
        private readonly IntegrationEventLogContext eventLogContext;
        private readonly IIntegrationEventLogService eventLogService;
        private readonly ILog<EventService> logger;

        /// <summary>
        /// 集成事件服务
        /// </summary>
        /// <param name="eventBus"></param>
        /// <param name="context"></param>
        /// <param name="eventLogContext"></param>
        /// <param name="integrationEventLogServiceFactory"></param>
        /// <param name="logger"></param>
        public IntegrationEventService(IEventBus eventBus,
            Context context,
            IntegrationEventLogContext eventLogContext,
            Func<DbConnection, IIntegrationEventLogService> integrationEventLogServiceFactory,
           ILog<EventService> logger)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.eventLogContext = eventLogContext ?? throw new ArgumentNullException(nameof(eventLogContext));
            this.integrationEventLogServiceFactory = integrationEventLogServiceFactory ?? throw new ArgumentNullException(nameof(integrationEventLogServiceFactory));
            this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
#if NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
            eventLogService = this.integrationEventLogServiceFactory(context.Database.GetDbConnection());
#else
            eventLogService = this.integrationEventLogServiceFactory(context.Database.GetConnection<DbConnection>());
#endif
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        /// <summary>
        /// 发布事件 到事件总线
        /// </summary>
        /// <returns></returns>
        public virtual async Task PublishEventsThroughEventBusAsync()
        {
            var pendindLogEvents = await eventLogService.RetrieveEventLogsPendingToPublishAsync();

            foreach (var logEvt in pendindLogEvents)
            {
                logger.Log(LogLevel.Info, $"----- Publishing integration event: { logEvt.EventId} from {AppName} - ({logEvt.IntegrationEvent})");

                try
                {
                    await eventLogService.MarkEventAsInProgressAsync(logEvt.EventId);
                    eventBus.Publish(logEvt.IntegrationEvent);
                    await eventLogService.MarkEventAsPublishedAsync(logEvt.EventId);
                }
                catch (Exception ex)
                {
                    logger.LogException(LogLevel.Error, "ERROR publishing integration event: {logEvt.EventId} from {AppName}",ex);

                    await eventLogService.MarkEventAsFailedAsync(logEvt.EventId);
                }
            }
        }

        /// <summary>
        /// 添加 或 保存事件
        /// </summary>
        /// <param name="evt"></param>
        /// <returns></returns>
        public virtual async Task AddAndSaveEventAsync(IntegrationEvent evt)
        {
            logger.Log(LogLevel.Info, $"----- Enqueuing integration event {evt.Id} to repository ({evt})");
#if NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
            await eventLogService.SaveEventAsync(evt, context.GetCurrentTransaction.GetDbTransaction());
#else
            await eventLogService.SaveEventAsync(evt, context.GetCurrentTransaction.UnderlyingTransaction);
#endif
        }
    }
}
#endif