//#if !(NET10 || NET11 || NET20 || NET30 || NET35 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
#if  NET40 ||NET45 || NET451 || NET452 || NET46 ||NET461 || NET462|| NET47 || NET471 || NET472|| NET48 ||  NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using System;
using System.Linq.Expressions;
using System.Linq;
using Utility.Model;
using System.Threading.Tasks;
using System.Threading;
using Utility.Ef.Repositories;
using Utility.DAL;
using System.Collections.Generic;

#if NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48
using System.Data.Entity;
#else
using Microsoft.EntityFrameworkCore;
#endif

namespace Utility.Ef.DAL
{

    /// <summary>
    /// ef 数据访问层 
    /// </summary>
    /// <typeparam name="Model"></typeparam>
    /// <typeparam name="Key"></typeparam>
    public class BaseEfDAL<Model, Key> : BaseEfDAL<Model>,IDAL<Model,Key> where Model : class, IModel<Key>
    {
        /// <summary> 构造函数注入</summary>
        /// <param name="context">数据库上下文</param>
        public BaseEfDAL(DbContext context) : base(context)
        {

        }
        /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public virtual int Delete(Key id)
        {
            return Repository.UnitWork.Delete<Model,Key>(id);
        }

        /// <summary>根据id删除实体类信息(多删除)</summary>
        /// <param name="ids">id</param>
        ///<return>返回删除结果(多删除),大于0 返回删除成功(多删除),小于等于0 返回删除失败(多删除) </return>
        public virtual int DeleteList(Key[] ids)
        {
            return Repository.UnitWork.DeleteList<Model, Key>(ids);
        }
        /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public virtual Task<int> DeleteAsync(Key id, CancellationToken cancellationToken = default)
        {
            var res = Delete(id);
            return new Task<int>(()=>res);
        }

        /// <summary>根据id删除实体类信息(多删除)</summary>
        /// <param name="ids">id</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回删除结果(多删除),大于0 返回删除成功(多删除),小于等于0 返回删除失败(多删除) </return>
        public virtual Task<int> DeleteListAsync(Key[] ids, CancellationToken cancellationToken = default)
        {
            var res = DeleteList(ids);
            return new Task<int>(()=>res);
        }
    }

    /// <summary>数据访问层接口基类</summary>
    public class BaseEfDAL<Model>  where Model : class
    {
        /// <summary>
        /// 
        /// </summary>
        protected BaseEfRepository<Model> Repository { get; set; }
        /// <summary> 构造函数注入</summary>
        /// <param name="context">数据库上下文</param>
        public BaseEfDAL(DbContext context) 
        {
            this.Repository = new BaseEfRepository<Model>(context);
        }
        /// <summary>添加实体类信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回添加结果,大于0 返回添加成功,小于等于0 返回添加失败 </return>
        public virtual int Insert(Model obj)
        {
            Repository.Insert(obj);
            return 1;
        }

        /// <summary>修改实体类信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回修改结果,大于0 返回修改成功,小于等于0 返回修改失败 </return>
        public virtual int Update(Model obj)
        {
            Repository.Update(obj);
            return 1;
        }

        /// <summary> 批量删除 </summary>
        /// <param name="where">条件</param>
        public virtual void Delete<T>(Expression<Func<T, bool>> where = null) where T : class
        {
            Repository.UnitWork.Delete(where);
        }

        /// <summary>根据条件查询实体类数据集数量信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回实体类数据集数量信息</return>
        public virtual int Count(Model obj)
        {
            var count = Repository.Count(QueryWhere(obj));
            return count;
        }

        /// <summary>根据条件查询实体类数据集信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回实体类数据集信息</return>
        public virtual List<Model> FindList(Model obj)
        {
            var datas = Repository.Find(QueryWhere(obj)).ToList();
            return datas;
        }

        /// <summary>根据条件及分页查询实体类数据集信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<return>返回实体类数据集信息</return>
        public virtual List<Model> FindListByPage(Model obj, int page=1, int size=10)
        {
            Expression<Func<Model, bool>> where = QueryWhere(obj);
            return Repository.UnitWork.FindListByPage(where,page, size);
        }

        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
        public virtual ResultModel<Model> FindResultModelByPage(Model obj, int page=1, int size=10)
        {
            Expression<Func<Model, bool>> where = QueryWhere(obj);
            return FindResultModelByPage(where,page,size);
        }

        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息</summary>
        /// <param name="page">页数</param>
        /// <param name="where"></param>
        /// <param name="size">每页记录</param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
        public virtual ResultModel<Model> FindResultModelByPage(Expression<Func<Model, bool>> where=null, int page=1, int size=10)
        {
           var tuple= Repository.UnitWork.FindTupleByPage(where, page, size);
            ResultModel<Model> result = new ResultModel<Model>(tuple,page,size);
            return result;
        }

        /// <summary>查询wehere sql </summary>
        /// <param name="obj">地址信息</param>
        /// <returns></returns>
        protected virtual Expression<Func<Model, bool>> QueryWhere(Model obj)
        {
            return null;
        }

#region async
        /// <summary>添加实体类信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回添加结果,大于0 返回添加成功,小于等于0 返回添加失败 </return>
        public virtual
#if !NET40
            async
#endif
            Task<int> InsertAsync(Model obj,CancellationToken cancellationToken=default)
        { 
#if !NET40
            await Repository.InsertAsync(obj, cancellationToken);
            return 1;
#else
            Repository.Insert(obj);
            return new Task<int>(()=>1);
#endif

        }

        /// <summary>修改实体类信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回修改结果,大于0 返回修改成功,小于等于0 返回修改失败 </return>
        public virtual 
#if !NET40
            async
#endif
            Task<int> UpdateAsync(Model obj, CancellationToken cancellationToken = default)
        {
#if !NET40
            await Repository.UpdateAsync(obj, cancellationToken);
            return 1;
#else
            Repository.Update(obj);
            return new Task<int>(()=>1);
#endif
        }


        /// <summary>根据条件查询实体类数据集数量信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集数量信息</return>
        public virtual Task<int> CountAsync(Model obj, CancellationToken cancellationToken = default)
        {
            var count = Repository.CountAsync(QueryWhere(obj), cancellationToken);
            return count;
        }

        /// <summary>根据条件查询实体类数据集信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息</return>
        public virtual Task<List<Model>> FindListAsync(Model obj, CancellationToken cancellationToken = default)
        {
            var datas = Repository.UnitWork.FindListAsync(QueryWhere(obj));
            return datas;
        }

        /// <summary>根据条件及分页查询实体类数据集信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息</return>
        public virtual Task<List<Model>> FindListByPageAsync(Model obj, int page=1, int size=10, CancellationToken cancellationToken = default)
        {
             Expression<Func<Model, bool>> where = QueryWhere(obj);
             return Repository.UnitWork.FindListByPageAsync(where, page,size);
        }

        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
        public virtual  Task<ResultModel<Model>> FindResultModelByPageAsync(Model obj, int page=1, int size=10, CancellationToken cancellationToken = default)
        {
            Expression<Func<Model, bool>> where = QueryWhere(obj);
            return  FindResultModelByPageAsync(where,page,size,cancellationToken);
        }

        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息</summary>
        /// <param name="where">条件</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
        public virtual
#if !NET40
            async
#endif
            Task<ResultModel<Model>> FindResultModelByPageAsync( Expression<Func<Model, bool>> where=null, int page=1, int size=10, CancellationToken cancellationToken = default)
        {
#if !NET40
            var tuple = await Repository.UnitWork.FindTupleByPageAsync(where, page, size, cancellationToken);
            ResultModel<Model> result = new ResultModel<Model>(tuple, page, size);
            return result;
#else
            var tuple =  Repository.UnitWork.FindTupleByPage(where, page, size);
            ResultModel<Model> result = new ResultModel<Model>(tuple, page, size);
            return new Task<ResultModel<Model>>(()=>result);
#endif

        }
#endregion async
    }

   

}
#endif