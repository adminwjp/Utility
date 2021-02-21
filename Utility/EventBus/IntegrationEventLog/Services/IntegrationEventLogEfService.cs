//#if !(NET10 || NET11 || NET20 || NET30 || NET35 || NET40 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
#if NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1

#if NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48
using System.Data.Entity;
#else
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
#endif
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Utility.EventBus.Events;

namespace Utility.IntegrationEventLog.Services
{
    /// <summary>
    /// 集成 事件 日志服务 
    /// </summary>
    public class IntegrationEventLogEfService : IIntegrationEventLogService
    {
        private readonly IntegrationEventLogContext _integrationEventLogContext;
        //private readonly DbConnection _dbConnection;
        private readonly List<Type> _eventTypes;

        /*
        public IntegrationEventLogEfService(DbConnection dbConnection)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
#if NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48
          _integrationEventLogContext = new IntegrationEventLogContext(_dbConnection.ConnectionString);
#else
#if NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_1
         _integrationEventLogContext = new IntegrationEventLogContext(
                new DbContextOptionsBuilder<IntegrationEventLogContext>()
                    .UseSqlServer(_dbConnection)
                    .ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryPossibleExceptionWithAggregateOperatorWarning))
                    .Options);
#else
         _integrationEventLogContext = new IntegrationEventLogContext(
                new DbContextOptionsBuilder<IntegrationEventLogContext>()
                    .UseSqlServer(_dbConnection)
                    .ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning))
                    .Options);
#endif
        
            
#endif

            _eventTypes = Assembly.Load(Assembly.GetEntryAssembly().FullName)
                .GetTypes()
                .Where(t => t.Name.EndsWith(nameof(IntegrationEvent)))
                .ToList();
        }    
		*/

        /// <summary>
        /// 集成 事件 日志服务 
        /// </summary>
        /// <param name="context"></param>
        public IntegrationEventLogEfService(IntegrationEventLogContext context)
        {
            this._integrationEventLogContext = context;
            _eventTypes = Assembly.Load(Assembly.GetEntryAssembly().FullName)
                .GetTypes()
                .Where(t => t.Name.EndsWith(nameof(IntegrationEvent)))
                .ToList();
        }

        /// <summary>
        /// 集成事件日志发布
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<IntegrationEventLogEntry>> RetrieveEventLogsPendingToPublishAsync()
        {
            return await _integrationEventLogContext.IntegrationEventLogs
                .Where(e => e.State == EventStateEnum.NotPublished)
                .OrderBy(o => o.CreationTime)
                .Select(e => e.DeserializeJsonContent(_eventTypes.Find(t=> t.Name == e.EventTypeShortName)))
                .ToListAsync();              
        }


        /// <summary>
        /// 集成事件日志 保存
        /// </summary>
        /// <param name="event">事件</param>
        /// <param name="transaction">事务</param>
        /// <returns></returns>
        public Task SaveEventAsync(IntegrationEvent @event, DbTransaction transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction), $"A {typeof(DbTransaction).FullName} is required as a pre-requisite to save the event.");
            }

            var eventLogEntry = new IntegrationEventLogEntry(@event);

            _integrationEventLogContext.Database.UseTransaction(transaction);
            _integrationEventLogContext.IntegrationEventLogs.Add(eventLogEntry);

            return _integrationEventLogContext.SaveChangesAsync();
        }

        /// <summary>
        /// 创建集成事件日志 发布
        /// </summary>
        /// <param name="eventId">事件 id</param>
        /// <returns></returns>
        public Task MarkEventAsPublishedAsync(string eventId)
        {
            return UpdateEventStatus(eventId, EventStateEnum.Published);
        }

        /// <summary>
        /// 创建集成事件日志 进程发布
        /// </summary>
        /// <param name="eventId">事件 id</param>
        /// <returns></returns>
        public Task MarkEventAsInProgressAsync(string eventId)
        {
            return UpdateEventStatus(eventId, EventStateEnum.InProgress);
        }

        /// <summary>
        /// 创建集成事件日志 保存失败
        /// </summary>
        /// <param name="eventId">事件 id</param>
        /// <returns></returns>
        public Task MarkEventAsFailedAsync(string eventId)
        {
            return UpdateEventStatus(eventId, EventStateEnum.PublishedFailed);
        }

        private Task UpdateEventStatus(string eventId, EventStateEnum status)
        {
            var eventLogEntry = _integrationEventLogContext.IntegrationEventLogs.Single(ie => ie.EventId == eventId);
            eventLogEntry.State = status;

            if(status == EventStateEnum.InProgress)
                eventLogEntry.TimesSent++;
#if NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48
             var entry = this._integrationEventLogContext.Entry(eventLogEntry);
            //entry.CurrentValues.SetValues(entity);
            //更新时可能出现异常 bug 
            entry.State = EntityState.Modified;
            //如果数据没有发生变化
            if (!this._integrationEventLogContext.ChangeTracker.HasChanges())
            {
                return new Task(()=>{});
            }
            return  _integrationEventLogContext.SaveChangesAsync();
#else
            _integrationEventLogContext.IntegrationEventLogs.Update(eventLogEntry);
            return _integrationEventLogContext.SaveChangesAsync();
#endif
        }
    }
}
#endif