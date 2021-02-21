#if !(NET10 || NET11 || NET20 || NET30 || NET35 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
//#if  NET40 ||NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using Utility.Model;

namespace Utility.Nhibernate.DAL.Mapp
{
    /// <summary>nhibernate 基类 xml mapp 必须虚方法(属性)不然错误 ,注解可以不需要 虚方法(属性) </summary>
    public abstract class BaseNhibernateMapp<Model,Key> : BaseNhibernateMapp<Model> where Model : class, IModel<Key>
    {
        /// <summary>
        /// nhibernate 基类 xml mapp 必须虚方法(属性)不然错误 ,注解可以不需要 虚方法(属性)
        /// </summary>
        /// <param name="tableName">表名</param>
        public BaseNhibernateMapp(string tableName):base(tableName)
        {
            Id(x => x.Id, map =>
            {
                if (typeof(Key).IsValueType)
                {
                    map.Generator(NHibernate.Mapping.ByCode.Generators.Native);
                }
                else
                {
                    map.Length(36);
                }
            });//编号

            
        }
    }

    /// <summary>nhibernate 基类 xml mapp 必须虚方法(属性)不然错误 ,注解可以不需要 虚方法(属性) </summary>
    public abstract class BaseNhibernateMapp<Model> : NHibernate.Mapping.ByCode.Conformist.ClassMapping<Model> where Model : class
    {
        /// <summary>
        /// nhibernate 基类 xml mapp 必须虚方法(属性)不然错误 ,注解可以不需要 虚方法(属性)
        /// </summary>
        public BaseNhibernateMapp()
        {
            Set();
        }

        /// <summary>
        /// nhibernate 基类 xml mapp 必须虚方法(属性)不然错误 ,注解可以不需要 虚方法(属性)
        /// </summary>
        /// <param name="tableName">表名</param>
        public BaseNhibernateMapp(string tableName)
        {
            Table(tableName);
            Lazy(false);
            Set();
        }
        protected abstract void Set();
    }
}
#endif