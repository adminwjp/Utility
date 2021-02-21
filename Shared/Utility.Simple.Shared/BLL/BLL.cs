using System.Collections.Generic;
using System.Threading;
#if !(NET20 || NET30 || NET35)
using System.Threading.Tasks;
#endif
using Utility.DAL;
using Utility.Model;
using Utility.Cache;

namespace Utility.BLL
{
    /// <summary>业务层基类接口  </summary>
    public interface IBLL<Model, Key> : IDAL<Model, Key> where Model : class, IModel<Key>
    {

    }

    /// <summary>
    /// 数据访问层接口 IBLL 别名
    /// </summary>
    /// <typeparam name="Model">实体模型 实现于 <see cref="IModel{Key}"/></typeparam>
    /// <typeparam name="Key">实体主键类型</typeparam>
    public interface IService<Model, Key> : IBLL<Model, Key> where Model : class, IModel<Key>
    {
    }
    /// <summary>
    /// 基类 用于 aop
    /// </summary>
    public class BLL
    {
        /// <summary>
        /// 缓存
        /// </summary>

        protected virtual ICacheContent Cache { get; set; }

    }

    /// <summary>
    /// 基类 用于 aop
    /// </summary>
    public class Service:BLL
    {

    }
    /// <summary>
    /// 业务逻辑层 
    /// </summary>
    /// <typeparam name="DALImpl">数据 访问 层</typeparam>
    /// <typeparam name="Model">模型层</typeparam>
    /// <typeparam name="Key">主键</typeparam>
    public class BLL<DALImpl,Model, Key> : BLL, IBLL<Model, Key> 
        where DALImpl : IDAL<Model, Key>
        where Model : class, IModel<Key>
    {
     
        /// <summary>
        /// 数据 访问 层
        /// </summary>
        protected  DALImpl DAL;//单个使用 可以 如果有多个这样的 接口 实现方式 多种 可读性 太差
        /// <summary>
        /// 业务逻辑层
        /// </summary>
        /// <param name="dAL">数据 访问 层</param>
        public BLL(DALImpl dAL)
        {
            this.DAL = dAL;
        }

        /// <summary>添加实体类信息</summary>
        /// <param name="model">实体类</param>
        ///<return>返回添加结果,大于0 返回添加成功,小于等于0 返回添加失败 </return>
        public virtual int Insert(Model model)
        {
            return DAL.Insert(model);
        }

        /// <summary>修改实体类信息</summary>
        /// <param name="model">实体类</param>
        ///<return>返回修改结果,大于0 返回修改成功,小于等于0 返回修改失败 </return>
        public virtual int Update(Model model)
        {
            return DAL.Update(model);
        }


        /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public virtual int Delete(Key id)
        {
            return DAL.Delete(id);
        }

        /// <summary>根据id删除实体类信息(多删除)</summary>
        /// <param name="ids">id</param>
        ///<return>返回删除结果(多删除),大于0 返回删除成功(多删除),小于等于0 返回删除失败(多删除) </return>
        public virtual int DeleteList(Key[] ids)
        {
            return DAL.DeleteList(ids);
        }

        /// <summary>根据条件查询实体类数据集信息</summary>
        /// <param name="model">实体类</param>
        ///<return>返回实体类数据集信息 </return>
        public virtual List<Model> FindList(Model model)
        {
            return DAL.FindList(model);
        }

       

        /// <summary>根据条件查询实体类数据集数量信息</summary>
        /// <param name="model">实体类</param>
        ///<return>返回实体类数据集数量信息</return>
        public virtual int Count(Model model)
        {
            return DAL.Count(model);
        }

        /// <summary>根据条件及分页查询实体类数据集信息</summary>
        /// <param name="model">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<return>返回实体类数据集信息</return>
        public virtual List<Model> FindListByPage(Model model, int page, int size)
        {
            return DAL.FindListByPage(model, page, size);
        }

    
        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息</summary>
        /// <param name="model">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
        public virtual ResultModel<Model> FindResultModelByPage(Model model, int page, int size)
        {
            return DAL.FindResultModelByPage(model, page, size);
        }

#if !(NET20 || NET30 || NET35)
        /// <summary>添加实体类信息</summary>
        /// <param name="model">实体类</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回添加结果,大于0 返回添加成功,小于等于0 返回添加失败 </return>
        public virtual Task<int> InsertAsync(Model model, CancellationToken cancellationToken = default)
        {
            return DAL.InsertAsync(model, cancellationToken);
        }

        /// <summary>修改实体类信息</summary>
        /// <param name="model">实体类</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回修改结果,大于0 返回修改成功,小于等于0 返回修改失败 </return>
        public virtual Task<int> UpdateAsync(Model model,CancellationToken cancellationToken = default)
        {
            return DAL.UpdateAsync(model, cancellationToken);
        }

        /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public virtual Task<int> DeleteAsync(Key id,CancellationToken cancellationToken = default)
        {
            return DAL.DeleteAsync(id, cancellationToken);
        }

        /// <summary>根据id删除实体类信息(多删除)</summary>
        /// <param name="ids">id</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回删除结果(多删除),大于0 返回删除成功(多删除),小于等于0 返回删除失败(多删除) </return>
        public virtual Task<int> DeleteListAsync(Key[] ids,
            CancellationToken cancellationToken = default)
        {
            return DAL.DeleteListAsync(ids, cancellationToken);
        }

        /// <summary>根据条件查询实体类数据集信息</summary>
        /// <param name="model">实体类</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息 </return>
        public virtual Task<List<Model>> FindListAsync(Model model,
            CancellationToken cancellationToken = default)
        {
            return DAL.FindListAsync(model, cancellationToken);
        }


        /// <summary>根据条件查询实体类数据集数量信息</summary>
        /// <param name="model">实体类</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集数量信息</return>
        public virtual Task<int> CountAsync(Model model,
            CancellationToken cancellationToken = default)
        {
            return DAL.CountAsync(model, cancellationToken);
        }

        /// <summary>根据条件及分页查询实体类数据集信息</summary>
        /// <param name="model">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息</return>
        public virtual Task<List<Model>> FindListByPageAsync(Model model, int page, int size,
            CancellationToken cancellationToken = default)
        {
            return DAL.FindListByPageAsync(model, page, size, cancellationToken);
        }

        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息</summary>
        /// <param name="model">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
        public virtual Task<ResultModel<Model>> FindResultModelByPageAsync(Model model, int page, int size,
            CancellationToken cancellationToken = default)
        {
            return DAL.FindResultModelByPageAsync(model, page, size, cancellationToken);
        }

#endif

    }

    /// <summary>
    /// 业务逻辑层
    /// </summary>
    /// <typeparam name="ManagerImpl">服务事项</typeparam>
    /// <typeparam name="Model">模型</typeparam>
    /// <typeparam name="Key">主键</typeparam>
    public class Serivce<ManagerImpl, Model, Key> : BLL<ManagerImpl, Model, Key>, IManager<Model, Key>
        where ManagerImpl : IManager<Model, Key>
        where Model : Utility.Model.Model<Key>, IModel<Key>, new()
    {
        /// <summary>
        /// 业务逻辑层
        /// </summary>
        /// <param name="dAL">数据访问层</param>
        public Serivce(ManagerImpl dAL) : base(dAL)
        {

        }
    }
}
