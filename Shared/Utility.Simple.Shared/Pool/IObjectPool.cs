using System;

namespace Utility.Pool
{
    /// <summary>
    /// 
    /// </summary>
    public interface IObjectPool:IDisposable
    {     
        /// <summary>连接池最少数量 </summary>
        int MinPoolSize { get; }
        /// <summary>连接池最大数量 </summary>
        int MaxPoolSize { get; }

        /// <summary>连接对象空闲存活时间 100s</summary>
        double ActiveTime { get; }
        /// <summary>
        /// 连接池创建时间
        /// </summary>
        DateTime StartTime { get; }

        /// <summary> 目前在用连接对象数量 </summary>
        int UseCount { get; }

        /// <summary>目前连接对象的总数</summary>
        int Total { get; }
        //int MaxActive { get; set; }
        // int MinActive { get; set; }
        //int MaxIdea { get; set; }
        //int MinIdea { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
       
        object Get();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool Release(object obj);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IObjectPool Build();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IObjectPool<T> : IObjectPool where T:class
    {

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        new T Get();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool Release(T obj);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        new IObjectPool<T> Build();
    }
}
