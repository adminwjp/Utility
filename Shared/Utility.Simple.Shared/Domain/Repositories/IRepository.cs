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

namespace Utility.Domain.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="Entity"></typeparam>
    /// <typeparam name="Key"></typeparam>
    public interface IRepository<Entity, Key>: IRepository<Entity> where Entity : class, IEntity<Key>
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public interface IRepository
    {

    }
    /// <summary> 
    /// 通用泛型接口 ef 自动调用save 方法 dapper 无效 nhiberne无效 无效即未实现 
    /// <para>ef,nhibernate 支持 linq dapper 不支持linq 需要手动转换</para>
    /// <para>dapper CancellationToken 无效</para>
    /// <para>nhibernate CancellationToken 有效</para>
    /// </summary>
    /// <typeparam name="Entity"></typeparam>
    public interface IRepository<Entity>:IRepository,IDisposable where Entity : class
    {
#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
        /// <summary>数据库连接对象 </summary>
        IDbConnection Connection { get; }
#endif

#if !(NET20 || NET30)
        /// <summary>查找单个，且不被上下文所跟踪 ef,nhibernate 支持 linq dapper 不支持linq </summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
        Entity FindSingle(Expression<Func<Entity, bool>> where = null);

        /// <summary> 是否存在 默认实现 ef,nhibernate 支持 linq dapper 不支持linq  </summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
        bool IsExist(Expression<Func<Entity, bool>> where=null);

        /// <summary> 查询数据 默认实现 ef,nhibernate 支持 linq dapper 不支持linq dapper默认查询所有结果集基于内存 条件查询</summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
        IQueryable<Entity> Find(Expression<Func<Entity, bool>> where = null);

        /// <summary> 查询数据 默认实现 ef,nhibernate 支持 linq dapper 不支持linq orderby参数无效 dapper默认查询所有结果集基于内存 条件查询 </summary>
        /// <param name="page">页数</param>
        /// <param name="size">记录</param>
        /// <param name="where">条件</param>
        /// <returns></returns>
        IQueryable<Entity> FindByPage(Expression<Func<Entity, bool>> where = null,int page = 1, int size = 10);


        /// <summary>查询数量  默认实现 ef,nhibernate 支持 linq dapper 不支持linq  dapper默认查询所有结果数量  </summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
        int Count(Expression<Func<Entity, bool>> where = null);
#endif

        /// <summary> 添加 </summary>
        /// <param name="entity">实体</param>
        void Insert(Entity entity);

        /// <summary>批量 添加 </summary>
        /// <param name="entities">实体</param>
        void BatchInsert(Entity[] entities);

        /// <summary> 更新一个实体的所有属性 </summary>
        /// <param name="entity">实体</param>
        void Update(Entity entity);

        /// <summary> 删除</summary>
        /// <param name="entity">实体</param>
        void Delete(Entity entity);
#if !(NET20 || NET30)
        /// <summary>
        /// 实现按需要只更新部分更新 默认实现 ef,nhibernate 支持 linq dapper 不支持linq dapper 未实现
        /// <para>如：Update(u =>u.Id==1,u =>new User{Name="ok"});</para>
        /// </summary>
        /// <param name="where">更新条件</param>
        /// <param name="update">更新后的实体</param>
        void Update(Expression<Func<Entity, bool>> where, Expression<Func<Entity, Entity>> update);

        /// <summary> 批量删除 默认实现 ef,nhibernate 支持 linq dapper 不支持linq dapper 未实现</summary>
        /// <param name="where">条件</param>
        void Delete(Expression<Func<Entity, bool>> where);
#endif
        /// <summary> 执行sql </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        int ExecuteSql(string sql);

        ///// <summary> 操作成功 保存到库里 默认实现 ef 支持  dapper nhibernate 无任何操作 </summary>
        //void Save();

#region async
#if !(NET20 || NET30 || NET35)
        /// <summary> 查询数据  默认实现 ef,nhibernate 支持 linq dapper 不支持linq  </summary>
        /// <param name="where">条件</param>
        /// <param name="cancellationToken">dapper ef 无效</param>
        /// <returns></returns>
        Task<Entity> FindSingleAsync(Expression<Func<Entity, bool>> where = null,CancellationToken cancellationToken=default);

        /// <summary> 是否存在 默认实现 ef,nhibernate 支持 linq dapper 不支持linq  </summary>
        /// <param name="where">条件</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> IsExistAsync(Expression<Func<Entity, bool>> where=null, CancellationToken cancellationToken = default);

        /// <summary> 查询数据 默认实现 ef,nhibernate 支持 linq dapper 不支持linq ef 相当于普通方法 未实现async</summary>
        /// <param name="where">条件</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IQueryable<Entity>> FindAsync(Expression<Func<Entity, bool>> where = null, CancellationToken cancellationToken = default);

        /// <summary> 查询数据 默认实现 ef,nhibernate 支持 linq dapper 不支持linq orderby参数无效 </summary>
        /// <param name="page">页数</param>
        /// <param name="size">记录</param>
        /// <param name="where">条件</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IQueryable<Entity>> FindByPageAsync(Expression<Func<Entity, bool>> where = null, int page = 1, int size = 10,
             CancellationToken cancellationToken = default);

        /// <summary>查询数量  默认实现 ef,nhibernate 支持 linq dapper 不支持linq  dapper默认查询所有结果数量  </summary>
        /// <param name="where">条件</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> CountAsync(Expression<Func<Entity, bool>> where = null, CancellationToken cancellationToken = default);

        /// <summary> 添加 </summary>
        /// <param name="entity">实体</param>
        /// <param name="cancellationToken">ef core 有效 </param>
        Task InsertAsync(Entity entity, CancellationToken cancellationToken = default);

        /// <summary>批量 添加 </summary>
        /// <param name="entities">实体</param>
        /// <param name="cancellationToken"></param>
        Task BatchInsertAsync(Entity[] entities, CancellationToken cancellationToken = default);

        /// <summary> 更新一个实体的所有属性 </summary>
        /// <param name="entity">实体</param>
        /// <param name="cancellationToken">ef 有效</param>
        Task UpdateAsync(Entity entity, CancellationToken cancellationToken = default);

        /// <summary> 删除</summary>
        /// <param name="entity">实体</param>
       /// <param name="cancellationToken">ef 有效</param>
        Task DeleteAsync(Entity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// 实现按需要只更新部分更新 默认实现 ef,nhibernate 支持 linq dapper 不支持linq
        /// <para>如：Update(u =>u.Id==1,u =>new User{Name="ok"});</para>
        /// </summary>
        /// <param name="where">更新条件</param>
        /// <param name="update">更新后的实体</param>
        /// <param name="cancellationToken">ef 有效</param>
        Task UpdateAsync(Expression<Func<Entity, bool>> where, Expression<Func<Entity, Entity>> update, CancellationToken cancellationToken = default);

        /// <summary> 批量删除 默认实现 ef,nhibernate 支持 linq dapper 不支持linq</summary>
        /// <param name="where">条件</param>
        /// <param name="cancellationToken">ef 有效</param>
        Task DeleteAsync(Expression<Func<Entity, bool>> where=null, CancellationToken cancellationToken = default);
     

        /// <summary> 执行sql </summary>
        /// <param name="sql"></param>
        /// <param name="cancellationToken">ef 有效</param>
        /// <returns></returns>
        Task<int> ExecuteSqlAsync(string sql, CancellationToken cancellationToken = default);

        ///// <summary> 操作成功 保存到库里 默认实现 ef 支持  dapper nhibernate 不支持 </summary>
        //Task SaveAsync(CancellationToken cancellationToken = default(CancellationToken));
#endif
#endregion async
    }
}
