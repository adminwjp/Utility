using System;

namespace Utility.Model
{
    /// <summary>
    /// 实体 模型 通用接口
    /// </summary>
    public interface IModel<Key>
    {
        /// <summary>
        /// 主键
        /// </summary>
        Key Id { get; set; }

    }
    /// <summary>
    /// 实体 模型 通用接口 实现
    /// </summary>
#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
    [Serializable]
#endif
    public class Model<Key> : IModel<Key>
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

}
