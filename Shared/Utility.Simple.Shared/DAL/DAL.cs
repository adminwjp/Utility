using System;
using System.Collections.Generic;
using System.Text;
using Utility.Cache;
using Utility.Domain.Uow;
using Utility.Model;
#if !(NET20 || NET30 || NET35)
using System.Threading.Tasks;
#endif
using System.Threading;

namespace Utility.DAL
{

    /// <summary>数据访问层基类接口  </summary>
    public interface IDAL<Model, Key> where Model : class
    {
        
        /// <summary>添加实体类信息</summary>
        /// <param name="model">实体类</param>
        ///<return>返回添加结果,大于0 返回添加成功,小于等于0 返回添加失败 </return>
        int Insert(Model model);

        /// <summary>修改实体类信息</summary>
        /// <param name="model">实体类</param>
        ///<return>返回修改结果,大于0 返回修改成功,小于等于0 返回修改失败 </return>
        int Update(Model model);

        /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        int Delete(Key id);

        /// <summary>根据id删除实体类信息(多删除)</summary>
        /// <param name="ids">id</param>
        ///<return>返回删除结果(多删除),大于0 返回删除成功(多删除),小于等于0 返回删除失败(多删除) </return>
        int DeleteList(Key[] ids);

        /// <summary>根据条件查询实体类数据集信息</summary>
        /// <param name="model">实体类</param>
        ///<return>返回实体类数据集信息 </return>
        List<Model> FindList(Model model);

        /// <summary>根据条件查询实体类数据集数量信息</summary>
        /// <param name="model">实体类</param>
        ///<return>返回实体类数据集数量信息</return>
        int Count(Model model);


        /// <summary>根据条件及分页查询实体类数据集信息</summary>
        /// <param name="model">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<return>返回实体类数据集信息</return>
        List<Model> FindListByPage(Model model, int page, int size);

        ///// <summary>根据条件及分页查询实体类数据集信息</summary>
        ///// <param name="model">实体类</param>
        ///// <param name="order">排序字段</param>
        ///// <param name="sort">排序方式 asc desc</param>
        ///// <param name="page">页数</param>
        ///// <param name="size">每页记录</param>
        /////<return>返回实体类数据集信息</return>
        //List<Model> FindListByPage(Model model,string order,string sort, int page, int size);

        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息</summary>
        /// <param name="model">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
        ResultModel<Model> FindResultModelByPage(Model model, int page, int size);

        ///// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息</summary>
        ///// <param name="model">实体类</param>
        ///// <param name="page">页数</param>
        ///// <param name="size">每页记录</param>
        /////<return>返回实体类数据集信息和实体类数据集数量信息</return>
        //ResultModel<Model> FindResultModelByPage(Model model, string order, string sort, int page, int size);

#if !(NET20 || NET30 || NET35)
        /// <summary>添加实体类信息</summary>
        /// <param name="model">实体类</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回添加结果,大于0 返回添加成功,小于等于0 返回添加失败 </return>
        Task<int> InsertAsync(Model model, CancellationToken cancellationToken = default);

        /// <summary>修改实体类信息</summary>
        /// <param name="model">实体类</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回修改结果,大于0 返回修改成功,小于等于0 返回修改失败 </return>
        Task<int> UpdateAsync(Model model, CancellationToken cancellationToken = default);


        /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        Task<int> DeleteAsync(Key id, CancellationToken cancellationToken = default);


        /// <summary>根据id删除实体类信息(多删除)</summary>
        /// <param name="ids">id</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回删除结果(多删除),大于0 返回删除成功(多删除),小于等于0 返回删除失败(多删除) </return>
        Task<int> DeleteListAsync(Key[] ids, CancellationToken cancellationToken = default);


        /// <summary>根据条件查询实体类数据集信息</summary>
        /// <param name="model">实体类</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息 </return>
        Task<System.Collections.Generic.List<Model>> FindListAsync(Model model, CancellationToken cancellationToken = default);


        /// <summary>根据条件查询实体类数据集数量信息</summary>
        /// <param name="model">实体类</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集数量信息</return>
        Task<int> CountAsync(Model model,CancellationToken cancellationToken = default);

        /// <summary>根据条件及分页查询实体类数据集信息</summary>
        /// <param name="model">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息</return>
        Task<List<Model>> FindListByPageAsync(Model model, int page, int size,CancellationToken cancellationToken = default);

        ///// <summary>根据条件及分页查询实体类数据集信息</summary>
        ///// <param name="model">实体类</param>
        ///// <param name="page">页数</param>
        ///// <param name="size">每页记录</param>
        ///// <param name="cancellationToken"></param>
        /////<return>返回实体类数据集信息</return>
        //Task<List<Model>> FindListByPageAsync(Model model, string order, string sort, int page, int size, CancellationToken cancellationToken = default);


        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息</summary>
        /// <param name="model">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        /// <param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
        Task<Utility.Model.ResultModel<Model>> FindResultModelByPageAsync(Model model,int page,int size,CancellationToken cancellationToken=default);

        ///// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息</summary>
        ///// <param name="model">实体类</param>
        ///// <param name="page">页数</param>
        ///// <param name="size">每页记录</param>
        ///// <param name="cancellationToken"></param>
        /////<return>返回实体类数据集信息和实体类数据集数量信息</return>
        //Task<ResultModel<Model>> FindResultModelByPageAsync(Model model, string order, string sort, int page, int size, CancellationToken cancellationToken = default);

#endif

    }

    /// <summary>
    /// 数据访问层接口 IDAL 别名
    /// </summary>
    /// <typeparam name="Model">实体模型 实现于 <see cref="IModel{Key}"/></typeparam>
    /// <typeparam name="Key">实体主键类型</typeparam>
    public interface IManager<Model, Key> : IDAL<Model, Key> where Model : class, IModel<Key>
    {
    }

    /// <summary>
    /// 基类  未 任何 实现 用于 aop
    /// </summary>
    public  class DAL
    {  
        /// <summary>
       /// 实体  数据访问层接口 实现 基类
       /// </summary>
        protected virtual IUnitWork UnitWork { get; set; }
        /// <summary>
        /// 缓存
        /// </summary>

        protected virtual ICacheContent Cache { get; set; }
    } 

    /// <summary>
    /// 基类  未 任何 实现 用于 aop
    /// </summary>
    public  class Manager:DAL
    {

    }
}
