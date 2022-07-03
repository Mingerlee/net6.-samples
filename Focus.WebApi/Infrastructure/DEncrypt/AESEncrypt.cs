using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.DEncrypt
{
    public class AESEncrypt
    {
        /// <summary>
        /// 加密密码用
        /// </summary>
        public const string pwdKey = "aseROugdayvEtrvf";

        public const string infoKey = "aseROugdayvEtree";


        /// <summary>
        /// AES加密方式(ECB模式)
        /// </summary>
        /// <param name="toEncrypt">明文</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static string Encrypt(string toEncrypt, string key)
        {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        /// <summary>
        /// AES解密方式(ECB模式)
        /// </summary>
        /// <param name="toDecrypt">密文</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static string Decrypt(string toDecrypt, byte[] key)
        {
            byte[] keyArray = key;// UTF8Encoding.UTF8.GetBytes(key);
            byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return UTF8Encoding.UTF8.GetString(resultArray);
        }
        /// <summary>
        /// AES解密方式(ECB模式)
        /// </summary>
        /// <param name="toDecrypt">密文</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static string Decrypt(string toDecrypt, string key)
        {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
            byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        /// <summary>
        /// 随机生成密钥
        /// </summary>
        /// <returns></returns>
        public static string GetIv(int n)
        {
            char[] arrChar = new char[]{
           'a','b','d','c','e','f','g','h','i','j','k','l','m','n','p','r','q','s','t','u','v','w','z','y','x',
           '0','1','2','3','4','5','6','7','8','9',
           'A','B','C','D','E','F','G','H','I','J','K','L','M','N','Q','P','R','T','S','V','U','W','X','Y','Z'
          };

            StringBuilder num = new StringBuilder();

            Random rnd = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < n; i++)
            {
                num.Append(arrChar[rnd.Next(0, arrChar.Length)].ToString());
                System.Threading.Thread.Sleep(10);
            }

            return num.ToString();
        }
        /// <summary>
        /// 字符串集合
        /// </summary>
        private static readonly char[] Constants =
        {
            '0','1','A','B','2','3','4','5','6','7','m','n','o','Y','Z','p','q','8','9',
            'a','b','c','d','e','f','j','k','L','M','N','O','P','l','r','s','t','u','v','V','W','X','w','x','y','z',
            'C','D','E','F','G','H','I','J','K','g','h','i','Q','R','S','T','U'
        };

        /// <summary>
        /// 生成私钥 或者直接使用16位MD5
        /// </summary>
        /// <returns></returns>

        public string CreateIv(int length = 16)
        {
            StringBuilder newRandom = new System.Text.StringBuilder(length);
            Random rd = new Random(Guid.NewGuid().GetHashCode());
            for (int i = 0; i < length; i++)
            {
                newRandom.Append(Constants[rd.Next(46)]);
            }
            return newRandom.ToString();
        }
        public static string GetMd5(string str)
        {
            MD5 m = MD5.Create();
            var bytes = m.ComputeHash(Encoding.UTF8.GetBytes(str));
            string res = BitConverter.ToString(bytes);
            res = res.ToLower().Replace("-", "");
            return res;
        }
    }
}
