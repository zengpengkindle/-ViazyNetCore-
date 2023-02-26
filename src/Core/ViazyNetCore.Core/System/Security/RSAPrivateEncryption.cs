using System.Numerics;
using System.Security.Cryptography;

namespace System.Security
{
    /// <summary>
    /// 表示一个 RSA 私钥加解密扩展。
    /// </summary>
    public static class RSAPrivateEncryption
    {
        /// <summary>
        /// 使用私钥加密数据。
        /// </summary>
        /// <param name="openRSA">一个基于 Open SSL 的操作实例。</param>
        /// <param name="data">原始数据。</param>
        /// <returns>加密后的数据。</returns>
        public static string PrivareEncrypt(this OpenRSA openRSA, string data)
        {
            if(openRSA is null)
            {
                throw new ArgumentNullException(nameof(openRSA));
            }

            if(data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            return Convert.ToBase64String(openRSA.PrivareEncrypt(openRSA.Encoding.GetBytes(data)));
        }

        /// <summary>
        /// 使用公钥解密数据。
        /// </summary>
        /// <param name="openRSA">一个基于 Open SSL 的操作实例。</param>
        /// <param name="data">加密后的数据。</param>
        /// <returns>原始数据。</returns>
        public static string PublicDecrypt(this OpenRSA openRSA, string data)
        {
            if(openRSA is null)
            {
                throw new ArgumentNullException(nameof(openRSA));
            }

            if(data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            return openRSA.Encoding.GetString(openRSA.PublicDecrypt(Convert.FromBase64String(data)));
        }

        /// <summary>
        /// 使用私钥加密数据。
        /// </summary>
        /// <param name="openRSA">一个基于 Open SSL 的操作实例。</param>
        /// <param name="data">原始数据。</param>
        /// <returns>加密后的数据。</returns>
        public static byte[] PrivareEncrypt(this OpenRSA openRSA, byte[] data)
        {
            if(openRSA is null)
            {
                throw new ArgumentNullException(nameof(openRSA));
            }

            if(data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            return openRSA.RSAObject.PrivareEncrypt(data);
        }

        /// <summary>
        /// 使用公钥解密数据。
        /// </summary>
        /// <param name="openRSA">一个基于 Open SSL 的操作实例。</param>
        /// <param name="data">加密后的数据。</param>
        /// <returns>原始数据。</returns>
        public static byte[] PublicDecrypt(this OpenRSA openRSA, byte[] data)
        {
            if(openRSA is null)
            {
                throw new ArgumentNullException(nameof(openRSA));
            }

            if(data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            return openRSA.RSAObject.PublicDecrypt(data);
        }

        /// <summary>
        /// 使用私钥加密数据。
        /// </summary>
        /// <param name="rsa">一个 <see cref="RSACryptoServiceProvider"/> 实例。</param>
        /// <param name="data">原始数据。</param>
        /// <returns>加密后的数据。</returns>
        private static byte[] PrivareEncrypt(this RSACryptoServiceProvider rsa, byte[] data)
        {
            if(rsa is null)
            {
                throw new ArgumentNullException(nameof(rsa));
            }

            if(data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if(rsa.PublicOnly)
                throw new InvalidOperationException("Private key is not loaded");

            var maxDataLength = (rsa.KeySize / 8) - 6;
            if(data.Length > maxDataLength)
                throw new ArgumentOutOfRangeException(nameof(data), string.Format(
                    "Maximum data length for the current key size ({0} bits) is {1} bytes (current length: {2} bytes)",
                    rsa.KeySize, maxDataLength, data.Length));

            // Add 4 byte padding to the data, and convert to BigInteger struct
            var numData = GetBig(AddPadding(data));

            var rsaParams = rsa.ExportParameters(true);
            var D = GetBig(rsaParams.D.MustBe());
            var Modulus = GetBig(rsaParams.Modulus.MustBe());
            var encData = BigInteger.ModPow(numData, D, Modulus);

            return encData.ToByteArray();
        }

        /// <summary>
        /// 使用公钥解密数据。
        /// </summary>
        /// <param name="rsa">一个 <see cref="RSACryptoServiceProvider"/> 实例。</param>
        /// <param name="data">加密后的数据。</param>
        /// <returns>原始数据。</returns>
        private static byte[] PublicDecrypt(this RSACryptoServiceProvider rsa, byte[] data)
        {
            if(data is null)
                throw new ArgumentNullException(nameof(rsa));

            var numEncData = new BigInteger(data);

            var rsaParams = rsa.ExportParameters(false);
            var Exponent = GetBig(rsaParams.Exponent.MustBe());
            var Modulus = GetBig(rsaParams.Modulus.MustBe());

            var decData = BigInteger.ModPow(numEncData, Exponent, Modulus).ToByteArray();
            var result = new byte[decData.Length - 1];
            Array.Copy(decData, result, result.Length);
            result = RemovePadding(result);

            Array.Reverse(result);
            return result;
        }

        private static BigInteger GetBig(byte[] data)
        {
            var inArr = (byte[])data.Clone();
            Array.Reverse(inArr);  // Reverse the byte order
            var final = new byte[inArr.Length + 1];  // Add an empty byte at the end, to simulate unsigned BigInteger (no negatives!)
            Array.Copy(inArr, final, inArr.Length);

            return new BigInteger(final);
        }

        private static byte[] AddPadding(byte[] data)
        {
            var rnd = new Random();
            var paddings = new byte[4];
            rnd.NextBytes(paddings);
            paddings[0] = (byte)(paddings[0] | 128);

            var results = new byte[data.Length + 4];

            Array.Copy(paddings, results, 4);
            Array.Copy(data, 0, results, 4, data.Length);
            return results;
        }

        private static byte[] RemovePadding(byte[] data)
        {
            var results = new byte[data.Length - 4];
            Array.Copy(data, results, results.Length);
            return results;
        }
    }
}
