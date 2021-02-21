#if !(NET10 || NET11 || NET20 || NET30 || NET35 || NET40 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
//#if NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1

using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Domain.Entities
{
    /// <summary>
    /// MediatR Domain 事件 接口
    /// </summary>
    public interface IDomainEvent
    {
        /// <summary>
        /// 双向 订阅
        /// </summary>
        IReadOnlyCollection<INotification> DomainEvents { get; }

        /// <summary>
        /// 添加 双向 订阅
        /// </summary>
        /// <param name="eventItem"></param>
        void AddDomainEvent(INotification eventItem);

        /// <summary>
        /// 移除 双向 订阅
        /// </summary>
        /// <param name="eventItem"></param>
        void RemoveDomainEvent(INotification eventItem);

        /// <summary>
        /// 清空 双向 订阅
        /// </summary>
        void ClearDomainEvents();
    }

    /// <summary>
    ///  MediatR Domain 事件 接口
    /// </summary>
    /// <typeparam name="Key"></typeparam>
    public class DomainEvent<Key>:IDomainEvent{
	 
	     int? _requestedHashCode;
        /// <summary>
        /// 主键 Specified key was too long; max key length is 767 bytes
        /// </summary>
        public virtual Key Id { get; set; }
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

        /// <summary>
        /// 是否持久化
        /// </summary>
        /// <returns></returns>
        public virtual bool IsTransient()
        {
            return false;//return this.Id == default(Key);
        }

        /// <summary>
        /// 重写 Equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is DomainEvent<Key>))
                return false;

            if (Object.ReferenceEquals(this, obj))
                return true;

            if (this.GetType() != obj.GetType())
                return false;

            DomainEvent<Key> item = (DomainEvent<Key>)obj;

            if (item.IsTransient() || this.IsTransient())
                return false;
            else
                return false;//item.Id == this.Id;
        }

        /// <summary>
        /// 重写 GetHashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                if (!_requestedHashCode.HasValue)
                    _requestedHashCode = this.Id.GetHashCode() ^ 31; // XOR for random distribution (http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx)

                return _requestedHashCode.Value;
            }
            else
                return base.GetHashCode();

        }

        /// <summary>
        /// == 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==( DomainEvent<Key> left,  DomainEvent<Key> right)
        {
            if (Object.Equals(left, null))
                return (Object.Equals(right, null)) ? true : false;
            else
                return left.Equals(right);
        }

        /// <summary>
        /// !=
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=( DomainEvent<Key> left,  DomainEvent<Key> right)
        {
            return !(left == right);
        }
	 }
}
#endif