#if !(NET10 || NET11 || NET20 || NET30 || NET35 || NET40 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 )
using Consul;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Utility.Consul;

namespace Utility.Consul
{
    public class ConsulServiceProvider
    {
        /// <summary>
        /// 发现服务
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public virtual async Task<List<ConsulEntity>> GetHealthServicesAsync(string url,string servicerName)
        {
            var consuleClient = new ConsulClient(consulConfig =>
            {
                consulConfig.Address = new Uri(url);
            }); 
             var queryResult = await consuleClient.Health.Service(servicerName);
            var result = new List<ConsulEntity>();
            foreach (var serviceEntry in queryResult.Response)
            {
                result.Add(new ConsulEntity() { 
                    Id= serviceEntry.Service.ID,
                    IP= serviceEntry.Service.Address,
                    Port= serviceEntry.Service.Port,
                    Name= serviceEntry.Service.Service,
                    ConsulIP=serviceEntry.Node.Address
                });
            }
            return result;
        }
        /// <summary>
        /// 发现服务
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public virtual async Task<List<ConsulEntity>> GetAgentServicesAsync(string url)
        {
            var consuleClient = new ConsulClient(consulConfig =>
            {
                consulConfig.Address = new Uri(url);
            });
            var queryResult = await consuleClient.Agent.Services();
            var result = new List<ConsulEntity>();
            foreach (var serviceEntry in queryResult.Response)
            {
                result.Add(new ConsulEntity()
                {
                    Id = serviceEntry.Value.ID,
                    IP = serviceEntry.Value.Address,
                    Port = serviceEntry.Value.Port,
                    Name = serviceEntry.Value.Service,
                    ConsulIP = serviceEntry.Value.Address
                });
            }
            return result;
        }
        /// <summary>
        /// 发现服务
        /// </summary>
        /// <param name="url"></param>
        /// <param name="servicerName"></param>
        /// <returns></returns>
        public virtual async Task<List<ConsulEntity>> GetCatalogServicesAsync(string url,string servicerName)
        {
            var consuleClient = new ConsulClient(consulConfig =>
            {
                consulConfig.Address = new Uri(url);
            });
            var queryResult = await consuleClient.Catalog.Service(servicerName);
            var result = new List<ConsulEntity>();
            foreach (var catalog in queryResult.Response)
            {
                result.Add(new ConsulEntity()
                {
                    Id = catalog.ServiceID,
                    IP = catalog.ServiceAddress,
                    Port = catalog.ServicePort,
                    Name = catalog.ServiceName,
                    ConsulIP = catalog.Node
                });
            }
            return result;
        }
    }
}
#endif