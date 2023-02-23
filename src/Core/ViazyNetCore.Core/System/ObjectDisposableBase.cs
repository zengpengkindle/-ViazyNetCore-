using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public abstract class ObjectDisposableBase : IDisposable
    {
        private bool _disposed = false;
        public void Dispose()
        {
            this._disposed = true;
        }

        protected void ThrowIfDisposed()
        {
            if (this._disposed)
                throw new ObjectDisposedException(this.GetType().Name);
        }

        protected abstract void DisposeManaged();
    }
}
