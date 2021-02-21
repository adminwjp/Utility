#if !(NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2)
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Utility.Net.Sockets
{
    /// <summary>
    /// 服务器 客户端  基类
    /// </summary>
    public abstract class BaseSocket
    {
        internal BufferManager BufferManager=null;//表示所有套接字操作的一组可重用的大缓冲区
        /// <summary>
        /// 用于侦听传入连接请求的套接字
        /// </summary>
        protected Socket Socket;
        /// <summary>
        /// 启动服务器以便它正在侦听传入连接请求 或 客户端 连接服务器 
        /// </summary>
        /// <param name="localEndPoint">服务器将在其上侦听连接请求的终结点</param>
        public abstract void Start(IPEndPoint localEndPoint);
        /// <summary>
        /// 此方法是与Socket.acceptsync操作关联的回调方法，并在接受操作完成时调用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected  virtual void AcceptEventArg_Completed(object sender, SocketAsyncEventArgs e)
        {
            ProcessAccept(e);
        }

        /// <summary>
        /// tpc 连接完成 绑定事件
        /// </summary>
        /// <returns></returns>
        public virtual SocketAsyncEventArgs   GetSocketAsyncEventArgs()
        {
            SocketAsyncEventArgs readWriteEventArg=new SocketAsyncEventArgs(); //预分配一组可重用的SocketAsyncEventArgs
            readWriteEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
            readWriteEventArg.UserToken = new AsyncUserToken() { Id = Guid.NewGuid().ToString("N") };
            this.BufferManager.SetBuffer(readWriteEventArg);//将缓冲池中的字节缓冲区分配给SocketAsyncEventArg对象
            return readWriteEventArg;
        }

        /// <summary>
        /// tpc 连接 上 数据通信处理
        /// </summary>
        /// <param name="e"></param>
        protected abstract void ProcessAccept(SocketAsyncEventArgs e);
        
        /// <summary>
        /// 每当在套接字上完成接收或发送操作时，都会调用此方法 需要改 不然通信一直循环(因为递归造成的)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">与已完成的接收操作关联的SocketAsyncEventArg</param>
        private void IO_Completed(object sender, SocketAsyncEventArgs e)
        {
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Receive:
                    ProcessReceive(e);
                    break;
                case SocketAsyncOperation.Send:
                    ProcessSend(e);
                    break;
                default:
                    throw new ArgumentException("在套接字上完成的最后一个操作不是接收或发送");
            }
        }
        /// <summary>
        /// 此方法在异步接收操作完成时调用。如果远程主机关闭了连接，则套接字将关闭。如果接收到数据，则将数据回传给客户端。
        /// </summary>
        /// <param name="e"></param>
        protected virtual void ProcessReceive(SocketAsyncEventArgs e)
        {
            AsyncUserToken token = (AsyncUserToken)e.UserToken;
            if (e.BytesTransferred > 0 && e.SocketError == SocketError.Success)//检查远程主机是否已关闭连接
            {
                e.SetBuffer(e.Offset, e.BytesTransferred);//将接收到的数据回显到客户端
                bool willRaiseEvent = token.Socket.SendAsync(e);
                if (!willRaiseEvent)
                {
                    ProcessSend(e);
                }
            }
            else
            {
                CloseClientSocket(e);
            }
        }
        /// <summary>
        /// 此方法在异步发送操作完成时调用。方法在套接字上发出另一个接收，以读取任何附加的从客户端发送的数据
        /// </summary>
        /// <param name="e"></param>
        private void ProcessSend(SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                AsyncUserToken token = (AsyncUserToken)e.UserToken;//完成将数据回传给客户端
                bool willRaiseEvent = token.Socket.ReceiveAsync(e);//读取从客户端发送的下一个数据块
                if (!willRaiseEvent)
                {
                    ProcessReceive(e);
                }
            }
            else
            {
                CloseClientSocket(e);
            }
        }

        /// <summary>
        /// socket 客户端关闭
        /// </summary>
        /// <param name="e"></param>
        private void CloseClientSocket(SocketAsyncEventArgs e)
        {
            AsyncUserToken token = e.UserToken as AsyncUserToken;
            try
            {
                token.Socket.Shutdown(SocketShutdown.Send);//关闭与客户端关联的套接字
            }
            catch (Exception) { }//如果客户端进程已关闭，则引发
#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
            token.Socket.Close();
#else
            token.Socket.Dispose();
#endif
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        public virtual bool SendData(byte[] data)
        {
            if (this.Socket.Connected)
            {
               return this.Socket.Send(data, 0, data.Length, SocketFlags.None)>0;
            }
            return false;
            
        }

    }
}
#endif