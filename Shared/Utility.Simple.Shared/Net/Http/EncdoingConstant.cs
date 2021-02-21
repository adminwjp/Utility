using System.Text;

namespace Utility.Net.Http
{
    /// <summary>
    /// 常用编码
    /// </summary>
    public  struct EncdoingConstant
    {
        #region 编码格式 常量

        /// <summary> UTF-8 编码格式 </summary>
        public const string UTF8 = "UTF-8";

        /// <summary> GBK 编码格式 </summary>
        public const string GBK = "GBK";

        /// <summary> GBK2312 编码格式</summary>
        public const string GBK2312 = "GBK2312";

        /// <summary> ISO8859-1 编码格式 </summary>
        public const string ISO88591 = "ISO8859-1";

        #endregion 编码格式 常量

        /// <summary>返回字符 编码 格式   </summary>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static Encoding GetEncoding(string encoding=UTF8) => Encoding.GetEncoding(encoding);
    }
}
