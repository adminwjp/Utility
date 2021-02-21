//#if !(NET10 || NET11 || NET20 || NET30 || NET35 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
#if  NET40 ||NET45 || NET451 || NET452 || NET46 ||NET461 || NET462|| NET47 || NET471 || NET472|| NET48 ||  NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1

#if NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48
using System.Data.Entity;
#else
using Microsoft.EntityFrameworkCore;
#endif
using Utility.Model;

namespace Utility.Ef.DAL
{
    /// <summary>数据访问层接口基类</summary>
    public class BaseEfService<Model>:BaseEfDAL<Model> where Model : class
    {
        /// <summary> 构造函数注入</summary>
        /// <param name="context">数据库上下文</param>
        public BaseEfService(DbContext context) : base(context)
        {

        }
    }

    /// <summary>数据访问层接口基类</summary>
    public class BaseEfService<Model,Key> : BaseEfDAL<Model> where Model : class,IModel<Key>
    {
        /// <summary> 构造函数注入</summary>
        /// <param name="context">数据库上下文</param>
        public BaseEfService(DbContext context) : base(context)
        {

        }
    }


}
#endif