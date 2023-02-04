using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ViazyNetCore.Filter
{
    internal static class XCommonExtensions
    {
        /// <summary>
        /// 获取HTTP请求方法。
        /// </summary>
        /// <param name="info">当前方法实例。</param>
        /// <returns>返回HTTP请求方法。</returns>
        internal static string GetHttpMethod(this MemberInfo info)
        {
            if (info.IsDefined(typeof(HttpPostAttribute)))
                return HttpMethod.Post.ToString();
            if (info.IsDefined(typeof(HttpPutAttribute)))
                return HttpMethod.Put.ToString();
            if (info.IsDefined(typeof(HttpDeleteAttribute)))
                return HttpMethod.Delete.ToString();
            if (info.IsDefined(typeof(HttpHeadAttribute)))
                return HttpMethod.Head.ToString();
            if (info.IsDefined(typeof(HttpPatchAttribute)))
                return HttpMethod.Patch.ToString();
            if (info.IsDefined(typeof(HttpOptionsAttribute)))
                return HttpMethod.Options.ToString();
            return HttpMethod.Get.ToString();
        }
    }
}
