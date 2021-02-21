#if NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Utility.AspNetCore.Extensions
{
    /// <summary>
    /// 模型扩展类
    /// </summary>
    public static  class ModelValidateExtensions
    {
        /// <summary>
        /// 模型 错误 转换
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public static IDictionary<string, List<string>> Errors(this ModelStateDictionary dictionary)
        {
            IDictionary<string, List<string>> errors = new Dictionary<string, List<string>>();
            using (IEnumerator<string> keys = (IEnumerator<string>)dictionary.Keys.GetEnumerator())
            {
                using (IEnumerator<ModelStateEntry> values = (IEnumerator<ModelStateEntry>)dictionary.Values.GetEnumerator())
                {
                    while (keys.MoveNext() && values.MoveNext())
                    {
                        string key = keys.Current;
                        errors.Add(key, new List<string>());
                        foreach (var item in values.Current.Errors)
                        {
                            errors[key].Add(item.ErrorMessage);
                        }
                    }
                }
            }
            return errors;
        }

        /// <summary>
        /// 模型 错误 转换
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public static IDictionary<string, List<string>> Errors(this SerializableError dictionary)
        {
            IDictionary<string, List<string>> errors = new Dictionary<string, List<string>>();
            using (IEnumerator<string> keys = (IEnumerator<string>)dictionary.Keys.GetEnumerator())
            {
                using (IEnumerator<object> values = (IEnumerator<object>)dictionary.Values.GetEnumerator())
                {
                    while (keys.MoveNext() && values.MoveNext())
                    {
                        string key = keys.Current;
                        errors.Add(key, new List<string>());
                        errors[key].AddRange(values.Current as string[]);
                    }
                }
            }
            return errors;
        }
    }
}
#endif