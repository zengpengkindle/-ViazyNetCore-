using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.ShopMall.AppApi
{
    public static class ImageCdnExtensions
    {
        private const string IMG_BASEURL = "http://192.168.1.17:5156";
        public static string? ToCdnUrl(this string? imageUrl)
        {
            if (imageUrl.IsNull()) return null;
            return imageUrl!.Replace("/upload/", IMG_BASEURL + "/upload/");
        }
    }
}
