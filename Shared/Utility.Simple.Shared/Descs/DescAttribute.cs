using System;

namespace Utility.Descs
{
    /// <summary> 
    /// description attribute
    /// </summary>
    [AttributeUsage( AttributeTargets.All,AllowMultiple =true,Inherited =true)]
    public class DescAttribute:System.Attribute
    {

        /// <summary> 
        /// chinese description
        /// </summary>
        public string ChineseDesc { get; set; }


        /// <summary>
        /// english description 
        /// </summary>
        public string EnglishDesc { get; set; }
    }
}
