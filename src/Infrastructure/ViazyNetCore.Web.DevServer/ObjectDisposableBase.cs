using System.Threading;

namespace System
{
    /// <summary>
    /// 表示一个释放分配的资源的基类。
    /// </summary>
    public abstract class ObjectDisposableBase : IDisposable
    {
        private const int TRUE = 1;
        private int _isDisposed;

        /// <inheritdoc />
        public virtual bool IsDisposed => this._isDisposed == TRUE;

        /// <summary>
        /// 初始化一个 <see cref="ObjectDisposableBase"/> 类的新实例。
        /// </summary>
        protected ObjectDisposableBase()
        {
        }

        /// <summary>
        /// 执行与释放或重置托管资源相关的应用程序定义的任务。
        /// </summary>
        protected virtual void DisposeManaged()
        {
        }

        /// <summary>
        /// 执行与释放或重置非托管资源相关的应用程序定义的任务。
        /// </summary>
        protected virtual void DisposeUnmanaged()
        {
        }

        /// <summary>
        /// 执行与释放或重置非托管资源相关的应用程序定义的任务。
        /// </summary>
        /// <param name="disposing">为 <see langword="true"/> 则释放托管资源和非托管资源；为 <see langword="false"/> 则仅释放非托管资源。</param>
        protected virtual void Dispose(bool disposing)
        {
            if(this.IsDisposed || Interlocked.Exchange(ref this._isDisposed, TRUE) == TRUE) return;

            if(disposing)
            {
                this.DisposeManaged();
            }
            this.DisposeUnmanaged();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 终结器。
        /// </summary>
        ~ObjectDisposableBase()
        {
            this.Dispose(false);
        }
    }
}
