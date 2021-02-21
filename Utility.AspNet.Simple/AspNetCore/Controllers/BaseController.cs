#if NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0
using System;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Mvc;
using Utility.BLL;
using Utility.Enums;
using Utility.Json;
using Utility.Model;
using Utility.Response;

namespace Utility.AspNetCore.Controllers
{
    /// <summary>
    /// [FromForm,FromBody] 只支持 FromForm (支持 普通 类型,form body from-data xml都支持)
	///Microsoft.AspNetCore.Routing.Matching.AmbiguousMatchException: The request matched multiple endpoints. Matches:
	///async	 not access  访问不了 咋回事  asp.net core 5.0 
	///InsertAsync not access, Insert access
	///async FromBody 支持  FromForm 一直请求中
    /// </summary>
    /// <typeparam name="ResponseApiBLL"></typeparam>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="Key"></typeparam>
    /// <typeparam name="BLL"></typeparam>
    [Route("api/[controller]")]
#if !NETCOREAPP2_0
    [ApiController]
#endif
    [ProducesResponseType(typeof(IResponseApi),200)]

    public class BaseController<ResponseApiBLL,BLL, T, Key> : ControllerBase where ResponseApiBLL : Utility.BLL.ResponseApiBLL<BLL,T, Key>
         where BLL : IBLL<T, Key>, new()
         where T : class,IModel<Key>
    {
        protected ResponseApiBLL ApiBLL { get; set; }
        protected virtual Language GetLanguage()
        {
            return Language.Chinese;
        }
        /// <summary>添加实体类信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回添加结果,大于0 返回添加成功,小于等于0 返回添加失败 </return>
        [HttpPost("Add")]
        public virtual async Task<ResponseApi> Add([FromForm,FromBody]T obj)
        {
            //共享作用域不存在 数据没有
            if (Request.ContentType != null)
            {
                if (Request.ContentType.Contains("application/json"))
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(Request.Body))
                    {
                        Ref(ref obj, reader.ReadToEnd());
                    }
                }
                else if (Request.ContentType.Contains("text/xml"))
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(Request.Body))
                    {
                        Type t = typeof(T);
                        XmlSerializer serializer = new XmlSerializer(t);
                        obj = serializer.Deserialize(reader) as T;
                    }
                }
            }
            var res = await ApiBLL.InsertAsync(obj, GetLanguage());
            return res;
        }

        /// <summary>添加实体类信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回添加结果,大于0 返回添加成功,小于等于0 返回添加失败 </return>
        [HttpPost("Insert")]
        public virtual ResponseApi Insert([FromForm, FromBody] T obj)
        {
            //共享作用域不存在 数据没有
            if (Request.ContentType != null)
            {
                if (Request.ContentType.Contains("application/json"))
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(Request.Body))
                    {
                        Ref(ref obj, reader.ReadToEnd());
                    }
                }
                else if (Request.ContentType.Contains("text/xml"))
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(Request.Body))
                    {
                        Type t = typeof(T);
                        XmlSerializer serializer = new XmlSerializer(t);
                        obj = serializer.Deserialize(reader) as T;
                    }
                }
            }
            var res = ApiBLL.Insert(obj, GetLanguage());
            return res;
        }


        /// <summary>修改实体类信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回修改结果,大于0 返回修改成功,小于等于0 返回修改失败 </return>
        [HttpPost("Modify")]
        public virtual async  System.Threading.Tasks.Task<ResponseApi> Modify([FromForm, FromBody] T obj)
        {  
            //共享作用域不存在 数据没有
            if (Request.ContentType != null)
            {
                if (Request.ContentType.Contains("application/json"))
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(Request.Body))
                    {
                        Ref(ref obj, reader.ReadToEnd());
                    }
                }
                else if (Request.ContentType.Contains("text/xml"))
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(Request.Body))
                    {
                        Type t = typeof(T);
                        XmlSerializer serializer = new XmlSerializer(t);
                        obj = serializer.Deserialize(reader) as T;
                    }
                }
            }
            var res =await ApiBLL.UpdateAsync(obj,GetLanguage());
            return res;
        }

        /// <summary>修改实体类信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回修改结果,大于0 返回修改成功,小于等于0 返回修改失败 </return>
        [HttpPost("Update")]
        public virtual ResponseApi Update([FromForm, FromBody] T obj)
        {
            //共享作用域不存在 数据没有
            if (Request.ContentType != null)
            {
                if (Request.ContentType.Contains("application/json"))
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(Request.Body))
                    {
                        Ref(ref obj, reader.ReadToEnd());
                    }
                }
                else if (Request.ContentType.Contains("text/xml"))
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(Request.Body))
                    {
                        Type t = typeof(T);
                        XmlSerializer serializer = new XmlSerializer(t);
                        obj = serializer.Deserialize(reader) as T;
                    }
                }
            }
            var res = ApiBLL.Update(obj,GetLanguage());
            return res;
        }

        /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        [HttpGet("Remove")]
        public virtual async  System.Threading.Tasks.Task<ResponseApi> Remove(Key id)
        {
            var res =await  ApiBLL.DeleteAsync(id, GetLanguage());
            return res;
        }

        /// <summary>根据id删除实体类信息</summary>
        /// <param name="id">id</param>
        ///<return>返回删除结果,大于0 返回删除成功,小于等于0 返回删除失败 </return>
        [HttpGet("Delete")]
        public virtual ResponseApi Delete(Key id)
        {
            var res = ApiBLL.Delete(id, GetLanguage());
            return res;
        }

        /// <summary>根据id删除实体类信息(多删除)</summary>
        /// <param name="ids">id</param>
        ///<return>返回删除结果(多删除),大于0 返回删除成功(多删除),小于等于0 返回删除失败(多删除) </return>
        [HttpPost("RemoveList")]
        public  virtual async System.Threading.Tasks.Task<ResponseApi> RemoveList([FromForm, FromBody] DeleteModel<Key> ids)
        {
            //共享作用域不存在 数据没有
            if (Request.ContentType != null)
            {
                if (Request.ContentType.Contains("application/json"))
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(Request.Body))
                    {
                        Ref(ref ids, reader.ReadToEnd());
                    }
                }
                else if (Request.ContentType.Contains("text/xml"))
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(Request.Body))
                    {
                        Type t = typeof(DeleteModel<Key>);
                        XmlSerializer serializer = new XmlSerializer(t);
                        ids = serializer.Deserialize(reader) as DeleteModel<Key>;
                    }
                }
            }
            var res = await  ApiBLL.DeleteListAsync(ids.Ids, GetLanguage());
            return res;
        }

        /// <summary>根据id删除实体类信息(多删除)</summary>
        /// <param name="ids">id</param>
        ///<return>返回删除结果(多删除),大于0 返回删除成功(多删除),小于等于0 返回删除失败(多删除) </return>
        [HttpPost("DeleteList")]
        public virtual  ResponseApi DeleteList([FromForm, FromBody] DeleteModel<Key> ids)
        {
            //共享作用域不存在 数据没有
            if (Request.ContentType != null)
            {
                if (Request.ContentType.Contains("application/json"))
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(Request.Body))
                    {
                        Ref(ref ids, reader.ReadToEnd());
                    }
                }
                else if (Request.ContentType.Contains("text/xml"))
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(Request.Body))
                    {
                        Type t = typeof(DeleteModel<Key>);
                        XmlSerializer serializer = new XmlSerializer(t);
                        ids = serializer.Deserialize(reader) as DeleteModel<Key>;
                    }
                }
            }
            var res = ApiBLL.DeleteList(ids.Ids, GetLanguage());
            return res;
        }

        /// <summary>根据条件查询实体类数据集信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回实体类数据集信息 </return>
        [HttpPost("GetList")]
        public  virtual async System.Threading.Tasks.Task<ResponseApi<System.Collections.Generic.List<T>>> GetList([FromForm, FromBody] T obj)
        {
            //共享作用域不存在 数据没有
            if (Request.ContentType != null)
            {
                if (Request.ContentType.Contains("application/json"))
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(Request.Body))
                    {
                        Ref(ref obj, reader.ReadToEnd());
                    }
                }
                else if (Request.ContentType.Contains("text/xml"))
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(Request.Body))
                    {
                        Type t = typeof(T);
                        XmlSerializer serializer = new XmlSerializer(t);
                        obj = serializer.Deserialize(reader) as T;
                    }
                }
            }
            var res = await ApiBLL.FindListAsync(obj, GetLanguage());
            return res;
        }

        /// <summary>根据条件查询实体类数据集信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回实体类数据集信息 </return>
        [HttpPost("FindList")]
        public virtual  ResponseApi<System.Collections.Generic.List<T>> FindList([FromForm, FromBody] T obj)
        {
            //共享作用域不存在 数据没有
            if (Request.ContentType != null)
            {
                if (Request.ContentType.Contains("application/json"))
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(Request.Body))
                    {
                        Ref(ref obj, reader.ReadToEnd());
                    }
                }
                else if (Request.ContentType.Contains("text/xml"))
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(Request.Body))
                    {
                        Type t = typeof(T);
                        XmlSerializer serializer = new XmlSerializer(t);
                        obj = serializer.Deserialize(reader) as T;
                    }
                }
            }
            var res = ApiBLL.FindList(obj, GetLanguage());
            return res;
        }


        /// <summary>根据条件查询实体类数据集数量信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回实体类数据集数量信息</return>
        [HttpPost("GetCount")]
        public  virtual async System.Threading.Tasks.Task<ResponseApi<int>> GetCount([FromForm, FromBody] T obj)
        {
            //共享作用域不存在 数据没有
            if (Request.ContentType != null)
            {
                if (Request.ContentType.Contains("application/json"))
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(Request.Body))
                    {
                        Ref(ref obj, reader.ReadToEnd());
                    }
                }
                else if (Request.ContentType.Contains("text/xml"))
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(Request.Body))
                    {
                        Type t = typeof(T);
                        XmlSerializer serializer = new XmlSerializer(t);
                        obj = serializer.Deserialize(reader) as T;
                    }
                }
            }
            var res =await  ApiBLL.CountAsync(obj, GetLanguage());
            return res;
        }

        /// <summary>根据条件查询实体类数据集数量信息</summary>
        /// <param name="obj">实体类</param>
        ///<return>返回实体类数据集数量信息</return>
        [HttpPost("Count")]
        public virtual ResponseApi<int> Count([FromForm, FromBody] T obj)
        {
            //共享作用域不存在 数据没有
            if (Request.ContentType != null)
            {
                if (Request.ContentType.Contains("application/json"))
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(Request.Body))
                    {
                        Ref(ref obj, reader.ReadToEnd());
                    }
                }
                else if (Request.ContentType.Contains("text/xml"))
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(Request.Body))
                    {
                        Type t = typeof(T);
                        XmlSerializer serializer = new XmlSerializer(t);
                        obj = serializer.Deserialize(reader) as T;
                    }
                }
            }
            var res = ApiBLL.Count(obj, GetLanguage());
            return res;
        }

        /// <summary>根据条件及分页查询实体类数据集信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<return>返回实体类数据集信息</return>
        [HttpPost("GetListByPage")]
        public  virtual async System.Threading.Tasks.Task<ResponseApi<System.Collections.Generic.List<T>>> GetListByPage([FromForm, FromBody] T obj, int page, int size)
        {
            //共享作用域不存在 数据没有
            if (Request.ContentType != null)
            {
                if (Request.ContentType.Contains("application/json"))
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(Request.Body))
                    {
                        Ref(ref obj, reader.ReadToEnd());
                    }
                }
                else if (Request.ContentType.Contains("text/xml"))
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(Request.Body))
                    {
                        Type t = typeof(T);
                        XmlSerializer serializer = new XmlSerializer(t);
                        obj = serializer.Deserialize(reader) as T;
                    }
                }
            }
            var res =  await ApiBLL.FindListByPageAsync(obj, page, size, GetLanguage());
            return res;
        }

        /// <summary>根据条件及分页查询实体类数据集信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<return>返回实体类数据集信息</return>
        [HttpPost("FindListByPage")]
        public virtual ResponseApi<System.Collections.Generic.List<T>> FindListByPage([FromForm, FromBody] T obj, int page, int size)
        {
            //共享作用域不存在 数据没有
            if (Request.ContentType != null)
            {
                if (Request.ContentType.Contains("application/json"))
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(Request.Body))
                    {
                        Ref(ref obj, reader.ReadToEnd());
                    }
                }
                else if (Request.ContentType.Contains("text/xml"))
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(Request.Body))
                    {
                        Type t = typeof(T);
                        XmlSerializer serializer = new XmlSerializer(t);
                        obj = serializer.Deserialize(reader) as T;
                    }
                }
            }
            var res = ApiBLL.FindListByPage(obj, page, size, GetLanguage());
            return res;
        }

        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
        [HttpPost("GetResultModelByPage")]
        public  virtual async System.Threading.Tasks.Task<ResponseApi<Utility.Model.ResultModel<T>>> GetResultModelByPage([FromForm, FromBody] T obj, int page, int size)
        {
            //共享作用域不存在 数据没有
            if (Request.ContentType != null)
            {
                if (Request.ContentType.Contains("application/json"))
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(Request.Body))
                    {
                        Ref(ref obj, reader.ReadToEnd());
                    }
                }
                else if (Request.ContentType.Contains("text/xml"))
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(Request.Body))
                    {
                        Type t = typeof(T);
                        XmlSerializer serializer = new XmlSerializer(t);
                        obj = serializer.Deserialize(reader) as T;
                    }
                }
            }
            var res =  await ApiBLL.FindResultModelByPageAsync(obj, page, size, GetLanguage());
            return res;
        }

        /// <summary>根据条件及分页查询实体类数据集信息和实体类数据集数量信息</summary>
        /// <param name="obj">实体类</param>
        /// <param name="page">页数</param>
        /// <param name="size">每页记录</param>
        ///<return>返回实体类数据集信息和实体类数据集数量信息</return>
        [HttpPost("FindResultModelByPage")]
        public virtual  ResponseApi<Utility.Model.ResultModel<T>> FindResultModelByPage([FromForm, FromBody] T obj, int page, int size)
        {
            //共享作用域不存在 数据没有
            if (Request.ContentType != null)
            {
                if (Request.ContentType.Contains("application/json"))
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(Request.Body))
                    {
                        Ref(ref obj, reader.ReadToEnd());
                    }
                }
                else if (Request.ContentType.Contains("text/xml"))
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(Request.Body))
                    {
                        Type t = typeof(T);
                        XmlSerializer serializer = new XmlSerializer(t);
                        obj = serializer.Deserialize(reader) as T;
                    }
                }
            }
            var res =  ApiBLL.FindResultModelByPage(obj, page, size, GetLanguage());
            return res;
        }
        /// <summary>
        /// formbody 数据转换
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="str"></param>
        protected virtual void Ref<M>(ref M obj, string str) where M:class
        {
            obj = JsonHelper.ToObject<M>(str, JsonHelper.JsonSerializerSettings);
        }
    }
}
#endif