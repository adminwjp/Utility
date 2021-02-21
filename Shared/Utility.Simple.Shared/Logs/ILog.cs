using System;

namespace Utility.Logs
{
    /// <summary>
    /// 日志 等级
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// 无
        /// </summary>
        None=0x0,
        /// <summary>
        /// Info
        /// </summary>
        Info = 0x1,
        /// <summary>
        /// Debug
        /// </summary>
        Debug = 0x2,
        /// <summary>
        /// Warn
        /// </summary>
        Warn = 0x3,
        /// <summary>
        /// Fatal
        /// </summary>
        Fatal = 0x4,
        /// <summary>
        /// Error
        /// </summary>
        Error = 0x5
    }
    /// <summary>
    /// log interface
    /// </summary>
    public interface ILog<T> : ILog
    {

    }

    /// <summary>
    /// log interface
    /// </summary>
    public interface ILog
    {

        /// <summary>
        /// log 
        /// </summary>
        /// <param name="level"><see cref="LogLevel"/> log level</param>
        /// <param name="message">message</param>
        void Log(LogLevel level, object message);

        /// <summary>
        /// log exception
        /// </summary>
        /// <param name="level"><see cref="LogLevel"/> log level</param>
        /// <param name="message">message</param>
        /// <param name="exception">exception</param>
        void LogException(LogLevel level, object message, Exception exception);

        /// <summary>
        /// log format
        /// </summary>
        /// <param name="level"><see cref="LogLevel"/> log level</param>
        /// <param name="format">format</param>
        /// <param name="args">args</param>
        void LogFormat(LogLevel level, string format, params object[] args);

        /// <summary>
        /// log format
        /// </summary>
        /// <param name="level"><see cref="LogLevel"/> log level</param>
        /// <param name="format">format</param>
        /// <param name="arg0">arg0</param>
        void LogFormat(LogLevel level, string format, object arg0);

        /// <summary>
        /// log format
        /// </summary>
        /// <param name="level"><see cref="LogLevel"/> log level</param>
        /// <param name="format">format</param>
        /// <param name="arg0">arg0</param>
        /// <param name="arg1">arg1</param>

        void LogFormat(LogLevel level, string format, object arg0, object arg1);

        /// <summary>
        /// log format
        /// </summary>
        /// <param name="level"><see cref="LogLevel"/> log level</param>
        /// <param name="format">format</param>
        /// <param name="arg0">arg0</param>
        /// <param name="arg1">arg1</param>
        /// <param name="arg2">arg2</param>
        void LogFormat(LogLevel level, string format, object arg0, object arg1, object arg2);

        /// <summary>
        /// log format
        /// </summary>
        /// <param name="level"><see cref="LogLevel"/> log level</param>
        /// <param name="provider"><see cref="IFormatProvider"/> format provider</param>
        /// <param name="format">format</param>
        /// <param name="args">args</param>
        void LogFormat(LogLevel level, IFormatProvider provider, string format,  object[] args);

      
    }
}
