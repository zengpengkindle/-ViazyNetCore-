using System.Buffers;
using System.Security.Cryptography;
using System.Text;

namespace System
{
    /// <summary>
    /// 表示数据安全处理。
    /// </summary>
    public static class DataSecurity
    {
        /// <summary>
        /// 指定哈希算法，使用 <see cref="Encoding.UTF8"/> 编码方式哈希指定的文本。
        /// </summary>
        /// <param name="alog">哈希算法。</param>
        /// <param name="text">需哈希的字符串。</param>
        /// <returns>哈希后的字符串。</returns>
        public static string Crypto(HashAlgorithms alog, string text)
        {
            return BitConverter.ToString(Crypto(alog, Encoding.UTF8.GetBytes(text))).Replace("-", string.Empty).ToLower();
        }

        /// <summary>
        /// 指定哈希算法，哈希指定的。
        /// </summary>
        /// <param name="alog">哈希算法。</param>
        /// <param name="bytes">要计算其哈希代码的输入。</param>
        /// <returns>计算所得的哈希代码。</returns>
        public static byte[] Crypto(HashAlgorithms alog, byte[] bytes)
        {
            using var algorithm = CryptoConfig.CreateFromName(alog.ToString()).MustBe<HashAlgorithm>();
            return algorithm.ComputeHash(bytes);
        }

        /// <summary>
        /// 生产成指定字符串，生成 32位加盐值，并返回 44 位加盐散列后的文本。
        /// </summary>
        /// <param name="text">原始文本。</param>
        /// <param name="salt">加盐值。</param>
        /// <returns> 44 位加盐散列后的文本。</returns>
        public static string GenerateSaltedHash(string text, out Guid salt) => GenerateSaltedHash(text, salt = Guid.NewGuid());

        /// <summary>
        /// 生产成指定字符串和加盐值，并返回 44 位加盐散列后的文本。
        /// </summary>
        /// <param name="text">原始文本。</param>
        /// <param name="salt">加盐值。</param>
        /// <returns> 44 位加盐散列后的文本。</returns>
        public static string GenerateSaltedHash(string text, string salt) => GenerateSaltedHash(text, Guid.Parse(salt));

        /// <summary>
        /// 生产成指定字符串和加盐值，并返回 44 位加盐散列后的文本。
        /// </summary>
        /// <param name="text">原始文本。</param>
        /// <param name="salt">加盐值。</param>
        /// <returns> 44 位加盐散列后的文本。</returns>
        public static string GenerateSaltedHash(string text, Guid salt)
        {
            var bytes1 = Encoding.UTF8.GetBytes(text+ salt.ToString("N"));
            var hash = ArrayPool<byte>.Shared.Rent(bytes1.Length);
            try
            {
                bytes1.CopyTo(hash, 0);
                //bytes2.CopyTo(hash, bytes1.Length);

                return Convert.ToBase64String(Crypto(HashAlgorithms.SHA256, hash));
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(hash);
            }
        }

        /// <summary>
        /// 使用 AES 加密。
        /// </summary>
        /// <param name="text">原文。</param>
        /// <param name="key">密钥。</param>
        /// <returns>使用 BASE64 的密文。</returns>
        public static string Encrypt(string text, string key)
        {
            using var aesAlg = Aes.Create();
            var kbs = key.GetBytes();
            aesAlg.Key = Crypto(HashAlgorithms.SHA256, kbs);
            aesAlg.IV = Crypto(HashAlgorithms.MD5, kbs);

            using var encryptor = aesAlg.CreateEncryptor();
            using var msEncrypt = new MemoryStream();

            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            using (var swEncrypt = new StreamWriter(csEncrypt))
                swEncrypt.Write(text);

            return Convert.ToBase64String(msEncrypt.ToArray());
        }

        /// <summary>
        /// 使用 AES 解密。
        /// </summary>
        /// <param name="base64Text">一个 BASE64 的密文。</param>
        /// <param name="key">密钥。</param>
        /// <returns>原文。</returns>
        public static string Decrypt(string base64Text, string key)
        {
            using var aesAlg = Aes.Create();
            aesAlg.Key = Crypto(HashAlgorithms.SHA256, Encoding.UTF8.GetBytes(key));
            aesAlg.IV = Crypto(HashAlgorithms.MD5, Encoding.UTF8.GetBytes(key));

            using var decryptor = aesAlg.CreateDecryptor();
            using var msDecrypt = new MemoryStream(Convert.FromBase64String(base64Text));
            using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using var srDecrypt = new StreamReader(csDecrypt);

            return srDecrypt.ReadToEnd();
        }
    }


    /// <summary>
    /// 表示安全哈希算法。
    /// </summary>
    public enum HashAlgorithms
    {
        /// <summary>
        /// 使用 <see cref="Security.Cryptography.SHA1"/> 哈希函数计算基于哈希值的消息验证代码 (HMAC)。
        /// </summary>
        SHA1,
        /// <summary>
        /// 使用 <see cref="Security.Cryptography.SHA256"/> 哈希函数计算基于哈希值的消息验证代码 (HMAC)。
        /// </summary>
        SHA256,
        /// <summary>
        /// 使用 <see cref="Security.Cryptography.SHA384"/> 哈希函数计算基于哈希值的消息验证代码 (HMAC)。
        /// </summary>
        SHA384,
        /// <summary>
        /// 使用 <see cref="Security.Cryptography.SHA512"/> 哈希函数计算基于哈希值的消息验证代码 (HMAC)。
        /// </summary>
        SHA512,
        /// <summary>
        ///  提供 MD5（消息摘要 5）128 位哈希算法的实现。
        /// </summary>
        MD5
    }
}
