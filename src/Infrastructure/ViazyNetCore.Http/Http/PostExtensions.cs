using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Http
{
    public static class PostExtensions
    {
        public static IEasyRequest WithPostUrlEncoded(this IEasyRequest request, object data)
        {
            request.Content = new CapturedUrlEncodedContent(request.Settings.UrlEncodedSerializer.Serialize(data));
            return request;
        }

        public static IEasyRequest WithPostJson(this IEasyRequest request, object data)
        {
            request.Content = new CapturedJsonContent(request.Settings.JsonSerializer.Serialize(data));
            return request;
        }


        public static IEasyRequest WithPostString(this IEasyRequest request, string data)
        {
            request.Content = new CapturedStringContent(data);
            return request;
        }

        public static IEasyRequest WithPostData(this IEasyRequest request, HttpContent content)
        {
            request.Content = content;
            return request;
        }

        public static IEasyRequest WithPostOctet(this IEasyRequest request, byte[] data)
        {
            request.Content = new CapturedOctetContent(data); ;
            return request;
        }
    }
}
