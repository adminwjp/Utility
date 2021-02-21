namespace Utility.Net.Http
{
    /// <summary>   HTTP 请求协议 </summary>
    public enum HttpMethod
    {
        /// <summary>  代表 HTTP GET 协议方法 ў。 </summary>
        GET,
        /// <summary>表示用来替换实体由 URI 标识的 HTTP PUT 协议方法。</summary>
        PUT,
        /// <summary> 示用于将新实体添加作为发布到的 URI 的 HTTP POST 协议方法。 </summary>
        POST,
        /// <summary> 代表 HTTP DELETE 协议方法 ў。 </summary>
        DELETE,
        /// <summary> 表示 HTTP H e a d 协议方法。 HEAD 方法等同于 GET 只是服务器仅在响应中，但不包括消息正文中返回消息头。 </summary>
        HEAD,
        /// <summary>表示 HTTP 选项协议方法。</summary>
        OPTIONS,
        /// <summary> 表示 HTTP 跟踪协议方法。</summary>
        TRACE
    }
}
