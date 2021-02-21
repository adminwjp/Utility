#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Utility.Net.Sockets
{
    /// <summary>
    ///基于 TcpListener  tcp 服务器端 
    /// </summary>
    public class TcpListenerHelper:BaseTcpService,IDisposable
    {
        private TcpListener _server;//tcp listerner 实现
        /// <summary>
        /// 读取数据事件
        /// </summary>
        public event Action<byte[], int> ReciveEvent;//  继承(事件无法直接调用) 只能 add remove

        /// <summary>
        /// 基于 TcpListener  tcp 服务器端 
        /// </summary>
        /// <param name="server">服务器地址</param>
        /// <param name="port">服务器端口</param>
        public TcpListenerHelper(string server, int port)
        {
            IPAddress localAddr = IPAddress.Parse(server);
            this._server = new TcpListener(localAddr, port);
            base.Buffer = new byte[256];
            this._server.Server.SendTimeout = 500;
            this._server.Server.ReceiveTimeout = 500;
        }

        /// <summary>
        /// 基于 TcpListener  tcp 服务器端  启动
        /// </summary>
        public override void Start()
        {
            this._server.Start(20);//开始侦听客户端请求
            base.Start();
        }

        /// <summary>
        /// 基于 TcpListener  tcp 服务器端  停止
        /// </summary>
        public override void Dispose()
        {
            base.Stop();
            base.Dispose();
            this._server.Stop();
        }

        /// <summary>
        ///  基于 TcpListener  tcp 服务器 跟 客户端 通信 线程
        /// </summary>
        protected override void Loop()
        {
#if !(NET20 || NET30 || NET35 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2)
            //进入监听循环.
            while (!base.CancellationTokenSource.IsCancellationRequested)
#else
            while(base.IsCancellationRequested)
#endif
            {
                Console.WriteLine("{0} server 等待连接... ", DateTime.Now);
                //假死 
                //if (this._server.Pending())
                if(this._server.Server.Poll(100,SelectMode.SelectRead))
                {
                    TcpClient client =  this._server.AcceptTcpClient(); //执行阻塞调用以接受请求。您也可以在这里使用server.AcceptSocket()。
                    Console.WriteLine("{0} server 已连接 client!", DateTime.Now);
                    base.Stream = client.GetStream();//获取用于读写的流对象
                    int read;
                    //循环以接收客户端发送的所有数据.
                    while (base.Stream.DataAvailable && (read = base.Read(base.Buffer)) > 0)
                    {
                        this.ReciveEvent?.Invoke(base.Buffer, read);
                    }
                    client.Close();//关闭并结束连接
                }
                else
                {
                    Console.WriteLine("{0} 服务器未发现客户端连接!", DateTime.Now);
                    Thread.Sleep(100);
                }
                Thread.Sleep(20);

        }
        }
    }
}
#endif