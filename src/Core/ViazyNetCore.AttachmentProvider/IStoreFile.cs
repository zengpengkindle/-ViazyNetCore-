using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.AttachmentProvider
{
    /// <summary>
    /// 存储中的文件
    /// </summary>
    public interface IStoreFile
    {
        /// <summary>
        /// 文件名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 文件扩展名
        /// </summary>
        string Extension { get; }

        /// <summary>
        /// 文件大小
        /// </summary>
        long Size { get; }

        /// <summary>
        /// 相对StoragePath的路径
        /// </summary>
        string RelativePath { get; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        DateTime LastModified { get; }

        /// <summary>
        /// 获取用于读取文件的Stream
        /// </summary>
        Stream OpenReadStream();
    }

    /// <summary>
    /// 存储文件默认实现
    /// </summary>
    public class DefaultStoreFile : IStoreFile
    {
        private readonly FileInfo _fileInfo;

        /// <summary>
        /// 文件名称
        /// </summary>
        public string Name => this._fileInfo.Name;

        /// <summary>
        /// 文件扩展名
        /// </summary>
        public string Extension => this._fileInfo.Extension;

        /// <summary>
        /// 文件大小
        /// </summary>
        public long Size => this._fileInfo.Length;

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastModified => this._fileInfo.LastWriteTime;

        /// <summary>
        /// 文件相对路径
        /// </summary>
        public string RelativePath { get; }

        /// <summary>
        /// 完整文件物理路径(带fileName)
        /// </summary>
        public string FullLocalPath { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="relativePath"></param>
        /// <param name="fileInfo"></param>
        public DefaultStoreFile(string relativePath, FileInfo fileInfo)
        {
            this._fileInfo = fileInfo;
            this.RelativePath = Path.Combine(relativePath, fileInfo.Name);
            this.FullLocalPath = fileInfo.FullName;
        }

        /// <summary>
        /// 获取用于读取文件的Stream
        /// </summary>
        public Stream OpenReadStream()
        {
            return new FileStream(this._fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.Read);
        }
    }
}
