#if !(NET10 || NET11 || NET20 || NET30 || NET35  || NETCOREAPP1_0 || NETCOREAPP1_1 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
//#if NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using System.Data;
using Utility.Domain.Entities;
using Utility.Domain.Repositories;
using Utility.Dapper.Uow;

namespace Utility.Dapper.Repositories
{

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="Key"></typeparam>
    public class BaseDapperRepository<T, Key> : BaseDapperRepository<T>, IRepository<T> where T : class, IEntity<Key>
    {
        /// <summary> 构造注册数据库连接对象</summary>
        /// <param name="connection">数据库连接对象</param>
        public BaseDapperRepository(IDbConnection connection) : base(connection)
        {

        }
    }



    /// <summary>dapper linq 不支持 需要自己转换 </summary>
    public class BaseDapperRepository<T> : BaseRepository<T>, IRepository<T> where T : class
    {
        /// <summary>
        /// 
        /// </summary>
        public new DapperUnitWork UnitWork { get;protected set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        public BaseDapperRepository(IDbConnection connection) 
        {
            UnitWork = new DapperUnitWork(connection);
            base.UnitWork = UnitWork;
        }
    }
}
#endif