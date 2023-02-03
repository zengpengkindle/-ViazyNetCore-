using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Formatter.Response
{
    public static class AesHelper
    {
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="input"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static string EncryptCBC(this string input, string key, string iv)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(input))
                {
                    return input;
                }
                
                using(RijndaelManaged rijndaelManaged = new RijndaelManaged())
                {
                    rijndaelManaged.Mode = CipherMode.CBC;
                    rijndaelManaged.Padding = PaddingMode.PKCS7;
                    rijndaelManaged.FeedbackSize = 128;
                    rijndaelManaged.Key = Encoding.UTF8.GetBytes(key);
                    rijndaelManaged.IV = Encoding.UTF8.GetBytes(iv);
                    ICryptoTransform encryptor = rijndaelManaged.CreateEncryptor(rijndaelManaged.Key, rijndaelManaged.IV);
                    using(MemoryStream msEncrypt = new MemoryStream())
                    {
                        using(CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using(StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                            {
                                swEncrypt.Write(input);
                            }
                            byte[] bytes = msEncrypt.ToArray();
                            return Convert.ToBase64String(bytes);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                return "";
            }
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="input"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static string DecryptCBC(string input, string key, string iv)
        {
            if(string.IsNullOrWhiteSpace(input))
            {
                return input;
            }
            try
            {
                var buffer = Convert.FromBase64String(input);
                using(RijndaelManaged rijndaelManaged = new RijndaelManaged())
                {
                    rijndaelManaged.Mode = CipherMode.CBC;
                    rijndaelManaged.Padding = PaddingMode.PKCS7;
                    rijndaelManaged.FeedbackSize = 128;
                    rijndaelManaged.Key = Encoding.UTF8.GetBytes(key);
                    rijndaelManaged.IV = Encoding.UTF8.GetBytes(iv);
                    ICryptoTransform decryptor = rijndaelManaged.CreateDecryptor(rijndaelManaged.Key, rijndaelManaged.IV);
                    using(MemoryStream msEncrypt = new MemoryStream(buffer))
                    {
                        using(CryptoStream csEncrypt = new CryptoStream(msEncrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using(StreamReader srEncrypt = new StreamReader(csEncrypt))
                            {
                                return srEncrypt.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch(Exception)
            {
                return "";
            }
        }
    }
}
