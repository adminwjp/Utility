#if !(NET20 || NET30 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
using System;
using System.IO;
using log4net;
using log4net.Config;
using log4net.Repository;

namespace Utility.Logs
{
    /// <summary>
    /// <![CDATA[
    /// https://www.cnblogs.com/jxcia_Lai/p/3511247.html
    /// ]]>
    /// NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 
    /// </summary>
    public class Log4Log<T> : Log4Log,ILog<T>, ILog
    {
        /// <summary>
        /// log4net
        /// </summary>
        public Log4Log() : base(typeof(T))
        {

        }
    }

    /// <summary>
    /// <![CDATA[
    /// https://www.cnblogs.com/jxcia_Lai/p/3511247.html
    /// ]]>
    /// NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 
    /// </summary>
    public class Log4Log : ILog
    {
        static Log4Log()
        {
            lock(repository)
            {
                if(!load)
                {
                    XmlConfigurator.Configure(repository, new FileInfo("log4.config"));
                    load = true;
                }
            }
        }
        static bool load = false;
        /// <summary>
        /// log4 log repository
        /// </summary>
        public static  readonly ILoggerRepository repository = LogManager.CreateRepository("NETCoreRepository");

        /// <summary>
        /// <see cref="log4net.ILog"/> logger 
        /// </summary>
        public  log4net.ILog Logger { get; protected set; }

        /// <summary>
        /// has param constractor
        /// </summary>
        /// <param name="classType">class type</param>
        public Log4Log(Type classType)
        {
      
            //NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6
            // Logger = log4net.LogManager.GetLogger(repository.Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            Logger = log4net.LogManager.GetLogger(repository.Name, classType);
        }

        /// <summary>
        /// log 
        /// </summary>
        /// <param name="level"><see cref="LogLevel"/> log level</param>
        /// <param name="message">message</param>
        public virtual void Log(LogLevel level,object message)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    Logger.Debug(message);
                    break;
                case LogLevel.Warn:
                    Logger.Warn(message);
                    break;
                case LogLevel.Fatal:
                    Logger.Fatal(message);
                    break;
                case LogLevel.Error:
                    Logger.Error(message);
                    break;
                case LogLevel.None:
                case LogLevel.Info:
                    Logger.Info(message);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// log exception
        /// </summary>
        /// <param name="level"><see cref="LogLevel"/> log level</param>
        /// <param name="message">message</param>
        /// <param name="exception">exception</param>
        public virtual void LogException(LogLevel level,object message, Exception exception)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    Logger.Debug(message, exception);
                    break;
                case LogLevel.Warn:
                    Logger.Warn(message, exception);
                    break;
                case LogLevel.Fatal:
                    Logger.Fatal(message, exception);
                    break;
                case LogLevel.Error:
                    Logger.Error(message, exception);
                    break;
                case LogLevel.None:
                case LogLevel.Info:
                    Logger.Info(message, exception);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// log format
        /// </summary>
        /// <param name="level"><see cref="LogLevel"/> log level</param>
        /// <param name="format">format</param>
        /// <param name="args">args</param>
        public virtual void LogFormat(LogLevel level,string format,  object[] args)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    Logger.DebugFormat(format, args);
                    break;
                case LogLevel.Warn:
                    Logger.WarnFormat(format, args);
                    break;
                case LogLevel.Fatal:
                    Logger.FatalFormat(format, args);
                    break;
                case LogLevel.Error:
                    Logger.ErrorFormat(format, args);
                    break;
                case LogLevel.None:
                case LogLevel.Info:
                    Logger.InfoFormat(format, args);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// log format
        /// </summary>
        /// <param name="level"><see cref="LogLevel"/> log level</param>
        /// <param name="format">format</param>
        /// <param name="arg0">arg0</param>
        public virtual void LogFormat(LogLevel level, string format,  object arg0)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    Logger.DebugFormat(format, arg0);
                    break;
                case LogLevel.Warn:
                    Logger.WarnFormat(format, arg0);
                    break;
                case LogLevel.Fatal:
                    Logger.FatalFormat(format, arg0);
                    break;
                case LogLevel.Error:
                    Logger.ErrorFormat(format, arg0);
                    break;
                case LogLevel.None:
                case LogLevel.Info:
                    Logger.InfoFormat(format, arg0);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// log format
        /// </summary>
        /// <param name="level"><see cref="LogLevel"/> log level</param>
        /// <param name="format">format</param>
        /// <param name="arg0">arg0</param>
        /// <param name="arg1">arg1</param>

        public virtual void LogFormat(LogLevel level, string format, object arg0, object arg1)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    Logger.DebugFormat(format, arg0, arg1);
                    break;
                case LogLevel.Warn:
                    Logger.WarnFormat(format, arg0, arg1);
                    break;
                case LogLevel.Fatal:
                    Logger.FatalFormat(format, arg0, arg1);
                    break;
                case LogLevel.Error:
                    Logger.ErrorFormat(format, arg0, arg1);
                    break;
                case LogLevel.None:
                case LogLevel.Info:
                    Logger.InfoFormat(format, arg0, arg1);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// log format
        /// </summary>
        /// <param name="level"><see cref="LogLevel"/> log level</param>
        /// <param name="format">format</param>
        /// <param name="arg0">arg0</param>
        /// <param name="arg1">arg1</param>
        /// <param name="arg2">arg2</param>
        public  virtual void LogFormat(LogLevel level, string format, object arg0, object arg1, object arg2)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    Logger.DebugFormat(format, arg0, arg1, arg2);
                    break;
                case LogLevel.Warn:
                    Logger.WarnFormat(format, arg0, arg1, arg2);
                    break;
                case LogLevel.Fatal:
                    Logger.FatalFormat(format, arg0, arg1, arg2);
                    break;
                case LogLevel.Error:
                    Logger.ErrorFormat(format, arg0, arg1, arg2);
                    break;
                case LogLevel.None:
                case LogLevel.Info:
                    Logger.InfoFormat(format, arg0, arg1, arg2);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// log format
        /// </summary>
        /// <param name="level"><see cref="LogLevel"/> log level</param>
        /// <param name="provider"><see cref="IFormatProvider"/> format provider</param>
        /// <param name="format">format</param>
        /// <param name="args">args</param>
        public virtual void LogFormat(LogLevel level, IFormatProvider provider, string format,  object[] args)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    Logger.DebugFormat(provider, format, args);
                    break;
                case LogLevel.Warn:
                    Logger.WarnFormat(provider, format, args);
                    break;
                case LogLevel.Fatal:
                    Logger.FatalFormat(provider, format, args);
                    break;
                case LogLevel.Error:
                    Logger.ErrorFormat(provider, format, args);
                    break;
                case LogLevel.None:
                case LogLevel.Info:
                    Logger.InfoFormat(provider, format, args);
                    break;
                default:
                    break;
            }
        }
    }
}
#endif