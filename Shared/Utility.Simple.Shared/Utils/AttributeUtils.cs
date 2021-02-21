using System;
using System.Collections.Generic;
using System.Reflection;

namespace Utility.Utils
{
    /// <summary>特性 公共类 </summary>
    public class AttributeUtils
    {
        /// <summary>获取对应的特性信息 </summary>
        /// <typeparam name="T">特性</typeparam>
        /// <param name="objs">特性集合</param>
        /// <returns>返回对应的特性信息</returns>
        public static T Get<T>(object[] objs)where T : Attribute
        {
            foreach (var item in objs)
            {
                if (item is T) return (T)item;
            }
            return (T)null;
        }

        /// <summary>获取对应的Exists特性信息 不需要NotExists特性信息  </summary>
        /// <typeparam name="Exists">特性</typeparam>
        ///  <typeparam name="NotExists">特性</typeparam>
        /// <param name="objs">特性集合</param>
        /// <returns>返回对应的Exists特性信息</returns>
        public static IEnumerable<Exists> Get<Exists, NotExists>(object[] objs) where Exists : Attribute where NotExists : Attribute
        {
            foreach (var item in objs)
            {
                if (item is NotExists) continue;
                if (item is Exists)  yield return (Exists)item;
            }
        }

        /// <summary>
        /// 获取对应的Exists特性信息 不需要NotExists特性信息
        /// </summary>
        /// <typeparam name="Exists"></typeparam>
        /// <typeparam name="NotExists"></typeparam>
        /// <param name="objs"></param>
        /// <returns></returns>
        public static List<NotExists> GetList<Exists, NotExists>(object[] objs) where Exists : Attribute where NotExists : Attribute
        {
            IEnumerable<NotExists> notExists= Get<NotExists, Exists>(objs);
            if(notExists is List<NotExists> list)
            {
                return list;
            }
            return new List<NotExists>(notExists);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="Exists"></typeparam>
        /// <typeparam name="Exists1"></typeparam>
        /// <typeparam name="NotExists"></typeparam>
        /// <param name="objs"></param>
        /// <returns></returns>
        public static List<NotExists> GetList<Exists, Exists1, NotExists>(object[] objs) where Exists : Attribute where Exists1 : Attribute where NotExists : Attribute
        {
            List<NotExists> datas = new List<NotExists>();
            foreach (var item in objs)
            {
                if (item is Exists|| item is Exists1) continue;
                if (item is NotExists) datas.Add((NotExists)item);
            }
            return datas;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objs"></param>
        /// <returns></returns>
        public static bool Exists<T>(object[] objs) where T : Attribute
        {
            if (Get<T>(objs) != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <param name="objs"></param>
        /// <returns></returns>
        public static bool Exists<T,T1>(object[] objs) where T : Attribute where T1 :Attribute
        {
            foreach (var item in objs)
            {
                if (item is T|| item is T1) return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="objs"></param>
        /// <returns></returns>
        public static bool Exists<T, T1,T2>(object[] objs) where T : Attribute where T1 : Attribute where T2 : Attribute
        {
            foreach (var item in objs)
            {
                if (item is T || item is T1 ||  item is T2) return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <param name="objs"></param>
        /// <returns></returns>
        public static bool Exists<T, T1, T2,T3>(object[] objs) where T : Attribute where T1 : Attribute where T2 : Attribute where T3 : Attribute
        {
            foreach (var item in objs)
            {
                if (item is T || item is T1 || item is T2
                     || item is T3) return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <param name="objs"></param>
        /// <returns></returns>
        public static bool Exists<T, T1, T2, T3,T4>(object[] objs) where T : Attribute where T1 : Attribute 
            where T2 : Attribute where T3 : Attribute where T4 : Attribute
        {
            foreach (var item in objs)
            {
                if (item is T || item is T1 || item is T2
                       || item is T3 || item is T4) return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <param name="objs"></param>
        /// <returns></returns>
        public static bool Exists<T, T1, T2, T3, T4,T5>(object[] objs) where T : Attribute where T1 : Attribute
    where T2 : Attribute where T3 : Attribute where T4 : Attribute where T5 : Attribute
        {
            foreach (var item in objs)
            {
                if (item is T || item is T1 || item is T2
                      || item is T3 || item is T4
                      || item is T5) return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <param name="objs"></param>
        /// <returns></returns>
        public static bool Exists<T, T1, T2, T3, T4, T5,T6>(object[] objs) where T : Attribute where T1 : Attribute
where T2 : Attribute where T3 : Attribute where T4 : Attribute where T5 : Attribute where T6 : Attribute
        {
            foreach (var item in objs)
            {
                if (item is T || item is T1 || item is T2
                      || item is T3 || item is T4
                      || item is T5 || item is T6) return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <param name="objs"></param>
        /// <returns></returns>
        public static bool Exists<T, T1, T2, T3, T4, T5, T6,T7>(object[] objs) where T : Attribute where T1 : Attribute
where T2 : Attribute where T3 : Attribute where T4 : Attribute where T5 : Attribute where T6 : Attribute where T7 : Attribute
        {
            foreach (var item in objs)
            {
                if (item is T || item is T1 || item is T2
                      || item is T3 || item is T4
                      || item is T5 || item is T6
                       || item is T7) return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <typeparam name="T8"></typeparam>
        /// <param name="objs"></param>
        /// <returns></returns>
        public static bool Exists<T, T1, T2, T3, T4, T5, T6, T7,T8>(object[] objs) where T : Attribute where T1 : Attribute
where T2 : Attribute where T3 : Attribute where T4 : Attribute where T5 : Attribute where T6 : Attribute where T7 : Attribute
             where T8 : Attribute
        {
            foreach (var item in objs)
            {
                if (item is T || item is T1 || item is T2
                      || item is T3 || item is T4
                      || item is T5 || item is T6
                       || item is T7 || item is T8) return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <typeparam name="T8"></typeparam>
        /// <typeparam name="T9"></typeparam>
        /// <param name="objs"></param>
        /// <returns></returns>
        public static bool Exists<T, T1, T2, T3, T4, T5, T6, T7, T8,T9>(object[] objs) where T : Attribute where T1 : Attribute
where T2 : Attribute where T3 : Attribute where T4 : Attribute where T5 : Attribute where T6 : Attribute where T7 : Attribute
     where T8 : Attribute where T9 : Attribute
        {
            foreach (var item in objs)
            {
                if (item is T || item is T1 || item is T2
                      || item is T3 || item is T4
                      || item is T5 || item is T6
                       || item is T7 || item is T8
                       || item is T9) return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetEnumAttribute<T>(Type type) where T : System.Attribute
        {
#if !NETSTANDARD1_0 && !NETSTANDARD1_1 && !NETSTANDARD1_2 && !NETSTANDARD1_3 && !NETSTANDARD1_4 && !NETSTANDARD1_5 && !NETSTANDARD1_6
            foreach (var item in type.GetFields())
            {
#if !NET20 && !NET30 && !NET35 && !NET40
                T attribute = (T)item.GetCustomAttribute(typeof(T));
                if (attribute != null) yield return attribute;
#else
                foreach (var attr in item.GetCustomAttributes(typeof(T), false))
                {
                    if (attr is T attribute)
                    {
                        yield return attribute;
                    }
                }
#endif

            }
#else
            yield return (T)null;
#endif
        }
    }
}
