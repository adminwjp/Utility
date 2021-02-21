#if !(NET10 || NET11 || NET20 || NET30 || NET35 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
//#if NET40 ||NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using System;
using Utility.Domain.Entities;

namespace Utility.Nhibernate.EntityMappings
{

    /// <summary>nhibernate 基类 xml mapp 必须虚方法(属性)不然错误 ,注解可以不需要 虚方法(属性) </summary>
    public abstract class BaseFluentNhibernateMapp<Entity, Key> : BaseFluentNhibernateMapp<Entity> where Entity : class, IEntity<Key>
    {
        /// <summary>
        /// 
        /// </summary>
        public BaseFluentNhibernateMapp()
        {
            //隐式 调用 基类构造 函数
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        public BaseFluentNhibernateMapp(string tableName) : base(tableName)
        {
            var id = Id(x => x.Id);
            if (typeof(Key).IsValueType)
            {
                if(typeof(Key)== typeof(Guid) || typeof(Key) == typeof(Guid?))
                {
                    id.GeneratedBy.Guid();
                }
                else
                {
                    id.GeneratedBy.Native();
                }
            }
            else
            {
                id.GeneratedBy.UuidString();
            }


        }
    }

    /// <summary>nhibernate 基类 xml mapp 必须虚方法(属性)不然错误 ,注解可以不需要 虚方法(属性) </summary>
    public abstract class BaseFluentNhibernateMapp<Model> : FluentNHibernate.Mapping.ClassMap<Model> where Model :class
    {
        /// <summary>
        /// 
        /// </summary>
        public BaseFluentNhibernateMapp()
        {
            Set();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        public BaseFluentNhibernateMapp(string tableName)
        {
            Table(tableName);
            LazyLoad();
            Set();
        }

        /// <summary>
        /// 属性 映射
        /// </summary>
        protected abstract void Set();
    }

    /// <summary>nhibernate 基类 xml mapp 必须虚方法(属性)不然错误 ,注解可以不需要 虚方法(属性) </summary>
    public abstract class BaseNhibernateMapp<Entity,Key> : BaseNhibernateMapp<Entity> where Entity : class, IEntity<Key>
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        public BaseNhibernateMapp(string tableName):base(tableName)
        {
            Id(x => x.Id, map =>
            {
                if (typeof(Key).IsValueType)
                {
                    if (typeof(Key) == typeof(Guid) || typeof(Key) == typeof(Guid?))
                        map.Generator(NHibernate.Mapping.ByCode.Generators.Guid);
                    else
                        map.Generator(NHibernate.Mapping.ByCode.Generators.Native);
                }
                else
                {
                    map.Generator(NHibernate.Mapping.ByCode.Generators.UUIDString);
                }
            });//编号

            
        }
    }

    /// <summary>nhibernate 基类 xml mapp 必须虚方法(属性)不然错误 ,注解可以不需要 虚方法(属性) </summary>
    public abstract class BaseNhibernateMapp<Model> : NHibernate.Mapping.ByCode.Conformist.ClassMapping<Model> where Model : class
    {
        /// <summary>
        /// 
        /// </summary>
        public BaseNhibernateMapp()
        {
            Set();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        public BaseNhibernateMapp(string tableName)
        {
            Table(tableName);
            Lazy(false);
            Set();
        }

        /// <summary>
        /// 属性 映射
        /// </summary>
        protected abstract void Set();
    }
}
#endif