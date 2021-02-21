using System;

namespace Utility.Domain.Entities
{
    /// <summary>
    /// 实体
    /// </summary>
    public interface IEntity<Key>
    {
        /// <summary>
        /// 主键
        /// </summary>
        Key Id { get; set; }

    }
    /// <summary>
    /// 实体
    /// remote 不支持 泛型 需要包装
    /// </summary>
#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
    [Serializable]
#endif
    public class Entity<Key>:IEntity<Key>
    {
        private Key _id;//主键

        /// <summary>
        /// 主键
        /// </summary>
        public virtual Key Id { get { return this._id; } set { Set(ref _id, value, "Id"); } }

        /// <summary>
        /// 设置属性值 wpf 使用时直接继承 viewModel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="oldValue">旧值</param>
        /// <param name="newValue">新值</param>
        /// <param name="propertyName">属性名称 wpf 有效</param>

        protected virtual void Set<T>(ref T oldValue, T newValue, string propertyName)
        {
            oldValue = newValue;
        }

    }

    /// <summary>
    /// 键值
    /// <typeparamref name="K"/> 键 类型
    /// <typeparamref name="K"/> 值 类型
    /// </summary>
#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
    [Serializable]
#endif
    public class Entity<K, V>
    {
        /// <summary>
        /// 键
        /// </summary>
        public K Key { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public V Value { get; set; }
    }

    /// <summary>
    ///string  键值
    /// </summary>
#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
    [Serializable]
#endif
    public class StringEntity:Entity<string,string>
    {
        
    }
}
