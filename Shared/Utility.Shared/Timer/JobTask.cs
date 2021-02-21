#if !(NET10 || NET11 || NET20 || NET30 || NET35 || NET40 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
//#if NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using Quartz;
using System;
using System.Threading.Tasks;

namespace Utility.Timer
{
    /// <summary>
    /// 
    /// </summary>
    //禁止并发执行多个相同定义的JobDetail, 这个注解是加在Job类上的, 但意思并不是不能同时执行多个Job,
    //而是不能并发执行同一个Job Definition(由JobDetail定义), 但是可以同时执行多个不同的JobDetail
    [DisallowConcurrentExecution]
    //更新JobDetail的JobDataMap的存储副本，以便下一次执行这个任务接收更新的值而不是原始存储的值
    [PersistJobDataAfterExecution]
    public class JobTask : IJob
    {

        /// <summary>
        /// 
        /// </summary>
        public JobTask()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public virtual void Execute1(IJobExecutionContext context)
        {
            var jobData = context.JobDetail.JobDataMap;//获取Job中的参数

            var triggerData = context.Trigger.JobDataMap;//获取Trigger中的参数

            var data = context.MergedJobDataMap;//获取Job和Trigger中合并的参数

            var value1 = jobData.GetInt("key1");
            var value2 = jobData.GetString("key2");
            Console.WriteLine("job: key1-{0} key2={1} ", value1, value2);

            value1 = triggerData.GetInt("key1");
            value2 = triggerData.GetString("key2");
            Console.WriteLine("trigger: key1-{0} key2={1} ", value1, value2);

            value1 = data.GetInt("key1");
            value2 = data.GetString("key2");
            Console.WriteLine("merged: key1-{0} key2={1} ", value1, value2);
            /*return Task.Run(()=> {
                Log4Utils.Info("每隔10秒执行该方法");
            });*/
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
#if NET45 || NET451 || NET452 || NET46
        public virtual void Execute(IJobExecutionContext context)
        {
            Execute1(context);
        }
#else
        public virtual Task Execute(IJobExecutionContext context)
        {
            Execute1(context);
            return new Task(() => { });
        }
#endif

    }
}
#endif