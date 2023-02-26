using System;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace System.Security
{
    /// <summary>
	/// 表示一个基于 PEM 格式的 RSA 密钥对。
	/// </summary>
	public sealed class PEMParameters
    {
        private static readonly Regex XML_EXP = new("\\s*<RSAKeyValue>([<>\\/\\+=\\w\\s]+)</RSAKeyValue>\\s*", RegexOptions.Compiled);
        private static readonly Regex XML_TAG_EXP = new("<(.+?)>\\s*([^<]+?)\\s*</", RegexOptions.Compiled);

        private static readonly Regex PEM_CODE = new(@"--+.+?--+|\s+", RegexOptions.Compiled);
        private static readonly byte[] SEQ_OID = new byte[] { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
        private static readonly byte[] VER = new byte[] { 0x02, 0x01, 0x00 };

        private byte[]? _keyModulus;
        private byte[]? _keyExponent;
        private byte[]? _keyD;

        private byte[]? _valP;
        private byte[]? _valQ;
        private byte[]? _valDP;
        private byte[]? _valDQ;
        private byte[]? _valInverseQ;

        private PEMParameters() { }

        internal PEMParameters(RSACryptoServiceProvider rsa, bool convertToPublic = false)
        {
            var isPublic = convertToPublic || rsa.PublicOnly;
            var param = rsa.ExportParameters(!isPublic);

            this._keyModulus = param.Modulus;
            this._keyExponent = param.Exponent;

            if (!isPublic)
            {
                this._keyD = param.D;
                this._valP = param.P;
                this._valQ = param.Q;
                this._valDP = param.DP;
                this._valDQ = param.DQ;
                this._valInverseQ = param.InverseQ;
            }
        }

        private PEMParameters(byte[] modulus, byte[] exponent, byte[] dOrNull)
        {
            this._keyModulus = modulus;//modulus
            this._keyExponent = exponent;//publicExponent

            if (dOrNull is not null)
            {
                this._keyD = BigL(dOrNull, modulus.Length);//privateExponent

                //反推P、Q
                var n = BigX(modulus);
                var e = BigX(exponent);
                var d = BigX(dOrNull);
                var p = FindFactor(e, d, n);
                var q = n / p;
                if (p.CompareTo(q) > 0)
                {
                    var t = p;
                    p = q;
                    q = t;
                }
                var exp1 = d % (p - BigInteger.One);
                var exp2 = d % (q - BigInteger.One);
                var coeff = BigInteger.ModPow(q, p - 2, p);

                int keyLen = modulus.Length / 2;
                this._valP = BigL(BigB(p), keyLen);//prime1
                this._valQ = BigL(BigB(q), keyLen);//prime2
                this._valDP = BigL(BigB(exp1), keyLen);//exponent1
                this._valDQ = BigL(BigB(exp2), keyLen);//exponent2
                this._valInverseQ = BigL(BigB(coeff), keyLen);//coefficient
            }
        }

        /// <summary>
        /// 创建一个 <see cref="RSACryptoServiceProvider"/> 的新实例。
        /// </summary>
        /// <returns>新的 <see cref="RSACryptoServiceProvider"/> 的新实例。</returns>
        public RSACryptoServiceProvider CreateRSAObject()
        {
            var rsa = new RSACryptoServiceProvider();

            var param = new RSAParameters
            {
                Modulus = this._keyModulus,
                Exponent = this._keyExponent
            };

            if (this._keyD is not null)
            {
                param.D = this._keyD;
                param.P = this._valP;
                param.Q = this._valQ;
                param.DP = this._valDP;
                param.DQ = this._valDQ;
                param.InverseQ = this._valInverseQ;
            }
            rsa.ImportParameters(param);
            return rsa;
        }

        private static BigInteger BigX(byte[] bigb)
        {
            if (bigb[0] > 0x7F)
            {
                var c = new byte[bigb.Length + 1];
                Array.Copy(bigb, 0, c, 1, bigb.Length);
                bigb = c;
            }
            return new BigInteger(bigb.Reverse().ToArray());//C#的二进制是反的
        }

        private static byte[] BigB(BigInteger bigx)
        {
            var val = bigx.ToByteArray().Reverse().ToArray();//C#的二进制是反的
            if (val[0] == 0)
            {
                var c = new byte[val.Length - 1];
                Array.Copy(val, 1, c, 0, c.Length);
                val = c;
            }
            return val;
        }

        private static byte[] BigL(byte[] bytes, int keyLen)
        {
            if (keyLen - bytes.Length == 1)
            {
                byte[] c = new byte[bytes.Length + 1];
                Array.Copy(bytes, 0, c, 1, bytes.Length);
                bytes = c;
            }
            return bytes;
        }

        private static BigInteger FindFactor(BigInteger e, BigInteger d, BigInteger n)
        {
            var edMinus1 = e * d - BigInteger.One;
            var s = -1;
            if (edMinus1 != BigInteger.Zero)
            {
                s = (int)(BigInteger.Log(edMinus1 & -edMinus1) / BigInteger.Log(2));
            }
            var t = edMinus1 >> s;

            var now = GA.Now.Ticks;
            for (var aInt = 2; true; aInt++)
            {
                if (aInt % 10 == 0 && GA.Now.Ticks - now > 3000 * 10000)
                {
                    throw new TimeoutException("Find RSA 'P' value parameter is time out.");
                }

                var aPow = BigInteger.ModPow(new BigInteger(aInt), t, n);
                for (var i = 1; i <= s; i++)
                {
                    if (aPow == BigInteger.One)
                    {
                        break;
                    }
                    if (aPow == n - BigInteger.One)
                    {
                        break;
                    }
                    var aPowSquared = aPow * aPow % n;
                    if (aPowSquared == BigInteger.One)
                    {
                        return BigInteger.GreatestCommonDivisor(aPow - BigInteger.One, n);
                    }
                    aPow = aPowSquared;
                }
            }
        }

        /// <summary>
        /// 使用 PEM 格式的密钥对创建一个 <see cref="PEMParameters"/> 的新实例。
        /// </summary>
        /// <param name="pem">格式为 PEM 的密钥对。</param>
        /// <returns>一个 <see cref="PEMParameters"/> 的新实例。</returns>
        public static PEMParameters FromPEM(string pem)
        {
            if (pem is null)
            {
                throw new ArgumentNullException(nameof(pem));
            }

            var param = new PEMParameters();
            var base64 = string.Join(string.Empty, PEM_CODE.Replace(pem, string.Empty).Split('\r', '\n', ' '));
            var data = Convert.FromBase64String(base64);
            var idx = 0;

            //读取长度
            int readLen(byte first)
            {
                if (data[idx] == first)
                {
                    idx++;
                    if (data[idx] == 0x81)
                    {
                        idx++;
                        return data[idx++];
                    }
                    else if (data[idx] == 0x82)
                    {
                        idx++;
                        return (((int)data[idx++]) << 8) + data[idx++];
                    }
                    else if (data[idx] < 0x80)
                    {
                        return data[idx++];
                    }
                }
                throw new FormatException("Invalid PEM content.");
            }
            //读取块数据
            byte[] readBlock()
            {
                var len = readLen(0x02);
                if (data[idx] == 0x00)
                {
                    idx++;
                    len--;
                }
                var val = new byte[len];
                for (var i = 0; i < len; i++)
                {
                    val[i] = data[idx + i];
                }
                idx += len;
                return val;
            }
            //比较data从idx位置开始是否是byts内容
            bool eq(byte[] byts)
            {
                for (var i = 0; i < byts.Length; i++, idx++)
                {
                    if (idx >= data.Length)
                    {
                        return false;
                    }
                    if (byts[i] != data[idx])
                    {
                        return false;
                    }
                }
                return true;
            }

            if (pem.Contains("PUBLIC KEY"))
            {
                /****使用公钥****/
                //读取数据总长度
                readLen(0x30);

                //检测PKCS8
                var idx2 = idx;
                if (eq(SEQ_OID))
                {
                    //读取1长度
                    readLen(0x03);
                    idx++;//跳过0x00
                          //读取2长度
                    readLen(0x30);
                }
                else
                {
                    idx = idx2;
                }

                //Modulus
                param._keyModulus = readBlock();

                //Exponent
                param._keyExponent = readBlock();
            }
            else if (pem.Contains("PRIVATE KEY"))
            {
                /****使用私钥****/
                //读取数据总长度
                readLen(0x30);

                //读取版本号
                if (!eq(VER))
                {
                    throw new FormatException("Unkonw PEM version.");
                }

                //检测PKCS8
                var idx2 = idx;
                if (eq(SEQ_OID))
                {
                    //读取1长度
                    readLen(0x04);
                    //读取2长度
                    readLen(0x30);

                    //读取版本号
                    if (!eq(VER))
                    {
                        throw new FormatException("Unkonw PEM version.");
                    }
                }
                else
                {
                    idx = idx2;
                }

                //读取数据
                param._keyModulus = readBlock();
                param._keyExponent = readBlock();

                var keyLen = param._keyModulus.Length;
                param._keyD = BigL(readBlock(), keyLen);
                keyLen /= 2;

                param._valP = BigL(readBlock(), keyLen);
                param._valQ = BigL(readBlock(), keyLen);
                param._valDP = BigL(readBlock(), keyLen);
                param._valDQ = BigL(readBlock(), keyLen);
                param._valInverseQ = BigL(readBlock(), keyLen);
            }
            else
            {
                throw new FormatException("Invalid PEM header.");
            }

            return param;
        }

        /// <summary>
        /// 导出 PKCS#1 格式的 PEM 格式的密钥对。
        /// </summary>
        /// <param name="publicOnly">是否仅导出公钥。</param>
        /// <returns>一个 PEM 格式的密钥对。</returns>
        public string ToPKCS1(bool publicOnly = false)
        {
            return this.ToPEM(publicOnly, false, false);
        }

        /// <summary>
        /// 导出 PKCS#8 格式的 PEM 格式的密钥对。
        /// </summary>
        /// <param name="publicOnly">是否仅导出公钥。</param>
        /// <returns>一个 PEM 格式的密钥对。</returns>
        public string ToPKCS8(bool publicOnly = false)
        {
            return this.ToPEM(publicOnly, true, true);
        }

        private string ToPEM(bool publicOnly, bool privateUsePKCS8, bool publicUsePKCS8)
        {
            //https://www.jianshu.com/p/25803dd9527d
            //https://www.cnblogs.com/ylz8401/p/8443819.html
            //https://blog.csdn.net/jiayanhui2877/article/details/47187077
            //https://blog.csdn.net/xuanshao_/article/details/51679824
            //https://blog.csdn.net/xuanshao_/article/details/51672547

            using var ms = new MemoryStream();
            //写入一个长度字节码
            void writeLenByte(int len)
            {
                if (len < 0x80)
                {
                    ms.WriteByte((byte)len);
                }
                else if (len <= 0xff)
                {
                    ms.WriteByte(0x81);
                    ms.WriteByte((byte)len);
                }
                else
                {
                    ms.WriteByte(0x82);
                    ms.WriteByte((byte)(len >> 8 & 0xff));
                    ms.WriteByte((byte)(len & 0xff));
                }
            }
            //写入一块数据
            void writeBlock(byte[]? byts)
            {
                byts = byts.MustBe();
                var addZero = (byts[0] >> 4) >= 0x8;
                ms.WriteByte(0x02);
                var len = byts.Length + (addZero ? 1 : 0);
                writeLenByte(len);

                if (addZero)
                {
                    ms.WriteByte(0x00);
                }
                ms.Write(byts, 0, byts.Length);
            }
            //根据后续内容长度写入长度数据
            byte[] writeLen(int index, byte[] byts)
            {
                var len = byts.Length - index;

                ms.SetLength(0);
                ms.Write(byts, 0, index);
                writeLenByte(len);
                ms.Write(byts, index, len);

                return ms.ToArray();
            }

            static void writeAll(MemoryStream stream, byte[] byts)
            {
                stream.Write(byts, 0, byts.Length);
            }

            static string TextBreak(string text, int line)
            {
                var idx = 0;
                var len = text.Length;
                var builder = new StringBuilder();
                try
                {
                    while (idx < len)
                    {
                        if (idx > 0)
                        {
                            builder.Append('\n');
                        }
                        if (idx + line >= len)
                        {
                            builder.Append(text[idx..]);
                        }
                        else
                        {
                            builder.Append(text.Substring(idx, line));
                        }
                        idx += line;
                    }
                    return builder.ToString();
                }
                finally
                {
                }
            }

            if (this._keyD is null || publicOnly)
            {
                /****生成公钥****/

                //写入总字节数，不含本段长度，额外需要24字节的头，后续计算好填入
                ms.WriteByte(0x30);
                var index1 = (int)ms.Length;

                //PKCS8 多一段数据
                int index2 = -1, index3 = -1;
                if (publicUsePKCS8)
                {
                    //固定内容
                    // encoded OID sequence for PKCS #1 rsaEncryption szOID_RSA_RSA = "1.2.840.113549.1.1.1"
                    writeAll(ms, SEQ_OID);

                    //从0x00开始的后续长度
                    ms.WriteByte(0x03);
                    index2 = (int)ms.Length;
                    ms.WriteByte(0x00);

                    //后续内容长度
                    ms.WriteByte(0x30);
                    index3 = (int)ms.Length;
                }

                //写入Modulus
                writeBlock(this._keyModulus);

                //写入Exponent
                writeBlock(this._keyExponent);


                //计算空缺的长度
                var byts = ms.ToArray();

                if (index2 != -1)
                {
                    byts = writeLen(index3, byts);
                    byts = writeLen(index2, byts);
                }
                byts = writeLen(index1, byts);

                var flag = " PUBLIC KEY";
                if (!publicUsePKCS8)
                {
                    flag = " RSA" + flag;
                }
                return "-----BEGIN" + flag + "-----\n" + TextBreak(Convert.ToBase64String(byts), 64) + "\n-----END" + flag + "-----";
            }
            else
            {
                /****生成私钥****/

                //写入总字节数，后续写入
                ms.WriteByte(0x30);
                var index1 = (int)ms.Length;

                //写入版本号
                writeAll(ms, VER);

                //PKCS8 多一段数据
                int index2 = -1, index3 = -1;
                if (privateUsePKCS8)
                {
                    //固定内容
                    writeAll(ms, SEQ_OID);

                    //后续内容长度
                    ms.WriteByte(0x04);
                    index2 = (int)ms.Length;

                    //后续内容长度
                    ms.WriteByte(0x30);
                    index3 = (int)ms.Length;

                    //写入版本号
                    writeAll(ms, VER);
                }

                //写入数据
                writeBlock(this._keyModulus);
                writeBlock(this._keyExponent);
                writeBlock(this._keyD);
                writeBlock(this._valP);
                writeBlock(this._valQ);
                writeBlock(this._valDP);
                writeBlock(this._valDQ);
                writeBlock(this._valInverseQ);


                //计算空缺的长度
                var byts = ms.ToArray();

                if (index2 != -1)
                {
                    byts = writeLen(index3, byts);
                    byts = writeLen(index2, byts);
                }
                byts = writeLen(index1, byts);


                var flag = " PRIVATE KEY";
                if (!privateUsePKCS8)
                {
                    flag = " RSA" + flag;
                }
                return "-----BEGIN" + flag + "-----\n" + TextBreak(Convert.ToBase64String(byts), 64) + "\n-----END" + flag + "-----";
            }
        }

        /// <summary>
        /// 使用 XML 格式的密钥对创建一个 <see cref="PEMParameters"/> 的新实例。
        /// </summary>
        /// <param name="xml">格式为 PEM 的密钥对。</param>
        /// <returns>一个 <see cref="PEMParameters"/> 的新实例。</returns>
        public static PEMParameters FromXML(string xml)
        {
            var rtv = new PEMParameters();

            var xmlM = XML_EXP.Match(xml);
            if (!xmlM.Success)
            {
                throw new InvalidDataException("Invalid xml content.");
            }

            var tagM = XML_TAG_EXP.Match(xmlM.Groups[1].Value);
            while (tagM.Success)
            {
                var tag = tagM.Groups[1].Value;
                var b64 = tagM.Groups[2].Value;
                var val = Convert.FromBase64String(b64);
                switch (tag)
                {
                    case "Modulus": rtv._keyModulus = val; break;
                    case "Exponent": rtv._keyExponent = val; break;
                    case "D": rtv._keyD = val; break;

                    case "P": rtv._valP = val; break;
                    case "Q": rtv._valQ = val; break;
                    case "DP": rtv._valDP = val; break;
                    case "DQ": rtv._valDQ = val; break;
                    case "InverseQ": rtv._valInverseQ = val; break;
                }
                tagM = tagM.NextMatch();
            }

            if (rtv._keyModulus is null || rtv._keyExponent is null)
            {
                throw new InvalidDataException("Invalid xml public key.");
            }
            if (rtv._keyD is not null)
            {
                if (rtv._valP is null || rtv._valQ is null || rtv._valDP is null || rtv._valDQ is null || rtv._valInverseQ is null)
                {
                    return new PEMParameters(rtv._keyModulus, rtv._keyExponent, rtv._keyD);
                }
            }

            return rtv;
        }

        private static string ToBase64String(byte[]? bytes)
        {
            if (bytes is null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// 导出 XML 格式的密钥对。
        /// </summary>
        /// <param name="publicOnly">是否仅导出公钥。</param>
        /// <returns>一个 XML 格式的密钥对。</returns>
        public string ToXML(bool publicOnly = false)
        {
            var builder = new StringBuilder();
            try
            {
                builder.Append("<RSAKeyValue>");
                builder.Append("<Modulus>" + ToBase64String(this._keyModulus) + "</Modulus>");
                builder.Append("<Exponent>" + ToBase64String(this._keyExponent) + "</Exponent>");
                if (this._keyD is not null && !publicOnly)
                {
                    /****生成私钥****/
                    builder.Append("<P>" + ToBase64String(this._valP) + "</P>");
                    builder.Append("<Q>" + ToBase64String(this._valQ) + "</Q>");
                    builder.Append("<DP>" + ToBase64String(this._valDP) + "</DP>");
                    builder.Append("<DQ>" + ToBase64String(this._valDQ) + "</DQ>");
                    builder.Append("<InverseQ>" + ToBase64String(this._valInverseQ) + "</InverseQ>");
                    builder.Append("<D>" + ToBase64String(this._keyD) + "</D>");
                }

                builder.Append("</RSAKeyValue>");
                return builder.ToString();
            }
            finally
            {
            }
        }
    }
}
