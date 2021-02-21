#if !(NET10 || NET11 || NET20 || NET30 || NET35 || NET40 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0  || NETSTANDARD1_1 || NETSTANDARD1_2 )
using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Utility.MessageQueue
{
    /// <summary>
    /// Confluent.Kafka 实现
    /// 删除 kafka tool 上 删除 (还会存在 只不过连接不上,必须要在 zookeeper (持久化前提) 上删除 kafka 节点, kafka tool 但还会显示(只不过没用(删除 kafka 文件(kafka 停止) 就不会显示了)))
    /// 默认 7 天 自动 清理 
    /// 原理 参考:https://blog.csdn.net/lrxcmwy2/article/details/82853300
    /// https://blog.csdn.net/m0_46195271/article/details/108110832
    /// </summary>
    public class ConfluentKafkaHelper
    {
        private IEnumerable<KeyValuePair<string, string>> config;
        /// <summary>
        /// 初始化配置
        /// </summary>
        /// <param name="config"></param>
        public ConfluentKafkaHelper(IEnumerable<KeyValuePair<string, string>> config)
        {
            this.config = config ?? new Dictionary<string, string>
             {
                 //同一个Topic的一条消息只能被同一个Consumer Group内的一个Consumer消费，但多个Consumer Group可同时消费这一消息
                 { "group.id", "test-group" },
                  //kafka的集群消费地址
                 { "bootstrap.servers", "127.0.0.1:9092" },
                  //consumer向consumer提交offset的频率，单位ms
                 {"auto.commit.interval.ms","5000"},
                  //earliest和latest：当各分区下有已提交的offset时，从提交的offset开始消费，无提交的offset时，earliest是从头开始消费、latest从末尾开始消费；
                  //none：当各分区下存在已提交的offset时，从offset后开始消费，但只要有一个分区不存在已提交的offset时，则抛出异常
                 {"auto.offset.reset","earliest"},

            };
        }
        /// <summary>
        /// 生产者 生产数据 (推送数据)
        /// </summary>
        /// <param name="topic">一个Topic包含一个或多个Partition，建Topic的时候可以手动指定Partition个数，个数与服务器个数相当</param>
        /// <param name="msg">每条消息属于且仅属于一个Topic</param>
        /// <param name="key"></param>
        public virtual void Push(string topic, string key, string msg)
        {
            using (var producer = new ProducerBuilder<string, string>(config).Build())
            {
                var dr = producer.ProduceAsync(topic, new Message<string, string>() { Key = key, Value = msg }).Result;
                //Console.WriteLine(dr.TopicPartitionOffset);
                producer.Flush(TimeSpan.FromSeconds(3));
            }
        }
        /// <summary>
        /// 不推荐 使用 一条一条 处理 效率 慢 (重复创建对象)
        /// 消费者 处理 数据 单条 数据
        /// 怎么解决已处理过的数据了, 数据一直存在(正常情况下不会发生 ,自动处理, 杀死进程、 提前结束等结束 导致重写读取 到脏数据(因为内部偏移量 处理未完成))
        /// 有 数据 手动确认偏移量
        /// </summary>
        /// <param name="topic">一个Topic包含一个或多个Partition，建Topic的时候可以手动指定Partition个数，个数与服务器个数相当</param>
        /// <param name="result">结果</param>
        public virtual void Pull(string topic, Action<ConsumeResult<string, string>> result)
        {
            //连续 Pull  不能用 using 老是 没执行 完成自动退出了 还不会自动提交偏移量(不用 using 也是 没执行 完成自动退出,但会自动提交偏移量,不会出现脏读)
            // using (var consumer = new ConsumerBuilder<string, string>(config).Build())
            var consumer = new ConsumerBuilder<string, string>(config).Build();
            {
                //Consumer 同一程序集 下可用 internal 
                //consumer.Consume += (_, msg)
                //  => Console.WriteLine(msg.Value);

                //consumer.OnError += (_, error)
                //  => Console.WriteLine(error);

                //consumer.OnConsumeError += (_, msg)
                //  => Console.WriteLine(msg);
                //默认 方式 读取 未完成 ,可能重新读取(数据量多时处理慢会丢失已消费过的数据,最好手动提交偏移)也可能读取未消费过的数据.读取完成后,重新读取不到
                //消费者处理完成  标识已消费消息已处理
                //如何确定 消息一条数据标记一次
                consumer.Subscribe(topic);
                //可能 死线程 (条件触发才不是死线程)
                System.Threading.Thread.Sleep(2000);//立即触发会没有数据 为 null
                var data = consumer.Consume(100);//每次请求一次 
                //CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                //data=consumer.Consume(cancellationTokenSource.Token);//为 null 一直循环  找到为止
                if (data != null)
                {
                    //consumer.Assign(data.TopicPartition);//客户端保存自己的当前偏移
                    //自动 处理 (用不同 的包(kafka-core) 怎么 还重复 读脏数据)
                    //手动提交 稳定些 (不管他  自动 提交)
                    data.Offset = data.TopicPartitionOffset.Offset;//已处理偏移
                    consumer.Commit(data); //连同提交到服务端对应的偏移中进行更改
                    result?.Invoke(data);
                }

                //while (true)
                //{
                //    consumer.Poll(TimeSpan.FromMilliseconds(100));
                //}
            }
        }

        /// <summary>
        /// 消费者 处理 数据 多条 数据(一直处理 )
        /// 怎么解决已处理过的数据了, 数据一直存在(正常情况下不会发生 ,自动处理)
        /// 有 数据 手动确认偏移量
        /// </summary>
        /// <param name="topic">一个Topic包含一个或多个Partition，建Topic的时候可以手动指定Partition个数，个数与服务器个数相当</param>
        /// <param name="cancellationToken">信号</param>
        /// <param name="result">结果</param>
        public virtual void Pull(string topic, Action<ConsumeResult<string, string>> result, CancellationToken cancellationToken)
        {
            using (var consumer = new ConsumerBuilder<string, string>(config).Build())
            {
                //默认 方式 读取 未完成 ,可能重新读取(数据量多时处理慢会丢失已消费过的数据,最好手动提交偏移)也可能读取未消费过的数据.读取完成后,重新读取不到
                //消费者处理完成  标识已消费消息已处理
                //如何确定 消息一条数据标记一次
                consumer.Subscribe(topic);
                while (!cancellationToken.IsCancellationRequested)
                {
                    var data = consumer.Consume(cancellationToken);//为 null 一直循环  找到为止
                    if (data != null)
                    {
                        //consumer.Assign(data.TopicPartition);//客户端保存自己的当前偏移
                        //自动 处理 (用不同 的包(kafka-core) 怎么 还重复 读脏数据)
                        data.Offset = data.TopicPartitionOffset.Offset;//已处理偏移
                        consumer.Commit(data); //连同提交到服务端对应的偏移中进行更改
                        result?.Invoke(data);
                    }
                }
            }
        }
    }
}
#endif