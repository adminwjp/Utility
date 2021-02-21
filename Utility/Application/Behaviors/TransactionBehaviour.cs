#if NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0|| NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1

using MediatR;
#if NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using Microsoft.EntityFrameworkCore;
#endif
#if NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48
using System.Data.Entity;
#endif
using Serilog.Context;
using System;
using System.Threading;
using System.Threading.Tasks;
using Utility.Application.IntegrationEvents;
using Utility.Domain.Entities;
using Utility.EventBus.Extensions;
using Utility.Infrastructure;
using Utility.Logs;

namespace Utility.Application.Behaviors
{
    /// <summary>
    ///MediatR 订阅 事务  消息中间件 
    /// </summary>
    /// <typeparam name="Context"></typeparam>
    /// <typeparam name="Entity"></typeparam>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public class TransactionBehaviour<Context, Entity, TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
         where Context : BaseDbContext<Context, Entity>
        where Entity : class, IDomainEvent
    {
        private readonly ILog<TransactionBehaviour<Context, Entity, TRequest, TResponse>> logger;
        private readonly Context dbContext;
        private readonly IIntegrationEventService integrationEventService;

        /// <summary>
        /// MediatR 订阅 事务  消息中间件 
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="integrationEventService"></param>
        /// <param name="logger"></param>
        public TransactionBehaviour(Context dbContext,
            IIntegrationEventService integrationEventService,
            ILog<TransactionBehaviour<Context, Entity, TRequest, TResponse>> logger)
        {
            this.dbContext = dbContext ?? throw new ArgumentException(nameof(Context));
            this.integrationEventService = integrationEventService ?? throw new ArgumentException(nameof(integrationEventService));
            this.logger = logger ?? throw new ArgumentException(nameof(ILog));
        }

        /// <summary>
        /// 事务处理
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public virtual async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var response = default(TResponse);
            var typeName = request.GetGenericTypeName();

            try
            {
           
                if (dbContext.HasActiveTransaction)
                {
                    return await next();
                }
#if NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
                var strategy = dbContext.Database.CreateExecutionStrategy();
                
                await strategy.ExecuteAsync(async () =>
                {
                    using (var transaction = await dbContext.BeginTransactionAsync())
                    using (LogContext.PushProperty("TransactionContext", transaction.TransactionId))
                    {
                        logger.Log(LogLevel.Info, $"----- Begin transaction {transaction.TransactionId} for {typeName} ({request})");

                        response = await next();

                        logger.Log(LogLevel.Info,$"----- Commit transaction {transaction.TransactionId} for {typeName}");

                        await dbContext.CommitTransactionAsync(transaction);
                    }

                    await integrationEventService.PublishEventsThroughEventBusAsync();
                });

                return response;
#else
                //没有 这种 实现 需要 自己实现 support
				using (var transaction = await dbContext.BeginTransactionAsync())
                    using (LogContext.PushProperty("TransactionContext", transaction))
                    {
                        logger.Log(LogLevel.Info, $"----- Begin transaction {transaction} for {typeName} ({request})");

                        response = await next();

                        logger.Log(LogLevel.Info,$"----- Commit transaction {transaction} for {typeName}");

                        await dbContext.CommitTransactionAsync(transaction);
                    }

                    await integrationEventService.PublishEventsThroughEventBusAsync();
                return response;
#endif
            }
            catch (Exception ex)
            {
                logger.LogException(LogLevel.Error, $"ERROR Handling transaction for {typeName} ({request})", ex);

                throw;
            }
        }
    }
}
#endif