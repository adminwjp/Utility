namespace Utility.Pool
{
    /// <summary>对象使用状态 </summary>
    public enum ObjectState
    {
        /// <summary>无状态 </summary>
        None,
        
        /// <summary>使用中 </summary>
        Active,

        /// <summary>空闲中 </summary>
        Idea,

        /// <summary>未知情况 </summary>
        Unkow,
    }
}
