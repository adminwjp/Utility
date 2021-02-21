using System;
using System.Collections.Generic;
using System.Text;

namespace Utility.Application.Services.Dtos
{
     
    /// <summary>
    /// 排序
    /// </summary>
    public class OrderSortDto
    {
        /// <summary>
        /// 排序 列(属性名)
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 排序方式
        /// </summary>
        public SortFlag Sort { get; set; }
    }
    /// <summary>
    /// 排序方式
    /// </summary>
    public enum SortFlag
    {
        /// <summary>
        /// 升序
        /// </summary>
        Asc=0x0,
        /// <summary>
        /// 降序
        /// </summary>
        Desc=0x1
    }
}
