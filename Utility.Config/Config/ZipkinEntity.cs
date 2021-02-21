using System;
using System.Collections.Generic;
using System.Text;

namespace Utility.Config
{
    public class ZipkinEntity
    {
        public static readonly ZipkinEntity Empty = new ZipkinEntity();
        /// <summary>
        /// 
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
    }
}
