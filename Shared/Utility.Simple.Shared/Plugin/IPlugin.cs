#if !( NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 ||  NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
using System.Collections;
using Utility.Response;

namespace Utility.Plugin
{
    /// <summary>
    /// 插件
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// 插件名称
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        bool Enable { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        string Description { get; set; }
        /// <summary>
        /// 分类
        /// </summary>
        string Category { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        string Url { get; set; }
        /// <summary>
        /// 键
        /// </summary>
        string Key { get; set; }
        /// <summary>
        /// 任务
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        ResponseApi Collect(Hashtable param);
    }

    /// <summary>
    /// 插件 基本实现
    /// </summary>
    public abstract class AbstractPlugin : IPlugin
    {
        /// <summary>
        /// 插件名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 分类
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 键
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 任务
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public abstract ResponseApi Collect(Hashtable param);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual string GetString()
        {
            return string.Empty;
        }

    }
}
#endif