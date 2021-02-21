#if NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Utility.AspNet.Consul;
using Utility.Consul;

namespace Utility.AspNetCore.Consul
{
    [Produces("application/json")]
    [Route("api/Health")]
#if !NETCOREAPP2_0
    [ApiController]
#endif
    public class HealthController : ControllerBase
    {
        private readonly ConsulEntity _consulEntity;
        public HealthController(IOptions<ConsulEntity> consulOptions)
        {
            this._consulEntity = consulOptions.Value;
        }
        [HttpGet]
        public IActionResult Get() => Ok("ok");
        [HttpGet("service/{name}")]
        public  virtual async Task<List<ConsulEntity>> GetHealthService(string name) {
            return await new ConsulServiceProvider().GetHealthServicesAsync($"http://{this._consulEntity.ConsulIP}:{this._consulEntity.ConsulPort}",name);
        }
        [HttpGet("catalog/{name}")]
        public virtual async Task<List<ConsulEntity>> GetCatalogService(string name)
        {
            return await new ConsulServiceProvider().GetCatalogServicesAsync($"http://{this._consulEntity.ConsulIP}:{this._consulEntity.ConsulPort}", name);
        }
    }
}
#endif