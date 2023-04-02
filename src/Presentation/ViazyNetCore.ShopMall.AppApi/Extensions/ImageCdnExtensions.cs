using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.ShopMall.AppApi
{
    public static class ImageCdnExtensions
    {
        private const string IMG_BASEURL = "https://localhost:7277";
        public static string? ToCdnUrl(this string? imageUrl)
        {
            if (imageUrl.IsNull()) return null;
            return imageUrl!.Replace("/upload/", IMG_BASEURL + "/upload/");
        }
    }
}
