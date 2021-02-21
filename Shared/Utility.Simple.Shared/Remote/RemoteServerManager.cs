using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Remoting.Channels.Tcp;

namespace Utility.Remote
{
    /// <summary>
    /// 
    /// </summary>
    public class RemoteServerManager
    {
        /// <summary>
        /// 
        /// </summary>
        public static void StopServer()
        {
            //获得当前已注册的通道；
            IChannel[] channels = ChannelServices.RegisteredChannels;
            foreach (IChannel channel in channels)
            {
                if (channel is TcpChannel tcpChannel)
                {
                    tcpChannel.StopListening(null); //关闭监听；
                }
                else if (channel is HttpChannel httpChannel)
                {
                    httpChannel.StopListening(null); //关闭监听；
                }
                //注销通道；
                ChannelServices.UnregisterChannel(channel);
            }
        }
    }
}
