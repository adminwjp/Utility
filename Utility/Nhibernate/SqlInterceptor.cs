#if !(NET10 || NET11 || NET20 || NET30 || NET35 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
//#if NET40 ||NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48  || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using NHibernate;
using NHibernate.SqlCommand;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Utility.Nhibernate
{
    /// <summary>
    /// NHibernate 拦截器
    /// </summary>
    public class SqlInterceptor : EmptyInterceptor
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly SqlInterceptor Empty = new SqlInterceptor();
        /// <summary>
        /// NHibernate 拦截器
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public override SqlString OnPrepareStatement(SqlString sql)
        {
            Write($"{DateTime.Now.ToString("yyyy-MM-dd")}===>>{sql}");
            return base.OnPrepareStatement(sql);
        }
        private readonly object obj = new object();//锁
        /// <summary>
        /// 写入
        /// </summary>
        /// <param name="content"></param>
        private void Write(string content)
        {
            string dire = $"{Environment.CurrentDirectory}\\{DateTime.Now.ToString("yyyy-MM-dd")}";
            Write(dire, $"{dire}\\{DateTime.Now.ToString("yyyy-MM-dd")}.log", content);
        }
        /// <summary>
        /// 写入
        /// </summary>
        /// <param name="file"></param>
        /// <param name="content"></param>
        private void Write(string file, string content)
        {
            Write( $"{Environment.CurrentDirectory}\\{DateTime.Now.ToString("yyyy-MM-dd")}",file,content);
        }
        /// <summary>
        /// 写入
        /// </summary>
        /// <param name="dire"></param>
        /// <param name="file"></param>
        /// <param name="content"></param>
        private void Write(string dire,string file,string content)
        {
            lock (obj)
            {
                try
                {
                    if (Directory.Exists(dire))
                    {
                        Directory.CreateDirectory(dire);
                    }
                    using (StreamWriter sw = new StreamWriter($"{dire}\\{file}", true, Encoding.UTF8))
                    {
                        sw.WriteLine(content);
                    }
                }
                catch (Exception)
                {

                }
            }
        }
    }
}
#endif
