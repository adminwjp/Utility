#if !(NET10 || NET11 || NET20 || NET30 || NET35 || NET40 || NET45 || NET451 || NET452 || NET46 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5)

using Microsoft.Web.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using Utility.Regexs;

namespace Utility.IIS
{
    /// <summary>
    /// 基于 Microsoft.Web.Administration 实现
    /// </summary>
    public class IISAdministrationHelper
    {
        /// <summary>
        ///  不存在 创建  存在 修改 
        /// </summary>
        /// <param name="iISEntity"></param>
        public static void Create(IISEntity iISEntity)
        {
            using (ServerManager sm = new ServerManager())
            {
                bool channge = false;
                ApplicationPool applicationPool = sm.ApplicationPools[iISEntity.ApplicationName];
                if (applicationPool == null)
                {
                    //创建应用程序池
                    // applicationPool = sm.ApplicationPools.CreateElement("add");
                    applicationPool = sm.ApplicationPools.CreateElement();
                    applicationPool.Name = iISEntity.ApplicationName;
                    applicationPool.ManagedRuntimeVersion = iISEntity.Version;
                    //默认集成模式
                    // applicationPool.ManagedPipelineMode = ManagedPipelineMode.Classic; //托管模式Integrated为集成 Classic为经典
                    // applicationPool.ProcessModel.IdentityType = ProcessModelIdentityType.ApplicationPoolIdentity;
                    channge = true;
                    sm.ApplicationPools.Add(applicationPool);//必须要 不然 不会添加
                }
                else
                {
                    if (applicationPool.ManagedRuntimeVersion != iISEntity.Version)
                    {
                        applicationPool.ManagedRuntimeVersion = iISEntity.Version;//更新托管管道模式
                        channge = true;
                    }
                }
                if (channge)
                {
                    sm.CommitChanges();//更新 iis 信息
                    //不添加 放入 集合 报错
                    //Unable to load DLL 'api-ms-win-service-core-l1-1-1.dll' or one of its dependencies: 找不到指定的模块。 (0x8007007E)
                    System.Threading.Thread.Sleep(500);//有时 报 这个错误 调试 时 不报 正常时 可能出现
                    if (applicationPool.State == ObjectState.Started)
                    {
                        applicationPool.Stop();
                        applicationPool.Start();
                    }

                }

                Site site = sm.Sites[iISEntity.Name];//根据网站名称获取网站
                string bindingInformation = $"*:{iISEntity.Port}:";
                if (site == null)
                {
                    //创建网站
                    //site = sm.Sites.CreateElement("site");
                    site = sm.Sites.CreateElement();
                    site.Id = sm.Sites.Count + 1;
                    site.Name = iISEntity.Name;
                    site.Applications.Add("/", iISEntity.PhysicalPath);
                    site.ApplicationDefaults.ApplicationPoolName = iISEntity.ApplicationName;//绑定应用程序池
                                                                                             //绑定端口
                                                                                             //ip  默认 127.0.0.1
                    if (site.Bindings.Count == 0)
                    {
                        site.Bindings.Add(bindingInformation, "http");
                    }
                    sm.Sites.Add(site);
                    channge = true;
                }
                else
                {
                    if (site.Bindings.Count == 0)
                    {
                        site.Bindings.Add(bindingInformation, "http");
                        channge = true;
                    }
                    else if (site.Bindings[0].BindingInformation != bindingInformation)
                    {
                        site.Bindings[0].BindingInformation = bindingInformation;//更新端口
                        channge = true;
                    }
                    if (site.ApplicationDefaults.ApplicationPoolName != iISEntity.ApplicationName)
                    {
                        site.ApplicationDefaults.ApplicationPoolName = iISEntity.ApplicationName;//绑定应用程序池
                        channge = true;
                    }
                }
                if (channge)
                {
                    sm.CommitChanges();//更新 iis 信息
                    System.Threading.Thread.Sleep(500);//有时 报 这个错误 调试 时 不报 正常时 可能出现
                    if (site.State == ObjectState.Started)
                    {
                        site.Stop();
                        site.Start();
                    }

                }
            }
        }

        /// <summary>
        /// 移除 网站  应用程序池
        /// </summary>
        /// <param name="webSiteName">网站</param>
        /// <param name="applicationName">应用程序池</param>
        public static void Remove(string webSiteName, string applicationName)
        {
            using (ServerManager sm = new ServerManager())
            {
                bool channge = false;
                ApplicationPool applicationPool = sm.ApplicationPools[applicationName];
                if (applicationPool != null)
                {
                    if (applicationPool.State == ObjectState.Started)
                    {
                        applicationPool.Stop();
                    }
                    sm.ApplicationPools.Remove(applicationPool);
                    channge = true;
                }
                if (channge)
                {
                    sm.CommitChanges();//更新 iis 信息
                    channge = false;
                }

                Site site = sm.Sites[webSiteName];//根据网站名称获取网站

                if (site != null)
                {
                    if (site.State == ObjectState.Started)
                    {
                        site.Stop();
                    }
                    sm.Sites.Remove(site);
                    channge = true;
                }
                if (channge)
                {
                    sm.CommitChanges();//更新 iis 信息
                }
            }
        }

        /// <summary>
        /// 获取所有网站
        /// </summary>
        /// <returns></returns>
        public static List<IISEntity> GetList()
        {
            List<IISEntity> iISEntities = new List<IISEntity>();
            using (ServerManager sm = new ServerManager())
            {
                foreach (Site site in sm.Sites)
                {
                    IISEntity iISEntity = new IISEntity()
                    {
                        Name = site.Name,
                        ApplicationName = site.ApplicationDefaults.ApplicationPoolName,
                        Version = sm.ApplicationPools[site.ApplicationDefaults.ApplicationPoolName].ManagedRuntimeVersion
                    };
                    iISEntities.Add(iISEntity);
                    if (site.Bindings.Count > 0)
                    {
                        string port = site.Bindings[0].BindingInformation;
                        iISEntity.Port = RegexHelper.GetValue(port, "(\\d{2,5})");
                    }
                    //获取物理地址
                    Microsoft.Web.Administration.Application application = site.Applications.Where(it => it.Path == "/").Single();
                    VirtualDirectory virtualDirectory = application.VirtualDirectories.Where(it => it.Path == "/").Single();
                    string physicalPath = virtualDirectory.PhysicalPath;//物理路径
                    string temp = string.Empty;
                    if (physicalPath.IndexOf("%") > -1)
                    {
                        //https://blog.csdn.net/weixin_33704591/article/details/93697354
                        string[] paths = physicalPath.Split('%').Where(it => it != "%").ToArray();
                        for (int i = 0; i < paths.Length; i++)
                        {
                            string path = Environment.GetEnvironmentVariable(paths[i].ToLower());
                            if (!string.IsNullOrEmpty(path))
                            {
                                temp += path;
                            }
                            else
                            {
                                temp += paths[i].ToLower();
                            }
                        }
                    }
                    else
                    {
                        temp = physicalPath;
                    }
                    iISEntity.PhysicalPath = temp;

                }
            }
            return iISEntities;
        }
    }
}
#endif