using System.Collections.Generic;
using System.Threading;
#if !(NET20 || NET30 || NET35)
using System.Threading.Tasks;
#endif
using Utility.Enums;
using Utility.Model;
using Utility.Response;
using Utility.Cache;

namespace Utility.BLL
{
    /// <summary>
    ///统一 接口 返回 结果
    /// </summary>
    /// <typeparam name="Model"></typeparam>
    /// <typeparam name="Key"></typeparam>
    public interface IResponseApiBLL<Model, Key> where Model : class, IModel<Key>
    {


        /// <summary>添加实体类信息</summary>
        /// <param name="model">实体类</param>
        ///<param name="language"></param>
        ///<return>返回添加结果,大于0 返回添加成功,小于等于0 返回添加失败 </return>
        ResponseApi Insert(Model model, Language language = Language.Chinese);


        /// <summary>修改实体类信息</summary>
        /// <param name="model">实体类</param>
        ///<param name="language"></param>
        ///<return>返回修改结果,大于0 返回修改成功,小于等于0 返回修改失败 </return>
        ResponseApi Update(Model model, Language language = Language.Chinese);

        /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        ///<param name="language"></param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        ResponseApi Delete(Key id, Language language = Language.Chinese);

        /// <summary>根据id删除实体类信息(多删除)</summary>
        /// <param name="ids">id</param>
        ///<param name="language"></param>
        ///<return>返回删除结果(多删除),大于0 返回删除成功(多删除),小于等于0 返回删除失败(多删除) </return>
        ResponseApi DeleteList(Key[] ids, Language language = Language.Chinese);

        /// <summary>根据条件查询实体类数据集信息</summary>
        /// <param name="model">实体类</param>
        ///<param name="language"></param>
        ///<return>返回实体类数据集信息 </return>
        ResponseApi<List<Model>> FindList(Model model, Language language = Language.Chinese);

        /// <summary>根据条件查询实体类数据集数量信息</summary>
        /// <param name="model">实体类</param>
        ///<param name="language"></param>
        ///<return>返回实体类数据集数量信息</return>
        ResponseApi<int> Count(Model model, Language language = Language.Chinese);

        /// <summary>根据条件及分页查询实体类数据集信息</summary>
        /// <param name="model">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<param name="language"></param>
        ///<return>返回实体类数据集信息</return>
        ResponseApi<List<Model>> FindListByPage(Model model, int page, int size, Language language = Language.Chinese);


        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息</summary>
        /// <param name="model">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<param name="language"></param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
        ResponseApi<ResultModel<Model>> FindResultModelByPage(Model model, int page, int size, Language language = Language.Chinese);

#if !(NET20 || NET30 || NET35)

        /// <summary>添加实体类信息</summary>
        /// <param name="model">实体类</param>
        ///<param name="language"></param>
        ///<param name="cancellationToken"></param>
        ///<return>返回添加结果,大于0 返回添加成功,小于等于0 返回添加失败 </return>
        Task<ResponseApi> InsertAsync(Model model, Language language = Language.Chinese, CancellationToken cancellationToken = default);

        /// <summary>修改实体类信息</summary>
        /// <param name="model">实体类</param>
        ///<param name="language"></param>
        ///<param name="cancellationToken"></param>
        ///<return>返回修改结果,大于0 返回修改成功,小于等于0 返回修改失败 </return>
        Task<ResponseApi> UpdateAsync(Model model, Language language = Language.Chinese, System.Threading.CancellationToken cancellationToken = default);

        /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        ///<param name="language"></param>
        ///<param name="cancellationToken"></param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
       Task<ResponseApi> DeleteAsync(Key id, Language language = Language.Chinese, CancellationToken cancellationToken = default);



        /// <summary>根据id删除实体类信息(多删除)</summary>
        /// <param name="ids">id</param>
        ///<param name="language"></param>
        ///<param name="cancellationToken"></param>
        ///<return>返回删除结果(多删除),大于0 返回删除成功(多删除),小于等于0 返回删除失败(多删除) </return>
        Task<ResponseApi> DeleteListAsync(Key[] ids, Language language = Language.Chinese, CancellationToken cancellationToken = default);


        /// <summary>根据条件查询实体类数据集信息</summary>
        /// <param name="model">实体类</param>
        ///<param name="language"></param>
        ///<param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息 </return>
        Task<ResponseApi<List<Model>>> FindListAsync(Model model, Language language = Language.Chinese, CancellationToken cancellationToken = default);

        /// <summary>根据条件查询实体类数据集数量信息</summary>
        /// <param name="model">实体类</param>
        ///<param name="language"></param>
        ///<param name="cancellationToken"></param>
        ///<return>返回实体类数据集数量信息</return>
        Task<ResponseApi<int>> CountAsync(Model model, Language language = Language.Chinese,CancellationToken cancellationToken = default);


        /// <summary>根据条件及分页查询实体类数据集信息</summary>
        /// <param name="model">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<param name="language"></param>
        ///<param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息</return>
       Task<ResponseApi<List<Model>>> FindListByPageAsync(Model model, int page, int size, Language language = Language.Chinese,CancellationToken cancellationToken = default);


        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息</summary>
        /// <param name="model">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<param name="language"></param>
        ///<param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
       Task<ResponseApi<ResultModel<Model>>> FindResultModelByPageAsync(Model model, int page, int size, Language language = Language.Chinese, CancellationToken cancellationToken = default);
   
#endif
    }


    /// <summary>
    /// 统一 接口 返回 结果
    /// </summary>
    /// <typeparam name="Model"></typeparam>
    /// <typeparam name="Key"></typeparam>
    public interface IResponseApiService<Model, Key> : IResponseApiBLL<Model, Key> where Model : class, IModel<Key>
    {

    }

    /// <summary>
    /// 统一 接口 返回 结果
    /// </summary>
    /// <typeparam name="BLLImpl"></typeparam>
    /// <typeparam name="Model"></typeparam>
    /// <typeparam name="Key"></typeparam>
    public class ResponseApiBLL<BLLImpl,Model, Key> : IResponseApiBLL<Model, Key>
                where BLLImpl : IBLL<Model, Key>, new()
        where Model : class, IModel<Key>
    {
        /// <summary>
        /// 缓存
        /// </summary>

        protected virtual ICacheContent Cache { get; set; }
        /// <summary>
        /// 
        /// </summary>
        protected BLLImpl BLL { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ResponseApiBLL():this(new BLLImpl())
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bLL"></param>
        public ResponseApiBLL(BLLImpl bLL)
        {
            BLL = bLL;
        }


        /// <summary>添加实体类信息</summary>
        /// <param name="model">实体类</param>
        ///<param name="language"></param>
        ///<return>返回添加结果,大于0 返回添加成功,小于等于0 返回添加失败 </return>
        public virtual ResponseApi Insert(Model model, Language language = Language.Chinese)
        {
            var res =  BLL.Insert(model);
            return ResponseApi.Create(language, res > 0 ? Code.AddSuccess : Code.AddFail);
        }




        /// <summary>修改实体类信息</summary>
        /// <param name="model">实体类</param>
        ///<param name="language"></param>
        ///<return>返回修改结果,大于0 返回修改成功,小于等于0 返回修改失败 </return>
        public virtual ResponseApi Update(Model model, Language language = Language.Chinese)
        {
            var res =  BLL.Update(model);
            return ResponseApi.Create(language, res > 0 ? Code.ModifySuccess : Code.ModifyFail);
        }


        /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        ///<param name="language"></param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public virtual ResponseApi Delete(Key id, Language language = Language.Chinese)
       {
            var res =  BLL.Delete(id);
            return ResponseApi.Create(language, res > 0 ? Code.DeleteSuccess : Code.DeleteFail);
       }


        /// <summary>根据id删除实体类信息(多删除)</summary>
        /// <param name="ids">id</param>
        ///<param name="language"></param>
        ///<return>返回删除结果(多删除),大于0 返回删除成功(多删除),小于等于0 返回删除失败(多删除) </return>
       public virtual ResponseApi DeleteList(Key[] ids, Language language = Language.Chinese)
        {
            var res =  BLL.DeleteList(ids);
            return ResponseApi.Create(language, res > 0 ? Code.DeleteSuccess : Code.DeleteFail);
        }


        /// <summary>根据条件查询实体类数据集信息</summary>
        /// <param name="model">实体类</param>
        ///<param name="language"></param>
        ///<return>返回实体类数据集信息 </return>
        public virtual ResponseApi<List<Model>> FindList(Model model, Language language = Language.Chinese)
        {
            var res =  BLL.FindList(model);
            return ResponseApi<List<Model>>.Create(language, Code.QuerySuccess).SetData(res);
        }


        /// <summary>根据条件查询实体类数据集数量信息</summary>
        /// <param name="model">实体类</param>
        ///<param name="language"></param>
        ///<return>返回实体类数据集数量信息</return>
        public virtual ResponseApi<int> Count(Model model, Language language = Language.Chinese)
        {
            var res =  BLL.Count(model);
            return ResponseApi<int>.Create(language, Code.QuerySuccess).SetData(res);
        }


        /// <summary>根据条件及分页查询实体类数据集信息</summary>
        /// <param name="model">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<param name="language"></param>
        ///<return>返回实体类数据集信息</return>
        public virtual ResponseApi<List<Model>> FindListByPage(Model model, int page, int size, Language language = Language.Chinese)
        {
            var res =  BLL.FindListByPage(model, page, size);
            return ResponseApi<List<Model>>.Create(language, Code.QuerySuccess).SetData(res);
        }

        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息</summary>
        /// <param name="model">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<param name="language"></param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
        public virtual ResponseApi<ResultModel<Model>> FindResultModelByPage(Model model, int page, int size, Language language = Language.Chinese)
        {
            var res = BLL.FindResultModelByPage(model, page, size);
            return ResponseApi<ResultModel<Model>>.Create(language, Code.QuerySuccess).SetData(res);
        }

#if !(NET20 || NET30 || NET35)

        /// <summary>添加实体类信息</summary>
        /// <param name="model">实体类</param>
        ///<param name="language"></param>
        ///<param name="cancellationToken"></param>
        ///<return>返回添加结果,大于0 返回添加成功,小于等于0 返回添加失败 </return>

        public virtual Task<ResponseApi> InsertAsync(Model model, Language language = Language.Chinese, CancellationToken cancellationToken = default)
        { 
            var res =  BLL.InsertAsync(model, cancellationToken).Result;
            return new Task<ResponseApi>(() => ResponseApi.Create(language, res > 0 ? Code.AddSuccess : Code.AddFail));
        }

        /// <summary>修改实体类信息</summary>
        /// <param name="model">实体类</param>
        ///<param name="language"></param>
        ///<param name="cancellationToken"></param>
        ///<return>返回修改结果,大于0 返回修改成功,小于等于0 返回修改失败 </return>
        public virtual Task<ResponseApi> UpdateAsync(Model model, Language language = Language.Chinese,
            CancellationToken cancellationToken = default)
        {
            var res =  BLL.UpdateAsync(model, cancellationToken).Result;
            return new Task<ResponseApi>(() => ResponseApi.Create(language, res > 0 ? Code.ModifySuccess : Code.ModifyFail));

        }

        /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        ///<param name="language"></param>
        ///<param name="cancellationToken"></param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        public virtual Task<ResponseApi> DeleteAsync(Key id, Language language = Language.Chinese,
            CancellationToken cancellationToken = default)
        {
            var res =  BLL.DeleteAsync(id, cancellationToken).Result;
            return new Task<ResponseApi>(() => ResponseApi.Create(language, res > 0 ? Code.DeleteSuccess : Code.DeleteFail));
        }

        /// <summary>根据id删除实体类信息(多删除)</summary>
        /// <param name="ids">id</param>
        ///<param name="language"></param>
        ///<param name="cancellationToken"></param>
        ///<return>返回删除结果(多删除),大于0 返回删除成功(多删除),小于等于0 返回删除失败(多删除) </return>
        public virtual Task<ResponseApi> DeleteListAsync(Key[] ids, Language language = Language.Chinese,
            CancellationToken cancellationToken = default)
        {
            var res =  BLL.DeleteListAsync(ids, cancellationToken).Result;
            return new Task<ResponseApi>(() => ResponseApi.Create(language, res > 0 ? Code.DeleteSuccess : Code.DeleteFail));
        }

        /// <summary>根据条件查询实体类数据集信息</summary>
        /// <param name="model">实体类</param>
        ///<param name="language"></param>
        ///<param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息 </return>
        public virtual Task<ResponseApi<List<Model>>> FindListAsync(Model model, Language language = Language.Chinese,
            CancellationToken cancellationToken = default)
        {
            var res =  BLL.FindListAsync(model, cancellationToken).Result;
            return new Task<ResponseApi<List<Model>>>(() => ResponseApi<List<Model>>.Create(language, Code.QuerySuccess).SetData(res));
        }

        /// <summary>根据条件查询实体类数据集数量信息</summary>
        /// <param name="model">实体类</param>
        ///<param name="language"></param>
        ///<param name="cancellationToken"></param>
        ///<return>返回实体类数据集数量信息</return>
        public virtual Task<ResponseApi<int>> CountAsync(Model model, Language language = Language.Chinese,CancellationToken cancellationToken = default)
        {
            var res =  BLL.CountAsync(model, cancellationToken).Result;
            return new Task<ResponseApi<int>>(()=> ResponseApi<int>.Create(language, Code.QuerySuccess).SetData(res));
        }

        /// <summary>根据条件及分页查询实体类数据集信息</summary>
        /// <param name="model">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<param name="language"></param>
        ///<param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息</return>
        public virtual Task<ResponseApi<List<Model>>> FindListByPageAsync(Model model, int page, int size,
            Language language = Language.Chinese, CancellationToken cancellationToken = default)
        {
            var res =  BLL.FindListByPageAsync(model, page, size, cancellationToken).Result;
            return new Task<ResponseApi<List<Model>>>(()=> ResponseApi<List<Model>>.Create(language, Code.QuerySuccess).SetData(res));
        }

        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息</summary>
        /// <param name="model">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<param name="language"></param>
        ///<param name="cancellationToken"></param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
        public virtual Task<ResponseApi<ResultModel<Model>>> FindResultModelByPageAsync(Model model, int page, int size,
           Language language = Language.Chinese, CancellationToken cancellationToken = default)
        {
            var res = BLL.FindResultModelByPageAsync(model, page, size,cancellationToken).Result;
            return new Task<ResponseApi<ResultModel<Model>>>(()=> ResponseApi<ResultModel<Model>>.Create(language, Code.QuerySuccess).SetData(res));
        }
#endif

        }


    /// <summary>
    /// 统一 接口 返回 结果
    /// </summary>
    /// <typeparam name="ServiceImpl"></typeparam>
    /// <typeparam name="Model"></typeparam>
    /// <typeparam name="Key"></typeparam>
    public class ResponseApiService<ServiceImpl, Model, Key> : ResponseApiBLL<ServiceImpl, Model, Key>, IResponseApiService<Model, Key>
         where ServiceImpl : IService<Model, Key>, new()
        where Model : class, IModel<Key>
    {
        /// <summary>
        /// 
        /// </summary>
        public ResponseApiService() : this(new ServiceImpl())
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>

        public ResponseApiService(ServiceImpl service) : base(service)
        {

        }
    }

}
