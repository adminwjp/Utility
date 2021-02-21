using System;
using System.Threading;
#if !(NET20 || NET30 || NET35)
using System.Threading.Tasks;
#endif
using Utility.Cache;
using Utility.Domain.Uow;
using Utility.Model;

namespace Utility.DAL
{
    /// <summary>实体  数据访问层接口 实现 基类  </summary>
    public class BaseDAL<Model, Key> : BaseDAL<Model> where Model : class, IModel<Key>
    {
        /// <summary>
        /// set IUnitWork
        /// </summary>
        /// <param name="unitWork">unitWork</param>
        public BaseDAL(IUnitWork unitWork) : base(unitWork)
        {
        }

        /// <summary>
        /// table name
        /// </summary>
        /// <returns></returns>
        protected virtual string GetTable()
        {
            return string.Empty;
        }

        /// <summary>根据id删除用实体类信息</summary>
        /// <param name="id">实体类 id</param>
        ///<return>返回删除实体类信息结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public virtual int Delete(Key id)
        {
            return DALHelper<Model, Key>.Delete(UnitWork, id);
        }

        /// <summary>根据id删除实体类信息(多删除)</summary>
        /// <param name="ids">id</param>
        ///<return>返回删除结果(多删除),大于0 返回删除成功(多删除),小于等于0 返回删除失败(多删除) </return>
        public virtual int DeleteList(Key[] ids)
        {
            return DALHelper.DeleteList<Model, Key>(UnitWork, ids, GetTable(), DALHelper.IdColumn);
        }
#if !(NET20 || NET30 || NET35)
        /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public virtual  Task<int> DeleteAsync(Key id,CancellationToken cancellationToken = default)
        {
            return  DALHelper<Model, Key>.DeleteAsync(UnitWork, id,cancellationToken);
        }

        /// <summary>根据id删除实体类信息(多删除)</summary>
        /// <param name="ids">id</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回删除结果(多删除),大于0 返回删除成功(多删除),小于等于0 返回删除失败(多删除) </return>
        public virtual  Task<int> DeleteListAsync(Key[] ids, CancellationToken cancellationToken = default)
        {
            return  DALHelper<Model, Key>.DeleteListAsync(UnitWork, ids,GetTable(), DALHelper.IdColumn, cancellationToken);
        }

#endif
    }


    /// <summary> 数据访问层接口基类</summary>
    public    class BaseDAL<Model> :BaseDAL where Model:class
    {
        /// <summary>
        /// 构造注入
        /// </summary>
        /// <param name="unitWork"></param>
        public BaseDAL(IUnitWork unitWork):base(unitWork)
        {
            
        }

        /// <summary>添加实体类信息</summary>
        /// <param name="model">实体类</param>
        ///<return>返回添加结果,大于0 返回添加成功,小于等于0 返回添加失败 </return>
        public virtual int Insert(Model model)
        {
            return DALHelper<Model>.Insert(UnitWork, model);
        }

        /// <summary>修改实体类信息</summary>
        /// <param name="model">实体类</param>
        ///<return>返回修改结果,大于0 返回修改成功,小于等于0 返回修改失败 </return>
        public virtual int Update(Model model)
        {
            return DALHelper<Model>.Update(UnitWork, model);
        }

        /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public virtual int Delete(object id)
        {
            return DALHelper<Model>.Delete(UnitWork, id);
        }


        #region async
#if !(NET20 || NET30 || NET35)
        /// <summary>添加实体类信息</summary>
        /// <param name="model">实体类</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回添加结果,大于0 返回添加成功,小于等于0 返回添加失败 </return>
        public virtual   Task<int> InsertAsync(Model model,CancellationToken cancellationToken=default)
        {
            return  DALHelper<Model>.InsertAsync(UnitWork, model,cancellationToken);
        }

        /// <summary>修改实体类信息</summary>
        /// <param name="model">实体类</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回修改结果,大于0 返回修改成功,小于等于0 返回修改失败 </return>
        public virtual  Task<int> UpdateAsync(Model model, CancellationToken cancellationToken = default)
        {
            return  DALHelper<Model>.UpdateAsync(UnitWork,model,cancellationToken);
        }

        /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public Task<int> DeleteAsync(object id, CancellationToken cancellationToken = default)
        {
            return  DALHelper<Model>.DeleteAsync(UnitWork, id,cancellationToken);
        }
#endif
#endregion async
    }


    /// <summary>
    /// 实体  数据访问层接口  基类
    /// </summary>
    public class BaseDAL:DAL
    {
        /// <summary>
        /// 实体  数据访问层接口  基类
        /// </summary>
        /// <param name="unitWork"></param>
        public BaseDAL(IUnitWork unitWork)
        {
            UnitWork = unitWork;
        }
    }

    /// <summary>实体  数据访问层接口 实现 基类  </summary>
    public class BaseManager<Model, Key> : BaseDAL<Model,Key> where Model : class, IModel<Key>
    {
        /// <summary>
        /// 构造注入
        /// </summary>
        /// <param name="unitWork"></param>
        public BaseManager(IUnitWork unitWork) : base(unitWork)
        {

        }
    }

    /// <summary> 数据访问层接口基类</summary>
    public class BaseManager<Model> : BaseDAL<Model> where Model : class
    {
        /// <summary>
        /// 构造注入
        /// </summary>
        /// <param name="unitWork"></param>
        public BaseManager(IUnitWork unitWork) : base(unitWork)
        {

        }
    }

    /// <summary>
    /// 实体  数据访问层接口  基类
    /// </summary>
    public class BaseManager : BaseDAL
    {
        /// <summary>
        /// 构造注入
        /// </summary>
        /// <param name="unitWork"></param>
        public BaseManager(IUnitWork unitWork) : base(unitWork)
        {

        }
    }
}