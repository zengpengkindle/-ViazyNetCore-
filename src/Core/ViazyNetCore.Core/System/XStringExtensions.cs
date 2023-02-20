using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace System
{
    /// <summary>
    /// 提供一组用于实现字符换对象的扩展方法。
    /// </summary>
    public static class XStringExtensions
    {
        /// <summary>
        /// 将当前字符串转换为指定编码的的字节组。
        /// </summary>
        /// <param name="value">当前字符串。</param>
        /// <param name="encoding">编码。为 null 值表示 UTF8 的编码。</param>
        /// <returns>字节组。</returns>
        public static byte[] ToBytes(this string value, Encoding? encoding = null)
            => (encoding ?? Encoding.UTF8).GetBytes(value);

        /// <summary>
        /// 将当前字节组转换为指定编码的的字符串。
        /// </summary>
        /// <param name="bytes">当前字节组。</param>
        /// <param name="encoding">编码。为 null 值表示 UTF8 的编码。</param>
        /// <returns>字符串。</returns>
        public static string GetString(this byte[] bytes, Encoding? encoding = null)
            => (encoding ?? Encoding.UTF8).GetString(bytes);

        /// <summary>
        /// 将当前字符串转换为智能小写模式。
        /// </summary>
        /// <param name="s">当前字符串。</param>
        /// <returns>新的字符串。</returns>
        public static string ToCamelCase(this string s)
        {
            if (string.IsNullOrWhiteSpace(s) || !char.IsUpper(s[0])) return s;

            var chars = s.ToCharArray();

            for (int i = 0; i < chars.Length; i++)
            {
                var hasNext = i + 1 < chars.Length;
                if (i > 0 && hasNext && !char.IsUpper(chars[i + 1]))
                    break;
                chars[i] = char.ToLower(chars[i]);
            }

            return new string(chars);
        }
        /// <summary>
        /// 将当前字符串转换为智能大写模式。
        /// </summary>
        /// <param name="s">当前字符串。</param>
        /// <returns>新的字符串。</returns>
        public static string ToCamelCaseUpper(this string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return s;
            Globalization.TextInfo info = Globalization.CultureInfo.CurrentCulture.TextInfo;
            s = info.ToTitleCase(s.ToLower()).Replace("_", string.Empty);
            return s;
        }
        /// <summary>
        /// 忽略被比较字符串的大小写，确定两个指定的 <see cref="string"/> 实例是否具有同一值。
        /// </summary>
        /// <param name="a"><see cref="string"/>第一个 <see cref="string"/> 的实例。</param>
        /// <param name="b"><see cref="string"/>第二个 <see cref="string"/> 的实例。</param>
        /// <returns>如果 <paramref name="a"/> 参数的值等于 <paramref name="b"/> 参数的值，则为 true；否则为 false。</returns>
#pragma warning disable IDE1006 // 命名样式
        public static bool iEquals(this string a, string b)
#pragma warning restore IDE1006 // 命名样式
            => string.Equals(a, b, StringComparison.CurrentCultureIgnoreCase);

        /// <summary>
        /// 忽略被比较字符串的大小写，确定在使用指定的比较选项进行比较时此字符串实例的开头是否与指定的字符串匹配。
        /// </summary>
        /// <param name="a"><see cref="string"/>第一个 <see cref="string"/> 的实例。</param>
        /// <param name="b"><see cref="string"/>第二个 <see cref="string"/> 的实例。</param>
        /// <returns>如果 <paramref name="b"/> 参数与此字符串的开头匹配，则为 true；否则为 false。 </returns>
#pragma warning disable IDE1006 // 命名样式
        public static bool iStartsWith(this string a, string b)
#pragma warning restore IDE1006 // 命名样式
        {
            if (a is null && b is null) return true;
            if (a is null || b is null) return false;
            return a.StartsWith(b, StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// 忽略被比较字符串的大小写，确定使用指定的比较选项进行比较时此字符串实例的结尾是否与指定的字符串匹配。
        /// </summary>
        /// <param name="a"><see cref="string"/>第一个 <see cref="string"/> 的实例。</param>
        /// <param name="b"><see cref="string"/>第二个 <see cref="string"/> 的实例。</param>
        /// <returns>如果 <paramref name="b"/> 参数与此字符串的结尾匹配，则为 true；否则为 false。 </returns>
#pragma warning disable IDE1006 // 命名样式
        public static bool iEndsWith(this string a, string b)
#pragma warning restore IDE1006 // 命名样式
        {
            if (a is null && b is null) return true;
            if (a is null || b is null) return false;
            return a.EndsWith(b, StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// 忽略被比较字符串的大小写，返回一个值，该值指示指定的 <see cref="string"/> 对象是否出现在此字符串中。
        /// </summary>
        /// <param name="a"><see cref="string"/>第一个 <see cref="string"/> 的实例。</param>
        /// <param name="b"><see cref="string"/>第二个 <see cref="string"/> 的实例。</param>
        /// <returns>如果 <paramref name="b"/> 参数出现在此字符串中，或者 <paramref name="b"/> 为空字符串 ("")，则为 true；否则为 false。 </returns>
#pragma warning disable IDE1006 // 命名样式
        public static bool iContains(this string a, string b)
#pragma warning restore IDE1006 // 命名样式
        {
            if (a is null && b is null) return true;
            if (a is null || b is null) return false;
            return a.IndexOf(b, StringComparison.CurrentCultureIgnoreCase) > -1;
        }

        /// <summary>
        /// 在当前字符串的前后增加“%”符号。
        /// </summary>
        /// <param name="input">当前字符串。</param>
        /// <returns>新的字符串。</returns>
        public static string ToLiking(this string input)
            => string.Concat("%", input, "%");

        /// <summary>
        /// 返回表示当前 <see cref="string"/>，如果 <paramref name="input"/> 是一个 null 值，将返回 <see cref="string.Empty"/>。
        /// </summary>
        /// <param name="input">一个字符串。</param>
        /// <returns> <paramref name="input"/> 的 <see cref="string"/> 或 <see cref="string.Empty"/>。</returns>
        public static string ToStringOrEmpty(this string input)
            => input ?? string.Empty;

        /// <summary>
        /// 判定当前字符串是否是一个空的字符串。
        /// </summary>
        /// <param name="input">当前字符串。</param>
        /// <returns>如果字符串为 null、空 或 空白，将返回 true，否则返回 false。</returns>
        public static bool IsNull(this string? input)
            => string.IsNullOrWhiteSpace(input);

        /// <summary>
        /// 判定当前字符串是否不是一个空的字符串。
        /// </summary>
        /// <param name="input">当前字符串。</param>
        /// <returns>如果字符串为 null、空 或 空白，将返回 true，否则返回 false。</returns>
        public static bool IsNotNull(this string? input)
            => !string.IsNullOrWhiteSpace(input);

        /// <summary>
        /// 指定整串字符串的最大长度，剪裁字符串数据，超出部分将会在结尾添加“...”。
        /// </summary>
        /// <param name="input">一个字符串。</param>
        /// <param name="maxLength">字符串的最大长度（含）。</param>
        /// <param name="ellipsis">指定省略号的字符串，默认为“...”。</param>
        /// <returns>新的字符串 -或- 原字符串，该字符串的最大长度不超过 <paramref name="maxLength"/>。</returns>
        public static string? CutString(this string? input, int maxLength, string ellipsis = "...")
        {
            if (maxLength <= 0) throw new ArgumentOutOfRangeException(nameof(maxLength));
            if (string.IsNullOrWhiteSpace(ellipsis)) throw new ArgumentNullException(nameof(ellipsis));
            if (input is null || input.Length <= maxLength) return input;
            return input.Substring(0, maxLength) + ellipsis;
        }

        /// <summary>
        /// 截取字符串开头的内容。
        /// </summary>
        /// <param name="input">一个字符串。</param>
        /// <param name="length">获取的字符串长度。</param>
        /// <returns>新的字符串。</returns>
        public static string Starts(this string input, int length)
        {
            if (string.IsNullOrEmpty(input)) return input;
            return length >= input.Length ? input : input.Substring(0, length);
        }

        /// <summary>
        /// 截取字符串结尾的内容。
        /// </summary>
        /// <param name="input">一个字符串。</param>
        /// <param name="length">获取的字符串长度。</param>
        /// <returns>新的字符串。</returns>
        public static string Ends(this string input, int length)
        {
            if (string.IsNullOrEmpty(input)) return input;
            return length >= input.Length ? input : input.Substring(input.Length - length);
        }

        /// <summary>
        /// 删除当前字符串的开头的字符串。
        /// </summary>
        /// <param name="val">目标字符串。</param>
        /// <param name="count">要删除的字长度。</param>
        /// <returns>删除后的字符串。</returns>
        public static string RemoveStarts(this string val, int count = 1)
        {
            if (string.IsNullOrEmpty(val) || val.Length <= count) return val;
            return val.Remove(0, count);
        }

        /// <summary>
        /// 删除当前字符串的结尾的字符串。
        /// </summary>
        /// <param name="val">目标字符串。</param>
        /// <param name="count">要删除的字长度。</param>
        /// <returns>删除后的字符串。</returns>
        public static string RemoveEnds(this string val, int count = 1)
        {
            if (string.IsNullOrEmpty(val) || val.Length <= count) return val;
            return val.Remove(val.Length - count);
        }

        /// <summary>
        /// 获取字符串的字节数。
        /// </summary>
        /// <param name="val">目标字符串。</param>
        /// <returns>字符串的字节数。</returns>
        public static int GetDataLength(this string val)
        {
            if (string.IsNullOrEmpty(val)) return 0;

            int length = 0;
            foreach (var c in val)
            {
                length += (c >= 0 && c <= 128) ? 1 : 2;
            }
            return length;
        }

        /// <summary>
        /// 判断字符串是否只包含数字的主键(默认至少5位)
        /// </summary>
        /// <param name="source">目标字符串。</param>
        /// <param name="min">最少位数,默认5位</param>
        /// <returns></returns>
        public static bool IsIdNumber(this string source, int min = 5)
        {

            Match match = Regex.Match(source, $@"^\d{{{min},}}$");

            return match.Success;

        }

        /// <summary> 
        ///  将查询字符串解析转换为名值集合.
        /// </summary> 
        /// <param name="queryString"></param> 
        /// <returns></returns> 
        public static NameValueCollection GetQueryString(string url)
        {
            Uri uri = new Uri(url);
            string queryString = uri.Query;
            return GetQueryString(queryString, null, true);
        }


        ///   <summary> 
        ///  将查询字符串解析转换为名值集合.
        ///   </summary> 
        ///   <param name="queryString"></param> 
        ///   <param name="encoding"></param> 
        ///   <param name="isEncoded"></param> 
        ///   <returns></returns> 
        public static NameValueCollection GetQueryString(string queryString, Encoding? encoding, bool isEncoded)
        {
            queryString = queryString.Replace("?", "");
            NameValueCollection result = new NameValueCollection(StringComparer.OrdinalIgnoreCase);
            if (!string.IsNullOrEmpty(queryString))
            {
                int count = queryString.Length;
                for (int i = 0; i < count; i++)
                {
                    int startIndex = i;
                    int index = -1;
                    while (i < count)
                    {
                        char item = queryString[i];
                        if (item == '=')
                        {
                            if (index < 0)
                            {
                                index = i;
                            }
                        }
                        else if (item == '&')
                        {
                            break;
                        }
                        i++;
                    }

                    string? value = null;
                    string? key;
                    if (index >= 0)
                    {
                        key = queryString.Substring(startIndex, index - startIndex);
                        value = queryString.Substring(index + 1, (i - index) - 1);
                    }
                    else
                    {
                        key = queryString.Substring(startIndex, i - startIndex);
                    }
                    if (isEncoded)
                    {
                        result[MyUrlDeCode(key, encoding)] = MyUrlDeCode(value, encoding);
                    }
                    else
                    {
                        result[key] = value;
                    }
                    if ((i == (count - 1)) && (queryString[i] == '&'))
                    {
                        result[key] = string.Empty;
                    }
                }
            }
            return result;
        }
        ///   <summary> 
        ///  解码URL.
        ///   </summary> 
        ///   <param name="encoding"> null为自动选择编码 </param> 
        ///   <param name="str"></param> 
        ///   <returns></returns> 
        public static string? MyUrlDeCode(string? str, Encoding? encoding)
        {
            if (encoding == null)
            {
                Encoding utf8 = Encoding.UTF8;
                // 首先用utf-8进行解码                      
                string? code = HttpUtility.UrlDecode(str?.ToUpper(), utf8);
                // 将已经解码的字符再次进行编码. 
                string? encode = HttpUtility.UrlEncode(code, utf8)?.ToUpper();
                if (str == encode)
                    encoding = Encoding.UTF8;
                else
                    encoding = Encoding.GetEncoding("gb2312");
            }
            return HttpUtility.UrlDecode(str, encoding);
        }

        /// <summary>
        /// 将当前字符串转换为指定编码的的字节组。
        /// </summary>
        /// <param name="text">当前字符串。</param>
        /// <param name="encoding">编码。为 null 值表示 UTF8 的编码。</param>
        /// <returns>字节组。</returns>
        public static byte[] GetBytes(this string text, Encoding? encoding = null)
        {
            return (encoding ?? Encoding.UTF8)!.GetBytes(text);
        }

        public static string ToShortId(this Guid guid)
        {
            var str = Convert.ToBase64String(guid.ToByteArray());
            return str.Replace("/", "").Replace("+", "").Replace("=", "");
        }

        #region MD5加密
        /// <summary>
        /// MD5 加密(小写)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToMd5(this string value)
        {
            byte[] bytes;
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(value));
            }
            var result = new StringBuilder();
            foreach (byte t in bytes)
            {
                result.Append(t.ToString("x2"));
            }
            return result.ToString();
        }
        #endregion


        public static bool IsIn(this string str, params string[] data)
        {
            foreach (var item in data)
            {
                if (str == item)
                {
                    return true;
                }
            }
            return false;
        }

        public static string RemovePostFix(this string str, params string[] postFixes)
        {
            if (str == null)
            {
                return null;
            }

            if (str == string.Empty)
            {
                return string.Empty;
            }

            if (postFixes.IsNullOrEmpty())
            {
                return str;
            }

            foreach (var postFix in postFixes)
            {
                if (str.EndsWith(postFix))
                {
                    return str.Left(str.Length - postFix.Length);
                }
            }

            return str;
        }

        public static string RemovePreFix(this string str, params string[] preFixes)
        {
            if (str == null)
            {
                return null;
            }

            if (str == string.Empty)
            {
                return string.Empty;
            }

            if (preFixes.IsNullOrEmpty())
            {
                return str;
            }

            foreach (var preFix in preFixes)
            {
                if (str.StartsWith(preFix))
                {
                    return str.Right(str.Length - preFix.Length);
                }
            }

            return str;
        }


        public static string Left(this string str, int len)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }

            if (str.Length < len)
            {
                throw new ArgumentException("len argument can not be bigger than given string's length!");
            }

            return str.Substring(0, len);
        }


        public static string Right(this string str, int len)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }

            if (str.Length < len)
            {
                throw new ArgumentException("len argument can not be bigger than given string's length!");
            }

            return str.Substring(str.Length - len, len);
        }

        public static string GetCamelCaseFirstWord(this string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (str.Length == 1)
            {
                return str;
            }

            var res = Regex.Split(str, @"(?=\p{Lu}\p{Ll})|(?<=\p{Ll})(?=\p{Lu})");

            if (res.Length < 1)
            {
                return str;
            }
            else
            {
                return res[0];
            }
        }

        public static string GetPascalCaseFirstWord(this string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (str.Length == 1)
            {
                return str;
            }

            var res = Regex.Split(str, @"(?=\p{Lu}\p{Ll})|(?<=\p{Ll})(?=\p{Lu})");

            if (res.Length < 2)
            {
                return str;
            }
            else
            {
                return res[1];
            }
        }

        public static string GetPascalOrCamelCaseFirstWord(this string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (str.Length <= 1)
            {
                return str;
            }

            if (str[0] >= 65 && str[0] <= 90)
            {
                return GetPascalCaseFirstWord(str);
            }
            else
            {
                return GetCamelCaseFirstWord(str);
            }
        }

        public static string FirstCharToLower(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return s;

            string str = s.First().ToString().ToLower() + s.Substring(1);
            return str;
        }

        public static string FirstCharToUpper(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return s;

            string str = s.First().ToString().ToUpper() + s.Substring(1);
            return str;
        }

        public static string ToPascal(this string str)
        {
            string[] split = str.Split(new char[] { '/', ' ', '_', '.', '-' });
            string newStr = "";
            foreach (var item in split)
            {
                char[] chars = item.ToCharArray();
                chars[0] = char.ToUpper(chars[0]);
                for (int i = 1; i < chars.Length; i++)
                {
                    chars[i] = char.ToLower(chars[i]);
                }
                newStr += new string(chars);
            }
            return newStr;
        }
        public static string ToCamel(this string str)
        {
            return str.ToPascal().FirstCharToLower();
        }
    }
}
