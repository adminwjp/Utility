using System;
using System.Collections.Generic;
using System.Text;

namespace Utility.Config
{
    public class EurekaCloudEntity
    {
        public static readonly EurekaCloudEntity Empty = new EurekaCloudEntity();
        /// <summary>
        /// 
        /// </summary>
        public EurekaConfigEntity config { get; set; } = EurekaConfigEntity.Empty;
    }
    public class EurekaApplicationEntity
    {
        public static readonly EurekaApplicationEntity Empty = new EurekaApplicationEntity();
        /// <summary>
        /// 
        /// </summary>
        public string name { get; set; }
    }

    public class EurekaConfigEntity
    {
        public static readonly EurekaConfigEntity Empty = new EurekaConfigEntity();
        /// <summary>
        /// 
        /// </summary>
        public string uri { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool validate_certificates { get; set; }
    }

    public class EurekaSpringEntity
    {
        public static readonly EurekaSpringEntity Empty = new EurekaSpringEntity();
        /// <summary>
        /// 
        /// </summary>
        public EurekaApplicationEntity application { get; set; } = EurekaApplicationEntity.Empty;
        /// <summary>
        /// 
        /// </summary>
        public EurekaCloudEntity cloud { get; set; } = EurekaCloudEntity.Empty;
    }

    public class EurekaClientEntity
    {
        public static readonly EurekaClientEntity Empty = new EurekaClientEntity();
        /// <summary>
        /// 
        /// </summary>
        public string serviceUrl { get; set; }
        /// <summary>
        /// 是否将自己注册到Eureka服务中,因为该应用本身就是注册中心，不需要再注册自己（集群的时候为true）
        /// </summary>
        public bool shouldFetchRegistry { get; set; } = true;
        /// <summary>
        /// 是否从Eureka中获取注册信息,因为自己为注册中心,不会在该应用中的检索服务信息
        /// </summary>
        public bool validateCertificates { get; set; } = true;
    }

    public class EurekaInstanceEntity
    {
        public static readonly EurekaInstanceEntity Empty = new EurekaInstanceEntity();
        /// <summary>
        /// 
        /// </summary>
        public string hostname { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int port { get; set; }
        /// <summary>
        /// 每隔10s发送一次心跳
        /// </summary>
        public int leaseRenewalIntervalInSeconds { get; set; } = 10;
        /// <summary>
        /// 告知服务端30秒还未收到心跳的话，就将该服务移除列表
        /// </summary>
        public int leaseExpirationDurationInSeconds { get; set; } = 30;
    }

    public class EurekaEntity
    {
        public static readonly EurekaEntity Empty = new EurekaEntity();
        /// <summary>
        /// 
        /// </summary>
        public EurekaClientEntity client { get; set; } = EurekaClientEntity.Empty;
        /// <summary>
        /// 
        /// </summary>
        public EurekaInstanceEntity instance { get; set; } = EurekaInstanceEntity.Empty;
    }
}
