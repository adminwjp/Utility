#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
using System;
using System.Text;
using Utility.Validate;

namespace Utility.Script
{
    /// <summary> js 调用帮助类 >net4.5  >netcore 2.0 > netstandard 2.0  </summary>
    public class JintReflect
    {
      
        private  object _engine;//声明Engine对象
        private static readonly Type _type= Type.GetType("Jint.Engine,Jint");


        /// <summary>
        /// 
        /// </summary>
        public JintReflect()
        {
            if (_type != null)
            {
                _engine = Activator.CreateInstance(_type);
                IsSupport = true;
            }
            else
            {
                IsSupport = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSupport { get; internal set; }

        /// <summary> 执行的脚本文件 单脚本执行 </summary>
        /// <param name="file"></param>
        ///<exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public JintReflect Add(string file)
        {
            if (IsSupport)
            {
                ValidateHelper.ValidateArgumentNull("file", file);
                _engine = _type.GetMethod("Execute").Invoke(_engine, new object[] { System.IO.File.ReadAllText(file, Encoding.UTF8) });
            }
            return this;
        }

        /// <summary> 执行函数 单脚本执行 </summary>
        /// <param name="funName">函数名称</param>
        /// <param name="objs">参数</param>
        ///<exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public object Execute(string funName, params object[] objs)
        {
            if (IsSupport)
            {
                ValidateHelper.ValidateArgumentNull("funName", funName);
                //ArgumentsUtils.CheckArgumentNull("objs", objs);
                return _type.GetMethod("Invoke").Invoke(_engine, new object[] { funName, objs });
            }
            return null;
        }

        /// <summary> 执行函数 多 单脚本执行 </summary>
        /// <param name="file">执行的脚本文件</param>
        /// <param name="funName">函数名称</param>
        /// <param name="objs">参数</param>
        ///<exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public object Execute(string file, string funName, params object[] objs)
        {
            if (IsSupport)
            {
                ValidateHelper.ValidateArgumentNull("file", file);
                ValidateHelper.ValidateArgumentNull("funName", funName);
                //ArgumentsUtils.CheckArgumentNull("objs", objs);
                object obj = Activator.CreateInstance(_type);
                obj = _type.GetMethod("Execute").Invoke(obj, new object[] { System.IO.File.ReadAllText(file, Encoding.UTF8) });
                return _type.GetMethod("Invoke").Invoke(obj, new object[] { funName, objs });
            }
            return null;
        }
        /// <summary> 执行的脚本文件 多 单脚本执行 </summary>
        /// <param name="file"></param>
        ///<exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public object Execute(string file)
        {
            if (IsSupport)
            {
                ValidateHelper.ValidateArgumentNull("file", file);
                object obj = Activator.CreateInstance(_type);
                return obj = _type.GetMethod("Execute").Invoke(obj, new object[] { System.IO.File.ReadAllText(file, Encoding.UTF8) });
            }
            return null;
        }

        /// <summary> 执行函数 多 单脚本执行 </summary>
        /// <param name="engine">执行的脚本对象</param>
        /// <param name="funName">函数名称</param>
        /// <param name="objs">参数</param>
        ///<exception cref="ArgumentNullException"></exception>
        ///<exception cref="NotSupportedException"></exception>
        /// <returns></returns>
        public object Execute(object engine, string funName, params object[] objs)
        {
            if (IsSupport)
            {
                ValidateHelper.ValidateArgumentObjectNull("engine", engine);
                ValidateHelper.ValidateArgumentNull("funName", funName);
                //ArgumentsUtils.CheckArgumentNull("objs", objs);
                return _type.GetMethod("Invoke").Invoke(engine, new object[] { funName, objs });
            }
            return null;
        }
    }
}
#endif