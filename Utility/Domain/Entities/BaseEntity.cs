#if !(NET10 || NET11 || NET20 || NET30 || NET35 || NET40 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
using MediatR;
#endif
using System;
using System.Collections.Generic;
using Utility.Domain.Entities;

namespace Utility.Domain.Entities
{
    /// <summary>
    /// 基类 模型
    /// </summary>

    public class BaseEntity: BaseEntity<string>, IHasCreationTime,IHasModificationTime,IHasDeletionTime
        #if !(NET10 || NET11 || NET20 || NET30 || NET35 || NET40 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)

        , IDomainEvent
#endif
    {
        /// <summary>
        /// 是否 持久化
        /// </summary>
        /// <returns></returns>
        public override bool IsTransient()
        {
            return this.Id==default(string);
        }
        /// <summary>
        /// 重写 equal
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return base.Equals(obj)?true: this.Id==((BaseEntity)obj).Id;
        }
        /// <summary>
        /// 重写 hashcode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
#if !(NET10 || NET11 || NET20 || NET30 || NET35 || NET40 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)

        private List<INotification> _domainEvents;
        /// <summary>
        /// 双向 订阅
        /// </summary>
        public virtual IReadOnlyCollection<INotification> DomainEvents => _domainEvents?.AsReadOnly();


        /// <summary>
        /// 添加 双向 订阅
        /// </summary>
        /// <param name="eventItem"></param>
        public virtual void AddDomainEvent(INotification eventItem)
        {
            _domainEvents = _domainEvents ?? new List<INotification>();
            _domainEvents.Add(eventItem);
        }

        /// <summary>
        /// 移除 双向 订阅
        /// </summary>
        /// <param name="eventItem"></param>
        public virtual void RemoveDomainEvent(INotification eventItem)
        {
            _domainEvents?.Remove(eventItem);
        }

        /// <summary>
        /// 清空 双向 订阅
        /// </summary>
        public virtual void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }
#endif
    }
}
