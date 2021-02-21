using System;
using System.Collections.Generic;

namespace Utility.Validate
{
    /// <summary> param validate </summary>
    public class ValidateHelper
    {
        /// <summary>
        /// 通用 参数 为null 错误信息
        /// </summary>
        public const string ChineseParamNotNull = "参数不能为空";
        /// <summary> check string wether is null or null string of string delete space </summary>
        /// <param name="name">string variable name</param>
        /// <param name="value">string variable value</param>
        /// <exception cref="ArgumentNullException">string wether is null throw ArgumentNullException</exception>
        public static void ValidateArgumentNull(string name,string value)
        {
            if (string.IsNullOrEmpty(value?.Trim()))
            {
                throw new ArgumentNullException(name);
            }
        }

        /// <summary>check cite type  wether is null </summary>
        /// <param name="name"> cite type variable name</param>
        /// <param name="value">cite type variable value</param>
        /// <exception cref="NullReferenceException">cite type wether is null throw NullReferenceException</exception>
        public static void ValidateArgumentObjectNull<T>(string name, T value)where T:class
        {
            if (value==null)
            {
                throw new  NullReferenceException(name);
            }
        }

        /// <summary> check array wether is null or length equal zero  </summary>
        /// <param name="name">array variable name</param>
        /// <param name="value">array variable value</param>
        /// <exception cref="ArgumentNullException">array wether is null throw ArgumentNullException</exception>
        public static void ValidateArgumentArrayNull<T>(string name, T[] value)
        {
            if (value == null||value.Length==0)
            {
                throw new ArgumentNullException(name);
            }
        }

        /// <summary>
        /// 参数过滤
        /// </summary>
        /// <param name="args">参数</param>
        /// <param name="msg">提示信息</param>
        /// <returns></returns>
        public static Dictionary<string, string> ParamNotNull( Dictionary<string, string> args,string msg= ChineseParamNotNull)
        {
            Dictionary<string, string> datas = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> item in args)
            {
                if (string.IsNullOrEmpty(args[item.Key]))
                {
                    datas.Add(item.Key, msg);
                }
            }
            return datas;
        }

    }
}
