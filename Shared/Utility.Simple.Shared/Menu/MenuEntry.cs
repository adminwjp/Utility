using System.Collections.Generic;
using System;

namespace Utility.Menu
{
    /// <summary>
    /// 菜单 
    /// </summary>
    public class MenuEntry
    {
        /// <summary>菜单名称 </summary>
        public string Header { get; set; }
        /// <summary>控件类型名称 反射创建控件 </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Flag { get; set; }
        /// <summary>
        ///  构造函数参数
        /// </summary>
        public List<ConstractorEntry> Arags { get; set; }
        /// <summary>
        /// 子集
        /// </summary>
        public List<MenuEntry> Children { get; set; }
        /// <summary>
        /// 构造函数参数
        /// </summary>
        /// <returns></returns>
        public object[] ToConstractorArags()
        {
            if (Arags != null && Arags.Count > 0)
            {
                object[] objs = new object[Arags.Count];
                for (int i = 0; i < Arags.Count; i++)
                {
                    objs[i] = Arags[i].Value;
                }
                return objs;
            }
            return null;
        }
        /// <summary>
        /// 点击 事件
        /// </summary>
        public Action<object> ClickEvent { get; set; }
    }
}
