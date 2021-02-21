namespace Utility.Net.Http
{
    /// <summary>
    /// HTTP 请求返回结果类型
    /// </summary>
    public enum HttpRetunnResult
    {
        /// <summary>
        /// 返回文本类型
        /// </summary>
        String = 1,
        /// <summary>
        /// 返回字节类型
        /// </summary>
        Byte = 2,
        /// <summary>
        /// 返回流类型
        /// </summary>
        Stream = 3
    }
}
