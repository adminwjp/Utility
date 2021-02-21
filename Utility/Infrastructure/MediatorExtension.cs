#if  NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1

using MediatR;
using System.Linq;
using System.Threading.Tasks;
using Utility.Domain.Entities;

namespace Utility.Infrastructure
{
    /// <summary>
    /// Mediator 组合 ef 双向 订阅  实现
    /// </summary>
    static class MediatorExtension
    {
        public static async Task DispatchDomainEventsAsync<DbContext,Entity>(this IMediator mediator, BaseDbContext<DbContext,Entity> ctx)
            where DbContext:BaseDbContext<DbContext, Entity>
            where Entity: class,IDomainEvent
        {
            var domainEntities = ctx.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.ClearDomainEvents());

            //关键 双向订阅 发布 消息
            var tasks = domainEvents
                .Select(async (domainEvent) => {
                    await mediator.Publish(domainEvent);
                });

            await Task.WhenAll(tasks);
        }
    }
}
#endif