#if !(NET10 || NET11 || NET20 || NET30 || NET35 || NET40 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
//#if NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
#if NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48
using System.Data.Entity;
#else
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;
#endif
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Utility.Domain.Entities;
using Utility.Domain.Extensions;

namespace Utility.Infrastructure
{
    /// <summary>
    /// Domain Event 扩展类
    /// </summary>
    public static class DomainEventExtensions
    {   
        /// <summary>
        /// 手动赋值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="flag">1 add 2 update 3 delete</param>
        public  static bool UpdateValue<T>(T entity, int flag = 1) where T : class
        {
            return entity.UpdateValue(flag);
        }

        /// <summary>
        /// 基于 MediatR 事件 实现
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <param name="mediator"></param>
        /// <param name="entity"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static async Task DispatchDomainEventsAsync<Entity>(this IMediator mediator, Entity entity,int flag=1)
         where Entity : class, IDomainEvent
        {
            UpdateValue(entity, flag);
            var domainEntities = entity.DomainEvents != null && entity.DomainEvents.Any();

            var domainEvents = entity.DomainEvents.ToList();
        
            entity.ClearDomainEvents();

            //关键 双向订阅 发布 消息
            var tasks = domainEvents
                .Select(async (domainEvent) => {
                    await mediator.Publish(domainEvent);
                });

            await Task.WhenAll(tasks);
        }

        /// <summary>
        /// 基于 MediatR 事件 实现
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <param name="mediator"></param>
        /// <param name="entity"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static async Task DispatchDomainEventsAsyncV1<Entity>(this IMediator mediator, Entity entity,int flag=1) where Entity : class
        {
            UpdateValue(entity, flag);
            IDomainEvent  domain= ((IDomainEvent)entity);
            var domainEntities = domain.DomainEvents != null && domain.DomainEvents.Any();

            var domainEvents = domain.DomainEvents.ToList();

            domain.ClearDomainEvents();

            //关键 双向订阅 发布 消息
            var tasks = domainEvents
                .Select(async (domainEvent) => {
                    await mediator.Publish(domainEvent);
                });

            await Task.WhenAll(tasks);
        }

        /// <summary>
        /// 基于 MediatR 事件 实现
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <param name="mediator"></param>
        /// <param name="entities"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static async Task DispatchDomainEventsAsync<Entity>(this IMediator mediator, Entity[] entities,int flag=1)
        where Entity : class, IDomainEvent
        {
            foreach (var item in entities)
            {
                UpdateValue(item, flag);
            }
     
            var domainEntities = entities
               .Where(x => x.DomainEvents != null && x.DomainEvents.Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.DomainEvents)
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.ClearDomainEvents());

            //关键 双向订阅 发布 消息
            var tasks = domainEvents
                .Select(async (domainEvent) => {
                    await mediator.Publish(domainEvent);
                });

            await Task.WhenAll(tasks);
        }

        /// <summary>
        /// 基于 MediatR 事件 实现
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <param name="mediator"></param>
        /// <param name="entities"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static async Task DispatchDomainEventsAsyncV1<Entity>(this IMediator mediator, Entity[] entities, int flag = 1)
    where Entity : class
        {
            foreach (var item in entities)
            {
                UpdateValue(item, flag);
            }
            var domainEntities = entities.Select(it => (IDomainEvent)it)
               .Where(it => it.DomainEvents != null && it.DomainEvents.Any());

            var domainEvents = domainEntities
                .SelectMany(it => it.DomainEvents)
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.ClearDomainEvents());

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