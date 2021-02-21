using Utility.Descs;

namespace Utility.Enums
{
    /// <summary> 语言 </summary>
    public enum Language
    {
        /// <summary>
        /// 中文
        /// </summary>
        [Desc(ChineseDesc = "中文", EnglishDesc = "Chinese")]
        Chinese,

        /// <summary>
        /// 英文
        /// </summary>
        [Desc(ChineseDesc = "英文", EnglishDesc = "English")]
        English
    }
}
