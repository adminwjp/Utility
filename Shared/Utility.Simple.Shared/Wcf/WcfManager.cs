#if true
//#if  NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NET481 || NET482

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.ServiceModel.Channels;
using System.Runtime.Serialization;
using System.ServiceModel.Description;
using System.ServiceModel.Activation;

namespace Utility.Wcf
{
    /// <summary>
    /// 
    /// </summary>
    public class WcfManager
    {
        /// <summary>
        /// 
        /// </summary>
        public const string NetPipe="net.pipe://localhost";
        /// <summary>
        /// 
        /// </summary>
        public static readonly Binding NetPipeBind = new NetNamedPipeBinding();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static  Binding CreateNetPipe()
        {
            Binding bind = new NetNamedPipeBinding();
            return bind;
        }

        /// <summary>
        /// 
        /// </summary>
        public const string Name="wcf";
        /// <summary>
        /// 创建服务端
        /// <para>net.pipe://localhost/wcf</para>
        /// </summary>
        /// <param name="urls"></param>
        /// <returns></returns>
        public static ServiceHost GetServerHostService<T>(Uri[] urls)where T:class
        {
            return new ServiceHost(typeof(T), urls);
        }
        /// <summary>
        /// 创建 代理  客户端
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="F"></typeparam>
        /// <returns></returns>
        public static T GetProxyClient<T,F>()where T: ClientBase<T>,F,new() where F:class
        {
            return new T();
        }
    }
}
#endif