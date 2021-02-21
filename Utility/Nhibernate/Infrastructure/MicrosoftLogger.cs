#if !(NET10 || NET11 || NET20 || NET30 || NET35 || NET40 || NET45 || NET451 || NET452 || NET46 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
//#if   NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using System;
using System.Collections.Generic;
//using Microsoft.Extensions.Logging.Internal;
using NHibernate;

namespace Utility.Nhibernate.Infrastructure
{
	/// <summary>
	/// INHibernateLogger 日志
	/// </summary>
	public class MicrosoftLogger : INHibernateLogger
	{
#if NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
		private readonly Microsoft.Extensions.Logging.ILogger _msLogger;

		public MicrosoftLogger(Microsoft.Extensions.Logging.ILogger msLogger)
		{
			_msLogger = msLogger ?? throw new ArgumentNullException(nameof(msLogger));
		}

		private static readonly Dictionary<NHibernateLogLevel, Microsoft.Extensions.Logging.LogLevel> MapLevels = new Dictionary<NHibernateLogLevel, Microsoft.Extensions.Logging.LogLevel>
		{
			{ NHibernateLogLevel.Trace, Microsoft.Extensions.Logging.LogLevel.Trace },
			{ NHibernateLogLevel.Debug, Microsoft.Extensions.Logging.LogLevel.Debug },
			{ NHibernateLogLevel.Info, Microsoft.Extensions.Logging.LogLevel.Information },
			{ NHibernateLogLevel.Warn, Microsoft.Extensions.Logging.LogLevel.Warning },
			{ NHibernateLogLevel.Error, Microsoft.Extensions.Logging.LogLevel.Error },
			{ NHibernateLogLevel.Fatal, Microsoft.Extensions.Logging.LogLevel.Critical },
			{ NHibernateLogLevel.None, Microsoft.Extensions.Logging.LogLevel.None },
		};
#endif

		/// <summary>
		/// 日志
		/// </summary>
		/// <param name="logLevel"></param>
		/// <param name="state"></param>
		/// <param name="exception"></param>
		public void Log(NHibernateLogLevel logLevel, NHibernateLogValues state, Exception exception)
		{
			//_msLogger.Log(MapLevels[logLevel], 0, new FormattedLogValues(state.Format, state.Args), exception, MessageFormatter);
#if NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
			_msLogger.Log(MapLevels[logLevel], 0, (object)state, exception, MessageFormatter);
#endif
		}

		/// <summary>
		/// 是否启用
		/// </summary>
		/// <param name="logLevel"></param>
		/// <returns></returns>
		public bool IsEnabled(NHibernateLogLevel logLevel)
		{
#if NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
			return _msLogger.IsEnabled(MapLevels[logLevel]);
#else
			return false;
#endif
		}

		/// <summary>
		/// 格式化格式
		/// </summary>
		/// <param name="state"></param>
		/// <param name="error"></param>
		/// <returns></returns>
		private static string MessageFormatter(object state, Exception error)
		{
			return state.ToString();
		}
	}
}
#endif
