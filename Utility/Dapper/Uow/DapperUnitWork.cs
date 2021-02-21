#if !(NET10 || NET11 || NET20 || NET30 || NET35 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
//#if NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Utility.Domain.Entities;
using Utility.Domain.Extensions;
using Utility.Domain.Uow;
using Utility.Extensions;


namespace Utility.Dapper.Uow
{
    /// <summary>dapper linq 不支持 需要自己转换 </summary>

    public class DapperUnitWork :IUnitWork
    {
        /// <summary>
        /// 
        /// </summary>
        public DapperTemplate DapperTemplate { get;protected set; }
        /// <summary> 构造注册数据库连接对象</summary>
        /// <param name="connection">数据库连接对象</param>
        public DapperUnitWork(IDbConnection connection) 
        {
            this.Connection = connection;
            this.DapperTemplate = new DapperTemplate(connection);
            this.Thorw = false;
        }
        /// <summary>未实现是抛出异常还是不做任何操作 </summary>
        public bool Thorw { get; }
       

        /// <summary>
        /// 
        /// </summary>

        public IDbConnection Connection { get; protected set; }

        /// <summary>查找单个，且不被上下文所跟踪 ef,nhibernate 支持 linq dapper 不支持linq </summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public virtual T FindSingle<T>(Expression<Func<T, bool>> where=null) where T : class
        {
            return Find(where).FirstOrDefault();
        }

        /// <summary> 是否存在 默认实现 ef,nhibernate 支持 linq dapper 不支持linq  </summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public virtual bool IsExist<T>(Expression<Func<T, bool>> where=null) where T : class
        {
            return Count(where) > 0;
        }

        /// <summary> 查询数据 默认实现 ef,nhibernate 支持 linq dapper 不支持linq dapper默认查询所有结果集基于内存 条件查询</summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public virtual IQueryable<T> Find<T>(Expression<Func<T, bool>> where = null) where T : class
        {
            where = where.Filter();
            return DapperTemplate.FindList<T>().AsQueryable().Where(where);
        }

        /// <summary> 查询数据 默认实现 ef,nhibernate 支持 linq dapper 不支持linq orderby参数无效 dapper默认查询所有结果集基于内存 条件查询 </summary>
        /// <param name="page">页数</param>
        /// <param name="size">记录</param>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public virtual IQueryable<T> FindByPage<T>(Expression<Func<T, bool>> where = null,int page = 1, int size = 10) where T : class
        {
            where = where.Filter();
            return DapperTemplate.FindListByPage<T>("", "",null,page, size).AsQueryable().Where(where);
        }

        /// <summary>查询数量  默认实现 ef,nhibernate 支持 linq dapper 不支持linq  dapper默认查询所有结果数量  </summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public virtual int Count<T>(Expression<Func<T, bool>> where = null) where T : class
        {
            where=where.Filter();
            return DapperTemplate.Count<T>("", null);
        }

        /// <summary> 添加 </summary>
        /// <param name="entity">实体</param>
        public virtual  object Insert<T>(T entity) where T : class
        {
            UpdateValue(entity);
            return DapperTemplate.Insert(entity);
        }

        /// <summary>
        /// 手动赋值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="flag">1 add 2 update 3 delete</param>
        protected virtual bool UpdateValue<T>(T entity,int flag=1) where T : class
        {
           return entity.UpdateValue(flag);
        }

        /// <summary>批量 添加 </summary>
        /// <param name="entities">实体</param>
        public virtual void BatchInsert<T>(T[] entities) where T : class
        {
            IDbTransaction transaction = Connection.BeginTransaction();
            foreach (T entity in entities)
            {
                UpdateValue(entity);
                Connection.Insert<T>(entity, transaction);
            }
            transaction.Commit();
        }

        /// <summary> 更新一个实体的所有属性 </summary>
        /// <param name="entity">实体</param>
        public virtual void Update<T>(T entity) where T : class
        {
            UpdateValue(entity, 2);
            DapperTemplate.Update(entity);
        }

        /// <summary> 删除</summary>
        /// <param name="entity">实体</param>
        public virtual void Delete<T>(T entity) where T : class
        {
            if (UpdateValue(entity, 3))
            {
                DapperTemplate.Update(entity);
            }
            else {
                DapperTemplate.Delete(entity);
            }
        }
        /// <summary> 批量删除 默认实现  nhibernate 支持 EF dapper 未实现</summary>
        public virtual void Delete<T>(object id) where T : class
        {
            if (typeof(T).IsAssignableFrom(typeof(IHasDeletionTime)))
            {
                T obj = Connection.Get<T>(id);
                if(obj is IHasDeletionTime deletionTime)
                {
                    deletionTime.DeletionTime = DateTime.Now;
                    deletionTime.IsDeleted = true;
                }
                DapperTemplate.Update(obj);
            }
            else
            {
                DapperTemplate.Delete<T>(id);
            }
           
        }
        /// <summary>
        /// 实现按需要只更新部分更新 默认实现 ef,nhibernate 支持 linq dapper 不支持linq dapper 未实现
        /// <para>如：Update(u =>u.Id==1,u =>new User{Name="ok"});</para>
        /// </summary>
        /// <param name="where">更新条件</param>
        /// <param name="update">更新后的实体</param>
        public virtual void Update<T>(Expression<Func<T, bool>> where, Expression<Func<T, T>> update) where T : class
        {
            if (this.Thorw)
            {
                throw new NotImplementedException();
            }
        }

        /// <summary> 批量删除 默认实现 ef,nhibernate 支持 linq dapper 不支持linq dapper 未实现</summary>
        /// <param name="where">条件</param>
        public virtual void Delete<T>(Expression<Func<T, bool>> where) where T : class
        {
            if (this.Thorw) throw new NotImplementedException();
        }

        /// <summary> 执行sql </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public  virtual int ExecuteSql(string sql)
        {
            return DapperTemplate.Execute(sql);
        }


        /// <summary> 操作成功 保存到库里 默认实现 ef 支持  dapper nhibernate 不支持 </summary>
        public virtual void Save()
        {

        }



#region async

        /// <summary>查找单个，且不被上下文所跟踪 ef,nhibernate 支持 linq dapper 不支持linq </summary>
        /// <param name="where">条件</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task<T> FindSingleAsync<T>(Expression<Func<T, bool>> where, CancellationToken cancellationToken = default) where T : class
        {
            return new Task<T>(()=>FindSingle(where));
        }

        /// <summary> 是否存在 默认实现 ef,nhibernate 支持 linq dapper 不支持linq  </summary>
        /// <param name="where">条件</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual
#if !NET40
        async
#endif
        Task<bool> IsExistAsync<T>(Expression<Func<T, bool>> where, CancellationToken cancellationToken = default) where T : class
        {
#if !NET40
            var res = await CountAsync(where, cancellationToken);
            return res > 0;
#else
             var res = Count(where);
            return new Task<bool>(()=>res > 0);
#endif

        }

        /// <summary> 查询数据 默认实现 ef,nhibernate 支持 linq dapper 不支持linq dapper默认查询所有结果集基于内存 条件查询</summary>
        /// <param name="where">条件</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual
#if !NET40
        async
#endif
        Task<IQueryable<T>> FindAsync<T>(Expression<Func<T, bool>> where = null, CancellationToken cancellationToken = default) where T : class
        {
            where = where.Filter();
#if !NET40
            var res = await DapperTemplate.FindListAsync<T>();
            return res.AsQueryable().Where(where);
#else
            var res =  DapperTemplate.FindList<T>();
            return new Task<IQueryable<T>>(()=>res.AsQueryable().Where(where));
#endif
        }

        /// <summary> 查询数据 默认实现 ef,nhibernate 支持 linq dapper 不支持linq orderby参数无效 dapper默认查询所有结果集基于内存 条件查询 </summary>
        /// <param name="page">页数</param>
        /// <param name="size">记录</param>
        /// <param name="where">条件</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual 
#if !NET40
        async
#endif
        Task<IQueryable<T>> FindByPageAsync<T>(Expression<Func<T, bool>> where = null, int page = 1, int size = 10, CancellationToken cancellationToken = default) where T : class
        {
            where = where.Filter();
#if !NET40
            var res = await DapperTemplate.FindListByPageAsync<T>("","",null,page, size);
            return res.AsQueryable().Where(where);
#else
            var res =  DapperTemplate.FindListByPage<T>("","",null,page, size);
            return new Task<IQueryable<T>>(()=>res.AsQueryable().Where(where));
#endif
        }

        /// <summary>查询数量  默认实现 ef,nhibernate 支持 linq dapper 不支持linq  dapper默认查询所有结果数量  </summary>
        /// <param name="where">条件</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task<int> CountAsync<T>(Expression<Func<T, bool>> where = null, CancellationToken cancellationToken = default) where T : class
        {
            where = where.Filter();
            return DapperTemplate.CountAsync<T>("", null);
        }

        /// <summary> 添加 </summary>
        /// <param name="entity">实体</param>
        /// <param name="cancellationToken"></param>
        public virtual Task InsertAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class
        {
            UpdateValue(entity);
            return DapperTemplate.InsertAsync(entity);
        }

        /// <summary>批量 添加 </summary>
        /// <param name="entities">实体</param>
        ///         /// <param name="cancellationToken"></param>
        public virtual 
#if !NET40
        async
#endif
Task BatchInsertAsync<T>(T[] entities, CancellationToken cancellationToken = default) where T : class
        {
#if !NET40
            IDbTransaction transaction = Connection.BeginTransaction();
            foreach (T entity in entities)
            {
                UpdateValue(entity);
                await Connection.InsertAsync<T>(entity, transaction);
            }
            transaction.Commit();
#else
            BatchInsert(entities);
            return Utility.Threads.TaskHelper.CompletedTask;
#endif
        }

        /// <summary> 更新一个实体的所有属性 </summary>
        /// <param name="entity">实体</param>
        /// <param name="cancellationToken"></param>
        public virtual Task UpdateAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class
        {
            UpdateValue(entity,2);
            return DapperTemplate.UpdateAsync(entity);
        }

        /// <summary> 删除</summary>
        /// <param name="entity">实体</param>
        /// <param name="cancellationToken"></param>
        public virtual Task DeleteAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class
        {
            UpdateValue(entity,3);
            return DapperTemplate.UpdateAsync(entity);
            //return DapperTemplate.DeleteAsync(entity);
        }

        /// <summary> 批量删除 默认实现  nhibernate 支持 EF dapper 未实现</summary>
        ///<param name="id"></param>
        /// <param name="cancellationToken"></param>
        public virtual Task DeleteAsync<T>(object id, CancellationToken cancellationToken = default) where T : class
        {
            return new Task(() => Delete<T>(id));
        }
        /// <summary>
        /// 实现按需要只更新部分更新 默认实现 ef,nhibernate 支持 linq dapper 不支持linq dapper 未实现
        /// <para>如：Update(u =>u.Id==1,u =>new User{Name="ok"});</para>
        /// </summary>
        /// <param name="where">更新条件</param>
        /// <param name="update">更新后的实体</param>
        /// <param name="cancellationToken"></param>
        public virtual Task UpdateAsync<T>(Expression<Func<T, bool>> where, Expression<Func<T, T>> update, CancellationToken cancellationToken = default) where T : class
        {
            return new Task(() => Update(where, update));
        }

        /// <summary> 批量删除 默认实现 ef,nhibernate 支持 linq dapper 不支持linq dapper 未实现</summary>
        /// <param name="cancellationToken">dapper 无效</param>
        /// <param name="where">条件</param>
        public virtual Task DeleteAsync<T>(Expression<Func<T, bool>> where, CancellationToken cancellationToken = default) where T : class
        {
            return new Task(() => Delete(where));
        }

        /// <summary> 执行sql </summary>
        /// <param name="sql"></param>
        /// <param name="cancellationToken">dapper 无效</param>
        /// <returns></returns>
        public virtual Task<int> ExecuteSqlAsync(string sql, CancellationToken cancellationToken = default)
        {
            return DapperTemplate.ExecuteAsync(sql);
        }


        /// <summary> 操作成功 保存到库里 默认实现 ef 支持  dapper nhibernate 不支持 </summary>
        /// <param name="cancellationToken"></param>
        public virtual Task SaveAsync(CancellationToken cancellationToken = default) 
        {
            return new Task(()=> { });
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public  virtual void Dispose()
        {
            Connection.Dispose();
        }
#endregion async
    }
}
#endif