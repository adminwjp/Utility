#if NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
namespace Utility.ServiceBus
{
    using Autofac;
    using Microsoft.Azure.ServiceBus;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Utility.Logs;
    using Utility.Threads;

    /// <summary>
    /// ServiceBus
    /// </summary>
    public class DefaultServiceBus
    {
        private readonly IServiceBusPersisterConnection _serviceBusPersisterConnection;
        private readonly ILog<DefaultServiceBus> _logger;
        private readonly SubscriptionClient _subscriptionClient;
        readonly Func<string, string, Task<bool>> _func;

        /// <summary>
        /// ServiceBus
        /// </summary>
        /// <param name="serviceBusPersisterConnection"></param>
        /// <param name="logger"></param>
        /// <param name="subscriptionClientName"></param>
        /// <param name="func"></param>
        public DefaultServiceBus(IServiceBusPersisterConnection serviceBusPersisterConnection,
            ILog<DefaultServiceBus> logger,  string subscriptionClientName, Func<string, string, Task<bool>> func)
        {
            _serviceBusPersisterConnection = serviceBusPersisterConnection;
            _logger = logger;
            _func = func ?? throw new ArgumentNullException("func");
            _subscriptionClient = new SubscriptionClient(serviceBusPersisterConnection.ServiceBusConnectionStringBuilder,
                subscriptionClientName);
            RemoveDefaultRule();
            RegisterSubscriptionClientMessageHandler();
        }

        /// <summary>
        /// ServiceBus 发布
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="body"></param>
        /// <param name="label"></param>
        public virtual void Publish(string messageId, byte[] body, string label)
        {
            var message = new Message
            {
                MessageId = messageId,
                Body = body,
                Label = label,
            };
            var topicClient = _serviceBusPersisterConnection.CreateModel();
            topicClient.SendAsync(message).GetAwaiter().GetResult();
        }

        /// <summary>
        /// ServiceBus 订阅
        /// </summary>
        /// <param name="label"></param>
        public virtual void Subscribe(string label)
        {
            try
            {
                _subscriptionClient.AddRuleAsync(new RuleDescription
                {
                    Filter = new CorrelationFilter { Label = label },
                    Name = label
                }).GetAwaiter().GetResult();
            }
            catch (ServiceBusException)
            {
                _logger.LogFormat(LogLevel.Warn,"The messaging entity {0} already exists.", label);
            }
            _logger.LogFormat(LogLevel.Info,"Subscribing to event {0} ", label);
        }

        /// <summary>
        /// ServiceBus 取消 订阅
        /// </summary>
        /// <param name="label"></param>
        public virtual void Unsubscribe(string label)
        {
            try
            {
                _subscriptionClient.RemoveRuleAsync(label).GetAwaiter().GetResult();
            }
            catch (MessagingEntityNotFoundException)
            {
                _logger.LogFormat(LogLevel.Warn,"The messaging entity {0} Could not be found.", label);
            }

            _logger.LogFormat(LogLevel.Info,"Unsubscribing from event {0}", label);
        }

        /// <summary>
        /// 订阅 消息 处理
        /// </summary>
        private void RegisterSubscriptionClientMessageHandler()
        {
            _subscriptionClient.RegisterMessageHandler(
                async (message, token) =>
                {
                    var eventName = $"{message.Label}";
                    var messageData = Encoding.UTF8.GetString(message.Body);

                    // Complete the message so that it is not received again.
                    if (await _func(eventName, messageData))
                    {
                        await _subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
                    }
                },
                new MessageHandlerOptions(ExceptionReceivedHandler) { MaxConcurrentCalls = 10, AutoComplete = false });
        }

        /// <summary>
        /// 异常 处理 日志 输出
        /// </summary>
        /// <param name="exceptionReceivedEventArgs"></param>
        /// <returns></returns>
        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            var ex = exceptionReceivedEventArgs.Exception;
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            _logger.LogException(LogLevel.Warn,string.Format("ERROR handling message: {0} - Context: {1}", ex.Message, context),ex);
            //return Task.CompletedTask;
            return  TaskHelper.CompletedTask;

        }

        /// <summary>
        /// 移除规则
        /// </summary>
        private void RemoveDefaultRule()
        {
            try
            {
                _subscriptionClient.RemoveRuleAsync(RuleDescription.DefaultRuleName).GetAwaiter().GetResult();
            }
            catch (MessagingEntityNotFoundException)
            {
                _logger.LogFormat(LogLevel.Warn,"The messaging entity {0} Could not be found.", RuleDescription.DefaultRuleName);
            }
        }
    }
}
#endif