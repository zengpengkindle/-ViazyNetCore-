using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public partial class GA
    {
        /// <summary>
        /// 合并 URL 地址。
        /// </summary>
        /// <param name="baseUrl">URL 基地址。</param>
        /// <param name="relativeUrls">相对地址集合。</param>
        /// <returns>新的地址。</returns>
        public static string UrlCombine(string baseUrl, params string[] relativeUrls)
        {
            var uri = new Uri(baseUrl);
            if (relativeUrls.Length == 1 && Uri.TryCreate(relativeUrls[0], UriKind.Absolute, out var result))
            {
                return result.ToString();
            }

            return new Uri(relativeUrls.Aggregate(uri.AbsoluteUri, (current, path) => current.TrimEnd(System.IO.Path.AltDirectorySeparatorChar)
            + System.IO.Path.AltDirectorySeparatorChar + path.TrimStart(System.IO.Path.AltDirectorySeparatorChar))).ToString();
        }
    }
}
