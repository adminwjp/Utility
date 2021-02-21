#if !(NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2)
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
#if !(NET20 || NET30 || NET35 )
using System.Threading.Tasks;
#endif

namespace Utility.Net.Sockets
{
    /// <summary>
    /// tcp 基类
    /// </summary>
    public abstract class BaseTcpService:IDisposable
    {
        /// <summary>
        /// 网络流
        /// </summary>
        protected virtual NetworkStream Stream { get; set; }
        /// <summary>
        /// 线程信号量
        /// </summary>
        private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(false);
#if !(NET20 || NET30 || NET35)
        private Task _task;//线程
        /// <summary>
        /// 线程 信号
        /// </summary>
        protected CancellationTokenSource CancellationTokenSource;
#else
        private Thread _thread;//线程

#endif
        /// <summary>
        /// 线程 信号
        /// </summary>
        protected bool IsCancellationRequested=false;
        /// <summary>
        /// tcp 读取 缓存数据
        /// </summary>
        protected byte[] Buffer;

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="data">将消息发送到连接的Tcp</param>
        public  virtual void Send(byte[] data)
        {
            this.Stream.Write(data, 0, data.Length);//将消息发送到连接的Tcp
        }

        /// <summary>
        /// 读取信息
        /// </summary>
        /// <param name="data">接收Tcp.response 用于存储响应字节的缓冲区</param>
        /// <returns></returns>
        public  virtual int Read(byte[] data)
        {
            return this.Stream.Read(data, 0, data.Length);//读取第一批Tcp响应字节
        }

        /// <summary>
        /// tcp 线程启动
        /// </summary>
        public virtual void Start()
        {
#if !(NET20 || NET30 || NET35)
            if (this.CancellationTokenSource == null || this.CancellationTokenSource.IsCancellationRequested)
            {
                this.CancellationTokenSource = new CancellationTokenSource();
                this._task = Task.Factory.StartNew(Loop, this.CancellationTokenSource.Token);
            }
#else
            if (this._thread == null)
            {
                this._thread = new Thread(Loop);
            }
#endif
            IsCancellationRequested = false;
        }

        /// <summary>
        /// tcp 线程 停止
        /// </summary>
        public virtual void Stop()
        {
#if !(NET20 || NET30 || NET35)
            this.CancellationTokenSource?.Cancel();
#else
            this.IsCancellationRequested = true;
#endif
    
        }
        /// <summary>
        /// 通信线程
        /// </summary>
        protected abstract void Loop();
       
        /// <summary>
        /// 线程阻塞
        /// </summary>
        public virtual void WaitOne()
        {
            this._autoResetEvent.WaitOne();
        }
        
        /// <summary>
        /// 线程继续
        /// </summary>
        public virtual void Set()
        {
            this._autoResetEvent.Set();
        }

        /// <summary>
        /// tcp 线程停止 释放线程
        /// </summary>
        public virtual void  Dispose()
        {
            Stop();
#if !(NET20 || NET30 || NET35 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
            this._task.Dispose();
#endif
        }
    }
}
#endif