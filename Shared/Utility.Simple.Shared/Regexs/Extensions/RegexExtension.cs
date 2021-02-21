using System.Text.RegularExpressions;

namespace Utility.Regexs.Extensions
{
#if ! (NET20 || NET30 )
    /// <summary>
    /// 正则 扩展类
    /// </summary>
    public static class RegexExtension
    {
        /// <summary> 正则获取值 </summary>
        /// <param name="input">匹配的字符串</param>
        /// <param name="pattern">正则表达式</param>
        /// <param name="groupnum">获取的位置，索引从0开始</param>
        /// <param name="options">正则匹配模式</param>
        /// <returns></returns>
        public static string GetValue(this string input, string pattern, int groupnum = 1, RegexOptions options = RegexOptions.Singleline)
            => System.Text.RegularExpressions.Regex.Match(input, pattern, options).Groups[groupnum].Value;

        /// <summary> 正则匹配是否成功 </summary>
        /// <param name="input">匹配的字符串</param>
        /// <param name="pattern">正则表达式</param>
        /// <param name="options">正则匹配模式</param>
        /// <returns></returns>
        public static bool IsMatch(this string input, string pattern, RegexOptions options = RegexOptions.None) => System.Text.RegularExpressions.Regex.IsMatch(input: input, pattern: pattern, options: options);

        /// <summary>正则获取正则Match匹配对象</summary>
        /// <param name="input">匹配的字符串</param>
        /// <param name="pattern">正则表达式</param>
        /// <param name="options">正则匹配模式</param>
        /// <returns></returns>
        public static Match Match(this string input, string pattern, RegexOptions options = RegexOptions.None) => System.Text.RegularExpressions.Regex.Match(input: input, pattern: pattern, options: options);

        /// <summary>正则获取正则GroupCollection对象 ,即正则分组的信息 </summary>
        /// <param name="input">匹配的字符串</param>
        /// <param name="pattern">正则表达式</param>
        /// <param name="options">正则匹配模式</param>
        /// <returns></returns>
        public static GroupCollection Grgoups(this string input, string pattern, RegexOptions options = RegexOptions.None) => System.Text.RegularExpressions.Regex.Match(input: input, pattern: pattern, options: options).Groups;

        /// <summary>正则获取正则MatchCollection对象 ,即正则Match集合 </summary>
        /// <param name="input">匹配的字符串</param>
        /// <param name="pattern">正则表达式</param>
        /// <param name="options">正则匹配模式</param>
        /// <returns></returns>
        public static MatchCollection Matches(this string input, string pattern, RegexOptions options = RegexOptions.None) => System.Text.RegularExpressions.Regex.Matches(input: input, pattern: pattern, options: options);

    }
#endif
}
