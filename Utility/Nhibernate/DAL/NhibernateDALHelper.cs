#if !(NET10 || NET11 || NET20 || NET30 || NET35 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
//#if NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Utility.DAL;
using Utility.Model;
using Utility.Nhibernate.Uow;

namespace Utility.Nhibernate.DAL
{
    /// <summary>
    /// nhibernate 数据访问层基类
    /// </summary>
    /// <typeparam name="Model">实体模型</typeparam>
    /// <typeparam name="Key">主键类型</typeparam>
    public class NhibernateDALHelper<Model, Key>: NhibernateDALHelper<Model> where Model : class, IModel<Key>
    {


        /// <summary>根据id删除实体类信息(多删除)</summary>
        /// <param name="ids">id</param>
        /// <param name="idColumn"></param>
        /// <param name="table"></param>
        ///<return>返回删除结果(多删除),大于0 返回删除成功(多删除),小于等于0 返回删除失败(多删除) </return>
        public static int DeleteList(Key[] ids, string table, string idColumn = DALHelper.IdColumn) 
        {
            return DALHelper.DeleteList<Model, Key>(NhibernateUnitWork, ids, table, idColumn);
        }

        /// <summary>根据id删除实体类信息(多删除)</summary>
        /// <param name="ids">id</param>
        /// <param name="idColumn"></param>
        /// <param name="nhibernateUnitWork"></param>
        /// <param name="table"></param>
        ///<return>返回删除结果(多删除),大于0 返回删除成功(多删除),小于等于0 返回删除失败(多删除) </return>
        public static int DeleteList(NhibernateUnitWork nhibernateUnitWork, Key[] ids, string table, string idColumn = DALHelper.IdColumn) 
        {
            return DALHelper.DeleteList<Model, Key>(nhibernateUnitWork, ids,  table, idColumn);
        }

        /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public static int Delete(Key id) 
        {
            return DALHelper.Delete<Model,Key>(NhibernateUnitWork, id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nhibernateUnitWork"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int Delete(NhibernateUnitWork nhibernateUnitWork, Key id) 
        {
            return DALHelper.Delete<Model, Key>(nhibernateUnitWork, id);
        }


#region async

        /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public static  Task<int> DeleteAsync(Key id,CancellationToken cancellationToken= default)
        {
            return  DeleteAsync(NhibernateUnitWork, id, cancellationToken);
        }

        /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        /// <param name="nhibernateUnitWork"></param>
        /// <param name="cancellationToken"></param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public static  Task<int> DeleteAsync(NhibernateUnitWork nhibernateUnitWork, Key id, CancellationToken cancellationToken = default)
        {
            return  DALHelper.DeleteAsync<Model, Key>(nhibernateUnitWork, id, cancellationToken);
        }

        /// <summary>根据id删除实体类信息(多删除)</summary>
        /// <param name="ids">id</param>
        /// <param name="table"></param>
        /// <param name="idColumn"></param>
        /// <param name="cancellationToken"></param>
        ///<return>返回删除结果(多删除),大于0 返回删除成功(多删除),小于等于0 返回删除失败(多删除) </return>
        public static  Task<int> DeleteListAsyn(Key[] ids, string table, string idColumn = DALHelper.IdColumn,
           CancellationToken cancellationToken = default)
        {
            return  DeleteListAsync(NhibernateUnitWork, ids, table, idColumn, cancellationToken);
        }

        /// <summary>根据id删除实体类信息(多删除)</summary>
        /// <param name="ids">id</param>
        /// <param name="table"></param>
        /// <param name="nhibernateUnitWork"></param>
        /// <param name="idColumn"></param>
        /// <param name="cancellationToken"></param>
        ///<return>返回删除结果(多删除),大于0 返回删除成功(多删除),小于等于0 返回删除失败(多删除) </return>
        public static  Task<int> DeleteListAsync(NhibernateUnitWork nhibernateUnitWork, Key[] ids, string table, string idColumn = DALHelper.IdColumn,
            CancellationToken cancellationToken = default)
        {
            return  DALHelper.DeleteListAsync<Model, Key>(nhibernateUnitWork, ids, table, idColumn, cancellationToken);
        }
#endregion async
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="Model"></typeparam>
    public class NhibernateDALHelper<Model>  where Model : class
    {
        /// <summary>
        /// 
        /// </summary>
        public static ISession Session => NhibernateUnitWork != null ? NhibernateUnitWork.Session : null;

        /// <summary>
        /// 
        /// </summary>
        public static NhibernateUnitWork NhibernateUnitWork { get; set; }

        private static Func<Model, ICriteria> where;
        private static Func<Model, int, int, ICriteria> whereByPage;
        /// <summary>
        /// 
        /// </summary>

        protected readonly static Func<Model, ICriteria> WhereEmpty = (it) => null;
        /// <summary>
        /// 
        /// </summary>

        protected readonly static Func<Model, int, int, ICriteria> WhereByPageEmpty = (it, page, size) => null;
    
        /// <summary>
        /// 
        /// </summary>
        protected static ICriteria WhereCondition { get; set; }
        /// <summary>
        /// 
        /// </summary>
        protected static ICriteria WhereConditionByPage { get; set; }
        /// <summary>
        /// 
        /// </summary>

        public static Func<Model, ICriteria> Where { get => where?? WhereEmpty; set => where = value; }

        /// <summary>
        /// 
        /// </summary>
        public static Func<Model, int, int, ICriteria> WhereByPage { get => whereByPage?? WhereByPageEmpty; set => whereByPage = value; }

        /// <summary>添加实体类信息信息</summary>
        /// <param name="obj">实体类信息</param>
        ///<return>返回添加实体类信息结果,大于0 返回添加成功,小于等于0 返回添加用失败 </return>
        public static int Insert(Model obj)
        {
            return Insert(NhibernateUnitWork, obj);
        }

        /// <summary>添加实体类信息信息</summary>
        /// <param name="nhibernateUnitWork"></param>
        /// <param name="obj">实体类信息</param>
        ///<return>返回添加实体类信息结果,大于0 返回添加成功,小于等于0 返回添加用失败 </return>
        public static int Insert(NhibernateUnitWork nhibernateUnitWork, Model obj)
        {
            return DALHelper.Insert<Model>(nhibernateUnitWork, obj);
        }

        /// <summary>修改实体类信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回修改结果,大于0 返回修改成功,小于等于0 返回修改失败 </return>
        public static int Update(Model obj)
        {
            return Update(NhibernateUnitWork, obj);
        }

        /// <summary>修改实体类信息</summary>
        /// <param name="nhibernateUnitWork"></param>
        /// <param name="obj">实体类</param>
        ///<return>返回修改结果,大于0 返回修改成功,小于等于0 返回修改失败 </return>
        public static int Update(NhibernateUnitWork nhibernateUnitWork, Model obj)
        {
            return DALHelper.Update<Model>(nhibernateUnitWork, obj);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int Delete(object id)
        {
            return Delete(NhibernateUnitWork, id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nhibernateUnitWork"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int Delete(NhibernateUnitWork nhibernateUnitWork, object id)
        {
            return DALHelper.Delete<Model>(nhibernateUnitWork, id);
        }

        /// <summary>根据条件查询实体类数据集数量信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回实体类数据集数量信息</return>
        public static int Count(Model obj)
        {
            return Count(NhibernateUnitWork, obj);
        }

        /// <summary>根据条件查询实体类数据集数量信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="nhibernateUnitWork"></param>
        ///<return>返回实体类数据集数量信息</return>
        public static int Count(NhibernateUnitWork nhibernateUnitWork, Model obj)
        {
            using (Session.BeginTransaction())
            {
                ICriteria where = Where(obj);
                return NhibernateDALHelper.Count<Model>(nhibernateUnitWork, where);
            }
        }

        /// <summary>根据条件查询实体类数据集信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回实体类数据集信息</return>
        public static List<Model> FindList(Model obj)
        {
            return FindList(NhibernateUnitWork, obj);
        }

        /// <summary>根据条件查询实体类数据集信息</summary>
        /// <param name="nhibernateUnitWork"></param>
        /// <param name="obj">实体类</param>
        ///<return>返回实体类数据集信息</return>
        public static List<Model> FindList(NhibernateUnitWork nhibernateUnitWork, Model obj)
        {
            using (Session.BeginTransaction())
            {
                ICriteria where = Where(obj);
                return NhibernateDALHelper.FindList<Model>(nhibernateUnitWork, where);
            }
        }

        /// <summary>根据条件及分页查询实体类数据集信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<return>返回实体类数据集信息</return>
        public static List<Model> FindListByPage(Model obj, int page=1, int size=10)
        {
            return FindListByPage(NhibernateUnitWork, obj, page, size);
        }


        /// <summary>根据条件及分页查询实体类数据集信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="nhibernateUnitWork"></param>
        /// <param name="size">每页记录</param>
        ///<return>返回实体类数据集信息</return>
        public static List<Model> FindListByPage(NhibernateUnitWork nhibernateUnitWork, Model obj, int page=1, int size=10)
        {
            using (Session.BeginTransaction())
            {
                ICriteria where = WhereByPage(obj, page, size);
                return NhibernateDALHelper.FindListByPage<Model>(nhibernateUnitWork, where, page, size);
            }
        }

        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>

        public static ResultModel<Model> FindResultModelByPage(Model obj, int page=1, int size=10)
        {
            return FindResultModelByPage(NhibernateUnitWork, obj, page, size);
        }

        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="nhibernateUnitWork"></param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
        public static ResultModel<Model> FindResultModelByPage(NhibernateUnitWork nhibernateUnitWork, Model obj, int page=1, int size=10)
        {
            using (Session.BeginTransaction())
            {
                ICriteria where = WhereByPage(obj, page, size);
                return NhibernateDALHelper.FindResultModelByPage<Model>(nhibernateUnitWork, where, page, size);
            }
        }


#region async
        /// <summary>添加实体类信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回添加结果,大于0 返回添加成功,小于等于0 返回添加失败 </return>
        public static  Task<int> InsertAsync(Model obj, CancellationToken cancellationToken = default)
        {
            return  InsertAsync(NhibernateUnitWork, obj, cancellationToken);
        }

        /// <summary>添加实体类信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="nhibernateUnitWork"></param>
        /// <param name="cancellationToken"></param>
        ///<return>返回添加结果,大于0 返回添加成功,小于等于0 返回添加失败 </return>
        public static  Task<int> InsertAsync(NhibernateUnitWork nhibernateUnitWork, Model obj, CancellationToken cancellationToken = default)
        {
            return  DALHelper.InsertAsync<Model>(nhibernateUnitWork, obj, cancellationToken);
        }

        /// <summary>修改实体类信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回修改结果,大于0 返回修改成功,小于等于0 返回修改失败 </return>
        public static  Task<int> UpdateAsync(Model obj, CancellationToken cancellationToken = default)
        {
            return  UpdateAsync(NhibernateUnitWork, obj, cancellationToken);
        }

        /// <summary>修改实体类信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="nhibernateUnitWork"></param>
        /// <param name="cancellationToken"></param>
        ///<return>返回修改结果,大于0 返回修改成功,小于等于0 返回修改失败 </return>
        public static  Task<int> UpdateAsync(NhibernateUnitWork nhibernateUnitWork, Model obj, CancellationToken cancellationToken = default)
        {
            return DALHelper.UpdateAsync<Model>(nhibernateUnitWork, obj, cancellationToken);
        }

        /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public static  Task<int> DeleteAsync(object id, CancellationToken cancellationToken = default)
        {
            return  DeleteAsync(NhibernateUnitWork, id, cancellationToken);
        }

        /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        /// <param name="nhibernateUnitWork"></param>
        /// <param name="cancellationToken"></param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public static  Task<int> DeleteAsync(NhibernateUnitWork nhibernateUnitWork, object id, CancellationToken cancellationToken = default)
        {
            return DALHelper.DeleteAsync<Model>(nhibernateUnitWork, id, cancellationToken);
        }

        /// <summary>根据条件查询实体类数据集数量信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集数量信息</return>
        public static  Task<int> CountAsync(Model obj, CancellationToken cancellationToken = default)
        {
            return  CountAsync(NhibernateUnitWork, obj, cancellationToken);
        }

        /// <summary>根据条件查询实体类数据集数量信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="nhibernateUnitWork"></param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集数量信息</return>
        public static  Task<int> CountAsync(NhibernateUnitWork nhibernateUnitWork, Model obj, CancellationToken cancellationToken = default)
        {
            ICriteria where = Where(obj);
            return NhibernateDALHelper.CountAsync<Model>(nhibernateUnitWork, where, cancellationToken);
        }

        /// <summary>根据条件查询实体类数据集信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息</return>
        public static  Task<List<Model>> FindListAsync(Model obj, CancellationToken cancellationToken = default)
        {
            return  FindListAsync(NhibernateUnitWork, obj, cancellationToken);
        }

        /// <summary>根据条件查询实体类数据集信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="nhibernateUnitWork"></param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息</return>
        public static  Task<List<Model>> FindListAsync(NhibernateUnitWork nhibernateUnitWork, Model obj,
            CancellationToken cancellationToken = default)
        {
            ICriteria where = Where(obj);
            return  NhibernateDALHelper.FindListAsync<Model>(nhibernateUnitWork, where, cancellationToken);
        }

        /// <summary>根据条件及分页查询实体类数据集信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息</return>
        public static  Task<List<Model>> FindListByPageAsync(Model obj, int page, int size, CancellationToken cancellationToken = default)
        {
            return  FindListByPageAsync(NhibernateUnitWork, obj, page, size, cancellationToken);
        }

        /// <summary>根据条件及分页查询实体类数据集信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="nhibernateUnitWork"></param>
        /// <param name="size">每页记录</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息</return>
        public static  Task<List<Model>> FindListByPageAsync(NhibernateUnitWork nhibernateUnitWork, Model obj, int page=1, int size=10,
            CancellationToken cancellationToken = default)
        {
            ICriteria where = WhereByPage(obj, page, size);
            return  NhibernateDALHelper.FindListByPageAsync<Model>(nhibernateUnitWork, where, page, size,  cancellationToken);
        }

        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
        public static  Task<ResultModel<Model>> FindResultModelByPageAsync(Model obj, int page=1,
            int size=10, CancellationToken cancellationToken = default)
        {
            return  FindResultModelByPageAsync(NhibernateUnitWork, obj, page, size, cancellationToken);
        }

        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="nhibernateUnitWork"></param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
        public static  Task<ResultModel<Model>> FindResultModelByPageAsync(NhibernateUnitWork nhibernateUnitWork, Model obj, int page=1,
            int size=10, CancellationToken cancellationToken = default)
        {
            ICriteria where = WhereByPage(obj, page, size);
            return  NhibernateDALHelper.FindResultModelByPageAsync<Model>(nhibernateUnitWork, where, page, size,  cancellationToken);
        }
#endregion async
    }

    /// <summary>
    /// 
    /// </summary>
    public     class NhibernateDALHelper
    {

        /// <summary>
        /// 
        /// </summary>
        public static ISession Session => NhibernateUnitWork != null ? NhibernateUnitWork.Session : null;

        /// <summary>
        /// 
        /// </summary>
        public static NhibernateUnitWork NhibernateUnitWork { get; set; }

        /// <summary>添加实体类信息信息</summary>
        /// <param name="obj">实体类信息</param>
        ///<return>返回添加实体类信息结果,大于0 返回添加成功,小于等于0 返回添加用失败 </return>
        public  static int Insert<Model, Key>(Model obj) where Model : class, IModel<Key>
        {
            return DALHelper.Insert(NhibernateUnitWork, obj);
        }

        /// <summary>添加实体类信息信息</summary>
        /// <param name="obj">实体类信息</param>
        /// <param name="nhibernateUnitWork"></param>
        ///<return>返回添加实体类信息结果,大于0 返回添加成功,小于等于0 返回添加用失败 </return>
        public static int Insert<Model, Key>(NhibernateUnitWork nhibernateUnitWork, Model obj) where Model : class, IModel<Key>
        {
            var id = nhibernateUnitWork.Insert(obj);
            if (id != null)
            {
                obj.Id = (Key)id;
            }
            return 1;
        }

        /// <summary>根据条件查询实体类数据集数量信息</summary>
        /// <param name="where">条件</param>
        ///<return>返回实体类数据集数量信息</return>
        public static int Count<Model>(ICriteria where=null) where Model : class
        {
            return Count<Model>(NhibernateUnitWork, where);
        }


        /// <summary>根据条件查询实体类数据集数量信息</summary>
        /// <param name="where">条件</param>
        /// <param name="nhibernateUnitWork"></param>
        ///<return>返回实体类数据集数量信息</return>
        public static int Count<Model>(NhibernateUnitWork nhibernateUnitWork, ICriteria where = null) where Model : class
        {
            return nhibernateUnitWork.Count<Model>(where);
        }

        /// <summary>根据条件查询实体类数据集信息</summary>
        /// <param name="where">条件</param>
        ///<return>返回实体类数据集信息</return>
        public static List<Model> FindList<Model>(ICriteria where = null) where Model : class
        {
            return FindList<Model>(NhibernateUnitWork, where);
        }

        /// <summary>根据条件查询实体类数据集信息</summary>
        /// <param name="where">条件</param>
        /// <param name="nhibernateUnitWork"></param>
        ///<return>返回实体类数据集信息</return>
        public static List<Model> FindList<Model>(NhibernateUnitWork nhibernateUnitWork, ICriteria where = null) where Model : class
        {
            return nhibernateUnitWork.FindList<Model>(where);
        }

        /// <summary>根据条件及分页查询实体类数据集信息</summary>
        /// <param name="where">条件</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<return>返回实体类数据集信息</return>
        public static List<Model> FindListByPage<Model>(ICriteria where = null,int page=1, int size=10) where Model : class
        {
            return FindListByPage<Model>(NhibernateUnitWork, where, page,size);
        }


        /// <summary>根据条件及分页查询实体类数据集信息</summary>
        /// <param name="where">条件</param>
        /// <param name="page">页数</param>
        /// <param name="nhibernateUnitWork"></param>
        /// <param name="size">每页记录</param>
        ///<return>返回实体类数据集信息</return>
        public static List<Model> FindListByPage<Model>(NhibernateUnitWork nhibernateUnitWork, ICriteria where = null, int page=1, int size=10) where Model : class
        {
            return  nhibernateUnitWork.FindListByPage<Model>(where, page,size);
        }

        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息</summary>
        /// <param name="where">条件</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>

        public static ResultModel<Model> FindResultModelByPage<Model>( ICriteria where = null,int page=1, int size=10) where Model : class
        {
            return FindResultModelByPage<Model>(NhibernateUnitWork, where,  page, size);
        }


        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息</summary>
        /// <param name="where">条件</param>
        /// <param name="nhibernateUnitWork"></param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
        public static ResultModel<Model> FindResultModelByPage<Model>(NhibernateUnitWork nhibernateUnitWork, ICriteria where = null, int page=1, int size=10) where Model : class
        {
            var da =  nhibernateUnitWork.FindResultByPage<Model>(where,page, size);
            ResultModel<Model> result = new ResultModel<Model>(da, page, size);
            return result;
        }
        /// <summary>条件拼接 </summary>
        /// <param name="session"></param>
        /// <param name="ors">or条件</param>
        /// <param name="ands">and 条件</param>
        /// <returns>组装条件</returns>
        public static ICriteria QueryWhere<Model>(ISession session, List<AbstractCriterion> ors, 
            List<AbstractCriterion> ands) where Model : class
        {
            return NhibernateTemplate.QueryWhere<Model>(session,ors,ands);
        }


#region async



        /// <summary>添加实体类信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回添加结果,大于0 返回添加成功,小于等于0 返回添加失败 </return>
        public static  Task<int> InsertAsync<Model, Key>(Model obj, CancellationToken cancellationToken = default) 
            where Model : class, IModel<Key>
        {
            return  InsertAsync<Model, Key>(NhibernateUnitWork, obj, cancellationToken);
        }

        /// <summary>添加实体类信息</summary>
        /// <param name="nhibernateUnitWork"></param>
        /// <param name="obj">实体类</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回添加结果,大于0 返回添加成功,小于等于0 返回添加失败 </return>
        public static
#if !NET40
            async
#endif
            Task<int> InsertAsync<Model,Key>(NhibernateUnitWork nhibernateUnitWork, Model obj, CancellationToken cancellationToken = default)
            where Model:class,IModel<Key>
        {
#if !NET40
            var id = await NhibernateTemplate.Empty.InsertAsync(nhibernateUnitWork.Session,obj,cancellationToken);
            if (id != null)
            {
                obj.Id = (Key)id;
            }
            return 1;
#else
            return new Task<int>(()=> Insert<Model,Key>(obj));
#endif

        }

        /// <summary>根据条件查询实体类数据集数量信息</summary>
        /// <param name="where">条件</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集数量信息</return>
        public static  Task<int> CountAsync<Model>(ICriteria where = null, CancellationToken cancellationToken = default) where Model : class
        {
            return  CountAsync<Model>(NhibernateUnitWork, where,cancellationToken);
        }

        /// <summary>根据条件查询实体类数据集数量信息</summary>
        /// <param name="where">条件</param>
        /// <param name="nhibernateUnitWork"></param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集数量信息</return>
        public static  Task<int> CountAsync<Model>(NhibernateUnitWork nhibernateUnitWork, ICriteria where = null, CancellationToken cancellationToken = default) where Model : class
        {
            return  nhibernateUnitWork.CountAsync<Model>(where, cancellationToken);
        }

        /// <summary>根据条件查询实体类数据集信息</summary>
        /// <param name="where">条件</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息</return>
        public static  Task<List<Model>> FindListAsync<Model>(ICriteria where = null, CancellationToken cancellationToken = default) where Model : class
        {
            return  FindListAsync<Model>(NhibernateUnitWork,where,cancellationToken);
        }

        /// <summary>根据条件查询实体类数据集信息</summary>
        /// <param name="where">条件</param>
        /// <param name="nhibernateUnitWork"></param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息</return>
        public static  Task<List<Model>> FindListAsync<Model>(NhibernateUnitWork nhibernateUnitWork, ICriteria where = null,
            CancellationToken cancellationToken = default) where Model : class
        {
            return  nhibernateUnitWork.FindListAsync<Model>(where, cancellationToken);
        }

        /// <summary>根据条件及分页查询实体类数据集信息</summary>
        /// <param name="where">条件</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息</return>
        public static  Task<List<Model>> FindListByPageAsync<Model>(ICriteria where = null, int page=1, int size=10, 
             CancellationToken cancellationToken = default) where Model : class
        {
            return  FindListByPageAsync<Model>(NhibernateUnitWork, where, page,size, cancellationToken);
        }

        /// <summary>根据条件及分页查询实体类数据集信息</summary>
        /// <param name="where">条件</param>
        /// <param name="nhibernateUnitWork"></param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息</return>
        public static  Task<List<Model>> FindListByPageAsync<Model>(NhibernateUnitWork nhibernateUnitWork, ICriteria where = null,
            int page=1, int size=10,CancellationToken cancellationToken = default) where Model : class
        {
            return  nhibernateUnitWork.FindListByPageAsync<Model>(where, page, size, cancellationToken);
        }

        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息</summary>
        /// <param name="where">条件</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
        public static  Task<ResultModel<Model>> FindResultModelByPageAsync<Model>(ICriteria where = null, int page=1, 
            int size=10, CancellationToken cancellationToken = default) where Model : class
        {
            return  FindResultModelByPageAsync<Model>(NhibernateUnitWork, where, page,size,cancellationToken);
        }

        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息</summary>
        /// <param name="where">条件</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        /// <param name="cancellationToken"></param>
        /// <param name="nhibernateUnitWork"></param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
        public static  Task<ResultModel<Model>> FindResultModelByPageAsync<Model>(NhibernateUnitWork nhibernateUnitWork, ICriteria where = null, int page=1,
            int size=10, CancellationToken cancellationToken = default) where Model : class
        {
            var da = nhibernateUnitWork.FindTupleByPageAsync<Model>(where, page,size,cancellationToken).Result;
            ResultModel<Model> result = new ResultModel<Model>(da,page,size);
            return new  Task<ResultModel<Model>>(()=> result);
        }
#endregion async
    }
}
#endif