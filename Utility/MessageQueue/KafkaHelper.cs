#if  NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1

using System;
using System.Collections.Generic;
using System.Text;

namespace Utility.MessageQueue
{

#if KafkaNet
    /// <summary>
    /// 删除 kafka tool 上 删除 (还会存在 只不过连接不上,必须要在 zookeeper (持久化前提) 上删除 kafka 节点, kafka tool 但还会显示(只不过没用(删除 kafka 文件(kafka 停止) 就不会显示了)))
    /// kafka-core 实现  注意 zookeeper 数据 持久化删除 kafka  的节点 (也可以 删除 kafka 文件(kafka 停止)) 才能生效 .kafka 这里 删 没用 (又从 zookeeper 读取)
    /// 默认 7 天 自动 清理 
    /// 这个 包 有问题 消费者(不知道 怎么 处理的) 重复消费(再次消费)
    /// 原理 参考:https://blog.csdn.net/lrxcmwy2/article/details/82853300
    /// https://blog.csdn.net/m0_46195271/article/details/108110832
    /// </summary>
    public class KafkaNetHelper:IDisposable
    {
        /// <summary>
        /// System.ObjectDisposedException: 无法访问已释放的对象。 不释放 了 自己 开启线程 定时清理
        /// </summary>
        private IDictionary<string, Producer> producers = new ConcurrentDictionary<string, Producer>();
        /// <summary>
        /// BrokerRouter Kafka 服务器
        /// </summary>
        public BrokerRouter BrokerRouter { get; private set; }
        /// <summary>
        /// 初始化 地址连接
        /// </summary>
        /// <param name="url">http://127.0.0.1:9092</param>
        public KafkaNetHelper(string url= "http://127.0.0.1:9092")
        {
            var options = new KafkaOptions(new Uri(url));
            BrokerRouter = new BrokerRouter(options);
        }
        /// <summary>
        /// 初始化 地址连接
        /// </summary>
        /// <param name="urls"></param>
        public KafkaNetHelper(string[] urls)
        {
            Uri[] uris = new Uri[urls.Length];
            for (int i = 0; i < urls.Length; i++)
            {
                uris[i] = new Uri(urls[i]);
            }
            var options = new KafkaOptions(uris);
            BrokerRouter = new BrokerRouter(options);
        }
        /// <summary>
        /// 生产者 生产数据 (推送数据)
        /// </summary>
        /// <param name="topic">一个Topic包含一个或多个Partition，建Topic的时候可以手动指定Partition个数，个数与服务器个数相当</param>
        /// <param name="msg">每条消息属于且仅属于一个Topic</param>
        /// <param name="key"></param>
        public virtual void Push(string topic,string msg,string key=null)
        {
            List<Message> msgArr = new List<Message>();
            msgArr.Add(new Message(msg, key));
            Push(topic,msgArr.ToArray());
         

        }
        /// <summary>
        /// 生产者 生产数据 (推送数据)
        /// </summary>
        /// <param name="topic">一个Topic包含一个或多个Partition，建Topic的时候可以手动指定Partition个数，个数与服务器个数相当</param>
        /// <param name="messages">每条消息属于且仅属于一个Topic</param>
        public virtual void Push(string topic, Message[] messages)
        {
            //System.ObjectDisposedException: 无法访问已释放的对象。

            //Producer producer = null;
            //if (producers.ContainsKey(topic))
            //{
            //    producer = producers[topic];
            //}
            //else
            //{
            //    producer = new Producer(BrokerRouter);
            //    producers.Add(topic, producer);
            //}
            //生产者 在 使用 不能 释放 
            //using (var producer = new Producer(BrokerRouter))
            //kafka tool 时删除 时 topic 需要 手动创建 topic(创建不了) 什么 玩意
            //Awaiting message from: http://127.0.0.1:9092/
            //No connection to: http://127.0.0.1:9092/.  Attempting to connect..
            //Pull Failed connection to:http://127.0.0.1:9092/.  Will retry in:25600
            var producer = new Producer(BrokerRouter);
            {
                producer.SendMessageAsync(topic, messages); //.Wait(500);//bool
            }
        }

        /// <summary>
        /// 消费者 处理 数据 
        /// 怎么解决已处理过的数据了, 数据一直存在(正常情况下不会发生 ,自动处理)
        /// 有 数据 手动确认偏移量
        /// </summary>
        /// <param name="topic">一个Topic包含一个或多个Partition，建Topic的时候可以手动指定Partition个数，个数与服务器个数相当</param>
        /// <param name="cancellationToken">内部 使用的 BlockingCollection  一直阻塞 (int.MaxValue BlockingCollection 才终止,要么手动 会出现异常)</param>
        public  virtual void Pull(string topic, Action<Message> func, CancellationTokenSource cancellationToken)
        {
           //死循环 需要 手动 控制 不然 不会 自动 提交 偏移量
           // using (var consumer = new Consumer(new ConsumerOptions(topic, BrokerRouter)))
            var consumer = new Consumer(new ConsumerOptions(topic, BrokerRouter));
            {
                cancellationToken = cancellationToken ?? new CancellationTokenSource();
                //手动 提交 改变 偏移量 
                //consumer.SetOffsetPosition(new OffsetPosition() { });//内部 改变 了 则 更新 
                //这个拉取 怎么像 死循环 
                // Failed connection to:http://127.0.0.1:9092/.  Will retry in:25600

                //需要手动控制 cancellationToken(死循环) ？
                //Received message of size: 39 From: http://autobvt-3fi7bms:9092/
                //Awaiting message from: http://autobvt-3fi7bms:9092/

                var msgs = consumer.Consume(cancellationToken.Token);//这是一个死线程(消费者控制数据是否消费完成) 处理 数据 扔进 BlockingCollection
                //到底 cancellationToken 谁 控制的  好像 需要 手动(有时 报 BlockingCollection 取消异常(没有数据) )
                //BlockingCollection(内部 自动 处理 是否有对象,用完 则 释放 CancellationTokenSource(新的) )  一直阻塞(int.MaxValue BlockingCollection 才终止,要么手动)
                try
                {
                    foreach (var msg in msgs)
                    {
                        func?.Invoke(msg);
                       // consumer.SetOffsetPosition(new OffsetPosition() { PartitionId= msg .Meta.PartitionId,Offset= msg.Meta.Offset });
                        //cancellationToken.Cancel();//异常
                        // break;
                    }
                }
                catch (Exception e)
                {
                }
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public virtual void Dispose()
        {
            BrokerRouter.Dispose();
        }
    }
#endif

}
#endif