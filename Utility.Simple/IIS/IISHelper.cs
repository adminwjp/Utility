using System;
using System.Collections.Generic;
#if (NET10 || NET11 || NET20 || NET30 || NET35 || NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48)
using System.Diagnostics;
using System.DirectoryServices;
using Microsoft.Win32;
#endif
using Utility.Regexs;

namespace Utility.IIS
{
    /// <summary>
    /// iis 操作 接口
    /// </summary>
    public interface  IIISProvider
    {

        /// <summary>
        ///  不存在 创建  存在 修改 
        /// </summary>
        /// <param name="iISEntity"></param>
         void Create(IISEntity iISEntity);

        /// <summary>
        /// 移除 网站  应用程序池
        /// </summary>
        /// <param name="webSiteName">网站</param>
        /// <param name="applicationName">应用程序池</param>
         void Remove(string webSiteName, string applicationName);

        /// <summary>
        /// 获取所有网站
        /// </summary>
        /// <returns></returns>
         List<IISEntity> GetList();
    }
#if (NET10 || NET11 || NET20 || NET30 || NET35 || NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48)
    /// <summary>
    /// System.DirectoryServices 实现 iis
    /// </summary>
    public class IISDirectoryServicesProvider: IIISProvider
    {
        /// <summary>
        ///  不存在 创建  存在 修改 
        /// </summary>
        /// <param name="iISEntity"></param>
        public virtual void Create(IISEntity iISEntity)
        {
            IISHelper.Create(iISEntity);
        }

        /// <summary>
        /// 移除 网站  应用程序池
        /// </summary>
        /// <param name="webSiteName">网站</param>
        /// <param name="applicationName">应用程序池</param>
        public virtual void Remove(string webSiteName, string applicationName)
        {
            IISHelper.Remove(webSiteName,applicationName);
        }

        /// <summary>
        /// 获取所有网站
        /// </summary>
        /// <returns></returns>
        public virtual List<IISEntity> GetList()
        {
           return IISHelper.GetList();
        }
    }

    /// <summary>
    /// System.DirectoryServices 实现 iis
    /// </summary>
    //参考：https://www.cnblogs.com/chenkai/archive/2010/07/26/1785074.html
    //https://www.cnblogs.com/wujy/archive/2013/02/28/2937667.html
    //https://docs.microsoft.com/en-us/previous-versions/iis/6.0-sdk/ms524505(v=vs.90)?redirectedfrom=MSDN

    public class IISHelper
    {
#if (NET10 || NET11 || NET20 || NET30 || NET35 || NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48)
        /// <summary>
        /// 获取 iis 版本
        /// </summary>
        /// <param name="domainName"></param>
        /// <returns></returns>
        public static string GetVersion(string domainName = "localhost")
        {
            //ISS版本的获取 
            string version = null;
            //1.第一种方法是通过遍历DirectoryEntry实体目录
            using (DirectoryEntry directoryEntry = new DirectoryEntry($"IIS://{domainName}/W3SVC/INFO"))
            {

                if (directoryEntry.Properties.Contains("MajorIISVersionNumber"))
                {
                    version = directoryEntry.Properties["MajorIISVersionNumber"].Value?.ToString();// 7
                }
                if (version == null)
                {
                    if (directoryEntry.Properties.Contains("MinorIIsVersionNumber"))
                    {
                        version = version ?? directoryEntry.Properties["MinorIIsVersionNumber"].Value?.ToString();//5
                    }
                }
            }
            if (version == null && (domainName == "localhost" || domainName == "127.0.0.1"))
            {
                try
                {
                    //2.第二种方式是通过获取注册表的ISS修改版本值[经测试这种方式获取版本不稳定]
                    //RegistryKey表示 Windows 注册表中的项级节点.此类是注册表封装
                    using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("software\\microsoft\\inetstp"))
                    {
                        if (registryKey != null)
                        {
                            version = registryKey.GetValue("majorversion", "-1").ToString();
                        }
                    }
                }
                catch { }
            }
            return version;
        }


        /// <summary>
        /// 创建 应用程序池 网站 不存在 创建  存在 更新
        ///重启 服务 不要 重启 iis
        /// </summary>
        /// <param name="iISEntity">网站信息</param>
        /// <param name="hostName">localhost</param>
        /// <returns></returns>
        public static int Create(IISEntity iISEntity, string hostName = "localhost")
        {
            using (DirectoryEntry apppools = new DirectoryEntry("IIS://" + hostName + "/W3SVC/AppPools"))
            {
                DirectoryEntry applicationPool = null;
                foreach (DirectoryEntry getdir in apppools.Children)
                {
                    if (getdir.Name.Equals(iISEntity.ApplicationName))
                    {
                        applicationPool = getdir;
                        break;
                    }
                }
                if (applicationPool == null)
                {
                    //创建应用程序池
                    applicationPool = apppools.Children.Add(iISEntity.ApplicationName, "IIsApplicationPool");
                    //设置属性 访问用户名和密码 一般采取默认方式
                    // applicationPool.Properties["WAMUserName"][0] = Username;
                    // applicationPool.Properties["WAMUserPass"][0] = Password;
                    applicationPool.Properties["AppPoolIdentityType"][0] = "4"; //4
                    applicationPool.Properties["ManagedPipelineMode"][0] = "0"; //0:集成模式 1:经典模式
                    applicationPool.Properties["ManagedRuntimeVersion"][0] = iISEntity.Version;
                    applicationPool.CommitChanges();
                }
                else
                {
                    //修改 
                    if (applicationPool.Properties["ManagedRuntimeVersion"][0].ToString() != iISEntity.Version)
                    {
                        applicationPool.Properties["ManagedRuntimeVersion"][0] = iISEntity.Version;
                        applicationPool.CommitChanges();
                    }
                }
            }

            using (DirectoryEntry root = new DirectoryEntry("IIS://" + hostName + "/W3SVC"))
            {
                // 为新WEB站点查找一个未使用的ID
                int siteID = 1;
                DirectoryEntry site = null;
                foreach (DirectoryEntry e in root.Children)
                {

                    if (e.SchemaClassName == "IIsWebServer")
                    {
                        //根据网站名称匹配 网站
                        if (e.Properties["ServerComment"].Value.ToString() == iISEntity.Name)
                        {
                            site = e;
                            break;
                        }
                        int ID = Convert.ToInt32(e.Name);
                        if (ID >= siteID) { siteID = ID + 1; }
                    }
                }
                string port = $":{iISEntity.Port}:";
                if (site == null)
                {
                    // 创建WEB站点
                    //2种写法都可以  不知道是否支持
                    //写法1
                    site = (DirectoryEntry)root.Invoke("Create", "IIsWebServer", siteID);
                    //site.Properties["ServerComment"][0]
                    site.Invoke("Put", "ServerComment", iISEntity.Name);
                    site.Invoke("Put", "KeyType", "IIsWebServer");
                    site.Invoke("Put", "ServerBindings", port);
                    site.Invoke("Put", "ServerState", 2);
                    site.Invoke("Put", "FrontPageWeb", 1);
                    site.Invoke("Put", "DefaultDoc", "Default.html");
                    // site.Invoke("Put", "SecureBindings", ":443:");
                    site.Invoke("Put", "ServerAutoStart", 1);
                    site.Invoke("Put", "ServerSize", 1);
                    site.Invoke("SetInfo");

                    //写法2
                    // 创建应用程序虚拟目录
                    DirectoryEntry siteVDir = site.Children.Add("Root", "IISWebVirtualDir");
                    //DirectoryEntry siteVDir = new DirectoryEntry("IIS://" + hostName + "/W3SVC/root");
                    //siteVDir.Invoke("AppCreate", true);
                    //siteVDir.CommitChanges();
                    siteVDir.Properties["AnonymousPasswordSync"][0] = true;
                    //设置的端口绑定数据
                    //siteVDir.Properties["ServerBindings"].Value =port;
                    //设置起始默认页:
                    siteVDir.Properties["EnableDefaultDoc"][0] = true;
                    siteVDir.Properties["DefaultDoc"][0] = "Default.aspx,Index.Html,index.asp";

                    siteVDir.Properties["AppIsolated"][0] = 2;
                    siteVDir.Properties["Path"][0] = iISEntity.PhysicalPath;
                    siteVDir.Properties["AccessFlags"][0] = 513;
                    siteVDir.Properties["FrontPageWeb"][0] = 1;
                    siteVDir.Properties["AppRoot"][0] = "LM/W3SVC/" + siteID + "/Root";
                    siteVDir.Properties["AppFriendlyName"][0] = "Root";

                    siteVDir.Properties["AppPoolId"][0] = iISEntity.ApplicationName;//绑定 应用程序池
                    siteVDir.CommitChanges();
                    site.CommitChanges();
                }
                else
                {
                    //修改网站
                    //设置应用程序程序池 先获得应用程序 在设定应用程序程序池
                    //第一次测试根目录
                    foreach (DirectoryEntry getchild in site.Children)
                    {
                        if (getchild.SchemaClassName.Equals("IIsWebVirtualDir") && getchild.Name.ToLower() == "root")
                        {
                            //找到指定的虚拟目录.
                            //应用程序池 改变则更新
                            if (getchild.Properties["AppPoolId"].Value.ToString() != iISEntity.ApplicationName)
                            {
                                getchild.Properties["AppPoolId"].Value = iISEntity.ApplicationName;
                                getchild.CommitChanges();
                                break;
                            }
                        }
                    }
                    if (site.Properties["ServerBindings"][0].ToString() != port)
                    {
                        site.Properties["ServerBindings"][0] = port;//修改端口 
                        site.CommitChanges();
                    }
                }
                //网上 版本  这玩意 运行不了直接 iis 重启
                //一般修改目录属性后都选通过CommitChanges()方法提交保存, 但有时你会发现我明明修改属性 却没有保存生效. 这是因为ISS中部分属性设置需要重新启动ISS服务才能生效.这个时候我们需要对ISS服务进行控制

                //using (ServiceController sc = new ServiceController("iisadmin"))
                //{
                //    if (sc.Status == ServiceControllerStatus.Running)
                //    {
                //        sc.Stop();
                //        //System.InvalidOperationException : 无法启动计算机“.”上的服务 iisadmin
                //        sc.Start();
                //    }
                //    else
                //    {
                //        Console.WriteLine($"iis 未停止,不可操作,当前状态为:{sc.Status}");
                //    }
                //}
                return siteID;
            }
        }

        /// <summary>
        /// 移除 网站  应用程序池
        /// </summary>
        /// <param name="webSiteName">网站</param>
        /// <param name="applicationName">应用程序池</param>
        /// <param name="hostName">localhost</param>
        public static void Remove(string webSiteName, string applicationName, string hostName = "localhost")
        {
            using (DirectoryEntry root = new DirectoryEntry("IIS://" + hostName + "/W3SVC"))
            {
                foreach (DirectoryEntry e in root.Children)
                {
                    if (e.SchemaClassName == "IIsWebServer")
                    {
                        //根据网站名称匹配 网站
                        if (e.Properties["ServerComment"].Value.ToString() == webSiteName)
                        {
                            e.DeleteTree();
                            //e.CommitChanges();//error
                            root.CommitChanges();
                            break;
                        }
                    }
                }
            }

            using (DirectoryEntry apppools = new DirectoryEntry("IIS://" + hostName + "/W3SVC/AppPools"))
            {
                foreach (DirectoryEntry getdir in apppools.Children)
                {
                    if (getdir.Name.Equals(applicationName))
                    {
                        getdir.DeleteTree();
                        //getdir.CommitChanges();//error 没测 应该同理
                        apppools.CommitChanges();
                        break;
                    }
                }
            }


        }


        /// <summary>
        /// 获取站点名
        /// </summary>
        /// <param name="hostName">localhost</param>

        public static List<IISEntity> GetList(string hostName = "localhost")
        {
            List<IISEntity> iisList = new List<IISEntity>();
            string entPath = String.Format("IIS://{0}/w3svc", hostName);
            DirectoryEntry ent = new DirectoryEntry(entPath);
            foreach (DirectoryEntry child in ent.Children)
            {
                if (child.SchemaClassName.Equals("IIsWebServer", StringComparison.OrdinalIgnoreCase))
                {
                    IISEntity iISEntity = new IISEntity();
                    iisList.Add(iISEntity);
                    iISEntity.Name = child.Properties["ServerComment"].Value.ToString();
                    //获取应用程序池 名称
                    foreach (DirectoryEntry childEntry in child.Children)
                    {
                        if (childEntry.SchemaClassName == "IIsWebVirtualDir" && childEntry.Name.ToLower() == "root")
                        {
                            if (childEntry.Properties["Path"].Value != null)
                            {
                                iISEntity.PhysicalPath = childEntry.Properties["Path"].Value.ToString();
                            }
                            iISEntity.ApplicationName = childEntry.Properties["AppPoolId"].Value.ToString();//应用程序池
                        }

                    }
                    if (child.Properties["ServerBindings"].Value != null)
                    {
                        object objectArr = child.Properties["ServerBindings"].Value;
                        string serverBindingStr;
                        if (objectArr is object[])//如果有多个绑定站点时
                        {
                            object[] objectToArr = (object[])objectArr;
                            serverBindingStr = objectToArr[0].ToString();

                        }
                        else//只有一个绑定站点
                        {
                            serverBindingStr = child.Properties["ServerBindings"].Value.ToString();
                        }
                        iISEntity.Port = RegexHelper.GetValue(serverBindingStr, "(\\d{2,5})");
                    }
                }
            }
            return iisList;
        }

        /// <summary>
        /// iis 重启
        /// </summary>
        public static void IIRestart()
        {
            using (var process = Process.Start("iisreset"))
            {
                process.WaitForExit(5000);
            }
        }
#endif
    }
#endif


    /// <summary>
    /// iis 实体
    /// </summary>
    public class IISEntity
    {
        /// <summary>
        /// 网站名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 应用程序池名称 默认网站名称
        /// </summary>
        public string ApplicationName { get; set; }
        /// <summary>
        ///托管管道模式  asp.net:  v4.0, asp.net core: "" 无托管模式 怎么 设置参数了
        /// </summary>
        public string Version { get; set; }="v4.0";
        /// <summary>
        /// 网站地址
        /// </summary>
        public string PhysicalPath { get; set; }
        /// <summary>
        /// 端口 可能 有 多个 默认 支持 一个
        /// </summary>
        public string Port { get; set; }
    }

}
