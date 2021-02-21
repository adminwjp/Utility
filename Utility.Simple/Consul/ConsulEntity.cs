namespace  Utility.Consul
{
    /// <summary>
    /// consuel 注册 服务实体 
    /// </summary>
    public class ConsulEntity
    {
        /// <summary>
        /// 注册 服务 id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        ///注册 服务 ip
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        ///注册 服务 端口
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        ///注册 服务 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        ///注册 地址 ip
        /// </summary>
        public string ConsulIP { get; set; }
        /// <summary>
        ///注册 地址 端口
        /// </summary>
        public int ConsulPort { get; set; }
        /// <summary>
        ///注册 服务 标签
        /// </summary>
        public string[] Tags { get; set; }
    }
}