#if !(NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2)
using System.Net.Sockets;

namespace Utility.Net.Sockets
{
    /// <summary>
    /// socket 信息
    /// </summary>
    public class AsyncUserToken
    {

        /// <summary>
        /// socket 信息
        /// </summary>
        public AsyncUserToken()
        {
        }

        /// <summary>
        /// Socket 标识 好 区分
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Socket
        /// </summary>
        public Socket Socket { get; internal set; }
    }
}
#endif