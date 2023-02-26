using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace System.Security
{
    /// <summary>
	/// 表示一个基于 OpenSSL 的操作。
	/// </summary>
	public class OpenRSA
    {
        /// <summary>
        /// 获取 <see cref="RSACryptoServiceProvider"/> 的实例。
        /// </summary>
        public RSACryptoServiceProvider RSAObject { get; }

        /// <summary>
        /// 获取字符串编码格式。
        /// </summary>
        public Encoding Encoding { get; set; } = Encoding.UTF8;

        /// <summary>
        /// 获取密钥的位数。
        /// </summary>
        public int KeySize => this.RSAObject.KeySize;

        /// <summary>
        /// 获取一个值，表示当前密钥为私钥。
        /// </summary>
        public bool HasPrivate => !this.RSAObject.PublicOnly;


        /// <summary>
        /// 创建一个 <see cref="OpenRSA"/> 类的新实例。
        /// </summary>
        /// <param name="keySize">密钥位数。</param>
        /// <returns>一个 <see cref="OpenRSA"/> 类的新实例。</returns>
        public static OpenRSA Create(int keySize) => new(keySize);

        /// <summary>
        /// 创建一个 <see cref="OpenRSA"/> 类的新实例。
        /// </summary>
        /// <param name="xml">格式为 XML 的密钥对。</param>
        /// <returns>一个 <see cref="OpenRSA"/> 类的新实例。</returns>
        public static OpenRSA CreateXML(string xml) => new(xml, false);

        /// <summary>
        /// 创建一个 <see cref="OpenRSA"/> 类的新实例。
        /// </summary>
        /// <param name="pem">格式为 PEM 的密钥对。</param>
        /// <returns>一个 <see cref="OpenRSA"/> 类的新实例。</returns>
        public static OpenRSA CreatePEM(string pem) => new(pem, true);

        private OpenRSA(int keySize)
        {
            this.RSAObject = new RSACryptoServiceProvider(keySize);
        }

        private OpenRSA(string content, bool isPemContent)
        {
            if(content is null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if(isPemContent)
            {
                this.RSAObject = PEMParameters.FromPEM(content).CreateRSAObject();
            }
            else
            {
                this.RSAObject = new RSACryptoServiceProvider();
                this.RSAObject.FromXmlString(content);
            }
        }

        /// <summary>
        /// 导出 XML 格式的密钥对。
        /// </summary>
        /// <param name="publicOnly">是否仅导出公钥。</param>
        /// <returns>格式为 XML 的密钥对。</returns>
        public string ToXML(bool publicOnly = false)
        {
            return this.RSAObject.ToXmlString(!this.RSAObject.PublicOnly && !publicOnly);
        }

        /// <summary>
        /// 导出 PEM 格式的密钥对。
        /// </summary>
        /// <param name="publicOnly">是否仅导出公钥。</param>
        /// <returns>格式为 PEM 的密钥对。</returns>
        public PEMParameters ToPEM(bool publicOnly = false)
        {
            return new PEMParameters(this.RSAObject, publicOnly);
        }

        /// <summary>
        /// 使用公钥加密数据。
        /// <para>公钥和私钥都允许加密。</para>
        /// </summary>
        /// <param name="data">原始数据。</param>
        /// <returns>加密后的数据。</returns>
        public string Encrypt(string data)
        {
            if(data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            return Convert.ToBase64String(this.Encrypt(this.Encoding.GetBytes(data)));
        }

        /// <summary>
        /// 使用公钥加密数据。
        /// <para>公钥和私钥都允许加密。</para>
        /// </summary>
        /// <param name="data">原始数据。</param>
        /// <returns>加密后的数据。</returns>
        public byte[] Encrypt(byte[] data)
        {
            if(data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var blockLen = this.RSAObject.KeySize / 8 - 11;
            if(data.Length <= blockLen)
            {
                return this.RSAObject.Encrypt(data, false);
            }

            using var dataStream = new MemoryStream(data);
            using var enStream = new MemoryStream();

            var buffer = new byte[blockLen];
            var len = dataStream.Read(buffer, 0, blockLen);

            while(len > 0)
            {
                var block = new byte[len];
                Array.Copy(buffer, 0, block, 0, len);

                var enBlock = this.RSAObject.Encrypt(block, false);
                enStream.Write(enBlock, 0, enBlock.Length);

                len = dataStream.Read(buffer, 0, blockLen);
            }

            return enStream.ToArray();
        }

        /// <summary>
        /// 使用私钥解密数据。
        /// <para>只有私钥都才允许解密。</para>
        /// </summary>
        /// <param name="data">加密的数据。</param>
        /// <returns>解密后的数据。</returns>
        public string Decrypt(string data)
        {
            if(data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            return this.Encoding.GetString(this.Decrypt(Convert.FromBase64String(data)));
        }

        /// <summary>
        /// 使用私钥解密数据。
        /// <para>只有私钥都才允许解密。</para>
        /// </summary>
        /// <param name="data">加密的数据。</param>
        /// <returns>解密后的数据。</returns>
        public byte[] Decrypt(byte[] data)
        {
            if(data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var blockLen = this.RSAObject.KeySize / 8;
            if(data.Length <= blockLen)
            {
                return this.RSAObject.Decrypt(data, false);
            }

            using var dataStream = new MemoryStream(data);
            using var deStream = new MemoryStream();
            var buffer = new byte[blockLen];
            var len = dataStream.Read(buffer, 0, blockLen);

            while(len > 0)
            {
                var block = new byte[len];
                Array.Copy(buffer, 0, block, 0, len);

                var deBlock = this.RSAObject.Decrypt(block, false);
                deStream.Write(deBlock, 0, deBlock.Length);

                len = dataStream.Read(buffer, 0, blockLen);
            }

            return deStream.ToArray();

        }

        /// <summary>
        /// 将指定的数据进行签名。
        /// </summary>
        /// <param name="hash">签名的 Hash 算法。</param>
        /// <param name="data">签名的数据。</param>
        /// <returns>签名字符串。</returns>
        public string Sign(HashAlgorithmName hash, string data)
        {
            if(data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }
            return Convert.ToBase64String(this.Sign(hash, this.Encoding.GetBytes(data)));
        }

        /// <summary>
        /// 将指定的数据进行签名。
        /// </summary>
        /// <param name="hash">签名的 Hash 算法。</param>
        /// <param name="data">签名的数据。</param>
        /// <returns>签名字符串。</returns>
        public byte[] Sign(HashAlgorithmName hash, byte[] data)
        {
            if(data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            return this.RSAObject.SignData(data, hash.Name.MustBe());
        }

        /// <summary>
        /// 验证指定的签名是否有效。
        /// </summary>
        /// <param name="hash">签名的 Hash 算法。</param>
        /// <param name="signature">签名字符串。</param>
        /// <param name="data">签名的数据。</param>
        /// <returns>验证成功返回 <see langword="true"/> 值，否则返回 <see langword="false"/> 值。</returns>
        public bool Verify(HashAlgorithmName hash, string signature, string data)
        {
            return this.Verify(hash, Convert.FromBase64String(signature), this.Encoding.GetBytes(data));
        }

        /// <summary>
        /// 验证指定的签名是否有效。
        /// </summary>
        /// <param name="hash">签名的 Hash 算法。</param>
        /// <param name="signature">签名字符串。</param>
        /// <param name="data">签名的数据。</param>
        /// <returns>验证成功返回 <see langword="true"/> 值，否则返回 <see langword="false"/> 值。</returns>
        public bool Verify(HashAlgorithmName hash, byte[] signature, byte[] data)
        {
            return this.RSAObject.VerifyData(data, hash.Name.MustBe(), signature);
        }

    }
}
