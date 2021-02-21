namespace Utility.ObjectMapping
{
    /// <summary>
    /// object mapp interface
    /// </summary>
    public interface IObjectMapper
    {
        /// <summary>
        /// according source entity mapping  target entity
        /// </summary>
        /// <typeparam name="SoucreEntity">source entity</typeparam>
        /// <typeparam name="TargetEntity">target entity</typeparam>
        /// <param name="source">source entity</param>
        /// <returns>return target entity</returns>
        TargetEntity Map<SoucreEntity, TargetEntity>(SoucreEntity source);

        /// <summary>
        /// according source entity mapping  target entity
        /// </summary>
        /// <typeparam name="TargetEntity">target entity</typeparam>
        /// <param name="source">source entity</param>
        /// <returns>return target entity</returns>
        TargetEntity Map<TargetEntity>(object source);

        /// <summary>
        /// according source entity mapping  target entity
        /// </summary>
        /// <typeparam name="F">source entity</typeparam>
        /// <typeparam name="T">target entity</typeparam>
        /// <param name="source">source entity</param>
        /// <param name="target">target entity</param>
        F Map<T, F>(T source, F target);
    }
}
