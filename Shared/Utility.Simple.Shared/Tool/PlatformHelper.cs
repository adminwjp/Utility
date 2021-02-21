
using System;
using System.Collections.Generic;
using System.Text;

namespace Tool
{
    /// <summary>
    /// 
    /// </summary>
    public class PlatformHelper
    {
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="platform"></param>
        /// <param name="platformCodition"></param>
        /// <returns></returns>
        public static string GetCodition(Platform platform,PlatformCodition platformCodition)
        {
            StringBuilder builder = new StringBuilder(100);
            if (platformCodition.Net.Enable)
            {
                Append( builder, platformCodition.Net, platform.Net, platformCodition.Or, platformCodition.CodeCompile);
            }
            if (platformCodition.Core.Enable)
            {
                Append(builder, platformCodition.Core, platform.Core, platformCodition.Or, platformCodition.CodeCompile);
            }
            if (platformCodition.Standard.Enable)
            {
                Append(builder, platformCodition.Standard, platform.Standard, platformCodition.Or, platformCodition.CodeCompile,false);
            }
            string result = builder.ToString();
            char[] trimChars;
            if (platformCodition.CodeCompile)
            {
                trimChars = new char[2];
                trimChars[0] = ' ';
                trimChars[1] = platformCodition.Or ? '|' : '&';
            }
            else
            {
                if (platformCodition.Or)
                {
                    trimChars = new char[3];
                    trimChars[0] = ' ';
                    trimChars[1] = 'O';
                    trimChars[2] = 'R';
                }
                else
                {
                    trimChars = new char[4];
                    trimChars[0] = ' ';
                    trimChars[1] = 'A';
                    trimChars[2] = 'N';
                    trimChars[2] = 'D';
                }
            }
            result = result.TrimEnd(trimChars);
            if (platformCodition.InputWay)
            {
                if (platformCodition.CodeCompile)
                {
                    result = $"#if {result} \r\n #endif";
                }
                else
                {
                    result = $"<ItemGroup Condition=\"{ result}\"> \r\n </ItemGroup>";
                }
            }
            return result;
        }

#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Platform GetPlatform()
        {
            Platform platform = new Platform();
            int i = 1;
            string[] netVersion = {"10","11" ,"20","30", "35", "40",   "45", "451",   "452", "46", "461", "462", "47", "471", "48" };
            foreach (var item in netVersion)
            {
                platform.Net.Add(new PlatformCategory() { Id = i, Name = $"NET{item}", ConditionName = $"net{item}" });
                i++;
            }
            string[] netcoreVersion = { "10", "11", "12", "20", "21", "22", "30", "31", "50" };
            foreach (var item in netcoreVersion)
            {
                platform.Core.Add(new PlatformCategory() { Id = i, Name =$"NETCOREAPP{item.Insert(1,"_")}", ConditionName = $"netcoreapp{item.Insert(1, ".")}" });
                i++;
            }
            string[] netstandardVersion = { "10", "11", "12", "13", "14", "15", "16", "20", "21" };
            foreach (var item in netstandardVersion)
            {
                platform.Standard.Add(new PlatformCategory() { Id = i, Name = $"NETSTANDARD{item.Insert(1, "_")}", ConditionName = $"netstandard{item.Insert(1, ".")}" });
                i++;
            }
            return platform;
        }
#endif
        private static void Append(StringBuilder builder,Value value, List<PlatformCategory> platformCategories, bool or, bool codeCompile = true,bool need=true)
        {
            Append(builder, value.Min, value.Max, platformCategories, or, codeCompile);
            if (need&&value.Min >= value.Max)
            {
                if (codeCompile)
                {
                    builder.Append(or ? " || " : "  && ");
                }
                else
                {
                    builder.Append(or? " OR " : "  AND ");
                }
            }
        }
        private static void Append(StringBuilder builder, int min, int max, List<PlatformCategory> platformCategories, bool or,  bool codeCompile = true)
        {
            if (max > min)
            {
                for (int i = 0; i < platformCategories.Count; i++)
                {
                    var id = platformCategories[i].Id;
                    if (id >= min && id <= max)
                    {
                        if (codeCompile)
                        {
                            builder.Append(platformCategories[i].Name);
                            if (or)
                                builder.Append(" || ");
                            else
                                builder.Append(" && ");
                        }
                        else
                        {
                            builder = builder.Append("'$(TargetFramework)' == '").Append(platformCategories[i].ConditionName).Append("'");
                            if (or)
                                builder.Append(" OR ");
                            else
                                builder.Append(" AND ");
                        }
                    }
                }
            }
            else
            {
                if (codeCompile)
                    builder.Append(GetConditionName(min,platformCategories));
                else builder.Append("'$(TargetFramework)' == '").Append(GetConditionName(min, platformCategories,false)).Append("'");
            }
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="platformCategories"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private static string GetConditionName(int id, List<PlatformCategory> platformCategories,bool name=true)
        {
            foreach (var item in platformCategories)
            {
                if (item.Id == id) return name? item.Name: item.ConditionName;
            }
            return string.Empty;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string ParseStr(string str)
        {
            if (str.Length <= 1)
            {
                return str;
            }
            else
            {
                if (str.Contains("_"))
                {
                    return str.Replace("_", ".");
                }
                else
                {
                    //char[] chars = new char[str.Length * 2 - 1];
                    //for (int i = 0; i < str.Length; i++)
                    //{
                    //    chars[i * 2] = str[i];
                    //    if (i == str.Length - 1) break;
                    //    chars[i * 2 + 1] = '.';
                    //}
                    //return new string(chars);
                  //net461 -  net4.6.1
                    return str;
                }
            }
        }

       
    }
    /// <summary>
    /// 
    /// </summary>
    public class Platform
    {
        /// <summary>
        /// 
        /// </summary>
        public List<PlatformCategory> Net { get; set; } = new List<PlatformCategory>();
        /// <summary>
        /// 
        /// </summary>
        public List<PlatformCategory> Core { get; set; } = new List<PlatformCategory>();
        /// <summary>
        /// 
        /// </summary>
        public List<PlatformCategory> Standard { get; set; } = new List<PlatformCategory>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="platformCategories"></param>
        /// <returns></returns>
        public List<PlatformCategory> Copy(List<PlatformCategory> platformCategories)
        {
            return new List<PlatformCategory>(platformCategories);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class PlatformCategory
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ConditionName { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class PlatformCodition
    {
        /// <summary>
        /// 
        /// </summary>
        public bool Or { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Value Net { get; set; } = new Value();
        /// <summary>
        /// 
        /// </summary>
        public Value Core { get; set; } = new Value();
        /// <summary>
        /// 
        /// </summary>
        public Value Standard { get; set; } = new Value();
        /// <summary>
        /// 
        /// </summary>
        public bool CodeCompile { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool InputWay { get; set; }

        
    }

    /// <summary>
    /// 
    /// </summary>
    public class Value
    {
        /// <summary>
        ///是否 代码编译条件
        /// </summary>
        public bool Enable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Min { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Max { get; set; }
    }
}
