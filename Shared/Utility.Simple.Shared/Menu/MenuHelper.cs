#if !( NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
using System.Collections.Generic;
using System.Xml;

namespace Utility.Menu
{
    /// <summary>
    ///菜单帮助 类 用于 wpf winform
    /// </summary>
    public class MenuHelper
    {
        /// <summary>
        /// 加载菜单
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static List<MenuEntry> Load(string xml)
        {
            List<MenuEntry> menus = new List<MenuEntry>();
            XmlDocument document = new XmlDocument();
            document.Load(xml);//LoadXml 该方法从字符串中读取   XML
            if (document.DocumentElement.Name.Equals("Root"))
            {
           
                foreach (XmlNode xmlNode in document.DocumentElement.SelectNodes("Menu"))
                {
                    MenuEntry menu = new MenuEntry() { Header = xmlNode.Attributes["Header"].Value, TypeName = xmlNode.Attributes["TypeName"]?.Value,Children=new List<MenuEntry>() };
                    menus.Add(menu);
                    CursionChildrenMenu(menu, xmlNode);
                }
            }
            return menus;
        }

        /// <summary>
        /// 递归 更新 菜单信息
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="node"></param>
        public static void CursionChildrenMenu(MenuEntry menu,XmlNode node)
        {

            if (node.SelectSingleNode("Children") != null)
            {
                foreach (XmlNode childrenNode in node.SelectNodes("Children/Menu"))
                {
                    MenuEntry childrenMenu = new MenuEntry() { Header = childrenNode.Attributes["Header"].Value, TypeName = childrenNode.Attributes["TypeName"]?.Value,Children=new List<MenuEntry>() };
                    menu.Children.Add(childrenMenu);
                    CursionChildrenMenu(childrenMenu,childrenNode);
                }
            }
           
        }
    }
}
#endif