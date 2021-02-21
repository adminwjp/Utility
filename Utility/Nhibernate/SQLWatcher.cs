#if NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using Microsoft.Extensions.Logging;
using NHibernate;
using System;

namespace Utility.Nhibernate
{
    public class SQLWatcher : EmptyInterceptor
    {
        private readonly ILogger<SQLWatcher> _logger;
        public SQLWatcher(Microsoft.Extensions.Logging.ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<SQLWatcher>();
        }
        public override NHibernate.SqlCommand.SqlString OnPrepareStatement(NHibernate.SqlCommand.SqlString sql)
        {
            _logger.LogInformation($"sql语句:{sql}" );
            return base.OnPrepareStatement(sql);
        }
    }
}
#endif
