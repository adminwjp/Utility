using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
using System.Data;
#endif
using System.Reflection;
using System.Collections;
using Utility.Utils;
#if !(NET10 || NET20 || NET30 || NET35 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
using System.Runtime.Serialization.Json;
using System.Text.RegularExpressions;
#endif
namespace Utility.Json
{
    /// <summary>
    /// 统一 接口
    /// </summary>
    public interface IJson
    {

        /// <summary> json 字符串 转对象 </summary>
        /// <param name="json">json 字符串</param>
        /// <returns></returns>
        object ToObject(string json);

        /// <summary>  对象  转  json 字符串 </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        string ToJson(object obj);

        /// <summary>  对象  转  json 字符串 </summary>
        /// <param name="json">字符串</param>
         T ToObject<T>(string json);

     }
#if !(NET10 || NET20 || NET30 || NET35 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)

    /// <summary>
    /// DataContractJsonSerializer 实现
    /// </summary>
    public class DataContractJsonSerializerJson: IJson
    {  
         /// <summary>
        /// DataContractJsonSerializer 实现
        /// </summary>
        public static readonly IJson Json=new DataContractJsonSerializerJson();
        /// <summary> json 字符串 转对象 </summary>
        /// <param name="json">json 字符串</param>
        /// <returns></returns>
        public object ToObject(string json)
        {
            return JsonToObject<Dictionary<string,object>>(json, null);
        }

        /// <summary>  对象  转  json 字符串 </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public string ToJson(object obj)
        {
            return ObjectToJson(obj, null);
        }

        /// <summary>  对象  转  json 字符串 </summary>
        /// <param name="json">字符串</param>
        public T ToObject<T>(string json)
        {
            return JsonToObject<T>(json,null);
        }
        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="encod"></param>
        /// <returns></returns>
        public static string ObjectToJson<T>(T t, Encoding encod = null)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(T));
                json.WriteObject(ms, t);
                ms.Position = 0;
                byte[] data = new byte[ms.Length];
                ms.Read(data, 0, (int)data.Length);
                encod = encod ?? Encoding.UTF8;
                string jsonString = encod.GetString(ms.ToArray());
                ms.Close();
                string p = @"\\/Date\((\d+)\+\d+\)\\/";
                MatchEvaluator matchEvaluator = new MatchEvaluator(ConvertJsonDateToDateString);
                Regex reg = new Regex(p);
                jsonString = reg.Replace(jsonString, matchEvaluator);
                return jsonString;
            }
        }
        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <param name="encod"></param>
        /// <returns></returns>
        public static T JsonToObject<T>(string json, Encoding encod)
        {

            //将"yyyy-MM-dd HH:mm:ss"格式的字符串转为"\/Date(1294499956278+0800)\/"格式  
            string p = @"\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2}";
            MatchEvaluator matchEvaluator = new MatchEvaluator(ConvertDateStringToJsonDate);
            Regex reg = new Regex(p);
            json = reg.Replace(json, matchEvaluator);
            encod = encod ?? Encoding.UTF8;
            using (MemoryStream ms = new MemoryStream(encod.GetBytes(json)))
            {
                DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(T));
                return (T)dataContractJsonSerializer.ReadObject(ms);
            }
        }


        /// <summary> 
        /// 将Json序列化的时间由/Date(1294499956278+0800)转为字符串 
        /// </summary> 
        private static string ConvertJsonDateToDateString(Match m)
        {
            string result = string.Empty;
            DateTime dt = new DateTime(1970, 1, 1);
            dt = dt.AddMilliseconds(long.Parse(m.Groups[1].Value));
            dt = dt.ToLocalTime();
            result = dt.ToString("yyyy-MM-dd HH:mm:ss");
            return result;
        }

        /// <summary>  
        /// 将时间字符串转为Json时间 
        /// </summary> 
        private static string ConvertDateStringToJsonDate(Match m)
        {
            string result = string.Empty;
            DateTime dt = DateTime.Parse(m.Groups[0].Value);
            dt = dt.ToUniversalTime();
            TimeSpan ts = dt - DateTime.Parse("1970-01-01");
            result = string.Format("\\/Date({0}+0800)\\/", ts.TotalMilliseconds);
            return result;
        }
    }
#endif

    /// <summary>
    /// 自定义 解析 json
    /// </summary>
    public class DefaultJson
    {

        /// <summary>
        /// List转成json 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonName"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string ListToJson<T>(IList<T> list, string jsonName)
        {
#if !(NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
            StringBuilder Json = new StringBuilder();
            if (string.IsNullOrEmpty(jsonName))
                jsonName = list[0].GetType().Name;
            Json.Append("{\"" + jsonName + "\":[");
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    T obj = Activator.CreateInstance<T>();
                    System.Reflection.PropertyInfo[] pi = obj.GetType().GetProperties();
                    Json.Append("{");
                    for (int j = 0; j < pi.Length; j++)
                    {
                        Type type;
                        object o = pi[j].GetValue(list[i], null);
                        string v = string.Empty;
                        if (o != null)
                        {
                            type = o.GetType();
                            v = o.ToString();
                        }
                        else
                        {
                            type = typeof(string);
                        }

                        Json.Append("\"" + pi[j].Name.ToString() + "\":" + StringFormat(v, type));

                        if (j < pi.Length - 1)
                        {
                            Json.Append(",");
                        }
                    }
                    Json.Append("}");
                    if (i < list.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
            }
            Json.Append("]}");
            return Json.ToString();
#else
            return string.Empty;
#endif
        }

        /// <summary>
        /// List转成json 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string ListToJson<T>(IList<T> list)
        {
            object obj = list[0];
            return ListToJson(list,obj.GetType().Name);
        }

        /// <summary> 
        /// 对象转换为Json字符串 
        /// </summary> 
        /// <param name="jsonObject">对象</param> 
        /// <returns>Json字符串</returns> 
        public static string ObjToJson(object jsonObject)
        {
#if !(NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)

            StringBuilder jsonString = new StringBuilder();
            jsonString.Append("{");
            PropertyInfo[] propertyInfo = jsonObject.GetType().GetProperties();
            for (int i = 0; i < propertyInfo.Length; i++)
            {
                object objectValue = propertyInfo[i].GetGetMethod().Invoke(jsonObject, null);
                if (objectValue == null)
                {
                    continue;
                }
                StringBuilder value = new StringBuilder();
                if (objectValue is DateTime || objectValue is Guid || objectValue is TimeSpan)
                {
                    value.Append("\"" + objectValue.ToString() + "\"");
                }
                else if (objectValue is string)
                {
                    value.Append("\"" + objectValue.ToString() + "\"");
                }
                else if (objectValue is IEnumerable)
                {
                    value.Append(ToJson((IEnumerable)objectValue));
                }
                else
                {
                    value.Append("\"" + objectValue.ToString() + "\"");
                }
                jsonString.Append("\"" + propertyInfo[i].Name + "\":" + value + ","); ;
            }
            return jsonString.ToString().TrimEnd(',') + "}";
#else
            return string.Empty;
#endif
        }

        /// <summary> 
        /// 对象集合转换Json 
        /// </summary> 
        /// <param name="array">集合对象</param> 
        /// <returns>Json字符串</returns> 
        public static string ToJson(IEnumerable array)
        {
            string jsonString = "[";
            foreach (object item in array)
            {
                jsonString += ObjToJson(item) + ",";
            }
            if (jsonString.Length > 1)
            {
                jsonString.Remove(jsonString.Length - 1, jsonString.Length);
            }
            else
            {
                jsonString = "[]";
            }
            return jsonString + "]";
        }

        /// <summary> 
        /// 普通集合转换Json 
        /// </summary> 
        /// <param name="array">集合对象</param> 
        /// <returns>Json字符串</returns> 
        public static string ToArrayString(System.Collections.IEnumerable array)
        {
            string jsonString = "[";
            foreach (object item in array)
            {
                jsonString = ToJson(item.ToString()) + ",";
            }
            jsonString.Remove(jsonString.Length - 1, jsonString.Length);
            return jsonString + "]";
        }

#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
        /// <summary> 
        /// Datatable转换为Json 
        /// </summary> 
        /// <param name="dt">Datatable对象</param> 
        /// <returns>Json字符串</returns> 
        public static string ToJson(DataTable dt)
        {
            StringBuilder jsonString = new StringBuilder();
            jsonString.Append("[");
            DataRowCollection drc = dt.Rows;
            for (int i = 0; i < drc.Count; i++)
            {
                jsonString.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string strKey = dt.Columns[j].ColumnName;
                    string strValue = drc[i][j]?.ToString();
                    Type type = dt.Columns[j].DataType;
                    jsonString.Append("\"" + strKey + "\":");
                    strValue = StringFormat(strValue, type);
                    if (j < dt.Columns.Count - 1)
                    {
                        jsonString.Append(strValue + ",");
                    }
                    else
                    {
                        jsonString.Append(strValue);
                    }
                }
                jsonString.Append("},");
            }
            jsonString.Remove(jsonString.Length - 1, 1);
            jsonString.Append("]");
            if (jsonString.Length == 1)
            {
                return "[]";
            }
            return jsonString.ToString();
        }

        /// <summary>
        /// DataTable转成Json 
        /// </summary>
        /// <param name="jsonName"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToJson(System.Data.DataTable dt, string jsonName)
        {
            StringBuilder Json = new StringBuilder();
            if (string.IsNullOrEmpty(jsonName))
                jsonName = dt.TableName;
            Json.Append("{\"" + jsonName + "\":[");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Json.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        Type type = dt.Rows[i][j].GetType();
                        Json.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + StringFormat(dt.Rows[i][j] is DBNull ? string.Empty : dt.Rows[i][j].ToString(), type));
                        if (j < dt.Columns.Count - 1)
                        {
                            Json.Append(",");
                        }
                    }
                    Json.Append("}");
                    if (i < dt.Rows.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
            }
            Json.Append("]}");
            return Json.ToString();
        }


        /// <summary> 
        /// DataReader转换为Json 
        /// </summary> 
        /// <param name="dataReader">DataReader对象</param> 
        /// <returns>Json字符串</returns> 
        public static string ToJson(System.Data.IDataReader dataReader)
        {
            try
            {
                StringBuilder jsonString = new StringBuilder();
                jsonString.Append("[");

                while (dataReader.Read())
                {
                    jsonString.Append("{");
                    for (int i = 0; i < dataReader.FieldCount; i++)
                    {
                        Type type = dataReader.GetFieldType(i);
                        string strKey = dataReader.GetName(i);
                        string strValue = dataReader[i].ToString();
                        jsonString.Append("\"" + strKey + "\":");
                        strValue = StringFormat(strValue, type);
                        if (i < dataReader.FieldCount - 1)
                        {
                            jsonString.Append(strValue + ",");
                        }
                        else
                        {
                            jsonString.Append(strValue);
                        }
                    }
                    jsonString.Append("},");
                }
                if (!dataReader.IsClosed)
                {
                    dataReader.Close();
                }
                jsonString.Remove(jsonString.Length - 1, 1);
                jsonString.Append("]");
                if (jsonString.Length == 1)
                {
                    return "[]";
                }
                return jsonString.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary> 
        /// DataSet转换为Json 
        /// </summary> 
        /// <param name="dataSet">DataSet对象</param> 
        /// <returns>Json字符串</returns> 
        public static string ToJson(DataSet dataSet)
        {
            string jsonString = "{";
            foreach (DataTable table in dataSet.Tables)
            {
                jsonString += "\"" + table.TableName + "\":" + ToJson(table) + ",";
            }
            jsonString = jsonString.TrimEnd(',');
            return jsonString + "}";
        }
#endif
        /// <summary>
        /// 过滤特殊字符
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string String2Json(String s)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                char c = s.ToCharArray()[i];
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\""); break;
                    case '\\':
                        sb.Append("\\\\"); break;
                    case '/':
                        sb.Append("\\/"); break;
                    case '\b':
                        sb.Append("\\b"); break;
                    case '\f':
                        sb.Append("\\f"); break;
                    case '\n':
                        sb.Append("\\n"); break;
                    case '\r':
                        sb.Append("\\r"); break;
                    case '\t':
                        sb.Append("\\t"); break;
                    case '\v':
                        sb.Append("\\v"); break;
                    case '\0':
                        sb.Append("\\0"); break;
                    default:
                        sb.Append(c); break;
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 格式化字符型、日期型、布尔型
        /// </summary>
        /// <param name="str"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static string StringFormat(string str, Type type)
        {
            //if (type.IsAssignableFrom(typeof(Nullable<>))&&str==null)
            //{
            //    str = "null";
            //    return str;
            //}
            if (str == null)
            {
                str = "null";
                return str;
            }
            if (type != typeof(string) && string.IsNullOrEmpty(str))
            {
                str = "\"" + str + "\"";
            }
            else if (type == typeof(string))
            {
                str = String2Json(str);
                str = "\"" + str + "\"";
            }
            else if (TypeUtils.IsDateTime(type)|| TypeUtils.IsDateTimeOffset(type)|| TypeUtils.IsGuid(type))
            {
                str = "\"" + str + "\"";
            }
            else if (TypeUtils.IsBoolean(type))
            {
                str = str.ToLower();
            }
            else if (type == typeof(byte[]))
            {
                str = "null";
            }
            return str;
        }
    }
}
