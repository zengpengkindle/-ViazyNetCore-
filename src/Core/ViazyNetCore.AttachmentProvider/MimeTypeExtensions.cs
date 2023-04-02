using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.AttachmentProvider
{
    /// <summary>
    /// mime配置
    /// </summary>
    public class MimeTypeExtensions
    {
        static MimeTypeExtensions()
        {
            MimeTypes = new Dictionary<string, string> {
                {"htm","text/html"},
                {"html","text/html"},
                {"xml","text/xml"},
                {"css","text/css"},
                {"js","text/javascript"},
                {"doc","application/msword"},
                {"wps","application/octet-stream"},
                {"xls","application/vnd.ms-excel"},
                {"ppt","application/vnd.ms-powerpoint"},
                {"docx","application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
                {"xlsx","application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {"pptx","application/vnd.openxmlformats-officedocument.presentationml.presentation"},
                {"pps","application/vnd.ms-powerpoint"},
                {"ppsx","application/vnd.openxmlformats-officedocument.presentationml.slideshow"},
                {"rtf","application/msword"},
                {"pdf","application/pdf"},
                {"txt","text/plain"},
                {"zip","application/x-compressed"},
                {"rar","application/x-rar-compressed"},
                {"gif","image/gif"},
                {"bmp","image/bmp"},
                {"jpg","image/jpeg"},
                {"jpeg","image/jpeg"},
                {"webp","image/webp"},
                {"png","image/png"},
                {"swf","application/x-shockwave-flash"},
                {"mp3","audio/mpeg"},
                {"wav","audio/wav"},
                {"rm","audio/x-pn-realaudio"},
                {"flv","video/x-flv"},
                {"rmvb","video/vnd.rn-realvideo"},
                {"mp4","video/mp4"},
                {"mpg4","video/mp4"},
                {"3gp","video/3gpp"},
                {"mpeg","video/mpeg"},
                {"mpg","video/mpeg"},
                {"mpa","video/mpeg"},
                {"wmv","video/x-ms-wmv"},
                {"mov","video/quicktime"},
                {"avi","video/x-msvideo"},
                {"asf","video/x-ms-asf"},
                {"asr","video/x-ms-asf"},
                {"asx","video/x-ms-asf"},
                {"woff","application/x-font-woff"},
                { "woff2","application/x-font-woff"}
            };
        }
        private readonly static Dictionary<string, string> MimeTypes;

        /// <summary>
        /// 通过filename获取mime
        /// </summary>
        public static string GetMimeType(string fileName)
        {
            int index = fileName.LastIndexOf('.');
            if (index > 0 && index > fileName.LastIndexOf('\\'))
            {
                string extension = fileName.Substring(index + 1).ToLower(System.Globalization.CultureInfo.InvariantCulture);
                if (MimeTypes != null && MimeTypes.ContainsKey(extension))
                    return MimeTypes[extension];
            }
            return "application/octet-stream";
        }

        /// <summary>
        /// 通过mimeType获取文件扩展名
        /// </summary>
        public static string GetExtension(string mimeType)
        {
            if (MimeTypes != null)
            {
                mimeType = mimeType.ToLower();

                foreach (string extension in MimeTypes.Keys)
                {
                    if (MimeTypes[extension].ToLower() == mimeType)
                        return extension;
                }
            }

            return "unknown";
        }

        private static IList<string> imageExtensions = null;

        /// <summary>
        /// 获取所有是图片的后缀名集合
        /// </summary>
        public static IList<string> GetImageExtensions()
        {
            if (imageExtensions is null)
            {
                imageExtensions = MimeTypes.Where(n => n.Value.IndexOf("image") > -1).Select(n => n.Key).ToList();
            }
            return imageExtensions;
        }

        private static IList<string> extensions = null;

        /// <summary>
        /// 扩展名集合
        /// </summary>
        public static IList<string> Extensions
        {
            get
            {
                if (extensions is null)
                {
                    extensions = MimeTypes.Select(n => n.Key).ToList();
                }
                return extensions;
            }
        }
    }
}
