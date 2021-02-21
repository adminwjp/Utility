#if !(NET20 || NET30 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2)
using System;
using NLog;

namespace Utility.Logs
{

    /// <summary>
    ///  nlog log
    /// </summary>
    /// <typeparam name="T">generic class</typeparam>
    public class NLogLog<T> :NLogLog,ILog<T>, ILog
    {
        public NLogLog():base(typeof(T))
        {

        }
    }

    /// <summary>
    /// nlog log
    /// </summary>
    public class NLogLog:ILog
    {
        /// <summary>
        ///<see cref="NLog.ILogger"/> nlgo logger
        /// </summary>
        public ILogger Logger { get; protected set; }
        
        /// <summary>
        ///has param constractor
        /// </summary>
        /// <param name="classType">class type</param>
        public NLogLog(Type classType)
        {
            //Logger = LogManager.GetCurrentClassLogger();
            Logger = LogManager.GetCurrentClassLogger(classType);
        }

        /// <summary>
        /// log 
        /// </summary>
        /// <param name="level"><see cref="LogLevel"/> log level</param>
        /// <param name="message">message</param>
        public virtual void Log(LogLevel level, object message)
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
        public virtual void LogException(LogLevel level, object message, Exception exception)
        {
            switch (level)
            {
                case LogLevel.Debug:
#pragma warning disable CS0618 // 类型或成员已过时
                    Logger.DebugException(message?.ToString(), exception);
#pragma warning restore CS0618 // 类型或成员已过时
                    break;
                case LogLevel.Warn:
#pragma warning disable CS0618 // 类型或成员已过时
                    Logger.WarnException(message?.ToString(), exception);
#pragma warning restore CS0618 // 类型或成员已过时
                    break;
                case LogLevel.Fatal:
#pragma warning disable CS0618 // 类型或成员已过时
                    Logger.FatalException(message?.ToString(), exception);
#pragma warning restore CS0618 // 类型或成员已过时
                    break;
                case LogLevel.Error:
#pragma warning disable CS0618 // 类型或成员已过时
                    Logger.ErrorException(message?.ToString(), exception);
#pragma warning restore CS0618 // 类型或成员已过时
                    break;
                case LogLevel.None:
                case LogLevel.Info:
#pragma warning disable CS0618 // 类型或成员已过时
                    Logger.InfoException(message?.ToString(), exception);
#pragma warning restore CS0618 // 类型或成员已过时
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
        public virtual void LogFormat(LogLevel level, string format,  object[] args)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    Logger.Debug(format, args);
                    break;
                case LogLevel.Warn:
                    Logger.Warn(format, args);
                    break;
                case LogLevel.Fatal:
                    Logger.Fatal(format, args);
                    break;
                case LogLevel.Error:
                    Logger.Error(format, args);
                    break;
                case LogLevel.None:
                case LogLevel.Info:
                    Logger.Info(format, args);
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
        public virtual void LogFormat(LogLevel level, string format, object arg0)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    Logger.Debug(format, arg0);
                    break;
                case LogLevel.Warn:
                    Logger.Warn(format, arg0);
                    break;
                case LogLevel.Fatal:
                    Logger.Fatal(format, arg0);
                    break;
                case LogLevel.Error:
                    Logger.Error(format, arg0);
                    break;
                case LogLevel.None:
                case LogLevel.Info:
                    Logger.Info(format, arg0);
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
                    Logger.Debug(format, arg0, arg1);
                    break;
                case LogLevel.Warn:
                    Logger.Warn(format, arg0, arg1);
                    break;
                case LogLevel.Fatal:
                    Logger.Fatal(format, arg0, arg1);
                    break;
                case LogLevel.Error:
                    Logger.Error(format, arg0, arg1);
                    break;
                case LogLevel.None:
                case LogLevel.Info:
                    Logger.Info(format, arg0, arg1);
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
        public virtual void LogFormat(LogLevel level, string format, object arg0, object arg1, object arg2)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    Logger.Debug(format, arg0, arg1, arg2);
                    break;
                case LogLevel.Warn:
                    Logger.Warn(format, arg0, arg1, arg2);
                    break;
                case LogLevel.Fatal:
                    Logger.Fatal(format, arg0, arg1, arg2);
                    break;
                case LogLevel.Error:
                    Logger.Error(format, arg0, arg1, arg2);
                    break;
                case LogLevel.None:
                case LogLevel.Info:
                    Logger.Info(format, arg0, arg1, arg2);
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
                    Logger.Debug(provider, format, args);
                    break;
                case LogLevel.Warn:
                    Logger.Warn(provider, format, args);
                    break;
                case LogLevel.Fatal:
                    Logger.Fatal(provider, format, args);
                    break;
                case LogLevel.Error:
                    Logger.Error(provider, format, args);
                    break;
                case LogLevel.None:
                case LogLevel.Info:
                    Logger.Info(provider, format, args);
                    break;
                default:
                    break;
            }
        }

    
    }
}
#endif