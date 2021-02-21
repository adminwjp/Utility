#if !(NET10 || NET11 || NET20 || NET30 || NET35 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
//#if NET40 ||NET45 || NET451 || NET452 || NET46 || NET40 ||NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48  || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Criterion;
using Utility.DAL;
using Utility.Model;
using Utility.Nhibernate.Repositories;

namespace Utility.Nhibernate.DAL
{

    /// <summary>实体 nhibernate 数据访问层接口 实现   </summary>
    public class BaseNhibernateDAL<Model, Key> : BaseNhibernateDAL<Model>, IDAL<Model, Key> where Model : class, IModel<Key>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        public BaseNhibernateDAL(ISession session) : base(session)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual string GetTable()
        {
            return string.Empty;
        }
        /// <summary>添加实体类信息信息</summary>
        /// <param name="obj">实体类信息</param>
        ///<return>返回添加实体类信息结果,大于0 返回添加成功,小于等于0 返回添加用失败 </return>
        public new int Insert(Model obj)
        {
            return NhibernateDALHelper<Model, Key>.Insert(NhibernateRepository.UnitWork,obj);
        }

        /// <summary>根据id删除用实体类信息</summary>
        /// <param name="id">实体类 id</param>
        ///<return>返回删除实体类信息结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public virtual int Delete(Key id)
        {
            NhibernateRepository.Delete(id);
            return 1;
        }

        /// <summary>根据id删除实体类信息(多删除)</summary>
        /// <param name="ids">id</param>
        ///<return>返回删除结果(多删除),大于0 返回删除成功(多删除),小于等于0 返回删除失败(多删除) </return>
        public virtual int DeleteList(Key[] ids)
        {
            return DALHelper.DeleteList<Model, Key>(NhibernateRepository.UnitWork, ids, GetTable(),DALHelper.IdColumn);
        }
        /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public virtual  Task<int> DeleteAsync(Key id, CancellationToken cancellationToken = default)
        {
            return  DALHelper.DeleteAsync<Model, Key>(id,cancellationToken);
        }

        /// <summary>根据id删除实体类信息(多删除)</summary>
        /// <param name="ids">id</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回删除结果(多删除),大于0 返回删除成功(多删除),小于等于0 返回删除失败(多删除) </return>
        public virtual  Task<int> DeleteListAsync(Key[] ids, CancellationToken cancellationToken = default)
        {
            return  NhibernateDALHelper<Model, Key>.DeleteListAsync(NhibernateRepository.UnitWork,ids,GetTable(), DALHelper.IdColumn, cancellationToken);
        }
    }

    /// <summary>Nhibernate 数据访问层接口基类 </summary>
    public    class BaseNhibernateDAL<Model>  where Model:class
    {
        /// <summary>
        /// 
        /// </summary>
        protected BaseNhibernateRepository<Model> NhibernateRepository { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        public BaseNhibernateDAL(ISession session)
        {
            this.NhibernateRepository = new BaseNhibernateRepository<Model>(session);
        }
        /// <summary>添加实体类信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回添加结果,大于0 返回添加成功,小于等于0 返回添加失败 </return>
        public virtual int Insert(Model obj)
        {
             NhibernateRepository.Insert(obj);//什么 编译器 提示 
             return 1;
        }

        /// <summary>修改实体类信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回修改结果,大于0 返回修改成功,小于等于0 返回修改失败 </return>
        public virtual int Update(Model obj)
        {
             NhibernateRepository.Update(obj);
            //NhibernateRepository.Session.Flush();
            return 1;
        }

        /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public int Remove(object id)
        {
             NhibernateRepository.Delete(id);
             return 1;
        }

        /// <summary>根据条件查询实体类数据集数量信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回实体类数据集数量信息</return>
        public virtual int Count(Model obj)
        {
            ICriteria criteria = GetWhere(obj);
            return NhibernateRepository.UnitWork.Count<Model>(criteria);
        }


        /// <summary>根据条件查询实体类数据集信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回实体类数据集信息</return>
        public virtual List<Model> FindList(Model obj)
        {
            ICriteria criteria = GetWhere(obj);
            return NhibernateDALHelper.FindList<Model>(NhibernateRepository.UnitWork, criteria);
        }

        /// <summary>
        /// 如果每次 逻辑 不同了 (就不能用同一个缓存了)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected virtual ICriteria GetWhere(Model obj)
        {
            List<AbstractCriterion> ors = new List<AbstractCriterion>();
            List<AbstractCriterion> ands = new List<AbstractCriterion>();
            this.QueryFilterByOr(ors, obj);
            this.QueryFilterByAnd(ands, obj);
            bool res = ors.Count > 0 || ands.Count > 0;
            ICriteria criteria = null;
            if (res)
            {
                criteria = NhibernateDALHelper.QueryWhere<Model>(NhibernateRepository.Session, ors, ands);
            }
            return criteria;
        }

        /// <summary>根据条件及分页查询实体类数据集信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<return>返回实体类数据集信息</return>
        public virtual List<Model> FindListByPage(Model obj,int page=1,int size=10)
        {
   
            //using (NhibernateRepository.Session.BeginTransaction())
            {
                ICriteria criteria = GetWhere(obj);
                return NhibernateDALHelper.FindListByPage<Model>(NhibernateRepository.UnitWork, criteria, page, size);
            }
        }

        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
        public virtual ResultModel<Model> FindResultModelByPage(Model obj,  int page=1, int size=10)
        {
            using (NhibernateRepository.Session.BeginTransaction())
            {
                ICriteria criteria = GetWhere(obj);
                return NhibernateDALHelper.FindResultModelByPage<Model>(NhibernateRepository.UnitWork, criteria, page, size);
            }
        }
       
        /// <summary>
        /// 模糊查询 通用查询 默认实现
        /// </summary>
        /// <param name="criterias"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected virtual void QueryFilterByAnd(List<AbstractCriterion> criterias, Model obj)
        {
    
        }

        /// <summary>
        /// 模糊查询 通用查询 默认实现
        /// </summary>
        /// <param name="criterias"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected virtual void QueryFilterByOr(List<AbstractCriterion> criterias, Model obj)
        {
            
        }

#region async
        /// <summary>添加实体类信息</summary>
        /// <param name="obj">实体类</param>
        ///<param name="cancellationToken"></param>
        ///<return>返回添加结果,大于0 返回添加成功,小于等于0 返回添加失败 </return>
        public virtual   Task<int> InsertAsync(Model obj,CancellationToken cancellationToken=default)
        {
            return  DALHelper.InsertAsync(NhibernateRepository.UnitWork, obj,cancellationToken);
        }

        /// <summary>修改实体类信息</summary>
        /// <param name="obj">实体类</param>
        ///<param name="cancellationToken"></param>
        ///<return>返回修改结果,大于0 返回修改成功,小于等于0 返回修改失败 </return>
        public virtual  Task<int> UpdateAsync(Model obj, CancellationToken cancellationToken = default)
        {
            return  DALHelper.UpdateAsync(NhibernateRepository.UnitWork, obj,cancellationToken);
        }

        /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        ///<param name="cancellationToken"></param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public  Task<int> RemoveAsync(object id, CancellationToken cancellationToken = default)
        {
            return  DALHelper.DeleteAsync<Model>(NhibernateRepository.UnitWork, id,cancellationToken);
        }


        /// <summary>根据条件查询实体类数据集数量信息</summary>
        /// <param name="obj">实体类</param>
        ///<param name="cancellationToken"></param>
        ///<return>返回实体类数据集数量信息</return>
        public virtual  Task<int> CountAsync(Model obj, CancellationToken cancellationToken = default)
        {
            using (NhibernateRepository.Session.BeginTransaction())
            {
                ICriteria criteria = GetWhere(obj);
                return  NhibernateDALHelper.CountAsync<Model>(NhibernateRepository.UnitWork, criteria, cancellationToken);
            }
        }

        /// <summary>根据条件查询实体类数据集信息</summary>
        /// <param name="obj">实体类</param>
        ///<param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息</return>
        public virtual  Task<List<Model>> FindListAsync(Model obj,  CancellationToken cancellationToken = default)
        {
            using (NhibernateRepository.Session.BeginTransaction())
            {
                ICriteria criteria = GetWhere(obj);
                return  NhibernateDALHelper.FindListAsync<Model>(NhibernateRepository.UnitWork, criteria, cancellationToken);
            }
        }

        /// <summary>根据条件及分页查询实体类数据集信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息</return>
        public virtual  Task<List<Model>> FindListByPageAsync(Model obj,  int page=1, int size=10, CancellationToken cancellationToken = default)
        {
            using (NhibernateRepository.Session.BeginTransaction())
            {
                ICriteria criteria = GetWhere(obj);
                return  NhibernateDALHelper.FindListByPageAsync<Model>(NhibernateRepository.UnitWork, criteria, page, size, cancellationToken);
            }
        }
        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
        public virtual  Task<ResultModel<Model>> FindResultModelByPageAsync(Model obj
            ,int page=1, int size=10, CancellationToken cancellationToken = default)
        {
            using(NhibernateRepository.Session.BeginTransaction())
            {
                ICriteria criteria = GetWhere(obj);
                return  NhibernateDALHelper.FindResultModelByPageAsync<Model>(NhibernateRepository.UnitWork, criteria, page, size, cancellationToken);
            }
        }
#endregion async
    }


  
}
#endif