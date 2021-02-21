using Consul;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Utility.Config
{
    /// <summary>
    /// 配置生成 默认 存储 到 consul 其他 zookeeper db redis 等自己实现 主要没有这方面生态(开源框架)
    /// </summary>
    public class TestConfig
    {
        static ConfigEntity config;
        public static void Main(string[] args)
        {
            //asp.net core 支持 启动多个端口 需要注册 多个服务 但是 配置不支持 默认 是单端口启动 注册服务 不然 不好控制
            //默认 没有启动 注册服务 日志监控 组件
            //ShopEmail
            ConfigEntity config = ConfigHelper.Init();
            //string json = System.Text.Json.JsonSerializer.Serialize(config);
            //string val = ConfigManager.GetByConsul("ShopEmail");
            // val = ConfigManager.GetByConsul("ShopGift/ShopGift");
            // ConfigManager.SetConfig("ShopEmail", config);

            //ShopGift();

            //ShopProduct();

            //ShopComment();

            ShopTask();

            using (var cts = new CancellationTokenSource())
            {
                cts.CancelAfter(1000);
                cts.Token.Register(() => {
                    Console.WriteLine(11);
                }, true);

                cts.Token.Register(() => {
                    Console.WriteLine(22);
                }, true);

                cts.Token.Register(() => {
                    Console.WriteLine(33);
                }, true);

                Task.Run(() =>
                {
                    Console.WriteLine(44);
                }, cts.Token);
                cts.Cancel();
            }


            Console.ReadKey();
        }

        static void ShopTask()
        {
            //ShopTask
            ConfigHelper.SqliteConnectionString = "Data Source=ShopTask.db;";
            ConfigHelper.MySqlConnectionString = $"Database=ShopTask;Data Source={ConfigHelper.LocalIp};User Id=root;Password=wjp930514.;Old Guids=True;charset=utf8;";
            ConfigHelper.SqlServerConnectionString = $"Database=ShopTask;Data Source={ConfigHelper.LinuxIp};User Id=root;Password=wjp930514.;Old Guids=True;charset=utf8;";
            ConfigHelper.OracleConnectionString = $"DATA SOURCE={ConfigHelper.LinuxIp}:1522/orcl;USER ID=ShopTask;PASSWORD=123456;";
            ConfigHelper.PostgreConnectionString = $"User ID=postgres;Password=wjp930514.;Host={ConfigHelper.LocalIp};Port=5432;Database=ShopTask;Pooling=true;";
            ConfigHelper.ElasticsearchConnectionString = $"http://{ConfigHelper.LocalIp}:9200/";
            ConfigHelper.ConsulAddress = $"http://{ConfigHelper.LocalIp}:8500,http://{ConfigHelper.LocalIp}:8501,http://{ConfigHelper.LocalIp}:8502";
            ConfigHelper.RedisConnectionString = $"{ConfigHelper.LocalIp}:6379,password=wjp930514.,connectTimeout=1000,connectRetry=1,syncTimeout=10000";
            ConfigHelper.Name = "Shop.Task.Api";
            ConfigHelper.RegisterIp = ConfigHelper.LocalIp;
            ConfigHelper.RegisterPort = 6002;
            ConfigHelper.EurekaAddress = $"http://{ConfigHelper.LocalIp}:4000/eureka/,http://{ConfigHelper.LocalIp}:4001/eureka/,http://{ConfigHelper.LocalIp}:4002/eureka/";
            ConfigHelper.ConsulTages = new string[] { "Shop.Task", "Shop.Task Api Service" };
            config = ConfigHelper.Init();
            ConfigManager.SetConfig("ShopTask", config);
        }


        static void ShopComment()
        {
            //ShopComment
            ConfigHelper.SqliteConnectionString = "Data Source=ShopComment.db;";
            ConfigHelper.MySqlConnectionString = $"Database=ShopComment;Data Source={ConfigHelper.LocalIp};User Id=root;Password=wjp930514.;Old Guids=True;charset=utf8;";
            ConfigHelper.SqlServerConnectionString = $"Database=ShopComment;Data Source={ConfigHelper.LinuxIp};User Id=root;Password=wjp930514.;Old Guids=True;charset=utf8;";
            ConfigHelper.OracleConnectionString = $"DATA SOURCE={ConfigHelper.LinuxIp}:1522/orcl;USER ID=ShopComment;PASSWORD=123456;";
            ConfigHelper.PostgreConnectionString = $"User ID=postgres;Password=wjp930514.;Host={ConfigHelper.LocalIp};Port=5432;Database=ShopComment;Pooling=true;";
            ConfigHelper.ElasticsearchConnectionString = $"http://{ConfigHelper.LocalIp}:9200/";
            ConfigHelper.ConsulAddress = $"http://{ConfigHelper.LocalIp}:8500,http://{ConfigHelper.LocalIp}:8501,http://{ConfigHelper.LocalIp}:8502";
            ConfigHelper.RedisConnectionString = $"{ConfigHelper.LocalIp}:6379,password=wjp930514.,connectTimeout=1000,connectRetry=1,syncTimeout=10000";
            ConfigHelper.Name = "Shop.Comment.Api";
            ConfigHelper.RegisterIp = ConfigHelper.LocalIp;
            ConfigHelper.RegisterPort = 6002;
            ConfigHelper.EurekaAddress = $"http://{ConfigHelper.LocalIp}:4000/eureka/,http://{ConfigHelper.LocalIp}:4001/eureka/,http://{ConfigHelper.LocalIp}:4002/eureka/";
            ConfigHelper.ConsulTages = new string[] { "Shop.Comment", "Shop.Comment Api Service" };
            config = ConfigHelper.Init();
            ConfigManager.SetConfig("ShopComment", config);
        }

        static void ShopProduct()
        {
            //ShopProduct
            ConfigHelper.SqliteConnectionString = "Data Source=ShopProduct.db;";
            ConfigHelper.MySqlConnectionString = $"Database=ShopProduct;Data Source={ConfigHelper.LocalIp};User Id=root;Password=wjp930514.;Old Guids=True;charset=utf8;";
            ConfigHelper.SqlServerConnectionString = $"Database=ShopProduct;Data Source={ConfigHelper.LinuxIp};User Id=root;Password=wjp930514.;Old Guids=True;charset=utf8;";
            ConfigHelper.OracleConnectionString = $"DATA SOURCE={ConfigHelper.LinuxIp}:1522/orcl;USER ID=ShopProduct;PASSWORD=123456;";
            ConfigHelper.PostgreConnectionString = $"User ID=postgres;Password=wjp930514.;Host={ConfigHelper.LocalIp};Port=5432;Database=ShopProduct;Pooling=true;";
            ConfigHelper.ElasticsearchConnectionString = $"http://{ConfigHelper.LocalIp}:9200/";
            ConfigHelper.ConsulAddress = $"http://{ConfigHelper.LocalIp}:8500,http://{ConfigHelper.LocalIp}:8501,http://{ConfigHelper.LocalIp}:8502";
            ConfigHelper.RedisConnectionString = $"{ConfigHelper.LocalIp}:6379,password=wjp930514.,connectTimeout=1000,connectRetry=1,syncTimeout=10000";
            ConfigHelper.Name = "Shop.Product.Api";
            ConfigHelper.RegisterIp = ConfigHelper.LocalIp;
            ConfigHelper.RegisterPort = 6002;
            ConfigHelper.EurekaAddress = $"http://{ConfigHelper.LocalIp}:4000/eureka/,http://{ConfigHelper.LocalIp}:4001/eureka/,http://{ConfigHelper.LocalIp}:4002/eureka/";
            ConfigHelper.ConsulTages = new string[] { "Shop.Product", "Shop.Product Api Service" };
            config = ConfigHelper.Init();
            ConfigManager.SetConfig("ShopProduct", config);
        }

        static void ShopGift()
        {
            //ShopGift
            ConfigHelper.SqliteConnectionString = "Data Source=ShopGift.db;";
            ConfigHelper.MySqlConnectionString = $"Database=ShopGift;Data Source={ConfigHelper.LocalIp};User Id=root;Password=wjp930514.;Old Guids=True;charset=utf8;";
            ConfigHelper.SqlServerConnectionString = $"Database=ShopGift;Data Source={ConfigHelper.LinuxIp};User Id=root;Password=wjp930514.;Old Guids=True;charset=utf8;";
            ConfigHelper.OracleConnectionString = $"DATA SOURCE={ConfigHelper.LinuxIp}:1522/orcl;USER ID=ShopGift;PASSWORD=123456;";
            ConfigHelper.PostgreConnectionString = $"User ID=postgres;Password=wjp930514.;Host={ConfigHelper.LocalIp};Port=5432;Database=ShopGift;Pooling=true;";
            ConfigHelper.ElasticsearchConnectionString = $"http://{ConfigHelper.LocalIp}:9200/";
            ConfigHelper.ConsulAddress = $"http://{ConfigHelper.LocalIp}:8500,http://{ConfigHelper.LocalIp}:8501,http://{ConfigHelper.LocalIp}:8502";
            ConfigHelper.RedisConnectionString = $"{ConfigHelper.LocalIp}:6379,password=wjp930514.,connectTimeout=1000,connectRetry=1,syncTimeout=10000";
            ConfigHelper.Name = "Shop.Gift.Api";
            ConfigHelper.RegisterIp = ConfigHelper.LocalIp;
            ConfigHelper.RegisterPort = 6002;
            ConfigHelper.EurekaAddress = $"http://{ConfigHelper.LocalIp}:4000/eureka/,http://{ConfigHelper.LocalIp}:4001/eureka/,http://{ConfigHelper.LocalIp}:4002/eureka/";
            ConfigHelper.ConsulTages = new string[] { "Shop.Gift", "Shop.Gift Api Service" };
            config = ConfigHelper.Init();
             ConfigManager.SetConfig("ShopGift", config);
        }
    }

   
    /**
     {
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "IdentityUrl": "http://192.168.2.110:5000",
  "spring": {
    "application": { "name": "shop.email.api" },
    "cloud": {
      "config": {
        "uri": "http://192.168.2.110:8084",
        "validate_certificates": false
      }
    }
  },
  "eureka": {
    "client": {
      "serviceUrl": "http://192.168.2.110:8079/eureka/",
      "shouldFetchRegistry": true, //是否将自己注册到Eureka服务中,因为该应用本身就是注册中心，不需要再注册自己（集群的时候为true）
      "validateCertificates": true //是否从Eureka中获取注册信息,因为自己为注册中心,不会在该应用中的检索服务信息
    },
    "instance": {
      "hostname": "shop.email.api",
      "port": 8084,
      "leaseRenewalIntervalInSeconds": 10, //每隔10s发送一次心跳
      "leaseExpirationDurationInSeconds": 30 //告知服务端30秒还未收到心跳的话，就将该服务移除列表
    }
  },
  "EnableConsul": false,
  "Consul": {
    "Name": "shop.email.api",
    "Port": 8084,
    "ConsulIP": "192.168.2.110",
    "ConsulPort": 8500,
    "Tags": [ "Shop.Email", "Shop.Email Api Service" ]
  },
  "EnableZipkin": false,
  "Zipkin": {
    "Address": "http://192.168.2.110:9411",
    "Name": "shop.email.api"
  },
  "ConnectionStrings": {
    "SqliteConnectionString": "Data Source=ShopEmail.db;",
    "MySqlConnectionString": "Database=ShopEmail;Data Source=localhost;User Id=root;Password=wjp930514.;Old Guids=True;charset=utf8;",
    "SqlServerConnectionString": "server=192.168.99.101;database=ShopEmail;user=sa;pwd=wjp930514.",
    "OracleConnectionString": "DATA SOURCE=192.168.99.101:1521/orcl;USER ID=ShopEmail;PASSWORD=123456;",
    "PostgreConnectionString": "User ID=postgres;Password=wjp930514.;Host=localhost;Port=5432;Database=ShopEmail;Pooling=true;",
    "ElasticsearchConnectionString": "http://192.168.2.110:9200/",
    "RedisConnectionString": "192.168.2.110:6379,password=wjp930514.,connectTimeout=1000,connectRetry=1,syncTimeout=10000"
  },
  "Emails": [
    {
      "Email": "973513569@qq.com",
      "Code": "awalxnuvfcogbbjj"
    }
  ],
  "AllowedHosts": "*"
}

     */
}
