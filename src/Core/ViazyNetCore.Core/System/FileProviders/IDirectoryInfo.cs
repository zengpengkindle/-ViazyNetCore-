using System;
using System.IO;
using System.Text;
using Microsoft.Extensions.Primitives;

namespace Microsoft.Extensions.FileProviders
{
    /// <summary>
    /// 定义一个目录提供程序。
    /// </summary>
    public interface IDirectoryInfo : IFileProvider
    {
        /// <summary>
        /// 获取目录的物理路径。
        /// </summary>
        string PhysicalPath { get; }
        /// <summary>
        /// 获取一个值，指示目录是否存在。
        /// </summary>
        bool Exists { get; }
        /// <summary>
        /// 获取目录名称。
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 创建或获取一个文件。
        /// </summary>
        /// <param name="subpath">子路径。</param>
        /// <returns>文件信息。</returns>
        IFileInfo GetOrCreateFileInfo(string subpath);

        /// <summary>
        /// 打开写入流。
        /// </summary>
        /// <param name="subpath">文件路径。</param>
        /// <param name="encoding">文件编码，默认为 <see cref="Encoding.UTF8"/>。</param>
        /// <returns>写入流。</returns>
        StreamWriter OpenWriteStream(string subpath, Encoding? encoding = null);
    }

    class PhysicalDirectoryInfo : IDirectoryInfo
    {
        private readonly IFileProvider? _current;

        public bool Exists { get; }
        public string PhysicalPath { get; }
        public string Name { get; }
        public DirectoryInfo Directory { get; }

        public PhysicalDirectoryInfo(string physicalPath, bool createWithNotExists) : this(new DirectoryInfo(physicalPath), createWithNotExists) { }

        public PhysicalDirectoryInfo(DirectoryInfo directory, bool createWithNotExists)
        {
            //Physical.PhysicalFileInfo
            this.Exists = directory.Exists;
            if(createWithNotExists && !this.Exists)
            {
                directory.Create();
                this.Exists = true;
            }

            if(this.Exists) this._current = new PhysicalFileProvider(directory.FullName);
            this.Name = directory.Name;
            this.PhysicalPath = directory.FullName;
            this.Directory = directory;
        }

        private IFileProvider GetProvider()
        {
            if(this._current is null) throw new DirectoryNotFoundException();
            return this._current;
        }

        public IDirectoryContents GetDirectoryContents(string subpath) => this.GetProvider().GetDirectoryContents(subpath);

        public IFileInfo GetFileInfo(string subpath) => this.GetProvider().GetFileInfo(subpath);

        public IChangeToken Watch(string filter) => this.GetProvider().Watch(filter);

        public IFileInfo GetOrCreateFileInfo(string subpath)
        {
            if(!this.Exists) return new NotFoundFileInfo(subpath); 

            var fileInfo = this.GetFileInfo(subpath);
            if(fileInfo.Exists) return fileInfo;

            File.Create(fileInfo.PhysicalPath).Dispose();
            return this.GetFileInfo(subpath);

        }

        public StreamWriter OpenWriteStream(string subpath, Encoding? encoding = null)
        {
            if(!this.Exists) throw new FileNotFoundException($"The directory {this.PhysicalPath} is not exists.", this.PhysicalPath);

            var fileInfo = this.GetOrCreateFileInfo(subpath);
            return new StreamWriter(new FileStream(fileInfo.PhysicalPath, FileMode.Create, FileAccess.Write, FileShare.Read), encoding ?? Encoding.UTF8);
        }
    }
}
