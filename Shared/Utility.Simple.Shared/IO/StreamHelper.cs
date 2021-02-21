using System.IO;

namespace Utility.IO
{
    /// <summary>
    /// 列 帮助 类
    /// </summary>
    public class StreamHelper
    {
        /// <summary>
        /// 将 流 转换 为 字节
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] GetBuffer(Stream stream)
        {
            using (MemoryStream ms=new MemoryStream())
            {
                byte[] data = new byte[255];
                int line = 0;
                while ((line = stream.Read(data, 0, data.Length)) > 0)
                {
                    ms.Write(data,0,line);
                }
//#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)

//                stream.Close();
//#endif
                stream.Dispose();
                return ms.ToArray();
            }
        }

        /// <summary>
        /// 将 流 转换 为 字符串
        /// </summary>
        /// <param name="stream">流</param>
        /// <returns></returns>
        public static string GetString(Stream stream)
        {
            using (StreamReader ms = new StreamReader(stream))
            {
                return ms.ReadToEnd();
            }
        }

        /// <summary>
        /// 将 字节 转换 为 流
        /// </summary>
        /// <param name="buffer">字节</param>
        /// <returns></returns>
        public static Stream  GetStream(byte[] buffer)
        {
            MemoryStream ms = new MemoryStream();
            {
                ms.Write(buffer, 0, buffer.Length);
                return ms;
            }
        }
    }
}
