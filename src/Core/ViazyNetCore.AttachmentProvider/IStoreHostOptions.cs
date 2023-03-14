using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.AttachmentProvider
{
    /// <summary>
    /// 附件服务器配置
    /// </summary>
    public interface IStoreHostOptions
    {
        /// <summary>
        /// 服务器地址
        /// </summary>
        string ServerUrl { get; }

        /// <summary>
        /// 上传图片类型限制
        /// </summary>
        List<MediaType> MediaTypes { get; }

        /// <summary>
        /// 上传大小限制，单位B
        /// </summary>
        long FileMaxSize { get; }

        /// <summary>
        /// 附件服务器空间限制，单位MB
        /// </summary>
        long StroeMaxSize { get; }

        /// <summary>
        /// 允许上传图片格式
        /// </summary>
        List<string> ImageAllowFiles { get; }

        /// <summary>
        /// 允许上传文件格式
        /// </summary>
        List<string> FileAllowFiles { get; }
    }

    /// <summary>
    /// 存储服务器默认配置项
    /// </summary>
    public static class DefaultStoreHostOptions
    {
        /// <summary>
        /// 默认允许图片格式配置
        /// </summary>
        public static List<string> ImageAllowFiles = new List<string> { ".png", ".jpg", ".jpeg", ".gif", ".bmp" };
        /// <summary>
        /// 默认允许文件格式配置
        /// </summary>
        public static List<string> FileAllowFiles = new List<string> { ".png",
    ".jpg",
    ".jpeg",
    ".gif",
    ".bmp",
    ".flv",
    ".swf",
    ".mkv",
    ".avi",
    ".rm",
    ".rmvb",
    ".mpeg",
    ".mpg",
    ".ogg",
    ".ogv",
    ".mov",
    ".wmv",
    ".mp4",
    ".webm",
    ".mp3",
    ".wav",
    ".mid",
    ".rar",
    ".zip",
    ".tar",
    ".gz",
    ".7z",
    ".bz2",
    ".cab",
    ".iso",
    ".doc",
    ".docx",
    ".xls",
    ".xlsx",
    ".ppt",
    ".pptx",
    ".pdf",
    ".txt",
    ".md",
    ".xml" };
    }
}
