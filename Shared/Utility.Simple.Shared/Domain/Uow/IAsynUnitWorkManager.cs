#if !(NET20 || NET30 || NET35 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Domain.Uow
{
    /// <summary>
    /// 统一 管理
    /// </summary>
    public interface IAsynUnitWorkManager
    {
        /// <summary>
        /// 开始事务
        /// </summary>
        Task BeginAsyn();

        /// <summary>
        /// 提交事务
        /// </summary>
        Task CommitAsyn();

        /// <summary>
        /// 回滚事务
        /// </summary>
        Task RollbakAsyn();

        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <param name="entity"></param>
        Task InsertAsyn<Entity>(Entity entity) where Entity : class;


        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <param name="entities"></param>
        Task InsertBatchAsyn<Entity>(Entity[] entities) where Entity : class;

        /// <summary>
        /// 编辑
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <param name="entity"></param>
        Task UpdateAsyn<Entity>(Entity entity) where Entity : class;

        /// <summary>
        /// 编辑
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <param name="entities"></param>
        Task UpdateBatchAsyn<Entity>(Entity[] entities) where Entity : class;


        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <typeparam name="Key"></typeparam>
        /// <param name="id"></param>
        Task DeleteAsyn<Entity, Key>(Key id) where Entity : class;

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <typeparam name="Key"></typeparam>
        /// <param name="ids"></param>
        Task DeleteBatchAsyn<Entity, Key>(Key[] ids) where Entity : class;


        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <param name="entity"></param>
        Task DeleteAsyn<Entity>(Entity entity) where Entity : class;

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <param name="entities"></param>
        Task DeleteBatchAsyn<Entity>(Entity[] entities) where Entity : class;
    }
}
#endif