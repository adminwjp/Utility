using System;
using System.Collections.Generic;
using System.Text;

namespace Utility.Domain.Uow
{
    /// <summary>
    /// 统一 管理
    /// </summary>
    public interface IUnitWorkManager
    {
        /// <summary>
        /// 开始事务
        /// </summary>
        void Begin();

        /// <summary>
        /// 提交事务
        /// </summary>
        void Commit();
        
        /// <summary>
        /// 回滚事务
        /// </summary>
        void Rollbak();

        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <param name="entity"></param>
        void Insert<Entity>(Entity entity) where Entity : class;


        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <param name="entities"></param>
        void InsertBatch<Entity>(Entity[] entities) where Entity : class;

        /// <summary>
        /// 编辑
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <param name="entity"></param>
        void Update<Entity>(Entity entity) where Entity : class;

        /// <summary>
        /// 编辑
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <param name="entities"></param>
        void UpdateBatch<Entity>(Entity[] entities) where Entity : class;


         /// <summary>
         /// 删除
         /// </summary>
         /// <typeparam name="Entity"></typeparam>
         /// <typeparam name="Key"></typeparam>
         /// <param name="id"></param>
        void Delete<Entity,Key>(Key id) where Entity : class;

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <typeparam name="Key"></typeparam>
        /// <param name="ids"></param>
        void DeleteBatch<Entity, Key>(Key[] ids) where Entity : class;


        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <param name="entity"></param>
        void Delete<Entity>(Entity entity) where Entity : class;

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <param name="entities"></param>
        void DeleteBatch<Entity>(Entity[] entities) where Entity : class;
    }
}
