//#if !(NET10 || NET11 || NET20 || NET30 || NET35 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
#if NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1

#if NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48
using System.Data.Entity;
#else
using Microsoft.EntityFrameworkCore;
#endif
using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;
using System.Collections.Generic;
using Utility.Application.Services.Dtos;
using Utility.Extensions;
using Utility.Page;
using Utility.Threads;
using Utility.Domain.Entities;
using Utility.Domain.Extensions;

namespace Utility.Ef
{

    /// <summary>
    /// 
    /// </summary>
    public class EfTemaplate:IDisposable
    {

        /// <summary>
        /// 
        /// </summary>
        protected readonly DbContext context;//数据库上下文
   
		/// <summary>
        /// 是否启用 
        /// </summary>
        internal bool Enable{ get; set; } = true;
		
        /// <summary> 构造函数注入</summary>
        /// <param name="context">数据库上下文</param>
        public EfTemaplate(DbContext context)
        {
            this.context = context;
            this.Thorw = false;
        }


        /// <summary>未实现是抛出异常还是不做任何操作 </summary>
        public virtual bool Thorw { get; }

        /// <summary>数据库连接对象 </summary>
        public virtual IDbConnection Connection =>
#if NET20 || NET30 || NET35 || NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48
        context.Database.Connection;
#else
        context.Database.GetDbConnection();
#endif

        /// <summary>
        /// 手动赋值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="flag">1 add 2 update 3 delete</param>
        protected virtual bool UpdateValue<T>(T entity, int flag = 1) where T : class
        {
            return Enable?entity.UpdateValue(flag):false;
        }

        /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public virtual int Delete<T, Key>(Key id) where T : class
        {
            //语法不支持
            //UnitWork.Delete<Entity>(it => it.Id == id);
            Expression<Func<T, bool>> where = LinqExpression.IdEqual<T, Key>(id);
            if (Enable&&typeof(T).IsAssignableFrom(typeof(IHasDeletionTime)))
            {
                where = where.Filter();
                T obj = FindSingle<T>(where);
                if (obj != null)
                {
                    if (obj is IHasDeletionTime deletionTime)
                    {
                        deletionTime.DeletionTime = DateTime.Now;
                        deletionTime.IsDeleted = true;
                    }
                    Update(obj,2);
					Save();
                }
                return 1;
            }
            else
            {
               
                this.Delete<T>(where);
                return 1;
            }
           
        }

        /// <summary>根据id删除实体类信息(多删除)</summary>
        /// <param name="ids">id</param>
        ///<return>返回删除结果(多删除),大于0 返回删除成功(多删除),小于等于0 返回删除失败(多删除) </return>
        public virtual int DeleteList<T, Key>(Key[] ids) where T : class
        {
            //语法不支持
            //Expression<Func<T, bool>> where = null;
            //foreach (var id in ids)
            //{
            //    where = where.Or(it => it.Id == id);
            //}
            //Repository.Delete(where);
            //return 1;
             Expression<Func<T, bool>> where = null;
			foreach (var id in ids)
			{
				where = where.Or(id.IdEqual<T, Key>());
			}
            if (Enable&&typeof(T).IsAssignableFrom(typeof(IHasDeletionTime)))
            {
                List<T> objs = FindList<T>(where);
                foreach (var item in objs)
                {
                    if (item is IHasDeletionTime deletionTime &&!deletionTime.IsDeleted)
                    {
                        deletionTime.DeletionTime = DateTime.Now;
                        deletionTime.IsDeleted = true;
                    }
                    Update(item,2);
                }
				Save();
                return 1;
            }
            else
            {
                this.Delete(where);
                return 1;
            }
          
        }
        /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public virtual Task<int> DeleteAsync<T, Key>(Key id, CancellationToken cancellationToken = default)
            where T:class
        {
            var res = Delete<T,Key>(id);
            return new Task<int>(()=>res);
        }

        /// <summary>根据id删除实体类信息(多删除)</summary>
        /// <param name="ids">id</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回删除结果(多删除),大于0 返回删除成功(多删除),小于等于0 返回删除失败(多删除) </return>
        public virtual Task<int> DeleteListAsync<T, Key>(Key[] ids, CancellationToken cancellationToken = default)
            where T:class
        {
            var res = DeleteList<T,Key>(ids);
            return new Task<int>(()=>res);
        }



        /// <summary>查找单个，且不被上下文所跟踪 </summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public virtual T FindSingle<T>(Expression<Func<T, bool>> where=null) where T : class
        {
            where =Enable? where.Filter():where;
            return where == null ? context.Set<T>().AsNoTracking().FirstOrDefault() : context.Set<T>().AsNoTracking().FirstOrDefault(where);
        }

        /// <summary> 是否存在 </summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public virtual bool IsExist<T>(Expression<Func<T, bool>> where = null) where T : class
        {
            where =Enable? where.Filter():where;
            return where == null ? context.Set<T>().Any() : context.Set<T>().Any(where);
        }

        /// <summary>根据过滤条件，获取记录 </summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public virtual IQueryable<T> Find<T>(Expression<Func<T, bool>> where = null) where T : class
        {
            where =Enable? where.Filter():where;
            return Filter(where);
        }

        /// <summary>根据过滤条件，获取记录 </summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public virtual List<T> FindList<T>(Expression<Func<T, bool>> where = null) where T : class
        {
            where = where.Filter();
            return Filter(where).ToList();
        }

        /// <summary> 分页记录 </summary>
        /// <param name="page">页数</param>
        /// <param name="size">记录</param>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public virtual IQueryable<T> FindByPage<T>(Expression<Func<T, bool>> where = null,int page=1, int size=10) where T : class
        {
            PageHelper.Set(ref page, ref size);
            where =Enable? where.Filter():where;
            return Filter(where).Skip(size * (page - 1)).Take(size);
        }

        /// <summary> 分页记录 </summary>
        /// <param name="page">页数</param>
        /// <param name="size">记录</param>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public virtual List<T> FindListByPage<T>(Expression<Func<T, bool>> where = null, int page = 1, int size = 10) where T : class
        {
            PageHelper.Set(ref page, ref size);
            where =Enable? where.Filter():where;
            return Filter(where).Skip(size * (page - 1)).Take(size).ToList();
        }

        /// <summary> 分页记录 </summary>
        /// <param name="page">页数</param>
        /// <param name="size">记录</param>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public virtual Tuple<List<T>, int> FindTupleByPage<T>(Expression<Func<T, bool>> where = null, int page = 1, int size = 10) where T : class
        {
            PageHelper.Set(ref page, ref size);
			where =Enable? where.Filter():where;
            var datas = FindByPage(where,page, size ).ToList();
            var count = Count(where);
            var result = new Tuple<List<T>, int>(datas, count);
            return result;
        }

        /// <summary> 分页记录 </summary>
        /// <param name="page">页数</param>
        /// <param name="size">记录</param>
        /// <param name="where">条件</param>
        /// <returns></returns>

        public virtual ResultDto<T> FindResultDtoByPage<T>(Expression<Func<T, bool>> where = null, int page = 1, int size = 10) where T : class
        {
			where =Enable? where.Filter():where;
            PageHelper.Set(ref page, ref size);
            var tuple = FindTupleByPage(where, page, size);
            var result = new ResultDto<T>(tuple, page, size);
            return result;
        }

        /// <summary> 分页记录 </summary>
        /// <param name="page">页数</param>
        /// <param name="size">记录</param>
        /// <param name="where">条件</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task<ResultDto<T>> FindResultDtoByPageAsync<T>(Expression<Func<T, bool>> where = null, int page = 1, int size = 10,
         CancellationToken cancellationToken = default) where T : class
        {
			where =Enable? where.Filter():where;
            PageHelper.Set(ref page, ref size);
            var tuple = FindTupleByPageAsync(where,page,size,cancellationToken).Result;
            var result = new ResultDto<T>(tuple, page, size);
            return new Task<ResultDto<T>>(() => result);
        }

        /// <summary> 分页记录 </summary>
        /// <param name="page">页数</param>
        /// <param name="size">记录</param>
        /// <param name="where">条件</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task<Tuple<List<T>, int>> FindTupleByPageAsync<T>(Expression<Func<T, bool>> where = null, int page = 1, int size = 10,
            CancellationToken cancellationToken = default)where T:class
        {
			where =Enable? where.Filter():where;
            PageHelper.Set(ref page, ref size);
#if NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
            var datas =  FindByPage(where, page,size ).ToListAsync(cancellationToken).Result;
            var count =  CountAsync(where, cancellationToken).Result;
#else
            var datas = FindByPage(where, page,size ).ToList();
            var count = Count(where);
#endif

            var result =   new Tuple<List<T>, int>(datas, count);
            return new Task<Tuple<List<T>, int>>(() => result);
        }

        /// <summary> 根据过滤条件获取记录数 </summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public virtual int Count<T>(Expression<Func<T, bool>> where = null) where T : class
        {
            where =Enable? where.Filter():where;
            return Filter(where).Count();
        }

        /// <summary> 添加 </summary>
        /// <param name="entity">实体</param>
        public virtual object Insert<T>(T entity) where T : class
        {
            UpdateValue(entity);
            context.Set<T>().Add(entity);
            Save();
            context.Entry(entity).State = EntityState.Detached;
            return 1;
        }

        /// <summary> 批量添加</summary>
        /// <param name="entities">The entities.</param>
        public virtual void BatchInsert<T>(T[] entities) where T : class
        {
            foreach (var item in entities)
            {
                UpdateValue(item);
            }
            context.Set<T>().AddRange(entities);
            //foreach (var item in entities)
            //{
            //    Microsoft.EntityFrameworkCore.DbContext context = (Microsoft.EntityFrameworkCore.DbContext)Activator.CreateInstance(DbContext.GetType())
            //    context.Add(item);
            //    context.SaveChanges();
            //    context = null;
            //}
            Save();
        }

        /// <summary> 更新一个实体的所有属性 </summary>
        /// <param name="entity">实体</param>
        public virtual void Update<T>(T entity) where T : class
        {
            if(Update(entity,2))
			{
				Save();
			}
        }

        /// <summary> 更新一个实体的所有属性 </summary>
        /// <param name="entity">实体</param>
        /// <param name="flag"></param>
        public virtual bool Update<T>(T entity,int flag=2) where T : class
        {
			if(Enable)
			{
				UpdateValue(entity, flag);
			}
            var entry = this.context.Entry(entity);
            //entry.CurrentValues.SetValues(entity);
            //更新时可能出现异常 bug 
            entry.State = EntityState.Modified;
            //如果数据没有发生变化
            if (!this.context.ChangeTracker.HasChanges())
            {
                return false;
            }
            //此处则没更新
            // entry.State = EntityState.Modified;
			return true;
        }

        /// <summary> 删除 </summary>
        /// <param name="entity">实体</param>
        public virtual void Delete<T>(T entity) where T : class
        {
            if(Enable&&entity is IHasDeletionTime)
            {
                if(Update(entity,3))
				{
					Save();
				}
            }
            else
            {
                 context.Set<T>().Remove(entity);
                 Save();
            }
        }

        /// <summary> 批量删除 默认实现  nhibernate 支持 EF dapper 未实现</summary>
       public virtual void Delete<T>(object id) where T : class
        {
            T obj = context.Set<T>().Find(new object[] { id });
            if(Enable&&obj is  IHasDeletionTime deletionTime)
            {
                if (!deletionTime.IsDeleted)
                {
                    UpdateValue(obj, 3);
                    var entry = this.context.Entry(obj);
                    //entry.CurrentValues.SetValues(entity);
                    //更新时可能出现异常 bug 
                    entry.State = EntityState.Modified;
                    //如果数据没有发生变化
                    if (!this.context.ChangeTracker.HasChanges())
                    {
                        return;
                    }
                    //此处则没更新
                    // entry.State = EntityState.Modified;
                    Save();
                }
            }
            else
            {
                context.Set<T>().Remove(obj);
                Save();
            }


        }
        /// <summary>
        /// 实现按需要只更新部分更新
        /// <para>如：Update(u =>u.Id==1,u =>new User{Name="ok"});</para>
        /// </summary>
        /// <param name="where">The where.</param>
        /// <param name="update">The entity.</param>
        public virtual void Update<T>(Expression<Func<T, bool>> where, Expression<Func<T, T>> update) where T : class
        {
			where =Enable? where.Filter():where;
            context.Set<T>().Where(where).Update(update);
        }

        /// <summary> 批量删除 </summary>
        /// <param name="where">条件</param>
        public virtual void Delete<T>(Expression<Func<T, bool>> where=null) where T : class
        {
            where =Enable? where.Filter():where;
            context.Set<T>().Where(where).Delete();
        }

        /// <summary> 操作成功 保存到库里 </summary>
        public virtual void Save()
        {
            //try
            //{
            //context.SaveChanges();
            //}
            //catch (DbEntityValidationException e)
            //{
            //    throw new Exception(e.EntityValidationErrors.First().ValidationErrors.First().ErrorMessage);
            //}
        }

        /// <summary> 条件查询 </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        protected virtual IQueryable<T> Filter<T>(Expression<Func<T, bool>> where) where T : class
        {
			where =Enable? where.Filter():where;
            var dbSet = context.Set<T>().AsNoTracking().AsQueryable();
            if (where != null)
                dbSet = dbSet.Where(where);
            return dbSet;
        }

        /// <summary>执行sql </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public virtual int ExecuteSql(string sql)
        {
            //return  context.Database.ExecuteSqlRaw(sql);
#pragma warning disable CS0618 // 类型或成员已过时
#if NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_1
            return context.Database.ExecuteSqlRaw(sql);
#else
            return context.Database.ExecuteSqlCommand(sql);
#endif
#pragma warning restore CS0618 // 类型或成员已过时
        }

#region async

        /// <summary>查找单个，且不被上下文所跟踪 </summary>
        /// <param name="where">条件</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task<T> FindSingleAsync<T>(Expression<Func<T, bool>> where=null, CancellationToken cancellationToken = default) where T : class
        {
#if !NET40
			where =Enable? where.Filter():where;
            return where == null ? context.Set<T>().AsNoTracking().FirstOrDefaultAsync() : context.Set<T>().AsNoTracking().FirstOrDefaultAsync(where);
#else
            return new Task<T>(()=>FindSingle(where));
#endif
        }

        /// <summary> 是否存在 </summary>
        /// <param name="where">条件</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task<bool> IsExistAsync<T>(Expression<Func<T, bool>> where=null, CancellationToken cancellationToken = default) where T : class
        {
#if !NET40
			where =Enable? where.Filter():where;
            return where == null ? context.Set<T>().AnyAsync() : context.Set<T>().AnyAsync(where);
#else
            return new Task<bool>(()=>IsExist(where));
#endif
        }

        /// <summary> 查询数据 默认实现 ef,nhibernate 支持 linq dapper 不支持linq</summary>
        /// <param name="where">条件</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task<IQueryable<T>> FindAsync<T>(Expression<Func<T, bool>> where = null, CancellationToken cancellationToken = default) where T : class
        {
            return new Task<IQueryable<T>>(()=>Find(where));
        }

        /// <summary> 查询数据 默认实现 ef,nhibernate 支持 linq dapper 不支持linq</summary>
        /// <param name="where">条件</param>
        /// <param name="cancellationToken"></param>
        public virtual Task<List<T>> FindListAsync<T>(Expression<Func<T, bool>> where = null, CancellationToken cancellationToken = default) where T : class
        {
#if !(NET20 || NET30 || NET35 || NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48)
            return Find(where).ToListAsync(cancellationToken);
#else
            return new Task<List<T>>(() => FindList(where));
#endif
        }



        /// <summary> 查询数据 默认实现 ef,nhibernate 支持 linq dapper 不支持linq orderby参数无效 </summary>
        /// <param name="page">页数</param>
        /// <param name="size">记录</param>
        /// <param name="where">条件</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task<IQueryable<T>> FindByPageAsync<T>(Expression<Func<T, bool>> where = null, int page = 1, int size = 10, 
             CancellationToken cancellationToken = default) where T : class
        {
            PageHelper.Set(ref page, ref size);
            return new Task<IQueryable<T>>(()=>FindByPage(where,page, size));
        }


        /// <summary> 查询数据 默认实现 ef,nhibernate 支持 linq dapper 不支持linq orderby参数无效 </summary>
        /// <param name="page">页数</param>
        /// <param name="size">记录</param>
        /// <param name="where">条件</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task<List<T>> FindListByPageAsync<T>(Expression<Func<T, bool>> where = null, int page = 1, int size = 10, 
           CancellationToken cancellationToken = default) where T : class
        {
            PageHelper.Set(ref page, ref size);
#if !(NET20 || NET30 || NET35 || NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48)
            return FindByPage(where,page, size).ToListAsync(cancellationToken);
#else
              return new  Task<List<T>>(() => FindListByPage(where,page, size));
#endif
        }


        /// <summary> 根据过滤条件获取记录数 </summary>
        /// <param name="where">条件</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task<int> CountAsync<T>(Expression<Func<T, bool>> where = null, CancellationToken cancellationToken = default) where T : class
        {
#if !NET40
            where =Enable? where.Filter():where;
            return Filter(where).CountAsync();
#else
            return new Task<int>(()=>Count(where));
#endif
        }

        /// <summary> 添加 </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="entity">实体</param>
        public virtual 
#if !NET40
            async
#endif
            Task InsertAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class
        {
#if !(NET20 || NET30 || NET35 || NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48)
            UpdateValue(entity);
            await context.Set<T>().AddAsync(entity, cancellationToken);
             context.Entry(entity).State = EntityState.Detached;
            await SaveAsync();
#else
            Insert(entity);
#if !(NET40 || NET45 || NET451 || NET452)
            await Task.CompletedTask;
#elif (NET45 || NET451 || NET452)
            await TaskHelper.CompletedTask;
#else
            return TaskHelper.CompletedTask;
#endif
#endif
        }


        /// <summary> 批量添加</summary>
        /// <param name="cancellationToken"></param>
        /// <param name="entities">The entities.</param>
        public virtual 
#if !NET40
            async
#endif
            Task BatchInsertAsync<T>(T[] entities, CancellationToken cancellationToken = default) where T : class
        {
#if !(NET20 || NET30 || NET35 || NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48)
			if(Enable)
			{
			    foreach (var entity in entities)
				{
					UpdateValue(entity);
				}
			}
            await context.Set<T>().AddRangeAsync(entities);
            await SaveAsync();
#else
             context.Set<T>().AddRange(entities);
#if !NET40
            await SaveAsync();
#else
            Save();
            return TaskHelper.CompletedTask;
#endif
#endif
        }

        /// <summary> 更新一个实体的所有属性 </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="entity">实体</param>
        public virtual Task UpdateAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class
        {
            Update(entity);
#if !(NET40 || NET45 || NET451 || NET452)
            return Task.CompletedTask;
#else
            return TaskHelper.CompletedTask;
#endif
        }

        /// <summary> 删除 </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="entity">实体</param>
        public virtual Task DeleteAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class
        {
            Delete(entity);
#if !(NET40 || NET45 || NET451 || NET452)
            return Task.CompletedTask;
#else
            return TaskHelper.CompletedTask;
#endif
        }

       
        /// <summary> 批量删除 默认实现  nhibernate 支持 EF dapper 未实现</summary>
        public virtual Task DeleteAsync<T>(object id, CancellationToken cancellationToken = default) where T : class
        {
            Delete<T>(id);
#if !(NET40 || NET45 || NET451 || NET452)
            return Task.CompletedTask;
#else
            return TaskHelper.CompletedTask;
#endif
        }



        /// <summary>
        /// 实现按需要只更新部分更新
        /// <para>如：Update(u =>u.Id==1,u =>new User{Name="ok"});</para>
        /// </summary>
        /// <param name="where">The where.</param>
        /// <param name="update">The entity.</param>
        /// <param name="cancellationToken"></param>
        public virtual Task UpdateAsync<T>(Expression<Func<T, bool>> where, Expression<Func<T, T>> update, CancellationToken cancellationToken = default) where T : class
        {
#if !(NET40)
            where =Enable? where.Filter():where;
            return context.Set<T>().Where(where).UpdateAsync(update);
#else
            Update(where,update);
            return TaskHelper.CompletedTask;
#endif

        }

        /// <summary> 批量删除 </summary>
        /// <param name="where">条件</param>
        /// <param name="cancellationToken"></param>
        public virtual Task DeleteAsync<T>(Expression<Func<T, bool>> where=null, CancellationToken cancellationToken = default) where T:class
        {
#if !(NET40)
            where =Enable? where.Filter():where;
            return context.Set<T>().Where(where).DeleteAsync(cancellationToken);
#else
            Delete(where);
            return TaskHelper.CompletedTask;
#endif
        }


        /// <summary>执行sql </summary>
        /// <param name="sql"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task<int> ExecuteSqlAsync(string sql, CancellationToken cancellationToken = default)
        {
#if !(NET40)
            //return  _context.Database.ExecuteSqlRaw(sql);
#pragma warning disable CS0618 // 类型或成员已过时
#if NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_1
            return context.Database.ExecuteSqlRawAsync(sql, cancellationToken);
#else
            return context.Database.ExecuteSqlCommandAsync(sql, cancellationToken);
#endif
#pragma warning restore CS0618 // 类型或成员已过时
#else
            return new Task<int>(()=>ExecuteSql(sql));
#endif
        }

        /// <summary> 操作成功 保存到库里 </summary>
        /// <param name="cancellationToken"></param>
        public virtual Task SaveAsync(CancellationToken cancellationToken = default)
        {
            return TaskHelper.CompletedTask;
#if !(NET40)
            //try
            //{
            // return context.SaveChangesAsync();
            //}
            //catch (DbEntityValidationException e)
            //{
            //    throw new Exception(e.EntityValidationErrors.First().ValidationErrors.First().ErrorMessage);
            //}
#else
           // Save();
            //return TaskHelper.CompletedTask;
#endif
        }

#endregion async

        void IDisposable.Dispose()
        {
            context.Dispose();
        }
    }
}
#endif