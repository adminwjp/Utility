#if  NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NET481 || NET482
using System;
using System.ServiceModel;
using System.Messaging;

namespace Utility.MessageQueue
{
    /// <summary>
    /// 基于 微软 消息队列
    /// </summary>
    public class WcfMessageQueue
    {
        /// <summary>
        /// 创建消息队列服务端
        /// <para>messageQueue:net.msmq://localhost/Private/SampleQueue</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="implementedContractType"></param>
        /// <param name="urls">net.msmq://localhost/Private/SampleQueue</param>
        /// <returns></returns>
        public static ServiceHost GetMessageQueueServiceHostService<T>(string name, Type implementedContractType, Uri[] urls) where T : class
        {
            var serviceHost = new ServiceHost(typeof(T), urls);
            NetMsmqBinding binding = new NetMsmqBinding(NetMsmqSecurityMode.None);
            binding.ExactlyOnce = false;
            binding.Durable = true;
            serviceHost.AddServiceEndpoint(implementedContractType, binding, name);
            serviceHost.Open();
            Console.WriteLine("服务器已经准备好");
            return serviceHost;
        }
        /// <summary>
        /// 创建消息队列客户端
        /// <para>messageQueue:net.msmq://localhost/Private/SampleQueue</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">net.msmq://localhost/Private/SampleQueue</param>
        /// <returns></returns>
        public static T MessageQueueServiceHostClient<T>(string url) where T : class
        {
            NetMsmqBinding binding = new NetMsmqBinding(NetMsmqSecurityMode.None);
            binding.ExactlyOnce = false;
            binding.Durable = true;
            ChannelFactory<T> channel = new ChannelFactory<T>(binding, new EndpointAddress(url));
            T client = channel.CreateChannel();
            return client;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="label"></param>
        /// <param name="queueName"></param>
        public static void SendMessage(string msg, string label = "测试消息", string queueName = @".\Private$\SampleQueue")
        {
            System.Messaging.Message message = new System.Messaging.Message(msg);
            System.Messaging.MessageQueue mq = null;
            if (!System.Messaging.MessageQueue.Exists(queueName))
                mq = System.Messaging.MessageQueue.Create(queueName);
            else
                mq = new System.Messaging.MessageQueue(queueName);
            mq.Formatter = new XmlMessageFormatter(new[] { "System.String" });
            mq.Send(message, label);
            Console.WriteLine("消息发送成功");
        }

        /// <summary>
        /// 读取消息
        /// </summary>
        /// <param name="queueName"></param>
        public static void ReadMessage(string queueName = @".\Private$\SampleQueue")
        {
            System.Messaging.MessageQueue mq = new System.Messaging.MessageQueue(queueName);
            mq.Formatter = new XmlMessageFormatter(new[] { "System.String" });
            System.Messaging.Message msg = mq.Receive();
            Console.WriteLine(msg.Label);
            Console.WriteLine(msg.Body.ToString());

        }
    }
}
#endif