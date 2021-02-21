#if !(NET10 || NET11 || NET20 || NET30 || NET35 || NET40 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 )
using Consul;
using System;
using Utility.Consul;

namespace Utility.AspNet.Consul
{
    /// <summary>
    ///  Consul 服务帮助 类型
    /// </summary>
    public class ConsulHelper
    {
        private ConsulClient consulClient;
        private ConsulEntity consulEntity;
        private bool isStart;

        /// <summary>
        /// Consul 服务帮助 类型
        /// </summary>
        /// <param name="consulEntity">服务信息</param>
        public ConsulHelper(ConsulEntity consulEntity)
        {
            this.consulEntity = consulEntity;
            consulClient = new ConsulClient(x => x.Address = new Uri($"http://{consulEntity.ConsulIP}:{consulEntity.ConsulPort}"));//请求注册的 Consul 地址
        }

        /// <summary>
        /// 是否启动
        /// </summary>
        public bool IsStart { get => isStart;  }

        /// <summary>
        /// 启动服务
        /// </summary>
        public virtual void Start()
        {
            if (!isStart)
            {
                isStart = true;
            }
            var httpCheck = new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//服务启动多久后注册
                Interval = TimeSpan.FromSeconds(10),//健康检查时间间隔，或者称为心跳间隔
                HTTP = $"http://{consulEntity.IP}:{consulEntity.Port}/api/health",//健康检查地址
                // 超时时间
                Timeout = TimeSpan.FromSeconds(5),
                TLSSkipVerify=false
            };
            // Register service with consul
            var registration = new AgentServiceRegistration()
            {
                Checks = new[] { httpCheck },
                ID = consulEntity.Id,
                Name = consulEntity.Name,
                Address = consulEntity.IP,
                Port = consulEntity.Port,
                Tags = consulEntity.Tags//添加  格式的 tag 标签，以便 Fabio 识别
            };
            consulClient.Agent.ServiceRegister(registration).Wait();//服务启动时注册，内部实现其实就是使用 Consul API 进行注册（HttpClient发起）


        }
        /// <summary>
        /// 停止服务
        /// </summary>
        public virtual void Stop()
        {
            if(isStart)
            {
                isStart = false;
                consulClient.Agent.ServiceDeregister(consulEntity.Id).Wait();//服务停止时取消注册
            }
        }
    }
}
#endif