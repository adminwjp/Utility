#if !(NET10 || NET11 || NET20 || NET30 || NET35 || NET40 || NET45 || NET451 || NET452 || NET46 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
//#if   NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using NHibernate;
using System;

namespace Utility.Nhibernate.Infrastructure
{
	/// <summary>
	/// 
	/// </summary>
	public class MicrosoftLoggerFactory : INHibernateLoggerFactory
	{


#if NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
		private readonly Microsoft.Extensions.Logging.ILoggerFactory _loggerFactory;

		public MicrosoftLoggerFactory(Microsoft.Extensions.Logging.ILoggerFactory loggerFactory)
		{
			_loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
		}

#endif

		/// <summary>
		/// 
		/// </summary>
		/// <param name="keyName"></param>
		/// <returns></returns>
#if NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
		public INHibernateLogger LoggerFor(string keyName)
		{
			var msLogger = _loggerFactory.CreateLogger(keyName);
			return new MicrosoftLogger(msLogger);
		}
#else

		public INHibernateLogger LoggerFor(string keyName)
		{
			return null;
		}
#endif

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public INHibernateLogger LoggerFor(System.Type type)
		{
            //return LoggerFor(Microsoft.Extensions.Logging.Abstractions.Internal.TypeNameHelper.GetTypeDisplayName(type));
            return LoggerFor(type.Name);
        }
	}
}
#endif
