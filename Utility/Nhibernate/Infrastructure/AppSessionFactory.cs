#if !(NET10 || NET11 || NET20 || NET30 || NET35 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
//#if NET40 ||NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using System;

using System.Collections.Generic;
using System.IO;

namespace Utility.Nhibernate.Infrastructure
{
    /// <summary>
    /// nhibernate  config 管理 
    /// </summary>
    public class AppSessionFactory
	{
        /// <summary>
        /// get nhibernate open IStatelessSession  效率 比 session快.性能 session高
        /// </summary>
        /// <returns></returns>
        public IStatelessSession OpenStatelessSession()
		{
            return SessionFactory.OpenStatelessSession();
        }

        /// <summary>
        ///  get nhibernate open session 
        /// </summary>
        /// <returns></returns>
        public ISession OpenSession()
        {
#pragma warning disable CS0618 // 类型或成员已过时
            return SessionFactory.OpenSession(Interceptor ?? SqlInterceptor.Empty);
#pragma warning restore CS0618 // 类型或成员已过时
        }

        /// <summary>
        /// nhibernate 配置
        /// </summary>
        public Configuration Configuration { get; }
        /// <summary>
        /// nhibernate session factory
        /// </summary>
        public ISessionFactory SessionFactory { get; }
        /// <summary>
        ///  nhibernate  sql intercepter
        /// </summary>
        public IInterceptor Interceptor { get; set; }

        /// <summary>
        ///  nhibernate 配置
        /// </summary>
        /// <param name="interceptor"></param>
        public AppSessionFactory(IInterceptor interceptor = null) : this(config =>
           {
               config = config.Configure("Config/hibernate.cfg.xml");
               config.Interceptor = interceptor;
               foreach (var item in Directory.GetFiles("Config/hbm", "*.hbm.xml"))
               {
                   config.AddXmlFile(item);
               }
               SchemaExport export = new SchemaExport(config);
               export.SetOutputFile(Path.Combine(Directory.GetCurrentDirectory(), "sql.sql")); //设置输出目录
               // export.Drop(true, true);//设置生成表结构存在性判断,并删除
               export.Create(false, false);//设置是否生成脚本,是否导出来
            })
        {

        }

        /// <summary>
        ///  nhibernate 配置
        /// </summary>
        /// <param name="config"></param>
        public AppSessionFactory(Action<Configuration> config)
        {
            Configuration = new Configuration();
            config(Configuration);
            SessionFactory = Configuration.BuildSessionFactory();
        }

        /// <summary>
        ///  nhibernate 硬代码 配置
        /// </summary>
        /// <param name="config"></param>
        public static void DefaultCodeConfig(Configuration config)
        {
            config.Properties["connection.driver_class"] = "NHibernate.Driver.MySqlDataDriver";
            config.Properties["connection.connection_string"] = string.Empty;
            config.Properties["dialect"] = "NHibernate.Dialect.MySQL5Dialect";
            config.Properties["use_sql_comments"] = "true";
            config.Properties["command_timeout"] = "30";
            config.Properties["adonet.batch_size"] = "100";
            config.Properties["order_inserts"] = "true";
            config.Properties["order_updates"] = "true";
            config.Properties["adonet.batch_versioned_data"] = "true";
            config.Properties["show_sql"] = "true";
            config.Properties["format_sql"] = "true";
            config.Properties["hbm2ddl.auto"] = "update";
            //config.Properties["hbm2ddl.auto"] = "create";//
            config.Properties["current_session_context_class"] = "thread_static";
        }

        /// <summary>
        /// nhibernate 配置
        /// </summary>
        /// <param name="mappTypes"></param>
        /// <param name="sqlServerConnectionstring"></param>
        public AppSessionFactory(IEnumerable<Type> mappTypes,string sqlServerConnectionstring):this((conf)=> {
            conf.DataBaseIntegration(it =>
            {
                it.ConnectionString = sqlServerConnectionstring;//"Data Source=localhost;Initial Catalog=gibson;Integrated Security=True;Connect Timeout=150;Encrypt=False;uid=sa;pwd=wjp930514*;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                //控制台程序 https://www.cnblogs.com/kissdodog/p/4564204.html
                it.LogFormattedSql = true;
                it.LogSqlInConsole = true;
                it.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
                it.Driver<SqlClientDriver>();
                it.Dialect<MsSql2012Dialect>();
                it.SchemaAction = SchemaAutoAction.Update;

            });  
        },mappTypes)
        {
            
        }


        /// <summary>
        /// nhibernate 配置
        /// </summary>
        /// <param name="config"></param>
        /// <param name="mappTypes">mapp 映射类 类型</param>
        public AppSessionFactory(Action<Configuration> config,IEnumerable<Type> mappTypes)
        {
            var mapper = new ModelMapper();
            mapper.AddMappings(mappTypes);
            var domainMapping = mapper.CompileMappingForAllExplicitlyAddedEntities();
            Configuration = new Configuration();
            config(Configuration);
            Configuration.AddMapping(domainMapping);
            Configuration.SessionFactory().GenerateStatistics();
            //用NHibernate.Tool.hbm2ddl.SchemaExport生成表结构到D:\sql.sql文件当中
            SchemaExport export = new SchemaExport(Configuration);
            export.SetOutputFile("sql.sql"); //设置输出目录
            // export.Drop(true, true);//设置生成表结构存在性判断,并删除
            //export.Create(false, false);//设置是否生成脚本,是否导出来
            SessionFactory = Configuration.BuildSessionFactory();
        }
      
    }
}
#endif
