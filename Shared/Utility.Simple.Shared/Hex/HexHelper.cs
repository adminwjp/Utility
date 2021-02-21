using System;
#if !(NET20 || NET30 || NET35)
using System.Linq;
#endif
using System.Text;

namespace Utility.Hex
{
    /// <summary>
    /// 16 进制 帮助 类
    /// </summary>
    public class HexHelper
    { 
       /// <summary> 将字节转换为16进制字符串 </summary>
       /// <param name="data">字节</param>
       /// <returns></returns>
        public static string ToHex(byte[] data)
        {
            if (data == null)
            {
                return string.Empty;
            }
            else
            {
#if !(NET20 || NET30 || NET35)
                return string.Join("", data.Select(it => it.ToString("x2")));
#else
                StringBuilder sb = new StringBuilder();
                    foreach (var item in data)
                    {
                        sb.Append(item.ToString("x2"));
                    }
                    return sb.ToString();
#endif
            }
        }

        /// <summary> 将16进制字符串转换为字节 </summary>
        /// <param name="hex">16进制字符串</param>
        /// <returns></returns>
        public static byte[] ToByte(string hex)
        {
            if (string.IsNullOrEmpty(hex))
            {
                return null;
            }
            else
            {
                hex = hex.Replace(" ", "");
                if (hex.Length % 2 != 0) hex += " ";
                byte[] buffer = new byte[hex.Length / 2];
                for (int i = 0; i < hex.Length / 2; i++)
                {
                    buffer[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
                }
                return buffer;
            }
        }
    }
}
