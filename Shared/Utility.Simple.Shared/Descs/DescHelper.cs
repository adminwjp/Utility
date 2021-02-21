#if !(NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
using System.Collections.Generic;
using System.Reflection;
using Utility.Enums;
using Utility.Validate;
using Utility.Utils;

namespace Utility.Descs
{

    /// <summary> description attribute  helper </summary>
    public class DescHelper
    {
        /// <summary>
        /// cache enum  Code state information
        /// </summary>
#if NET10 || NET11 || NET20 || NET30 || NET35
      public static readonly IDictionary<string, DescAttribute> CodeDescCache = new Collections.ConcurrentCollection<string, DescAttribute>();
#else
        public static readonly IDictionary<string, DescAttribute> CodeDescCache = new System.Collections.Concurrent.ConcurrentDictionary<string, DescAttribute>();
#endif

        /// <summary>
        /// load  enum  Code state information
        /// </summary>
        static DescHelper()
        {
            InitCode();
        }

        /// <summary>
        /// load  enum  Code state information
        /// </summary>
        private static void InitCode()
        {
            foreach (var item in typeof(Code).GetFields())
            {
#if !(NET20 || NET30 || NET35 || NET40)
                var desc = item.GetCustomAttribute<DescAttribute>();
#else
                var desc=AttributeUtils.Get<DescAttribute>(item.GetCustomAttributes(true));
#endif
                if(desc != null)
                {
                    //怎么像执行 了 多遍 ArgumentException: The key already existed in the dictionary
                    string code = item.GetValue(Code.Success).ToString();
                    if (!CodeDescCache.ContainsKey(code))
                        CodeDescCache.Add(code, desc);
                }
            }
        }

        /// <summary>
        /// according enum value get  DescAttribute
        /// </summary>
        /// <param name="val">enum value</param>
        /// <returns>return DescAttribute for  according enum value get  DescAttribute success,if or return null. </returns>
        public static DescAttribute GetDescAttribute(object val)
        {
            ValidateHelper.ValidateArgumentObjectNull("obj", val);
            int code = (int)val;
            foreach (var item in val.GetType().GetFields())
            {
                object res = item.GetValue(val);
                if ((int)res == code)
                {
#if !(NET20 || NET30 || NET35 || NET40)
                    var desc = item.GetCustomAttribute<DescAttribute>();
#else
                var desc=AttributeUtils.Get<DescAttribute>(item.GetCustomAttributes(true));
#endif
                    if (desc == null) continue;
                    return desc;
                }
            }
            return (DescAttribute)null;
        }

    }

}
#endif