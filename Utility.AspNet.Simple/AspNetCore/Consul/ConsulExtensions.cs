#if NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using System;
using Utility.AspNet.Consul;
using Utility.Consul;
using Utility.Net.Sockets;

namespace Utility.AspNetCore.Consul
{
    /// <summary>
    ///   Consul 注册 服务 
    /// </summary>
    public static class ConsulExtensions
    {
        /// <summary>
        ///  注册 服务 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="serviceEntity"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseConsul(this IApplicationBuilder app,  ConsulEntity serviceEntity)
        {
            ConsulHelper consulHelper = new ConsulHelper(serviceEntity);

            StartHelper.ApplicationStarted.Register(()=> {
                //如果有多个网卡 则 可能ip无法 通信 ip 不对 最好自己配置 
                if (string.IsNullOrEmpty(serviceEntity.IP))
                {
                    serviceEntity.IP = NetworkHelper.LocalIp;
                }
              
                consulHelper.Start();
            });

            StartHelper.ApplicationStopped.Register(()=> {
                consulHelper.Stop();//服务停止时取消注册
            });
            return app;
        }
        /// <summary>
        /// 注册 服务 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseConsul(this IApplicationBuilder app, IConfiguration configuration)
        {
            if(bool.TryParse(configuration["EnableConsul"],out bool enable))
            {
                if (!enable)
                {
                    return app;
                }
            }
            //string ip = NetworkHelper.LocalIp;
            //string id = configuration["Consul:Id"];
            //int port = Convert.ToInt32(configuration["Consul:Port"]);
            //string serviceName = configuration["Consul:Name"];
            //string consulIP = configuration["Consul:ConsulIP"];
            //int consulPort = Convert.ToInt32(configuration["Consul:ConsulPort"]);
            //var tags = configuration.GetValue<string[]>("Consul:Tags");
            //ConsulEntity serviceEntity = new ConsulEntity() { Id = id, IP = ip, Port = port, Name = serviceName, ConsulIP = consulIP, ConsulPort = consulPort,Tags= tags };

            ConsulEntity serviceEntity = configuration.GetSection("Consul").Get<ConsulEntity>();
            return UseConsul(app,serviceEntity);
        }
     
    }
}
#endif