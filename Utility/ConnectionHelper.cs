using System;
using System.Collections.Generic;
using System.Text;
#if  NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using Microsoft.Extensions.Configuration;
#endif
#if !( NETCOREAPP1_0 || NETCOREAPP1_1 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
using System.Configuration;
#endif

namespace Utility
{
    /// <summary>
    ///   连接地址 帮助类
    /// </summary>
    public class ConnectionHelper
    {
        private static string connectionString;

        /// <summary>
        /// 连接地址
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(connectionString))
                {
                    connectionString = GetConnectionString();
                }
                return connectionString;
            }
            set { connectionString = value; }
        }

        /// <summary>
        /// 获取  连接地址
        /// </summary>
        /// <returns></returns>

        public static string GetConnectionString()
        {
            string name = $"{DbConfig.Flag}ConnectionString";
          
            string sqlConnectionString = string.Empty;
#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
            if (string.IsNullOrEmpty(sqlConnectionString))
            {
                sqlConnectionString = ConfigurationManager.ConnectionStrings[name]?.ConnectionString;
                Console.WriteLine($"app.config or web.config read {name} value {sqlConnectionString}");
            }
#endif
#if  NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1

            if (string.IsNullOrEmpty(sqlConnectionString))
            {
                string text = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                if (string.IsNullOrEmpty(text))
                {
                    text = "Development";
                }
                IConfigurationBuilder configurationBuilder = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings." + text + ".json", optional: false, reloadOnChange: true);
                //.AddEnvironmentVariables();
                IConfigurationRoot configuration = configurationBuilder.Build();
                sqlConnectionString = configuration.GetConnectionString(name);
            }
#endif
            connectionString = sqlConnectionString;
            return sqlConnectionString;

        }
    }
}
