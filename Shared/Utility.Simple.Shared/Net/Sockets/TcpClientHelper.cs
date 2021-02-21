#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Utility.Net.Sockets
{
    /// <summary>
    ///基于 TcpClient  tcp 客户端 
    /// </summary>
    public class TcpClientHelper:BaseTcpService
    {
        private TcpClient _client;//tcp TcpClient  实现
        /// <summary>
        /// 读取数据事件
        /// </summary>
        public event Action<byte[], int> ReciveEvent;//  继承(事件无法直接调用) 只能 add remove

        /// <summary>
        /// 基于 TcpClient  tcp 客户端 
        /// </summary>
        /// <param name="client">客户端地址</param>
        /// <param name="port">客户端端口</param>
        /// <param name="remote">服务器端地址</param>
        /// <param name="remotePort">服务器端端口</param>
        public TcpClientHelper(string client, int port,string remote,int remotePort)
        {
            //this._client = new TcpClient(remote, remotePort);//连接 服务器
            this._client = new TcpClient();
            this._client.Client.Bind(new IPEndPoint(IPAddress.Parse(client), port));
            this._client.Connect(remote, remotePort);//连接服务器
            //base.Stream = this._client.GetStream();//获取用于读写的客户端流
            base.Stream = new NetworkStream(this._client.Client);
            base.Buffer = new byte[256];
            this._client.SendTimeout = 500;
            this._client.ReceiveTimeout = 500;
        }

        /// <summary>
        /// 客户端通信线程
        /// </summary>
        protected override void Loop()
        {
            int read = 0;
#if !(NET20 || NET30 || NET35)
            while (!base.CancellationTokenSource.IsCancellationRequested)
#else
            while(!base.IsCancellationRequested)
#endif
            {
                if (base.Stream.DataAvailable&&(read = base.Read(base.Buffer))>0)
                {
                    this.ReciveEvent?.Invoke(base.Buffer, read);
                }
                Thread.Sleep(20);
            }
        }
        /// <summary>
        /// 客户端是否 连接上服务器端
        /// </summary>
        /// <returns></returns>
        public bool Create()
        {
            return this._client.Connected;
        }

        /// <summary>
        /// 客户端 通信线程 停止
        /// </summary>
        public override void Stop()
        {
            base.Stop();
        }

        /// <summary>
        /// 客户端 客户端停止
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            //关闭所有内容.
            base.Stream?.Close();
            this._client?.Close();
        }
    }
}
#endif