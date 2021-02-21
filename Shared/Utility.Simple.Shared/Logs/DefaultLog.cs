using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Utility.IO;

namespace Utility.Logs
{
    /// <summary>
    /// 默认 <see cref="ILog{T}"/> 实现
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DefaultLog<T> : DefaultLog, ILog<T>, ILog
    {
        /// <summary>
        /// 默认 <see cref="ILog{T}"/> 实现
        /// </summary>
        public DefaultLog():base(typeof(T))
        {
        }
    }

    /// <summary>
    ///  默认<see cref="ILog"/> 实现
    /// </summary>
    public class DefaultLog:ILog
    {
        /// <summary>
        /// 具体日志位置
        /// </summary>
        protected Type ClassType { get; set; }
        /// <summary>
        /// 日志 根 路劲
        /// </summary>
        public static string BasePath { get; set; } = string.Empty;
        /// <summary>
        /// 日志路劲
        /// </summary>
        protected  string Path
        {
            get
            {
                return $"Logs\\{DateTime.Now.ToString("yyyy-MM-dd")}\\{DateTime.Now.ToString("yyyy-MM-dd-HH")}.log";
            }
        }

        /// <summary>
        /// 默认<see cref="ILog"/> 实现
        /// </summary>
        /// <param name="classType"></param>
        public DefaultLog(Type classType)
        {
          
        }
  
        /// <summary>
        /// log 
        /// </summary>
        /// <param name="level"><see cref="LogLevel"/> log level</param>
        /// <param name="message">message</param>
        public virtual void Log(LogLevel level, object message)
        {
            string msg = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff")}====>> {message}";
            string output = $"{ClassType.FullName} {level.ToString().ToUpper()}: {msg}";
#if !(NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2)
            Console.WriteLine(output);
#endif
            FileHelper.WriteFile(Path, output);
        }

        /// <summary>
        /// log exception
        /// </summary>
        /// <param name="level"><see cref="LogLevel"/> log level</param>
        /// <param name="message">message</param>
        /// <param name="exception">exception</param>
        public virtual void LogException(LogLevel level, object message, Exception exception)
        {
            string msg = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff")}====>> {message}\r\nexception:{exception.Message}\r\n{exception.StackTrace}";
            string output = $"{ClassType.FullName} {level.ToString().ToUpper()}: {msg}";
#if !(NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2)
            Console.WriteLine(output);
#endif
            FileHelper.WriteFile(Path, output);
        }

        /// <summary>
        /// log format
        /// </summary>
        /// <param name="level"><see cref="LogLevel"/> log level</param>
        /// <param name="format">format</param>
        /// <param name="args">args</param>
        public virtual void LogFormat(LogLevel level, string format, object[] args)
        {
            string msg = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff")}====>> {string.Format(format, args)}";
            string output = $"{ClassType.FullName} {level.ToString().ToUpper()}: {msg}";
#if !(NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2)
            Console.WriteLine(output);
#endif
            FileHelper.WriteFile(Path, output);
        }

        /// <summary>
        /// log format
        /// </summary>
        /// <param name="level"><see cref="LogLevel"/> log level</param>
        /// <param name="format">format</param>
        /// <param name="arg0">arg0</param>
        public virtual void LogFormat(LogLevel level, string format, object arg0)
        {
            string msg = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff")}====>> {string.Format(format, arg0)}";
            string output = $"{ClassType.FullName} {level.ToString().ToUpper()}: {msg}";
#if !(NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2)
            Console.WriteLine(output);
#endif
            FileHelper.WriteFile(Path, output);
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
            string msg = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff")}====>> {string.Format(format, arg0, arg1)}";
            string output = $"{ClassType.FullName} {level.ToString().ToUpper()}: {msg}";
#if !(NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2)
            Console.WriteLine(output);
#endif
            FileHelper.WriteFile(Path, output);
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
            string msg = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff")}====>> {string.Format(format, arg0, arg1, arg2)}";
            string output = $"{ClassType.FullName} {level.ToString().ToUpper()}: {msg}";
#if !(NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2)
            Console.WriteLine(output);
#endif
            FileHelper.WriteFile(Path, output);
        }

        /// <summary>
        /// log format
        /// </summary>
        /// <param name="level"><see cref="LogLevel"/> log level</param>
        /// <param name="provider"><see cref="IFormatProvider"/> format provider</param>
        /// <param name="format">format</param>
        /// <param name="args">args</param>
        public virtual void LogFormat(LogLevel level, IFormatProvider provider, string format, object[] args)
        {
            string msg = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff")}====>> {string.Format(provider, format, args)}";
            string output = $"{ClassType.FullName} {level.ToString().ToUpper()}: {msg}";
#if !(NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2)
            Console.WriteLine(output);
#endif
            FileHelper.WriteFile(Path, output);
        }
    }
}
