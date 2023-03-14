using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Microsoft.Extensions.FileProviders
{
    /// <summary>
    /// Provides read-only file system view over a zip file's contents.
    /// </summary>
    public class ZipFileProvider : IFileProvider
    {
        private readonly ZipArchive _zipArchive;
        private readonly IList<ZipEntryInfo> _folderEntries;
        private readonly StringComparison _comparison;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZipFileProvider" /> class.
        /// </summary>
        /// <param name="zipFilePath">The zip file path.</param>
        /// <param name="caseSensitive">if set to <c>true</c> then path comparisons will be case sensitive.</param>
        public ZipFileProvider(string zipFilePath, bool caseSensitive = false) : this(File.Create(zipFilePath), caseSensitive) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZipFileProvider" /> class.
        /// </summary>
        /// <param name="zipData">The zip file's data.</param>
        /// <param name="caseSensitive">if set to <c>true</c> then path comparisons will be case sensitive.</param>
        /// <exception cref="System.ArgumentNullException">zipData</exception>
        public ZipFileProvider(byte[] zipData, bool caseSensitive = false)
            : this(new MemoryStream(zipData), caseSensitive) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZipFileProvider" /> class.
        /// </summary>
        /// <param name="stream">The zip stream.</param>
        /// <param name="caseSensitive">if set to <c>true</c> then path comparisons will be case sensitive.</param>
        /// <exception cref="System.ArgumentNullException">zipData</exception>
        public ZipFileProvider(Stream stream, bool caseSensitive = false)
        {
            if(stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            this._zipArchive = new ZipArchive(stream, ZipArchiveMode.Read);

            this._comparison = caseSensitive ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
       
            this._folderEntries = this._zipArchive.ReadFolders(this._comparison);
        }

        /// <summary>
        /// Enumerate a directory at the given path, if any.
        /// </summary>
        /// <param name="subpath">Relative path that identifies the directory.</param>
        /// <returns>
        /// Returns the contents of the directory.
        /// </returns>
        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            Debug.WriteLine($"GetDirectoryContents({subpath})");

            if(string.IsNullOrEmpty(subpath)) { return NotFoundDirectoryContents.Singleton; }

            var isRoot = string.Equals(subpath, "/", StringComparison.Ordinal);

            subpath = subpath.Trim('/');
            var folder = this._folderEntries
                .FirstOrDefault(entry => string.Equals(entry.PhysicalPath, subpath, this._comparison));
            if(folder is null && !isRoot)
            {
                return NotFoundDirectoryContents.Singleton;
            }


            var all = this._zipArchive.ReadFiles()
                            .Union(this._folderEntries);
            var matchItems = all.Where(entry => string.Equals(Path.GetDirectoryName(entry.PhysicalPath).Replace('\\', '/'), subpath, this._comparison))
                            .ToList();
            return new ZipDirectoryContents(matchItems);

        }

        /// <summary>
        /// Locate a file at the given path.
        /// </summary>
        /// <param name="subpath">Relative path that identifies the file.</param>
        /// <returns>
        /// The file information. Caller must check Exists property.
        /// </returns>
        /// <exception cref="NotFoundFileInfo"></exception>
        public IFileInfo GetFileInfo(string subpath)
        {
            Debug.WriteLine($"GetFileInfo({subpath})");

            var isRoot = string.Equals(subpath, "/", StringComparison.Ordinal);

            if(string.IsNullOrEmpty(subpath) || isRoot)
            {
                return new NotFoundFileInfo(subpath);
            }

            subpath = subpath.Trim('/');

            IFileInfo file = this._zipArchive.ReadFiles()
                        .FirstOrDefault(entry => string.Equals(entry.PhysicalPath, subpath, this._comparison));
            return file ?? new NotFoundFileInfo(subpath);
        }

        /// <summary>
        /// Always returns a <see cref="NullChangeToken"/>.
        /// </summary>
        /// <param name="filter">Not used since zip file is read-only.</param>
        /// <returns>
        /// A <see cref="NullChangeToken"/>.
        /// </returns>
        public IChangeToken Watch(string filter) => NullChangeToken.Singleton;
    }
}
