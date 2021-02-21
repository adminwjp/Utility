using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    /// <summary>
    /// 服务 配置
    /// </summary>
    public class ServiceConfig
    {
        /// <summary>
        /// 服务存储标识
        /// </summary>
        public static ServiceFlag Flag { get; set; }
    }
    /// <summary>
    /// 服务存储标识
    /// </summary>
    [Flags]
    public enum ServiceFlag
    {
        /// <summary>
        /// 无
        /// </summary>
        None=0x0,
        /// <summary>
        /// Eureka
        /// </summary>
        Eureka = 0x1,
        /// <summary>
        /// Consul
        /// </summary>
        Consul = 0x2,
        /// <summary>
        /// Zookeeper
        /// </summary>
        Zookeeper = 0x3,
        /// <summary>
        /// ServiceFabric
        /// </summary>
        ServiceFabric = 0x4,
        /// <summary>
        /// Redis
        /// </summary>
        Redis = 0x5,
        /// <summary>
        /// Rabbitmq
        /// </summary>
        Rabbitmq = 0x6,
        /// <summary>
        /// Kubernetes
        /// </summary>
        Kubernetes = 0x7
    }
}
