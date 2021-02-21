using System;

namespace Utility.Domain.Entities
{
    /// <summary>
    /// 是否选中
    /// </summary>
    public interface ISelectedEntity<Key>
    {
        /// <summary>
        /// 是否选中
        /// </summary>
        bool IsSelected { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        Key Id { get; set; }
    }
    /// <summary>
    /// 实体 是否选中 
    /// remote 不支持 泛型 需要包装
    /// </summary>
#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
    [Serializable]
#endif
    public class SelectEdEntity<Key> : Entity<Key>, ISelectedEntity<Key>
    {

        private bool _isSelected;//是否选中

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsSelected { get { return this._isSelected; } set { Set(ref _isSelected, value, "IsSelected"); } }
    }
}
