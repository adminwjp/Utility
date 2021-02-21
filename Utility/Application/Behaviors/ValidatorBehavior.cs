#if NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using FluentValidation;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Utility.EventBus.Extensions;
using Utility.Logs;

namespace Utility.Application.Behaviors
{
    /// <summary>
    ///MediatR 订阅 验证  消息中间件 
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public class ValidatorBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILog<ValidatorBehavior<TRequest, TResponse>> logger;
        private readonly IValidator<TRequest>[] validators;

        /// <summary>
        /// MediatR 订阅 验证  消息中间件 
        /// </summary>
        /// <param name="validators"></param>
        /// <param name="logger"></param>
        public ValidatorBehavior(IValidator<TRequest>[] validators, ILog<ValidatorBehavior<TRequest, TResponse>> logger)
        {
            this.validators = validators;
            this.logger = logger;
        }

        /// <summary>
        /// 验证处理
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public virtual async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var typeName = request.GetGenericTypeName();

            logger.Log(LogLevel.Info, $"----- Validating command {typeName}");

            var failures = validators
                .Select(v => v.Validate(request))
                .SelectMany(result => result.Errors)
                .Where(error => error != null)
                .ToList();

            if (failures.Any())
            {
                logger.Log(LogLevel.Warn, $"Validation errors - {typeName} - Command: {request} - Errors: {failures}");

                throw new Exception(
                    $"Command Validation Errors for type {typeof(TRequest).Name}", new ValidationException("Validation exception", failures));
            }

            return await next();
        }
    }
}
#endif