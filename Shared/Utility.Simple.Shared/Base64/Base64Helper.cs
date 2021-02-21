using System;

namespace Utility.Base64
{
    /// <summary>
    /// base64 helper
    /// </summary>
    public class Base64Helper
    {
        /// <summary>base64 decrypt </summary>
        /// <param name="str">decrypt string</param>
        /// <returns>return null if string is null or null string ,if or return base64 decrypt </returns>
        public static byte[] FromBase64String(string str)
        {
            return string.IsNullOrEmpty(str) ? (byte[])null : Convert.FromBase64String(str);
        }

        /// <summary> base64 encrypt </summary>
        /// <param name="data">encrypt  byte</param>
        /// <returns>return null if byte array is null ,if or return base64 encrypt</returns>
        public static string Base64String(byte[] data)
        {
            return data == null ? string.Empty : Convert.ToBase64String(data);
        }
    }
}
