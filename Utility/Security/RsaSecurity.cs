#if !(NET20 || NET30 || NET40 || NETCOREAPP1_0 || NETCOREAPP1_1  || NETCOREAPP1_2|| NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Utility.Security
{
    /// <summary>
    /// Rsa c#  js相对应操作
    /// <para>https://www.cnblogs.com/alunchen/p/5758585.html</para>
    /// <para>https://www.cnblogs.com/hubro/p/4538925.html</para>
    /// </summary>
    public class RsaSecurity
    {
#region BouncyCastle
        /// <summary>
        /// RSA签名
        /// </summary>
        /// <param name="content">数据</param>
        /// <param name="privateKey">RSA密钥</param>
        /// <returns></returns>
        public static byte[] Sign(byte[] content, byte[] privateKey)
        {
            var signer = SignerUtilities.GetSigner("SHA1withRSA");
            //将java格式的rsa密钥转换成.net格式
            var privateKeyParam = (RsaPrivateCrtKeyParameters)PrivateKeyFactory.CreateKey(privateKey);
            signer.Init(true, privateKeyParam);
            signer.BlockUpdate(content, 0, content.Length);
            var signBytes = signer.GenerateSignature();
            return signBytes;
        }
        /// <summary>
        /// RSA验签
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="publicKey">RSA公钥</param>
        /// <returns></returns>
        public static byte[] VerifySign(byte[] content, byte[] publicKey)
        {
            var signer = SignerUtilities.GetSigner("SHA1withRSA");
            var publicKeyParam = (RsaKeyParameters)PublicKeyFactory.CreateKey(publicKey);
            signer.Init(false, publicKeyParam);
            signer.BlockUpdate(content, 0, content.Length);
            var signBytes = signer.GenerateSignature();
            return signBytes;
        }
        /// <summary>
        /// x509格式公钥转换为Der格式
        /// </summary>
        /// <param name="x509PublicKey">x509格式公钥字符串 字节</param>
        /// <returns>Der格式公钥字符串 字节</returns>
        public static byte[] GetRsaPublicKeyDerFromX509(byte[] x509PublicKey)
        {
            try
            {
                Asn1InputStream aIn = new Asn1InputStream(x509PublicKey);
                SubjectPublicKeyInfo info = SubjectPublicKeyInfo.GetInstance(aIn.ReadObject());
#pragma warning disable CS0618 // 类型或成员已过时
                RsaPublicKeyStructure @struct = RsaPublicKeyStructure.GetInstance(info.GetPublicKey());
#pragma warning restore CS0618 // 类型或成员已过时
                if (aIn != null)
                    aIn.Close();
                return @struct.GetDerEncoded();
            }
            catch (IOException e)
            {
                return null;
            }
        }
#region x509与.NET相互转换
        /// <summary>
        /// RSA私钥格式转换，java->.net
        /// </summary>
        /// <param name="privateKeyInfoData">java生成的RSA私钥</param>
        /// <returns></returns>
        public static string RSAPrivateKeyJava2DotNet(byte[] privateKeyInfoData)
        {
            RsaPrivateCrtKeyParameters privateKeyParam = (RsaPrivateCrtKeyParameters)PrivateKeyFactory.CreateKey(privateKeyInfoData);
            return Create(privateKeyParam);
        }
        public static string Create(RsaPrivateCrtKeyParameters rsaPrivateCrtKey)
        {
            return string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent><P>{2}</P><Q>{3}</Q><DP>{4}</DP><DQ>{5}</DQ><InverseQ>{6}</InverseQ><D>{7}</D></RSAKeyValue>",
              Convert.ToBase64String(rsaPrivateCrtKey.Modulus.ToByteArrayUnsigned()),
              Convert.ToBase64String(rsaPrivateCrtKey.PublicExponent.ToByteArrayUnsigned()),
              Convert.ToBase64String(rsaPrivateCrtKey.P.ToByteArrayUnsigned()),
              Convert.ToBase64String(rsaPrivateCrtKey.Q.ToByteArrayUnsigned()),
              Convert.ToBase64String(rsaPrivateCrtKey.DP.ToByteArrayUnsigned()),
              Convert.ToBase64String(rsaPrivateCrtKey.DQ.ToByteArrayUnsigned()),
              Convert.ToBase64String(rsaPrivateCrtKey.QInv.ToByteArrayUnsigned()),
              Convert.ToBase64String(rsaPrivateCrtKey.Exponent.ToByteArrayUnsigned()));
        }
        /// <summary>
        /// RSA私钥格式转换，.net->java
        /// </summary>
        /// <param name="privateKey">.net生成的私钥</param>
        /// <returns></returns>
        public static byte[] RSAPrivateKeyDotNet2Java(string privateKey)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(privateKey);
            BigInteger m = new BigInteger(1, Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("Modulus")[0].InnerText));
            BigInteger exp = new BigInteger(1, Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("Exponent")[0].InnerText));
            BigInteger d = new BigInteger(1, Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("D")[0].InnerText));
            BigInteger p = new BigInteger(1, Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("P")[0].InnerText));
            BigInteger q = new BigInteger(1, Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("Q")[0].InnerText));
            BigInteger dp = new BigInteger(1, Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("DP")[0].InnerText));
            BigInteger dq = new BigInteger(1, Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("DQ")[0].InnerText));
            BigInteger qinv = new BigInteger(1, Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("InverseQ")[0].InnerText));

            RsaPrivateCrtKeyParameters privateKeyParam = new RsaPrivateCrtKeyParameters(m, exp, d, p, q, dp, dq, qinv);

            PrivateKeyInfo privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(privateKeyParam);
            byte[] serializedPrivateBytes = privateKeyInfo.ToAsn1Object().GetEncoded();
            return serializedPrivateBytes;
            //return Convert.ToBase64String(serializedPrivateBytes);
        }

        /// <summary>
        /// RSA公钥格式转换，java->.net
        /// </summary>
        /// <param name="keyInfoData">java生成的公钥</param>
        /// <returns></returns>
        public static string RSAPublicKeyJava2DotNet(byte[] keyInfoData)
        {
            RsaKeyParameters publicKeyParam = (RsaKeyParameters)PublicKeyFactory.CreateKey(keyInfoData);
            return string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent></RSAKeyValue>",
                Convert.ToBase64String(publicKeyParam.Modulus.ToByteArrayUnsigned()),
                Convert.ToBase64String(publicKeyParam.Exponent.ToByteArrayUnsigned()));
        }

        /// <summary>
        /// RSA公钥格式转换，.net->java
        /// </summary>
        /// <param name="publicKey">.net生成的公钥</param>
        /// <returns></returns>
        public static byte[] RSAPublicKeyDotNet2Java(string publicKey)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(publicKey);
            BigInteger m = new BigInteger(1, Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("Modulus")[0].InnerText));
            BigInteger p = new BigInteger(1, Convert.FromBase64String(doc.DocumentElement.GetElementsByTagName("Exponent")[0].InnerText));
            RsaKeyParameters pub = new RsaKeyParameters(false, m, p);

            SubjectPublicKeyInfo publicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(pub);
            byte[] serializedPublicBytes = publicKeyInfo.ToAsn1Object().GetDerEncoded();
            return serializedPublicBytes;
            //return Convert.ToBase64String(serializedPublicBytes);
        }
        #endregion x509与.NET相互转换

        #region PEM与.NET相互转换
#if !(NET20 || NET30 || !NETSTANDARD1_0 || !NETSTANDARD1_1 || !NETSTANDARD1_2 || !NETSTANDARD1_3 || !NETSTANDARD1_4 || !NETSTANDARD1_5 || !NETSTANDARD1_6 || NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2)
        public static void Xml2PemPrivate(string xml, string saveFile)
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(xml);
            var p = rsa.ExportParameters(true);
            var key = new RsaPrivateCrtKeyParameters(
                new BigInteger(1, p.Modulus), new BigInteger(1, p.Exponent), new BigInteger(1, p.D),
                new BigInteger(1, p.P), new BigInteger(1, p.Q), new BigInteger(1, p.DP), new BigInteger(1, p.DQ),
                new BigInteger(1, p.InverseQ));
            using (var sw = new StreamWriter(saveFile))
            {
                var pemWriter = new Org.BouncyCastle.OpenSsl.PemWriter(sw);
                pemWriter.WriteObject(key);
            }
        }
        public static string Pem2XmlPrivate(string pemFile)
        {
            AsymmetricCipherKeyPair keyPair;
            using (var sr = new StreamReader(pemFile))
            {
                var pemReader = new Org.BouncyCastle.OpenSsl.PemReader(sr);
                keyPair = (AsymmetricCipherKeyPair)pemReader.ReadObject();
            }
            var key = (RsaPrivateCrtKeyParameters)keyPair.Private;
            var p = new RSAParameters
            {
                Modulus = key.Modulus.ToByteArrayUnsigned(),
                Exponent = key.PublicExponent.ToByteArrayUnsigned(),
                D = key.Exponent.ToByteArrayUnsigned(),
                P = key.P.ToByteArrayUnsigned(),
                Q = key.Q.ToByteArrayUnsigned(),
                DP = key.DP.ToByteArrayUnsigned(),
                DQ = key.DQ.ToByteArrayUnsigned(),
                InverseQ = key.QInv.ToByteArrayUnsigned(),
            };
            var rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(p);
            return rsa.ToXmlString(true);
        }


        public static string Xml2PemPublic(string xml, string saveFile)
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(xml);
            var p = rsa.ExportParameters(false);
            RsaKeyParameters key = new RsaKeyParameters(false, new BigInteger(1, p.Modulus), new BigInteger(1, p.Exponent));
            using (var sw = new StreamWriter(saveFile))
            {
                var pemWriter = new Org.BouncyCastle.OpenSsl.PemWriter(sw);
                pemWriter.WriteObject(key);
            }
            return System.IO.File.ReadAllText(saveFile);
        }
#endif
        public static string Pem2XmlPublic(string pemFileConent)
        {
            pemFileConent = pemFileConent.Replace("-----BEGIN PUBLIC KEY-----", "").Replace("-----END PUBLIC KEY-----", "").Replace("\n", "").Replace("\r", "");
            var data = Convert.FromBase64String(pemFileConent);
            return RSAPublicKeyJava2DotNet(data);
        }
#endregion PEM与.NET相互转换

#endregion BouncyCastle


        public static byte[] Decrypt(byte[] cipherText,byte[] privateKey)
        {
            RSACryptoServiceProvider privateKeyRsaProvider = CreateRsaProviderFromPrivateKey(privateKey);
            if (privateKeyRsaProvider == null)
            {
                throw new Exception("_privateKeyRsaProvider is null");
            }
            return privateKeyRsaProvider.Decrypt(cipherText, false);
        }

        public static byte[] Encrypt(byte[] text, byte[] publicKey)
        {
            RSACryptoServiceProvider publicKeyRsaProvider = CreateRsaProviderFromPublicKey(publicKey);
            if (publicKeyRsaProvider == null)
            {
                throw new Exception("_publicKeyRsaProvider is null");
            }
            return publicKeyRsaProvider.Encrypt(text, false);
        }

        private static RSACryptoServiceProvider CreateRsaProviderFromPrivateKey(byte[] privateKey)
        {
            var RSA = new RSACryptoServiceProvider();
            var RSAparams = new RSAParameters();
            using (BinaryReader binr = new BinaryReader(new MemoryStream(privateKey)))
            {
                byte bt = 0;
                ushort twobytes = 0;
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130)
                    binr.ReadByte();
                else if (twobytes == 0x8230)
                    binr.ReadInt16();
                else
                    throw new Exception("Unexpected value read binr.ReadUInt16()");

                twobytes = binr.ReadUInt16();
                if (twobytes != 0x0102)
                    throw new Exception("Unexpected version");

                bt = binr.ReadByte();
                if (bt != 0x00)
                    throw new Exception("Unexpected value read binr.ReadByte()");

                RSAparams.Modulus = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.Exponent = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.D = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.P = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.Q = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.DP = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.DQ = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.InverseQ = binr.ReadBytes(GetIntegerSize(binr));
            }
            RSA.ImportParameters(RSAparams);
            return RSA;
        }

        private static int GetIntegerSize(BinaryReader binr)
        {
            byte bt = 0;
            byte lowbyte = 0x00;
            byte highbyte = 0x00;
            int count = 0;
            bt = binr.ReadByte();
            if (bt != 0x02)
                return 0;
            bt = binr.ReadByte();

            if (bt == 0x81)
                count = binr.ReadByte();
            else
                if (bt == 0x82)
            {
                highbyte = binr.ReadByte();
                lowbyte = binr.ReadByte();
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                count = BitConverter.ToInt32(modint, 0);
            }
            else
            {
                count = bt;
            }

            while (binr.ReadByte() == 0x00)
            {
                count -= 1;
            }
            binr.BaseStream.Seek(-1, SeekOrigin.Current);
            return count;
        }

        private static RSACryptoServiceProvider CreateRsaProviderFromPublicKey(byte[] publicKey)
        {
            // encoded OID sequence for  PKCS #1 rsaEncryption szOID_RSA_RSA = "1.2.840.113549.1.1.1"
            byte[] SeqOID = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
            byte[] seq = new byte[15];
            // ---------  Set up stream to read the asn.1 encoded SubjectPublicKeyInfo blob  ------
            using (MemoryStream mem = new MemoryStream(publicKey))
            {
                using (BinaryReader binr = new BinaryReader(mem))  //wrap Memory Stream with BinaryReader for easy reading
                {
                    byte bt = 0;
                    ushort twobytes = 0;

                    twobytes = binr.ReadUInt16();
                    if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                        binr.ReadByte();    //advance 1 byte
                    else if (twobytes == 0x8230)
                        binr.ReadInt16();   //advance 2 bytes
                    else
                        return null;

                    seq = binr.ReadBytes(15);       //read the Sequence OID
                    if (!CompareBytearrays(seq, SeqOID))    //make sure Sequence for OID is correct
                        return null;

                    twobytes = binr.ReadUInt16();
                    if (twobytes == 0x8103) //data read as little endian order (actual data order for Bit String is 03 81)
                        binr.ReadByte();    //advance 1 byte
                    else if (twobytes == 0x8203)
                        binr.ReadInt16();   //advance 2 bytes
                    else
                        return null;

                    bt = binr.ReadByte();
                    if (bt != 0x00)     //expect null byte next
                        return null;

                    twobytes = binr.ReadUInt16();
                    if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                        binr.ReadByte();    //advance 1 byte
                    else if (twobytes == 0x8230)
                        binr.ReadInt16();   //advance 2 bytes
                    else
                        return null;

                    twobytes = binr.ReadUInt16();
                    byte lowbyte = 0x00;
                    byte highbyte = 0x00;

                    if (twobytes == 0x8102) //data read as little endian order (actual data order for Integer is 02 81)
                        lowbyte = binr.ReadByte();  // read next bytes which is bytes in modulus
                    else if (twobytes == 0x8202)
                    {
                        highbyte = binr.ReadByte(); //advance 2 bytes
                        lowbyte = binr.ReadByte();
                    }
                    else
                        return null;
                    byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };   //reverse byte order since asn.1 key uses big endian order
                    int modsize = BitConverter.ToInt32(modint, 0);

                    int firstbyte = binr.PeekChar();
                    if (firstbyte == 0x00)
                    {   //if first byte (highest order) of modulus is zero, don't include it
                        binr.ReadByte();    //skip this null byte
                        modsize -= 1;   //reduce modulus buffer size by 1
                    }

                    byte[] modulus = binr.ReadBytes(modsize);   //read the modulus bytes

                    if (binr.ReadByte() != 0x02)            //expect an Integer for the exponent data
                        return null;
                    int expbytes = (int)binr.ReadByte();        // should only need one byte for actual exponent data (for all useful values)
                    byte[] exponent = binr.ReadBytes(expbytes);

                    // ------- create RSACryptoServiceProvider instance and initialize with public key -----
                    RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                    RSAParameters RSAKeyInfo = new RSAParameters();
                    RSAKeyInfo.Modulus = modulus;
                    RSAKeyInfo.Exponent = exponent;
                    RSA.ImportParameters(RSAKeyInfo);
                    return RSA;
                }
            }
        }

        private static bool CompareBytearrays(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                return false;
            int i = 0;
            foreach (byte c in a)
            {
                if (c != b[i])
                    return false;
                i++;
            }
            return true;
        }


    }
}
#endif