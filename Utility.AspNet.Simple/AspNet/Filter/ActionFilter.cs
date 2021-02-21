#if NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Utility.AspNet.Filter
{
    /// <summary>
    /// 绑定 参数 模型
    /// </summary>
    public class ActionFilter : IActionFilter
    {
        /// <summary>
        /// 是否 允许 
        /// </summary>
        public bool AllowMultiple => false;

        /// <summary>
        /// 参数 绑定 时 更新 post 模型 绑定 
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="continuation"></param>
        /// <returns></returns>
        public Task<HttpResponseMessage> ExecuteActionFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            
            return continuation?.Invoke();
        }
    }
}
#endif