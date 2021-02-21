using System;

namespace Utility.Model
{
    /// <summary>
    /// 删除 实体 模型
    /// </summary>
    /// <typeparam name="Key"></typeparam>
    public class IDeleteModel<Key>
    {
        /// <summary>
        ///  实体 id 集合
        /// </summary>
        Key[] Ids { get; set; }
    }
    /// <summary>
    /// 删除 实体 模型
    /// </summary>
    /// <typeparam name="Key"></typeparam>

#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
    [Serializable]
#endif
    public class DeleteModel<Key>: IDeleteModel<Key>
    {
        /// <summary>
        ///  实体 id 集合
        /// </summary>
        public Key[] Ids { get; set; }
    }
}
