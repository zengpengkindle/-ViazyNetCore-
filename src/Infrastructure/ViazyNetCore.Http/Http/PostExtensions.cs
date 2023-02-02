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
        public static ICaesarRequest WithPostUrlEncoded(this ICaesarRequest request, object data)
        {
            request.Content = new CapturedUrlEncodedContent(request.Settings.UrlEncodedSerializer.Serialize(data));
            return request;
        }

        public static ICaesarRequest WithPostJson(this ICaesarRequest request, object data)
        {
            request.Content = new CapturedJsonContent(request.Settings.JsonSerializer.Serialize(data));
            return request;
        }


        public static ICaesarRequest WithPostString(this ICaesarRequest request, string data)
        {
            request.Content = new CapturedStringContent(data);
            return request;
        }

        public static ICaesarRequest WithPostData(this ICaesarRequest request, HttpContent content)
        {
            request.Content = content;
            return request;
        }

        public static ICaesarRequest WithPostOctet(this ICaesarRequest request, byte[] data)
        {
            request.Content = new CapturedOctetContent(data); ;
            return request;
        }
    }
}
