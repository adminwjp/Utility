#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Utility.Reflect
{
    /// <summary>
    /// reflect util reflection implement
    /// </summary>
    public  class ReflectHelper
    {
        /// <summary>
        /// according to the  property name get property value
        /// </summary>
        /// <param name="obj">target object of property  </param>
        /// <param name="name">property name</param>
        /// <returns></returns>
        public static object GetValue(object obj,string name)
        {
            object val = obj.GetType().GetProperty(name).GetValue(obj, null);
            return val;
        }

        /// <summary>
        ///  according to the  property name set property value
        /// </summary>
        /// <param name="obj">target object of property  </param>
        /// <param name="name">property name</param>
        /// <param name="val">property value</param>
        /// <param name="parse">复杂数据类型</param>
        public static void SetValue(object obj, string name,object val,bool parse=false)
        {
            var pro = obj.GetType().GetProperty(name);
            if (pro != null)
            {
                if(parse)
                {
                    //匹配不上？
                    // if (pro.PropertyType.IsAssignableFrom(typeof(Nullable<>)))// pass but fail
                    // if (typeof(Nullable<>).IsAssignableFrom(pro.PropertyType)) // fail 
                    //#if NET10 || NET11 || NET20 || NET30 || NET35|| NET30
                    //if(pro.PropertyType.GenericTypeArguments!=null&& pro.PropertyType.GenericTypeArguments.Length==1)
                    if (pro.PropertyType.GetGenericTypeDefinition() != null)
                    {
                        //pro.SetValue(obj, Convert.ChangeType(val, Nullable.GetUnderlyingType(pro.PropertyTyp)), null);
                        pro.SetValue(obj, Convert.ChangeType(val, pro.PropertyType.GetGenericTypeDefinition()), null);
                    }
                    else
                    //#endif
                    {
                        pro.SetValue(obj, Convert.ChangeType(val, pro.PropertyType), null);
                    }
                }
                else
                {
                   pro.SetValue(obj, val, null);
                }
            }
        }

      

        /// <summary>
        /// according to the  property  set property value
        /// </summary>
        /// <param name="obj">target object of property</param>
        /// <param name="propertyInfo">property</param>
        /// <param name="val">property value</param>
        public static void SetValue(object obj, PropertyInfo propertyInfo, object val)
        {
            propertyInfo.SetValue(obj, val,null);
        }

        /// <summary>
        /// according the classType, get property collection 
        /// </summary>
        /// <param name="classType">class type</param>
        /// <returns></returns>

        public static PropertyInfo[] GetProperties(Type classType)
        {
            var pros = classType.GetProperties();//这样 写 基类的属性 都 获取到了 需要重新排序 不像 java(实现原理都不同 ) 
            Utility.Collections.Array<PropertyInfo> properties = new Collections.Array<PropertyInfo>(pros.Length);
            for (int i = 0; i < pros.Length; i++)
            {
                int j = ReflectHelper.SortCursions(-1, classType, pros[i].Name);
                if (j == -1)
                {
                    properties.Add(pros[i]);
                }
                else
                {
                    properties.Insert(j, pros[i]);
                }
            }
            //properties.InsertRange(0, type.GetProperties());
            //if (type.BaseType != null)
            //{
            //    CursionProperties(type.BaseType, properties);
            //}
            return properties.ToArray();
        }

        /// <summary>get the location for the insert,has base class to zero start autoincrement ,otherwise original location autoincrement   </summary>
        /// <param name="index">index</param>
        /// <param name="classType">class type</param>
        /// <param name="name">property name</param>
        /// <returns>return the location index for the insert</returns>
        public static int SortCursions(int index, Type classType, string name)
        {
            if (classType.BaseType != null && classType.BaseType.GetProperty(name) != null)
            {
                index++;
                var temp = SortCursions(-1, classType.BaseType, name);
                if (temp == -1)
                    return index;
            }
            return -1;
        }

#if !(NET20 || NET30 || NET35 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)

        /// <summary>
        /// 不做任何操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T Set<T>(dynamic data)where T : class
        {
            return default(T);
        }
#endif
        /// <summary>
        /// 绑定 对象 属性 的值 
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <param name="propertyNames">属性名称,属性名必须存在</param>
        /// <param name="datas">数据值</param>
        /// <returns></returns>
        public static T Bind<T>(string[] propertyNames, object[] datas)
        {
            if (datas.Length == propertyNames.Length && propertyNames.Length > 0)
            {
                Type type = typeof(T);
                T obj = (T)Activator.CreateInstance(type);
                for (int i = 0; i < propertyNames.Length; i++)
                {
                    type.GetProperty(propertyNames[i]).SetValue(obj, datas[i],null);
                }
                return obj;
            }
            return default(T);
        }

        /// <summary>
        /// 获取 静态 方法
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="paramterType"></param>
        /// <returns></returns>
        public static MethodInfo GetMethodInfo(Type type,string name,Type paramterType)
        {
            if (type!= null)
            {
                MethodInfo method = null;
                foreach (MethodInfo methodInfo in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.InvokeMethod))
                {
                    if (methodInfo.Name == name)
                    {
                        bool has = false;
                        foreach (ParameterInfo parameterInfo in methodInfo.GetParameters())
                        {
                            if (parameterInfo.ParameterType == paramterType)
                            {
                                has = true;
                                break;
                            }
                        }
                        if (!has)
                        {
                            method = methodInfo;
                            break;
                        }
                    }
                }
                if (method == null)
                {
                    method = GetMethodInfo(type.BaseType, name, paramterType);
                }
                return method;
            }
            return null;
        }
    }
}
#endif