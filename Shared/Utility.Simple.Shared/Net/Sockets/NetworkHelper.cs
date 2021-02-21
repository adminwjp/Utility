#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
using System.Net;
using System.Net.Sockets;

namespace Utility.Net.Sockets
{
    /// <summary>
    ///   netowork helper
    /// </summary>
    public class NetworkHelper
    {


        /// <summary>
        /// get local ip address
        /// </summary>
        public static string LocalIp
        {
            get
            {
                // NetworkInterface.GetAllNetworkInterfaces();
                string HostName = Dns.GetHostName(); // get host name
                IPHostEntry IpEntry = Dns.GetHostEntry(HostName);
                for (int i = 0; i < IpEntry.AddressList.Length; i++)
                {
                    //从IP地址列表中筛选出IPv4类型的IP地址
                    //AddressFamily.InterNetwork表示此IP为IPv4,
                    //AddressFamily.InterNetworkV6表示此地址为IPv6类型
                    if (IpEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        return IpEntry.AddressList[i].ToString();
                    }
                }
                return string.Empty;
            }
        }


    }
}
#endif