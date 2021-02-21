#if !( NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET5_0 || NETSTANDARD2_0 || NETSTANDARD2_1)
using System;
using System.Runtime.InteropServices;

namespace Utility
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class AutoMapAttribute : Attribute
    {
        public Type Type { get; set; }

        public AutoMapAttribute(Type type)
        {
            Type = type;
        }
    }
}
#endif