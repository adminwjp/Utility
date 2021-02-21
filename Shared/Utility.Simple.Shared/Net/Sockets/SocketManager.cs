#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Utility.Net.Sockets
{
    /// <summary>
    /// socket 管理类
    /// </summary>
    public class SocketManager
    {

        /// <summary>
        /// socket 连接 http
        /// </summary>
        /// <param name="server">http ip</param>
        /// <param name="port">http port</param>
        /// <returns></returns>
        private static Socket SocketHttp(string server, int port)
        {
            Socket socket = null;
            IPHostEntry  hostEntry = Dns.GetHostEntry(server); //获取主机相关信息
            //循环访问AddressList以获取支持的AddressFamily。这是为了避免当主机IP地址与地址系列不兼容时发生的异常（在IPv6的情况下是典型的）。
            foreach (IPAddress address in hostEntry.AddressList)
            {
                IPEndPoint ipe = new IPEndPoint(address, port);
                Socket tempSocket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                tempSocket.Connect(ipe);
                if (tempSocket.Connected)
                {
                    socket = tempSocket;
                    break;
                }
                else
                {
                    continue;
                }
            }
            return socket;
        }

        /// <summary>
        /// 此方法请求指定服务器的主页内容
        /// </summary>
        /// <returns></returns>
        public static string SocketHttpSendReceive(string server, int port,byte[] bytesSent,int maxbytesReceived=256)
        {
            Byte[] bytesReceived = new Byte[maxbytesReceived];
            string page = string.Empty;
            using (Socket socket = SocketHttp(server, port))// 创建与指定服务器和端口的套接字连接.
            {
                if (socket == null) return ("连接失败");
                socket.Send(bytesSent, bytesSent.Length, 0);//向服务器发送请求.
                int bytes = 0;// 接收服务器主页内容.
                do// 以下内容将被阻止，直到页面被传输.
                {
                    bytes = socket.Receive(bytesReceived, bytesReceived.Length, 0); 
                     page = Encoding.ASCII.GetString(bytesReceived, 0, bytes); 
                }
                while (bytes > 0);
            }
            Console.WriteLine(page);
            return page;
        }


        /// <summary>
        /// 发送消息 并接受回复消息
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string SocketSendReceive(string[] args)
        {
            string host;
            int port = 80;
            if (args.Length == 0)
                // 如果没有服务器名作为参数传递给此程序,使用当前主机名作为默认名称.
                host = Dns.GetHostName();
            else
                host = args[0];
            return SocketSendReceiveTest(host, port);
        }

        /// <summary>
        /// 发送消息 并接受回复消息
        /// </summary>
        /// <param name="server"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static string SocketSendReceiveTest(string server, int port)
        {
            string request = $"GET / HTTP/1.1\r\nHost: {server}\r\nConnection: Close\r\n\r\n";
            Byte[] bytesSent = Encoding.ASCII.GetBytes(request);
            return SocketHttpSendReceive(server, port, bytesSent);
        }

        /// <summary>
        /// 显示数据
        /// </summary>
        /// <param name="socket"></param>
       public static void DisplayPendingByteCount(Socket socket)
        {
            byte[] outValue = BitConverter.GetBytes(0);
            socket.IOControl(IOControlCode.DataToRead, null, outValue);// 检查已接收的字节数.
            uint bytesAvailable = BitConverter.ToUInt32(outValue, 0);
            Console.Write("服务器有{0}字节挂起. ", bytesAvailable);
            Console.WriteLine("可用属性显示 {1}.", socket.Available);
        }

        /// <summary>
        /// TcpClient 客户端
        /// </summary>
        /// <param name="client"></param>
        /// <param name="port"></param>
        /// <param name="remote"></param>
        /// <param name="remotePort"></param>
        /// <param name="message"></param>
        public static void TcpClientConnect(string client, int port, string remote, int remotePort, string message)
        {
            TcpClientHelper tcpClientService = new TcpClientHelper(client, port, remote, remotePort);
            Action<byte[], int> read = (buffer, bytes) => {
                var str = System.Text.Encoding.ASCII.GetString(buffer, 0, bytes);
                Console.WriteLine("server Received: {0}", str);
                if (tcpClientService.Create())
                {
                    Console.WriteLine("客户端连接服务器成功");
                }
                else
                {
                    Console.WriteLine("客户端连接服务器失败");
                }
                tcpClientService.Send(System.Text.Encoding.ASCII.GetBytes("client send msg"));
                Console.WriteLine("client send: client send msg ");
            };
            tcpClientService.ReciveEvent -= read;
            tcpClientService.ReciveEvent += read;
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);//将传递的消息转换为ASCII并将其存储为字节数组。
            tcpClientService.Send(data);
            Console.WriteLine("client Sent: {0}", message);
            tcpClientService.Start();
            Console.WriteLine("\n Press Enter to continue...");
            Console.Read();
            //关闭所有内容.
            tcpClientService.Dispose();
        }

        /// <summary>
        /// TcpListener 服务器端
        /// </summary>
        /// <param name="server"></param>
        /// <param name="port"></param>
        public static void TcpListenerConnect(string server, int port)
        {
            TcpListenerHelper tcpServerService = new TcpListenerHelper(server,port);
            Action<byte[], int> read = (buffer, bytes) => {
                var data = System.Text.Encoding.ASCII.GetString(buffer, 0, bytes);//将数据字节转换为ASCII字符串
                Console.WriteLine("{1} client Received: {0}", data,DateTime.Now);
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(" server send msg");
                tcpServerService.Send(msg);//发回回复
                Console.WriteLine("{1} server Sent: {0}", " server send msg", DateTime.Now);
            };
            tcpServerService.ReciveEvent -= read;
            tcpServerService.ReciveEvent += read;
            tcpServerService.Start();
            Console.WriteLine("\n按回车键继续...");
            Console.Read();
        }

    }
}
#endif