#if NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using Utility.Response;

namespace Utility.AspNetCore.Filter
{
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<HttpGlobalExceptionFilter> _logger;
#if NETCOREAPP3_0 || NETCOREAPP3_1 ||NET5_0
        private readonly IWebHostEnvironment _env;
        public HttpGlobalExceptionFilter(IWebHostEnvironment env, ILogger<HttpGlobalExceptionFilter> logger)
        {
            this._env = env;
            this._logger = logger;
        }
#else
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _env;
        public HttpGlobalExceptionFilter(Microsoft.AspNetCore.Hosting.IHostingEnvironment env, ILogger<HttpGlobalExceptionFilter> logger)
        {
            this._env = env;
            this._logger = logger;
        }
#endif
        public virtual void OnException(ExceptionContext context)
        {
            _logger.LogError(new EventId(context.Exception.HResult),
                context.Exception,
                context.Exception.Message);

            if (context.Exception.HResult==400)
            {
                context.Result = new BadRequestObjectResult(new ResponseApi()) { StatusCode = StatusCodes.Status400BadRequest };
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else
            {
                var result=new ResponseApi<Exception>();
                if (_env.IsDevelopment())
                {
                    result.Data = context.Exception;
                }

                context.Result = new ObjectResult(result) { StatusCode = StatusCodes.Status500InternalServerError };
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            context.ExceptionHandled = true;
        }
    }
}
#endif