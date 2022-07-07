using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DEncrypt
{
    public class Encrypts
    {
        public static string EncryptMd5(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }
            else
            {
                using (MD5 mi = MD5.Create())
                {
                    byte[] buffer = Encoding.Default.GetBytes(str);
                    //MD5加密
                    byte[] newBuffer = mi.ComputeHash(buffer);
                    //SHA256加密
                    byte[] hash = SHA256.Create().ComputeHash(newBuffer);
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < newBuffer.Length; i++)
                    {
                        sb.Append(hash[i].ToString("x2"));
                    }
                    return sb.ToString();
                }
            }
        }

        #region 银联支付相关
        public static string GetMD5Hash(string fileName)
        {
            try
            {
                FileStream file = new FileStream(fileName, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5Hash() fail,error:" + ex.Message);
            }
        }
        /// <summary>
        ///字节码数据加签
        /// </summary>
        /// <param name="contentForSign">待签名数据</param>
        /// <param name="priKeyFile">私钥证书文件</param>
        /// <param name="keyPwd">密码</param>
        /// <returns></returns>
        public static string Sign(string contentForSign, string priKeyFile, string keyPwd)
        {
            var rsa = GetPrivateKey(priKeyFile, keyPwd);
            // Create a new RSACryptoServiceProvider
            var rsaClear = new RSACryptoServiceProvider();
            // Export RSA parameters from 'rsa' and import them into 'rsaClear'
            var paras = rsa.ExportParameters(true);
            rsaClear.ImportParameters(paras);
            using (var sha256 = new SHA256CryptoServiceProvider())
            {
                var signData = rsaClear.SignData(Encoding.UTF8.GetBytes(contentForSign), sha256);
                return BytesToHex(signData);
            }
        }
        /// <summary>
        /// 验签 根据公钥验证文件是否正确 sha256withRsa
        /// </summary>
        /// <param name="contentForSign">回盘文件</param>
        /// <param name="signedData">回盘文件校验文件字节数据</param>
        /// <param name="pubKeyFile">验签证书</param>
        /// <returns></returns>
        public static bool VerifySign(string contentForSign, string signedData, string pubKeyFile)
        {
            var rsa = GetPublicKey(pubKeyFile);
            using (var sha256 = new SHA256CryptoServiceProvider())
            {
                return rsa.VerifyData(Encoding.UTF8.GetBytes(contentForSign), sha256, HexToBytes(signedData));
            }
        }
        /// <summary>
        /// 获取签名证书私钥
        /// </summary>
        /// <param name="priKeyFile"></param>
        /// <param name="keyPwd"></param>
        /// <returns></returns>
        private static RSACryptoServiceProvider GetPrivateKey(string priKeyFile, string keyPwd)
        {
            var pc = new X509Certificate2(priKeyFile, keyPwd, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);
            return (RSACryptoServiceProvider)pc.PrivateKey;
        }
        /// <summary>
        /// 获取验签证书
        /// </summary>
        /// <param name="pubKeyFile"></param>
        /// <returns></returns>
        private static RSACryptoServiceProvider GetPublicKey(string pubKeyFile)
        {
            var pc = new X509Certificate2(pubKeyFile);
            return (RSACryptoServiceProvider)pc.PublicKey.Key;
        }
        public static byte[] HexToBytes(string text)
        {
            text = text.Trim();
            if (text.Length % 2 != 0)
                throw new ArgumentException("text 长度为奇数。");

            List<byte> lstRet = new List<byte>();
            for (int i = 0; i < text.Length; i = i + 2)
            {
                Console.WriteLine(text.Substring(i, 2));
                lstRet.Add(Convert.ToByte(text.Substring(i, 2), 16));
            }
            return lstRet.ToArray();
        }
        /// <summary>
        /// bytes转换hex
        /// </summary>
        /// <param name="data">bytes</param>
        /// <returns>转换后的hex字符串</returns>
        public static string BytesToHex(byte[] data)
        {
            StringBuilder sbRet = new StringBuilder(data.Length * 2);
            for (int i = 0; i < data.Length; i++)
            {
                sbRet.Append(Convert.ToString(data[i], 16).PadLeft(2, '0'));
            }
            return sbRet.ToString();
        }

        #endregion

        #region SHA256
        /// <summary>
        /// SHA256加密(UTF8)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string SHA256EncryptString(string data)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            byte[] hash = SHA256.Create().ComputeHash(bytes);

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                builder.Append(hash[i].ToString("x2"));
            }
            return builder.ToString();
        }
        #endregion

        #region Base64
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultEncoding"></param>
        /// <returns></returns>
        public static string EncodeBase64(string str, Encoding defaultEncoding = null)
        {
            string encode = "";
            if (string.IsNullOrEmpty(str))
                return encode;
            if (defaultEncoding == null)
                defaultEncoding = Encoding.Default;
            byte[] bytes = defaultEncoding.GetBytes(str);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// 采用指定编码对字符串进行Base64解码，如果未指定则为系统默认编码方式
        /// </summary>
        /// <param name="str">待解码的字符串</param>
        /// <param name="defaultEncoding">指定的编码方式，如果不指定则为系统默认</param>
        /// <returns>解码后的字符串</returns>
        /// <exception cref="System.Exception">对字符串进行BASE64解码时出现异常</exception>
        public static string DecodeBase64(string str, Encoding defaultEncoding = null)
        {
            string decode = "";
            if (string.IsNullOrEmpty(str))
                return decode;
            if (defaultEncoding == null)
                defaultEncoding = System.Text.Encoding.Default;
            byte[] bytes = Convert.FromBase64String(str);
            try
            {
                decode = defaultEncoding.GetString(bytes);
            }
            catch
            {
                throw new Exception("对字符串进行BASE64解码时出现异常");
            }
            return decode;
        }
        #endregion

        #region 对称加解密
        /// <summary>
        /// DES 加密(数据加密标准，速度较快，适用于加密大量数据的场合)
        /// </summary>
        /// <param name="EncryptString">待加密的密文</param>
        /// <param name="EncryptKey">加密的密钥</param>
        /// <returns>returns</returns>
        public static string DESEncrypt(string EncryptString, string EncryptKey)
        {
            if (string.IsNullOrEmpty(EncryptString)) { throw (new Exception("密文不得为空")); }

            if (string.IsNullOrEmpty(EncryptKey)) { throw (new Exception("密钥不得为空")); }

            if (EncryptKey.Length != 8) { throw (new Exception("密钥必须为8位")); }

            byte[] m_btIV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

            string m_strEncrypt = "";

            DESCryptoServiceProvider m_DESProvider = new DESCryptoServiceProvider();

            try
            {
                byte[] m_btEncryptString = Encoding.Default.GetBytes(EncryptString);

                MemoryStream m_stream = new MemoryStream();

                CryptoStream m_cstream = new CryptoStream(m_stream, m_DESProvider.CreateEncryptor(Encoding.Default.GetBytes(EncryptKey), m_btIV), CryptoStreamMode.Write);

                m_cstream.Write(m_btEncryptString, 0, m_btEncryptString.Length);

                m_cstream.FlushFinalBlock();

                m_strEncrypt = Convert.ToBase64String(m_stream.ToArray());

                m_stream.Close(); m_stream.Dispose();

                m_cstream.Close(); m_cstream.Dispose();
            }
            catch (IOException ex) { throw ex; }
            catch (CryptographicException ex) { throw ex; }
            catch (ArgumentException ex) { throw ex; }
            catch (Exception ex) { throw ex; }
            finally { m_DESProvider.Clear(); }

            return m_strEncrypt;
        }
        /// <summary>
        /// DES 解密(数据加密标准，速度较快，适用于加密大量数据的场合)
        /// </summary>
        /// <param name="DecryptString">待解密的密文</param>
        /// <param name="DecryptKey">解密的密钥</param>
        /// <returns>returns</returns>
        public static string DESDecrypt(string DecryptString, string DecryptKey)
        {
            if (string.IsNullOrEmpty(DecryptString)) { throw (new Exception("密文不得为空")); }

            if (string.IsNullOrEmpty(DecryptKey)) { throw (new Exception("密钥不得为空")); }

            if (DecryptKey.Length != 8) { throw (new Exception("密钥必须为8位")); }

            byte[] m_btIV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

            string m_strDecrypt = "";

            DESCryptoServiceProvider m_DESProvider = new DESCryptoServiceProvider();

            try
            {
                byte[] m_btDecryptString = Convert.FromBase64String(DecryptString);

                MemoryStream m_stream = new MemoryStream();

                CryptoStream m_cstream = new CryptoStream(m_stream, m_DESProvider.CreateDecryptor(Encoding.Default.GetBytes(DecryptKey), m_btIV), CryptoStreamMode.Write);

                m_cstream.Write(m_btDecryptString, 0, m_btDecryptString.Length);

                m_cstream.FlushFinalBlock();

                m_strDecrypt = Encoding.Default.GetString(m_stream.ToArray());

                m_stream.Close(); m_stream.Dispose();

                m_cstream.Close(); m_cstream.Dispose();
            }
            catch (IOException ex) { throw ex; }
            catch (CryptographicException ex) { throw ex; }
            catch (ArgumentException ex) { throw ex; }
            catch (Exception ex) { throw ex; }
            finally { m_DESProvider.Clear(); }

            return m_strDecrypt;
        }
        /// <summary>
        /// RC2 加密(用变长密钥对大量数据进行加密)
        /// </summary>
        /// <param name="EncryptString">待加密密文</param>
        /// <param name="EncryptKey">加密密钥</param>
        /// <returns>returns</returns>
        public static string RC2Encrypt(string EncryptString, string EncryptKey)
        {
            if (string.IsNullOrEmpty(EncryptString)) { throw (new Exception("密文不得为空")); }

            if (string.IsNullOrEmpty(EncryptKey)) { throw (new Exception("密钥不得为空")); }

            if (EncryptKey.Length < 5 || EncryptKey.Length > 16) { throw (new Exception("密钥必须为5-16位")); }

            string m_strEncrypt = "";

            byte[] m_btIV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

            RC2CryptoServiceProvider m_RC2Provider = new RC2CryptoServiceProvider();

            try
            {
                byte[] m_btEncryptString = Encoding.Default.GetBytes(EncryptString);

                MemoryStream m_stream = new MemoryStream();

                CryptoStream m_cstream = new CryptoStream(m_stream, m_RC2Provider.CreateEncryptor(Encoding.Default.GetBytes(EncryptKey), m_btIV), CryptoStreamMode.Write);

                m_cstream.Write(m_btEncryptString, 0, m_btEncryptString.Length);

                m_cstream.FlushFinalBlock();

                m_strEncrypt = Convert.ToBase64String(m_stream.ToArray());

                m_stream.Close(); m_stream.Dispose();

                m_cstream.Close(); m_cstream.Dispose();
            }
            catch (IOException ex) { throw ex; }
            catch (CryptographicException ex) { throw ex; }
            catch (ArgumentException ex) { throw ex; }
            catch (Exception ex) { throw ex; }
            finally { m_RC2Provider.Clear(); }

            return m_strEncrypt;
        }
        /// <summary>
        /// RC2 解密(用变长密钥对大量数据进行加密)
        /// </summary>
        /// <param name="DecryptString">待解密密文</param>
        /// <param name="DecryptKey">解密密钥</param>
        /// <returns>returns</returns>
        public static string RC2Decrypt(string DecryptString, string DecryptKey)
        {
            if (string.IsNullOrEmpty(DecryptString)) { throw (new Exception("密文不得为空")); }

            if (string.IsNullOrEmpty(DecryptKey)) { throw (new Exception("密钥不得为空")); }

            if (DecryptKey.Length < 5 || DecryptKey.Length > 16) { throw (new Exception("密钥必须为5-16位")); }

            byte[] m_btIV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

            string m_strDecrypt = "";

            RC2CryptoServiceProvider m_RC2Provider = new RC2CryptoServiceProvider();

            try
            {
                byte[] m_btDecryptString = Convert.FromBase64String(DecryptString);

                MemoryStream m_stream = new MemoryStream();

                CryptoStream m_cstream = new CryptoStream(m_stream, m_RC2Provider.CreateDecryptor(Encoding.Default.GetBytes(DecryptKey), m_btIV), CryptoStreamMode.Write);

                m_cstream.Write(m_btDecryptString, 0, m_btDecryptString.Length);

                m_cstream.FlushFinalBlock();

                m_strDecrypt = Encoding.Default.GetString(m_stream.ToArray());

                m_stream.Close(); m_stream.Dispose();

                m_cstream.Close(); m_cstream.Dispose();
            }
            catch (IOException ex) { throw ex; }
            catch (CryptographicException ex) { throw ex; }
            catch (ArgumentException ex) { throw ex; }
            catch (Exception ex) { throw ex; }
            finally { m_RC2Provider.Clear(); }
            return m_strDecrypt;
        }
        /// <summary>
        /// 3DES 加密(基于DES，对一块数据用三个不同的密钥进行三次加密，强度更高)
        /// </summary>
        /// <param name="EncryptString">待加密密文</param>
        /// <param name="EncryptKey1">密钥一</param>
        /// <param name="EncryptKey2">密钥二</param>
        /// <param name="EncryptKey3">密钥三</param>
        /// <returns>returns</returns>
        public static string DES3Encrypt(string EncryptString, string EncryptKey1, string EncryptKey2, string EncryptKey3)
        {
            string m_strEncrypt = "";

            try
            {
                m_strEncrypt = DESEncrypt(EncryptString, EncryptKey3);

                m_strEncrypt = DESEncrypt(m_strEncrypt, EncryptKey2);

                m_strEncrypt = DESEncrypt(m_strEncrypt, EncryptKey1);
            }
            catch (Exception ex) { throw ex; }

            return m_strEncrypt;
        }
        /// <summary>
        /// 3DES 解密(基于DES，对一块数据用三个不同的密钥进行三次加密，强度更高)
        /// </summary>
        /// <param name="DecryptString">待解密密文</param>
        /// <param name="DecryptKey1">密钥一</param>
        /// <param name="DecryptKey2">密钥二</param>
        /// <param name="DecryptKey3">密钥三</param>
        /// <returns>returns</returns>
        public static string DES3Decrypt(string DecryptString, string DecryptKey1, string DecryptKey2, string DecryptKey3)
        {
            string m_strDecrypt = "";

            try
            {
                m_strDecrypt = DESDecrypt(DecryptString, DecryptKey1);

                m_strDecrypt = DESDecrypt(m_strDecrypt, DecryptKey2);

                m_strDecrypt = DESDecrypt(m_strDecrypt, DecryptKey3);
            }
            catch (Exception ex) { throw ex; }

            return m_strDecrypt;
        }
        /// <summary>
        /// AES 加密(高级加密标准，是下一代的加密算法标准，速度快，安全级别高，目前 AES 标准的一个实现是 Rijndael 算法)
        /// </summary>
        /// <param name="EncryptString">待加密密文</param>
        /// <param name="EncryptKey">加密密钥</param>
        /// <returns></returns>
        public static string AESEncrypt(string EncryptString, string EncryptKey)
        {
            if (string.IsNullOrEmpty(EncryptString)) { throw (new Exception("密文不得为空")); }

            if (string.IsNullOrEmpty(EncryptKey)) { throw (new Exception("密钥不得为空")); }

            string m_strEncrypt = "";

            byte[] m_btIV = Convert.FromBase64String("Rkb4jvUy/ye7Cd7k89QQgQ==");

            Rijndael m_AESProvider = Rijndael.Create();

            try
            {
                byte[] m_btEncryptString = Encoding.Default.GetBytes(EncryptString);

                MemoryStream m_stream = new MemoryStream();

                CryptoStream m_csstream = new CryptoStream(m_stream, m_AESProvider.CreateEncryptor(Encoding.Default.GetBytes(EncryptKey), m_btIV), CryptoStreamMode.Write);

                m_csstream.Write(m_btEncryptString, 0, m_btEncryptString.Length); m_csstream.FlushFinalBlock();

                m_strEncrypt = Convert.ToBase64String(m_stream.ToArray());

                m_stream.Close(); m_stream.Dispose();

                m_csstream.Close(); m_csstream.Dispose();
            }
            catch (IOException ex) { throw ex; }
            catch (CryptographicException ex) { throw ex; }
            catch (ArgumentException ex) { throw ex; }
            catch (Exception ex) { throw ex; }
            finally { m_AESProvider.Clear(); }

            return m_strEncrypt;
        }
        /// <summary>
        /// AES 解密(高级加密标准，是下一代的加密算法标准，速度快，安全级别高，目前 AES 标准的一个实现是 Rijndael 算法)
        /// </summary>
        /// <param name="DecryptString">待解密密文</param>
        /// <param name="DecryptKey">解密密钥</param>
        /// <returns></returns>
        public static string AESDecrypt(string DecryptString, string DecryptKey)
        {
            if (string.IsNullOrEmpty(DecryptString)) { throw (new Exception("密文不得为空")); }

            if (string.IsNullOrEmpty(DecryptKey)) { throw (new Exception("密钥不得为空")); }

            string m_strDecrypt = "";

            byte[] m_btIV = Convert.FromBase64String("Rkb4jvUy/ye7Cd7k89QQgQ==");

            Rijndael m_AESProvider = Rijndael.Create();

            try
            {
                byte[] m_btDecryptString = Convert.FromBase64String(DecryptString);

                MemoryStream m_stream = new MemoryStream();

                CryptoStream m_csstream = new CryptoStream(m_stream, m_AESProvider.CreateDecryptor(Encoding.Default.GetBytes(DecryptKey), m_btIV), CryptoStreamMode.Write);

                m_csstream.Write(m_btDecryptString, 0, m_btDecryptString.Length); m_csstream.FlushFinalBlock();

                m_strDecrypt = Encoding.Default.GetString(m_stream.ToArray());

                m_stream.Close(); m_stream.Dispose();

                m_csstream.Close(); m_csstream.Dispose();
            }
            catch (IOException ex) { throw ex; }
            catch (CryptographicException ex) { throw ex; }
            catch (ArgumentException ex) { throw ex; }
            catch (Exception ex) { throw ex; }
            finally { m_AESProvider.Clear(); }

            return m_strDecrypt;
        }

        #endregion

        #region RSA加密、解密、签名、验签
        /// <summary>
        /// 获取密钥新
        /// </summary>
        public static KeyValuePair<string, string> GetCryptKey()
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            return new KeyValuePair<string, string>(rsa.ToXmlString(true), rsa.ToXmlString(false));

        }
        /// <summary>
        /// RSA加密
        /// </summary>
        /// <param name="content">加密内容</param>
        /// <param name="publicKey">密钥</param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string EncryptRSA(string content, string publicKey, Encoding encoding)
        {
            if (string.IsNullOrEmpty(content)) throw new Exception("待加密内容不能为空");
            if (string.IsNullOrEmpty(publicKey)) throw new Exception("密钥不能为空");
            RSACryptoServiceProvider oRSA = new RSACryptoServiceProvider();
            byte[] messagebytes = null;
            if (encoding == null) 
                messagebytes = Encoding.Default.GetBytes(content);
            else
                messagebytes = encoding.GetBytes(content); //需要加密的数据 
            //公钥加密 
            RSACryptoServiceProvider oRSA1 = new RSACryptoServiceProvider();
            oRSA1.FromXmlString(publicKey); //加密要用到公钥所以导入公钥 
            byte[] AOutput = oRSA1.Encrypt(messagebytes, false); //AOutput 加密以后的数据 
            return Convert.ToBase64String(AOutput);
        }
        /// <summary>
        /// rsa 解密
        /// </summary>
        /// <param name="content">待解密内容</param>
        /// <param name="decryptKey">密钥</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static string DecryptRSA(string content, string decryptKey, Encoding encoding)
        {
            if (string.IsNullOrEmpty(content)) throw new Exception("待解密内容不能为空");
            if (string.IsNullOrEmpty(decryptKey)) throw new Exception("密钥不能为空");
            RSACryptoServiceProvider oRSA = new RSACryptoServiceProvider();
            oRSA.FromXmlString(decryptKey);
            byte[] AInput = oRSA.Decrypt(Convert.FromBase64String(content), false);
            if (encoding == null) return Encoding.Default.GetString(AInput);
            else
                return encoding.GetString(AInput);
        }
        /// <summary>
        /// RSA签名
        /// </summary>
        /// <param name="content">待签名内容</param>
        /// <param name="privatekey">密钥</param>
        /// <param name="encoding">编码格式 为空时，默认为default</param>
        /// <param name="sha">哈希flag  shatype</param>
        /// <returns></returns>
        public static string SignRSA(string content, string privatekey, Encoding encoding,string sha="SHA1")
        {
            if (string.IsNullOrEmpty(content)) throw new Exception("待解密内容不能为空");
            if (string.IsNullOrEmpty(privatekey)) throw new Exception("密钥不能为空");
            byte[] signdata = null;//需要签名的数据 
            if (encoding == null) signdata=Encoding.Default.GetBytes(content);
            else
                signdata= encoding.GetBytes(content);
            //私钥签名 
            RSACryptoServiceProvider oRSA3 = new RSACryptoServiceProvider();
            oRSA3.FromXmlString(privatekey);
            byte[] signresult = oRSA3.SignData(signdata, sha);
            return Convert.ToBase64String(signresult);
        }
        /// <summary>
        /// 验签
        /// </summary>
        /// <param name="singstring">签名后base64编码后字符串</param>
        /// <param name="content">签名内容</param>
        /// <param name="publicKey">公钥</param>
        /// <param name="encoding">编码格式</param>
        /// <param name="sha">哈希算法默认sha1 </param>
        /// <returns></returns>
        public static bool VerifyRSA(string singstring, string content, string publicKey, Encoding encoding, string sha = "SHA1")
        {
            if (string.IsNullOrEmpty(content)) throw new Exception("签名内容不能为空");
            if (string.IsNullOrEmpty(singstring)) throw new Exception("签名密文内容不能为空");
            if (string.IsNullOrEmpty(content)) throw new Exception("签名内容不能为空");
            if (string.IsNullOrEmpty(publicKey)) throw new Exception("密钥不能为空");
            byte[] signdata = null;//需要验签的数据 
            if (encoding == null) signdata = Encoding.Default.GetBytes(content);
            else
                signdata = encoding.GetBytes(content);
            //私钥签名 
            byte[] sourcedata = Convert.FromBase64String(singstring); //签名后内容进行base64解码
            RSACryptoServiceProvider oRSA3 = new RSACryptoServiceProvider();
            oRSA3.FromXmlString(publicKey);
            return oRSA3.VerifyData(signdata, sha, sourcedata);
        }

        #endregion
    }
}
