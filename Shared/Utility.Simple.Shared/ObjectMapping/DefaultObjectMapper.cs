#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
using System;
using System.Reflection;

namespace Utility.ObjectMapping
{

    /// <summary>
    ///<see cref="IObjectMapper"/>  interface  implement.
    ///simple custom implement
    /// </summary>
    public class DefaultObjectMapper : IObjectMapper
    {

        /// <summary>
        /// new DefaultObjectMapper()
        /// </summary>
        public static readonly DefaultObjectMapper Empty = new DefaultObjectMapper();

        /// <summary>
        /// according source entity mapping  target entity
        /// </summary>
        /// <typeparam name="SoucreEntity">source entity</typeparam>
        /// <typeparam name="TargetEntity">target entity</typeparam>
        /// <param name="source">source entity</param>
        /// <returns>return target entity</returns>
        public virtual TargetEntity Map<SoucreEntity, TargetEntity>(SoucreEntity source)
        {
            TargetEntity target = Activator.CreateInstance<TargetEntity>();
            Mapp<SoucreEntity,TargetEntity>(source, target);
            return target;
        }

        /// <summary>
        /// according source entity mapping  target entity
        /// </summary>
        /// <typeparam name="TargetEntity">target entity</typeparam>
        /// <param name="source">source entity</param>
        /// <returns>return target entity</returns>
        public virtual TargetEntity Map<TargetEntity>(object source)
        {
            TargetEntity target = Activator.CreateInstance<TargetEntity>();
            Mapp<object, TargetEntity>(source, target);
            return target;
        }


        /// <summary>
        /// according source entity mapping  target entity
        /// </summary>
        /// <typeparam name="T">target entity</typeparam>
        /// <typeparam name="F">source entity</typeparam>
        /// <param name="source">source entity</param>
        /// <returns>return target entity</returns>
        public virtual F Mapp<T, F>(T source) where F : new()
        {
            F target = new F();
            Mapp(source, target);
            return target;
        }

        /// <summary>
        /// according source entity mapping  target entity
        /// </summary>
        /// <param name="destination"></param>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination">target entity</typeparam>
        /// <param name="obj">source entity</param>
        /// <returns>return target entity</returns>
        public virtual TDestination Map<TSource, TDestination>(TSource obj, TDestination destination)
        {
            Mapp(obj, destination);
            return destination;
        }

        /// <summary>
        /// according source entity mapping  target entity
        /// </summary>
        /// <typeparam name="F">source entity</typeparam>
        /// <typeparam name="T">target entity</typeparam>
        /// <param name="source">source entity</param>
        /// <param name="target">target entity</param>
        public virtual void Mapp<T, F>(T source, F target)
        {
            Type sourceType = source.GetType();
            Type targetType = target.GetType();
            foreach (PropertyInfo property in sourceType.GetProperties())
            {
                PropertyInfo propertyInfo = targetType.GetProperty(property.Name);
                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(target, property.GetValue(source, null),null);
                }
            }
        }
    }
}
#endif