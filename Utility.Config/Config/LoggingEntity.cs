using System;
using System.Collections.Generic;
using System.Text;

namespace Utility.Config
{
    /// <summary>
    /// asp.net core 自带 日志 配置 使用其他日志框架则不生效
    /// </summary>
    public class LoggingEntity
    {
        public static readonly LoggingEntity Empty = new LoggingEntity();

        /// <summary>
        /// 日志等级 输出配置
        /// </summary>
        public LogLevelEntity LogLevel { get; set; } = LogLevelEntity.Empty;
    }

    /// <summary>
    /// asp.net core 自带 日志 配置 使用其他日志框架则不生效
    /// </summary>
    public class LogLevelEntity
    {
        public IDictionary<string, string> Logging { get; set; }

        public static readonly LogLevelEntity Empty = new LogLevelEntity();

        public LogLevelEntity()
        {
            Logging = new Dictionary<string, string>()
            {
                ["Default"] = "Information",
                ["Microsoft"] = "Warning",
                ["Microsoft.Hosting.Lifetime"] = "Information"
            };
        }
    }
}
