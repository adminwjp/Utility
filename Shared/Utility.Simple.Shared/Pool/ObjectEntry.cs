using System;

namespace Utility.Pool
{
    /// <summary>
    /// 
    /// </summary>
    public class ObjectEntry
    {
        /// <summary>
        /// 
        /// </summary>
        public object Object { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ObjectState State { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime IdeaTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double IdeaMilliseconds { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateOrUseTime { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectEntry<T> : ObjectEntry
    {
        /// <summary>
        /// 
        /// </summary>
        public new T Object { get; set; }
    }
}
