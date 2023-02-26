using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace System.Providers
{
    /// <summary>
    /// 定义一个锁的提供程序。
    /// </summary>
    public interface ILockProvider
    {
        /// <summary>
        /// 获取或设置一个值，表示超时时间。
        /// </summary>
        /// <value>默认 15 秒。</value>
        TimeSpan Timeout { get; set; }

        /// <summary>
        /// 锁定指定种子。
        /// </summary>
        /// <param name="key">锁的键名，锁定的键名不区分大小写。</param>
        /// <param name="timeout">锁的超时时间。</param>
        /// <returns>可解锁的对象。</returns>
        IDisposable Lock(string key, TimeSpan? timeout = null);

        /// <summary>
        /// 异步锁定指定种子。
        /// </summary>
        /// <param name="key">锁的键名，锁定的键名不区分大小写。</param>
        /// <param name="timeout">锁的超时时间。</param>
        /// <returns>可解锁的对象。</returns>
        ValueTask<IDisposable> LockAsync(string key, TimeSpan? timeout = null);
    }

    /// <summary>
    /// 表示一个锁的提供程序。
    /// </summary>
    public sealed class LockProvider : ILockProvider
    {
        /// <summary>
        /// 获取内存锁。
        /// </summary>
        public static readonly ILockProvider Memory = new LockProvider();

        private readonly static ConditionalWeakTable<string, Seed> LockeableObjects = new();
        private static ILockProvider? InnerDefault;

        /// <summary>
        /// 获取或设置一个值，表示默认的锁提供程序。
        /// </summary>
        public static ILockProvider Default
        {
            get => InnerDefault ??= Memory;
            set => InnerDefault = value;
        }

        /// <summary>
        /// 锁定指定种子。
        /// </summary>
        /// <param name="key">锁的键名，锁定的键名不区分大小写。</param>
        /// <param name="timeout">锁的超时时间。</param>
        /// <returns>可解锁的对象。</returns>
        public static IDisposable Lock(string key, TimeSpan? timeout = null) => Default.Lock(key, timeout);

        /// <summary>
        /// 异步锁定指定种子。
        /// </summary>
        /// <param name="key">锁的键名，锁定的键名不区分大小写。</param>
        /// <param name="timeout">锁的超时时间。</param>
        /// <returns>可解锁的对象。</returns>
        public static ValueTask<IDisposable> LockAsync(string key, TimeSpan? timeout = null) => Default.LockAsync(key, timeout);

        /// <inheritdoc />
        public TimeSpan Timeout { get; set; }

        private LockProvider()
        {
            this.Timeout = TimeSpan.FromSeconds(15);
        }

        private static Seed GetSeed(string key)
        {
            key = string.Intern(key);
            return LockeableObjects.GetValue(key, key => new Seed(key));
        }

        IDisposable ILockProvider.Lock(string key, TimeSpan? timeout)
        {
            return GetSeed(key).Lock(timeout ?? this.Timeout);
        }

        ValueTask<IDisposable> ILockProvider.LockAsync(string key, TimeSpan? timeout)
        {
            return GetSeed(key).LockAsync(timeout ?? this.Timeout);
        }

        #region Inner

        static Exception TimeoutError(string key, TimeSpan timeout)
        {
            return new TimeoutException($"The key '{key}' lock '{timeout}' timeout.");
        }

        struct LockDisposable : IDisposable
        {
            private readonly Action _disposing;
            private int _isDisposed;
            public LockDisposable(Action disposing)
            {
                this._isDisposed = 0;
                this._disposing = disposing;
            }
            void IDisposable.Dispose()
            {
                if (Interlocked.Exchange(ref this._isDisposed, 1) == 0)
                {
                    this._disposing();
                }
            }
        }

        class Seed
        {
            private readonly string _key;
            private readonly SemaphoreSlim _sema;
            private int _counter;

            public Seed(string key)
            {
                this._key = key;
                this._sema = new SemaphoreSlim(1, 1);
            }

            IDisposable GetDisposable()
            {
                return new LockDisposable(() =>
                {
                    Interlocked.Decrement(ref this._counter);
                    this._sema.Release();
                });
            }

            public IDisposable Lock(TimeSpan timeout)
            {
                Interlocked.Increment(ref this._counter);
                if (!this._sema.Wait(timeout)) throw TimeoutError(this._key, timeout);
                return this.GetDisposable();
            }

            public async ValueTask<IDisposable> LockAsync(TimeSpan timeout)
            {
                Interlocked.Increment(ref this._counter);
                if (!await this._sema.WaitAsync(timeout)) throw TimeoutError(this._key, timeout);
                return this.GetDisposable();
            }
        }

        #endregion
    }
}
