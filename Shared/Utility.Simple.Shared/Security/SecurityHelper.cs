#if !( NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
using System;
using System.Security.Cryptography;
using System.Text;
using Utility.Base64;
using Utility.Hex;

namespace Utility.Security
{
    /// <summary>
    /// 安全 加密帮助类 不支持 netstandard 1.0 - 1.2
    /// </summary>
    public static class SecurityHelper
    {

        /// <summary>
        /// str sha1 加密
        /// </summary>
        /// <param name="str">加密字符</param>
        /// <param name="defaultEncoding">编码类型</param>
        /// <returns></returns>
        public static string Sha1(string str, string defaultEncoding = "utf-8")
        {
            return HexHelper.ToHex(Sha1ToByte(str, defaultEncoding));
        }
        /// <summary>
        /// str sha1 加密
        /// </summary>
        /// <param name="str">加密字符</param>
        /// <param name="defaultEncoding">编码类型</param>
        /// <returns></returns>
        public static byte[] Sha1ToByte(string str, string defaultEncoding = "utf-8")
        {
            if (string.IsNullOrEmpty(str))
            {
                return (byte[])null;
            }
            else
            {
                using (SHA1 sHA1 = SHA1.Create())
                {
                    return sHA1.ComputeHash(Encoding.GetEncoding(defaultEncoding).GetBytes(str));
                }
            }
        }

        /// <summary>
        /// str md5 加密
        /// </summary>
        /// <param name="str">加密字符</param>
        /// <param name="defaultEncoding">编码类型</param>
        /// <returns></returns>
        public static string Md5(string str, string defaultEncoding = "utf-8")
        {
            return HexHelper.ToHex(Md5ToByte(str, defaultEncoding));
        }

        /// <summary>
        /// str md5 加密
        /// </summary>
        /// <param name="str">加密字符</param>
        /// <param name="defaultEncoding">编码类型</param>
        /// <returns></returns>
        public static byte[] Md5ToByte(string str, string defaultEncoding = "utf-8")
        {
            if (string.IsNullOrEmpty(str))
            {
                return (byte[])null;
            }
            else
            {
                using (MD5 mD5 = MD5.Create())
                {
                    return mD5.ComputeHash(Encoding.GetEncoding(defaultEncoding).GetBytes(str));
                }
            }
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="encryptString">加密字符串</param>
        /// <param name="key">密匙</param>
        /// <param name="iv">向量</param>
        /// <param name="defaultEncoding">默认编码utf-8</param>
        /// <returns></returns>
        public static byte[] AesEncryptToByte(string encryptString, string key, string iv, string defaultEncoding = "utf-8")
        {
#if !(NET20 || NET30 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
            if (string.IsNullOrEmpty(encryptString))
            {
                return (byte[])null;
            }
            else
            {
                //using (RijndaelManaged rijndaelProvider = new RijndaelManaged()) //加密可以,解密错误
                using (Aes rijndaelProvider = new AesCryptoServiceProvider())
                {
                    byte[] keyTemp =Encoding.GetEncoding(defaultEncoding).GetBytes(key);
                    byte[] keyBuffer = new byte[32];
                    //keyBuffer = Encoding.GetEncoding(defaultEncoding).GetBytes(key.Substring(0, 32));
                    Array.Copy(keyTemp, 0, keyBuffer, 0, keyTemp.Length > 32 ? 32 : keyTemp.Length);
                    rijndaelProvider.Key =keyBuffer;
                    byte[] buffer = new byte[16];
                    byte[] temp = Encoding.GetEncoding(defaultEncoding).GetBytes(iv);
                    Array.Copy(temp, 0, buffer, 0, temp.Length > 16 ? 16 : temp.Length);
                    rijndaelProvider.IV = buffer;
                    using (ICryptoTransform rijndaelEncrypt = rijndaelProvider.CreateEncryptor())
                    {
                        byte[] inputData = Encoding.GetEncoding(defaultEncoding).GetBytes(encryptString);
                        return rijndaelEncrypt.TransformFinalBlock(inputData, 0, inputData.Length);
                    }
                }
            }
#else
            return (byte[])null;
#endif
        }
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="encryptString">加密字符串</param>
        /// <param name="key">密匙</param>
        /// <param name="iv">向量</param>
        /// <param name="defaultEncoding">默认编码utf-8</param>
        /// <returns></returns>
        public static string AesEncrypt(string encryptString, string key, string iv, string defaultEncoding = "utf-8")
        {
            return Base64Helper.Base64String(AesEncryptToByte(encryptString, key, iv, defaultEncoding));
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="inputData">解密字节</param>
        /// <param name="key">密匙</param>
        /// <param name="iv">向量</param>
        /// <param name="defaultEncoding">默认编码utf-8</param>
        /// <returns></returns>
        public static byte[] AesDecryptToByte(byte[] inputData, string key, string iv, string defaultEncoding = "utf-8")
        {
#if !(NET20 || NET30 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
            if (inputData == null)
            {
                return inputData;
            }
            //using (RijndaelManaged rijndaelProvider = new RijndaelManaged()) //加密可以,解密错误
            using (Aes rijndaelProvider = new AesCryptoServiceProvider())
            {
                byte[] keyTemp =Encoding.GetEncoding(defaultEncoding).GetBytes(key);
                byte[] keyBuffer = new byte[32];
                //keyBuffer = Encoding.GetEncoding(defaultEncoding).GetBytes(key.Substring(0, 32));
                Array.Copy(keyTemp, 0, keyBuffer, 0, keyTemp.Length > 32 ? 32 : keyTemp.Length);
                rijndaelProvider.Key = keyBuffer;
                byte[] buffer = new byte[16];
                byte[] temp = Encoding.GetEncoding(defaultEncoding).GetBytes(iv);
                Array.Copy(temp, 0, buffer, 0, temp.Length>16? 16: temp.Length);
                rijndaelProvider.IV = buffer;
                using (ICryptoTransform rijndaelEncrypt = rijndaelProvider.CreateDecryptor())
                {
                    return rijndaelEncrypt.TransformFinalBlock(inputData, 0, inputData.Length);
                }
            }
#else
            return (byte[])null;
#endif
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="decryptString">解密字符串</param>
        /// <param name="key">密匙</param>
        /// <param name="iv">向量</param>
        /// <param name="defaultEncoding">默认编码utf-8</param>
        /// <returns></returns>
        public static string AesDecrypt(string decryptString, string key, string iv, string defaultEncoding = "utf-8")
        {
            var data = AesDecryptToByte(Base64Helper.FromBase64String(decryptString), key, iv, defaultEncoding);
            return data == null ? string.Empty : Encoding.GetEncoding(defaultEncoding).GetString(data);
        }
#if NET472 || NET48
#region sha

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] Sha1Cng(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0) return (byte[])null;
            using (SHA1 sHA1 = SHA1Cng.Create()) return sHA1.ComputeHash(buffer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] Sha256Cng(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0) return (byte[])null;
            using (SHA256 sHA256 = SHA256Cng.Create()) return sHA256.ComputeHash(buffer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] Sha384Cng(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0) return (byte[])null;
            using (SHA384 sHA384 = SHA384Cng.Create()) return sHA384.ComputeHash(buffer);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] Sha512Cng(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0) return (byte[])null;
            using (SHA512 sHA512 = SHA512Cng.Create()) return sHA512.ComputeHash(buffer);
        }
#endregion sha

#endif
#region sha
#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2)
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] Sha1Managed(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0) return (byte[])null;
            using (SHA1 sHA1 = SHA1Managed.Create()) return sHA1.ComputeHash(buffer);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] Sha1CryptoServiceProvider(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0) return (byte[])null;
            using (SHA1 sHA1 = SHA1CryptoServiceProvider.Create()) return sHA1.ComputeHash(buffer);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] Sha256(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0) return (byte[])null;
            using (SHA256 sHA256 = SHA256.Create()) return sHA256.ComputeHash(buffer);
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] Sha256Managed(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0) return (byte[])null;
            using (SHA256 sHA256 = SHA256Managed.Create()) return sHA256.ComputeHash(buffer);
        }
#endif
#if !(NET20 || NET30 || NET35 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2)
       
       /// <summary>
       /// /
       /// </summary>
       /// <param name="buffer"></param>
       /// <returns></returns>
        public static byte[] Sha256CryptoServiceProvider(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0) return (byte[])null;
            using (SHA256 sHA256 = SHA256CryptoServiceProvider.Create()) return sHA256.ComputeHash(buffer);
        }
       

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] Sha384(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0) return (byte[])null;
            using (SHA384 sHA384 = SHA384.Create()) return sHA384.ComputeHash(buffer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] Sha384CryptoServiceProvider(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0) return (byte[])null;
            using (SHA384 sHA384 = SHA384CryptoServiceProvider.Create()) return sHA384.ComputeHash(buffer);
        }
#endif
#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] Sha384Managed(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0) return (byte[])null;
            using (SHA384 sHA384 = SHA384Managed.Create())
                return sHA384.ComputeHash(buffer);
        }
#endif
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] Sha512(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0) return (byte[])null;
            using (SHA512 sHA512 = SHA512.Create()) return sHA512.ComputeHash(buffer);
        }
#if !(NET20 || NET30 || NET35 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] Sha512CryptoServiceProvider(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0) return (byte[])null;
            using (SHA512 sHA512 = SHA512CryptoServiceProvider.Create()) return sHA512.ComputeHash(buffer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] Sha512Managed(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0) return (byte[])null;
            using (SHA512 sHA512 = SHA512Managed.Create()) return sHA512.ComputeHash(buffer);
        }
#endif
#endregion sha 


#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2)
#region HASH

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] Hash(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0) return (byte[])null;
            using (HashAlgorithm hash = HashAlgorithm.Create()) return hash.ComputeHash(buffer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] Hmac(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0) return (byte[])null;
            using (HMAC hMAC = HMAC.Create()) return hMAC.ComputeHash(buffer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] HmacMD5(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0) return (byte[])null;
            using (HMAC hMAC = HMACMD5.Create()) return hMAC.ComputeHash(buffer);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] HmacSHA1(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0) return (byte[])null;
            using (HMAC hMAC = HMACSHA1.Create()) return hMAC.ComputeHash(buffer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] HmacSHA256(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0) return (byte[])null;
            using (HMAC hMAC = HMACSHA256.Create()) return hMAC.ComputeHash(buffer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] HmacSHA384(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0) return (byte[])null;
            using (HMAC hMAC = HMACSHA384.Create()) return hMAC.ComputeHash(buffer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] HmacSHA512(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0) return (byte[])null;
            using (HMAC hMAC = HMACSHA512.Create()) return hMAC.ComputeHash(buffer);
        }
#endregion HASH


        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] Md5CryptoServiceProvider(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0) return (byte[])null;
            using (MD5 mD5 = MD5CryptoServiceProvider.Create()) return mD5.ComputeHash(buffer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="xmlString"></param>
        /// <param name="defaultEncoding"></param>
        /// <returns></returns>
        public static byte[] RsaEncryptToByte(string data,string xmlString,string defaultEncoding="utf-8")
        {
            using (RSA rSA=RSA.Create())
            {
                rSA.FromXmlString(xmlString);
                return rSA.EncryptValue(Encoding.GetEncoding(defaultEncoding).GetBytes(data));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="xmlString"></param>
        /// <param name="defaultEncoding"></param>
        /// <returns></returns>
        public static byte[] RsaDecryptToByte(string data, string xmlString, string defaultEncoding = "utf-8")
        {
            using (RSA rSA = RSA.Create())
            {
                rSA.FromXmlString(xmlString);
                return rSA.DecryptValue(Encoding.GetEncoding(defaultEncoding).GetBytes(data));
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="xmlString"></param>
        /// <param name="defaultEncoding"></param>
        /// <returns></returns>
        public static string RsaEncrypt(string data, string xmlString, string defaultEncoding = "utf-8")
        {
            return HexHelper.ToHex(RsaEncryptToByte(data,xmlString, defaultEncoding));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="xmlString"></param>
        /// <param name="defaultEncoding"></param>
        /// <returns></returns>
        public static string RsaDecrypt(string data, string xmlString, string defaultEncoding = "utf-8")
        {
            return HexHelper.ToHex(RsaDecryptToByte(data, xmlString, defaultEncoding));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyBit"></param>
        /// <param name="data"></param>
        /// <param name="xmlString"></param>
        /// <param name="signData"></param>
        /// <param name="defaultEncoding"></param>
        /// <returns></returns>
        public static byte[] RsaEncryptToByte(int keyBit, string data, string xmlString, byte[] signData, string defaultEncoding = "utf-8")
        {
            using (RSACryptoServiceProvider rSA = new RSACryptoServiceProvider(keyBit))
            {
                rSA.FromXmlString(xmlString);
                //rSA.SignData(signData, null);
                return rSA.Encrypt(Encoding.GetEncoding(defaultEncoding).GetBytes(data),true);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyBit"></param>
        /// <param name="data"></param>
        /// <param name="xmlString"></param>
        /// <param name="signData"></param>
        /// <param name="defaultEncoding"></param>
        /// <returns></returns>
        public static byte[] RsaDecryptToByte(int keyBit,string data, string xmlString, byte[] signData, string defaultEncoding = "utf-8")
        {
            using (RSACryptoServiceProvider rSA = new RSACryptoServiceProvider(keyBit))
            {
                rSA.FromXmlString(xmlString);
                rSA.SignData(signData,null);
                return rSA.DecryptValue(Encoding.GetEncoding(defaultEncoding).GetBytes(data));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyBit"></param>
        /// <param name="data"></param>
        /// <param name="xmlString"></param>
        /// <param name="signData"></param>
        /// <param name="defaultEncoding"></param>
        /// <returns></returns>
        public static string RsaEncrypt(int keyBit, string data, string xmlString, string signData, string defaultEncoding = "utf-8")
        {
            return HexHelper.ToHex(RsaEncryptToByte(keyBit,data, xmlString, Encoding.GetEncoding(defaultEncoding).GetBytes(signData), defaultEncoding));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyBit"></param>
        /// <param name="data"></param>
        /// <param name="xmlString"></param>
        /// <param name="signData"></param>
        /// <param name="defaultEncoding"></param>
        /// <returns></returns>
        public static string RsaDecrypt(int keyBit, string data, string xmlString, string signData, string defaultEncoding = "utf-8")
        {
            return HexHelper.ToHex(RsaDecryptToByte(keyBit,data, xmlString, Encoding.GetEncoding(defaultEncoding).GetBytes(signData), defaultEncoding));
        }
#endif
    }
}
#endif
