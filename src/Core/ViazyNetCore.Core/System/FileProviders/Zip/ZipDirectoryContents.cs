using Microsoft.Extensions.FileProviders;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Extensions.FileProviders
{
    /// <summary>
    /// Provides file contents of a zip folder entry.
    /// </summary>
    /// <seealso cref="Microsoft.Extensions.FileProviders.IDirectoryContents" />
    class ZipDirectoryContents : IDirectoryContents
    {
        private readonly IEnumerable<IFileInfo> _files;

        public ZipDirectoryContents(IEnumerable<IFileInfo> files)
        {
            this._files = files;
        }

        public bool Exists => true;

        public IEnumerator<IFileInfo> GetEnumerator()
            => this._files.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => this._files.GetEnumerator();
    }
}
