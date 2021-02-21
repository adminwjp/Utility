using System;

namespace Utility.Attributes
{
    /// <summary>描述 </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public class HeaderAttribute : System.Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
    }
}
