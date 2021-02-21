#if !(NET20 || NET30|| NET10 || NET11 || NETCOREAPP1_0 || NETCOREAPP1_1 ||NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
using System;
using System.Collections.Generic;
#if !(NET20 || NET30|| NET10 || NET11)
using System.Linq;
#endif

namespace Utility.EventBus.Extensions
{
    /// <summary>
    /// 泛型 类型 扩展类
    /// </summary>
    public static class GenericTypeExtensions
    {
        /// <summary>
        /// 获取泛型 类型 名称
        /// </summary>
        /// <param name="type">泛型类型</param>
        /// <returns></returns>
        public static string GetGenericTypeName(this Type type)
        {
            var typeName = string.Empty;

            if (type.IsGenericType)
            {
#if !(NET20 || NET30|| NET10 || NET11)
                //var genericTypes = string.Join(",", type.GetGenericArguments().Select(t => t.Name).ToArray());
#endif
                var genericTypes = new List<string>();
                foreach (var item in type.GetGenericArguments())
                {
                    genericTypes.Add(item.Name);
                }
                typeName = $"{type.Name.Remove(type.Name.IndexOf('`'))}<{genericTypes}>";
            }
            else
            {
                typeName = type.Name;
            }

            return typeName;
        }

        /// <summary>
        /// 获取对象名称
        /// </summary>
        /// <param name="object">对象</param>
        /// <returns></returns>

        public static string GetGenericTypeName(this object @object)
        {
            return @object.GetType().GetGenericTypeName();
        }
    }
}
#endif