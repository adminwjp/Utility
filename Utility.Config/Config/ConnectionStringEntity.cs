using System;
using System.Collections.Generic;
using System.Text;

namespace Utility.Config
{
    public class ConnectionStringEntity
    {
        public static readonly ConnectionStringEntity Empty = new ConnectionStringEntity();
        /// <summary>
        /// 
        /// </summary>
        public string SqliteConnectionString { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string MySqlConnectionString { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SqlServerConnectionString { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string OracleConnectionString { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PostgreConnectionString { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ElasticsearchConnectionString { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string RedisConnectionString { get; set; }
    }
}
