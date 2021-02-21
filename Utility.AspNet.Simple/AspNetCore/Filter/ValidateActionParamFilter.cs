#if NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using Utility.AspNetCore.Extensions;
using Utility.Response;
using Utility;
using Utility.Enums;
using System.Xml.Serialization;
using Utility.Utils;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Utility.Json;

namespace Utility.AspNetCore.Filter
{
    public class ValidateActionParamFilter : IActionFilter
    {
        public virtual void OnActionExecuted(ActionExecutedContext context)
        {
          
        }
       
        protected virtual  void ActionExecut(ActionExecutingContext context)
        {
            var Request = context.HttpContext.Request;
           
            if (Request.Method.ToLower() == "post")
            {
                object obj = null;
                Type type = null;
                ParameterDescriptor parameterDescriptor = null;
                for (int i = 0; i < context.ActionDescriptor.Parameters.Count; i++)
                {
                    parameterDescriptor = context.ActionDescriptor.Parameters[i];
                    if (!TypeUtils.IsCommonType(parameterDescriptor.ParameterType))
                    {
                        type = parameterDescriptor.ParameterType;
                        obj = context.ActionArguments[parameterDescriptor.Name];
                        break;
                    }
                }
                if (type != null)
                {
                    if (Request.ContentType != null)
                    {
                        if (Request.ContentType.Contains("application/json"))
                        {
                            using (System.IO.StreamReader reader = new System.IO.StreamReader(Request.Body))
                            {
                                obj = JsonHelper.ToObject(reader.ReadToEnd(),type, JsonHelper.JsonSerializerSettings);
                                //Ref(ref obj, reader.ReadToEnd());
                            }
                            context.ActionArguments[parameterDescriptor.Name] = obj;
                        }
                        else if (Request.ContentType.Contains("text/xml"))
                        {
                            using (System.IO.StreamReader reader = new System.IO.StreamReader(Request.Body))
                            {
                                XmlSerializer serializer = new XmlSerializer(type);
                                obj = serializer.Deserialize(reader);
                            }
                            context.ActionArguments[parameterDescriptor.Name] = obj;
                        }
                    }
                }
            }
        }

        public virtual void OnActionExecuting(ActionExecutingContext context)
        {
            ActionExecut(context);//绑定数据
            //asp.net core api  需要手动调用
            if (!context.ModelState.IsValid)
            {
                ResponseApi responseApi = ResponseApi.Create(Language.Chinese, Code.ParamError);
                responseApi.Data = context.ModelState.Errors();
                context.Result = new JsonResult(responseApi) { StatusCode = 400 };
            }
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