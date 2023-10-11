using Newtonsoft.Json.Linq;
using System;
using System.Text.RegularExpressions;

namespace ViazyNetCore.Formatter.Response.Extensions
{
    internal static class StringExtension
    {
        public static (bool IsEncoded, string ParsedText) VerifyBodyContent(this string text)
        {
            try
            {
                var obj = JToken.Parse(text);
                return (true, obj.ToString());
            }
            catch (Exception)
            {
                return (false, text);
            }
        }

        public static bool IsHtml(this string text)
        {
            Regex tagRegex = new Regex(@"<\s*([^ >]+)[^>]*>.*?<\s*/\s*\1\s*>");

            return tagRegex.IsMatch(text);
        }


        public static string ToCamelCase(this string str)
        {
            if (!string.IsNullOrEmpty(str) && str.Length > 1)
            {
                return char.ToLowerInvariant(str[0]) + str[1..];
            }
            return str;
        }
        
    }
}
