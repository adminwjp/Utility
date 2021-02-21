namespace Utility.Net.Http
{
    /// <summary>
    /// 编码 类型
    /// </summary>
    public struct ContentTypeConstant
    {
        #region  contetnType 常量
        /// <summary>
        /// json 请求类型
        /// </summary>
        public const string APPLICATION_JSON = "application/json";
        /// <summary>
        /// form 请求类型
        /// </summary>
        public const string APPLICATION_X_WWW_FORM_URLENCODED = "application/x-www-form-urlencoded";
        /// <summary>
        /// 文件表单 请求类型
        /// </summary>
        public const string APPLICATION_Multipart = "multipart/form-data";
        #endregion  contetnType 常量
    }
}
