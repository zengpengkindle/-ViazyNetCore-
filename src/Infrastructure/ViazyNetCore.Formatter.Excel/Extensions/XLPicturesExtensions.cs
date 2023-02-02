using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using ClosedXML.Excel.Drawings;

namespace ViazyNetCore.Formatter.Excel.Extensions
{
    public static class XLPicturesExtensions
    {
        public static IXLPicture AddImgLink(this IXLWorksheet worksheet, string imgUrl)
        {
            using var client = new WebClient();
            var bytes = client.DownloadData(imgUrl);
            using var stream = new MemoryStream(bytes);
            return worksheet.Pictures.Add(stream);
        }
    }
}
