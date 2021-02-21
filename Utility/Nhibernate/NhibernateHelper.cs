#if !(NET10 || NET11 || NET20 || NET30 || NET35 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
using System;
using System.Collections.Generic;
using System.Text;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;

namespace Utility.Nhibernate
{
    /// <summary>
    /// Nhibernate 帮助类 支持 sqlite mysql sqlserver postgre oracle
    /// </summary>
    public static class NhibernateHelper
    {
        /// <summary>
        /// 支持 sqlite mysql sqlserver postgre oracle
        /// </summary>
        /// <param name="configuration"></param>
        public static void UseNhibernate(this FluentConfiguration configuration)
        {
            var connectionString=ConnectionHelper.ConnectionString;
            switch (DbConfig.Flag)
            {
                case DbFlag.MySql:
                    configuration.Database(MySQLConfiguration.Standard.ConnectionString(connectionString)
                        .ShowSql().FormatSql().Raw("hbm2ddl.auto", "update"));
                    break;
                case DbFlag.SqlServer:
                    configuration.Database(MsSqlCeConfiguration.Standard.ConnectionString(connectionString)
                         .ShowSql().FormatSql().Raw("hbm2ddl.auto", "update"));
                    break;
                case DbFlag.Oracle:
                    configuration.Database(OracleManagedDataClientConfiguration.Oracle10.ConnectionString(connectionString)
                        .ShowSql().FormatSql().Raw("hbm2ddl.auto", "update"));
                    break;
                case DbFlag.Postgre:
                    configuration.Database(PostgreSQLConfiguration.Standard.ConnectionString(connectionString)
                        .ShowSql().FormatSql().Raw("hbm2ddl.auto", "update"));
                    break;
                case DbFlag.Sqlite:
                    configuration.Database(SQLiteConfiguration.Standard.ConnectionString(connectionString)
                         .ShowSql().FormatSql().Raw("hbm2ddl.auto", "update"));
                    break;
                default:
                    throw new System.NotSupportedException("not support database");
            }
        }
    }
}
#endif