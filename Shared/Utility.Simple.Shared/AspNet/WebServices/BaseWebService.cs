#if NET35 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET472 || NET48
#if !NET35
using System.Threading.Tasks;
#endif
using System.Web.Services;
using Utility.BLL;
using Utility.Enums;
using Utility.Model;
using Utility.Response;

namespace Utility.AspNet.WebServices
{
    /// <summary>
    /// BaseService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    [System.Web.Script.Services.ScriptService]

    public abstract class BaseWebService<ResponseApiBLL, T, Key, BLL> : System.Web.Services.WebService
        where ResponseApiBLL :  Utility.BLL.ResponseApiBLL<BLL,T, Key>
        where BLL : IBLL<T, Key>, new()
        where T : class,IModel<Key>
    {
        /// <summary>
        /// 
        /// </summary>
        protected ResponseApiBLL ApiBLL { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual Language GetLanguage()
        {
            return Language.Chinese;
        }
        

        /// <summary>添加实体类信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回添加结果,大于0 返回添加成功,小于等于0 返回添加失败 </return>
        [WebMethod()]
        public virtual ResponseApi Insert(T obj)
        {
            var res = ApiBLL.Insert(obj, GetLanguage());
            return res;
        }



        /// <summary>修改实体类信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回修改结果,大于0 返回修改成功,小于等于0 返回修改失败 </return>
        [WebMethod()]
        public virtual ResponseApi Update(T obj)
        {

            var res = ApiBLL.Update(obj, GetLanguage());
            return res;
        }

       

        /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        [WebMethod()]
        public virtual ResponseApi Delete(Key id)
        {
            var res = ApiBLL.Delete(id, GetLanguage());
            return res;
        }
       

        /// <summary>根据id删除实体类信息(多删除)</summary>
        /// <param name="ids">id</param>
        ///<return>返回删除结果(多删除),大于0 返回删除成功(多删除),小于等于0 返回删除失败(多删除) </return>
        [WebMethod()]
        public virtual ResponseApi DeleteList(DeleteModel<Key> ids)
        {

            var res = ApiBLL.DeleteList(ids.Ids, GetLanguage());
            return res;
        }

      

        /// <summary>根据条件查询实体类数据集信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回实体类数据集信息 </return>
        [WebMethod()]
        public virtual  ResponseApi<System.Collections.Generic.List<T>> FindList( T obj)
        {
            var res = ApiBLL.FindList(obj, GetLanguage());
            return res;
        }


     
        /// <summary>根据条件查询实体类数据集数量信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回实体类数据集数量信息</return>
        [WebMethod()]
        public virtual ResponseApi<int> Count( T obj)
        {
            var res = ApiBLL.Count(obj, GetLanguage());
            return res;
        }


        /// <summary>根据条件及分页查询实体类数据集信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<return>返回实体类数据集信息</return>
        [WebMethod()]
        public  virtual ResponseApi<System.Collections.Generic.List<T>> FindListByPage(T obj, int page, int size)
        {
            var res = ApiBLL.FindListByPage(obj, page, size, GetLanguage());
            return res;
        }

        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
        [WebMethod()]
        public virtual ResponseApi<Utility.Model.ResultModel<T>> FindResultModelByPage(T obj, int page, int size)
        {
            var res = ApiBLL.FindResultModelByPage(obj, page, size, GetLanguage());
            return res;
        }
#if !NET35

        /// <summary>添加实体类信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回添加结果,大于0 返回添加成功,小于等于0 返回添加失败 </return>
        [WebMethod()]
        public virtual Task<ResponseApi> InsertAsync(T obj)
        {
            var res = ApiBLL.InsertAsync(obj, GetLanguage());
            return res;
        }

        /// <summary>修改实体类信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回修改结果,大于0 返回修改成功,小于等于0 返回修改失败 </return>
        [WebMethod()]
        public virtual System.Threading.Tasks.Task<ResponseApi> UpdateAsync(T obj)
        {
            var res = ApiBLL.UpdateAsync(obj, GetLanguage());
            return res;
        }

        /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        [WebMethod()]
        public virtual System.Threading.Tasks.Task<ResponseApi> DeleteAsync(Key id)
        {
            var res = ApiBLL.DeleteAsync(id, GetLanguage());
            return res;
        }

        /// <summary>根据id删除实体类信息(多删除)</summary>
        /// <param name="ids">id</param>
        ///<return>返回删除结果(多删除),大于0 返回删除成功(多删除),小于等于0 返回删除失败(多删除) </return>
        [WebMethod()]
        public virtual System.Threading.Tasks.Task<ResponseApi> DeleteListAsync(DeleteModel<Key> ids)
        {
            var res = ApiBLL.DeleteListAsync(ids.Ids, GetLanguage());
            return res;
        }

        /// <summary>根据条件查询实体类数据集信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回实体类数据集信息 </return>
        [WebMethod()]
        public virtual System.Threading.Tasks.Task<ResponseApi<System.Collections.Generic.List<T>>> FindListAsync(T obj)
        {
            var res = ApiBLL.FindListAsync(obj, GetLanguage());
            return res;
        }

        /// <summary>根据条件查询实体类数据集数量信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回实体类数据集数量信息</return>
        [WebMethod()]
        public  virtual System.Threading.Tasks.Task<ResponseApi<int>> CountAsync(T obj)
        {
            var res = ApiBLL.CountAsync(obj, GetLanguage());
            return res;
        }

        /// <summary>根据条件及分页查询实体类数据集信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<return>返回实体类数据集信息</return>
        [WebMethod()]
        public virtual System.Threading.Tasks.Task<ResponseApi<System.Collections.Generic.List<T>>> FindListByPageAsync(T obj, int page, int size)
        {
            var res = ApiBLL.FindListByPageAsync(obj, page, size, GetLanguage());
            return res;
        }

        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
        [WebMethod()]
        public virtual System.Threading.Tasks.Task<ResponseApi<Utility.Model.ResultModel<T>>> FindResultModelByPageAsync(T obj, int page, int size)
        {
            var res = ApiBLL.FindResultModelByPageAsync(obj, page, size, GetLanguage());
            return res;
        }

#endif
    }
}
#endif