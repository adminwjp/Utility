#if !(NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Utility.Logs;
using Utility.Randoms;

namespace Utility.Threads
{
    /// <summary>
    /// 任务 队列
    /// </summary>
    public  class TaskQueue
    {
#if !(NET20 || NET30 || NET35 || NETSTANDARD1_0)
        private readonly System.Collections.Concurrent.ConcurrentQueue<Action> _queue = new System.Collections.Concurrent.ConcurrentQueue<Action>();
#else
        private readonly Utility.Collections.ConcurrentArray<Action> _queue = new Utility.Collections.ConcurrentArray<Action>();
#endif
        private ILog<TaskQueue> log;

        /// <summary>
        /// 任务 队列
        /// </summary>
        public TaskQueue()
        {
            this.log = new DefaultLog<TaskQueue>();
        }

        private ThreadEntity _mainTask;

        /// <summary>
        /// 线程 帮助类
        /// </summary>
        public ThreadHelper _threadHelper { get; set; } = ThreadHelper.Instance;
        private readonly AutoResetEvent _waitHandle = new AutoResetEvent(false);
#if !(NET10 || NET11 || NET20 || NET30 || NET35)
        private readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(10);
#endif
        /// <summary>
        /// 最小 任务
        /// </summary>
        public int MinTask { get; set; } = 5;
        /// <summary>
        ///最大 任务
        /// </summary>
        public int MaxTask { get; set; } = 10;
        /// <summary>
        /// 最小空闲 任务
        /// </summary>
        public int MinIdeaTask { get; set; } = 7;
        /// <summary>
        /// 最大 空闲 任务
        /// </summary>
        public int MaxIdeaTask { get; set; } = 7;
        /// <summary>
        /// 休眠 最小 时间
        /// </summary>
        public int MinSleep { get; set; } = 100;
        /// <summary>
        /// 休眠 最大 时间
        /// </summary>
        public int MaxSleep { get; set; } = 500;
        /// <summary>
        /// 是否 推送 任务 完成  用于 检测 任务 是否 完成 不然 无法确定 任务 完成
        /// </summary>
        public bool PullTaskEnd { get; set; }

        /// <summary>
        /// 任务 是否结束
        /// </summary>
        public virtual bool TaskComplete { get {
                if (this.PullTaskEnd&&this._threadHelper != null&&this._threadHelper.Complete)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 任务 数量
        /// </summary>
        public int Count
        {
            get
            {
                return this._queue.Count;
            }
        }

        /// <summary>
        /// 获取 任务
        /// </summary>
        /// <returns></returns>
        private Action Pop()
        {
             this._queue.TryDequeue(out Action action);
             return action;
        }

        /// <summary>
        /// 推送 任务
        /// </summary>
        /// <param name="action"></param>
        public virtual void Push(Action action)
        {
            this._queue.Enqueue(action);
        }

        /// <summary>
        /// 启动 执行 任务
        /// </summary>
        public virtual void Start()
        {
#if !(NET20 || NET30 || NET35)
            if (_mainTask!=null&&_mainTask.CancellationToken != null && !_mainTask.CancellationToken.IsCancellationRequested)
            {
                return;
            }
#else
            if (_mainTask != null && !_mainTask.IsCancellationRequested) return;
#endif
            this._mainTask = new ThreadEntity(true).Create(this.Execute);
            this._mainTask.Start();
            Initial();
        }

        /// <summary>
        /// 停止 任务
        /// </summary>
        public virtual void Stop()
        {
            this._mainTask?.Abort();
            //foreach (var item in _threadHelper.Threads)
            //{
            //    if (!item.Value.Stop)
            //    {
            //        item.Value.Abort();
            //    }
            //}
        }

        /// <summary>
        /// 分配任务 
        /// </summary>
        protected virtual void Execute()
        {
   
#if !(NET20 || NET30 || NET35)
            while (!_mainTask.CancellationToken.IsCancellationRequested)
#else
            while (!_mainTask.IsCancellationRequested)
#endif
            {
                if (this._queue.Count > 0)
                {
                    //分配任务
                    this._waitHandle.Set();
                }
                Thread.Sleep(RandomHelper.Random.Next(this.MinSleep / 10, this.MaxSleep / 10));
            }
        }

        /// <summary>
        /// 任务执行 
        /// </summary>
        /// <param name="obj"></param>
        private void DefaultTask(object obj)
        {
            
            //semaphoreSlim.Wait();//有毛线用 这种场景 下 限制
            ThreadEntity thread = obj as ThreadEntity;
            if (thread == null)
            {
                throw new ArgumentNullException("thread is not null");
            }
#if !(NET20 || NET30 || NET35)
            while (!thread.CancellationToken.IsCancellationRequested)
#else
            while (!thread.IsCancellationRequested)
#endif
            {
                try
                {
                 
                    this._waitHandle.WaitOne(10*500);//等待分配任务
                
                    var task = this.Pop();
                    if (task != null)
                    {
                        thread.Status = false;
                        task?.Invoke();
                        thread.Status = true;
                    }
                    else
                    {
                        thread.Status = true;
                    }
                    Thread.Sleep(RandomHelper.Random.Next(this.MinSleep, this.MaxSleep));
                }
                catch (Exception e)
                {
                    log.LogException(LogLevel.Error, "execute task fail !", e);
                }
            }
            //semaphoreSlim.Release();
        }
        
        /// <summary>
        /// 启动 任务
        /// </summary>
        private void Initial()
        {
            if (this._threadHelper.Threads.Count == 0)
            {
                for (int i = 0; i < this.MaxTask; i++)
                {
                    string name = $"task{i + 1}";
                    this._threadHelper.Create(name, (it=> { this.DefaultTask(it); }));
                    this._threadHelper[name].Start(this._threadHelper[name]);
                }
            }
        }
    }
}
#endif