using System;
using System.Collections.Generic;
using System.Text;

namespace Utility.IntegrationEventLog
{
    /// <summary>
    /// 事件 状态 
    /// </summary>
    public enum EventStateEnum
    {
        /// <summary>
        /// 未 发布
        /// </summary>
        NotPublished = 0,
        /// <summary>
        /// 基于  进程 事件 
        /// </summary>
        InProgress = 1,
        /// <summary>
        /// 发布成功
        /// </summary>
        Published = 2,
        /// <summary>
        /// 发布失败
        /// </summary>
        PublishedFailed = 3
    }
}
