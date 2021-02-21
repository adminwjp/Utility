//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Thrift.Protocol;
//using Thrift.Server;
//using Thrift.Transport;
//using static HelloWorldService;

//namespace Utility
//{
//    public class HelloWroldImpl : HelloWorldService.Iface
//    {
//        public string sayHello(string username)
//        {
//            return string.Format("{0:yyyy/MM/dd hh:mm:ss} hello: {1}", DateTime.Now, username);
//        }
//    }
//    /// <summary>
//    /// thrift test
//    /// https://www.cnblogs.com/amosli/p/3948342.html 
//    /// </summary>
//    public class ThriftTest
//    {
//        public static void Server(int port)
//        {
//            TProtocolFactory ProtocolFactory = new TBinaryProtocol.Factory(true, true);
//            TTransportFactory TransportFactory = new TFramedTransport.Factory();
//            TServerSocket serverTransport = new TServerSocket(port, 0, false);
//            Processor processor = new Processor(new HelloWroldImpl());
//            TMultiplexedProcessor multiplex = new TMultiplexedProcessor();
//            multiplex.RegisterProcessor("RTDListenerService", processor);
//            TThreadPoolServer server = new TThreadPoolServer(processor, serverTransport, TransportFactory, ProtocolFactory);
//        }

//        public static void Client()
//        {
//            TTransport trans = new TSocket("10.232.158.49", 8083);
//            TProtocol Protocol = new TBinaryProtocol(trans, true, true);
//            TMultiplexedProtocol multiplex = new TMultiplexedProtocol(Protocol, "RTDListenerService");
//            Iface client = new Client(multiplex);
//            client.sayHello(null);
//        }
//    }


//}
