#if NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Xml.Serialization;
using Utility.BLL;
using Utility.DAL;
using Utility.Enums;
using Utility.IO;
using Utility.Json;
using Utility.Model;
using Utility.Response;

namespace Utility.AspNet.Controllers
{
    /// <summary>
    /// [FromForm,FromBody] 只支持 FromForm Multiple actions were found that match the request
    /// "Config/api/{controller}/{action}/{id}", 
    /// //webform mvc webapi 混合搭配时 值 接受不到 
    /// </summary>
    /// <typeparam name="ResponseApiBLL"></typeparam>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="Key"></typeparam>
    /// <typeparam name="BLL"></typeparam>
    [Route("api/[version]/[controller]")]
    
    public abstract class BaseController<ResponseApiBLL, BLL, T, Key> : ApiController 
        where ResponseApiBLL : ResponseApiBLL<BLL,T, Key>
        where BLL : IBLL<T, Key>, new()
        where T : class, IModel<Key>
    {
        protected ResponseApiBLL ApiBLL { get; set; }
        protected virtual Language GetLanguage()
        {
            return Language.Chinese;
        }
        /// <summary>添加实体类信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回添加结果,大于0 返回添加成功,小于等于0 返回添加失败 </return>
        [HttpPost()]
        [Route("InsertAsync")]
        public virtual Task<ResponseApi> InsertAsync([FromBody]T obj)
        {
            //共享作用域不存在 数据没有
            if (Request.Content.Headers.ContentType != null)
            {
                if (Request.Content.Headers.ContentType.MediaType.Contains("application/json"))
                {
                    Ref(ref obj, StreamHelper.GetString(Request.Content.ReadAsStreamAsync().Result));
                }
                else if (Request.Content.Headers.ContentType.MediaType.Contains("text/xml"))
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(Request.Content.ReadAsStreamAsync().Result))
                    {
                        Type t = typeof(T);
                        XmlSerializer serializer = new XmlSerializer(t);
                        obj = serializer.Deserialize(reader) as T;
                    }
                }
            }
            var res = ApiBLL.InsertAsync(obj, GetLanguage());
            return res;
        }

        /// <summary>添加实体类信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回添加结果,大于0 返回添加成功,小于等于0 返回添加失败 </return>
        [HttpPost]
        [Route("Insert")]
        public virtual ResponseApi Insert([FromBody] T obj)
        {
           // Ref(ref obj, StreamHelper.GetString(Request.Content.ReadAsStreamAsync().Result));//webform mvc webapi 混合搭配时 值 接受不到 
            var res = ApiBLL.Insert(obj, GetLanguage());
            return res;
        }


        /// <summary>修改实体类信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回修改结果,大于0 返回修改成功,小于等于0 返回修改失败 </return>
        [HttpPost]
        [Route("UpdateAsync")]
        public virtual Task<ResponseApi> UpdateAsync([FromBody] T obj)
        {
            var res = ApiBLL.UpdateAsync(obj, GetLanguage());
            return res;
        }

        /// <summary>修改实体类信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回修改结果,大于0 返回修改成功,小于等于0 返回修改失败 </return>
        [HttpPost]
        [Route("Update")]
        public virtual ResponseApi Update( [FromBody] T obj)
        {
           
            var res = ApiBLL.Update(obj, GetLanguage());
            return res;
        }

        /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        [HttpGet]
        [Route("DeleteAsync")]
        public virtual Task<ResponseApi> DeleteAsync(Key id)
        {
            var res = ApiBLL.DeleteAsync(id, GetLanguage());
            return res;
        }

        /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        [HttpGet]
        [Route("Delete")]
        public virtual ResponseApi Delete(Key id)
        {
            var res = ApiBLL.Delete(id, GetLanguage());
            return res;
        }

        /// <summary>根据id删除实体类信息(多删除)</summary>
        /// <param name="ids">id</param>
        ///<return>返回删除结果(多删除),大于0 返回删除成功(多删除),小于等于0 返回删除失败(多删除) </return>
        [HttpPost]
        [Route("DeleteListAsync")]
        public virtual Task<ResponseApi> DeleteListAsync([ FromBody] DeleteModel<Key> ids)
        {
            //共享作用域不存在 数据没有
            if (Request.Content.Headers.ContentType != null)
            {
                if (Request.Content.Headers.ContentType.MediaType.Contains("application/json"))
                {
                    Ref(ref ids, StreamHelper.GetString(Request.Content.ReadAsStreamAsync().Result));
                }
                else if (Request.Content.Headers.ContentType.MediaType.Contains("text/xml"))
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(Request.Content.ReadAsStreamAsync().Result))
                    {
                        Type t = typeof(DeleteModel<Key>);
                        XmlSerializer serializer = new XmlSerializer(t);
                        ids = serializer.Deserialize(reader) as DeleteModel<Key>;
                    }
                }
            }
            var res = ApiBLL.DeleteListAsync(ids.Ids, GetLanguage());
            return res;
        }

        /// <summary>根据id删除实体类信息(多删除)</summary>
        /// <param name="ids">id</param>
        ///<return>返回删除结果(多删除),大于0 返回删除成功(多删除),小于等于0 返回删除失败(多删除) </return>
        [HttpPost]
        [Route("DeleteList")]
        public virtual ResponseApi DeleteList([FromBody] DeleteModel<Key> ids)
        {
            
            var res = ApiBLL.DeleteList(ids.Ids, GetLanguage());
            return res;
        }

        /// <summary>根据条件查询实体类数据集信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回实体类数据集信息 </return>
        [HttpPost]
        [Route("FindListAsync")]
        public virtual Task<ResponseApi<List<T>>> FindListAsync([ FromBody] T obj)
        {
            var res = ApiBLL.FindListAsync(obj, GetLanguage());
            return res;
        }

        /// <summary>根据条件查询实体类数据集信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回实体类数据集信息 </return>
        [HttpPost]
        [Route("FindList")]
        public virtual ResponseApi<List<T>> FindList([FromBody] T obj)
        {
            var res = ApiBLL.FindList(obj, GetLanguage());
            return res;
        }


        /// <summary>根据条件查询实体类数据集数量信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回实体类数据集数量信息</return>
        [HttpPost]
        [Route("CountAsync")]
        public virtual Task<ResponseApi<int>> CountAsync([FromBody] T obj)
        {
            var res = ApiBLL.CountAsync(obj, GetLanguage());
            return res;
        }
        /// <summary>根据条件查询实体类数据集数量信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回实体类数据集数量信息</return>
        [HttpPost]
        [Route("Count")]
        public virtual ResponseApi<int> Count([FromBody] T obj)
        {
            var res = ApiBLL.Count(obj, GetLanguage());
            return res;
        }

        /// <summary>根据条件及分页查询实体类数据集信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        /// <param name="sortOrder"></param>
        ///<return>返回实体类数据集信息</return>
        [HttpPost]
        [Route("FindListByPageAsync")]
        public virtual Task<ResponseApi<List<T>>> FindListByPageAsync([FromBody] T obj, int page, int size,string sortOrder)
        {
            var res = ApiBLL.FindListByPageAsync(obj, page, size, GetLanguage());
            return res;
        }

        /// <summary>根据条件及分页查询实体类数据集信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        /// <param name="sortOrder"></param>
        ///<return>返回实体类数据集信息</return>
        [HttpPost]
        [Route("FindListByPage")]
        public virtual ResponseApi<List<T>> FindListByPage([ FromBody] T obj, int page, int size, string sortOrder)
        {
            var res = ApiBLL.FindListByPage(obj, page, size, GetLanguage());
            return res;
        }

        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        /// <param name="sortOrder"></param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
        [HttpPost]
        [Route("FindResultModelByPage")]
        public virtual ResponseApi<ResultModel<T>> FindResultModelByPage([FromBody] T obj, int page, int size, string sortOrder)
        {
            var res = ApiBLL.FindResultModelByPage(obj, page, size, GetLanguage());
            return res;
        }

        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        /// <param name="sortOrder"></param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
        [HttpPost]
        [Route("FindResultModelByPageAsync")]
        public virtual Task<ResponseApi<ResultModel<T>>> FindResultModelByPageAsync([FromBody] T obj, int page, int size, string sortOrder)
        {
            var res = ApiBLL.FindResultModelByPageAsync(obj, page, size, GetLanguage());
            return res;
        }
        /// <summary>
        /// formbody 数据转换
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="str"></param>
        protected virtual void Ref<M>(ref M obj, string str) where M : class
        {
            obj = JsonHelper.ToObject<M>(str, JsonHelper.JsonSerializerSettings);
        }
    }

    /// <summary>
    /// [FromForm,FromBody] 只支持 FromForm
    /// </summary>
    /// <typeparam name="ResponseApiBLL"></typeparam>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="Key"></typeparam>
    /// <typeparam name="BLL"></typeparam>
    /// <typeparam name="DAL"></typeparam>
    [Route("api/[version]/[controller]")]

    public abstract class BaseController<ResponseApiBLL, BLL, DAL, T, Key> : ApiController
        where ResponseApiBLL : ResponseApiBLLHelper<BLL, DAL,T, Key>
        where BLL : BLLHelper<DAL,T, Key>
        where DAL : DALHelper<T, Key>
        where T : class, IModel<Key>
    {
        private static Type apiBLLType;

        /// <summary>
        /// 
        /// </summary>
        //DAL.Method 不支持这种语法
        protected static Type GetApiBLLType()
        {
            return apiBLLType;
        }

        /// <summary>
        /// apiBLLType.Method 不支持这种语法
        /// </summary>
        protected static void SetApiBLLType(Type value)
        {
            apiBLLType = value;
        }

        public BaseController()
        {
            apiBLLType = typeof(ResponseApiBLL);
        }
        protected virtual Language GetLanguage()
        {
            return Language.Chinese;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static MethodInfo GetMethodInfo(string name)
        {
            return apiBLLType.BaseType.GetMethod(name, BindingFlags.Static | BindingFlags.Public | BindingFlags.InvokeMethod);
        }
        /// <summary>添加实体类信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回添加结果,大于0 返回添加成功,小于等于0 返回添加失败 </return>
        [HttpPost]
        public virtual Task<ResponseApi> InsertAsync([FromBody] T obj)
        {
            //共享作用域不存在 数据没有
            if (Request.Content.Headers.ContentType != null)
            {
                if (Request.Content.Headers.ContentType.MediaType.Contains("application/json"))
                {
                    Ref(ref obj, StreamHelper.GetString(Request.Content.ReadAsStreamAsync().Result));
                }
                else if (Request.Content.Headers.ContentType.MediaType.Contains("text/xml"))
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(Request.Content.ReadAsStreamAsync().Result))
                    {
                        Type t = typeof(T);
                        XmlSerializer serializer = new XmlSerializer(t);
                        obj = serializer.Deserialize(reader) as T;
                    }
                }
            }
            var res=(Task<ResponseApi>)GetMethodInfo("InsertAsync").Invoke(null, new object[] { obj, GetLanguage() });
            return res;
        }

        /// <summary>添加实体类信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回添加结果,大于0 返回添加成功,小于等于0 返回添加失败 </return>
        [HttpPost]
        public virtual ResponseApi Insert([FromBody] T obj)
        {
            Ref(ref obj, StreamHelper.GetString(Request.Content.ReadAsStreamAsync().Result));//webform mvc webapi 混合搭配时 值 接受不到 
            var res = (ResponseApi)GetMethodInfo("Insert").Invoke(null, new object[] { obj, GetLanguage() });
            return res;
        }


        /// <summary>修改实体类信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回修改结果,大于0 返回修改成功,小于等于0 返回修改失败 </return>
        [HttpPost]
        public virtual Task<ResponseApi> UpdateAsync([FromBody] T obj)
        {
            var res = (Task<ResponseApi>)GetMethodInfo("UpdateAsync").Invoke(null, new object[] { obj, GetLanguage() });
            return res;
        }

        /// <summary>修改实体类信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回修改结果,大于0 返回修改成功,小于等于0 返回修改失败 </return>
        [HttpPost]
        public virtual ResponseApi Update([FromBody] T obj)
        {
            var res = (ResponseApi)GetMethodInfo("Update").Invoke(null, new object[] { obj, GetLanguage() });
            return res;
        }

        /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        [HttpGet]
        public virtual Task<ResponseApi> DeleteAsync(Key id)
        {
            var res = (Task<ResponseApi>)GetMethodInfo("DeleteAsync").Invoke(null, new object[] { id, GetLanguage() });
            return res;
        }

        /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        [HttpGet]
        public virtual ResponseApi Delete(Key id)
        {
            var res = (ResponseApi)GetMethodInfo("Delete").Invoke(null, new object[] { id, GetLanguage() });
            return res;
        }

        /// <summary>根据id删除实体类信息(多删除)</summary>
        /// <param name="ids">id</param>
        ///<return>返回删除结果(多删除),大于0 返回删除成功(多删除),小于等于0 返回删除失败(多删除) </return>
        [HttpPost]
        public virtual Task<ResponseApi> DeleteListAsync([FromBody] DeleteModel<Key> ids)
        {
            //共享作用域不存在 数据没有
            if (Request.Content.Headers.ContentType != null)
            {
                if (Request.Content.Headers.ContentType.MediaType.Contains("application/json"))
                {
                    Ref(ref ids, StreamHelper.GetString(Request.Content.ReadAsStreamAsync().Result));
                }
                else if (Request.Content.Headers.ContentType.MediaType.Contains("text/xml"))
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(Request.Content.ReadAsStreamAsync().Result))
                    {
                        Type t = typeof(DeleteModel<Key>);
                        XmlSerializer serializer = new XmlSerializer(t);
                        ids = serializer.Deserialize(reader) as DeleteModel<Key>;
                    }
                }
            }
            var res = (Task<ResponseApi>)GetMethodInfo("DeleteListAsync").Invoke(null, new object[] { ids.Ids, GetLanguage() });
            return res;
        }

        /// <summary>根据id删除实体类信息(多删除)</summary>
        /// <param name="ids">id</param>
        ///<return>返回删除结果(多删除),大于0 返回删除成功(多删除),小于等于0 返回删除失败(多删除) </return>
        [HttpPost]
        public virtual ResponseApi DeleteList([FromBody] DeleteModel<Key> ids)
        {
            var res = (ResponseApi)GetMethodInfo("DeleteList").Invoke(null, new object[] { ids.Ids, GetLanguage() });
            return res;
        }

        /// <summary>根据条件查询实体类数据集信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回实体类数据集信息 </return>
        [HttpPost]
        public virtual Task<ResponseApi<List<T>>> FindListAsync([FromBody] T obj)
        {
            var res = (Task<ResponseApi<List<T>>>)GetMethodInfo("FindListAsync").Invoke(null, new object[] { obj, GetLanguage() });
            return res;
        }

        /// <summary>根据条件查询实体类数据集信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回实体类数据集信息 </return>
        [HttpPost]
        public virtual ResponseApi<List<T>> FindList([FromBody] T obj)
        {
            var res = (ResponseApi<List<T>>)GetMethodInfo("FindList").Invoke(null, new object[] { obj, GetLanguage() });
            return res;
        }


        /// <summary>根据条件查询实体类数据集数量信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回实体类数据集数量信息</return>
        [HttpPost]
        public virtual Task<ResponseApi<int>> CountAsync([FromBody] T obj)
        {
            var res = (Task<ResponseApi<int>>)GetMethodInfo("CountAsync").Invoke(null, new object[] { obj, GetLanguage() });
            return res;
        }
        /// <summary>根据条件查询实体类数据集数量信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回实体类数据集数量信息</return>
        [HttpPost]
        public virtual ResponseApi<int> Count([FromBody] T obj)
        {
            var res = (ResponseApi<int>)GetMethodInfo("Count").Invoke(null, new object[] { obj, GetLanguage() });
            return res;
        }

        /// <summary>根据条件及分页查询实体类数据集信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        /// <param name="sortOrder"></param>
        ///<return>返回实体类数据集信息</return>
        [HttpPost]
        public virtual Task<ResponseApi<List<T>>> FindListByPageAsync([FromBody] T obj, int page, int size, string sortOrder)
        {
            var res = (Task<ResponseApi<List<T>>>)GetMethodInfo("FindListByPageAsync").Invoke(null, new object[] { obj, page, size, GetLanguage() });
            return res;
        }

        /// <summary>根据条件及分页查询实体类数据集信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        /// <param name="sortOrder"></param>
        ///<return>返回实体类数据集信息</return>
        [HttpPost]
        public virtual ResponseApi<List<T>> FindListByPage([FromBody] T obj, int page, int size, string sortOrder)
        {
            var res = (ResponseApi<List<T>>)GetMethodInfo("FindListByPage").Invoke(null, new object[] { obj, page, size, GetLanguage() });
            return res;
        }

        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        /// <param name="sortOrder"></param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
        [HttpPost]
        public virtual ResponseApi<ResultModel<T>> FindResultModelByPage([FromBody] T obj, int page, int size, string sortOrder)
        {
            var res = (ResponseApi<ResultModel<T>>)GetMethodInfo("FindResultModelByPage").Invoke(null, new object[] { obj, page, size, GetLanguage() });
            return res;
        }

        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        /// <param name="sortOrder"></param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
        [HttpPost]
        public virtual Task<ResponseApi<ResultModel<T>>> FindResultModelByPageAsync([FromBody] T obj, int page, int size, string sortOrder)
        {
            var res = (Task<ResponseApi<ResultModel<T>>>)GetMethodInfo("FindResultModelByPageAsync").Invoke(null, new object[] { obj, page, size, GetLanguage() });
            return res;
        }
        /// <summary>
        /// formbody 数据转换
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="str"></param>
        protected virtual void Ref<M>(ref M obj, string str) where M : class
        {
            obj = JsonHelper.ToObject<M>(str, JsonHelper.JsonSerializerSettings);
        }
    }

}
#endif