#if NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0|| NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Utility.EventBus.Extensions;
using Utility.Logs;

namespace Utility.Application.Behaviors
{
    /// <summary>
    ///MediatR 订阅 日志 消息中间件 
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILog<LoggingBehavior<TRequest, TResponse>> _logger;

        /// <summary>
        ///MediatR 订阅 日志 消息中间件 
        /// </summary>
        public LoggingBehavior(ILog<LoggingBehavior<TRequest, TResponse>> logger) => _logger = logger;

        /// <summary>
        /// 日志 处理
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public virtual async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _logger.Log(LogLevel.Info, $"----- Handling command {request.GetGenericTypeName()} ({request})");
            var response = await next();
            _logger.Log(LogLevel.Info, $"----- Command {request.GetGenericTypeName()} handled - response: {response}");

            return response;
        }
    }
}
#endif
