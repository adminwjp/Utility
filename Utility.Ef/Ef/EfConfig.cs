using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility.Ef
{
    /// <summary>
    /// ef 配置
    /// </summary>
    public class EfConfig
    {
        /// <summary>
        /// 是否 是 abp 框架 配置不同造成其他错误影响
        /// </summary>
        public static bool IsAbpEf { get; set; }
    }
}
