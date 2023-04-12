using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace System
{
    public static class ValidateExtension
    {
        private static Regex RegNumber = new Regex("^[0-9]+$");
        private static Regex RegNumberSign = new Regex("^[+-]?[0-9]+$");
        private static Regex RegDecimal = new Regex("^[0-9]+[.]?[0-9]+$");
        private static Regex RegDecimalSign = new Regex("^[+-]?[0-9]+[.]?[0-9]+$"); //等价于^[+-]?\d+[.]?\d+$
        private static Regex RegEmail = new Regex(@"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$");//w 英文字母或数字的字符串，和 [a-zA-Z0-9] 语法一样 
        private static Regex RegCHZN = new Regex("[\u4e00-\u9fa5]");
        
        #region 用户名密码格式

        /// <summary>
        /// 返回字符串真实长度, 1个汉字长度为2
        /// </summary>
        /// <returns>字符长度</returns>
        public static int GetStringLength(this string stringValue)
        {
            return Encoding.Default.GetBytes(stringValue).Length;
        }

        /// <summary>
        /// 检测用户名格式是否有效
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static bool IsValidUserName(this string userName)
        {
            int userNameLength = GetStringLength(userName);
            if (userNameLength >= 4 && userNameLength <= 20 && Regex.IsMatch(userName, @"^([\u4e00-\u9fa5A-Za-z_0-9]{0,})$"))
            {   // 判断用户名的长度（4-20个字符）及内容（只能是汉字、字母、下划线、数字）是否合法
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 密码有效性
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool IsValidPassword(this string password)
        {
            return Regex.IsMatch(password, @"^[A-Za-z_0-9]{6,16}$");
        }
        #endregion

        #region 数字字符串检查

        /// <summary>
        /// int有效性
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        static public bool IsValidInt(this string val)
        {
            return Regex.IsMatch(val, @"^[1-9]\d*\.?[0]*$");
        }

        /// <summary>
        /// 是否数字字符串
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public static bool IsNumber(this string inputData)
        {
            Match m = RegNumber.Match(inputData);
            return m.Success;
        }

        /// <summary>
        /// 是否数字字符串 可带正负号
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public static bool IsNumberSign(this string inputData)
        {
            Match m = RegNumberSign.Match(inputData);
            return m.Success;
        }

        /// <summary>
        /// 是否是浮点数
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public static bool IsDecimal(this string inputData)
        {
            Match m = RegDecimal.Match(inputData);
            return m.Success;
        }

        /// <summary>
        /// 是否是浮点数 可带正负号
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public static bool IsDecimalSign(this string inputData)
        {
            Match m = RegDecimalSign.Match(inputData);
            return m.Success;
        }

        #endregion

        #region 中文检测

        /// <summary>
        /// 检测是否有中文字符
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static bool IsHasCHZN(this string inputData)
        {
            Match m = RegCHZN.Match(inputData);
            return m.Success;
        }

        /// <summary> 
        /// 检测含有中文字符串的实际长度 
        /// </summary> 
        /// <param name="str">字符串</param> 
        public static int GetCHZNLength(this string inputData, Encoding? encoding = null)
        {
            encoding ??= Encoding.UTF8;
            byte[] bytes = encoding.GetBytes(inputData);

            int length = 0; // l 为字符串之实际长度 
            for (int i = 0; i <= bytes.Length - 1; i++)
            {
                if (bytes[i] == 63) //判断是否为汉字或全脚符号 
                {
                    length++;
                }
                length++;
            }
            return length;

        }

        #endregion

        #region 常用格式

        /// <summary>
        /// 验证身份证是否合法  15 和  18位两种
        /// </summary>
        /// <param name="idCard">要验证的身份证</param>
        public static bool IsIdCard(this string idCard)
        {
            if (string.IsNullOrEmpty(idCard))
            {
                return false;
            }

            if (idCard.Length == 15)
            {
                return Regex.IsMatch(idCard, @"^[1-9]\d{7}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}$");
            }
            else if (idCard.Length == 18)
            {
                return Regex.IsMatch(idCard, @"^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])((\d{4})|\d{3}[A-Z])$", RegexOptions.IgnoreCase);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 是否是邮件地址
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public static bool IsEmail(this string inputData)
        {
            Match m = RegEmail.Match(inputData);
            return m.Success;
        }

        /// <summary>
        /// 邮编有效性
        /// </summary>
        /// <param name="zip"></param>
        /// <returns></returns>
        public static bool IsValidZip(this string zip)
        {
            Regex rx = new Regex(@"^\d{6}$", RegexOptions.None);
            Match m = rx.Match(zip);
            return m.Success;
        }

        /// <summary>
        /// 固定电话有效性
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static bool IsValidPhone(this string phone)
        {
            Regex rx = new Regex(@"^(\(\d{3,4}\)|\d{3,4}-)?\d{7,8}$", RegexOptions.None);
            Match m = rx.Match(phone);
            return m.Success;
        }

        /// <summary>
        /// 手机有效性
        /// </summary>
        /// <param name="strMobile"></param>
        /// <returns></returns>
        public static bool IsValidMobile(this string mobile)
        {
            Regex rx = new Regex(@"^(13|15|17|18|19)\d{9}$", RegexOptions.None);
            Match m = rx.Match(mobile);
            return m.Success;
        }

        /// <summary>
        /// 电话有效性（固话和手机 ）
        /// </summary>
        /// <param name="strVla"></param>
        /// <returns></returns>
        public static bool IsValidPhoneAndMobile(this string number)
        {
            Regex rx = new Regex(@"^(\(\d{3,4}\)|\d{3,4}-)?\d{7,8}$|^(13|15)\d{9}$", RegexOptions.None);
            Match m = rx.Match(number);
            return m.Success;
        }

        /// <summary>
        /// Url有效性
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        static public bool IsValidURL(this string url)
        {
            return Regex.IsMatch(url, @"^(http|https|ftp)\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&%\$#\=~])*[^\.\,\)\(\s]$");
        }

        /// <summary>
        /// IP有效性
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsValidIP(this string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        /// <summary>
        /// domain 有效性
        /// </summary>
        /// <param name="host">域名</param>
        /// <returns></returns>
        public static bool IsValidDomain(this string host)
        {
            Regex r = new Regex(@"^\d+$");
            if (host.IndexOf(".") == -1)
            {
                return false;
            }
            return r.IsMatch(host.Replace(".", string.Empty)) ? false : true;
        }

        /// <summary>
        /// 判断是否为base64字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsBase64String(this string str)
        {
            return Regex.IsMatch(str, @"[A-Za-z0-9\+\/\=]");
        }

        /// <summary>
        /// 验证字符串是否是GUID
        /// </summary>
        /// <param name="guid">字符串</param>
        /// <returns></returns>
        public static bool IsGuid(this string guid)
        {
            if (string.IsNullOrEmpty(guid))
                return false;

            return Regex.IsMatch(guid, "[A-F0-9]{8}(-[A-F0-9]{4}){3}-[A-F0-9]{12}|[A-F0-9]{32}", RegexOptions.IgnoreCase);
        }

        #endregion

        #region 日期检查

        /// <summary>
        /// 判断输入的字符是否为日期
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static bool IsDate(this string strValue)
        {
            return Regex.IsMatch(strValue, @"^((\d{2}(([02468][048])|([13579][26]))[\-\/\s]?((((0?[13578])|(1[02]))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[469])|(11))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(30)))|(0?2[\-\/\s]?((0?[1-9])|([1-2][0-9])))))|(\d{2}(([02468][1235679])|([13579][01345789]))[\-\/\s]?((((0?[13578])|(1[02]))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[469])|(11))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(30)))|(0?2[\-\/\s]?((0?[1-9])|(1[0-9])|(2[0-8]))))))");
        }

        /// <summary>
        /// 判断输入的字符是否为日期,如2004-07-12 14:25|||1900-01-01 00:00|||9999-12-31 23:59
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static bool IsDateHourMinute(this string strValue)
        {
            return Regex.IsMatch(strValue, @"^(19[0-9]{2}|[2-9][0-9]{3})-((0(1|3|5|7|8)|10|12)-(0[1-9]|1[0-9]|2[0-9]|3[0-1])|(0(4|6|9)|11)-(0[1-9]|1[0-9]|2[0-9]|30)|(02)-(0[1-9]|1[0-9]|2[0-9]))\x20(0[0-9]|1[0-9]|2[0-3])(:[0-5][0-9]){1}$");
        }

        #endregion

        #region 其他

        /// <summary>
        /// 检查字符串最大长度，返回指定长度的串
        /// </summary>
        /// <param name="sqlInput">输入字符串</param>
        /// <param name="maxLength">最大长度</param>
        /// <returns></returns>            
        public static string CheckMathLength(this string inputData, int maxLength)
        {
            if (inputData != null && inputData != string.Empty)
            {
                inputData = inputData.Trim();
                if (inputData.Length > maxLength)//按最大长度截取字符串
                {
                    inputData = inputData.Substring(0, maxLength);
                }
            }
            return inputData;
        }

        /// <summary>
        /// 转换成 HTML code
        /// </summary>
        /// <param name="str">string</param>
        /// <returns>string</returns>
        public static string Encode(this string str)
        {
            str = str.Replace("&", "&amp;");
            str = str.Replace("'", "''");
            str = str.Replace("\"", "&quot;");
            str = str.Replace(" ", "&nbsp;");
            str = str.Replace("<", "&lt;");
            str = str.Replace(">", "&gt;");
            str = str.Replace("\n", "<br>");
            return str;
        }
        /// <summary>
        ///解析html成 普通文本
        /// </summary>
        /// <param name="str">string</param>
        /// <returns>string</returns>
        public static string Decode(this string str)
        {
            str = str.Replace("<br>", "\n");
            str = str.Replace("&gt;", ">");
            str = str.Replace("&lt;", "<");
            str = str.Replace("&nbsp;", " ");
            str = str.Replace("&quot;", "\"");
            return str;
        }

        #endregion

        #region 字符串格式验证

        /// <summary>
        /// 如果无值则取默认值
        /// </summary>
        /// <param name="source"></param>
        /// <param name="df"></param>
        /// <returns></returns>
        public static string DefaultValue(this string source, string df)
        {
            if (source.IsNull())
            {
                return df;
            }
            return source;
        }

        /// <summary>
        /// 判断字符串是否只包含数字或字母
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNumOrAlp(this string source)
        {

            Match match = Regex.Match(source, @"^[A-Za-z0-9]+$");

            return match.Success;

        }
        /// <summary>
        /// 判断字符串是否为整型
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsInteger(this string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return false;
            }
            int i;
            return Int32.TryParse(source, out i);
        }

        /// <summary>
        /// 判断是否IP
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsIP(this string source)
        {
            return Regex.IsMatch(source, @"^(((25[0-5]|2[0-4][0-9]|19[0-1]|19[3-9]|18[0-9]|17[0-1]|17[3-9]|1[0-6][0-9]|1[1-9]|[2-9][0-9]|[0-9])\.(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9][0-9]|[0-9]))|(192\.(25[0-5]|2[0-4][0-9]|16[0-7]|169|1[0-5][0-9]|1[7-9][0-9]|[1-9][0-9]|[0-9]))|(172\.(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|1[0-5]|3[2-9]|[4-9][0-9]|[0-9])))\.(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9][0-9]|[0-9])\.(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9][0-9]|[0-9])$");
        }

        /// <summary>
        /// 检查字符串是否为A-Z、0-9及下划线以内的字符
        /// </summary>
        /// <param name="source">被检查的字符串</param>
        /// <returns>是否有特殊字符</returns>
        public static bool IsLetterOrNumber(this string source)
        {
            bool b = System.Text.RegularExpressions.Regex.IsMatch(source, @"\w");
            return b;
        }

        /// <summary>
        /// 验输入字符串是否含有“/\:.?*|$]”特殊字符
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsSpecialChar(this string source)
        {
            Regex r = new Regex(@"[/\<>:.?*|$]");
            return r.IsMatch(source);
        }

        /// <summary>
        /// 是否全为中文/日文/韩文字符
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <returns></returns>
        public static bool IsChineseChar(this string source)
        {
            //中文/日文/韩文: [\u4E00-\u9FA5]
            //英文:[a-zA-Z]
            return Regex.IsMatch(source, @"^[\u4E00-\u9FA5]+$");
        }

        /// <summary>
        /// 是否包含双字节字符(允许有单字节字符)
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsDoubleChar(this string source)
        {
            return Regex.IsMatch(source, @"[^\x00-\xff]");
        }


        /// <summary>
        /// 是否为时间型字符串
        /// </summary>
        /// <param name="source">时间字符串(15:00:00)</param>
        /// <returns></returns>
        public static bool IsTime(this string source)
        {
            return Regex.IsMatch(source, @"^((20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d)$");
        }

        /// <summary>
        /// 是否为日期+时间型字符串
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsDateTime(this string source)
        {
            return Regex.IsMatch(source, @"^(((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-)) (20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d)$");
        }

        /// <summary>
        /// 是否是手机号码
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsMobile(this string source)
        {
            return Regex.IsMatch(source, @"^((13[0-9])|(14[0-9])|(15[0-9])|(16[0-9])|(17[0-9])|(18[0-9])|(19[0-9]))\d{8}$");
        }

        /// <summary>
        /// 手机号脱敏处理
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string MobileDesensitization(this string source)
        {
            if (source.IsNull())
            {
                return "";
            }

            return Regex.Replace(source, "(\\d{3})\\d{4}(\\d{4})", "$1****$2");
        }

        /// <summary>
        /// 脱敏
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        public static string ToSensitiveName(this string fullName)
        {
            if (string.IsNullOrEmpty(fullName)) return string.Empty;

            string familyName = fullName.Substring(0, 1);
            string end = fullName.Substring(fullName.Length - 1, 1);
            string name = string.Empty;
            //长度为2
            if (fullName.Length <= 2) name = familyName + "*";
            //长度大于2
            else if (fullName.Length >= 3)
            {
                var hid = (fullName.Length - ((int)Math.Round(fullName.Length * 1m / 3))) / 2;
                name = fullName.Substring(0, hid) + "".PadLeft(fullName.Length - hid - hid, '*') + fullName.Substring(fullName.Length - hid);
            }
            return name;
        }

        /// <summary>
        /// 是否是Url
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsUrl(this string source)
        {
            if (source.IsNull())
            {
                return false;
            }


            Regex rg = new Regex(@"^(https?|s?ftp):\/\/(((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(#((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?$", RegexOptions.IgnoreCase);

            return rg.IsMatch(source);
        }
        #endregion
    }
}
