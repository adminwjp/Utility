using System.Text;
using System.IO;
using System.Collections.Generic;
using Utility.Collections;


namespace Utility.IO
{
    /// <summary>文件 公共类 不支持netstandard 1.0 - 1.2 </summary>
    public class FileHelper
    {
        private static  readonly object _obj = new object();//锁
        //private static readonly System.Threading.ReaderWriterLockSlim obj = new System.Threading.ReaderWriterLockSlim();

        /// <summary>文件 公共类 是否支持 </summary>
        public static bool IsSupport
        {
            get
            {
#if !(NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2)
                return true;
#else
                return false;
#endif


            }
        }
  
        /// <summary> 写入信息到文件 目录不存在则创建目录 不支持netstandard 1.0 - 1.2 </summary>
        /// <param name="path">路径</param>
        /// <param name="content">内容</param>
        /// <param name="defaultEncoding">默认编码utf-8</param>
        public static void WriteFile(string path, string content, string defaultEncoding="utf-8")
        {
            WriteFile(path, Encoding.GetEncoding(defaultEncoding).GetBytes(content));
        }

        /// <summary> 写入信息到文件 目录不存在则创建目录 不支持netstandard 1.0 - 1.2 </summary>
        /// <param name="path">路径</param>
        /// <param name="content">内容</param>
        public static void WriteFile(string path, byte[] content)
        {
#if !(NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2)
            lock (_obj)
            {
                FileInfo fileInfo = new FileInfo(path);
                if (!fileInfo.Directory.Exists)
                {
                    fileInfo.Directory.Create();
                }
                File.WriteAllBytes(path, content);
            }
#endif
        }

        /// <summary> 读取文件信息 文件存在则读取 不支持netstandard 1.0 - 1.2 </summary>
        /// <param name="path">路径</param>
        /// <param name="defaultEncoding">编码  默认utf-8 </param> 
        /// <returns></returns>
        public static string ReadFile(string path, string defaultEncoding = "utf-8")
        {
#if !(NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2)
            lock (_obj)
            {
                return File.Exists(path)? File.ReadAllText(path, Encoding.GetEncoding(defaultEncoding)):string.Empty;
            }
#else
            return string.Empty;
#endif
        }

        /// <summary> 读取文件信息 文件存在则读取 不支持netstandard 1.0 - 1.2 </summary>
        /// <param name="path">路径</param>
        /// <param name="defaultEncoding">编码  默认utf-8 </param> 
        /// <returns></returns>
        public static byte[] ReadByte(string path, string defaultEncoding = "utf-8")
        {
#if !(NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2)
            lock (_obj)
            {
                return File.Exists(path) ? File.ReadAllBytes(path) : null;
            }
#else
            return (byte[])null;
#endif
        }

        /// <summary> 创建目录 不支持netstandard 1.0 - 1.2 </summary>
        /// <param name="path">路径</param>
        /// <param name="isdel">是否存在目录，true存在删除再创建</param>
        public static void CreateDirectory(string path, bool isdel = false)
        {
#if !(NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2)
            if (Directory.Exists(path))
            {
                if (isdel)
                {
                    Directory.Delete(path);
                    Directory.CreateDirectory(path);
                }
            }
            else
            {
                Directory.CreateDirectory(path);
            }
#endif
        }

        /// <summary> 存在则复制文件 不支持netstandard 1.0 - 1.2 </summary>
        /// <param name="path">文件路径</param>
        /// <param name="copypath">复制文件路径</param>
        public static void CopyFile(string path, string copypath)
        {
#if !(NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2)
            if (File.Exists(path))
            {
                File.Copy(path, copypath);
            }
#endif
        }

        /// <summary> 存在则删除文件 不支持netstandard 1.0 - 1.2 </summary>
        /// <param name="path">文件路径</param>
        public static void DeleteFile(string path)
        {
#if !(NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2)
            if (File.Exists(path))
            {
                File.Delete(path);
            }
#endif
        }

        /// <summary>写入文件 默认utf-8 不支持netstandard 1.0 - 1.2</summary>
        /// <param name="filename">文件名</param>
        /// <param name="content">文件内容</param>
        /// <param name="defaultEncoding">编码  默认utf-8 </param> 
        public static void AppendAllText(string filename, string content, string defaultEncoding = "utf-8")
        {
#if !(NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2)
            lock (_obj)
            {
                File.AppendAllText(filename, content, Encoding.GetEncoding(defaultEncoding));
            }
#endif
        }

        /// <summary> 根据目录获取所有目录名 递归 不支持netstandard 1.0 - 1.2</summary>
        /// <param name="path">文件路径</param>
        public static List<string> GetDirectories(string path)
        {
#if !(NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2)
            if (Directory.Exists(path))
            {
                List<string> datas = new List<string>();
                datas.Add(path);
                foreach (var item in Directory.GetDirectories(path))
                {
                    datas.AddRange(GetDirectories(item));
                }
                return datas;
            }
#endif
            return CollectionHelper<string>.EmptyList;
        }

        /// <summary>根据目录获取当前目录下所有目录名  不支持netstandard 1.0 - 1.2 </summary>
        /// <param name="path">文件路径</param>
        public static List<string> GetCurrentDirectories(string path)
        {
#if !(NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2)
            return Directory.Exists(path)? new List<string>(Directory.GetDirectories(path)): CollectionHelper<string>.EmptyList;
#else
            return CollectionHelper<string>.EmptyList;
#endif
        }

        /// <summary> 根据目录获取所有文件 不支持netstandard 1.0 - 1.2 </summary>
        /// <param name="path">文件路径</param>
        public static List<string> GetFiles(string path)
        {
#if !(NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2)
            List<string> directories = GetDirectories(path);
            List<string> datas = new List<string>();
            foreach (var item in directories)
            {
                datas.AddRange(Directory.GetFiles(item));
            }
           // datas.AddRange(Directory.GetFiles(path));
            return datas;
#else
            return CollectionHelper<string>.EmptyList;
#endif
        }

        /// <summary> 根据目录获取当前目录下所有文件 不支持netstandard 1.0 - 1.2 </summary>
        /// <param name="path">文件路径</param>
        public static List<string> GetCurrentFiles(string path)
        {
#if !(NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2)
            return Directory.Exists(path)? new List<string>(Directory.GetFiles(path)): CollectionHelper<string>.EmptyList;
#else
            return CollectionHelper<string>.EmptyList;
#endif
        }
    }
}
