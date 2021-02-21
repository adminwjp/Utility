using System;
using System.Collections.Generic;
using System.Text;

namespace Utility.Config
{

    public class ConsulEntity
    {
        public static readonly ConsulEntity Empty = new ConsulEntity();
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        public string Ip { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ConsulIP { get; set; }
        /// <summary>
        /// 8500
        /// </summary>
        public int ConsulPort { get; set; } = 8500;
        /// <summary>
        /// 
        /// </summary>
        public List<string> Tags { get; set; }
    }
}
