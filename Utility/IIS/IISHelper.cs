#if !(NET10 || NET11 || NET20 || NET30 || NET35 || NET40 || NET45 || NET451 || NET452 || NET46 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5)

using System.Collections.Generic;

namespace Utility.IIS
{

#if !(NET10 || NET11 || NET20 || NET30 || NET35 || NET40 || NET45 || NET451 || NET452 || NET46 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5)

    /// <summary>
    /// 基于 Microsoft.Web.Administration 实现
    /// </summary>
    public class IISAdministrationProvider : IIISProvider
    {
        /// <summary>
        ///  不存在 创建  存在 修改 
        /// </summary>
        /// <param name="iISEntity"></param>
        public virtual void Create(IISEntity iISEntity)
        {
            IISAdministrationHelper.Create(iISEntity);
        }

        /// <summary>
        /// 移除 网站  应用程序池
        /// </summary>
        /// <param name="webSiteName">网站</param>
        /// <param name="applicationName">应用程序池</param>
        public  virtual void Remove(string webSiteName, string applicationName)
        {
            IISAdministrationHelper.Remove(webSiteName, applicationName);
        }

        /// <summary>
        /// 获取所有网站
        /// </summary>
        /// <returns></returns>
        public virtual List<IISEntity> GetList()
        {
            return IISAdministrationHelper.GetList();
        }
    }

#endif


}
#endif