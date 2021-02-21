using System;

namespace Utility.Model
{
    /// <summary>
    /// 是否选中
    /// </summary>
    /// <typeparam name="Key"></typeparam>
    public interface ISelectedModel<Key> : IModel<Key>
    {
        /// <summary>
        /// 是否选中
        /// </summary>
        bool IsSelected { get; set; }
    }
    /// <summary>
    /// 实体 是否选中
    /// </summary>
#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
    [Serializable]
#endif
    public class SelectModel<Key> :Model<Key>, ISelectedModel<Key>, IModel<Key>
    {

        private bool _isSelected;//是否选中

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsSelected { get { return this._isSelected; } set { Set(ref _isSelected, value, "IsSelected"); } }
    }
}
