using System;

namespace Utility
{
    /// <summary>
    /// 配置
    /// </summary>
    public struct ConfigHelper
    {
        /// <summary>
        /// key 名称
        /// </summary>
        public static string Key { get; set; }
    }

    /// <summary>
    /// 数据库配置 操作
    /// </summary>
    public class DbConfig
    {
        /// <summary>
        /// 数据库标识
        /// </summary>
        public static DbFlag Flag { get; set; } = DbFlag.SqlServer;
        /// <summary>
        /// 数据库版本
        /// </summary>
        public static decimal Version { get; set; }
    }
    /// <summary>
    /// 数据库标识
    /// </summary>
    [Flags]
    public enum DbFlag
    {
        /// <summary>SqlServer  数据库/ </summary>
        SqlServer,
        /// <summary>MySql  数据库 5.5/ </summary>
        MySql,
        /// <summary>Sqlite  数据库/ </summary>
        Sqlite,
        /// <summary>Oracle  数据库/ </summary>
        Oracle,
        /// <summary>Postgre  数据库/ </summary>
        Postgre
    }
}
