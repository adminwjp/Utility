#if NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Utility.AspNetCore.Filters
{
    public class AuthFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string token = context.HttpContext.Request.Headers["token"];
            if (!string.IsNullOrEmpty(token))
            {
                if (token.Equals("test"))
                {

                }
                else
                {
                    //
                }
            }
            else
            {
                context.Result = new UnauthorizedResult() { };
            }
            base.OnActionExecuting(context);
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }
    }
    public class AuthFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        public virtual void OnActionExecuting(ActionExecutingContext context)
        {
            string token=context.HttpContext.Request.Headers["token"];
            if (!string.IsNullOrEmpty(token) )
            {
                if (token.Equals("test"))
                {

                }
                else
                {
                    //
                }
            }
            else
            {
                context.Result = new UnauthorizedResult() { };
            }
        }
    }
}
#endif