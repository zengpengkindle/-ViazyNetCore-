using Microsoft.Extensions.FileProviders;
using System;
using System.IO;
using System.IO.Compression;

namespace Microsoft.Extensions.FileProviders
{
    /// <summary>
    /// An <see cref="IFileInfo"/> for a zip entry.
    /// </summary>
    /// <seealso cref="Microsoft.Extensions.FileProviders.IFileInfo" />
    class ZipEntryInfo : IFileInfo
    {
        private readonly string _path;
        private readonly string _name;
        private readonly DateTimeOffset _modified;
        private readonly bool _isDir;
        private readonly long _lenth;
        private readonly ZipArchiveEntry _fileEntry;

        public ZipEntryInfo(ZipArchiveEntry entry)
        {
            this._modified = entry.LastWriteTime;
            this._isDir = string.IsNullOrEmpty(entry.Name);
            if (this._isDir)
            {
                this._path = Path.GetDirectoryName(entry.FullName).Replace('\\', '/');
                this._name = Path.GetFileName(this._path);
                this._lenth = -1;
            }
            else
            {
                this._fileEntry = entry;
                this._path = entry.FullName.Replace('\\', '/');
                this._name = entry.Name;
                this._lenth = entry.Length;
            }
        }

        public ZipEntryInfo(string path)
        {
            this._name = Path.GetFileName(this._path) ?? string.Empty;
            this._path = path.Replace('\\', '/');
        }

        public bool Exists => true;

        public long Length => this._lenth;

        public string PhysicalPath => this._path;

        public string Name => this._name;

        public DateTimeOffset LastModified => this._modified;

        public bool IsDirectory => this._isDir;

        public Stream CreateReadStream()
        {
            if (this._isDir) throw new InvalidOperationException("Cannot create a stream for a directory.");

            try
            {
                return this._fileEntry.Open().MakeSeekable();//        return new StreamWithDisposables(this._fileEntry.Open().MakeSeekable(), this._fileEntry.Archive);
            }
            catch
            {
                //this._fileEntry.Archive.Dispose();
                throw;
            }
        }

        public override string ToString() => this.PhysicalPath;
    }
}