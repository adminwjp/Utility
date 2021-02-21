using System;

namespace Utility.Domain.Entities
{
    /// <summary>
    /// 基类 实体
    /// </summary>
    public class BaseEntity<Key>:IEntity<Key>, IHasCreationTime,IHasModificationTime,IHasDeletionTime
    {
        int? _requestedHashCode;
        /// <summary>
        /// 主键 Specified key was too long; max key length is 767 bytes
        /// </summary>
        public virtual Key Id { get; set; }
        /// <summary>
        /// 创建 时间 datetime(6) mysql 5.5 不支持 ; datetime mysql 5.5 支持
        /// </summary>
        public virtual DateTime CreationTime { get; set; }
        /// <summary>
        /// 修改 时间
        /// </summary>
        public virtual DateTime? LastModificationTime { get; set; }
        /// <summary>
        /// 删除时间
        /// </summary>
        public virtual DateTime? DeletionTime { get; set; }
        /// <summary>
        /// 软删除 标识
        /// </summary>
        public virtual bool IsDeleted { get; set; }

      
        /// <summary>
        /// 是否 持久化 
        /// </summary>
        /// <returns></returns>
        public virtual bool IsTransient()
        {
            return false;//this.Id == default(string);
        }

       /// <summary>
       /// 是否匹配 
       /// </summary>
       /// <param name="obj"></param>
       /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is BaseEntity<Key>))
                return false;

            if (Object.ReferenceEquals(this, obj))
                return true;

            if (this.GetType() != obj.GetType())
                return false;

            BaseEntity<Key> item = (BaseEntity<Key>)obj;

            if (item.IsTransient() || this.IsTransient())
                return false;
            else
                return false;//item.Id == this.Id;
        }

        /// <summary>
        /// 重写 hashcode
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
        /// == 实现
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(BaseEntity<Key> left, BaseEntity<Key> right)
        {
            if (Object.Equals(left, null))
                return (Object.Equals(right, null)) ? true : false;
            else
                return left.Equals(right);
        }

        /// <summary>
        /// != 实现
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(BaseEntity<Key> left, BaseEntity<Key> right)
        {
            return !(left == right);
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <param name="entity"></param>
        public virtual void Update<Entity>(Entity entity)
          where Entity : BaseEntity<Key>
        {
            this.Id = entity.Id;
            this.CreationTime = entity.CreationTime;
            this.LastModificationTime = entity.LastModificationTime;
            this.DeletionTime = entity.DeletionTime;
            this.IsDeleted = entity.IsDeleted;
        }
    }
}
