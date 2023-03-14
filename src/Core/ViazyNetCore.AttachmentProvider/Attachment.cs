using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace ViazyNetCore.AttachmentProvider
{
    /// <summary>
    /// 附件实体
    /// </summary>
    public class Attachment
    {
        private readonly IPathFormatter _pathFormatter;
        #region Properties

        /// <summary>
        ///附件关联Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///拥有者Id
        /// </summary>
        public long OwnerId { get; set; }

        /// <summary>
        ///租户类型Id
        /// </summary>
        public string TenantTypeId { get; set; }

        /// <summary>
        ///实际存储文件名称
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        ///文件显示名称
        /// </summary>
        public string FriendlyFileName { get; set; }

        /// <summary>
        ///文件大小
        /// </summary>
        public long FileLength { get; private set; }

        /// <summary>
        ///附件MIME类型
        /// </summary>
        public string ContentType { get; private set; }

        /// <summary>
        /// 附件类型（<see cref="ViazyNetCore.AttachmentProvider.MediaType"/>）
        /// </summary>
        public MediaType MediaType { get; set; }

        /// <summary>
        /// 附件本地路径
        /// </summary>
        public string FileLocalUrl { get; internal set; }


        /// <summary>
        /// 附件本地相对路径
        /// </summary>
        public string RelativeUrl { get; internal set; }

        /// <summary>
        /// 远程附件
        /// </summary>
        public List<HostFile> HostFiles { get; set; }

        /// <summary>
        /// 表示一个远程附件
        /// </summary>
        public class HostFile
        {
            /// <summary>
            /// 图片存储ID
            /// </summary>
            public string Id { get; set; }
            /// <summary>
            /// 图片地址
            /// </summary>
            public string FileUrl { get; set; }
            /// <summary>
            /// 服务器名
            /// </summary>
            public string HostName { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileUrl"></param>
        /// <param name="hostName"></param>
        public void AddHostFile(string fileUrl, string hostName)
        {
            this.HostFiles.Add(new HostFile
            {
                Id = this.Id,
                FileUrl = fileUrl,
                HostName = hostName
            });
        }

        #endregion
        /// <summary>
        /// 构造函数
        /// </summary>
        public Attachment()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="postedFile"></param>
        /// <param name="pathFormatter"></param>
        /// <param name="contentType">指定contentType，会优先采用此contentType</param>
        public Attachment(IFormFile postedFile, IPathFormatter pathFormatter, string contentType = null)
        {
            this._pathFormatter = pathFormatter;

            this.FileLength = postedFile.Length;

            if(!string.IsNullOrEmpty(contentType))
                this.ContentType = contentType;
            else if(!string.IsNullOrEmpty(postedFile.ContentType))
                this.ContentType = postedFile.ContentType;
            else
                this.ContentType = string.Empty;

            //先根据contentType 获取媒体类型，如果contentType为空，则直接根据文件获取媒体类型及ContentType
            if(!string.IsNullOrEmpty(this.ContentType))
            {
                this.ContentType = this.ContentType.Replace("pjpeg", "jpeg");
                this.MediaType = this.GetMediaType(this.ContentType);
            }
            else
            {
                this.MediaType = this.GetMediaTypeByFileName(postedFile.FileName);
                this.ContentType = MimeTypeExtensions.GetMimeType(postedFile.FileName);
            }

            if(Path.GetExtension(postedFile.FileName) == "")
            {
                switch(this.ContentType)
                {
                    case "image/jpeg":
                        this.FileName = postedFile.FileName + ".jpg";
                        break;
                    case "image/gif":
                        this.FileName = postedFile.FileName + ".gif";
                        break;
                    case "image/png":
                        this.FileName = postedFile.FileName + ".png";
                        break;
                    default:
                        break;
                }
            }
            else
            {
                this.FileName = postedFile.FileName;
            }

            this.FriendlyFileName = this.FileName.Substring(this.FileName.LastIndexOf("\\") + 1);

            this.Initization();
            //自动生成用于存储的文件名称
            //CheckImageInfo(postedFile.InputStream);
        }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="contentType"></param>
        /// <param name="friendlyFileName"></param>
        public Attachment(Stream stream, string contentType, string friendlyFileName)
        {
            this.FileLength = stream.Length;
            this.ContentType = contentType;
            this.MediaType = this.GetMediaType(this.ContentType);
            this.FriendlyFileName = friendlyFileName;

            this.Initization();
        }

        private void Initization()
        {

            this.FileName = this.GenerateFileName(Path.GetExtension(this.FriendlyFileName));

            this.HostFiles = new List<HostFile>();
        }


        /// <summary>
        /// 生成随机文件名
        /// </summary>
        /// <returns></returns>
        private string GenerateFileName(string extension)
        {
            return this._pathFormatter.Format("{time}{rand:6}") + extension;
        }

        /// <summary>
        /// 生成随机文件名
        /// </summary>
        /// <returns></returns>
        public string GenerateFileName()
        {
            return this.GenerateFileName(Path.GetExtension(this.FriendlyFileName));
        }

        /// <summary>
        /// 依据MIME获取MediaType
        /// </summary>
        /// <param name="contentType">附件MIME类型</param>
        /// <returns></returns>
        public MediaType GetMediaType(string contentType)
        {
            if(this.ContentType is null)
                return MediaType.Other;
            var extensionType = MimeTypeExtensions.GetExtension(contentType);
            switch(extensionType)
            {
                case "txt":
                case "rtf":
                case "doc":
                case "docx":
                case "wps":
                case "pptx":
                case "ppt":
                case "pps":
                case "xls":
                case "xlsx":
                case "pdf":
                    return MediaType.Document;//可在线预览
                case "jpg":
                case "png":
                case "bmp":
                case "gif":
                    return MediaType.Image;

                case "flv":
                case "rmvb":
                case "mp4":
                case "3gp":
                case "mpeg":
                case "wmv":
                case "mov":
                case "avi":
                case "asf":
                    return MediaType.Video;

                case "7z":
                case "tar":
                case "tar.gz":
                case "zip":
                case "rar":
                    return MediaType.Compressed;

                case "swf":
                    return MediaType.Flash;

                case "mp3":
                case "wav":
                case "rm":
                    return MediaType.Audio;

                default:
                    return MediaType.Other;
            }
        }


        /// <summary>
        /// 依据FileName获取MediaType
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public MediaType GetMediaTypeByFileName(string fileName)
        {
            var extensionType = Path.GetExtension(fileName).ToLower().TrimStart('.');
            switch(extensionType)
            {
                case "txt":
                case "rtf":
                case "doc":
                case "docx":
                case "wps":
                case "pptx":
                case "ppt":
                case "pps":
                case "xls":
                case "xlsx":
                case "pdf":
                    return MediaType.Document;//可在线预览
                case "jpg":
                case "png":
                case "bmp":
                case "gif":
                    return MediaType.Image;

                case "flv":
                case "rmvb":
                case "mp4":
                case "3gp":
                case "mpeg":
                case "wmv":
                case "mov":
                case "avi":
                case "asf":
                    return MediaType.Video;

                case "zip":
                case "rar":
                    return MediaType.Compressed;

                case "swf":
                    return MediaType.Flash;

                case "mp3":
                case "wav":
                case "rm":
                    return MediaType.Audio;

                default:
                    return MediaType.Other;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetMediaTypeText()
        {
            return this.MediaType switch
            {
                MediaType.Image => "image",
                MediaType.Video => "video",
                _ => "file",
            };
        }
    }

    /// <summary>
    /// 附件媒体类型
    /// </summary>
    public enum MediaType
    {
        /// <summary>
        /// 图片
        /// </summary>
        [Display(Name = "图片")]
        Image = 1,

        /// <summary>
        /// 视频
        /// </summary>
        [Display(Name = "视频")]
        Video = 2,

        /// <summary>
        /// Flash
        /// </summary>
        [Display(Name = "Flash")]
        Flash = 3,

        /// <summary>
        /// 音乐
        /// </summary>
        [Display(Name = "音乐")]
        Audio = 4,

        /// <summary>
        /// 文档
        /// </summary>
        [Display(Name = "文档")]
        Document = 5,

        /// <summary>
        /// 压缩包
        /// </summary>
        [Display(Name = "压缩包")]
        Compressed = 6,

        /// <summary>
        /// 其他类型
        /// </summary>
        [Display(Name = "其他类型")]
        Other = 99
    }
}
