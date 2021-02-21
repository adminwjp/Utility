using System;

namespace Utility.ObjectMapping
{
    /// <summary>
    ///<see cref="IObjectMapper"/>  interface  implement.
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    public class EmptyObjectMapper : IObjectMapper
    {
        /// <summary>
        /// new EmptyObjectMapper();
        /// </summary>
        public static readonly EmptyObjectMapper Empty = new EmptyObjectMapper();

        /// <summary>
        /// according source entity mapping  target entity
        /// </summary>
        /// <typeparam name="SoucreEntity">source entity</typeparam>
        /// <typeparam name="TargetEntity">target entity</typeparam>
        /// <param name="source">source entity</param>
        /// <returns>return target entity</returns>
        public TargetEntity Map<SoucreEntity, TargetEntity>(SoucreEntity source)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// according source entity mapping  target entity
        /// </summary>
        /// <typeparam name="TargetEntity">target entity</typeparam>
        /// <param name="source">source entity</param>
        /// <returns>return target entity</returns>

        public TargetEntity Map<TargetEntity>(object source)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// according source entity mapping  target entity
        /// </summary>
        /// <typeparam name="F">source entity</typeparam>
        /// <typeparam name="T">target entity</typeparam>
        /// <param name="source">source entity</param>
        /// <param name="target">target entity</param>
        public virtual F Map<T, F>(T source, F target)
        {
            throw new NotImplementedException();
        }
    }
}
