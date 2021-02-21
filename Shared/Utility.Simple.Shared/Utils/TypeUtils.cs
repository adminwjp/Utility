using System;
using System.Collections;
using System.Collections.Generic;
#if !(NET10  || NET20 || NET30 || NET35)
using System.Linq;
#endif

namespace Utility.Utils
{
    /// <summary>
    /// type util by reflection 
    /// </summary>
    public class TypeUtils
    {
        /// <summary>
        /// ICollection Generic type
        /// </summary>
        public static readonly Type CollectionType=typeof(ICollection<>);

        /// <summary>
        /// IList Generic type
        /// </summary>
        public static readonly Type ListType = typeof(IList<>);


#if !(NET20 || NET30 || NET35 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
        /// <summary>
        /// ISet Generic type
        /// </summary>
        public static readonly Type SetType = typeof(ISet<>);
#endif

        /// <summary>
        /// string type
        /// </summary>
        public static readonly Type StringType = typeof(string);

        /// <summary>
        /// char type
        /// </summary>
        public static readonly Type CharType = typeof(char);

        /// <summary>
        /// new System.Collections.Generic.List &lt; &gt;();
        /// </summary>
        /// <param name="type">Generic type</param>
        /// <returns>reuten List  object</returns>
        public static IList CreateList(Type type)
        {
            //string typeName = $"System.Collections.Generic.List`1[[{type.AssemblyQualifiedName}]]";
            //Type listyType = Type.GetType(typeName);
            Type listyType = typeof(List<>).MakeGenericType(type);
            return (IList)Activator.CreateInstance(listyType);
        }

        /// <summary>
        /// new System.Collections.Generic.HashSet &lt; &gt;();
        /// </summary>
        /// <param name="type">generic type</param>
        /// <returns>reuten HashSet  object</returns>
        public static ICollection CreateSet(Type type)
        {
            //string typeName = $"System.Collections.Generic.HashSet`1[[{type.AssemblyQualifiedName}]]";
            //Type setType = Type.GetType(typeName);
#if !(NET20 || NET30 )
            Type setType = typeof(HashSet<>).MakeGenericType(type);
            return (ICollection)Activator.CreateInstance(setType);
#else
            return null;
#endif
        }

#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
        /// <summary>
        /// type is IList Generic type?
        /// </summary>
        /// <param name="type">generic type</param>
        /// <returns>reuten true  for type is IList Generic type,if or return  false</returns>
        public static bool IsList(Type type)
        {
            bool implements = IsICollection(type, ListType);
            return implements;
        }

#if !(NET20 || NET30 || NET35 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)

        /// <summary>
        /// type is ISet Generic type?
        /// </summary>
        /// <param name="type">generic type</param>
        /// <returns>reuten true  for type is ISet Generic type,if or return  false</returns>
        public static bool IsSet(Type type)
        {
            bool implements = IsICollection(type, SetType);
            return implements;
        }
#endif

        /// <summary>
        /// IsPrimitive为true，表示是.net的原生类型，即基础类型，注意string类型，自定义的struct  class不是原生类型
        /// <para>IsClass 表示是实体类</para>
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static bool IsCustomClass(Type type)
        {
            bool implements = IsICollection(type, CollectionType);
            return !(type == null || type.IsPrimitive )&&type!=typeof(string)&& type.IsClass&&!implements;
        }

        /// <summary>
        /// type is collection  Generic  type?
        /// </summary>
        /// <param name="type">type</param>
        /// <param name="definitionType">generic type</param>
        /// <returns>reuten true  for type is  Generic type,if or return  false</returns>
        public static bool IsICollection(Type type,Type definitionType)
        {
#if !(NET10 || NET20 || NET30 || NET35)
            bool implements = type.GetInterfaces()
              .Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == definitionType);
            //不是泛型会异常
            /* var genericT = CollectionType.GetGenericArguments()[0]; // a generic type parameter, T.
             bool implements = CollectionType.MakeGenericType(genericT).IsAssignableFrom(type.MakeGenericType(genericT)); // willl be true
             //bool implements = CollectionType.MakeGenericType(genericT).GetTypeInfo().IsAssignableFrom(type.MakeGenericType(genericT).GetTypeInfo()); // willl be true.*/

            // bool implements =CollectionType.IsAssignableFrom(type)//无效
            return implements;
#else
            foreach (var x in type.GetInterfaces())
            {
                if (x.IsGenericType && x.GetGenericTypeDefinition() == CollectionType)
                    return true;
            }
            return false;
#endif

        }

        /// <summary>
        /// according enum type ,get enum value name collection
        /// </summary>
        /// <param name="type">enum type</param>
        /// <returns>return enum value name collection</returns>
        public static IEnumerable<string> GetFields(Type type)
        {
            foreach (var item in type.GetFields())
            {
                yield return item.Name;
            }
        }

        /// <summary>
        /// / according enum type ,get all enum value 
        /// </summary>
        /// <typeparam name="T">enum value </typeparam>
        /// <param name="obj">enum type</param>
        /// <returns>return  all enum value </returns>
        public static IEnumerable<T> GetFieldsByEnum<T>(T obj)
        {
            foreach (var item in obj.GetType().GetFields())
            {
                yield return (T)item.GetValue(obj);
            }
        }

        /// <summary>
        /// / according enum value ,get all enum value name and  enum value collection
        /// </summary>
        /// <param name="obj">enum value</param>
        /// <returns>return all enum value name and  enum value collection</returns>
        public static IEnumerable<EnumEntity> GetFieldsEnum(object obj)
        {
            foreach (var item in obj.GetType().GetFields())
            {
                yield return new EnumEntity() { Name=item.Name,Value=(int)item.GetValue(obj)};
            }
        }
#endif


        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsValueTypeDate(Type type)
        {
            return type == typeof(DateTime) || type == typeof(DateTimeOffset) || type == typeof(TimeSpan)
                || type == typeof(DateTime?) || type == typeof(DateTimeOffset?) || type == typeof(TimeSpan?);
        }

        /// <summary>
        /// type is 
        /// byte type or byte? type
        ///  or short type or short? type
        ///  or int type or int? type 
        ///  or long type or long? type
        ///  or  float type or float? type
        ///  or double type or double? type
        ///  or decimal type or decimal? type
        ///  or char type or char? type  or bool type or bool? type
        ///  or string type 
        ///  or TimeSpan type or TimeSpan? type
        ///  or DateTime type or DateTime? type
        ///  or DateTimeOffset type or DateTimeOffset? type 
        ///  or Guid type or Guid? type 
        ///  ?
        /// </summary>
        /// <param name="type">data type</param>
        /// <returns> 
        /// return true for type is 
        /// byte type or byte? type
        ///  or short type or short? type
        ///  or int type or int? type 
        ///  or long type or long? type
        ///  or  float type or float? type
        ///  or double type or double? type
        ///  or decimal type or decimal? type
        ///  or char type or char? type  or bool type or bool? type
        ///  or string type 
        ///  or TimeSpan type or TimeSpan? type
        ///  or DateTime type or DateTime? type
        ///  or DateTimeOffset type or DateTimeOffset? type
        ///   or Guid type or Guid? type ,if or return false.
        /// </returns>
        public static bool IsCommonType(Type type)
        {
            return IsNumber(type)||IsBoolean(type)|| IsString(type) || IsChar(type)
                ||IsDateTime(type)||IsDateTimeOffset(type)||IsTimeSpan(type)
                ||IsGuid(type)
#if !(  NETCOREAPP1_0 || NETCOREAPP1_1 ||NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
                || type.IsEnum
#endif
                ;
        }

        /// <summary>
        /// type is byte type or byte? type
        ///  or short type or short? type
        ///  or int type or int? type 
        ///  or long type or long? type
        ///  or  float type or float? type
        ///  or double type or double? type
        ///  or decimal type or decimal? type ?
        /// </summary>
        /// <param name="type">data type</param>
        /// <returns> 
        /// return true for type is byte type or  byte? type
        ///  or short type or short? type
        ///  or int type or int? type 
        ///  or long type or long? type
        ///  or  float type or float? type
        ///  or double type or double? type
        ///  or decimal type or decimal? type ? type,if or return false.
        /// </returns>
        public static bool IsNumber(Type type)
        {
            return IsDecimal(type)||IsInterger(type);
        }

        /// <summary>
        /// type is float type or float? type
        ///  or double type or double? type
        ///  or decimal type or decimal? type  ?
        /// </summary>
        /// <param name="type">data type</param>
        /// <returns> 
        /// return true for type is float type or float? type
        ///  or double type or double? type
        ///  or decimal type or decimal? type,if or return false.
        ///  </returns>
        public static bool IsDecimal(Type type)
        {
            return  type == typeof(float) || type == typeof(float?)
                || type == typeof(double) || type == typeof(double?)
                || type == typeof(decimal) || type == typeof(decimal?);
        }

        /// <summary>
        /// type is byte type or byte? type
        ///  or short type or short? type
        ///  or int type or int? type 
        ///  or long type or long? type ?
        /// </summary>
        /// <param name="type">data type</param>
        /// <returns> 
        /// return true for type is  byte type or byte? type
        ///  or short type or short? type
        ///  or int type or int? type 
        ///  or long type or long? type,if or return false.
        ///  </returns>
        public static bool IsInterger(Type type)
        {
            return type == typeof(byte) || type == typeof(byte?)
                || type == typeof(short) || type == typeof(short?)
                || type == typeof(int) || type == typeof(int?)
                || type == typeof(long) || type == typeof(long?)
                 || type == typeof(ushort) || type == typeof(ushort?)
                  || type == typeof(UInt32) || type == typeof(UInt32?)
                 || type == typeof(ulong) || type == typeof(ulong?);
        }

        /// <summary>
        /// type is TimeSpan type or TimeSpan? type ?
        /// </summary>
        /// <param name="type">data type</param>
        /// <returns> return true for type is TimeSpan type or TimeSpan? type,if or return false. </returns>
        public static bool IsTimeSpan(Type type)
        {
            return type == typeof(TimeSpan) || type == typeof(TimeSpan?);
        }

        /// <summary>
        /// type is DateTimeOffset type or DateTimeOffset? type ?
        /// </summary>
        /// <param name="type">data type</param>
        /// <returns> return true for type is DateTimeOffset type or DateTimeOffset? type,if or return false. </returns>
        public static bool IsDateTimeOffset(Type type)
        {
            return type == typeof(DateTimeOffset) || type == typeof(DateTimeOffset?);
        }

        /// <summary>
        /// type is DateTime type or DateTime? type ?
        /// </summary>
        /// <param name="type">data type</param>
        /// <returns> return true for type is  DateTime type or DateTime? type,if or return false. </returns>
        public static bool IsDateTime(Type type)
        {
            return type == typeof(DateTime) || type == typeof(DateTime?);
        }

        /// <summary>
        /// type is Guid type or Guid? type ?
        /// </summary>
        /// <param name="type">data type</param>
        /// <returns> return true for type is  Guid type or Guid? type,if or return false. </returns>
        public static bool IsGuid(Type type)
        {
            return type == typeof(Guid) || type == typeof(Guid?);
        }

        /// <summary>
        /// type is bool type or bool? type  ?
        /// </summary>
        /// <param name="type">data type</param>
        /// <returns> return true for  bool type or bool? type  ,if or return false. </returns>
        public static bool IsBoolean(Type type)
        {
            return type == typeof(bool) || type == typeof(bool?);
        }

        /// <summary>
        /// type is char type or char? type  ?
        /// </summary>
        /// <param name="type">data type</param>
        /// <returns> return true for  char type or char? type  ,if or return false. </returns>
        public static bool IsChar(Type type)
        {
            return type == typeof(char) || type == typeof(char?);
        }

        /// <summary>
        /// type is string type ?
        /// </summary>
        /// <param name="type">data type</param>
        /// <returns> return true for type is string type,if or return false. </returns>
        public static bool IsString(Type type)
        {
            return type == typeof(string);
        }
    }

    /// <summary>
    /// enum entity
    /// </summary>
    public class EnumEntity
    {
        /// <summary>
        /// enum value name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// enum value
        /// </summary>
        public int Value { get; set; }
    }
}
