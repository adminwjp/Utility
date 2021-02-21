using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Utility.Json;

namespace Utility.Utils
{
    /// <summary>
    /// string util
    /// </summary>
    public class StringUtils
    {
        /// <summary> 空字符串 </summary>
        public const string Empty ="";

        /// <summary>
        /// 
        /// </summary>
        public static readonly Type Type = typeof(string);

        /// <summary> 是否是char 字符 </summary>
        /// <param name="type">类型</param>
        /// <returns>是char 字符 返回 true 否则 false </returns>
        public static bool IsChar(Type type)
        {
            return type == typeof(char) || type == typeof(char?);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsEmpty(string str)
        {
            return str == null || str == Empty;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsBank(string str)
        {
            return str == null || str == Empty || str.Trim()==Empty;
        }

        /// <summary>字符串转换 </summary>
        /// <param name="str">字符串</param>
        /// <param name="format">转换格式</param>
        /// <returns>返回转换后字符串</returns>
        public static  string Parse(string str, StringFormat format)
        {
            switch (format)
            {
                case StringFormat.Lower:
                    return str.ToLower();
                case StringFormat.Upper:
                    return str.ToUpper();
                case StringFormat.InitialLetterUpperCaseLower:
                    {
                        StringBuilder builder = new StringBuilder(str.Length + 10);
                        for (int i = 0; i < str.Length; i++)
                        {
                            if (i == 0)
                                builder.Append(char.ToLower(str[i]));
                            else if (char.IsUpper(str[i]))
                            {
                                builder.Append("_").Append(char.ToLower(str[i]));
                            }
                            else
                            {
                                builder.Append(str[i]);
                            }
                        }
                        str = builder.ToString();
                        return str;
                    }
                case StringFormat.InitialLetterLowerCaseUpper:
                    {
                        StringBuilder builder = new StringBuilder(str.Length);
                        for (int i = 0; i < str.Length;)
                        {
                            if (i == 0)
                                builder.Append(char.ToUpper(str[i]));
                            else if (str[i] == '_')
                            {
                                i++;
                                builder.Append(char.ToUpper(str[i]));
                            }
                            else
                            {
                                builder.Append(str[i]);
                            }
                            i++;
                        }
                        str = builder.ToString();
                        return str;
                    }
                case StringFormat.InitialLetterLower:
                    if (char.IsUpper(str[0]))
                    {
                        char[] chars = str.ToCharArray();
                        chars[0] = char.ToLower(chars[0]);
                        return new string(chars);
                    }
                    return str;
                case StringFormat.InitialLetterUpper:
                    if (char.IsLower(str[0]))
                    {
                        char[] chars = str.ToCharArray();
                        chars[0] = char.ToUpper(chars[0]);
                        return new string(chars);
                    }
                    return str;
                case StringFormat.None:
                default:
                    return str;
            }
        }

        /// <summary> 获取对应字符串转换</summary>
        /// <param name="stringFormat">转换格式</param>
        /// <returns>返回对应转换字符串</returns>
        public static StringFormat Get(StringFormat stringFormat)
        {
            StringFormat format = StringFormat.None;
            switch (stringFormat)
            {
                case StringFormat.Lower:
                    format = StringFormat.Upper;
                    break;
                case StringFormat.Upper:
                    format = StringFormat.Lower;
                    break;
                case StringFormat.InitialLetterUpperCaseLower:
                    format = StringFormat.InitialLetterLowerCaseUpper;
                    break;
                case StringFormat.InitialLetterLowerCaseUpper:
                    format = StringFormat.InitialLetterUpperCaseLower;
                    break;
                case StringFormat.InitialLetterLower:
                    format = StringFormat.InitialLetterUpper;
                    break;
                case StringFormat.InitialLetterUpper:
                    format = StringFormat.InitialLetterLower;
                    break;
                case StringFormat.None:
                default:
                    break;
            }
            return format;
        }

        /// <summary> 获取字符串不同之处的位置 </summary>
        /// <param name="str1">字符串</param>
        /// <param name="str2">比较字符串</param>
        /// <returns>返回字符串不同之处的位置</returns>
        public static int GetDiffIndex(string str1,string str2)
        {
            if (str2.Length > str1.Length)
            {
                return str1.Length;
            }
            for (int i = 0; i <str1.Length&&i<str2.Length; i++)
            {
                if (!str1[i].Equals(str2[i]) )
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// trim spce \r \n
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Trim(string str)
        {
            return Regex.Replace(str, "[\\s\r\n]+", "");
        }
        /// <summary>
        /// 字符串unicode转换
        /// <para>
        /// 不支持 netstandard 1.0 - 1.2
        /// </para>
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <returns></returns>
        public static string Unicode2String(string source)
        {
#if !(NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 )
            return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(source, x => Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)).ToString());
#else
             return string.Empty;
#endif
        }
        /// <summary>
        /// guid string 
        /// </summary>
        public static string Id { get { return Guid.NewGuid().ToString("N"); } }

        /// <summary>
        /// 判断<paramref name="str"/>是否符合<paramref name="strs"/>,都符合则true
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="strs">条件</param>
        /// <returns></returns>
        public static bool StartWithAll( string str, IEnumerable<string> strs)
        {
            foreach (var item in strs)
            {
                if (!str.StartsWith(item))
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 判断<paramref name="str"/>是否符合<paramref name="strs"/>,符合则true
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="strs">条件</param>
        /// <returns></returns>
        public static bool StartWithAny( string str, IEnumerable<string> strs)
        {
            foreach (var item in strs)
            {
                if (str.StartsWith(item))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 将字符串转为为<see cref="Dictionary{TKey, TValue}"/>,格式：
        /// <code>
        /// a=b nbsp; c=d
        /// </code>
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static Dictionary<string, string> ToDictionary( string str)
        {
            Dictionary<string, string> pairs = new Dictionary<string, string>();
            foreach (var item in str.Split('&'))
            {
                var strs = item.Split('=');
                pairs.Add(strs[0], strs[1]);
            }
            return pairs;
        }

        /// <summary>
        /// 解析键值集合对象转换为键值对象，如{"a":"{\"b\":\"c\"}"} => {"a":{b:"c"}}
        /// </summary>
        /// <param name="data">键值对象</param>
        /// <returns></returns>
        public static Dictionary<string, object> ParseDictionary( Dictionary<string, object> data)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            foreach (KeyValuePair<string, object> keypair in data)
            {
                ParseObject(result, keypair);
            }
            return result;
        }
        private static void ParseObject(Dictionary<string, object> result, KeyValuePair<string, object> keypair)
        {
            string json = keypair.Value.ToString().Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
            if (json.StartsWith("["))
            {
                if (json.Equals("[]"))
                {
                    result.Add(keypair.Key, keypair.Value);
                }
                else
                {
                    json = json.TrimStart('[').TrimEnd(']').Trim();
                    //对象数组
                    if (json.StartsWith("{"))
                    {
                        List<Dictionary<string, object>> keyValuePairs = new List<Dictionary<string, object>>();
                        List<object> objs = JsonHelper.ToObject<List<object>>($"[{json}]");
                        foreach (object str in objs)
                        {
                            Dictionary<string, object> res = new Dictionary<string, object>();
                            Dictionary<string, object> temp = JsonHelper.ToObject<Dictionary<string, object>>(str.ToString());
                            foreach (KeyValuePair<string, object> item in temp)
                            {
                                ParseObject(res, item);
                            }
                            keyValuePairs.Add(res);
                        }
                        result.Add(keypair.Key, keyValuePairs);
                    }
                    else
                    {
                        List<string> objs = JsonHelper.ToObject<List<string>>(json);
                        result.Add(keypair.Key, objs);
                    }
                }
            }
            else if (json.StartsWith("{"))
            {
                Dictionary<string, object> res = new Dictionary<string, object>();
                Dictionary<string, object> temp = JsonHelper.ToObject<Dictionary<string, object>>(json);
                foreach (KeyValuePair<string, object> item in ParseDictionary(temp))
                {
                    ParseObject(res, item);
                }
                result.Add(keypair.Key, res);
            }
            else
            {
                result.Add(keypair.Key, keypair.Value);
            }
        }

        /// <summary>
        /// url unicode
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string UnicodeUrl( string url)
        {
            if (string.IsNullOrEmpty(url)) { throw new ArgumentException($"url{Enums.Code.ParamNotNull.ToString()}"); }
            else
            {
                StringBuilder stringBuilder = new StringBuilder(url.Length * 2);
                foreach (var item in url)
                {
                    switch (item)
                    {
                        case ',': stringBuilder.Append("%2c"); break;
                        default:
                            break;
                    }
                }
                return stringBuilder.ToString();
            }
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public enum StringFormat
    {
        /// <summary> 默认字符 aBc aBc </summary>
        None,

        /// <summary> 小写 aBc abc </summary>
        Lower,

        /// <summary> 大写 aBc ABC </summary>
        Upper,

        /// <summary> 首字母大写转小写 其他大写字母转横线加小写字母 aBc a_bc </summary>
        InitialLetterUpperCaseLower,

        /// <summary>  首字母小写转 大写 其他横线加小写字母转大写字母  a_bc aBc </summary>
        InitialLetterLowerCaseUpper,

        /// <summary> 首字母大写 </summary>
        InitialLetterUpper,

        /// <summary>  首字母小写 </summary>
        InitialLetterLower
    }
}
