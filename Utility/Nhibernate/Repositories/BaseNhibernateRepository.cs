#if !(NET10 || NET11 || NET20 || NET30 || NET35 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
//#if NET40 ||NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NET481 || NET482 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NETCOREAPP3_2 || NETSTANDARD2_0 || NETSTANDARD2_1
using NHibernate;
using Utility.Domain.Entities;
using Utility.Domain.Repositories;
using Utility.Nhibernate.Uow;

namespace Utility.Nhibernate.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="Entity"></typeparam>
    /// <typeparam name="Key"></typeparam>
    public class BaseNhibernateRepository<Entity,Key> : BaseNhibernateRepository<Entity>, IRepository<Entity,Key> where Entity : class, IEntity<Key>
   
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        public BaseNhibernateRepository(ISession session):base(session)
        {
            
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="Entity"></typeparam>
    public class BaseNhibernateRepository<Entity> : BaseRepository<Entity>, IRepository<Entity> where Entity:class
    {

        /// <summary>
        /// 
        /// </summary>
        public new NhibernateUnitWork UnitWork { get;protected set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        public BaseNhibernateRepository(ISession session)
        {
            UnitWork = new NhibernateUnitWork(session);
            base.UnitWork = UnitWork;
            Session = session;
        }
        /// <summary>
        /// 查询 条件 不支持 linq 需要 自己转换(组合linq)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected virtual ICriteria GetWhere(Entity entity)
        {
            return null;
        }
        //public NhibernateUnitWork(AppSessionFactory sessionFactory) : base(sessionFactory)
        //{
        //    
        //}
        /// <summary> 数据库上下文 ISession nhibernate 有效 </summary>
        public ISession Session { get; protected set; }


        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="Key"></typeparam>
        /// <param name="id"></param>
        public override void Delete<Key>(Key id)
        {
            base.UnitWork.Delete<Entity>(id);
        }
    }
}
#endif
