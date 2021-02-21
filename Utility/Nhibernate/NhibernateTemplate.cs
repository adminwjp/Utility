#if !(NET10 || NET11 || NET20 || NET30 || NET35 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
//#if NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using NHibernate;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;
using NHibernate.Linq;
using Utility.Nhibernate.Infrastructure;
using System.Threading;
using NHibernate.Criterion;
using Utility.Page;
using Utility.Threads;
//async await >=45 

namespace Utility.Nhibernate
{
    /// <summary> Nihernate 模板 </summary>
    public class NhibernateTemplate
    { 
        /// <summary>
        /// 
        /// </summary>
        public  IInterceptor Interceptor { get;private set; }

        /// <summary>
        /// 
        /// </summary>
        protected ISessionFactory SessionFactory { get; }
        /// <summary>
        /// 
        /// </summary>
        public AppSessionFactory AppSessionFactory { get; }
        /// <summary>
        ///构造函数 
        /// </summary>
        public NhibernateTemplate(AppSessionFactory sessionFactory)
        {
            AppSessionFactory = sessionFactory;
            SessionFactory = sessionFactory.SessionFactory;
            Interceptor = sessionFactory.Interceptor;
        } 
      
        /// <summary>
        /// 
        /// </summary>
        protected NhibernateTemplate()
        {

        }
        class Inner {
          public static readonly   NhibernateTemplate Instance = new NhibernateTemplate();
        }

          /// <summary>
        ///NhibernateTemplate 
        /// </summary>
        public static NhibernateTemplate Empty => Inner.Instance;

#region ISession
        /// <summary> Nihernate 添加操作 </summary>
        /// <param name="session"><see cref="ISession"/></param>
        /// <param name="obj"></param>
        /// <returns>获取主键编号</returns>
        public virtual object Insert<T>(ISession session, T obj) where T : class
        {
            if (obj == null) throw new ArgumentNullException("obj");
			ITransaction transaction = null;
			try
			{
				using (transaction = session.BeginTransaction())
				{
					object result =  Add(session,obj);
					transaction.Commit();
					return result;
				}
			}
			catch (Exception e)
			{
				if (transaction != null)
				{
					transaction.Rollback();
				}
                return -1;
			}
			
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public object Add<T>(ISession session, T obj) where T : class
		{
			object result = session.Save(obj);
			return result;
		}

        /// <summary> Nihernate 添加操作 </summary>
        /// <param name="session"><see cref="ISession"/></param>
        /// <param name="obj"></param>
        /// <returns>获取主键编号</returns>
        public virtual int BatchInsert<T>(ISession session, T[] obj) where T : class
        {
            if (obj == null||obj.Length==0) throw new ArgumentNullException("obj");
			using (ITransaction transaction = session.BeginTransaction())
			{
				int res = BatchAdd(session,obj);
				transaction.Commit();
				return res;
			}
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
		 public virtual int BatchAdd<T>(ISession session, T[] obj) where T : class
		 {
			int res = 0;
			foreach (var item in obj)
			{
				session.Save(item);
				res++;
			}
			return res;
		 }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="obj"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual
#if !(NET40 || NET45 || NET451 || NET452 || NET46)
            async
#endif
            Task<int> BatchInsertAsync<T>(ISession session, T[] obj, CancellationToken cancellationToken = default) where T : class
        {
#if (NET40 || NET45 || NET451 || NET452 || NET46)
           var res= BatchInsert(session,obj);
           return new Task<int>(()=>res);
#else
            if (obj == null || obj.Length == 0) throw new ArgumentNullException("obj");
            ITransaction transaction = null;
            try
            {
                using (transaction = session.BeginTransaction())
                {
                    int res =await BatchAddAsync(session,obj,cancellationToken);
                    await transaction.CommitAsync(cancellationToken);
                    return res;
                }
            }
            catch (Exception e)
            {
                if (transaction != null)
                {
                   await transaction.RollbackAsync(cancellationToken);
                }
                throw e;
            }
#endif
		}

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="obj"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual
#if !(NET40 || NET45 || NET451 || NET452 || NET46)
		async
#endif
		Task<int> BatchAddAsync<T>(ISession session, T[] obj, CancellationToken cancellationToken = default) where T : class
		{
#if (NET40 || NET45 || NET451 || NET452 || NET46)
           var res= BatchAdd(session,obj);
           return new Task<int>(()=>res);
#else
			int res = 0;
			foreach (var item in obj)
			{
			   await session.SaveAsync(item, cancellationToken);
			   res++;
			}
			return res;
#endif
		}

    /// <summary> Nihernate 添加异步操作 </summary>
    /// <param name="session"><see cref="ISession"/></param>
    /// <param name="obj"></param>
    ///<param name="cancellationToken"></param>
    /// <returns>获取主键编号</returns>
    public virtual 
#if !(NET40 || NET45 || NET451 || NET452 || NET46)
    async
#endif
    Task<object> InsertAsync<T>(ISession session, T obj, CancellationToken cancellationToken = default) where T : class
        {
#if NET40 || NET45 || NET451 || NET452 || NET46
            var res=Insert(session,obj);
            return new Task<object>(()=>res);
#else
            if (obj == null) throw new ArgumentNullException("obj");
            ITransaction transaction = null;
            try
            {
                using (transaction =  session.BeginTransaction())
                {
                    var  result =await AddAsync(session,obj, cancellationToken);
                    await transaction.CommitAsync(cancellationToken);
                    return result;
                }
            }
            catch (Exception e)
            {
                if (transaction != null)
                {
                    await transaction.RollbackAsync(cancellationToken);
                }
                return new Task<object>(()=>null);
            }
#endif
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="obj"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
		public virtual 
#if !(NET40 || NET45 || NET451 || NET452 || NET46)
		async
#endif
		Task<object> AddAsync<T>(ISession session, T obj, CancellationToken cancellationToken = default) where T : class
        {
#if NET40 || NET45 || NET451 || NET452 || NET46
            var res=Add(session,obj);
            return new Task<object>(()=>res);
#else
			var  result =await session.SaveAsync(obj, cancellationToken);
			return result;
#endif			 
		}

        /// <summary> Nihernate 修改操作 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"><see cref="ISession"/></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual void Update<T>(ISession session, T obj) where T : class
        {
            if (obj == null) throw new ArgumentNullException("obj");
            ITransaction transaction = null;
            try
            {
                using (transaction = session.BeginTransaction())
                {
                    Modify(session,obj);
                    transaction.Commit();
                }
            }
            catch (Exception e)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
            }
        }
		
        /// <summary>
        /// 修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="obj"></param>
		public virtual void Modify<T>(ISession session, T obj) where T : class
		{
			session.Clear();
			//session.Refresh();
			//session.Merge(obj);
			session.Update(obj);
		}

        /// <summary> Nihernate 修改操作 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"><see cref="ISession"/></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual void BatchUpdate<T>(ISession session, T[] obj) where T : class
        {
            if (obj == null || obj.Length == 0) throw new ArgumentNullException("obj");
            ITransaction transaction = null;
            try
            {
                using (transaction = session.BeginTransaction())
                {
                    BatchModify(session,obj);
                    transaction.Commit();
                }
            }
            catch (Exception e)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
            }
        }
		
        /// <summary>
        /// 批量修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="obj"></param>
		public virtual void BatchModify<T>(ISession session, T[] obj) where T : class
		{
		    foreach (var item in obj)
			{
				session.Update(item);
			}
		}

        /// <summary> Nihernate 修改异步操作 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"><see cref="ISession"/></param>
        /// <param name="obj"></param>
        ///<param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task UpdateAsync<T>(ISession session, T obj, CancellationToken cancellationToken = default) where T : class
        {
#if NET40 || NET45 || NET451 || NET452 || NET46
            Update(session,obj);
            return new Task(()=>{});
#else
            if (obj == null ) throw new ArgumentNullException("obj");
            ITransaction transaction = null;
            try
            {
                using (transaction = session.BeginTransaction())
                {
                    Task task = ModifyAsync(session,obj,cancellationToken);
                    transaction.CommitAsync();
                    return task;
                }
            }
            catch (Exception e)
            {
                if (transaction != null)
                {
                    transaction.RollbackAsync();
                }
                throw e;
            }
#endif
        }
		
        /// <summary>
        /// 修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="obj"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="tx"></param>
        /// <returns></returns>
		public virtual Task ModifyAsync<T>(ISession session, T obj, CancellationToken cancellationToken = default,bool tx=true) where T : class
		{
#if NET40 || NET45 || NET451 || NET452 || NET46
            Modify(session,obj);
            return new Task(()=>{});
#else
			Task task = session.UpdateAsync(obj,cancellationToken);
			return task;
#endif
		}

        /// <summary> Nihernate 删除操作 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"><see cref="ISession"/></param>
        /// <param name="obj"></param>
        public virtual void Delete<T>(ISession session, T obj) where T : class
        {
            if (obj == null ) throw new ArgumentNullException("obj");
            ITransaction transaction = null;
            try
            {
                using (transaction = session.BeginTransaction())
                {
                    Remove(session,obj);
                    transaction.Commit();
                }
            }
            catch (Exception e)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="obj"></param>
		public virtual void Remove<T>(ISession session, T obj) where T : class
		{
			 session.Delete(obj);
		}
		
        /// <summary> Nihernate 删除操作 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"><see cref="ISession"/></param>
        /// <param name="id"></param>
        public virtual void Delete<T>(ISession session, object id) where T : class
        {
            if (string.IsNullOrEmpty(id?.ToString())) throw new ArgumentNullException("id");
            ITransaction transaction = null;
            try
            {
                using (transaction = session.BeginTransaction())
                {
                    Remove<T>(session,id);
                    transaction.Commit();
                }
            }
            catch (Exception e)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
            }
        }
		
        /// <summary>
        /// 根据 id 删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="id"></param>
		public virtual void Remove<T>(ISession session, object id) where T : class
		{
		    var obj = session.Get<T>(id);
			session.Delete(obj);
		}

        /// <summary> Nihernate 删除异步操作 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"><see cref="ISession"/></param>
        /// <param name="obj"></param>
        ///<param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task DeleteAsync<T>(ISession session, T obj, CancellationToken cancellationToken = default) where T : class
        {
#if NET40 || NET45 || NET451 || NET452 || NET46
            Delete(session,obj);
            return new Task(()=>{});
#else
            if (obj == null ) throw new ArgumentNullException("obj");
            ITransaction transaction = null;
            try
            {
                using (transaction = session.BeginTransaction())
                {
                    Task task = RemoveAsync(session,obj,cancellationToken);
                    transaction.CommitAsync();
                    return task;
                }
            }
            catch (Exception e)
            {
                if (transaction != null)
                {
                   return transaction.RollbackAsync();
                }
                return TaskHelper.CompletedTask;
            }
#endif
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="obj"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
		public virtual Task RemoveAsync<T>(ISession session, T obj, CancellationToken cancellationToken = default) where T : class
		{
#if NET40 || NET45 || NET451 || NET452 || NET46
            Remove(session,obj);
            return new Task(()=>{});
#else
			Task task = session.DeleteAsync(obj,cancellationToken);
			return task;
#endif
		}

        /// <summary> Nihernate 删除异步操作 </summary>
        /// <param name="session"><see cref="ISession"/></param>
        /// <param name="id"></param>
        ///<param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task DeleteAsync<T>(ISession session, object id, CancellationToken cancellationToken = default) where T : class
        {
#if NET40 || NET45 || NET451 || NET452 || NET46
            Delete<T>(session,id);
            return new Task(()=>{});
#else
            if (string.IsNullOrEmpty(id?.ToString())) throw new ArgumentNullException("id");
            ITransaction transaction = null;
            try
            {
                using (transaction = session.BeginTransaction())
                {
                    Task task = RemoveAsync<T>(session,id,cancellationToken);
                    transaction.CommitAsync(cancellationToken);
                    return task;
                }
            }
            catch (Exception e)
            {
                if (transaction != null)
                {
                 return   transaction.RollbackAsync(cancellationToken);
                }
                return TaskHelper.CompletedTask;
            }
#endif
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
		 public virtual Task RemoveAsync<T>(ISession session, object id, CancellationToken cancellationToken = default) where T : class
		 {
#if NET40 || NET45 || NET451 || NET452 || NET46
            Remove<T>(session,id);
            return new Task(()=>{});
#else
			Task task = session.DeleteAsync(session.GetAsync<T>(id, cancellationToken).Result, cancellationToken);
			return task;
#endif			
		 }

        /// <summary> Nihernate 查询操作 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"><see cref="IStatelessSession"/></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T FindSingle<T>(ISession session, object id) where T : class
        {
            if (string.IsNullOrEmpty(id?.ToString())) throw new ArgumentNullException("id");
            using (ITransaction transaction = session.BeginTransaction())
            {
                return Get<T>(session,id);
            }
        }
		
        /// <summary>
        /// 根据 id 查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="id"></param>
        /// <returns></returns>
		public virtual T Get<T>(ISession session, object id) where T : class
		{
			return session.Get<T>(id);
		}
		
        /// <summary> Nihernate 查询异步操作 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"><see cref="ISession"/></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual Task<T> FindSingleAsync<T>(ISession session, object id) where T : class
        {
#if NET40 || NET45 || NET451 || NET452 || NET46
            return new Task<T>(()=>FindSingle<T>(session,id));
#else
            if (string.IsNullOrEmpty(id?.ToString())) throw new ArgumentNullException("id");
            using (ITransaction transaction = session.BeginTransaction())
                return GetAsync<T>(session,id);
#endif
        }

        /// <summary>
        /// 根据 id 查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual Task<T> GetAsync<T>(ISession session, object id) where T : class
		{
#if NET40 || NET45 || NET451 || NET452 || NET46
            return new Task<T>(()=>Get<T>(session,id));
#else
			return session.GetAsync<T>(id);
#endif
		}
		
        /// <summary>
        /// 根据实体名称查询，没有则根据泛型查询  异常
        /// identifier of an instance of  was altered from 10 to 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"><see cref="ISession"/></param>
        /// <param name="entityName"></param>
        /// <returns></returns>
        public virtual IQueryable<T> Query<T>(ISession session, string entityName = null) where T : class
        {
            using (ITransaction transaction = session.BeginTransaction())
			{
				return GetQuery<T>(session,entityName);
			}
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="entityName"></param>
        /// <returns></returns>
		public virtual IQueryable<T> GetQuery<T>(ISession session, string entityName = null) where T : class
		{
		   return entityName == null ? session.Query<T>() : session.Query<T>(entityName);//同一个session 这样查询没问题 添加时有出现该问题
		}

        /// <summary>
        /// 根据实体名称查询，没有则根据泛型查询  异常
        /// identifier of an instance of  was altered from 10 to 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"><see cref="IStatelessSession"/></param>
        /// <param name="entityName"></param>
        /// <returns></returns>
        public virtual IQueryable<T> Query<T>(IStatelessSession session, string entityName = null) where T : class
        {
            using (ITransaction transaction = session.BeginTransaction())
			{
				return GetQuery<T>(session,entityName);
			}
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="entityName"></param>
        /// <returns></returns>
        public virtual IQueryable<T> GetQuery<T>(IStatelessSession session, string entityName = null) where T : class
		{
			return entityName == null ? session.Query<T>() : session.Query<T>(entityName);//同一个session 这样查询没问题 添加时有出现该问题
		}
		
        /// <summary>
        /// 根据条件查询，没有则根据泛型查询  异常
        /// identifier of an instance of  was altered from 10 to 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"><see cref="ISession"/></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual IQueryable<T> Query<T>(ISession session, Expression<Func<T, bool>> where = null) where T : class
        {
            using (ITransaction transaction = session.BeginTransaction())
			{
				return GetQuery<T>(session,where);
			}
        }

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual IQueryable<T> GetQuery<T>(ISession session, Expression<Func<T, bool>> where = null) where T : class
		{
			return where != null ? session.Query<T>().Where(where) : session.Query<T>();;//同一个session 这样查询没问题 添加时有出现该问题
		}

        /// <summary>
        /// 根据条件查询，没有则根据泛型查询  异常
        /// identifier of an instance of  was altered from 10 to 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"><see cref="IStatelessSession"/></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual IQueryable<T> Query<T>(IStatelessSession session, Expression<Func<T, bool>> where = null) where T : class
        {
            using (ITransaction transaction = session.BeginTransaction())
			{
				return GetQuery<T>(session,where);
			}
        }

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="where"></param>
        /// <returns></returns>
		public virtual IQueryable<T> GetQuery<T>(IStatelessSession session, Expression<Func<T, bool>> where = null) where T : class
		{
			return where != null ? session.Query<T>().Where(where) : session.Query<T>();;//同一个session 这样查询没问题 添加时有出现该问题
		}

        /// <summary>根据实体类型查询，没有则根据泛型查询 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"><see cref="ISession"/></param>
        /// <param name="alias"></param>
        /// <returns></returns>
        public virtual IList<T> FindList<T>(ISession session, Expression<Func<T>> alias = null) where T : class
        {
			using (ITransaction transaction = session.BeginTransaction())
			{
				return GetList<T>(session,alias);
			}
        }

        /// <summary>
        /// 根据实体类型查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="alias"></param>
        /// <returns></returns>
        public virtual IList<T> GetList<T>(ISession session, Expression<Func<T>> alias = null) where T : class
		{
			return alias == null ? session.QueryOver<T>().List() : session.QueryOver<T>(alias).List();
		}


        /// <summary> Nihernate 添加操作 </summary>
        /// <param name="obj"></param>
        /// <returns>获取主键编号</returns>
        public virtual object InsertBySession(object obj)
        {
            using (ISession session =GetSession())
            {
                return Insert(session, obj);
            }
        }

        /// <summary> Nihernate 添加操作 </summary>
        /// <param name="obj"></param>
        /// <returns>获取主键编号</returns>
        public virtual int BatchInsertBySession<T>(T[] obj) where T : class
        {
            using (ISession session =GetSession())
            {
                return BatchInsert(session, obj);
            }
        }

        /// <summary> Nihernate 添加异步操作 </summary>
        /// <param name="obj"></param>
        /// <returns>获取主键编号</returns>
        public virtual Task<object> InsertAsyncBySession(object obj)
        {
            using (ISession session = GetSession())
            {
                return InsertAsync(session, obj);
            }
        }

        /// <summary> Nihernate 修改操作 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual void UpdateBySession<T>(T obj) where T : class
        {
            using (ISession session = GetSession())
            {
                Update(session, obj);
            }
        }

        /// <summary> Nihernate 修改操作 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual void BatchUpdateBySession<T>(T[] obj) where T : class
        {
            using (ISession session =GetSession())
            {
                BatchUpdate(session, obj);
            }
        }

        /// <summary> Nihernate 修改异步操作 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual Task UpdateAsyncBySession<T>(T obj) where T : class
        {
            using (ISession session = GetSession())
            {
                return UpdateAsync(session, obj);
            }
        }

        /// <summary> Nihernate 删除操作 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        public virtual void DeleteBySession<T>(T obj) where T : class
        {
            using (ISession session =GetSession())
            {
                Delete(session, obj);
            }
        }

        /// <summary> Nihernate 删除操作 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        public virtual void DelBySession<T>(object id) where T : class
        {
            using (ISession session = GetSession())
            {
                Delete<T>(session, id);
            }
        }

        /// <summary> Nihernate 删除异步操作 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual Task DeleteAsyncBySession<T>(T obj) where T : class
        {
            using (ISession session =GetSession())
            {
                return DeleteAsync(session, obj);
            }
        }

        /// <summary> Nihernate 删除异步操作 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual Task DeleteAsyncBySession<T>(object id) where T : class
        {
            using (ISession session = GetSession())
            {
                return DeleteAsync<T>(session, id);
            }
        }

        /// <summary> Nihernate 查询操作 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T FindSingleBySession<T>(object id) where T : class
        {
            using (ISession session = GetSession())
            {
                return FindSingle<T>(session, id);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual ISession GetSession()
        {
#pragma warning disable CS0618 // 类型或成员已过时
            ISession session = Interceptor == null ? SessionFactory.OpenSession() : SessionFactory.OpenSession(Interceptor);
#pragma warning restore CS0618 // 类型或成员已过时
            return session;
        }

        /// <summary> Nihernate 查询异步操作 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual Task<T> FindSingleAsyncBySession<T>(object id) where T : class
        {
            using (ISession session = GetSession())
            {
                return FindSingleAsync<T>(session, id);
            }
        }

        /// <summary>根据实体名称查询，没有则根据泛型查询  异常</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityName"></param>
        /// <returns></returns>
        public virtual  IQueryable<T> FindBySession<T>(string entityName = null) where T : class
        {
            using (ISession session = GetSession())
            {
                return Query<T>(session, entityName);
            }
        }

        /// <summary>根据实体类型查询，没有则根据泛型查询 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="alias"></param>
        /// <returns></returns>
        public virtual IList<T> FindListBySession<T>(Expression<Func<T>> alias = null) where T : class
        {
            using (ISession session = GetSession())
            {
                return FindList<T>(session, alias);
            }
        }

        /// <summary>
        ///update or delete or add 
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public virtual int ExecuteBySession(string sql)
        {
            if (string.IsNullOrEmpty(sql)) throw new ArgumentNullException("sql");
            ITransaction transaction = null;
            try
            {
                using (ISession session =GetSession())
                {
                    using (transaction = session.BeginTransaction())
                    {
                        int res = session.CreateQuery(sql).ExecuteUpdate();
                        transaction.Commit();
                        return res;
                    }
                }
            }
            catch (Exception e)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                return -1;
            }
        }
		        
		
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="size">每页记录</param>
        /// <returns></returns>
        public virtual IList<T> FindListBySession<T>(string sql, int size) where T : class
        {
            if (string.IsNullOrEmpty(sql)) throw new ArgumentNullException("sql");
            size = size > 1 ? 10 : size > 100 ? 100 : size;
            using (ISession session =GetSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                    return session.CreateQuery(sql).SetMaxResults(size).List<T>();
            }
        }

        /// <summary>
        /// 根据 条件查询
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public virtual T FindSingleBySession<T>(string sql, string[] param) where T : class
        {
            if (string.IsNullOrEmpty(sql)) throw new ArgumentNullException("sql");
            using (ISession session =GetSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                   return Get<T>(session,sql,param);
                }
            }
        }

        /// <summary>
        /// 根据 条件查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
		public virtual T Get<T>(ISession session,string sql, string[] param) where T : class
		{
			IQuery query = session.CreateQuery(sql);
			if (param != null)
			{
				for (int i = 0; i < param.Length; i++)
				{
					query = query.SetString(i, param[i]);
				}
			}
			return query.List<T>()[0];
		}

        #region linq https://nhibernate.info/previous-doc/v5.1/ref/querylinq.html

        /// <summary>
        /// 根据条件 查询 数量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="where"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task<int> CountAsync<T>(ISession session, Expression<Func<T, bool>> where, CancellationToken cancellationToken = default) where T : class
        {
			using (ITransaction tx = session.BeginTransaction())
			{
				return GetCountAsync(session,where,cancellationToken);
			}
        }

        /// <summary>
        /// 注意 重载(使用默认参数情况下) 可能调用 的是 自身。 构造函数 是的 堆载问题  函数签名最好不一致
        /// 根据条件 查询 数量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="where"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task<int> GetCountAsync<T>(ISession session, Expression<Func<T, bool>> where, CancellationToken cancellationToken = default) where T : class
		{
          
#if NET40 || NET45 || NET451 || NET452 || NET46
			return new Task<int>(()=>Count(session,where));
#else
			IFutureValue<Task<int>> value = Filter(session.Query<T>(), where).ToFutureValue(it => it.CountAsync(cancellationToken));
			return value.Value;
#endif
        }

        /// <summary>
        /// 根据条件 查询 数量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="where"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task<int> CountAsync<T>(IStatelessSession session, Expression<Func<T, bool>> where, CancellationToken cancellationToken = default) where T : class
        {
			using (ITransaction tx = session.BeginTransaction())
			{
				return GetCountAsync(session,where,cancellationToken);
			}
        }

        /// <summary>
        /// 根据条件 查询 数量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="where"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
		public virtual Task<int> GetCountAsync<T>(IStatelessSession session, Expression<Func<T, bool>> where, CancellationToken cancellationToken = default) where T : class
		{
          
#if NET40 || NET45 || NET451 || NET452 || NET46
            return new Task<int>(()=>Count(session,where));
#else
			IFutureValue<Task<int>> value = Filter(session.Query<T>(), where).ToFutureValue(it => it.CountAsync(cancellationToken));
			return value.Value;
#endif
        }

        /// <summary>
        /// 根据条件 查询 数量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual int Count<T>(ISession session, Expression<Func<T, bool>> where=null) where T : class
        {
			using (ITransaction tx= session.BeginTransaction())
			{
				return GetCount(session,where);
			}
        }
		
        /// <summary>
        /// 根据条件 查询 数量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual int GetCount<T>(ISession session, Expression<Func<T, bool>> where=null) where T : class
        {
			IFutureValue<int> value = Filter(session.Query<T>(), where).ToFutureValue(it => it.Count());
            return value.Value;
        }

        /// <summary>
        /// 获取 最大 对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual T Max<T>(ISession session, Expression<Func<T, bool>> where=null) where T : class
        {
			using (ITransaction tx = session.BeginTransaction())
			{
				return GetMax(session, where);
			}
        }

        /// <summary>
        ///  获取 最大 对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="where"></param>
        /// <returns></returns>

        public virtual T GetMax<T>(ISession session, Expression<Func<T, bool>> where=null) where T : class
        {
            return Filter(session.Query<T>(), where).Max();
        }
        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="where"></param>
        /// <param name="entityName"></param>
        /// <returns></returns>
        public virtual IList<T> FindList<T>(ISession session, Expression<Func<T, bool>> where=null, string entityName = "") where T : class
        {
            using (ITransaction tx = session.BeginTransaction())
                return GetList(session,where,entityName);
        }

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="where"></param>
        /// <param name="entityName"></param>
        /// <returns></returns>
		public virtual IList<T> GetList<T>(ISession session, Expression<Func<T, bool>> where=null, string entityName = "") where T : class
		{
			return Query(session,where,entityName).ToList() ;
		}

        /// <summary>
        /// 过滤
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        protected virtual IQueryable<T> Filter<T>(IQueryable<T> source, Expression<Func<T, bool>> where) where T: class
        {
            return (where == null ? source : source.Where(where));
        }

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="where"></param>
        /// <param name="entityName"></param>
        /// <returns></returns>
        public virtual IQueryable<T> Query<T>(ISession session, Expression<Func<T, bool>> where=null, string entityName = "") where T : class
        {
            return string.IsNullOrEmpty(entityName) ? Filter(session.Query<T>(), where) : Filter(session.Query<T>(entityName), where);
        }

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="datas"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual IList<T> FindList<T>(IEnumerable<T> datas, Expression<Func<T, bool>> where=null) where T : class
        {
             return Filter(datas.AsQueryable(), where).ToList();
        }

        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual int Delete<T>(ISession session, Expression<Func<T, bool>> where=null) where T : class
        {
#if NET40 || NET45 || NET451 || NET452 || NET46
            return -1;
#else
            using (ITransaction tx= session.BeginTransaction())
              return Remove(session, where);
#endif
        }

        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="where"></param>
        /// <returns></returns>
		public virtual int Remove<T>(ISession session, Expression<Func<T, bool>> where=null) where T : class
        {
#if NET40 || NET45 || NET451 || NET452 || NET46
            return -1;
#else
            return Filter(session.Query<T>(), where).Delete();
#endif
        }

        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="where"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public virtual Task<int> DeleteAsync<T>(ISession session, Expression<Func<T, bool>> where=null, CancellationToken cancellation = default) where T : class
        {
#if NET40 || NET45 || NET451 || NET452 || NET46
            return new Task<int>(()=>Delete(session,where));
#else
			using (ITransaction tx= session.BeginTransaction())
				return RemoveAsync(session, where,cancellation);
#endif
        }

        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="where"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public virtual   Task<int> RemoveAsync<T>(ISession session, Expression<Func<T, bool>> where=null, CancellationToken cancellation = default) where T : class
        {
#if NET40 || NET45 || NET451 || NET452 || NET46
            return new Task<int>(()=>Remove(session,where));
#else
            return  Filter(session.Query<T>(), where).DeleteAsync(cancellation);
#endif
        }

        /// <summary>
        /// 根据条件修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="where"></param>
        /// <param name="update"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public virtual   Task<int> UpdateAsync<T>(ISession session, Expression<Func<T, bool>> where, Expression<Func<T, T>> update, CancellationToken cancellation = default) where T : class
        {
#if NET40 || NET45 || NET451 || NET452 || NET46
            return new Task<int>(()=>Update(session,where,update));
#else
			using (ITransaction tx= session.BeginTransaction())
				return  ModifyAsync(session, where,update, cancellation);
#endif
        }

        /// <summary>
        /// 根据条件修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="where"></param>
        /// <param name="update"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public virtual   Task<int> ModifyAsync<T>(ISession session, Expression<Func<T, bool>> where, Expression<Func<T, T>> update, CancellationToken cancellation = default) where T : class
        {
#if NET40 || NET45 || NET451 || NET452 || NET46
            return new Task<int>(()=>Modify(session,where,update));
#else
            return  Filter(session.Query<T>(), where).UpdateAsync(update, cancellation);
#endif
        }

        /// <summary>
        /// 根据条件修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="where"></param>
        /// <param name="update"></param>
        /// <returns></returns>
        public virtual int Update<T>(ISession session, Expression<Func<T, bool>> where, Expression<Func<T, T>> update) where T : class
        {
#if NET40 || NET45 || NET451 || NET452 || NET46
            return -1;
#else
			using (ITransaction tx= session.BeginTransaction())
				return Modify(session, where,update);
#endif
        }

        /// <summary>
        /// 根据条件修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="where"></param>
        /// <param name="update"></param>
        /// <returns></returns>
        public virtual int Modify<T>(ISession session, Expression<Func<T, bool>> where, Expression<Func<T, T>> update) where T : class
        {
#if NET40 || NET45 || NET451 || NET452 || NET46
            return -1;
#else
            return Filter(session.Query<T>(), where).Update(update);
#endif
        }

        /// <summary>
        /// 根据条件修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProp"></typeparam>
        /// <param name="session"></param>
        /// <param name="where"></param>
        /// <param name="update"></param>
        /// <returns></returns>
        public virtual int Update<T, TProp>(ISession session, Expression<Func<T, bool>> where, Expression<Func<T, TProp>>[] update) where T : class
        {
#if NET40 || NET45 || NET451 || NET452 || NET46
            return -1;
#else
            using (ITransaction tx= session.BeginTransaction())
				return Modify(session, where,update);
#endif
        }

        /// <summary>
        /// 根据条件修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProp"></typeparam>
        /// <param name="session"></param>
        /// <param name="where"></param>
        /// <param name="update"></param>
        /// <returns></returns>
        public virtual int Modify<T, TProp>(ISession session, Expression<Func<T, bool>> where, Expression<Func<T, TProp>>[] update) where T : class
        {
#if NET40 || NET45 || NET451 || NET452 || NET46
            return -1;
#else
            UpdateBuilder<T> updateBuilder = Filter(session.Query<T>(), where).UpdateBuilder<T>();
            for (int i = 0; i < update.Length/2; i++)
            {
                updateBuilder = updateBuilder.Set(update[i], update[i * 2 + 1]);
            }
            return updateBuilder.Update();
#endif
        }

        /// <summary>
        /// 根据条件修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="where"></param>
        /// <param name="update"></param>
        /// <returns></returns>
        public virtual int Update<T>(ISession session, Expression<Func<T, bool>> where, Expression<Func<T, dynamic>> update) where T : class
        {
#if NET40 || NET45 || NET451 || NET452 || NET46
            return -1;
#else
           using (ITransaction tx= session.BeginTransaction())
				return Modify(session, where,update);
#endif
        }

        /// <summary>
        /// 根据条件修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="where"></param>
        /// <param name="update"></param>
        /// <returns></returns>
		public virtual int Modify<T>(ISession session, Expression<Func<T, bool>> where, Expression<Func<T, dynamic>> update) where T : class
        {
#if NET40 || NET45 || NET451 || NET452 || NET46
            return -1;
#else
            return Filter(session.Query<T>(), where).Update(update);
#endif
        }
		
#endregion linq https://nhibernate.info/previous-doc/v5.1/ref/querylinq.html

#endregion ISession

#region IStatelessSession

        /// <summary> Nihernate 添加操作 </summary>
        /// <param name="session"><see cref="IStatelessSession"/></param>
        /// <param name="obj"></param>
        /// <returns>获取主键编号</returns>
        public virtual object Insert<T>(IStatelessSession session,T obj) where T : class
        {
            if (obj == null) throw new ArgumentNullException("obj");
            ITransaction transaction=null;
            try
            {
                using (transaction = session.BeginTransaction())
                {
                    object result = Add(session,obj);
                    transaction.Commit();
                    return result;
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
		
        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
		public virtual object Add<T>(IStatelessSession session,T obj) where T : class
        {
            object result = session.Insert(obj);
			return result;
        }
		
        /// <summary> Nihernate 添加操作 </summary>
        /// <param name="session"><see cref="IStatelessSession"/></param>
        /// <param name="obj"></param>
        /// <returns>获取主键编号</returns>
        public virtual int BatchInsert<T>(IStatelessSession session, T[] obj) where T : class
        {
            if (obj == null|| obj.Length==0) throw new ArgumentNullException("obj");
            ITransaction transaction = null;
            try
            {
                using (transaction = session.BeginTransaction())
                {
                    int res = BatchAdd(session,obj);
                    transaction.Commit();
                    return res;
                }
            }
            catch (Exception e)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                return -1;
            }
        }
		
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
		public virtual int BatchAdd<T>(IStatelessSession session, T[] obj) where T : class
        {
            int res = 0;
			foreach (var item in obj)
			{
				session.Insert(item);
				res++;
			}
			return res;
        }
        
        /// <summary> Nihernate 添加异步操作 </summary>
        /// <param name="session"><see cref="IStatelessSession"/></param>
        /// <param name="obj"></param>
        /// <returns>获取主键编号</returns>
        public virtual Task<object> InsertAsync<T>(IStatelessSession session, T obj, CancellationToken cancellationToken = default) where T : class
        {
#if NET40 || NET45 || NET451 || NET452 || NET46
            var res=Insert(session,obj);
            return new Task<object>(()=>res);
#else
            if (obj == null) throw new ArgumentNullException("obj");
            ITransaction transaction = null;
            try
            {
                using (transaction = session.BeginTransaction())
                {
                    Task<object> result = AddAsync(session,obj,cancellationToken);
                    transaction.CommitAsync(cancellationToken);
                    return result;
                }
            }
            catch (Exception e)
            {
                if (transaction != null)
                {
                    transaction.RollbackAsync(cancellationToken);
                }
                return new Task<object>(()=>null);
            }
#endif
        }


        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="obj"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
		 public virtual Task<object> AddAsync<T>(IStatelessSession session, T obj, CancellationToken cancellationToken = default) where T : class
        {
#if NET40 || NET45 || NET451 || NET452 || NET46
            var res=Add(session,obj);
            return new Task<object>(()=>res);
#else
             Task<object> result = session.InsertAsync(obj,cancellationToken) as Task<object>;
			 return result;
#endif
        }
      
        /// <summary> Nihernate 修改操作 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"><see cref="IStatelessSession"/></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual void Update<T>(IStatelessSession session, T obj) where T : class
        {
            if (obj == null) throw new ArgumentNullException("obj");
            ITransaction transaction = null;
            try
            {
                using (transaction = session.BeginTransaction())
                {
                    Modify(session,obj);
                    transaction.Commit();
                }
            }
            catch (Exception e)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="obj"></param>
	    public virtual void Modify<T>(IStatelessSession session, T obj) where T : class
        {
             session.Update(obj);
        }

        /// <summary> Nihernate 修改操作 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"><see cref="IStatelessSession"/></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual void BatchUpdate<T>(IStatelessSession session, T[] obj) where T : class
        {
            if (obj == null || obj.Length == 0) throw new ArgumentNullException("obj");
            ITransaction transaction = null;
            try
            {
                using (transaction = session.BeginTransaction())
                {
                    BatchModify(session,obj);
                    transaction.Commit();
                }
            }
            catch (Exception e)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
            }
        }

        /// <summary>
        /// 批量修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="obj"></param>
		public virtual void BatchModify<T>(IStatelessSession session, T[] obj) where T : class
        {
            foreach (var item in obj)
			{
				session.Update(item);
			}
        }

        /// <summary> Nihernate 修改异步操作 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"><see cref="IStatelessSession"/></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual Task UpdateAsync<T>(IStatelessSession session, T obj, CancellationToken cancellationToken = default) where T : class
        {
#if (NET40 || NET45 || NET451 || NET452 || NET46)
           Update(session,obj);
           return new Task(()=>{});
#else
            if (obj == null) throw new ArgumentNullException("obj");
            ITransaction transaction = null;
            try
            {
                using (transaction = session.BeginTransaction())
                {
                    Task task = ModifyAsync(session,obj,cancellationToken);
                    transaction.CommitAsync(cancellationToken);
                    return task;
                }
            }
            catch (Exception e)
            {
                if (transaction != null)
                {
                   return transaction.RollbackAsync(cancellationToken);
                }
                return TaskHelper.CompletedTask;
            }
#endif
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="obj"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
		public virtual Task ModifyAsync<T>(IStatelessSession session, T obj, CancellationToken cancellationToken = default) where T : class
        {
#if (NET40 || NET45 || NET451 || NET452 || NET46)
           Modify(session,obj);
           return new Task(()=>{});
#else
            Task task = session.UpdateAsync(obj,cancellationToken);
			return task;
#endif
        }

        /// <summary> Nihernate 删除操作 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"><see cref="IStatelessSession"/></param>
        /// <param name="obj"></param>
        public virtual void Delete<T>(IStatelessSession session, T obj) where T : class
        {
            if (obj == null) throw new ArgumentNullException("obj");
            ITransaction transaction = null;
            try
            {
                using (transaction = session.BeginTransaction())
                {
                    Remove(session,obj);
                    transaction.Commit();
                }
            }
            catch (Exception e)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="obj"></param>
		public virtual void Remove<T>(IStatelessSession session, T obj) where T : class
        {
           session.Delete(obj);
        }

        /// <summary> Nihernate 删除操作 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"><see cref="IStatelessSession"/></param>
        /// <param name="id"></param>
        public virtual void Delete<T>(IStatelessSession session, object id) where T:class
        {
            if (string.IsNullOrEmpty(id?.ToString())) throw new ArgumentNullException("id");
            ITransaction transaction = null;
            try
            {
                using (transaction = session.BeginTransaction())
                {
                   Remove<T>(session,id);
                   transaction.Commit();
                }
            }
            catch (Exception e)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="id"></param>
		public virtual void Remove<T>(IStatelessSession session, object id) where T:class
        {
            session.Delete(session.Get<T>(id));
        }
        /// <summary> Nihernate 删除异步操作 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"><see cref="IStatelessSession"/></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual Task DeleteAsync<T>(IStatelessSession session, T obj, CancellationToken cancellationToken = default) where T : class
        {
#if (NET40 || NET45 || NET451 || NET452 || NET46)
           Delete(session,obj);
           return new Task(()=>{});
#else
            if (obj == null) throw new ArgumentNullException("obj");
            ITransaction transaction = null;
            try
            {
                using (transaction = session.BeginTransaction())
                {
                    Task task = RemoveAsync(session,obj,cancellationToken);
                    transaction.CommitAsync(cancellationToken);
                    return task;
                }
            }
            catch (Exception e)
            {
                if (transaction != null)
                {
                   return transaction.RollbackAsync(cancellationToken);
                }
                return TaskHelper.CompletedTask;
            }
#endif
        }

        /// <summary>
        /// 删除 
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <param name="session">会话</param>
        /// <param name="obj">对象</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
	    public virtual Task RemoveAsync<T>(IStatelessSession session, T obj, CancellationToken cancellationToken = default) where T : class
        {
#if (NET40 || NET45 || NET451 || NET452 || NET46)
           Remove(session,obj);
           return new Task(()=>{});
#else
            Task task = session.DeleteAsync(obj,cancellationToken);
			return task;
#endif
        }

        /// <summary> Nihernate 删除异步操作 </summary>
        /// <param name="session"><see cref="IStatelessSession"/></param>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task DeleteAsync<T>(IStatelessSession session, object id, CancellationToken cancellationToken = default) where T : class
        {
#if (NET40 || NET45 || NET451 || NET452 || NET46)
            Delete(session,id);
            return new Task(()=>{});
#else
            if (string.IsNullOrEmpty(id?.ToString())) throw new ArgumentNullException("id");
            ITransaction transaction = null;
            try
            {
                using (transaction = session.BeginTransaction())
                {
                    Task task = RemoveAsync<T>(session,id,cancellationToken);
                    transaction.CommitAsync(cancellationToken);
                    return task;
                }
            }
            catch (Exception e)
            {
                if (transaction != null)
                {
                   return transaction.RollbackAsync(cancellationToken);
                }
                return TaskHelper.CompletedTask;
            }
#endif
        }

        /// <summary>
        /// 根据 id 删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
		public virtual Task RemoveAsync<T>(IStatelessSession session, object id, CancellationToken cancellationToken = default) where T : class
        {
#if (NET40 || NET45 || NET451 || NET452 || NET46)
            Remove(session,id);
            return new Task(()=>{});
#else
             Task task = session.DeleteAsync(session.GetAsync<T>(id,cancellationToken).Result,cancellationToken);
			 return task;
#endif
        }

        /// <summary> Nihernate 根据 id 查询 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"><see cref="IStatelessSession"/></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T FindSingle<T>(IStatelessSession session, object id) where T : class
        {
            if (string.IsNullOrEmpty(id?.ToString())) throw new ArgumentNullException("id");
            using (ITransaction transaction = session.BeginTransaction())
            {
                return Get<T>(session,id);
            }
        }

        /// <summary>
        /// 根据 id 查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="id"></param>
        /// <returns></returns>
		public virtual T Get<T>(IStatelessSession session, object id) where T : class
        {
            return session.Get<T>(id);
        }

        /// <summary> 根据 id 查询 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"><see cref="IStatelessSession"/></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual Task<T> FindSingleAsync<T>(IStatelessSession session, object id, CancellationToken cancellationToken = default) where T : class
        {
#if (NET40 || NET45 || NET451 || NET452 || NET46)
           var res= FindSingle<T>(session,id);
           return new Task<T>(()=>res);
#else
            if (string.IsNullOrEmpty(id?.ToString())) throw new ArgumentNullException("id");
            using (ITransaction transaction = session.BeginTransaction())
                return GetAsync<T>(session,id,cancellationToken);
#endif
        }

        /// <summary>
        /// 根据 id 查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
		public virtual Task<T> GetAsync<T>(IStatelessSession session, object id, CancellationToken cancellationToken = default) where T : class
        {
#if (NET40 || NET45 || NET451 || NET452 || NET46)
           var res= Get<T>(session,id);
           return new Task<T>(()=>res);
#else
           return session.GetAsync<T>(id,cancellationToken);
#endif
        }
      
        /// <summary>根据实体类型查询，没有则根据泛型查询 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"><see cref="IStatelessSession"/></param>
        /// <param name="alias"></param>
        /// <returns></returns>
        public virtual IList<T> FindList<T>(IStatelessSession session, Expression<Func<T>> alias=null) where T : class
        {
            using (ITransaction tx = session.BeginTransaction())
                return GetList<T>(session,alias);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="alias"></param>
        /// <returns></returns>
		public virtual IList<T> GetList<T>(IStatelessSession session, Expression<Func<T>> alias=null) where T : class
        {
			return alias == null ? session.QueryOver<T>().List() : session.QueryOver<T>(alias).List();
        }

        /// <summary> Nihernate 添加操作 </summary>
        /// <param name="obj"></param>
        /// <returns>获取主键编号</returns>
        public virtual object Insert(object obj)
        {
            using (IStatelessSession session = SessionFactory.OpenStatelessSession())
            {
                return Insert(session,obj);
            }
        }
        /// <summary> Nihernate 添加操作 </summary>
        /// <param name="obj"></param>
        /// <returns>获取主键编号</returns>
        public virtual int BatchInsert<T>(T[] obj) where T : class
        {
            using (IStatelessSession session = SessionFactory.OpenStatelessSession())
            {
                return BatchInsert(session, obj);
            }
        }

        /// <summary> Nihernate 添加异步操作 </summary>
        /// <param name="obj"></param>
        /// <returns>获取主键编号</returns>
        public virtual Task<object> InsertAsync( object obj)
        {
            using (IStatelessSession session = SessionFactory.OpenStatelessSession())
            {
#if (NET40 || NET45 || NET451 || NET452 || NET46)
           var res=Insert(session,obj);
           return new Task<object>(()=>res);
#else
                return InsertAsync(session, obj);
#endif
            }
        }

        /// <summary> Nihernate 修改操作 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual void Update<T>( T obj) where T : class
        {
            using (IStatelessSession session = SessionFactory.OpenStatelessSession())
            {
                Update(session, obj);
            }
        }

        /// <summary> Nihernate 修改操作 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual void BatchUpdate<T>( T[] obj) where T : class
        {
            using (IStatelessSession session = SessionFactory.OpenStatelessSession())
            {
                BatchUpdate(session, obj);
            }
        }

        /// <summary> Nihernate 修改异步操作 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual Task UpdateAsync<T>( T obj) where T : class
        {
            using (IStatelessSession session = SessionFactory.OpenStatelessSession())
            {
#if (NET40 || NET45 || NET451 || NET452 || NET46)
           Update(session,obj);
           return new Task(()=>{});
#else
              return  UpdateAsync(session, obj);
#endif
            }
        }

        /// <summary> Nihernate 删除操作 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        public virtual void Delete<T>(T obj) where T : class
        {
            using (IStatelessSession session = SessionFactory.OpenStatelessSession())
            {
                 Delete(session, obj);
            }
        }

        /// <summary> Nihernate 删除操作 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        public virtual void Delete<T>(object id) where T : class
        {
            using (IStatelessSession session = SessionFactory.OpenStatelessSession())
            {
                Delete<T>(session,id);
            }
        }

        /// <summary> Nihernate 删除异步操作 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual Task DeleteAsync<T>( T obj) where T : class
        {
            using (IStatelessSession session = SessionFactory.OpenStatelessSession())
            {
#if (NET40 || NET45 || NET451 || NET452 || NET46)
               Delete(session,obj);
               return new Task(()=>{});
#else
               return  DeleteAsync(session, obj);
#endif
            }
        }

        /// <summary> Nihernate 删除异步操作 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual Task DeleteAsync<T>( object id) where T : class
        {
            using (IStatelessSession session = SessionFactory.OpenStatelessSession())
            {
#if (NET40 || NET45 || NET451 || NET452 || NET46)
           Delete(session,id);
           return new Task(()=>{});
#else
                return DeleteAsync<T>(session, id);
#endif
            }
        }

        /// <summary> Nihernate 查询操作 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T FindSingle<T>(object id) where T : class
        {
            using (IStatelessSession session = SessionFactory.OpenStatelessSession())
            {
                return FindSingle<T>(session, id);
            }
        }


        /// <summary> Nihernate 查询异步操作 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual Task<T> FindSingleAsync<T>( object id) where T : class
        {
            using (IStatelessSession session = SessionFactory.OpenStatelessSession())
            {
#if (NET40 || NET45 || NET451 || NET452 || NET46)
			   var res=FindSingle<T>(session,id);
			   return new Task<T>(()=>res);
#else
			   return FindSingleAsync<T>(session, id);
#endif
            }
        }
        
        /// <summary>根据实体名称查询，没有则根据泛型查询  异常</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityName"></param>
        /// <returns></returns>
        public virtual IQueryable<T> Query<T>( string entityName = null) where T : class
        {
            using (IStatelessSession session = SessionFactory.OpenStatelessSession())
            {
                return Query<T>(session, entityName);
            }
        }

        /// <summary>根据实体类型查询，没有则根据泛型查询 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="alias"></param>
        /// <returns></returns>
        public virtual IList<T> FindList<T>( Expression<Func<T>> alias = null) where T : class
        {
            using (IStatelessSession session = SessionFactory.OpenStatelessSession())
            {
                return FindList<T>(session, alias);
            }
        }
        /// <summary>
        ///增 删 该
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public virtual int  ExecuteQuery(string sql)
        {
            using (IStatelessSession session = SessionFactory.OpenStatelessSession())
            {
                return ExecuteQuery(session, sql);
            }
        }

        /// <summary>
        /// 增 删 该
        /// </summary>
        /// <param name="session"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public virtual int ExecuteQuery(ISession session, string sql)
        {
           ITransaction transaction = null;
            try
            {
                using (transaction = session.BeginTransaction())
                {
                  
                    //select table 
                    //int res = session.CreateQuery(sql).ExecuteUpdate();  //from tble
                    int res = session.CreateSQLQuery(sql).ExecuteUpdate();  ////select table 
                    transaction.Commit();
                    return res;
                }
            }
            catch (Exception e)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                return -1;
            }
        }

        /// <summary>
        /// 增 删 改 
        /// </summary>
        /// <param name="session"></param>
        /// <param name="sql"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual
#if !(NET40 || NET45 || NET451 || NET452 || NET46)
            async
#endif
            Task<int> ExecuteQueryAsync(ISession session, string sql, CancellationToken cancellationToken = default)
        {
#if (NET40 || NET45 || NET451 || NET452 || NET46)
           var res= ExecuteQuery(session,sql);
           return new Task<int>(()=>res);
#else
            ITransaction transaction = null;
            try
            {
                using (transaction = session.BeginTransaction())
                {

                    //select table 
                    //int res = session.CreateQuery(sql).ExecuteUpdate();  //from tble
                    int res =await session.CreateSQLQuery(sql).ExecuteUpdateAsync(cancellationToken);  ////select table 
                    await transaction.CommitAsync(cancellationToken);
                    return res;
                }
            }
            catch (Exception e)
            {
                if (transaction != null)
                {
                   await  transaction.RollbackAsync(cancellationToken);
                }
                return -1;
            }
#endif
        }

        /// <summary>
        /// 增 删 改 
        /// </summary>
        /// <param name="session"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public virtual int ExecuteQuery(IStatelessSession session,string sql)
        {
            ITransaction transaction = null;
            try
            {
                using (transaction = session.BeginTransaction())
                {
                    int res = session.CreateSQLQuery(sql).ExecuteUpdate();
                    transaction.Commit();
                    return res;
                }
            }
            catch (Exception e)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                return -1;
            }
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="size">每页记录</param>
        /// <returns></returns>
        public virtual IEnumerable<T> FindList<T>(string sql, int size) where T:class
        {  
            using (IStatelessSession session = SessionFactory.OpenStatelessSession())
            {
                using (ITransaction tx = session.BeginTransaction())
                    return session.CreateQuery(sql).SetMaxResults(size).List<T>();
            }
        }
		
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public virtual T FindSingle<T>(string sql,string[] param) where T:class
        {
            using (IStatelessSession session = SessionFactory.OpenStatelessSession())
            {
                using (ITransaction tx = session.BeginTransaction())
                {
                    IQuery query = session.CreateQuery(sql);
                    for (int i = 0; i < param.Length; i++)
                    {
                        query = query.SetString(i, param[i]);
                    }
                    return query.List<T>()[0];
                }
            }
        }
#region linq https://nhibernate.info/previous-doc/v5.1/ref/querylinq.html
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual int Count<T>(IStatelessSession session, Expression<Func<T, bool>> where) where T : class
        {
            using (ITransaction tx = session.BeginTransaction())
            {
                return GetCount(session,where);
            }
        }
		public virtual int GetCount<T>(IStatelessSession session, Expression<Func<T, bool>> where) where T : class
        {
            IFutureValue<int> value = Filter(session.Query<T>(), where).ToFutureValue(it => it.Count());
			return value.Value;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="where"></param>
        /// <param name="entityName"></param>
        /// <returns></returns>
        public virtual IList<T> FindList<T>(IStatelessSession session, Expression<Func<T, bool>> where,string entityName="") where T : class
        {
            using (ITransaction tx = session.BeginTransaction())
                return GetList<T>(session, where,entityName);
        }
		public virtual IList<T> GetList<T>(IStatelessSession session, Expression<Func<T, bool>> where,string entityName="") where T : class
        {
              return string.IsNullOrEmpty(entityName) ? Filter(session.Query<T>(), where).ToList(): Filter(session.Query<T>(entityName), where).ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="where"></param>
        /// <param name="entityName"></param>
        /// <returns></returns>
        public virtual IQueryable<T> Query<T>(IStatelessSession session, Expression<Func<T, bool>> where, string entityName = "") where T : class
        {
			using (ITransaction tx1 = session.BeginTransaction())
				return GetQuery<T>(session, where,entityName);
        }
		public virtual IQueryable<T> GetQuery<T>(IStatelessSession session, Expression<Func<T, bool>> where, string entityName = "") where T : class
        {
            return string.IsNullOrEmpty(entityName) ? Filter(session.Query<T>(), where) : Filter(session.Query<T>(entityName), where);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual int Delete<T>(IStatelessSession session, Expression<Func<T, bool>> where) where T : class
        {
#if NET40 || NET45 || NET451 || NET452 || NET46
            return -1;
#else
            using (ITransaction tx = session.BeginTransaction())
                return Remove(session, where);
#endif
        }
		public virtual int Remove<T>(IStatelessSession session, Expression<Func<T, bool>> where) where T : class
        {
#if NET40 || NET45 || NET451 || NET452 || NET46
            return -1;
#else
			return Filter(session.Query<T>(), where).Delete();
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="where"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public virtual Task<int> DeleteAsync<T>(IStatelessSession session, Expression<Func<T, bool>> where, System.Threading.CancellationToken cancellation= default) where T : class
        {
#if NET40 || NET45 || NET451 || NET452 || NET46
            return new Task<int>(()=>-1) ;
#else
            using (ITransaction tx = session.BeginTransaction())
                return  RemoveAsync(session, where,cancellation);
#endif
        }
		
		public virtual Task<int> RemoveAsync<T>(IStatelessSession session, Expression<Func<T, bool>> where, System.Threading.CancellationToken cancellation= default) where T : class
        {
#if NET40 || NET45 || NET451 || NET452 || NET46
            return new Task<int>(()=>-1) ;
#else
			return  Filter(session.Query<T>(), where).DeleteAsync(cancellation);
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="where"></param>
        /// <param name="update"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public virtual   Task<int> UpdateAsync<T>(IStatelessSession session, Expression<Func<T, bool>> where, Expression<Func<T, T>> update, CancellationToken cancellation = default) where T : class
        {
#if NET40 || NET45 || NET451 || NET452 || NET46
            return new Task<int>(()=>-1) ;
#else
            using (ITransaction tx = session.BeginTransaction())
                return  ModifyAsync(session,where,update,cancellation);
#endif
        }
		  public virtual   Task<int> ModifyAsync<T>(IStatelessSession session, Expression<Func<T, bool>> where, Expression<Func<T, T>> update, CancellationToken cancellation = default) where T : class
        {
#if NET40 || NET45 || NET451 || NET452 || NET46
            return new Task<int>(()=>-1) ;
#else
			return  session.Query<T>().Where(where).UpdateAsync(update,cancellation);
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="where"></param>
        /// <param name="update"></param>
        /// <returns></returns>
        public virtual int Update<T>(IStatelessSession session, Expression<Func<T, bool>> where,Expression<Func<T,T>> update) where T : class
        {
#if NET40 || NET45 || NET451 || NET452 || NET46
            return -1 ;
#else
            using (ITransaction tx = session.BeginTransaction())
               return  Modify(session,where,update);
#endif
        }
		public virtual int  Modify<T>(IStatelessSession session, Expression<Func<T, bool>> where,Expression<Func<T,T>> update) where T : class
        {
#if NET40 || NET45 || NET451 || NET452 || NET46
            return -1 ;
#else
			return Filter(session.Query<T>(), where).Update(update);
#endif
        }
#endregion linq https://nhibernate.info/previous-doc/v5.1/ref/querylinq.html

#endregion IStatelessSession

        /// <summary>条件拼接 </summary>
        /// <param name="session"></param>
        /// <param name="ors">or条件</param>
        /// <param name="ands">and 条件</param>
        /// <returns>组装条件</returns>
        public static ICriteria QueryWhere<T>(ISession session, List<AbstractCriterion> ors,
            List<AbstractCriterion> ands) where T : class
        {
            ICriteria criteria = session.CreateCriteria<T>();
            return QueryWhere(criteria, ors, ands);
        }

        /// <summary>条件拼接 </summary>
        /// <param name="session"></param>
        /// <param name="ors">or条件</param>
        /// <param name="ands">and 条件</param>
        /// <returns>组装条件</returns>
        public static ICriteria QueryWhere<T>(IStatelessSession session, List<AbstractCriterion> ors,
            List<AbstractCriterion> ands) where T : class
        {
            ICriteria criteria = session.CreateCriteria<T>();
            return QueryWhere(criteria, ors, ands);
        }

        /// <summary>条件拼接 </summary>
        /// <param name="criteria"></param>
        /// <param name="ors">or条件</param>
        /// <param name="ands">and 条件</param>
        /// <returns>组装条件</returns>
        public static ICriteria QueryWhere(ICriteria criteria, List<AbstractCriterion> ors,
            List<AbstractCriterion> ands)
        {
            AbstractCriterion abstractCriterion = ors.Any() ? ors[0] : ands[0];
            for (int i = ors.Any() ? 0 : 1; i < ands.Count; i++)
            {
                abstractCriterion &= ands[i];
            }
            for (int i = 1; i < ors.Count; i++)
            {
                abstractCriterion |= ors[i];
            }

            criteria = criteria.Add(abstractCriterion);
            return criteria;
        }

        /// <summary>根据条件查询实体类数据集数量信息</summary>
        /// <param name="where">条件</param>
        /// <param name="session"></param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集数量信息</return>
        public virtual  Task<int> CountAsync<T>(ISession session,ICriteria where = null, CancellationToken cancellationToken = default) where T : class
        {
            using (ITransaction tx = session.BeginTransaction())
			{
				return GetCountAsync<T>(session,where,cancellationToken);
			}
        }
		
	    public virtual  Task<int> GetCountAsync<T>(ISession session,ICriteria where = null, CancellationToken cancellationToken = default) where T : class
        {
            if (where != null)
            {
#if NET40 || NET45 || NET451 || NET452 || NET46
				var count = where.SetProjection(Projections.RowCount()).UniqueResult<int>();
				return new Task<int>(() => count);
#else
				var count =  where.SetProjection(Projections.RowCount()).UniqueResultAsync<int>(cancellationToken);
				return count;
#endif
            }
            else
            {
#if NET40 || NET45 || NET451 || NET452 || NET46
                var count =  GetCount<T>(session, (Expression<Func<T, bool>>)null);
                return new Task<int>(()=>count);
#else
                var count =  GetCountAsync<T>(session, (Expression<Func<T, bool>>)null, cancellationToken);
                return count;
#endif
            }
        }

        /// <summary>根据条件查询实体类数据集数量信息</summary>
        /// <param name="where">条件</param>
        /// <param name="session"></param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集数量信息</return>
        public virtual  Task<int> CountAsync<T>(IStatelessSession session,ICriteria where = null, CancellationToken cancellationToken = default) where T : class
        {
            using (ITransaction tx = session.BeginTransaction())
			{
				return GetCountAsync<T>(session,where,cancellationToken);
			}
        }
		
		public virtual  Task<int> GetCountAsync<T>(IStatelessSession session,ICriteria where = null, CancellationToken cancellationToken = default) where T : class
        {
            if (where != null)
            {
#if NET40 || NET45 || NET451 || NET452 || NET46
				var count = where.SetProjection(Projections.RowCount()).UniqueResult<int>();
				return new Task<int>(() => count);
#else
				var count =  where.SetProjection(Projections.RowCount()).UniqueResultAsync<int>(cancellationToken);
				return count;
#endif
            }
            else
            {
#if NET40 || NET45 || NET451 || NET452 || NET46
                var count =  GetCount<T>(session, (Expression<Func<T, bool>>)null);
                return new Task<int>(()=>count);
#else
                var count =  GetCountAsync<T>(session, (Expression<Func<T, bool>>)null, cancellationToken);
                return count;
#endif
            }
        }


        /// <summary>根据条件查询实体类数据集数量信息</summary>
        /// <param name="where">条件</param>
        /// <param name="session"></param>
        ///<return>返回实体类数据集数量信息</return>

        public virtual int Count<T>(ISession session,ICriteria where = null) where T : class
        {
            using (ITransaction tx = session.BeginTransaction())
			{
				return GetCount<T>(session,where);
			}
        }
		
		public virtual int GetCount<T>(ISession session,ICriteria where = null) where T : class
        {
            if (where != null)
            {
                var count = where.SetProjection(Projections.RowCount()).UniqueResult<int>();
                return count;
            }
            else
            {
                var count = this.GetCount<T>(session,(Expression<Func<T, bool>>)null);
                return count;
            }
        }
        /// <summary>根据条件查询实体类数据集数量信息</summary>
        /// <param name="where">条件</param>
        /// <param name="session"></param>
        ///<return>返回实体类数据集数量信息</return>

        public virtual int Count<T>(IStatelessSession session, ICriteria where = null) where T : class
        {
             using (ITransaction tx = session.BeginTransaction())
			{
				return GetCount<T>(session,where);
			}
        }
		
		public virtual int GetCount<T>(IStatelessSession session,ICriteria where = null) where T : class
        {
            if (where != null)
            {
                var count = where.SetProjection(Projections.RowCount()).UniqueResult<int>();
                return count;
            }
            else
            {
                var count = this.GetCount<T>(session,(Expression<Func<T, bool>>)null);
                return count;
            }
        }

        /// <summary>根据条件查询实体类数据集信息</summary>
        /// <param name="where">条件</param>
        /// <param name="session"></param>
        ///<return>返回实体类数据集信息</return>
        public virtual List<T> FindList<T>(ISession session,ICriteria where = null) where T : class
        {
            using (ITransaction tx = session.BeginTransaction())
			{
				var data = GetList<T>(session,where);
				return data;
			}
        }
		public virtual List<T> GetList<T>(ISession session,ICriteria where = null) where T : class
        {
            if (where != null)
            {
                var data = where.List<T>().ToList();
			    return data;
            }
            else
            {
                var data = this.GetQuery<T>(session,(Expression<Func<T, bool>>)null).ToList();
				return data;
            }
        }
        /// <summary>根据条件查询实体类数据集信息</summary>
        /// <param name="where">条件</param>
        /// <param name="session"></param>
        ///<return>返回实体类数据集信息</return>
        public virtual List<T> FindList<T>(IStatelessSession session, ICriteria where = null) where T : class
        {
            using (ITransaction tx = session.BeginTransaction())
			{
				var data = GetList<T>(session,where);
				return data;
			}
        }
		public virtual List<T> GetList<T>(IStatelessSession session,ICriteria where = null) where T : class
        {
            if (where != null)
            {
                var data = where.List<T>().ToList();
			    return data;
            }
            else
            {
                var data = this.GetQuery<T>(session,(Expression<Func<T, bool>>)null).ToList();
				return data;
            }
        }
        /// <summary>根据条件及分页查询实体类数据集信息</summary>
        /// <param name="where">条件</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        /// <param name="session"></param>
        ///<return>返回实体类数据集信息</return>

        public virtual List<T> FindListByPage<T>(ISession session, ICriteria where = null, int page = 1, int size = 10) where T : class
        {
            using (ITransaction tx = session.BeginTransaction())
			{
				return GetListByPage<T>(session,where,page,size);
			}
        }
		public virtual List<T> GetListByPage<T>(ISession session, ICriteria where = null, int page = 1, int size = 10) where T : class
        {
            PageHelper.Set(ref page, ref size);
            if (where != null)
            {
                var data = where.SetFirstResult((page - 1) * size).SetMaxResults(size).List<T>().ToList();
                return data;
            }
            else
            {
				var data = this.GetQuery<T>(session,(Expression<Func<T, bool>>)null).Skip((page - 1) * size).Take(size).ToList();// 修改后 查询  修改的数据 都是默认值(null or "" ) cs 模式下 
				//var data = session.Query<T>().Skip((page - 1) * size).Take(size).ToList();//修改后 查询异常
				return data;
            }
        }
        /// <summary>根据条件及分页查询实体类数据集信息</summary>
        /// <param name="where">条件</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        /// <param name="session"></param>
        ///<return>返回实体类数据集信息</return>

        public virtual List<T> FindListByPage<T>(IStatelessSession session, ICriteria where = null, int page = 1, int size = 10) where T : class
        {
            using (ITransaction tx = session.BeginTransaction())
			{
				return GetListByPage<T>(session,where,page,size);
			}
        }
		public virtual List<T> GetListByPage<T>(IStatelessSession session, ICriteria where = null, int page = 1, int size = 10) where T : class
        {
            PageHelper.Set(ref page, ref size);
            if (where != null)
            {
                var data = where.SetFirstResult((page - 1) * size).SetMaxResults(size).List<T>().ToList();
                return data;
            }
            else
            {
				var data = this.GetQuery<T>(session,(Expression<Func<T, bool>>)null).Skip((page - 1) * size).Take(size).ToList();// 修改后 查询  修改的数据 都是默认值(null or "" ) cs 模式下 
				//var data = session.Query<T>().Skip((page - 1) * size).Take(size).ToList();//修改后 查询异常
				return data;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="where"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public virtual Tuple<List<T>, int> FindTupleByPage<T>(IStatelessSession session,ICriteria where = null, int page = 1, int size = 10) where T : class
        {
            using (ITransaction tx = session.BeginTransaction())
			{
				return GetTupleByPage<T>(session,where,page,size);
			}
        }
		public virtual Tuple<List<T>, int> GetTupleByPage<T>(IStatelessSession session,ICriteria where = null, int page = 1, int size = 10) where T : class
        {
            PageHelper.Set(ref page, ref size);
            var datas = GetListByPage<T>(session,where, page, size);
            var count = GetCount<T>(session,where!=null ? (ICriteria)where.Clone():null);
            return new Tuple<List<T>, int>(datas, count);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="where"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>

        public virtual Tuple<List<T>, int> FindTupleByPage<T>(ISession session, ICriteria where = null, int page = 1, int size = 10) where T : class
        {
            using (ITransaction tx = session.BeginTransaction())
			{
				return GetTupleByPage<T>(session,where,page,size);
			}
        }
		
	    public virtual Tuple<List<T>, int> GetTupleByPage<T>(ISession session,ICriteria where = null, int page = 1, int size = 10) where T : class
        {
            PageHelper.Set(ref page, ref size);
            var datas = GetListByPage<T>(session,where, page, size);
            var count = GetCount<T>(session,where!=null ? (ICriteria)where.Clone():null);
            return new Tuple<List<T>, int>(datas, count);
        }
#region async



        /// <summary>根据条件查询实体类数据集信息</summary>
        /// <param name="where">条件</param>
        /// <param name="session"></param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息</return>

        public virtual  Task<List<T>> FindListAsync<T>(ISession session,ICriteria where = null,
            CancellationToken cancellationToken = default) where T : class
        {
            using (ITransaction tx = session.BeginTransaction())
			{
				return GetListAsync<T>(session,where,cancellationToken);
			}
        }
		public virtual  Task<List<T>> GetListAsync<T>(ISession session,ICriteria where = null,
            CancellationToken cancellationToken = default) where T : class
        {
            if (where != null)
            {

#if NET40 || NET45 || NET451 || NET452 || NET46
                 var data =  (IList<T>)where.List();
                return new Task<List<T>>(()=> data.ToList());
#else
                var data =  where.ListAsync<T>(cancellationToken).Result;
                return new Task<List<T>>(()=>data.ToList());
#endif
            }
            else
            {
#if NET40 || NET45 || NET451 || NET452 || NET46
                var data =  this.GetQuery<T>(session, (Expression<Func<T, bool>>)null).ToList();
                return new Task<List<T>>(()=> data);
#else
				var data =  this.GetQuery<T>(session,(Expression<Func<T, bool>>)null).ToListAsync(cancellationToken);
				return data;
#endif
            }
        }

        /// <summary>根据条件查询实体类数据集信息</summary>
        /// <param name="where">条件</param>
        /// <param name="session"></param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息</return>

        public virtual  Task<List<T>> FindListAsync<T>(IStatelessSession session,ICriteria where = null,
            CancellationToken cancellationToken = default) where T : class
        {
            using (ITransaction tx = session.BeginTransaction())
			{
				return GetListAsync<T>(session,where,cancellationToken);
			}
        }
		public virtual  Task<List<T>> GetListAsync<T>(IStatelessSession session,ICriteria where = null,
            CancellationToken cancellationToken = default) where T : class
        {
            if (where != null)
            {

#if NET40 || NET45 || NET451 || NET452 || NET46
                 var data =  (IList<T>)where.List();
                return new Task<List<T>>(()=> data.ToList());
#else
                var data =  where.ListAsync<T>(cancellationToken).Result;
                return new Task<List<T>>(()=>data.ToList());
#endif
            }
            else
            {
#if NET40 || NET45 || NET451 || NET452 || NET46
                var data =  this.GetQuery<T>(session, (Expression<Func<T, bool>>)null).ToList();
                return new Task<List<T>>(()=> data);
#else
				var data =  this.GetQuery<T>(session,(Expression<Func<T, bool>>)null).ToListAsync(cancellationToken);
				return data;
#endif
            }
        }
        /// <summary>根据条件及分页查询实体类数据集信息</summary>
        /// <param name="where">条件</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        /// <param name="session"></param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息</return>
        public virtual Task<List<T>> FindListByPageAsync<T>(ISession session,ICriteria where = null, int page=1, int size=10,
            CancellationToken cancellationToken = default) where T : class
        {
			using (ITransaction tx = session.BeginTransaction())
			{
				return GetListByPageAsync<T>(session,where,page,size,cancellationToken);
			}
        }
		public virtual Task<List<T>> GetListByPageAsync<T>(ISession session,ICriteria where = null, int page=1, int size=10,
            CancellationToken cancellationToken = default) where T : class
        {
            PageHelper.Set(ref page, ref size);
            if (where != null)
            {
#if NET40 || NET45 || NET451 || NET452 || NET46
                var data =  (IList<T>) where.SetFirstResult((page - 1) * size).SetMaxResults(size).List();
                return new Task<List<T>>(()=> data.ToList());
#else
                var data =  where.SetFirstResult((page - 1) * size).SetMaxResults(size).ListAsync<T>(cancellationToken).Result;
                return new Task<List<T>>(()=>data.ToList());
#endif
            }
            else
            {
#if NET40 || NET45 || NET451 || NET452 || NET46
			   var data =  this.GetQuery<T>(session, (Expression<Func<T, bool>>)null).Skip((page - 1) * size).Take(size).ToList();
			   return new Task<List<T>>(()=> data);
#else
			   var data =  this.GetQuery<T>(session, (Expression<Func<T, bool>>)null).Skip((page - 1) * size).Take(size).ToListAsync(cancellationToken);
			   return data;
#endif
            }
        }

        /// <summary>根据条件及分页查询实体类数据集信息</summary>
        /// <param name="where">条件</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        /// <param name="session"></param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息</return>
       public virtual Task<List<T>> FindListByPageAsync<T>(IStatelessSession session,ICriteria where = null, int page=1, int size=10,
            CancellationToken cancellationToken = default) where T : class
        {
			using (ITransaction tx = session.BeginTransaction())
			{
				return GetListByPageAsync<T>(session,where,page,size,cancellationToken);
			}
        }
		public virtual Task<List<T>> GetListByPageAsync<T>(IStatelessSession session,ICriteria where = null, int page=1, int size=10,
            CancellationToken cancellationToken = default) where T : class
        {
            PageHelper.Set(ref page, ref size);
            if (where != null)
            {
#if NET40 || NET45 || NET451 || NET452 || NET46
                var data =  (IList<T>) where.SetFirstResult((page - 1) * size).SetMaxResults(size).List();
                return new Task<List<T>>(()=> data.ToList());
#else
                var data =  where.SetFirstResult((page - 1) * size).SetMaxResults(size).ListAsync<T>(cancellationToken).Result;
                return new Task<List<T>>(()=>data.ToList());
#endif
            }
            else
            {
#if NET40 || NET45 || NET451 || NET452 || NET46
			   var data =  this.GetQuery<T>(session, (Expression<Func<T, bool>>)null).Skip((page - 1) * size).Take(size).ToList();
			   return new Task<List<T>>(()=> data);
#else
			   var data =  this.GetQuery<T>(session, (Expression<Func<T, bool>>)null).Skip((page - 1) * size).Take(size).ToListAsync(cancellationToken);
			   return data;
#endif
            }
        }
        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息</summary>
        /// <param name="where">条件</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        /// <param name="session"></param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
        public virtual Task<Tuple<List<T>, int>> FindTupleByPageAsync<T>(ISession session, ICriteria where = null, int page=1,
            int size=10,  CancellationToken cancellationToken = default) where T : class
        {
            using (ITransaction tx = session.BeginTransaction())
			{
				return GetTupleByPageAsync<T>(session,where,page,size,cancellationToken);
			}
        }
		 public virtual
#if !(NET40 || NET45 || NET451 || NET452 || NET46)
            async
#endif
            Task<Tuple<List<T>, int>> GetTupleByPageAsync<T>(ISession session, ICriteria where = null, int page=1,
            int size=10,  CancellationToken cancellationToken = default) where T : class
        {
            PageHelper.Set(ref page, ref size);
#if NET40 || NET45 || NET451 || NET452 || NET46
            var da =  GetListByPage<T>(session, where, page, size);
            ICriteria pageCriteria = where!=null ? (ICriteria)where.Clone():null;
            var count =  GetCount<T>(session,pageCriteria);
            return  new Task<Tuple<List<T>, int>>(()=>new Tuple<List<T>, int>(da, count));
#else
            var da = await GetListByPageAsync<T>(session, where, page, size, cancellationToken);
            ICriteria pageCriteria = where!=null ? (ICriteria)where.Clone():null;
            var count = await GetCountAsync<T>(session,pageCriteria, cancellationToken);
            return new Tuple<List<T>, int>(da, count);
#endif
        }

        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息</summary>
        /// <param name="session"></param>
        /// <param name="where">条件</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
         public virtual Task<Tuple<List<T>, int>> FindTupleByPageAsync<T>(IStatelessSession session, ICriteria where = null, int page=1,
            int size=10,  CancellationToken cancellationToken = default) where T : class
        {
            using (ITransaction tx = session.BeginTransaction())
			{
				return GetTupleByPageAsync<T>(session,where,page,size,cancellationToken);
			}
        }
		 public virtual
#if !(NET40 || NET45 || NET451 || NET452 || NET46)
            async
#endif
            Task<Tuple<List<T>, int>> GetTupleByPageAsync<T>(IStatelessSession session, ICriteria where = null, int page=1,
            int size=10,  CancellationToken cancellationToken = default) where T : class
        {
            PageHelper.Set(ref page, ref size);
#if NET40 || NET45 || NET451 || NET452 || NET46
            var da =  GetListByPage<T>(session, where, page, size);
            ICriteria pageCriteria = where!=null ? (ICriteria)where.Clone():null;
            var count =  GetCount<T>(session,pageCriteria);
            return  new Task<Tuple<List<T>, int>>(()=>new Tuple<List<T>, int>(da, count));
#else
            var da = await GetListByPageAsync<T>(session, where, page, size, cancellationToken);
            ICriteria pageCriteria =where!=null ? (ICriteria)where.Clone():null;
            var count = await GetCountAsync<T>(session,pageCriteria, cancellationToken);
            return new Tuple<List<T>, int>(da, count);
#endif
        }
#endregion async


        /// <summary>查找单个</summary>
        /// <param name="session"></param>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public virtual T FindSingle<T>(ISession session, Expression<Func<T, bool>> where = null) where T : class
        {
            using (ITransaction tx = session.BeginTransaction())
			{
				return Get<T>(session,where);
			}
        }

		public virtual T Get<T>(ISession session, Expression<Func<T, bool>> where = null) where T : class
        {
            return this.GetQuery<T>(session,where).FirstOrDefault() ;
        }
        /// <summary>查找单个</summary>
        /// <param name="session"></param>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public virtual T FindSingle<T>(IStatelessSession session, Expression<Func<T, bool>> where = null) where T : class
        {
            using (ITransaction tx = session.BeginTransaction())
			{
				return Get<T>(session,where);
			}
        }
		public virtual T Get<T>(IStatelessSession session, Expression<Func<T, bool>> where = null) where T : class
        {
            return this.GetQuery<T>(session,where).FirstOrDefault() ;
        }

        /// <summary> 查询数据 </summary>
        /// <param name="session"></param>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public virtual IQueryable<T> Find<T>(ISession session, Expression<Func<T, bool>> where = null) where T : class
        {
            return this.Query<T>(session,where) ;
        }

        /// <summary> 查询数据 </summary>
        /// <param name="session"></param>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public virtual IQueryable<T> Find<T>(IStatelessSession session, Expression<Func<T, bool>> where = null) where T : class
        {
             return this.Query<T>(session,where) ;
        }


        /// <summary> 查询数据  </summary>
        /// <param name="session"></param>
        /// <param name="where">条件</param>
        /// <param name="page">页数</param>
        /// <param name="size">记录</param>
        /// <returns></returns>
        public virtual IQueryable<T> FindByPage<T>(ISession session, Expression<Func<T, bool>> where = null, int page = 1, int size = 10) where T : class
        {
            using (ITransaction tx = session.BeginTransaction())
			{
				return GetByPage<T>(session,where,page,size);
			}
        }
		public virtual IQueryable<T> GetByPage<T>(ISession session, Expression<Func<T, bool>> where = null, int page = 1, int size = 10) where T : class
        {
            PageHelper.Set(ref page, ref size);
            return this.GetQuery<T>(session, where).Skip((page - 1) * size).Take(size) ;
        }
        /// <summary> 查询数据  </summary>
        /// <param name="session"></param>
        /// <param name="where">条件</param>
        /// <param name="page">页数</param>
        /// <param name="size">记录</param>
        /// <returns></returns>
        public virtual IQueryable<T> FindByPage<T>(IStatelessSession session, Expression<Func<T, bool>> where = null, int page = 1, int size = 10) where T : class
        {
            using (ITransaction tx = session.BeginTransaction())
			{
				return GetByPage<T>(session,where,page,size);
			}
        }
		public virtual IQueryable<T> GetByPage<T>(IStatelessSession session, Expression<Func<T, bool>> where = null, int page = 1, int size = 10) where T : class
        {
            PageHelper.Set(ref page, ref size);
            return this.GetQuery<T>(session, where).Skip((page - 1) * size).Take(size) ;
        }
#region async

        /// <summary>查找单个 </summary>
        /// <param name="session"></param>
        /// <param name="where">条件</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task<T> FindSingleAsync<T>(ISession session, Expression<Func<T, bool>> where = null, CancellationToken cancellationToken = default) where T : class
        {
			using (ITransaction tx = session.BeginTransaction())
			{
				return GetAsync<T>(session,where,cancellationToken);
			}
        }
		public virtual Task<T> GetAsync<T>(ISession session, Expression<Func<T, bool>> where = null, CancellationToken cancellationToken = default) where T : class
        {
#if NET40 || NET45 || NET451 || NET452 || NET46
            var res=this.GetQuery<T>(session, where).FirstOrDefault();
            return new Task<T>(()=>res);
#else
            return this.GetQuery<T>(session, where).FirstOrDefaultAsync(cancellationToken) ;
#endif
        }

        /// <summary> 查找单个  </summary>
        /// <param name="session"></param>
        /// <param name="where">条件</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task<T> FindSingleAsync<T>(IStatelessSession session,Expression<Func<T, bool>> where = null, CancellationToken cancellationToken = default) where T : class
        {
			using (ITransaction tx = session.BeginTransaction())
			{
				return GetAsync<T>(session,where,cancellationToken);
			}
        }
		public virtual Task<T> GetAsync<T>(IStatelessSession session, Expression<Func<T, bool>> where = null, CancellationToken cancellationToken = default) where T : class
        {
#if NET40 || NET45 || NET451 || NET452 || NET46
            var res=this.GetQuery<T>(session, where).FirstOrDefault();
            return new Task<T>(()=>res);
#else
            return this.GetQuery<T>(session, where).FirstOrDefaultAsync(cancellationToken) ;
#endif
        }
        /// <summary> 是否存在 </summary>
        /// <param name="session"></param>
        /// <param name="where">条件</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task<bool> IsExistAsync<T>(ISession session, Expression<Func<T, bool>> where, CancellationToken cancellationToken = default) where T : class
        {
			using (ITransaction tx = session.BeginTransaction())
			{
				return HasExistAsync<T>(session,where,cancellationToken);
			}
        }
		public virtual 
#if !(NET40 || NET45 || NET451 || NET452 || NET46)
        async
#endif
        Task<bool> HasExistAsync<T>(ISession session, Expression<Func<T, bool>> where, CancellationToken cancellationToken = default) where T : class
        {
#if (NET40 || NET45 || NET451 || NET452 || NET46)
          var res= this.GetCount<T>(session, where) >= 1;
           return new Task<bool>(()=>res);
#else
            return await this.GetCountAsync<T>(session, where) >= 1;
#endif
        }

        /// <summary> 是否存在 </summary>
        /// <param name="session"></param>
        /// <param name="where">条件</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual    Task<bool> IsExistAsync<T>(IStatelessSession session, Expression<Func<T, bool>> where, CancellationToken cancellationToken = default) where T : class
        {
			using (ITransaction tx = session.BeginTransaction())
			{
				return HasExistAsync<T>(session,where,cancellationToken);
			}
        }
		public virtual 
#if !(NET40 || NET45 || NET451 || NET452 || NET46)
        async
#endif
        Task<bool> HasExistAsync<T>(IStatelessSession session, Expression<Func<T, bool>> where, CancellationToken cancellationToken = default) where T : class
        {
#if (NET40 || NET45 || NET451 || NET452 || NET46)
          var res= this.GetCount<T>(session, where) >= 1;
           return new Task<bool>(()=>res);
#else
            return await this.GetCountAsync<T>(session, where) >= 1;
#endif
        }

        /// <summary> 查询数据 </summary>
        /// <param name="session"></param>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public virtual Task<IQueryable<T>> FindAsync<T>(ISession session, Expression<Func<T, bool>> where = null) where T : class
        {
            var res =this.Query<T>(session, where);
            return new Task<IQueryable<T>>(()=>res);
        }

        /// <summary> 查询数据 </summary>
        /// <param name="session"></param>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public virtual Task<IQueryable<T>> FindAsync<T>(IStatelessSession session,Expression<Func<T, bool>> where = null) where T : class
        {
            var res = this.Query<T>(session,where) ;
            return new Task<IQueryable<T>>(()=>res);
        }

        /// <summary> 查询数据  </summary>
        /// <param name="session"></param>
        /// <param name="where">条件</param>
        /// <param name="page">页数</param>
        /// <param name="size">记录</param>
        /// <returns></returns>
        public virtual Task<IQueryable<T>> FindByPageAsync<T>(ISession session, Expression<Func<T, bool>> where = null, int page = 1, int size = 10) where T : class
        {
            PageHelper.Set(ref page, ref size);
            var res =  this.Query<T>(session, where).Skip((page - 1) * size).Take(page) ;
            return new Task<IQueryable<T>>(()=>res);
        }

        /// <summary> 查询数据  </summary>
        /// <param name="session"></param>
        /// <param name="where">条件</param>
        /// <param name="page">页数</param>
        /// <param name="size">记录</param>
        /// <returns></returns>
        public virtual Task<IQueryable<T>> FindByPageAsync<T>(IStatelessSession session, Expression<Func<T, bool>> where = null, int page = 1, int size = 10) where T : class
        {
            PageHelper.Set(ref page, ref size);
            var res = this.Query<T>(session,where).Skip((page - 1) * size).Take(page);
            return new Task<IQueryable<T>>(()=>res);
        }
#endregion async

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="where"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public virtual Tuple<List<T>, int> FindTupleByPage<T>(ISession session,Expression<Func<T, bool>> where = null, int page = 1, int size = 10) where T : class
        {
            using (ITransaction tx = session.BeginTransaction())
			{
				return GetTupleByPage<T>(session,where,page,size);
			}
        }
		public virtual Tuple<List<T>, int> GetTupleByPage<T>(ISession session,Expression<Func<T, bool>> where = null, int page = 1, int size = 10) where T : class
        {
            PageHelper.Set(ref page, ref size);
            List<T> data =  this.GetQuery<T>(session, where).Skip((page - 1) * page).Take(size).ToList();
            int count = this.GetCount(session, where);
            return new Tuple<List<T>, int>(data, count);
        }

       /// <summary>
       /// 
       /// </summary>
       /// <typeparam name="T"></typeparam>
       /// <param name="session"></param>
       /// <param name="where"></param>
       /// <param name="page"></param>
       /// <param name="size"></param>
       /// <returns></returns>
        public virtual Tuple<List<T>, int> FindTupleByPage<T>(IStatelessSession session, Expression<Func<T, bool>> where = null, int page = 1, int size = 10) where T : class
        {
            using (ITransaction tx = session.BeginTransaction())
			{
				return GetTupleByPage<T>(session,where,page,size);
			}
        }
	    public virtual Tuple<List<T>, int> GetTupleByPage<T>(IStatelessSession session,Expression<Func<T, bool>> where = null, int page = 1, int size = 10) where T : class
        {
            PageHelper.Set(ref page, ref size);
            List<T> data =  this.GetQuery<T>(session, where).Skip((page - 1) * page).Take(size).ToList();
            int count = this.GetCount(session, where);
            return new Tuple<List<T>, int>(data, count);
        }
    }
}
#endif