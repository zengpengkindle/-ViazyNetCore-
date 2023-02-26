using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace System.Security
{
    /// <summary>
    /// 定义一个签名提供程序。
    /// </summary>
    public interface ISignatureProvider
    {
        /// <summary>
        /// 创建一个新的密钥。
        /// </summary>
        /// <returns>密钥。</returns>
        string NewSecret();
        /// <summary>
        /// 签名指定内容。
        /// </summary>
        /// <param name="body">内容。</param>
        /// <param name="timestamp">时间戳。</param>
        /// <param name="secret">密钥。</param>
        /// <returns>签名。</returns>
        string Sign(string body, long timestamp, string secret);
        /// <summary>
        /// 验证指定签名内容。
        /// </summary>
        /// <param name="body">内容。</param>
        /// <param name="timestamp">时间戳。</param>
        /// <param name="secret">密钥。</param>
        /// <param name="sign">签名。</param>
        /// <returns>验证成功返回 <see langword="true"/> 值，否则返回<see langword="false"/> 值。</returns>
        bool Verify(string body, long timestamp, string secret, string sign);
    }

    /// <summary>
    /// 表示一个基于 MD5 的签名提供程序。
    /// </summary>
    public class MD5SignatureProvider : ISignatureProvider
    {
        /// <inheritdoc />
        public string NewSecret() => Guid.NewGuid().ToString("N");

        /// <inheritdoc />
        public string Sign(string body, long timestamp, string secret)
        {
            var raw = body + "|" + timestamp + "|" + secret;
            return raw.ToMd5();
        }

        /// <inheritdoc />
        public bool Verify(string body, long timestamp, string secret, string sign)
        {
            return this.Sign(body, timestamp, secret) == sign;
        }
    }

    /// <summary>
    /// 表示一个基于 RSA 的签名提供程序。
    /// </summary>
    public class RSASignatureProvider : ISignatureProvider
    {
        /// <summary>
        /// 获取或设置一个值，表示 RSA 的密钥位数。
        /// </summary>
        public int KeySize { get; set; } = 2048;

        /// <summary>
        /// 获取或设置一个值，表示签名的 Hash 算法。
        /// </summary>
        public HashAlgorithmName Algorithm { get; set; } = HashAlgorithmName.SHA256;

        /// <inheritdoc />
        public string NewSecret()
        {
            return OpenRSA.Create(this.KeySize).ToPEM().ToPKCS8();
        }

        /// <inheritdoc />
        public string Sign(string body, long timestamp, string secret)
        {
            return OpenRSA.CreatePEM(secret).Sign(this.Algorithm, body + "|" + timestamp);
        }

        /// <inheritdoc />
        public bool Verify(string body, long timestamp, string secret, string sign)
        {
            return OpenRSA.CreatePEM(secret).Verify(this.Algorithm, sign, body + "|" + timestamp);
        }
    }
}
