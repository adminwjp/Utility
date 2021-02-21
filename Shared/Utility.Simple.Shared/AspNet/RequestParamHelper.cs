#if NET35 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48
using Newtonsoft.Json;
using System;
using System.IO;
using System.Web;
using System.Xml.Serialization;
using Utility.BLL;
using Utility.Json;
using Utility.Model;
using Utility.Net.Http;
using Utility.Page;

namespace Utility.AspNet
{
    internal  class RequestParamHelper
    {
        public static JsonSerializerSettings Settings = new JsonSerializerSettings()
        {
            //忽略循环引用
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            //使用 ab_c AbC  实际 AbC  ab_c 
            //ContractResolver = JsonContractResolver.ObjectResolverJson,
            ContractResolver = JsonContractResolver.JsonResolverObject,
            //设置时间格式
            DateFormatString = "yyyy-MM-dd HH:mm:ss"
        };

        public static Tuple<T, Key, DeleteModel<Key>, int, int> GetRequestParam<ResponseApiBLL, T, Key, BLL>(HttpContext context,string m=null) 
            where ResponseApiBLL : Utility.BLL.ResponseApiBLL<BLL,T, Key>
            where BLL : IBLL<T, Key>, new()
            where T : class, IModel<Key>
        {
            m = m?? context.Request.QueryString["m"]?.ToLower();
            m = m?.ToLower();
            T obj = null;
            Key id = default(Key);
            int page = 1;
            int size = 10;
            DeleteModel<Key> deleteModel = null;
            bool isPost = context.Request.HttpMethod.ToLower() == "post";
            if (isPost)
            {
                if (context.Request.ContentType == ContentTypeConstant.APPLICATION_X_WWW_FORM_URLENCODED
                    || context.Request.ContentType == ContentTypeConstant.APPLICATION_Multipart)
                {
                    if (context.Request.Form.Count > 0)
                    {
                        //绑定数据源有点麻烦 算了 直接用反射算了 绑定
                    }
                }
                else if (context.Request.ContentType == ContentTypeConstant.APPLICATION_JSON)
                {
                    using (StreamReader reader = new StreamReader(context.Request.InputStream))
                    {
                        string json = reader.ReadToEnd();
                        if ("deletelistsync".Equals(m) || "deletelist".Equals(m))
                        {
                            deleteModel = JsonHelper.ToObject<DeleteModel<Key>>(json, JsonHelper.JsonSerializerSettings);
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
            else if (context.Request.HttpMethod.ToLower() == "get")
            {
                if (context.Request["id"] != null)
                {
                    id = (Key)Convert.ChangeType(context.Request["id"], typeof(Key));
                }
            }
            else
            {
                return null;
            }
            if ("deletesync".Equals(m) || "delete".Equals(m))
            {
                if (isPost)
                {
                    return null;
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
            return new Tuple<T, Key, DeleteModel<Key>, int, int>(obj,id,deleteModel,page,size);
        }
    }
}
#endif