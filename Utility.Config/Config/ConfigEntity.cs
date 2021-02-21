using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility.Config
{
    public class ConfigHelper
    {
        public static string LocalIp { get; set; } ="192.168.1.3";
        /// <summary>
        /// 本机 docker 则使用 docker ip vm docker 则 使用 vm ip
        /// </summary>
        public static string DockerIp { get; set; } ="192.168.1.4";

        public static string LinuxIp { get; set; } = "192.168.1.4";

        /// <summary>
        /// 使用哪个版本 有些 版本 有问题 默认基于 内存 支持 es rabbitmq 
        /// </summary>
        public static string ZipkinAddress = $"http://{LocalIp}:9411";

        public static string SqliteConnectionString= "Data Source=ShopEmail.db;";
        /// <summary>
        /// mysql 5.5 Integrated Security=False;
        /// </summary>
        public static string MySqlConnectionString = $"Database=ShopEmail;Data Source={LocalIp};User Id=root;Password=wjp930514.;Old Guids=True;charset=utf8;";
        public static string SqlServerConnectionString = $"Database=ShopEmail;Data Source={LinuxIp};User Id=root;Password=wjp930514.;Old Guids=True;charset=utf8;";

        public static string OracleConnectionString = $"DATA SOURCE={LinuxIp}:1522/orcl;USER ID=ShopEmail;PASSWORD=123456;";
        public static string PostgreConnectionString = $"User ID=postgres;Password=wjp930514.;Host={LocalIp};Port=5432;Database=ShopEmail;Pooling=true;";
        public static string ElasticsearchConnectionString = $"http://{LocalIp}:9200/";

        /// <summary>
        /// 单机 集群    使用 负载均衡器 实现 
        /// 但 编程 只支持一个 地址 即 集群 主节点地址
        /// </summary>
        public static string ConsulAddress = $"http://{LocalIp}:8500,http://{LocalIp}:8501,http://{LocalIp}:8502";


        /// <summary>
        /// 集群地址 默认单机 集群  win7 需要 6台 设置 密码 最好 密码一致 需要 ruby 环境
        ///  集群 需要 至少 6台 机器
        /// 192.168.1.3:6379,password=wjp930514.,connectTimeout=1000,connectRetry=1,syncTimeout=10000;192.168.1.3:6380,password=wjp930514.,connectTimeout=1000,connectRetry=1,syncTimeout=10000;
        /// 192.168.1.3:6381,password=wjp930514.,connectTimeout=1000,connectRetry=1,syncTimeout=10000;192.168.1.3:6382,password=wjp930514.,connectTimeout=1000,connectRetry=1,syncTimeout=10000;
        /// 92.168.1.3:6383,password=wjp930514.,connectTimeout=1000,connectRetry=1,syncTimeout=10000;192.168.1.3:6384,password=wjp930514.,connectTimeout=1000,connectRetry=1,syncTimeout=10000;
        /// </summary>
        public static string RedisConnectionString  = $"{LocalIp}:6379,password=wjp930514.,connectTimeout=1000,connectRetry=1,syncTimeout=10000";

        public static string Name { get; set; } = "Shop.Email.Api";
        public static string RegisterIp { get; set; } = LocalIp;
        public static int RegisterPort { get; set; } = 6000;
        /// <summary>
        /// 集群 使用 其他负载均衡器 实现
        /// </summary>
        public static string EurekaAddress { get; set; } = $"http://{LocalIp}:4000/eureka/,http://{LocalIp}:4001/eureka/,http://{LocalIp}:4002/eureka/";

        public static string[] ConsulTages =new string[] { "Shop.Email", "Shop.Email Api Service" };

        public static ConfigEntity Init()
        {
            ConfigEntity config = new ConfigEntity();

            //eureka
            config.spring.application.name = Name;
            config.spring.cloud.config.uri = $"http://{RegisterIp}:{RegisterPort}";

            config.eureka.client.serviceUrl = EurekaAddress;
            config.eureka.instance.hostname = RegisterIp;
            config.eureka.instance.port = RegisterPort;

            //consul
            config.Consul.Ip = RegisterIp;
            config.Consul.Port = RegisterPort;
            config.Consul.ConsulIP = LocalIp;
            config.Consul.Tags = ConsulTages.ToList();
            config.Consul.Name = Name;

            //zipkin
            config.Zipkin.Name = Name;
            config.Zipkin.Address = ZipkinAddress;


            //connectionString
            config.ConnectionStrings.SqliteConnectionString = SqliteConnectionString;
            config.ConnectionStrings.MySqlConnectionString = MySqlConnectionString;
            config.ConnectionStrings.SqlServerConnectionString = SqlServerConnectionString;
            config.ConnectionStrings.OracleConnectionString = OracleConnectionString;
            config.ConnectionStrings.PostgreConnectionString = PostgreConnectionString;
            config.ConnectionStrings.ElasticsearchConnectionString = ElasticsearchConnectionString;
            config.ConnectionStrings.RedisConnectionString = RedisConnectionString;

            //email
            config.Emails.Add(new EmailEntity() { Email= "973513569@qq.com", Code= "awalxnuvfcogbbjj" });

            return config;
        }
    }

    //###### asp.net core 配置文件信息 #####
    /// <summary>
    /// 配置实体 配置信息经常变动 麻烦 使用动态配置信息
    /// 主要 ip 频繁 更新 简单项目配置文件 硬代码 就可以了 
    ///一旦多了 不好维护了 使用 动态配置 支持动态则使用动态配置
    ///json 转实体
    ///https://www.sojson.com/json2entity.html
    /// </summary>
    public class ConfigEntity
    {
        /// <summary>
        /// asp.net core 自带 日志 配置 使用其他日志框架则不生效
        /// </summary>
        public LoggingEntity Logging { get; set; } = LoggingEntity.Empty;

        public bool EnableEureka { get; set; }
        public EurekaSpringEntity spring { get; set; } = EurekaSpringEntity.Empty;
        public EurekaEntity eureka { get; set; } = EurekaEntity.Empty;

        public bool EnableConsul { get; set; }
        public ConsulEntity Consul { get; set; } = ConsulEntity.Empty;

        public bool EnableZipkin { get; set; }
        public ZipkinEntity Zipkin { get; set; } = ZipkinEntity.Empty;

        public ConnectionStringEntity ConnectionStrings { get; set; } = ConnectionStringEntity.Empty;

        /// <summary>
        /// 
        /// </summary>
        public List<EmailEntity> Emails { get; set; } = new List<EmailEntity>();

        /// <summary>
        /// urls server.urls
        /// </summary>
        public string urls { get; set; }

        public string AllowedHosts { get; set; } = "*";
    }

    public class EmailEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Code { get; set; }
    }



}
