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
    /// 实现套接字服务器的连接逻辑。接受连接后，从客户端读取的所有数据发送回客户端。读取并回显到客户机模式 一直持续到客户端断开连接。
    /// </summary>
    public class SocketServer
    {
        private  int _numConnections;//同时处理的最大连接数
        private int _receiveBufferSize;//用于每个套接字I/O操作的缓冲区大小
        private BufferManager _bufferManager;//表示所有套接字操作的一组可重用的大缓冲区
        private const int opsToPreAlloc = 2;//读、写（不为accepts分配缓冲区空间）
        private Socket _serverSocket;//用于侦听传入连接请求的套接字
        private SocketAsyncEventArgsPool _readWritePool;//可重用SocketAsyncEventArgs对象池，用于写入、读取和接受套接字操作
        private int _totalBytesRead;           // 服务器接收的总字节的计数器
        private int _numConnectedSockets;      // 连接到服务器的客户端总数
        private Semaphore _maxNumberAcceptedClients;//线程同时访问同一资源最大访问量 信号量
        /// <summary>
        /// 创建未初始化的服务器实例。启动服务器以侦听连接请求 先调用Init方法，再调用Start方法
        /// </summary>
        /// <param name="numConnections">同时处理的最大连接数</param>
        /// <param name="receiveBufferSize">用于每个套接字I/O操作的缓冲区大小</param>
        public SocketServer(int numConnections, int receiveBufferSize)
        {
            _totalBytesRead = 0;
            this._numConnectedSockets = 0;
            this._numConnections = numConnections;
            this._receiveBufferSize = receiveBufferSize;
            //分配缓冲区，以便最大数量的套接字可以有一个未完成的读取和同时写入到套接字
            this._bufferManager = new BufferManager(receiveBufferSize * numConnections * opsToPreAlloc,receiveBufferSize);
            this._readWritePool = new SocketAsyncEventArgsPool(numConnections);
            this._maxNumberAcceptedClients = new Semaphore(numConnections, numConnections);
        }
        /// <summary>
        ///通过预先分配可重用缓冲区和上下文对象。这些对象不需要预先分配或者重用，但是这样做是为了说明API如何易于用于创建可重用对象以提高服务器性能。
        /// </summary>
        public virtual void Init()
        {
            _bufferManager.InitBuffer();//分配一个大字节缓冲区，所有I/O操作都使用一个大字节缓冲区。反对内存碎片
            SocketAsyncEventArgs readWriteEventArg;//预分配SocketAsyncEventArgs对象池
            for (int i = 0; i < this._numConnections; i++)
            {
                readWriteEventArg = new SocketAsyncEventArgs(); //预分配一组可重用的SocketAsyncEventArgs
                readWriteEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
                readWriteEventArg.UserToken = new AsyncUserToken() { /*Id=Guid.NewGuid().ToString("N")*/};
                this._bufferManager.SetBuffer(readWriteEventArg);//将缓冲池中的字节缓冲区分配给SocketAsyncEventArg对象
                this._readWritePool.Push(readWriteEventArg);  // 将SocketAsyncEventArg添加到池中
            }
        }
        /// <summary>
        /// 启动服务器以便它正在侦听传入连接请求。
        /// </summary>
        /// <param name="localEndPoint">服务器将在其上侦听连接请求的终结点</param>
        public virtual void Start(IPEndPoint localEndPoint)
        {
            this._serverSocket = new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);//创建侦听传入连接的套接字
            this._serverSocket.Bind(localEndPoint);//绑定服务器地址
            this._serverSocket.Listen(100);//使用100个连接的侦听积压启动服务器
            StartAccept(null);//监听socket 接受的 连接
            Console.WriteLine("按任意键终止服务器进程....");
            Console.ReadKey();
        }
        /// <summary>
        /// 开始接受来自客户端的连接请求的操作
        /// </summary>
        /// <param name="acceptEventArg">在服务器的侦听套接字上发出接受操作时要使用的上下文对象</param>
        private void StartAccept(SocketAsyncEventArgs acceptEventArg)
        {
            if (acceptEventArg == null)
            {
                acceptEventArg = new SocketAsyncEventArgs();
                acceptEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(AcceptEventArg_Completed);
            }
            else
            {
                acceptEventArg.AcceptSocket = null;//必须清除套接字，因为正在重用上下文对象
            }
            this._maxNumberAcceptedClients.WaitOne();
            bool willRaiseEvent = this._serverSocket.AcceptAsync(acceptEventArg);
            //异步连接失败 重新 拿个服务器监听 回复消息 
            if (!willRaiseEvent)
            {
                ProcessAccept(acceptEventArg);
            }
        }
        /// <summary>
        /// 此方法是与Socket.acceptsync操作关联的回调方法，并在接受操作完成时调用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private  void AcceptEventArg_Completed(object sender, SocketAsyncEventArgs e)
        {
            ProcessAccept(e);
        }
        private void ProcessAccept(SocketAsyncEventArgs e)
        {
            Interlocked.Increment(ref _numConnectedSockets);
            Console.WriteLine("已接受客户端连接。有{0}个客户端连接到服务器", this._numConnectedSockets);
            SocketAsyncEventArgs readEventArgs = _readWritePool.Pop(); //获取接受的客户端连接的套接字并将其放入ReadEventArg对象用户令牌
             ((AsyncUserToken)readEventArgs.UserToken).Socket = e.AcceptSocket;
            bool willRaiseEvent = e.AcceptSocket.ReceiveAsync(readEventArgs);//一旦客户端连接，就向连接发送一个接收
            if (!willRaiseEvent)
            {
                ProcessReceive(readEventArgs);
            }
            StartAccept(e);//接受下一个连接请求
        }
        /// <summary>
        /// 每当在套接字上完成接收或发送操作时，都会调用此方法
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
        private void ProcessReceive(SocketAsyncEventArgs e)
        {
            AsyncUserToken token = (AsyncUserToken)e.UserToken;
            if (e.BytesTransferred > 0 && e.SocketError == SocketError.Success)//检查远程主机是否已关闭连接
            {
                Interlocked.Add(ref this._totalBytesRead, e.BytesTransferred);//增加服务器接收的总字节数
                Console.WriteLine("服务器总共读取了{0}字节", this._totalBytesRead);
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
        private void CloseClientSocket(SocketAsyncEventArgs e)
        {
            AsyncUserToken token = e.UserToken as AsyncUserToken;
            try
            {
                token.Socket.Shutdown(SocketShutdown.Send);//关闭与客户端关联的套接字
            }
            catch (Exception) { }//如果客户端进程已关闭，则引发
#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 ||  NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
            token.Socket.Close();
#else
            token.Socket.Dispose();
#endif
            Interlocked.Decrement(ref this._numConnectedSockets);//减少跟踪连接到服务器的客户端总数的计数器
            this._readWritePool.Push(e);//释放SocketAsyncEventArg，以便其他客户端可以重用它们
            this._maxNumberAcceptedClients.Release();
            Console.WriteLine("客户端已与服务器断开连接。有{0}个客户端连接到服务器", this._numConnectedSockets);
        }
    }
}
#endif