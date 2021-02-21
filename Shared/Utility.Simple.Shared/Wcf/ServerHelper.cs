#if true
//#if NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 
using System.Collections.Generic;


namespace Utility.Wcf
{
    /// <summary>
    ///  wcf 服务 帮助类  
    /// </summary>
    public class ServerHelper
    {
       /// <summary>
       /// wcf 服务
       /// </summary>
        protected  readonly List<System.ServiceModel.ServiceHost> ServiceHosts = new List<System.ServiceModel.ServiceHost>();

        /// <summary>
        /// 启动服务
        /// </summary>
        public virtual  void RegisterServer()
        {
            //契约实现不能有task 不然一直加载 
            // 有参构造函数 方案
            //这种方式比较麻烦 多余的配置也要注释掉 不然改配置无效
            //< add baseAddress = "net.pipe://localhost/service" />
            //< add baseAddress = "http://localhost:8733/service" />
            // < add baseAddress = "tcp://localhost:8733/service" />
            //这种方式必须指定地址 还需配置以下
            //< serviceHostingEnvironment multipleSiteBindingsEnabled = "true" ></ serviceHostingEnvironment >
            //实现契约 必须注解     [System.ServiceModel.ServiceBehavior(InstanceContextMode = System.ServiceModel.InstanceContextMode.Single)]
            //ServiceHosts.Add(new ServiceHost(new ServiceService(ServiceManager), new System.Uri[] {
            //       new System.Uri("net.pipe://localhost/service"),
            //       new System.Uri("http://localhost:8733/service"),
            //       new System.Uri("net.tcp://localhost:8733/service")
            //   }));

            // 无参构造函数 方案 [System.ServiceModel.ServiceBehavior(InstanceContextMode = System.ServiceModel.InstanceContextMode.Single)] 报错 注释掉 上面是硬代码有参配置的
    
            foreach (var item in ServiceHosts)
            {
                item.Open();
            }
        }

        /// <summary>
        /// 关闭服务
        /// </summary>
        public  void StopServer()
        {
            foreach (var item in ServiceHosts)
            {
                item.Close();
            }
        }
    }
}
#endif