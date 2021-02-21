using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Tool
{
    /// <summary>
    /// 
    /// </summary>
    public class StringHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="stringType"></param>
        /// <returns></returns>
        public static string GetString(string str,StringType stringType)
        {
            switch (stringType)
            {
                case StringType.Address:
                    return str.Replace("/", "\\").Replace("/", "\\"); ;
                case StringType.AddressSingle:
                    return str.Replace("//", "/").Replace("\\", "/");
                case StringType.StringSingle:
                    return Regex.Replace(str.Replace("\'", "\\\'").Replace("\"", "\\\'"), "[\\s|\t|\r|\n]+", " ");
                case StringType.String:
                default:
                    return Regex.Replace(str.Replace("\'", "\\\"").Replace("\"", "\\\""), "[\\s|\t|\r|\n]+", " ");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Clear(string str)
        {
            return Regex.Replace(str, "[\\s|\t|\r|\n]+", " ");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<StringEntity> Initial()
        {
            List<StringEntity> stringEntities = new List<StringEntity>();
            stringEntities.Add(new StringEntity() { StringType= StringType.Address,Name="地址正斜线"});
            stringEntities.Add(new StringEntity() { StringType = StringType.AddressSingle, Name = "地址反斜线" });
            stringEntities.Add(new StringEntity() { StringType = StringType.String, Name = "字符串双引号" });
            stringEntities.Add(new StringEntity() { StringType = StringType.StringSingle, Name = "字符串单引号" });
            return stringEntities;
        }

        /// <summary>
        /// 
        /// </summary>
        public enum StringType
        {
            /// <summary>
            /// 
            /// </summary>
            Address,
            /// <summary>
            /// 
            /// </summary>
            AddressSingle,
            /// <summary>
            /// 
            /// </summary>
            String,
            /// <summary>
            /// 
            /// </summary>
            StringSingle
        }

        /// <summary>
        /// 
        /// </summary>
        public class StringEntity
        {
            /// <summary>
            /// 
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public StringType StringType { get; set; }
        }
    }
}
