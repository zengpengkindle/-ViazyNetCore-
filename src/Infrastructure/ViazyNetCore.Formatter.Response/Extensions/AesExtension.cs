using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace ViazyNetCore.Formatter.Response.Extensions
{
    internal static class AesExtension
    {
        public static string EncryptCBC(this object body, HttpContext context)
        {
            var content = JsonConvert.SerializeObject(body);
            var path = context.Request.Path.Value ?? "";
            var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(path));
            var sec = (base64 + base64 + base64).PadRight(40, 'k');
            var key = sec.Substring(0, 16);
            var iv = sec.Substring(12, 16);
            var v = content.EncryptCBC(key, iv);
            return v;
        }
    }
}
