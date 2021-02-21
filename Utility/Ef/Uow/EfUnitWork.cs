//#if !(NET10 || NET11 || NET20 || NET30 || NET35 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
#if  NET40 ||NET45 || NET451 || NET452 || NET46 ||NET461 || NET462|| NET47 || NET471 || NET472|| NET48 ||  NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1

#if NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48
using System.Data.Entity;
#else
using Microsoft.EntityFrameworkCore;
#endif
using System;
using Utility.Domain.Uow;
using System.Threading.Tasks;
using System.Threading;

namespace Utility.Ef.Uow
{
    /// <summary>
    /// 工作单元接口
    /// <para> 适合在一下情况使用:</para>
    /// <para>1 在同一事务中进行多表操作</para>
    /// <para>2 需要多表联合查询</para>
    /// <para>因为架构采用的是EF访问数据库，暂时可以不用考虑采用传统Unit Work的注册机制</para>
    /// </summary>
    public class EfUnitWork :EfTemaplate, IUnitWork
    {

        /// <summary> 构造函数注入</summary>
        /// <param name="context">数据库上下文</param>
        public EfUnitWork(DbContext context):base(context)
        {
        }


        /// <summary> 数据库上下文 dbcontext ef 有效 </summary>

        public virtual  DbContext DbContext { get { return this.context; } }

        /// <summary> 操作成功 保存到库里 </summary>
        public override void Save()
        {
            this.context.SaveChanges();
        }

        /// <summary> 操作成功 保存到库里 </summary>
        /// <param name="cancellationToken"></param>
        public override Task SaveAsync(CancellationToken cancellationToken = default)
        {
#if !(NET40)
            //try
            //{
                 return context.SaveChangesAsync();
            //}
            //catch (DbEntityValidationException e)
            //{
            //    throw new Exception(e.EntityValidationErrors.First().ValidationErrors.First().ErrorMessage);
            //}
#else
            Save();
            return TaskHelper.CompletedTask;
#endif
        }

    }
}
#endif