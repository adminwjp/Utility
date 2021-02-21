//#if !(NET10 || NET11 || NET20 || NET30 || NET35 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
#if  NET40 ||NET45 || NET451 || NET452 || NET46 ||NET461 || NET462|| NET47 || NET471 || NET472|| NET48 ||  NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1


#if NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48
using System.Data.Entity;
#else
using Microsoft.EntityFrameworkCore;
#endif
using System;
using System.Linq.Expressions;
using Utility.Domain.Entities;
using Utility.Domain.Repositories;
using Utility.Ef.Uow;

namespace Utility.Ef.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="Context"></typeparam>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="Key"></typeparam>
    public class BaseEfRepository<Context,T, Key> : BaseEfRepository<T> 
        where Context:DbContext
        where T : class, IEntity<Key>
    {
        /// <summary> 构造函数注入</summary>
        /// <param name="context">数据库上下文</param>
        public BaseEfRepository(Context context) : base(context)
        {
            this.DbContext = context;
        }

        /// <summary>
        /// 
        /// </summary>
        public new virtual Context DbContext { get;private set; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="Key"></typeparam>
    public class BaseEfRepository<T,Key>: BaseEfRepository<T> where T : class, IEntity<Key>
    {
        /// <summary> 构造函数注入</summary>
        /// <param name="context">数据库上下文</param>
        public BaseEfRepository(DbContext context):base(context)
        {
        }
    }

    /// <summary> ef </summary>
    /// <typeparam name="T"></typeparam>
    public  class BaseEfRepository<T> : BaseRepository<T>,IDisposable, IRepository<T> where T : class
    {
        /// <summary>
        /// 
        /// </summary>
        public new EfUnitWork UnitWork { get;protected set; }

        /// <summary> 构造函数注入</summary>
        /// <param name="context">数据库上下文</param>
        public BaseEfRepository(DbContext context)
        {
            DbContext = context;
            UnitWork = new EfUnitWork(context);
            base.UnitWork = this.UnitWork;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected virtual Expression<Func<T,bool>> GetWhere(T entity)
        {
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual  DbContext DbContext { get; protected set; }

    }
}
#endif