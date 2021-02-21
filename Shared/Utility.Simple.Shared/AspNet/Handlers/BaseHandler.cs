#if NET35 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48
using Newtonsoft.Json;
using System;
using System.IO;
using System.Web;
using System.Xml.Serialization;
using Utility.BLL;
using Utility.Enums;
using Utility.Json;
using Utility.Model;
using Utility.Net.Http;
using Utility.Page;
using Utility.Response;

namespace Utility.AspNet.Handlers
{
    /// <summary>
    /// BaseHandler 的摘要说明 简单 操作 示例
    /// </summary>

    public abstract class BaseHandler<ResponseApiBLL, T, Key, BLL> : IHttpHandler where ResponseApiBLL :class, IResponseApiBLL<T, Key>,new()
      where T : class, IModel<Key>
      where BLL : IBLL<T, Key>, new()
    {
        /// <summary>
        /// 
        /// </summary>
        protected ResponseApiBLL ApiBLL { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public virtual void ProcessRequest(HttpContext context)
        {
            string m = context.Request.QueryString["m"]?.ToLower();
            IResponseApi responseApi = null;
            T obj = null;
            Key id=default;
            int page = 1;
            int size = 10;
            DeleteModel<Key> deleteModel=null;
            bool isPost = context.Request.HttpMethod.ToLower() == "post";
            if (isPost)
            {
                if(context.Request.ContentType== ContentTypeConstant.APPLICATION_X_WWW_FORM_URLENCODED
                    || context.Request.ContentType == ContentTypeConstant.APPLICATION_Multipart)
                {
                    if (context.Request.Form.Count > 0)
                    {
                       //绑定数据源有点麻烦 算了 直接用反射算了 绑定
                    }
                }
                else if (context.Request.ContentType == ContentTypeConstant.APPLICATION_JSON)
                {
                    using (StreamReader reader=new StreamReader(context.Request.InputStream))
                    {
                        string json = reader.ReadToEnd();
                        if("deletelistsync".Equals(m)|| "deletelist".Equals(m))
                        {
                            deleteModel = JsonHelper.ToObject<DeleteModel<Key>>(json, RequestParamHelper.Settings);
                        }
                        else
                        {
                            obj = JsonHelper.ToObject<T>(json, JsonHelper.JsonSerializerSettings);
                        }
                    }
                }
                else if (context.Request.ContentType == "text/xml")
                {
                    using (StreamReader reader = new StreamReader(context.Request.InputStream))
                    {
                        if ("deletelistsync".Equals(m) || "deletelist".Equals(m))
                        {
                            Type t = typeof(DeleteModel<Key>);
                            XmlSerializer serializer = new XmlSerializer(t);
                            deleteModel = serializer.Deserialize(reader) as DeleteModel<Key>;
                        }
                        else
                        {
                            Type t = typeof(T);
                            XmlSerializer serializer = new XmlSerializer(t);
                            obj = serializer.Deserialize(reader) as T;
                        }
                    }
                }
            }
            else if(context.Request.HttpMethod.ToLower() == "get")
            {
                if(context.Request["id"]!=null)
                {
                    id = (Key)Convert.ChangeType(context.Request["id"], typeof(Key));
                }
            }
            else
            {
                responseApi = ResponseApi.Create(GetLanguage(context), Code.NotSupport);
                Output(context, responseApi);
                return;
            }
            if ("deletesync".Equals(m) || "delete".Equals(m))
            {
                if (isPost)
                {
                    NotSupportOutput(context);
                    return;
                }
            }
            if (context.Request["page"] != null)
            {
                page = Convert.ToInt32(context.Request["page"]);
            }
            if (context.Request["size"] != null)
            {
                size = int.Parse(context.Request["size"]);
            }
            PageHelper.Set(ref page, ref size);
            switch (m)
            {
                case "insert":
                    {
                        responseApi = ApiBLL.Insert(obj, GetLanguage(context));
                    }
                    break;
                case "update":
                    {
                        responseApi = ApiBLL.Update(obj, GetLanguage(context));
                    }
                    break;
                case "delete":
                    {
                        responseApi = ApiBLL.Delete(id, GetLanguage(context));
                    }
                    break;
                case "deletelist":
                    {
                        responseApi = ApiBLL.DeleteList(deleteModel.Ids, GetLanguage(context));
                    }
                    break;
               
                case "findlist":
                    {
                        responseApi = ApiBLL.FindList(obj, GetLanguage(context));
                    }
                    break;
                case "findlistbypage":
                    {
                        responseApi = ApiBLL.FindListByPage(obj, page, size, GetLanguage(context));
                    }
                    break;
                case "findresultmodelbypage":
                    {
                        responseApi = ApiBLL.FindResultModelByPage(obj, page, size, GetLanguage(context));
                    }
                    break;

  #if !NET35
                case "insertasync":
                    {
                        responseApi = ApiBLL.InsertAsync(obj, GetLanguage(context)).Result;
                    }
                    break;
                case "updateasync":
                    {
                        responseApi = ApiBLL.UpdateAsync(obj, GetLanguage(context)).Result;
                    }
                    break;
                case "deletesync":
                    {
                        responseApi = ApiBLL.DeleteAsync(id, GetLanguage(context)).Result;
                    }
                    break;
                case "deletelistsync":
                    {
                        responseApi = ApiBLL.DeleteListAsync(deleteModel.Ids, GetLanguage(context)).Result;
                    }
                    break;
                case "findlistsync":
                    {
                        responseApi = ApiBLL.FindListAsync(obj, GetLanguage(context)).Result;
                    }
                    break;
                case "findlistbypagesync":
                    {
                        responseApi = ApiBLL.FindListByPageAsync(obj, page, size, GetLanguage(context)).Result;
                    }
                    break;
                case "findresultmodelbypagesync":
                    {
                        responseApi = ApiBLL.FindResultModelByPageAsync(obj, page, size, GetLanguage(context)).Result;
                    }
                    break;
#endif
                default:
                    responseApi = ResponseApi.Create(GetLanguage(context), Code.NotSupport);
                    break;
            }
            Output(context, responseApi);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="responseApi"></param>
        protected virtual void Output(HttpContext context,IResponseApi responseApi)
        {
            //context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            context.Response.ContentType = ContentTypeConstant.APPLICATION_JSON;
            context.Response.Write(JsonHelper.ToJson(responseApi, new JsonSerializerSettings()
            {
                //忽略循环引用
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                //使用 ab_c AbC  实际 AbC  ab_c 
                 ContractResolver = JsonContractResolver.ObjectResolverJson,
                //ContractResolver = JsonContractResolver.JsonResolverObject,
                //设置时间格式
                DateFormatString = "yyyy-MM-dd HH:mm:ss"
            }));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        protected virtual void NotSupportOutput(HttpContext context)
        {
            var responseApi = ResponseApi.Create(GetLanguage(context), Code.NotSupport);
            Output(context, responseApi);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected virtual Language GetLanguage(HttpContext context)
        {
            return Language.Chinese;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
#endif