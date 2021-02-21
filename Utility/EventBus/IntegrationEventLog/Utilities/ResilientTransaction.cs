#if !(NET10 || NET11 || NET20 || NET30 || NET35 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
//#if   NET40 ||NET45 || NET451 || NET452 || NET46 ||NET461 || NET462|| NET47 || NET471 || NET472|| NET48 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1

#if NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48
using System.Data.Entity;
#else
using Microsoft.EntityFrameworkCore;
#endif
using System;
using System.Threading.Tasks;

namespace Utility.IntegrationEventLog.Utilities
{
    /// <summary>
    /// DbContext 事务
    /// </summary>
    public class ResilientTransaction
    {
        private DbContext _context;
        private ResilientTransaction(DbContext context) =>
            _context = context ?? throw new ArgumentNullException(nameof(context));

        /// <summary>
        /// new object()
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static ResilientTransaction New (DbContext context) =>
            new ResilientTransaction(context);        

        /// <summary>
        /// 执行事务任务
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public
#if !NET40
            async
#endif
            Task ExecuteAsync(Func<Task> action)
        {
            //Use of an EF Core resiliency strategy when using multiple DbContexts within an explicit BeginTransaction():
            //See: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency
#if NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48
                       using (var transaction = _context.Database.BeginTransaction())
            {
#if NET40
                action().Wait();
                transaction.Commit();
                return new Task(()=>{});
#else
                await action();
                transaction.Commit();
#endif
            }
#else
            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    await action();
                    transaction.Commit();
                }
            });
#endif

        }
    }
}
#endif