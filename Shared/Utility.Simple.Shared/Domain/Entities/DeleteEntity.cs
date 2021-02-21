using System;

namespace Utility.Domain.Entities
{
    /// <summary>
    /// 删除 实体
    /// </summary>
    /// <typeparam name="Key"></typeparam>
    public interface IDeleteEntity<Key>
    {
        /// <summary>
        ///  实体 id 集合
        /// </summary>
        Key[] Ids { get; set; }
    }

    /// <summary>
    /// 删除 实体
    /// remote 不支持 泛型 需要包装
    /// </summary>
    /// <typeparam name="Key"></typeparam>
#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
    [Serializable]
#endif
    public class DeleteEntity<Key>:IDeleteEntity<Key>
    {
        /// <summary>
        ///  实体 id 集合
        /// </summary>
        public Key[] Ids { get; set; }
    }
}
