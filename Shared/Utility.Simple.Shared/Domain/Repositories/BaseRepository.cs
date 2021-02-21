using System;
#if !( NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 ||  NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
using System.Data;
#endif
#if !(NET20 || NET30 || NET35)
using System.Threading.Tasks;
#endif
#if !(NET20 || NET30)
using System.Linq;
using System.Linq.Expressions;
#endif
using System.Threading;
using Utility.Domain.Entities;
using Utility.Domain.Uow;


namespace Utility.Domain.Repositories
{
    /// <summary>
    /// 工作创建基类
    /// </summary>
    /// <typeparam name="Entity"></typeparam>
    public abstract class BaseRepository<Entity> : IRepository<Entity> where Entity : class
    {
        /// <summary>
        /// 工作单元
        /// </summary>
        protected IUnitWork UnitWork { get; set; }
        /// <summary>未实现是抛出异常还是不做任何操作 </summary>
        bool Thorw { get; }
#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
        /// <summary>数据库连接对象 </summary>
        public IDbConnection Connection { get {
                return UnitWork?.Connection;
            }
        }
#endif


#if !(NET20 || NET30)
        /// <summary>查找单个，且不被上下文所跟踪 ef,nhibernate 支持 linq dapper 不支持linq </summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public virtual Entity FindSingle(Expression<Func<Entity, bool>> where = null)
        {
            return UnitWork.FindSingle(where);
        }

        /// <summary> 是否存在 默认实现 ef,nhibernate 支持 linq dapper 不支持linq  </summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public virtual bool IsExist(Expression<Func<Entity, bool>> where=null)
        {
            return UnitWork.IsExist(where);
        }

        /// <summary> 查询数据 默认实现 ef,nhibernate 支持 linq dapper 不支持linq dapper默认查询所有结果集基于内存 条件查询</summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public virtual IQueryable<Entity> Find(Expression<Func<Entity, bool>> where = null)
        {
            return UnitWork.Find(where);
        }

        /// <summary> 查询数据 默认实现 ef,nhibernate 支持 linq dapper 不支持linq orderby参数无效 dapper默认查询所有结果集基于内存 条件查询 </summary>
        /// <param name="page">页数</param>
        /// <param name="size">记录</param>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public virtual  IQueryable<Entity> FindByPage(Expression<Func<Entity, bool>> where = null,int page = 1, int size = 10)
        {
            return UnitWork.FindByPage(where,page,size);
        }


        /// <summary>查询数量  默认实现 ef,nhibernate 支持 linq dapper 不支持linq  dapper默认查询所有结果数量  </summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
       public virtual int Count(Expression<Func<Entity, bool>> where = null)
       {
            return UnitWork.Count(where);
        }
#endif

        /// <summary> 添加 </summary>
        /// <param name="entity">实体</param>
        public virtual void Insert(Entity entity)
        {
             UnitWork.Insert(entity);
        }

        /// <summary>批量 添加 </summary>
        /// <param name="entities">实体</param>
        public virtual void BatchInsert(Entity[] entities)
        {
            UnitWork.BatchInsert(entities);
        }

        /// <summary> 更新一个实体的所有属性 </summary>
        /// <param name="entity">实体</param>
        public virtual void Update(Entity entity)
        {
            UnitWork.Update(entity);
        }

        /// <summary> 删除</summary>
        /// <param name="entity">实体</param>
        public virtual void Delete(Entity entity)
        {
            UnitWork.Delete(entity);
        }


#if !(NET20 || NET30)
#if !(NET10 || NET20 || NET30 || NET35)
        /// <summary> 删除</summary>
        /// <param name="id">实体 id</param>
        public virtual void Delete<Key>(Key id)
        {
            UnitWork.Delete(Utility.Extensions.LinqExpression.IdEqual<Entity, Key>(id));
        }
#endif
        /// <summary>
        /// 实现按需要只更新部分更新 默认实现 ef,nhibernate 支持 linq dapper 不支持linq dapper 未实现
        /// <para>如：Update(u =>u.Id==1,u =>new User{Name="ok"});</para>
        /// </summary>
        /// <param name="where">更新条件</param>
        /// <param name="update">更新后的实体</param>
        public virtual void Update(Expression<Func<Entity, bool>> where, Expression<Func<Entity, Entity>> update)
        {
            UnitWork.Update(where,update);
        }

        /// <summary> 批量删除 默认实现 ef,nhibernate 支持 linq dapper 不支持linq dapper 未实现</summary>
        /// <param name="where">条件</param>
        public virtual void Delete(Expression<Func<Entity, bool>> where)
        {
            UnitWork.Delete(where);
        }
#endif
        /// <summary> 执行sql </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public virtual int ExecuteSql(string sql)
        {
          return  UnitWork.ExecuteSql(sql);
        }

        ///// <summary> 操作成功 保存到库里 默认实现 ef 支持  dapper nhibernate 无任何操作 </summary>
        //void Save();

        #region async
#if !(NET20 || NET30 || NET35)
        /// <summary> 查询数据  默认实现 ef,nhibernate 支持 linq dapper 不支持linq  </summary>
        /// <param name="where">条件</param>
        /// <param name="cancellationToken">dapper ef 无效</param>
        /// <returns></returns>
        public virtual Task<Entity> FindSingleAsync(Expression<Func<Entity, bool>> where = null,CancellationToken cancellationToken=default)
        {
            return UnitWork.FindSingleAsync(where,cancellationToken);
        }

        /// <summary> 是否存在 默认实现 ef,nhibernate 支持 linq dapper 不支持linq  </summary>
        /// <param name="where">条件</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task<bool> IsExistAsync(Expression<Func<Entity, bool>> where=null, CancellationToken cancellationToken = default)
        {
            return UnitWork.IsExistAsync(where, cancellationToken);
        }

        /// <summary> 查询数据 默认实现 ef,nhibernate 支持 linq dapper 不支持linq ef 相当于普通方法 未实现async</summary>
        /// <param name="where">条件</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task<IQueryable<Entity>> FindAsync(Expression<Func<Entity, bool>> where = null, CancellationToken cancellationToken = default)
        {
            return UnitWork.FindAsync(where, cancellationToken);
        }

        /// <summary> 查询数据 默认实现 ef,nhibernate 支持 linq dapper 不支持linq orderby参数无效 </summary>
        /// <param name="page">页数</param>
        /// <param name="size">记录</param>
        /// <param name="where">条件</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task<IQueryable<Entity>> FindByPageAsync(Expression<Func<Entity, bool>> where = null, int page = 1, int size = 10,
             CancellationToken cancellationToken = default)
        {
            return UnitWork.FindByPageAsync(where,page,size, cancellationToken);
        }

        /// <summary>查询数量  默认实现 ef,nhibernate 支持 linq dapper 不支持linq  dapper默认查询所有结果数量  </summary>
        /// <param name="where">条件</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task<int> CountAsync(Expression<Func<Entity, bool>> where = null, CancellationToken cancellationToken = default)
        {
            return UnitWork.CountAsync(where, cancellationToken);
        }

        /// <summary> 添加 </summary>
        /// <param name="entity">实体</param>
        /// <param name="cancellationToken">ef core 有效 </param>
        public virtual Task InsertAsync(Entity entity, CancellationToken cancellationToken = default)
        {
            return UnitWork.InsertAsync(entity, cancellationToken);
        }

        /// <summary>批量 添加 </summary>
        /// <param name="entities">实体</param>
        /// <param name="cancellationToken"></param>
        public virtual Task BatchInsertAsync(Entity[] entities, CancellationToken cancellationToken = default)
        {
            return UnitWork.BatchInsertAsync(entities, cancellationToken);
        }

        /// <summary> 更新一个实体的所有属性 </summary>
        /// <param name="entity">实体</param>
        /// <param name="cancellationToken">ef 有效</param>
        public virtual Task UpdateAsync(Entity entity, CancellationToken cancellationToken = default)
        {
            return UnitWork.UpdateAsync(entity, cancellationToken);
        }

        /// <summary> 删除</summary>
        /// <param name="entity">实体</param>
        /// <param name="cancellationToken">ef 有效</param>
        public virtual Task DeleteAsync(Entity entity, CancellationToken cancellationToken = default)
        {
            return UnitWork.DeleteAsync(entity, cancellationToken);
        }

        /// <summary>
        /// 实现按需要只更新部分更新 默认实现 ef,nhibernate 支持 linq dapper 不支持linq
        /// <para>如：Update(u =>u.Id==1,u =>new User{Name="ok"});</para>
        /// </summary>
        /// <param name="where">更新条件</param>
        /// <param name="update">更新后的实体</param>
        /// <param name="cancellationToken">ef 有效</param>
        public virtual Task UpdateAsync(Expression<Func<Entity, bool>> where, Expression<Func<Entity, Entity>> update, CancellationToken cancellationToken = default)
        {
            return UnitWork.UpdateAsync(where, update, cancellationToken);
        }

        /// <summary> 批量删除 默认实现 ef,nhibernate 支持 linq dapper 不支持linq</summary>
        /// <param name="where">条件</param>
        /// <param name="cancellationToken">ef 有效</param>
        public virtual Task DeleteAsync(Expression<Func<Entity, bool>> where=null, CancellationToken cancellationToken = default)
        {
            return UnitWork.DeleteAsync(where, cancellationToken);
        }


        /// <summary> 执行sql </summary>
        /// <param name="sql"></param>
        /// <param name="cancellationToken">ef 有效</param>
        /// <returns></returns>
        public virtual Task<int> ExecuteSqlAsync(string sql, CancellationToken cancellationToken = default)
        {
            return UnitWork.ExecuteSqlAsync(sql, cancellationToken);
        }


        ///// <summary> 操作成功 保存到库里 默认实现 ef 支持  dapper nhibernate 不支持 </summary>
        //Task SaveAsync(CancellationToken cancellationToken = default(CancellationToken));
#endif
        #endregion async

        /// <summary>
        /// 释放资源
        /// </summary>
        public virtual void Dispose()
        {
            UnitWork.Dispose();
        }

    }
}
