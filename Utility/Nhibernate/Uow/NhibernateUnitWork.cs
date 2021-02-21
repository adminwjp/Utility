#if !(NET10 || NET11 || NET20 || NET30 || NET35 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
//#if NET40 ||NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NET481 || NET482 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NETCOREAPP3_2 || NETSTANDARD2_0 || NETSTANDARD2_1
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using Utility.Application.Services.Dtos;
using Utility.Domain.Entities;
using Utility.Domain.Extensions;
using Utility.Domain.Uow;
using Utility.Threads;

namespace Utility.Nhibernate.Uow
{
    /// <summary>
    /// 如果 多线程每次执行 Save怎么确定提交 (怎么追踪每次的任务)
    /// </summary>
    public class NhibernateUnitWork : IUnitWork
    {
        /// <summary>
        /// 
        /// </summary>
        public  NHibernate.ISession Session;

        /// <summary>
        /// NhibernateTemplate
        /// </summary>
        protected NhibernateTemplate Template;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        public NhibernateUnitWork(NHibernate.ISession session)
        {
            this.Session = session;
            this.Connection = session.Connection;
            this.Template = NhibernateTemplate.Empty;
            this.Thorw = false;
        }

        //public NhibernateUnitWork(AppSessionFactory sessionFactory) : base(sessionFactory)
        //{
        //    this.Connection = sessionFactory.OpenSession().Connection;
        //}
        /// <summary>未实现是抛出异常还是不做任何操作 </summary>
        public bool Thorw { get; }

        /// <summary>数据库连接对象 </summary>
        public IDbConnection Connection { get; private set; }


        /// <summary>
        /// 手动赋值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="flag">1 add 2 update 3 delete</param>
        protected virtual bool UpdateValue<T>(T entity, int flag = 1) where T : class
        {
            return entity.UpdateValue(flag);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public virtual ResultDto<T> FindResultDtoByPage<T>(Expression<Func<T, bool>> exp = null,int pageindex = 1, int pagesize = 10) where T : class
        {
            var tuple = FindResultByPage(exp,pageindex,pagesize);
            return new ResultDto<T>(tuple, pageindex, pagesize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public virtual Tuple<List<T>,int> FindResultByPage<T>(Expression<Func<T, bool>> exp = null, int pageindex = 1, int pagesize = 10) where T : class
        {
            // using (var session = AppSessionFactory.OpenSession())
            {
                return Template.FindTupleByPage(Session,exp,pageindex,pagesize);
            }
        }
        /// <summary>根据条件查询实体类数据集数量信息</summary>
        /// <param name="where">条件</param>
        ///<return>返回实体类数据集数量信息</return>

        public virtual int Count<T>(ICriteria where=null) where T : class
        {
            return Template.Count<T>(Session, where);
        }

        /// <summary>根据条件查询实体类数据集信息</summary>
        /// <param name="where">条件</param>
        ///<return>返回实体类数据集信息</return>
        public virtual List<T> FindList<T>(ICriteria where = null) where T : class
        {
            return Template.FindList<T>(Session, where);
        }

        /// <summary>根据条件及分页查询实体类数据集信息</summary>
        /// <param name="where">条件</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<return>返回实体类数据集信息</return>

        public virtual List<T> FindListByPage<T>(ICriteria where = null,int page=1, int size=10) where T : class
        {
            return Template.FindListByPage<T>(Session, where, page, size);
        }

        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息</summary>
        /// <param name="where">条件</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
        public virtual ResultDto<T> FindResultDtoByPage<T>(ICriteria where = null, int page = 1, int size = 10) where T : class
        {
            var res = FindResultByPage<T>(where,page, size);
            return new ResultDto<T>(res.Item1,res.Item2,page,size);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public virtual Tuple<List<T>,int> FindResultByPage<T>(ICriteria where = null,int page=1, int size=10) where T : class
        {
            return Template.FindTupleByPage<T>(Session, where, page, size);
        }

#region async



        /// <summary>根据条件查询实体类数据集信息</summary>
        /// <param name="where">条件</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息</return>

        public virtual  Task<List<T>> FindListAsync<T>(ICriteria where = null,
            CancellationToken cancellationToken = default) where T : class
        {
            return Template.FindListAsync<T>(Session,where, cancellationToken);
        }

        /// <summary>根据条件及分页查询实体类数据集信息</summary>
        /// <param name="where">条件</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息</return>
        public virtual  Task<List<T>> FindListByPageAsync<T>(ICriteria where = null, int page=1, int size=10,
             CancellationToken cancellationToken = default) where T : class
        {
            return Template.FindListByPageAsync<T>(Session, where, page, size, cancellationToken);
        }
        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息</summary>
        /// <param name="where">条件</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
        public virtual  Task<Tuple<List<T>,int>> FindTupleByPageAsync<T>(ICriteria where = null, int page=1,
            int size=10, CancellationToken cancellationToken = default) where T : class
        {
            return Template.FindTupleByPageAsync<T>(Session, where,page,size,cancellationToken);
        }
#endregion async


        /// <summary>查找单个，且不被上下文所跟踪 ef,nhibernate 支持 linq dapper 不支持linq </summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public virtual T FindSingle<T>(Expression<Func<T, bool>> where = null) where T : class
        {
            // using (var session = AppSessionFactory.OpenSession())
			return Template.FindSingle(Session,where);
        }

        /// <summary> 是否存在 默认实现 ef,nhibernate 支持 linq dapper 不支持linq  </summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public bool IsExist<T>(Expression<Func<T, bool>> where) where T : class
        {
            // using (var session = AppSessionFactory.OpenSession())
            {
                return Template.Count<T>(Session, where) >= 1;
            }
        }

        /// <summary> 查询数据 默认实现 ef,nhibernate 支持 linq dapper 不支持linq dapper默认查询所有结果集基于内存 条件查询</summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public virtual IQueryable<T> Find<T>(Expression<Func<T, bool>> where = null) where T : class
        {
            // using (var session = AppSessionFactory.OpenSession())
			return Template.Find(Session, where);
        }

        /// <summary> 查询数据 默认实现 ef,nhibernate 支持 linq dapper 不支持linq orderby参数无效 dapper默认查询所有结果集基于内存 条件查询 </summary>
        /// <param name="where">条件</param>
        /// <param name="page">页数</param>
        /// <param name="size">记录</param>
        /// <returns></returns>
        public virtual IQueryable<T> FindByPage<T>(Expression<Func<T, bool>> where = null,int page = 1, int size = 10) where T : class
        {
            // using (var session = AppSessionFactory.OpenSession())
			return Template.FindByPage(Session, where, page, size);
        }

        /// <summary>查询数量  默认实现 ef,nhibernate 支持 linq dapper 不支持linq  dapper默认查询所有结果数量  </summary>
        /// <param name="wehre">条件</param>
        /// <returns></returns>
        public virtual int Count<T>(Expression<Func<T, bool>> wehre=null) where T : class
        {
            // using (var session = AppSessionFactory.OpenSession())
            {
                return Template.Count<T>(Session, wehre);
            }
        }

        /// <summary> 添加 </summary>
        /// <param name="entity">实体</param>
        public virtual object Insert<T>(T entity) where T : class
        {
            // using (var session=AppSessionFactory.OpenSession())
            {
                UpdateValue(entity);
               return Template.Insert<T>(Session, entity);
            }
        }

        /// <summary>批量 添加 </summary>
        /// <param name="entities">实体</param>
        public virtual  void BatchInsert<T>(T[] entities) where T : class
        {
            //using (var session = AppSessionFactory.OpenSession())
            {
                foreach (var entity in entities)
                {
                    UpdateValue(entity);
                }
                Template.BatchInsert<T>(Session, entities);
            }
        }

        /// <summary> 更新一个实体的所有属性 </summary>
        /// <param name="entity">实体</param>
        public virtual void Update<T>(T entity) where T : class
        {
            //using (var session = AppSessionFactory.OpenSession())
            {
                Update(entity,2);
            }
        }

        /// <summary> 更新一个实体的所有属性 </summary>
        /// <param name="entity">实体</param>
        public virtual void Update<T>(T entity,int flag) where T : class
        {
            //using (var session = AppSessionFactory.OpenSession())
            {
                UpdateValue(entity, flag);
                Template.Update<T>(Session, entity);
            }
        }

        /// <summary> 删除</summary>
        /// <param name="entity">实体</param>
        public virtual void Delete<T>(T entity) where T : class
        {
            // using (var session = AppSessionFactory.OpenSession())
            {
                if(entity is IHasDeletionTime)
                {
                    Update(entity, 3);
                }
                else
                {
                    Template.Delete<T>(Session, entity);
                }
            }
        }

        /// <summary> 批量删除 默认实现  nhibernate 支持 EF dapper 未实现</summary>
        public virtual void Delete<T>(object id) where T : class
        {
            // using (var session = AppSessionFactory.OpenSession())
            {
                if (typeof(T).IsAssignableFrom(typeof(IHasDeletionTime)))
                {
                    if (string.IsNullOrEmpty(id?.ToString())) throw new ArgumentNullException("id");
                    ITransaction transaction = null;
                    try
                    {
                        using (transaction = Session.BeginTransaction())
                        {
                            var obj = Session.Get<T>(id);
                            if(obj is IHasDeletionTime deletionTime)
                            {
                                if (!deletionTime.IsDeleted)
                                {
                                    UpdateValue(obj,3);
                                    Session.Update(obj);
                                    transaction.Commit();
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        if (transaction != null)
                        {
                            transaction.Rollback();
                        }
                        throw e;
                    }
                }
                else
                {
                    Template.Delete<T>(Session, id);
                }
               
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
            // using (var session = AppSessionFactory.OpenSession())
            Template.Update<T>(Session, where, update);

        }

        /// <summary> 批量删除 默认实现 ef,nhibernate 支持 linq dapper 不支持linq dapper 未实现</summary>
        /// <param name="where">条件</param>

        public virtual void Delete<T>(Expression<Func<T, bool>> where=null) where T : class
        {
            //using (var session = AppSessionFactory.OpenSession())
            {
                Template.Delete<T>(Session, where);
            }
        }


        /// <summary> 执行sql </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public virtual int ExecuteSql(string sql)
        {
            // using (var session = AppSessionFactory.OpenSession())
            {
                return Template.ExecuteQuery(Session, sql);
            }

        }
        /// <summary> 执行sql </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public virtual  Task<int> ExecuteSqlAsync(string sql)
        {
            // using (var session = AppSessionFactory.OpenSession())
            {
                return Template.ExecuteQueryAsync(Session, sql);
            }

        }
        /// <summary> 操作成功 保存到库里 默认实现 ef 支持  dapper nhibernate 无任何操作 </summary>
        public virtual void Save()
        {
        
        }

#region async
        /// <summary>查找单个，且不被上下文所跟踪 ef,nhibernate 支持 linq dapper 不支持linq </summary>
        /// <param name="where">条件</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task<T> FindSingleAsync<T>(Expression<Func<T, bool>> where = null, CancellationToken cancellationToken = default) where T : class
        {
            // using (var session = AppSessionFactory.OpenSession())
			return Template.FindSingleAsync(Session,where,cancellationToken);
        }

        /// <summary> 是否存在 默认实现 ef,nhibernate 支持 linq dapper 不支持linq  </summary>
        /// <param name="where">条件</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual  Task<bool> IsExistAsync<T>(Expression<Func<T, bool>> where=null, CancellationToken cancellationToken = default) where T : class
        {
            // using (var session = AppSessionFactory.OpenSession())
            {
                return new Task<bool>(()=>Template.CountAsync<T>(Session, where).Result >= 1);
            }
        }

        /// <summary> 查询数据 默认实现 ef,nhibernate 支持 linq dapper 不支持linq dapper默认查询所有结果集基于内存 条件查询</summary>
        /// <param name="where">条件</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task<IQueryable<T>> FindAsync<T>(Expression<Func<T, bool>> where = null, CancellationToken cancellationToken = default
            ) where T : class
        {
            // using (var session = AppSessionFactory.OpenSession())
			var res = Template.FindAsync(Session,where);
			return res;
        }

        /// <summary> 查询数据 默认实现 ef,nhibernate 支持 linq dapper 不支持linq orderby参数无效 dapper默认查询所有结果集基于内存 条件查询 </summary>
        /// <param name="page">页数</param>
        /// <param name="size">记录</param>
        /// <param name="where">条件</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task<IQueryable<T>> FindByPageAsync<T>(Expression<Func<T, bool>> where = null, int page = 1, int size = 10, CancellationToken cancellationToken = default) where T : class
        {
            // using (var session = AppSessionFactory.OpenSession())
			var res = Template.FindByPageAsync(Session,where, page, size);
			return res;
        }

        /// <summary>查询数量  默认实现 ef,nhibernate 支持 linq dapper 不支持linq  dapper默认查询所有结果数量  </summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
        /// <param name="cancellationToken"></param>
        public virtual Task<int> CountAsync<T>(Expression<Func<T, bool>> where = null, CancellationToken cancellationToken = default) where T : class
        {
            // using (var session = AppSessionFactory.OpenSession())
            {
                return Template.CountAsync<T>(Session, where, cancellationToken);
            }
        }
        /// <summary>查询数量  默认实现 ef,nhibernate 支持 linq dapper 不支持linq  dapper默认查询所有结果数量  </summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
        /// <param name="cancellationToken"></param>
        public virtual Task<int> CountAsync<T>(ICriteria  where = null, CancellationToken cancellationToken = default) where T : class
        {
            // using (var session = AppSessionFactory.OpenSession())
            {
                return Template.CountAsync<T>(Session, where, cancellationToken);
            }
        }
        /// <summary> 添加 </summary>
        /// <param name="entity">实体</param>
        /// <param name="cancellationToken"></param>
        public virtual Task InsertAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class
        {
            // using (var session=AppSessionFactory.OpenSession())
            {
                return Template.InsertAsync<T>(Session, entity, cancellationToken);
            }
        }

        /// <summary>批量 添加 </summary>
        /// <param name="entities">实体</param>
        /// <param name="cancellationToken"></param>
        public virtual Task BatchInsertAsync<T>(T[] entities, CancellationToken cancellationToken = default) where T : class
        {
            //using (var session = AppSessionFactory.OpenSession())
            {
                return Template.BatchInsertAsync<T>(Session, entities, cancellationToken);
            }
        }

        /// <summary> 更新一个实体的所有属性 </summary>
        /// <param name="entity">实体</param>
        /// <param name="cancellationToken"></param>
        public virtual Task UpdateAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class
        {
            //using (var session = AppSessionFactory.OpenSession())
            {
                return UpdateAsync( entity,2,cancellationToken);
            }
        }
        /// <summary> 更新一个实体的所有属性 </summary>
        /// <param name="entity">实体</param>
        /// <param name="flag"></param>
        /// <param name="cancellationToken"></param>
        public virtual Task UpdateAsync<T>(T entity,int flag=2, CancellationToken cancellationToken = default) where T : class
        {
            //using (var session = AppSessionFactory.OpenSession())
            {
                UpdateValue(entity, flag);
                return Template.UpdateAsync<T>(Session, entity,cancellationToken);
            }
        }
        /// <summary> 删除</summary>
        /// <param name="entity">实体</param>
        /// <param name="cancellationToken"></param>
        public virtual Task DeleteAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class
        {
            // using (var session = AppSessionFactory.OpenSession())
            {
                if(entity is IHasDeletionTime)
                {
                    return UpdateAsync(entity,3,cancellationToken);
                }
                return Template.DeleteAsync<T>(Session, entity, cancellationToken);
            }
        }

        /// <summary> 批量删除 默认实现  nhibernate 支持 EF dapper 未实现</summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        public virtual Task DeleteAsync<T>(object id, CancellationToken cancellationToken = default) where T : class
        {
            // using (var session = AppSessionFactory.OpenSession())
            {
                if (typeof(T).IsAssignableFrom(typeof(IHasDeletionTime)))
                {
                    if (string.IsNullOrEmpty(id?.ToString())) throw new ArgumentNullException("id");
                    ITransaction transaction = null;
                    try
                    {
                        using (transaction = Session.BeginTransaction())
                        {
#if NET40 || NET45 || NET451 || NET452 || NET46
                            var obj = Session.Get<T>(id);
#else
                            var obj = Session.GetAsync<T>(id, cancellationToken).Result;
#endif
                            if (obj is IHasDeletionTime deletionTime)
                            {
                                if (!deletionTime.IsDeleted)
                                {
                                    UpdateValue(obj, 3);
#if NET40 || NET45 || NET451 || NET452 || NET46
                                    Session.Update(obj);
                                    transaction.Commit();
                                    return new Task(()=>{});
#else
                                    Session.UpdateAsync(obj).GetAwaiter().GetResult();
                                    return transaction.CommitAsync(cancellationToken);
#endif
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        if (transaction != null)
                        {
#if NET40 || NET45 || NET451 || NET452 || NET46
                                    transaction.Rollback();
                                    return new Task(()=>{});
#else
                            transaction.RollbackAsync(cancellationToken);
#endif
                        }
                    }
                    return TaskHelper.CompletedTask;
                }
                else
                {
                    return Template.DeleteAsync<T>(Session, id, cancellationToken);
                }
            }
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
            // using (var session = AppSessionFactory.OpenSession())
            return Template.UpdateAsync<T>(Session, where, update, cancellationToken);
        }

        /// <summary> 批量删除 默认实现 ef,nhibernate 支持 linq dapper 不支持linq dapper 未实现</summary>
        /// <param name="where">条件</param>
        /// <param name="cancellationToken"></param>
        public virtual Task DeleteAsync<T>(Expression<Func<T, bool>> where=null, CancellationToken cancellationToken = default) where T : class
        {
            //using (var session = AppSessionFactory.OpenSession())
             return Template.DeleteAsync<T>(Session, where, cancellationToken);
        }


        /// <summary> 执行sql </summary>
        /// <param name="sql"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task<int> ExecuteSqlAsync(string sql, CancellationToken cancellationToken = default)
        {
            // using (var session = AppSessionFactory.OpenSession())
            {
                return Template.ExecuteQueryAsync(Session, sql, cancellationToken);
            }

        }


        /// <summary> 操作成功 保存到库里 默认实现 ef 支持  dapper nhibernate 无任何操作 </summary>
        /// <param name="cancellationToken"></param>
        public virtual Task SaveAsync(CancellationToken cancellationToken = default)
        {
#if NET40 || NET45 || NET451 || NET452 || NET46
            return  new Task(()=>{});
#else
            return Task.CompletedTask;
#endif
        }
#endregion async
        /// <summary>
        /// 
        /// </summary>
        void IDisposable.Dispose()
        {
            //SessionFactory.Dispose();
            Session.Dispose();
        }

      
    }
}
#endif