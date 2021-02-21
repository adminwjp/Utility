using System;
using System.Collections.Generic;
using System.Text;

namespace Utility.Domain.Entities
{
    /// <summary>
    /// 更新时间 接口
    /// </summary>
    public interface IHasModificationTime
    {
        /// <summary>更新时间</summary>
        DateTime? LastModificationTime { get; set; }
    }
}
