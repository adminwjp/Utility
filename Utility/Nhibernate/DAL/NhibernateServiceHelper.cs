#if !(NET10 || NET11 || NET20 || NET30 || NET35 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
//#if NET40 ||NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48  || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using Utility.Model;

namespace Utility.Nhibernate.DAL
{

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="Model"></typeparam>
    /// <typeparam name="Key"></typeparam>
    public class NhibernateServiceHelper<Model,Key> : NhibernateDALHelper<Model, Key> where Model : class, IModel<Key>
    {

    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="Model"></typeparam>
    public class NhibernateServiceHelper<Model> : NhibernateDALHelper<Model> where Model : class
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public  class NhibernateServiceHelper:NhibernateDALHelper
    {
    }
}
#endif
