using System;
using System.Collections.Generic;
using System.Text;

namespace Utility.Domain.Entities
{
    /// <summary>
    /// 删除时间 
    /// </summary>
    public interface IHasDeletionTime
    {
        /// <summary>软删除标识 </summary>
         bool IsDeleted { get; set; }
        /// <summary>软删除时间 </summary>
         DateTime? DeletionTime { get; set; }
    }
}
