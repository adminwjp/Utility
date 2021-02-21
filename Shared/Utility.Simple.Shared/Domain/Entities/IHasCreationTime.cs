using System;
using System.Collections.Generic;
using System.Text;

namespace Utility.Domain.Entities
{
    /// <summary>
    /// 创建时间
    /// </summary>
    public interface IHasCreationTime
    {     
        /// <summary>创建时间</summary>
         DateTime CreationTime { get; set; }
    }
}
