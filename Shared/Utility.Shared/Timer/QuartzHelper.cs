#if !(NET10 || NET11 || NET20 || NET30 || NET35 || NET40 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
//#if NET45 || NET451 || NET452 || NET46 ||NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using Quartz;
using Quartz.Impl;
using System;

namespace Utility.Timer
{
    /// <summary>
    /// 
    /// </summary>
    public  class QuartzHelper
    {
        /// <summary>
        /// 
        /// </summary>
        public  readonly ISchedulerFactory SchedulerFactory = new StdSchedulerFactory();
        private  static readonly object _obj = new object();
        private readonly  IScheduler _scheduler=null;
        private static QuartzHelper _factory =null;

        /// <summary>
        /// 
        /// </summary>
        public QuartzHelper()
        {
#if NET45 || NET451 || NET452 || NET46
            this._scheduler = SchedulerFactory.GetScheduler();
            throw new NotSupportedException();
#else
            //this._scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;//单例模式：是指无论多少个用户访问，都只有一个实例
            this._scheduler = SchedulerFactory.GetScheduler().Result;　//通过调度工厂获得调度器
#endif

        }

        /// <summary>
        /// 
        /// </summary>
        public static QuartzHelper Instance {
            get {
                if(_factory==null)
                {
                    lock(_obj)
                    {
                        if (_factory == null)
                        {
                            _factory = new QuartzHelper();
                        }
                    }
                }
                return _factory;
            }
        }

        /// <summary> 开始 </summary>
        public virtual  void Start()
        {
             this._scheduler.Start();//开启调度器
        }

        /// <summary> 停止 </summary>
        public virtual  void Stop()
        {
            if (!this._scheduler.IsShutdown)
            {
                 this._scheduler.Shutdown();
            }
        }

        /// <summary> 暂停</summary>
        public  virtual void Pause()
        {
             this._scheduler.PauseAll();
        }

        /// <summary>继续 </summary>
        public virtual  void Continue()
        {
            this._scheduler.ResumeAll();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="job"></param>
        /// <param name="trigger"></param>
        public virtual  void AddJob(IJobDetail job, ITrigger trigger)
        {
            this._scheduler.ScheduleJob(job,trigger);//将触发器和任务器绑定到调度器中
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jobKey"></param>
        /// <param name="triggerKey"></param>
        /// <param name="secods"></param>
        public virtual void AddJob<T>(JobKey jobKey, TriggerKey triggerKey,int secods)where T:IJob
        {
            this.AddJob<T>(jobKey,triggerKey,secods, Flag.Second);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jobKey"></param>
        /// <param name="triggerKey"></param>
        /// <param name="timer"></param>
        /// <param name="flag"></param>
        public virtual void AddJob<T>(JobKey jobKey, TriggerKey triggerKey, int timer,Flag flag= Flag.None) where T : IJob
        {
            IJobDetail job = JobBuilder.Create<T>().WithIdentity(jobKey).Build();//创建任务
            ITrigger trigger = TriggerBuilder.Create().StartNow().WithSimpleSchedule(it =>
            {
                switch (flag)
                {
                    case Flag.Second:
                        it.WithIntervalInSeconds(timer).RepeatForever();
                        break;
                    case Flag.Hour:
                        it.WithIntervalInHours(timer).RepeatForever();
                    
                        break;
                    case Flag.Minutes:
                        it.WithIntervalInMinutes(timer).RepeatForever();
                        break;
                    case Flag.None:
                    default:
                        it.WithInterval(new TimeSpan(timer * 10000)).RepeatForever();
                        break;
                }
            }
            ).WithIdentity(triggerKey).Build();//创建一个触发器
            this.AddJob(job, trigger);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public  virtual void DeleteJob(JobKey key)
        {
            this._scheduler?.DeleteJob(key);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual  void DefaultTask()
        {
           // Start();
            IJobDetail job = JobBuilder.Create<JobTask>()
                 .UsingJobData("key1", 123)//通过Job添加参数值
                 .UsingJobData("key2", "123")
                  //.WithIdentity(JobKey.Create("job","group"))
                  .StoreDurably(true)
                 .Build();
            ITrigger trigger = TriggerBuilder.Create().StartNow().WithSimpleSchedule(it=>it.WithIntervalInSeconds(2).RepeatForever().Build())//间隔10秒 一直执行
                 .UsingJobData("key1", 321)  //通过在Trigger中添加参数值
                  .UsingJobData("key2", "321")
                // .WithIdentity(new TriggerKey("trigger", "group"))
                .Build();
            this.AddJob(job,trigger);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public enum Flag
    {
        /// <summary>
        /// 
        /// </summary>
        None,

        /// <summary>
        /// 
        /// </summary>
        Second,

        /// <summary>
        /// 
        /// </summary>
        Hour,
        /// <summary>
        /// 
        /// </summary>
        Minutes
    }
}
#endif