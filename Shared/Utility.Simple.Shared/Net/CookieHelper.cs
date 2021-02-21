using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Text;
using Utility.Collections;
#if !NET20 && !NET30
    using System.Linq;
#endif

namespace Utility.Net
{
    /// <summary>
    /// cookie helper 
    /// </summary>
    public  class CookieHelper
    {
        #region cookie helper method
#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)

        /// <summary>
        ///   Cookie to string  parse 
        /// </summary>
        /// <param name="strs">string collection</param>
        /// <param name="cookieContainer">cookie container<see cref="CookieContainer"/></param>
        public static void UpdateCookie(IEnumerable<string> strs, CookieContainer cookieContainer)
        {
            foreach (var item in strs)
            {
                var cookieStrs = item.Split(';');
                var cookieInfo = cookieStrs[0].Split('=');
                Cookie cookie = new Cookie(name: cookieInfo[0], value: cookieInfo[1]);
                foreach (var str in cookieStrs)
                {
                    var line = str.Split('=');
                    var name = line[0].Replace(" ", "");
                    switch (name)
                    {
                        case "path": cookie.Path = line[1]; break;
                        case "domain": cookie.Domain = line[1]; break;
                        case "expires": if (DateTime.TryParse(line[1], out DateTime dt)) cookie.Expires = dt; break;
                        case "httponly": cookie.HttpOnly = true; break;
                        default: break;
                    }
                }
                if (string.IsNullOrEmpty(cookie.Domain) || string.IsNullOrEmpty(cookie.Path)) continue;
                else cookieContainer.Add(cookie);
            }
        }
#endif

#if !(NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4)
        /// <summary>
        ///  according CookieContainer get Cookie information
        /// </summary>
        /// <param name="cookieContainer">cookie container<see cref="CookieContainer"/></param>
        /// <returns></returns>
        public static IEnumerable<Cookie> GetCookie(CookieContainer cookieContainer)
        {

#if !(NET20 || NET30) &&!(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_5 || NETSTANDARD1_6)
            var data = cookieContainer.GetType().InvokeMember("m_domainTable", BindingFlags.NonPublic | BindingFlags.GetField
             | BindingFlags.Instance, null, cookieContainer, new object[] { }) as Hashtable;
            return (from object path in data.Values
                    select path.GetType().InvokeMember("m_list",
BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance, null, path, new object[] { })
as SortedList into cookies
                    from CookieCollection cook in cookies.Values
                    from Cookie c in cook
                    select c);
#elif NET20 || NET30  &&!(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_5 || NETSTANDARD1_6)
            var data = cookieContainer.GetType().InvokeMember("m_domainTable", BindingFlags.NonPublic | BindingFlags.GetField
             | BindingFlags.Instance, null, cookieContainer, new object[] { }) as Hashtable;
            foreach (var path in data.Values)
            {
                var cookies = path.GetType().InvokeMember("m_list",
BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance, null, path, new object[] { })
as SortedList;
                foreach (var item in cookies.Values)
                {
                    foreach (Cookie cook in ((CookieCollection)item))
                    {
                        yield return cook;
                    }
                }
            }
#else
             return (IEnumerable<Cookie>)null;
#endif

        }


        /// <summary>
        ///  according CookieContainer get Cookie string information
        /// </summary>
        /// <param name="cookieContainer">cookie container<see cref="CookieContainer"/></param>
        /// <returns></returns>
        public static List<string> GetListString(CookieContainer cookieContainer)
        {
            var strings = GetCookieStringCollection(GetCookie(cookieContainer));
            return strings is List<string>? (List<string>)strings : CollectionHelper.List<string>(GetCookieStringCollection(GetCookie(cookieContainer)));
        }
        
        /// <summary>
        /// 获取Cookie集合
        /// </summary>
        /// <param name="cookieContainer">cookie 容器<see cref="CookieContainer"/></param>
        /// <returns></returns>
        public static IEnumerable<string> GetCookieString(CookieContainer cookieContainer)
        {
            return GetCookieStringCollection(GetCookie(cookieContainer));
        }

#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
        /// <summary>
        ///  parse CookieCollection inforamtion of  CookieContainer inforamtion
        /// </summary>
        /// <param name="cookieContainer">cookie container<see cref="CookieContainer"/></param>
        /// <param name="cookieCollection">cookie collection<see cref="CookieCollection"/></param>
        public static void AddOrUpdateCookie(CookieContainer cookieContainer, CookieCollection cookieCollection)
        {
            Cookie temp = null;
            foreach (Cookie item in GetCookie(cookieContainer))
            {
                if ((temp = cookieCollection[item.Name]) != null)
                {
                    if (temp.Path != item.Path)
                    {
                        //!(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
                        cookieContainer.Add(temp);
                    }
                }
            }
        }
#endif

        /// <summary>
        /// resolver  CookieContainer to  cookie string
        /// <para>
        /// </para>
        /// </summary>
        /// <param name="cookieContainer">cookie container<see cref="CookieContainer"/></param>
        /// <returns></returns>
        public static string GetAllCookieString(CookieContainer cookieContainer)
        {
            StringBuilder sb = new StringBuilder();
            var cookies = GetAllCookie(cookieContainer);
            for (int i = 0; i < cookies.Count; i++)
            {
                var it = cookies[i];
                sb.Append(it.Name).Append("=").Append(it.Value);
                if (i != cookies.Count - 1) sb.Append(";");
            }
            string cookieStr = sb.ToString();
            return cookieStr;
        }

        /// <summary>
        ///  parse CookieContainer to  Cookie collection.
        /// </summary>
        /// <param name="cookieContainer">cookie 容器<see cref="CookieContainer"/></param>
        /// <returns></returns>
        public static List<Cookie> GetAllCookie(CookieContainer cookieContainer)
        {
            var cookies = GetCookie(cookieContainer);
            return  cookies is List<Cookie>?(List<Cookie>)cookies :new List<Cookie>(cookies);
        }
#endif

        /// <summary>
        ///  parse cookie  to  cookie string
        /// </summary>
        /// <param name="cookies"><see cref="Cookie"/>cookie collection</param>
        /// <returns></returns>
        public static IEnumerable<string> GetCookieStringCollection(IEnumerable<Cookie> cookies)
        {
            using (IEnumerator<Cookie> cs = cookies.GetEnumerator())
                while (cs.MoveNext())
                {
                    var cookie = cs.Current;
                    yield return $"{cookie.Name}={cookie.Value};domain={cookie.Domain};path={cookie.Path};expires={cookie.Expires}";
                }
        }

        #endregion cookie helper method
    }
}