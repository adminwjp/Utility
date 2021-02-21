using Utility.Security;

namespace Utility.Security.Extensions
{
#if !(NET20 || NET30 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
    /// <summary>
    /// 安全 加密扩展 不支持 netstandard 1.0 - 1.2
    /// </summary>
    public static class SecurityExtensions
    {
        /// <summary>
        /// str sha1 加密
        /// </summary>
        /// <param name="str">加密字符</param>
        /// <param name="defaultEncoding">编码类型</param>
        /// <returns></returns>
        public static string Sha1(this string str, string defaultEncoding = "utf-8")
        {
            return SecurityHelper.Sha1(str,defaultEncoding);
        }
        /// <summary>
        /// str sha1 加密
        /// </summary>
        /// <param name="str">加密字符</param>
        /// <param name="defaultEncoding">编码类型</param>
        /// <returns></returns>
        public static byte[] Sha1ToByte(this string str, string defaultEncoding = "utf-8")
        {
            return SecurityHelper.Sha1ToByte(str,defaultEncoding);
        }

        /// <summary>
        /// str md5 加密
        /// </summary>
        /// <param name="str">加密字符</param>
        /// <param name="defaultEncoding">编码类型</param>
        /// <returns></returns>
        public static string Md5(this string str, string defaultEncoding = "utf-8")
        {
            return SecurityHelper.Md5(str,defaultEncoding);
        }

        /// <summary>
        /// str md5 加密
        /// </summary>
        /// <param name="str">加密字符</param>
        /// <param name="defaultEncoding">编码类型</param>
        /// <returns></returns>
        public static byte[] Md5ToByte(this string str, string defaultEncoding = "utf-8")
        {
           return SecurityHelper.Md5ToByte(str,defaultEncoding);
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="encryptString">加密字符串</param>
        /// <param name="key">密匙</param>
        /// <param name="iv">向量</param>
        /// <param name="defaultEncoding">默认编码utf-8</param>
        /// <returns></returns>
        public static byte[] AesEncryptToByte(this string encryptString, string key , string iv , string defaultEncoding = "utf-8")
        {
            return SecurityHelper.AesEncryptToByte(encryptString,key,iv,defaultEncoding);
        }
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="encryptString">加密字符串</param>
        /// <param name="key">密匙</param>
        /// <param name="iv">向量</param>
        /// <param name="defaultEncoding">默认编码utf-8</param>
        /// <returns></returns>
        public static string AesEncrypt(this string encryptString, string key , string iv, string defaultEncoding = "utf-8")
        {
             return SecurityHelper.AesEncrypt(encryptString,key,iv,defaultEncoding);
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="inputData">解密字节</param>
        /// <param name="key">密匙</param>
        /// <param name="iv">向量</param>
        /// <param name="defaultEncoding">默认编码utf-8</param>
        /// <returns></returns>
        public static byte[] AesDecryptToByte(this byte[] inputData, string key, string iv , string defaultEncoding = "utf-8")
        {
             return SecurityHelper.AesDecryptToByte(inputData,key,iv,defaultEncoding);
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="decryptString">解密字符串</param>
        /// <param name="key">密匙</param>
        /// <param name="iv">向量</param>
        /// <param name="defaultEncoding">默认编码utf-8</param>
        /// <returns></returns>
        public static string AesDecrypt(this string decryptString, string key ,string iv, string defaultEncoding = "utf-8")
        {
              return SecurityHelper.AesDecrypt(decryptString,key,iv,defaultEncoding);
        }
#if NET472 ||NET48
        #region sha

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] Sha1Cng(this byte[] buffer)
        {
            return SecurityHelper.Sha1Cng(buffer);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] Sha256Cng(this byte[] buffer)
        {
            return SecurityHelper.Sha256Cng(buffer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] Sha384Cng(this byte[] buffer)
        {
          return SecurityHelper.Sha384Cng(buffer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] Sha512Cng(this byte[] buffer)
        {
          return SecurityHelper.Sha512Cng(buffer);
        }
        #endregion sha

#endif
        #region sha


        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] Sha1Managed(this byte[] buffer)
        {
            return SecurityHelper.Sha1Managed(buffer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] Sha1CryptoServiceProvider(this byte[] buffer)
        {
            return SecurityHelper.Sha1CryptoServiceProvider(buffer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] Sha256(this byte[] buffer)
        {
            return SecurityHelper.Sha256(buffer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] Sha256Managed(this byte[] buffer)
        {
            return SecurityHelper.Sha256Managed(buffer);
        }
#if !(NET35)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] Sha256CryptoServiceProvider(this byte[] buffer)
        {
            return SecurityHelper.Sha256CryptoServiceProvider(buffer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] Sha384(this byte[] buffer)
        {
            return SecurityHelper.Sha384(buffer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] Sha384CryptoServiceProvider(this byte[] buffer)
        {
            return SecurityHelper.Sha384CryptoServiceProvider(buffer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] Sha512CryptoServiceProvider(this byte[] buffer)
        {
            return SecurityHelper.Sha512CryptoServiceProvider(buffer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] Sha512Managed(this byte[] buffer)
        {
            return SecurityHelper.Sha512Managed(buffer);
        }
#endif

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] Sha384Managed(this byte[] buffer)
        {
            return SecurityHelper.Sha384Managed(buffer);
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] Sha512(this byte[] buffer)
        {
            return SecurityHelper.Sha512(buffer);
        }
     
        #endregion sha 

        #region HASH

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] Hash(this byte[] buffer)
        {
            return SecurityHelper.Hash(buffer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] Hmac(this byte[] buffer)
        {
            return SecurityHelper.Hmac(buffer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] HmacMD5(this byte[] buffer)
        {
            return SecurityHelper.HmacMD5(buffer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] HmacSHA1(this byte[] buffer)
        {
            return SecurityHelper.HmacSHA1(buffer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] HmacSHA256(this byte[] buffer)
        {
            return SecurityHelper.HmacSHA256(buffer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] HmacSHA384(this byte[] buffer)
        {
            return SecurityHelper.HmacSHA384(buffer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] HmacSHA512(this byte[] buffer)
        {
            return SecurityHelper.HmacSHA512(buffer);
        }
        #endregion HASH

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] Md5CryptoServiceProvider(this byte[] buffer)
        {
            return SecurityHelper.Md5CryptoServiceProvider(buffer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="xmlString"></param>
        /// <param name="defaultEncoding"></param>
        /// <returns></returns>
        public static byte[] RsaEncryptToByte(this string data, string xmlString, string defaultEncoding = "utf-8")
        {
            return SecurityHelper.RsaEncryptToByte(data, xmlString, defaultEncoding);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="xmlString"></param>
        /// <param name="defaultEncoding"></param>
        /// <returns></returns>
        public static byte[] RsaDecryptToByte(this string data, string xmlString, string defaultEncoding = "utf-8")
        {
            return SecurityHelper.RsaDecryptToByte(data, xmlString, defaultEncoding);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="xmlString"></param>
        /// <param name="defaultEncoding"></param>
        /// <returns></returns>
        public static string RsaEncrypt(this string data, string xmlString, string defaultEncoding = "utf-8")
        {
            return SecurityHelper.RsaEncrypt(data, xmlString, defaultEncoding);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="xmlString"></param>
        /// <param name="defaultEncoding"></param>
        /// <returns></returns>
        public static string RsaDecrypt(this string data, string xmlString, string defaultEncoding = "utf-8")
        {
            return SecurityHelper.RsaDecrypt(data, xmlString, defaultEncoding);
        }
    }
#endif
}
