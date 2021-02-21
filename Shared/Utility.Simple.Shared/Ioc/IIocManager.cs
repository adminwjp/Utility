namespace Utility.Ioc
{
    /// <summary>
    /// ioc manager
    /// </summary>
    public interface IIocManager
    {
        /// <summary>
        /// add transitent
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="ImplT"></typeparam>
        void AddTransient<T, ImplT>() where ImplT : class;

        /// <summary>
        /// add scope
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="ImplT"></typeparam>
        void AddScoped<T, ImplT>() where ImplT : class;

        /// <summary>
        /// add single instanc
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="ImplT"></typeparam>
        void SingleInstance<T, ImplT>() where ImplT : class;

        /// <summary>
        /// get object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Get<T>();
    }
}
