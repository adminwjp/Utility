#if NET20 ||NET30 || NET35 || NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NET481 || NET482
using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Remoting.Channels.Tcp;

namespace Utility.Remote
{
    /// <summary>
    /// 
    /// </summary>
    public class RemoteManager
    {
        /// <summary> 注册通道客户端</summary>
        /// <param name="url">tcp://localhost:20001/test</param>
        public static void RegisterActivatedClientTcp<T>(string url)where T:MarshalByRefObject
        {
            RemotingConfiguration.RegisterActivatedClientType(typeof(T), url);
        }

        /// <summary>注册通道服务端 </summary>
        /// <param name="port">20001</param>
        /// <param name="name">test</param>
        public static void RegisterActivatedServiceTcp<T>(int port, string name)where T:MarshalByRefObject
        {
            AddServiceTcpChannel(port);
            RemotingConfiguration.ApplicationName = name;
            RemotingConfiguration.RegisterActivatedServiceType(typeof(T));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        public static void RegisterActivatedServiceTcp<T>(string name)where T:MarshalByRefObject
        {
            RemotingConfiguration.ApplicationName = name;
            RemotingConfiguration.RegisterActivatedServiceType(typeof(T));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="port"></param>
        public static void AddServiceTcpChannel(int port)
        {
            //BinaryServerFormatterSinkProvider serverProvider = new BinaryServerFormatterSinkProvider();
            //BinaryClientFormatterSinkProvider clientProvider = new BinaryClientFormatterSinkProvider();
            //serverProvider.TypeFilterLevel = TypeFilterLevel.Full;
            //IDictionary props = new Hashtable();
            //props["port"] = 9999;
            //TcpChannel channels = new TcpChannel(props, clientProvider, serverProvider);
            TcpServerChannel channels = new TcpServerChannel(port);
            ChannelServices.RegisterChannel(channels, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="port"></param>
        public static void AddServiceHttpChannel(int port)
        {
            HttpChannel channels = new HttpChannel(port);
            ChannelServices.RegisterChannel(channels, false);
        }

        /// <summary> 注册通道客户端 </summary>
        /// <param name="url">tcp://localhost:20001/test</param>
        public static T RegisterClientTcp<T>(string url)where T:MarshalByRefObject
        {
            return (T)Activator.GetObject(typeof(T), url);
        }

        /// <summary>注册通道客户端</summary>
        /// <param name="url">tcp://localhost:20001/test</param>
        /// <param name="param">T 有自定义构造函数 参数</param>
        public static T RegisterClientTcpByHasConstractor<T>(string url,object[] param=null) where T : MarshalByRefObject
        {
            T obj = (T)Activator.CreateInstance(typeof(T), param, new object[] { new System.Runtime.Remoting.Activation.UrlAttribute(url) });
            return obj;
        }

        /// <summary>注册通道服务端 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="port">20001</param>
        /// <param name="name">test</param>
        /// <param name="wellKnownObjectMode">WellKnownObjectMode.Singleton</param>
        public static void RegisterServiceTcp<T>(int port,string name, WellKnownObjectMode wellKnownObjectMode= WellKnownObjectMode.Singleton) where T : MarshalByRefObject
        {
            AddServiceTcpChannel(port);
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(T), name, wellKnownObjectMode);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="port"></param>
        /// <param name="name"></param>
        /// <param name="wellKnownObjectMode"></param>
        public static void RegisterServiceHttp<T>(int port, string name, WellKnownObjectMode wellKnownObjectMode = WellKnownObjectMode.Singleton) where T : MarshalByRefObject
        {
            AddServiceHttpChannel(port);
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(T), name, wellKnownObjectMode);
        }

        /*
         <system.runtime.remoting>
            <application name="Remote.Server">
              <service>
                <wellknown type="Remote.RemoteObject,Remote" objectUri="test"
                    mode="Singleton" />
              </service>
              <channels>
                <channel ref="tcp" port="20001"/>
                [
                    <serverProviders> 
                        <provider ref="wsdl" />
                        <formatter ref="soap" typeFilterLevel="Full" />
                        <formatter ref="binary" typeFilterLevel="Full" />
                    </serverProviders>
                ]
              </channels>
            </application>
          </system.runtime.remoting>
             */
        /// <summary>
        /// 注册通道服务端
        /// </summary>
        /// <param name="config">Remote.Server.exe.config</param>
        public static void RegisterServiceTcpConfig(string config) 
        {
            RemotingConfiguration.Configure(config, false);
        }

        /// <summary>
        /// 注册通道服务端
        /// </summary>
        /// <param name="config">Remote.Server.exe.config</param>
        public static void RegisterServiceHttpConfig(string config)
        {
            RemotingConfiguration.Configure(config, false);
        }
    }
}
#endif