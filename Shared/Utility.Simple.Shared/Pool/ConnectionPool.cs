#if !( NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 ||  NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
using System;
using System.Data;
using System.Threading;
using Utility.Pool;
using Utility.Validate;

namespace Utility.Pool
{
    /// <summary>
    /// 
    /// </summary>
    public class ConnectionPool:ObjectPool<IDbConnection> , IObjectPool<IDbConnection> 
    {
        #region 单个库数据连接池模式
        private string _connectinString = string.Empty;
        [ThreadStatic]
        private static volatile ConnectionPool _connectionPool;
        /// <summary> 管理连接池的线程</summary>
        private Thread _thread = null;

        private Type _connectionType;


        private ConnectionPool(Type connectionType,string connectinString)
        {
            ValidateHelper.ValidateArgumentObjectNull("connectionType", connectionType);
            ValidateHelper.ValidateArgumentNull("connectinString", connectinString);
            this._connectinString = connectinString;
            this._connectionType = connectionType;
            this.Create = () => {
                IDbConnection connection = (IDbConnection)Activator.CreateInstance(this._connectionType, new object[] { this._connectinString });
                return connection;
            };
            Initial();
            //将线程设置为后台线程
            //使得在程序退出后，线程自动结束
            this._thread = new Thread(this.ManagePool)
            {
                Name = "DatabasePoolManagerThread",
                IsBackground = true
            };
            this._thread.Start();
            this.StartTime = DateTime.Now;
         
        }

        /// <summary>
        /// 获取对象锁 防止多线程情况操作不正确
        /// </summary>
        protected static readonly object ObjLock = new object();
        /// <summary>
        /// 入口 通过反射创建数据库连接对象 数据库未知
        /// </summary>
        /// <param name="connectionType"></param>
        /// <param name="connectinString"></param>
        /// <returns></returns>
        public static ConnectionPool GetInstance(Type connectionType, string connectinString)
        {
            if (_connectionPool == null)
            {
                lock (ObjLock)
                {
                    if (_connectionPool == null)
                    {
                        _connectionPool = new ConnectionPool(connectionType,connectinString);
                    }
                }
            }
            return _connectionPool;
        }

      

        /// <summary>
        /// 管理连接池
        /// 将长时间处于空闲状态的连接释放
        /// </summary>
        private void ManagePool()
        {
            while (true)
            {
                lock (this.ObjectEntries)
                {
                    // 已删除连接个数
                    int num = 0;
                    if (this.Total - this.UseCount > this.MinPoolSize)
                    {
                        // 空闲连接大于最小连接池大小
                        // 将多余的空闲连接删除
                     
                        for (int i = 0; i < this.ObjectEntries.Count; i++)
                        {
                            double mill = DateTime.Now.Subtract(this.ObjectEntries[i].CreateOrUseTime).TotalMilliseconds;
                            double idle = this.ObjectEntries[i].IdeaMilliseconds;
                            if (mill > idle + ActiveTime)
                            {
                                int index = i - num;
                                IDbConnection conn = (IDbConnection)this.ObjectEntries[index].Object;
                                conn.Close();
                                this.ObjectEntries.RemoveAt(index);
                                Interlocked.Decrement(ref _total);
                                num++;
                            }
                        }
                    }
                }
                Thread.Sleep(500);
            }
        }
        #endregion
    }

}
#endif