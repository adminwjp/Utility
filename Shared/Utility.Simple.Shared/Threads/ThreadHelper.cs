#if !(NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
namespace Utility.Threads
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    #if !(NET20 || NET30 || NET35)
    using System.Threading.Tasks;
#endif

    /// <summary>
    /// 线程 公共类 不支持 netstandard 1.0 - 1.6
    /// </summary>
    public class ThreadHelper
    {
        /// <summary>
        /// 
        /// </summary>
        public readonly static Action EmptyAction = () => { };
    
        /// <summary>
        /// 内部类
        /// </summary>
        class InnerThread
        {
            ///<summary>
            ///声明并初始化
            /// </summary>
            public static readonly ThreadHelper ThreadObject = new ThreadHelper();
        }
        /// <summary>
        /// 初始化 ThreadHelper 对象 饿汉式 单例模式 
        /// </summary>
        public static ThreadHelper Instance
        {
            get
            {
                return InnerThread.ThreadObject;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Complete { 
            get {
                if (this.Threads != null&& this.Threads.Count>0)
                {
                    foreach (var item in this.Threads)
                    {
                        if (!item.Value.Status)
                            return false;
                    }
                    return true;
                }
                return false;
            }
        }
        private readonly IDictionary<string, ThreadEntity> _threads = new Dictionary<string, ThreadEntity>();//线程集合类

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ThreadEntity this[string name]
        {
            get
            {
                if (_threads.ContainsKey(name)) return _threads[name];
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ThreadEntity this[int index]
        {
            get
            {
                index = index < 0 ? index + _threads.Count : index;
                if (_threads.Count > index || index < 0) 
                    return null;
                int i = 0;
                using (var iterator= _threads.Values.GetEnumerator())
                {

                    while (iterator.MoveNext())
                    {
                        if (i == index)
                        {
                            return iterator.Current;
                        }
                        ++i;
                    }
                }
                return null;
            }
        }
        /// <summary>
        /// 获取线程集合
        /// </summary>
        public virtual IDictionary<string, ThreadEntity> Threads => _threads;
        /// <summary>
        /// 创建线程
        /// </summary>
        /// <param name="name">线程名称</param>
        /// <param name="thread">线程对学校</param>
        /// <exception cref="Exception"></exception>
        public virtual void Create(string name, ThreadEntity thread)
        {
            if (Threads.ContainsKey(name))
            {
                throw new Exception($"key {nameof(name)} exists");
            }
            else
            {
                Threads.Add(name, thread);
            }
        }
        /// <summary>
        /// 创建线程
        /// </summary>
        /// <param name="name">线程名称</param>
        /// <exception cref="Exception"></exception>
        public virtual void Create(string name)
        {
            if (Threads.ContainsKey(name))
            {
                throw new Exception($"key {nameof(name)} exists");
            }
            else
            {
                Threads.Add(name, new ThreadEntity().Create(EmptyAction));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public virtual void StartTask(string name)
        {
            if (!Threads.ContainsKey(name))
            {
                throw new Exception($"key {nameof(name)} not exists");
            }
            var thread = this[name];
            thread.Start();
        }
  
        /// <summary>
        /// 创建线程
        /// </summary>
        /// <param name="name">线程名称</param>
        /// <param name="action">线程执行方法</param>
        public virtual void Create(string name, Action action)
        {
            Create(name, new ThreadEntity().Create(action));
        }
        /// <summary>
        /// 创建线程
        /// </summary>
        /// <param name="name">线程名称</param>
        /// <param name="action">线程执行方法</param>
        public virtual void Create(string name, Action<object> action)
        {
            Create(name, new ThreadEntity().Create(action));
        }

        /// <summary>
        /// 终止线程
        /// <para>
        /// 不支持netcoreapp 1.0 - 1.1
        /// </para>
        /// </summary>
        /// <param name="name">线程名称</param>
        /// <exception cref="Exception"></exception>
        public virtual void Abort(string name)
        {
#if !(NETSTANDARD1_0 || NETSTANDARD1_1 )
            if (!Threads.ContainsKey(name))
            {
                throw new Exception($"key {nameof(name)} exists");
            }
            else
            {
                try
                {
                    Threads[name].Abort();
                }
                catch (Exception)
                {
                }
            }
#endif
        }
        /// <summary>
        /// 阻塞线程
        /// </summary>
        /// <param name="name"></param>
        public virtual void Join(string name)
        {
            var thread = this[name];
            if (thread != null)
            {
                thread.Join();
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class ThreadEntity
    {
        private bool IsTask { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ThreadEntity():this(false)
        {
           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isTask"></param>
        public ThreadEntity(bool isTask)
        {
            IsTask = isTask;
        }
#if !(NET20 || NET30 || NET35)
        /// <summary>
        /// samephore
        /// </summary>
        public readonly CancellationTokenSource CancellationToken = new CancellationTokenSource();
#else
        /// <summary>
        /// samephore
        /// </summary>
        public bool IsCancellationRequested { get; set; }
#endif
        /// <summary>
        /// 线程
        /// </summary>

#if !(NET20 || NET30 || NET35 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
        private Task TaskThread;
#endif
        private Thread Thread { get; set; }

        /// <summary>
        /// 线程 执行 任务专题
        /// </summary>
        public bool Status { get; set; }
        /// <summary>
        /// 线程 是否停止 
        /// </summary>
        public bool Stop { get; set; }
        /// <summary>
        /// 任务
        /// </summary>
        private Action Action { get; set; }
        /// <summary>
        /// 任务
        /// </summary>
        private Action<object> Action1 { get; set; }
        /// <summary>
        /// 设置 线程 任务 
        /// </summary>
        /// <param name="action">线程执行方法</param>
        public  ThreadEntity Create(Action action)
        {
            Action = action;
            return this;
        }

        /// <summary>
        /// 创建 线程 并 启动
        /// </summary>
        /// <returns></returns>
        public ThreadEntity Start()
        {
            if (IsTask)
            {
#if !(NET20 || NET30 || NET35 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
            TaskThread=Task.Factory.StartNew(Action
#if !(NET20 || NET30 || NET35)
            ,CancellationToken.Token
#endif
            );
              return this;
#endif
            }
            Thread = new System.Threading.Thread(() => { Action(); });
            Thread.Start();
            return this;
        }

        /// <summary>
        ///  创建 线程 并 启动
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public ThreadEntity Start(object obj)
        {
            if (IsTask)
            {
#if !(NET20 || NET30 || NET35 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
            TaskThread=Task.Factory.StartNew(()=>Action1(obj)
#if !(NET20 || NET30 || NET35)
            ,CancellationToken.Token
#endif
            );
              return this;
#endif
            }
            Thread = new System.Threading.Thread((it) => Action1(it));
            Thread.Start(obj);
            return this;
        }
        /// <summary>
        ///  设置 线程 任务
        /// </summary>
        /// <param name="action">线程执行方法</param>
        public  ThreadEntity Create(Action<object> action)
        {
            Action1 = action;
            return this;
        }
        /// <summary>
        /// 终止线程
        /// <para>
        /// 不支持netcoreapp 1.0 - 1.1
        /// </para>
        /// </summary>
        public  ThreadEntity Abort()
        {
            try
            {
#if !(NET20 || NET30 || NET35)
            CancellationToken.Cancel();
#else
                IsCancellationRequested = true;
#endif


#if !(NET20 || NET30 || NET35 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
            if (IsTask)   return this;
#endif

#if !(NETCOREAPP1_0 || NETCOREAPP1_1)
                try
                {
                    Thread.Abort();
                }
                catch (Exception)
                {
                }
#endif
                return this;
            }
            finally
            {
                Stop = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  ThreadEntity Join()
        {

#if !(NET20 || NET30 || NET35 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
            if (IsTask)
             {
                TaskThread.Wait();
                  return this;
             }
#endif

            Thread.Join();
            return this;
        }
    }
}
#endif
