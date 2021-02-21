#if !(NET20 || NET30 || NET35 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2)
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace Utility.Net.Sockets
{
    /// <summary>
    /// socket 客户端
    /// </summary>
    public class SocketClient:BaseSocket
    {
        private Utility.Collections.Array<byte> _buffer; //接收缓存数据
        private AutoResetEvent  _autoResetEvent=new AutoResetEvent(false);
        private bool _connected;//设置socket connected的标志。
        private SocketAsyncEventArgs _socketAsyncEventArgs;//服务端连接回复
        private IPEndPoint _remoteEndPoint;//服务器 地址
       
        /// <summary>
        /// 创建未初始化的服务器实例。启动服务器以侦听连接请求 先调用Init方法，再调用Start方法
        /// </summary>
        /// <param name="receiveBufferSize">用于每个套接字I/O操作的缓冲区大小</param>
        public SocketClient(int receiveBufferSize)
        {
            base.BufferManager = new BufferManager(receiveBufferSize * 2, receiveBufferSize);
            this._buffer = new Utility.Collections.Array<byte>();
        }

        /// <summary>
        /// 绑定服务器
        /// </summary>
        /// <param name="remoteEndPoint"></param>
        public virtual void BindServer(IPEndPoint remoteEndPoint)
        {
            this._remoteEndPoint = remoteEndPoint;
        }

        /// <summary>
        /// 启动 tcp 客户端 连接 服务器端
        /// </summary>
        /// <param name="localEndPoint"></param>
        public override void Start(IPEndPoint localEndPoint)
        {
            base.Socket = new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            base.Socket.Bind(localEndPoint);//绑定客户端地址
            //服务器
            SocketAsyncEventArgs connectArgs = new SocketAsyncEventArgs();
            connectArgs.UserToken =new AsyncUserToken() { Socket = base.Socket,Id=Guid.NewGuid().ToString("N")};
            connectArgs.RemoteEndPoint = this._remoteEndPoint;//绑定服务器 地址
            connectArgs.Completed += new EventHandler<SocketAsyncEventArgs>(base.AcceptEventArg_Completed);
            base.Socket.ConnectAsync(connectArgs);
            this._autoResetEvent.WaitOne(); //阻塞. 让程序在这里等待,直到连接响应后再返回连接结果  
        }

        /// <summary>
        /// 客户端连接上 服务器端 数据通信 处理
        /// </summary>
        /// <param name="e"></param>
        protected override void ProcessAccept(SocketAsyncEventArgs e)
        {
            this._autoResetEvent.Set(); //表示连接结束 释放阻塞.  
            this._connected = (e.SocketError == SocketError.Success);//设置socket connected的标志。
            //如果连接成功,则初始化socketAsyncEventArgs  
            if (this._connected)
            {
                base.BufferManager.InitBuffer();
                this._socketAsyncEventArgs = base.GetSocketAsyncEventArgs();
                this._socketAsyncEventArgs.UserToken = e.UserToken;//一直循环  Completed事件 一直通信
                base.BufferManager.SetBuffer(this._socketAsyncEventArgs);
                if (!e.ConnectSocket.ReceiveAsync(this._socketAsyncEventArgs)) //启动接收,不管有没有,一定得启动.否则有数据来了也不知道.  
                    base.ProcessReceive(this._socketAsyncEventArgs);
            }
        }

        /// <summary>
        /// 测试 语法
        /// </summary>
        /// <returns></returns>
        int M()
        {
            int y;
            LocalFunction();
            return y;

            void LocalFunction() => y = 0;
        }
    }
}
#endif