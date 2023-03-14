using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.FileProviders
{
    /// <summary>
    /// Compares two <see cref="ZipEntryInfo"/> using <see cref="ZipEntryInfo.PhysicalPath"/>.
    /// </summary>
    /// <seealso cref="IEqualityComparer{T}" />
    class PhyPathEqualityComparer : IEqualityComparer<ZipEntryInfo>
    {
        private readonly StringComparison _comparison;


        /// <summary>
        /// Initializes a new instance of the <see cref="PhyPathEqualityComparer"/> class.
        /// </summary>
        /// <param name="comparison">The comparison rule.</param>
        public PhyPathEqualityComparer(StringComparison comparison)
        {
            this._comparison = comparison;
        }

        public bool Equals(ZipEntryInfo x, ZipEntryInfo y)
        {
            return string.Equals(x.PhysicalPath, y.PhysicalPath, this._comparison);
        }


        public int GetHashCode(ZipEntryInfo obj)
        {
            if(this._comparison == StringComparison.Ordinal)
                return obj.PhysicalPath.GetHashCode();

            return obj.PhysicalPath.ToUpperInvariant().GetHashCode();
        }
    }
}